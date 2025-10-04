using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists.Append;

[NodeCategory("Collections/Lists")]
[NodeName("Append List Element")]
[NodeOverload("Collections.List.Append")]
public class AppendReferenceListElement<T> : ActionNode<FrooxEngineContext>
    where T: class, IWorldElement, new()
{
    public readonly ObjectInput<SyncRefList<T>> List;
    public readonly ObjectInput<T> Value;
    public Continuation OnSuccess;
    public Continuation OnNotFound;
    
    protected override IOperation Run(FrooxEngineContext context)
    {
        SyncRefList<T> list = List.Evaluate(context);
        if (list == null) return OnNotFound.Target;
        list.Append(Value.Evaluate(context));
        return OnSuccess.Target;
    }
}