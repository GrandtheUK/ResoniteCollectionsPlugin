using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Parsing.Read;

[NodeCategory("Collections/Parsing")]
[NodeName("Read Override")]
[NodeOverload("Collections.Parsing.Read")]
[ContinuouslyChanging]
public class ReadReferenceOverride<T> : VoidNode<FrooxEngineContext>
    where T : class, IWorldElement
{
    public readonly ObjectInput<ReferenceUserOverride<T>.Override> Override;
    public readonly ObjectOutput<UserRef> UserRef;
    public readonly ObjectOutput<T> Value;

    protected override void ComputeOutputs(FrooxEngineContext context)
    {
        ReferenceUserOverride<T>.Override o = Override.Evaluate(context);
        if (o == null)
        {
            UserRef.Write(default, context);
            Value.Write(default,context);
            return;
        }
        UserRef.Write(o.User,context);
        Value.Write(o.Value.Target,context);
    }

    public ReadReferenceOverride()
    {
        UserRef = new ObjectOutput<UserRef>(this);
        Value = new ObjectOutput<T>(this);
    }
}