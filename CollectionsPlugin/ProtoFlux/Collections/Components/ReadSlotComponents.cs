using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

using CollectionsPlugin.Components.Permissions;

namespace CollectionsPlugin.ProtoFlux.Collections.Components;

[NodeCategory("Collections/Components")]
[NodeName("Read Component")]
[ContinuouslyChanging]
public class ReadSlotComponents : VoidNode<FrooxEngineContext>
{
    public readonly ObjectInput<Slot> Slot;
    public readonly ValueInput<int> Index;
    public readonly ObjectOutput<Component> Component;
    public readonly ValueOutput<bool> ReadAllowed;
    public readonly ValueOutput<bool> ComponentFound;

    protected override void ComputeOutputs(FrooxEngineContext context)
    {
        int i = Index.Evaluate(context);
        Slot s = Slot.Evaluate(context);
        if (s == null || i>=s.Components.Count() || i<0 || !s.Components.Any())
        {
            Component.Write(null,context);
            ReadAllowed.Write(false,context);
            ComponentFound.Write(false,context);
            return;
        }

        Slot rs = s;
        bool found = false;
        Component FoundComponent;
        while (rs != context.World.RootSlot || !found)
        {
            rs = rs.Parent;
            ComponentReadBlocker? c = (ComponentReadBlocker) s.Components.FirstOrDefault(c => c.GetType() == typeof(ComponentReadBlocker)) ?? (ComponentReadBlocker) rs.Components.FirstOrDefault(c => c.GetType() == typeof(ComponentReadBlocker));
            if (c != null && c.Active.Value)
            {
                ComponentReadPermit permit = (ComponentReadPermit) s.Components.FirstOrDefault(c => c.GetType() == typeof(ComponentReadPermit));
                if (permit != null && permit.Active.Value)
                {
                    if (permit.UserAccessControl.Value && permit._permittedUsers.FirstOrDefault(e => e.Value.User.Target == context.LocalUser).Value==null)
                    {
                        Component.Write(null,context);
                        ReadAllowed.Write(false,context);
                        ComponentFound.Write(false,context);
                        return;
                    }
                    FoundComponent = s.Components.ElementAt(i);
                        
                    Component.Write(FoundComponent,context);
                    ReadAllowed.Write(true,context);
                    ComponentFound.Write(FoundComponent!=null,context);
                    return;

                }
                Component.Write(null,context);
                ReadAllowed.Write(false,context);
                ComponentFound.Write(false,context);
                return;
            }
            FoundComponent = s.Components.ElementAt(i);
                    
            Component.Write(FoundComponent,context);
            ReadAllowed.Write(true,context);
            ComponentFound.Write(FoundComponent!=null,context);
            found = true;
        }
        FoundComponent = s.Components.ElementAt(i);
                    
        Component.Write(FoundComponent,context);
        ReadAllowed.Write(true,context);
        ComponentFound.Write(FoundComponent!=null,context);
    }

    public ReadSlotComponents()
    {
        Component = new ObjectOutput<Component>(this);
        ReadAllowed = new ValueOutput<bool>(this);
        ComponentFound = new ValueOutput<bool>(this);
    }
}