namespace Annotations;

using System;

/// <summary>
/// Identifies a type that supplies property accessors for the attributed class or struct.
/// </summary>
/// <remarks>Applies to classes and structs. Only a single instance is allowed and the attribute is not inherited.</remarks>
/// <typeparam name="T">The type that provides the property accessor implementations associated with the attributed type.</typeparam>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
public sealed class PropertyAccessorAttribute<T> : Attribute
{
}
