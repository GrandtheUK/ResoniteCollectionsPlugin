using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Parsing.Read;

[NodeCategory("Collections/Parsing")]
[NodeName("Read SyncRef")]
[NodeOverload("Collections.Parsing.Read")]
[ContinuouslyChanging]
public class ReadSyncRef<T> : ObjectFunctionNode<FrooxEngineContext,T>
    where T: class, IWorldElement, new()
{
    public readonly ObjectInput<SyncRef<T>> Reference;
    protected override T Compute(FrooxEngineContext context)
    {
        SyncRef<T> reference = Reference.Evaluate(context);
        return reference == null ? default : reference.Target;
    }
}