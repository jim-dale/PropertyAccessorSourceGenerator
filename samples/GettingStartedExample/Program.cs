using Annotations;

var alice = new Person("Alice", 30);

Console.WriteLine();
Console.WriteLine("Using TryGetPropertyValue:");

if (alice.TryGetPropertyValue("Name", out var name))
{
    Console.WriteLine($"\tName={name}");
}

if (alice.TryGetPropertyValue("Age", out var age))
{
    Console.WriteLine($"\tAge={age}");
}

bool success = alice.TryGetPropertyValue("Unknown", out _);
if (!success)
{
    Console.WriteLine("\tFailed to get the value of \"Unknown\" property");
}

Console.WriteLine();
Console.WriteLine("Using GetPropertyValues:");

var properties = alice.GetPropertyValues(["Name", "Age", "Unknown"]);
foreach (var property in properties)
{
    Console.WriteLine($"\t{property.Key}={property.Value}");
}

internal readonly struct Person
{
    public Person(string name, int age)
    {
        this.Name = name;
        this.Age = age;
    }

    public string Name { get; }

    public int Age { get; }
}

[PropertyAccessor<Person>]
internal static partial class PersonPropertyAccessor
{
}
