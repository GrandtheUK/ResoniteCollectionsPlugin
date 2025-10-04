using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists.Insert;

[NodeCategory("Collections/Lists")]
[NodeName("Insert List Element")]
[NodeOverload("Collections.List.Insert")]
public class InsertReferenceListElement<T> : ActionNode<FrooxEngineContext>
    where T: class, IWorldElement, new()
{
    public readonly ObjectInput<SyncRefList<T>> List;
    public readonly ValueInput<int> Index;
    public readonly ObjectInput<T> Value;
    public Continuation OnFailed;
    public Continuation OnSuccess;
    public Continuation OnNotFound;
    
    protected override IOperation Run(FrooxEngineContext context)
    {
        SyncRefList<T> list = List.Evaluate(context);
        int i = Index.Evaluate(context);
        if (list == null || i<0 ) return OnNotFound.Target;
        if (i > list.Count) return OnFailed.Target;
        list.Insert(i, Value.Evaluate(context));
        return OnSuccess.Target;
    }
}