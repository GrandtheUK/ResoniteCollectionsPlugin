using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Arrays;

[NodeCategory("Collections/Arrays")]
[NodeName("Remove At Array")]
public class RemoveAtArray<T> : ActionNode<FrooxEngineContext>
    where T: IEquatable<T>
{
    public readonly ObjectInput<SyncArray<T>> Array;
    public readonly ValueInput<int> Index;
    public Continuation OnFailure;
    public Continuation OnSuccess;
    protected override IOperation Run(FrooxEngineContext context)
    {
        SyncArray<T> array = Array.Evaluate(context);
        int i = Index.Evaluate(context);
        if (array == null || i >= array.Count || i < 0)
        {
            return OnFailure.Target;
        }
        array.RemoveAt(i);
        return OnSuccess.Target;
    }
}