using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Parsing.Write;

[NodeCategory("Collections/Parsing")]
[NodeName("Write UserRef")]
[NodeOverload("Collections.Parsing.Write")]
public class WriteUserRef : ActionNode<FrooxEngineContext>
{
    public readonly ObjectInput<UserRef> UserRef;
    public readonly ObjectInput<string> MachineId;
    public readonly ObjectInput<string> UserId;

    public Continuation OnSuccess;
    public Continuation OnFailed;
    
    protected override IOperation Run(FrooxEngineContext context)
    {
        UserRef userRef = UserRef.Evaluate(context);
        if (userRef == null)
        {
            return OnFailed.Target;
        }

        string id = MachineId.Evaluate(context) ?? UserId.Evaluate(context);
        userRef.SetFromIdAuto(id);
        return OnSuccess.Target;
    }
}