using FrooxEngine;

namespace CollectionsPlugin.Components;

[Category("Data")]
[GenericTypes(GenericTypesAttribute.Group.EnginePrimitives)]
public class ValueList<T> : Component
    where T: IEquatable<T?>
{
    public readonly SyncFieldList<T> Values;
}