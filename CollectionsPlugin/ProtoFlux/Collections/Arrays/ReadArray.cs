using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Arrays;

[NodeCategory("Collections.Arrays")]
[NodeName("Read Array")]
public class ReadArray<T> : VoidNode<FrooxEngineContext>
    where T: IEquatable<T>
{
    public readonly ObjectInput<SyncArray<T>> Array;
    public readonly ObjectInput<int> Index;
    public readonly ObjectOutput<T> Value;
    public readonly ObjectOutput<bool> HasValue;

    protected override void ComputeOutputs(FrooxEngineContext context)
    {
        SyncArray<T> array = Array.Evaluate(context);
        int i = Index.Evaluate(context);
        Value.Write(array[i],context);
    }
}