using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Parsing;

[NodeCategory("Collections/Parsing")]
[NodeName("Read Override")]
public class ReadObjectValueOverride<T> : VoidNode<FrooxEngineContext>
{
    public readonly ObjectInput<ValueOverrideBase<T>.Override> Override;
    public readonly ObjectOutput<User> User;
    public readonly ObjectOutput<T> Value;

    protected override void ComputeOutputs(FrooxEngineContext context)
    {
        ValueOverrideBase<T>.Override o = Override.Evaluate(context);
        if (o == null)
        {
            User.Write(null, context);
            Value.Write(default,context);
        }
        User.Write(o.User.Target,context);
        Value.Write(o.Value.Value,context);
    }

    public ReadObjectValueOverride()
    {
        User = new ObjectOutput<User>(this);
        Value = new ObjectOutput<T>(this);
    }
}