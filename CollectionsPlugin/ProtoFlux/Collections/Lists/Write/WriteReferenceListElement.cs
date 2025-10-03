using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists;

[NodeCategory("Collections/Lists")]
[NodeName("Write Reference List Element")]
[NodeOverload("Collections.Lists.Write")]
public class WriteReferenceListElement<T> : ActionNode<FrooxEngineContext>
    where T: class, IWorldElement
{
    public readonly ObjectInput<SyncRefList<T>> List;
    public readonly ObjectInput<T> Value;
    public readonly ValueInput<int> Index;
    public Continuation OnFailed;
    public Continuation OnSuccess;
    public Continuation OnNotFound;

    protected override IOperation Run(FrooxEngineContext context)
    {
        SyncRefList<T> list = List.Evaluate(context);
        int index = Index.Evaluate(context);
        if (list == null || index < 0) return OnNotFound.Target;
        if (index > list.Count) return OnFailed.Target;
        
        list[index] = Value.Evaluate(context);
        return OnSuccess.Target;
    }
}