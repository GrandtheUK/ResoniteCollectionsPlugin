using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Parsing.New;

[NodeCategory("Collections/Parsing")]
[NodeName("New UserRef")]
[NodeOverload("Collections.Parsing.New")]
public class NewUserRef : ActionNode<FrooxEngineContext>
{
    public readonly ObjectInput<string> UserId;
    public readonly ObjectInput<string> MachineId;

    public readonly ObjectOutput<UserRef> UserRef;

    public Continuation Next;
    protected override IOperation Run(FrooxEngineContext context)
    {
        string userId = UserId.Evaluate(context);
        string machineId = MachineId.Evaluate(context);
        UserRef userRef = new UserRef();
        string id = userId ?? machineId;
        userRef.SetFromIdAuto(id);
        UserRef.Write(userRef,context);
        return Next.Target;
    }
}