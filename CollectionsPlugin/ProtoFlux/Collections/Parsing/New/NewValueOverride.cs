using Elements.Core;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Parsing.New;

[NodeCategory("Collections/Parsing")]
[NodeName("New Override")]
[NodeOverload("Collections.Parsing.New")]
public class NewValueOverride<T> : ActionNode<FrooxEngineContext>
    where T: unmanaged
{
    public readonly ObjectInput<UserRef> UserRef;
    public readonly ValueInput<T> Value;
    public readonly ObjectOutput<ValueOverrideBase<T>.Override> Override;

    public Continuation Next;
    
    protected override IOperation Run(FrooxEngineContext context)
    {
        ValueOverrideBase<T>.Override newOverride = new ValueOverrideBase<T>.Override();
        UserRef user = UserRef.Evaluate(context,new UserRef());
        if (user != null)
        {
            try
            {
                string id = user.LinkedCloudId ?? user.LinkedMachineId;
                newOverride.User.SetFromIdAuto(id);
            }
            catch
            {
                UniLog.Warning("New Override created with no CloudId or MachineId");
            }
            
        }
        newOverride.Value.Value = Value.Evaluate(context);
        Override.Write(newOverride,context);
        return Next.Target;
    }
    
    public NewValueOverride()
    {
        Override = new ObjectOutput<ValueOverrideBase<T>.Override>(this);
    }
}