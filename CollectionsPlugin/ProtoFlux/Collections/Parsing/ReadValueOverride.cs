using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Parsing;

[NodeCategory("Collections/Parsing")]
[NodeName("Read Override")]
[ContinuouslyChanging]
public class ReadValueOverride<T> : VoidNode<FrooxEngineContext>
    where T : unmanaged
{
    public readonly ObjectInput<ValueOverrideBase<T>.Override> Override;
    public readonly ObjectOutput<User> User;
    public readonly ObjectOutput<string> MachineId;
    public readonly ObjectOutput<string> UserId;
    public readonly ValueOutput<T> Value;

    protected override void ComputeOutputs(FrooxEngineContext context)
    {
        ValueOverrideBase<T>.Override o = Override.Evaluate(context);
        if (o == null)
        {
            User.Write(default, context);
            MachineId.Write(default,context);
            UserId.Write(default,context);
            Value.Write(default,context);
            return;
        }
        User.Write(o.User.Target,context);
        MachineId.Write(o.User.LinkedMachineId,context);
        UserId.Write(o.User.LinkedCloudId,context);
        Value.Write(o.Value.Value,context);
    }

    public ReadValueOverride()
    {
        User = new ObjectOutput<User>(this);
        MachineId = new ObjectOutput<string>(this);
        UserId = new ObjectOutput<string>(this);
        Value = new ValueOutput<T>(this);
    }
}