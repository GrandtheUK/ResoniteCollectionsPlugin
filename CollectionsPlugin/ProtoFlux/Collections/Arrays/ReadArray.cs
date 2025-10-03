using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Arrays;

[NodeCategory("Collections/Arrays")]
[NodeName("Read Array")]
[ContinuouslyChanging]
public class ReadArray<T> : VoidNode<FrooxEngineContext>
    where T: unmanaged, IEquatable<T>
{
    public readonly ObjectInput<SyncArray<T>> Array;
    public readonly ValueInput<int> Index;
    public readonly ValueOutput<T> Value;
    public readonly ValueOutput<bool> HasValue;

    protected override void ComputeOutputs(FrooxEngineContext context)
    {
        SyncArray<T> array = Array.Evaluate(context);
        int i = Index.Evaluate(context);
        if (array == null || i >= array.Count || i < 0)
        {
            Value.Write(default,context);
            HasValue.Write(false,context);
            return;
        }
        Value.Write(array[i],context);
        HasValue.Write(true,context);
    }

    public ReadArray()
    {
        Value = new ValueOutput<T>(this);
        HasValue = new ValueOutput<bool>(this);
    }
}