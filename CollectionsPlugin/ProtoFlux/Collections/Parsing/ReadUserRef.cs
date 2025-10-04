using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Parsing;

[NodeCategory("Collections/Parsing")]
[NodeName("Read UserRef")]
[ContinuouslyChanging]
public class ReadUserRef : VoidNode<FrooxEngineContext>
{
    public readonly ObjectInput<UserRef> UserRef;
    public readonly ObjectOutput<User> User;
    public readonly ObjectOutput<string> MachineId;
    public readonly ObjectOutput<string> UserId;

    protected override void ComputeOutputs(FrooxEngineContext context)
    {
        UserRef userRef = UserRef.Evaluate(context);
        if (userRef == null || userRef.Target == null)
        {
            User.Write(null,context);
            MachineId.Write(null,context);
            UserId.Write(null,context);
            return;
        }
        User.Write(userRef.Target,context);
        MachineId.Write(userRef.LinkedMachineId,context);
        UserId.Write(userRef.LinkedCloudId,context);
    }

    public ReadUserRef()
    {
        User = new ObjectOutput<User>(this);
        MachineId = new ObjectOutput<string>(this);
        UserId = new ObjectOutput<string>(this);
    }
}