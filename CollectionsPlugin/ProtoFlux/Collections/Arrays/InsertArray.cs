using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Arrays;

[NodeCategory("Collections/Arrays")]
[NodeName("Insert Array")]
public class InsertArray<T> : ActionNode<FrooxEngineContext>
    where T: unmanaged, IEquatable<T>
{
    public readonly ObjectInput<SyncArray<T>> Array;
    public readonly ValueInput<int> Index;
    public readonly ValueInput<T> Value;
    public Continuation OnSuccess;
    public Continuation OnFailure;
    
    protected override IOperation Run(FrooxEngineContext context)
    {
        SyncArray<T> array = Array.Evaluate(context);
        int i = Index.Evaluate(context);
        
        if (array == null || i<0)
        {
            return OnFailure.Target;
        }
        array.Insert(Value.Evaluate(context),i);
        return OnSuccess.Target;
    }
}