# Property Accessor Source Generator

`PropertyAccessorSourceGenerator` is a Roslyn incremental source generator that creates a string-based property accessor for a class or struct.
Mark an accessor type with `PropertyAccessorAttribute<T>`, where `T` is the type whose public properties should be exposed, 
and the generator adds a `TryGetPropertyValue`, and `GetPropertyValues` extension methods at compile time.

## Example

The sample application defines a model and an accessor declaration:

```csharp
using Annotations;

[PropertyAccessor<Person>]
public static partial class PersonPropertyAccessor
{
}

public class Person
{
	public required string Name { get; set; }
	public int Age { get; set; }
}
```

The generated accessor can be used as follows:

```csharp
var person = new Person { Name = "Alice", Age = 30 };

if (person.TryGetPropertyValue("Name", out var value))
{
	Console.WriteLine(value);
}
```

The generated methods have the following shape:

```csharp
public IReadOnlyDictionary<string, object?> GetPropertyValues(string[] names)
public bool TryGetPropertyValue(string name, out object? value)
```

### GetPropertyValues

`GetPropertyValues` returns a read-only dictionary mapping each found property name to its value.
Names not present on the item are omitted; returns an empty dictionary if no names are provided 
or none are found.

### TryGetPropertyValue

`TryGetPropertyValue` returns `true` and assigns the matching property value when `name` matches a public property name.
For an unknown property name, it assigns `default` to `value` and returns `false`.

## How it works

1. `PropertyAccessorAttribute<T>` identifies the target type `T`.
2. The generator finds annotated classes and structs during compilation.
3. It reads the target type's public properties.
4. It emits a generated partial type containing the extension methods.

Generated source files are named `{AccessorTypeName}.g.cs` and include nullable annotations.
The generated accessor preserves the annotated type's namespace, accessibility, and class or struct kind.

## Current behavior and limitations

- Only public properties are included.
- Property lookup is case-sensitive and uses the property name exactly as declared.
- The return type is `object?`, so callers may need to cast or pattern-match the result.
- Unknown property names return `false`.
- The attribute can be applied once to a class or struct and is not inherited.
- The target type must be represented by a class or struct declaration annotated with `PropertyAccessorAttribute<T>`.

## Project structure

| Project | Description |
| --- | --- |
| `src/PropertyAccessorGenerator` | The incremental Roslyn source generator, targeting .NET Standard 2.0. |
| `src/Annotations` | Defines `PropertyAccessorAttribute<T>`, targeting .NET Standard 2.0. |
| `samples/GettingStartedExample` | A .NET 10 console sample that demonstrates the generator. |

## Requirements

- .NET 10 SDK to build and run the sample application.
- A compiler/toolchain that supports Roslyn source generators.
- C# 11 or later for the project source.

## Build and run

From the repository root:

```bash
dotnet build StringFormatWith.slnx
dotnet run --project src/StringFormatWith/StringFormatWith.csproj
```

The sample prints:

```text
Name: Alice
Age: 30
```

The sample project enables generated-file output. Generated files can be inspected under its intermediate `Generated` directory after a build.

## Using the generator in another project

Reference the generator project as an analyzer and reference the annotations project for the attribute:

```xml
<ItemGroup>
  <ProjectReference Include="path/to/Annotations/Annotations.csproj" />
  <ProjectReference
	Include="path/to/PropertyAccessorGenerator/PropertyAccessorGenerator.csproj"
	OutputItemType="Analyzer"
	ReferenceOutputAssembly="false" />
</ItemGroup>
```

Then declare a partial accessor type:

```csharp
using Annotations;

[PropertyAccessor<YourType>]
public static partial class YourTypePropertyAccessor
{
}
```

The generator package is configured as an analyzer package and targets `netstandard2.0` for broad compiler compatibility.

## License

This project is licensed under the MIT License.
