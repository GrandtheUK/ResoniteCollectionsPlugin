using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists;

[NodeCategory("Collections/Lists")]
[NodeName("Write Value List Element")]
[NodeOverload("Collections.Lists.Write")]
public class WriteValueListElement<T> : ActionNode<FrooxEngineContext>
    where T: unmanaged
{
    public readonly ObjectInput<ISyncList> List;
    public readonly ValueInput<int> Index;
    public readonly ValueInput<T> Value;
    public Continuation OnFailed;
    public Continuation OnSuccess;
    public Continuation OnNotFound;
    

    protected override IOperation Run(FrooxEngineContext context)
    {
        SyncFieldList<T> list = (SyncFieldList<T>) List.Evaluate(context);
        int index = Index.Evaluate(context);
        if (list == null || index < 0) return OnNotFound.Target;
        if (index > list.Count) return OnFailed.Target;
        
        list[index] = Value.Evaluate(context);
        return OnSuccess.Target;
    }
}