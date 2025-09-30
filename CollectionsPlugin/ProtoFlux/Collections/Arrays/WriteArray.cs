using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Arrays;

[NodeCategory("Collections/Arrays")]
[NodeName("Write Array")]
public class WriteArray<T> : ActionNode<FrooxEngineContext>
    where T: unmanaged,IEquatable<T>
{
    public readonly ObjectInput<SyncArray<T>> Array;
    public readonly ValueInput<int> Index;
    public readonly ValueInput<T> Value;
    public Continuation OnFailure;
    public Continuation OnSuccess;
    public Continuation OnNotFound;

    protected override IOperation Run(FrooxEngineContext context)
    {
        SyncArray<T> array = Array.Evaluate(context);
        int i = Index.Evaluate(context);
        
        if (array == null || i<0)
        {
            return OnNotFound.Target;
        }

        if (i >= array.Count)
        {
            return OnFailure.Target;
        }

        array[i] = Value.Evaluate(context);
        return OnSuccess.Target;
    }
}