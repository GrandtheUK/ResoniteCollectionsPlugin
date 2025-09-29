using System.Collections;
using Elements.Core;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

using IInput = ProtoFlux.Core.IInput;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists;

public abstract class WriteListElement : ActionNode<FrooxEngineContext>
{
    public readonly ObjectInput<ISyncList> List;
    public readonly ValueInput<int> Index;
    public Continuation OnFailed;
    public Continuation OnSuccess;
    public Continuation OnNotFound;

    protected IOperation Run(FrooxEngineContext context, ISyncList list, int index)
    {
        if (list == null || index < 0) return OnNotFound.Target;
        if (index > list.Count) return OnFailed.Target;
        return null!;
    }
}


[NodeCategory("Collections/Lists")]
[NodeName("Write Value List Element")]
[NodeOverload("Collections.Lists.Write")]
public class WriteValueListElement<T> : WriteListElement
    where T: unmanaged
{
    public readonly ValueInput<T> Value;

    protected override IOperation Run(FrooxEngineContext context)
    {
        SyncFieldList<T> list = (SyncFieldList<T>) List.Evaluate(context);
        int index = Index.Evaluate(context);
        IOperation ret = base.Run(context,list, index);
        if (ret != null) return ret;
        
        list[index] = Value.Evaluate(context);
        return OnSuccess.Target;
    }
}

[NodeCategory("Collections/Lists")]
[NodeName("Write Object List Element")]
[NodeOverload("Collections.Lists.Write")]
public class WriteObjectListElement<T> : WriteListElement
{
    public readonly ObjectInput<T> Value;

    protected override IOperation Run(FrooxEngineContext context)
    {
        SyncFieldList<T> list =  (SyncFieldList<T>) List.Evaluate(context);
        int index = Index.Evaluate(context);
        IOperation ret = base.Run(context, list, index);
        if (ret != null) return ret;
        
        list[index] = Value.Evaluate(context);
        return OnSuccess.Target;
    }
}

[NodeCategory("Collections/Lists")]
[NodeName("Write Reference List Element")]
[NodeOverload("Collections.Lists.Write")]
public class WriteRefListElement<T> : WriteListElement
    where T: class, IWorldElement
{
    public readonly ObjectInput<T> Value;

    protected override IOperation Run(FrooxEngineContext context)
    {
        SyncRefList<T> list =  (SyncRefList<T>) List.Evaluate(context);
        int index = Index.Evaluate(context);
        IOperation ret = base.Run(context, list, index);
        if (ret != null) return ret;
        
        list[index] = Value.Evaluate(context);
        return OnSuccess.Target;
    }
}