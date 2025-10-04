using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists.Insert;

public class InsertValueListElement<T> : ActionNode<FrooxEngineContext>
    where T: unmanaged
{
    public readonly ObjectInput<SyncFieldList<T>> List;
    public readonly ValueInput<int> Index;
    public readonly ValueInput<T> Value;
    public Continuation OnFailure;
    public Continuation OnSuccess;
    public Continuation OnNotFound;
    
    protected override IOperation Run(FrooxEngineContext context)
    {
        SyncFieldList<T> list = List.Evaluate(context);
        int i = Index.Evaluate(context);
        if (list == null || i<0 ) return OnNotFound.Target;
        if (i > list.Count) return OnFailure.Target;
        list.Insert(i, Value.Evaluate(context));
        return OnSuccess.Target;
    }
}