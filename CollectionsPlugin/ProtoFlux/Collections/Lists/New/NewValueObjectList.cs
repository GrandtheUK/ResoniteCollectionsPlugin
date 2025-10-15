using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists.New;

[NodeCategory("Collections/Lists")]
[NodeName("New List")]
[NodeOverload("Collections.Lists.NewList")]
public class NewValueObjectList<T> : ActionNode<FrooxEngineContext>
    where T: class
{
    public Call Next;
    public readonly ObjectOutput<SyncFieldList<T>> List;
    protected override IOperation Run(FrooxEngineContext context)
    {
        List.Write(new SyncFieldList<T>(),context);
        return Next.Target;
    }

    public NewValueObjectList()
    {
        List = new ObjectOutput<SyncFieldList<T>>(this);
    }
}