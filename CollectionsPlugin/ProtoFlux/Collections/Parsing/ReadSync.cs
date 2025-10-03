using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Parsing;

[NodeCategory("Collections/Parsing")]
[NodeName("Read Sync")]
[ContinuouslyChanging]
public class ReadSync<T> : ValueFunctionNode<FrooxEngineContext,T>
    where T: unmanaged
{
    public readonly ObjectInput<Sync<T>> Reference;
    protected override T Compute(FrooxEngineContext context)
    {
        Sync<T> reference = Reference.Evaluate(context);
        return reference == null ? default : reference.Value;
    }
}