using FrooxEngine;

namespace CollectionsPlugin.Components.Permissions;

[Category("Permissions")]
public class ComponentReadPermit : Component
{
    public readonly Sync<bool> Active;
    public readonly Sync<bool> UserAccessControl;
    public readonly SyncBag<UserRef> _permittedUsers;
}