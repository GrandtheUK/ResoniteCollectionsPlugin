using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Arrays;

[NodeCategory("Collections/Arrays")]
[NodeName("Array Length")]
public class ArrayLength<T> : ValueFunctionNode<FrooxEngineContext,int>
    where T: IEquatable<T>
{
    public readonly ObjectInput<SyncArray<T>> Array;

    protected override int Compute(FrooxEngineContext context)
    {
        SyncArray<T> array = Array.Evaluate(context);
        return array.Count;
    }
}