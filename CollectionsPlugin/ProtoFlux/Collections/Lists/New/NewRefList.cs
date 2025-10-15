using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists.New;

[NodeCategory("Collections/Lists")]
[NodeName("New List")]
[NodeOverload("Collections.Lists.NewList")]
public class NewRefList<T> : ActionNode<FrooxEngineContext>
    where T: class, IWorldElement
{
    public Call Next;
    public readonly ObjectOutput<SyncRefList<T>> List;
    protected override IOperation Run(FrooxEngineContext context)
    {
        List.Write(new SyncRefList<T>(),context);
        return Next.Target;
    }

    public NewRefList()
    {
        List = new ObjectOutput<SyncRefList<T>>(this);
    }
}