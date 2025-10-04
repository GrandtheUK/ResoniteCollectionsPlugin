using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists.Append;

[NodeCategory("Collections/Lists")]
[NodeName("Append List Element")]
[NodeOverload("Collections.List.Append")]
public class AppendObjectValueListElement<T> : ActionNode<FrooxEngineContext>
{
    public readonly ObjectInput<SyncFieldList<T>> List;
    public readonly ObjectInput<T> Value;
    public Continuation OnSuccess;
    public Continuation OnNotFound;
    
    protected override IOperation Run(FrooxEngineContext context)
    {
        SyncFieldList<T> list = List.Evaluate(context);
        if (list == null) return OnNotFound.Target;
        list.Append(Value.Evaluate(context));
        return OnSuccess.Target;
    }
}