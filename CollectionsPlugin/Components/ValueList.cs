using FrooxEngine;

namespace CollectionsPlugin.Components;

[Category("Data")]
public class ValueList<T> : Component
    where T: unmanaged
{
    public readonly SyncFieldList<T> Values;
}