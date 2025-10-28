using FrooxEngine;

namespace CollectionsPlugin.Components;

[Category("Data")]
[GenericTypes(GenericTypesAttribute.Group.EnginePrimitives)]
public class ValueList<T> : Component
    where T: unmanaged
{
    public readonly SyncFieldList<T> Values;
}