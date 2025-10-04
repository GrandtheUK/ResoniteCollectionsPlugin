using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists;

[NodeCategory("Collections/Lists")]
[NodeName("Remove List Element")]
public class RemoveListElement : ActionNode<FrooxEngineContext>
{
    public readonly ObjectInput<ISyncList> List;
    public readonly ValueInput<int> Index;
    public Continuation OnFailed;
    public Continuation OnSuccess;
    public Continuation OnNotFound;
    protected override IOperation Run(FrooxEngineContext context)
    {
        ISyncList list = List.Evaluate(context);
        int index = Index.Evaluate(context);
        if (list == null || index < 0) return OnNotFound.Target;
        if (index > list.Count) return OnFailed.Target;
        list.RemoveElement(index);
        return OnSuccess.Target;
    }
}