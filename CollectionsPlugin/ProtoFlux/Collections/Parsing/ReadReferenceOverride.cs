using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Parsing;

[NodeCategory("Collections/Parsing")]
[NodeName("Read Override")]
[ContinuouslyChanging]
public class ReadReferenceOverride<T> : VoidNode<FrooxEngineContext>
    where T : class, IWorldElement
{
    public readonly ObjectInput<ReferenceUserOverride<T>.Override> Override;
    public readonly ObjectOutput<User> User;
    public readonly ObjectOutput<string> MachineId;
    public readonly ObjectOutput<string> UserId;
    public readonly ObjectOutput<T> Value;

    protected override void ComputeOutputs(FrooxEngineContext context)
    {
        ReferenceUserOverride<T>.Override o = Override.Evaluate(context);
        if (o == null)
        {
            User.Write(default, context);
            MachineId.Write(null,context);
            UserId.Write(null,context);
            Value.Write(default,context);
        }
        User.Write(o.User.Target,context);
        MachineId.Write(o.User.LinkedMachineId,context);
        UserId.Write(o.User.LinkedCloudId,context);
        Value.Write(o.Value.Target,context);
    }

    public ReadReferenceOverride()
    {
        User = new ObjectOutput<User>(this);
        MachineId = new ObjectOutput<string>(this);
        UserId = new ObjectOutput<string>(this);
        Value = new ObjectOutput<T>(this);
    }
}