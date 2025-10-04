using FrooxEngine;

namespace CollectionsPlugin.Components.Permissions;

[Category("Permissions")]
public class ComponentReadBlocker : Component
{
    public readonly Sync<bool> Active;
}