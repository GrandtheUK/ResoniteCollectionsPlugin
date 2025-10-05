using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Bags;

[NodeCategory("Collections/Bags")]
[NodeName("Append Bag")]
public class AppendBag<T> : ActionNode<FrooxEngineContext>
    where T: class, ISyncMember, new()
{
    public readonly ObjectInput<SyncBag<T>> Bag;
    public readonly ObjectInput<T> Element;

    public Continuation OnSuccess;
    public Continuation OnFailed;
    protected override IOperation Run(FrooxEngineContext context)
    {
        SyncBag<T> bag = Bag.Evaluate(context);
        T elem = Element.Evaluate(context);
        if (bag == null)
        {
            return OnFailed.Target;
        }
        bag.Add(context.World.ReferenceController.PeekID(), elem, true);
        return OnSuccess.Target;
    }
}