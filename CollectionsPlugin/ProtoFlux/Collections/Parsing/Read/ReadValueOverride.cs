using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Parsing.Read;

[NodeCategory("Collections/Parsing")]
[NodeName("Read Override")]
[NodeOverload("Collections.Parsing.Read")]
[ContinuouslyChanging]
public class ReadValueOverride<T> : VoidNode<FrooxEngineContext>
    where T : unmanaged
{
    public readonly ObjectInput<ValueOverrideBase<T>.Override> Override;
    public readonly ObjectOutput<UserRef> UserRef;
    public readonly ValueOutput<T> Value;

    protected override void ComputeOutputs(FrooxEngineContext context)
    {
        ValueOverrideBase<T>.Override o = Override.Evaluate(context);
        if (o == null)
        {
            UserRef.Write(default, context);

            Value.Write(default,context);
            return;
        }
        UserRef.Write(o.User,context);
        Value.Write(o.Value.Value,context);
    }

    public ReadValueOverride()
    {
        UserRef = new ObjectOutput<UserRef>(this);
        Value = new ValueOutput<T>(this);
    }
}