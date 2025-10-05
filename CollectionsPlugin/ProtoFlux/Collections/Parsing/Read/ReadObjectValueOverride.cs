using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Parsing.Read;

[NodeCategory("Collections/Parsing")]
[NodeName("Read Override")]
[NodeOverload("Collections.Parsing.Read")]
[ContinuouslyChanging]
public class ReadObjectValueOverride<T> : VoidNode<FrooxEngineContext>
{
    public readonly ObjectInput<ValueOverrideBase<T>.Override> Override;
    public readonly ObjectOutput<UserRef> UserRef;
    public readonly ObjectOutput<T> Value;

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

    public ReadObjectValueOverride()
    {
        UserRef = new ObjectOutput<UserRef>(this);
        Value = new ObjectOutput<T>(this);
    }
}