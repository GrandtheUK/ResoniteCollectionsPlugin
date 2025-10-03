using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Bags;

[NodeCategory("Collections/Bags")]
public class ReadBag<T> : ObjectFunctionNode<FrooxEngineContext,T>
    where T: class, ISyncMember, new()
{
    public readonly ObjectInput<SyncBag<T>> Bag;
    public readonly ValueInput<int> Index;
    protected override T Compute(FrooxEngineContext context)
    {
        SyncBag<T> bag = Bag.Evaluate(context);
        int i = Index.Evaluate(context);
        if (bag == null || bag.Count == 0 || i < 0 || i >= bag.Count)
        {
            return default;
        }

        if (typeof(T).IsGenericType)
        {
            return bag.ElementAt(i).Value;
        }
        return bag.ElementAt(i).Value as T;
    }
}