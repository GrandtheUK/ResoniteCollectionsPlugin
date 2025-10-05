using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists.Append;

[NodeCategory("Collections/Lists")]
[NodeName("Append List Element")]
[NodeOverload("Collections.List.Append")]
public class AppendValueListElement<T> : ActionNode<FrooxEngineContext>
    where T: unmanaged
{
    public readonly ObjectInput<SyncFieldList<T>> List;
    public readonly ValueInput<T> Value;
    public Continuation OnSuccess;
    public Continuation OnNotFound;
    
    protected override IOperation Run(FrooxEngineContext context)
    {
        SyncFieldList<T> list = List.Evaluate(context);
        if (list == null) return OnNotFound.Target;
        list.Insert(list.Count, Value.Evaluate(context));
        return OnSuccess.Target;
    }
}