using Elements.Core;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists;

[NodeCategory("Collections/Lists")]
[NodeName("Get Value List Element")]
[NodeOverload("Collections.Lists.Read")]
public class GetValueListElement<T> : VoidNode<FrooxEngineContext> where T: unmanaged
{
    public readonly ObjectInput<SyncFieldList<T>> List;
    public readonly ValueInput<int> Index;
    public readonly ValueOutput<T> Value;
    public readonly ValueOutput<bool> HasValue;
    
    protected override void ComputeOutputs(FrooxEngineContext context)
    {
        SyncFieldList<T> l = List.Evaluate(context);
        int i = Index.Evaluate(context);
        if (i < 0 ||  l == null || i>=l.Count || List.Source == null || l.Count == 0) {
            Value.Write(default,context);
            HasValue.Write(false,context);
            return;
        }
        Value.Write(l[i],context);
        HasValue.Write(true,context);
    }
    public GetValueListElement()
    {
        Value = new ValueOutput<T>(this);
        HasValue = new ValueOutput<bool>(this);
    }
}


[NodeCategory("Collections/Lists")]
[NodeName("Get Object List Element")]
[NodeOverload("Collections.Lists.Read")]
public class GetObjectListElement<T> : VoidNode<FrooxEngineContext>
{
    public readonly ObjectInput<ISyncList> List;
    public readonly ValueInput<int> Index;
    public readonly ObjectOutput<T> Value;
    public readonly ValueOutput<bool> HasValue;
    
    Type[] _types = [typeof(string),typeof(Uri),typeof(Nullable)];

    protected override void ComputeOutputs(FrooxEngineContext context)
    {
        ISyncList l = List.Evaluate(context);
        int i = Index.Evaluate(context);
        if (i < 0 || l == null || i >= l.Count || l.Count == 0 || !_types.Contains(typeof(T)))
        {
            Value.Write(default,context);
            HasValue.Write(false,context);
            return;
        }
        Value.Write((T)l.GetElement(i), context);
        HasValue.Write(true,context);
    }

    public GetObjectListElement()
    {
        Value = new ObjectOutput<T>(this);
        HasValue = new ValueOutput<bool>(this);
    }
}

[NodeCategory("Collections/Lists")]
[NodeName("Get Reference List Element")]
[NodeOverload("Collections.Lists.Read")]
public class GetRefListElement<T> : VoidNode<FrooxEngineContext> where T : class, IWorldElement
{
    public readonly ObjectInput<SyncRefList<T>> List;
    public readonly ValueInput<int> Index;
    public readonly ObjectOutput<T> Value;
    public readonly ValueOutput<bool> HasValue;

    protected override void ComputeOutputs(FrooxEngineContext context)
    {
        SyncRefList<T> l = List.Evaluate(context);
        int i = Index.Evaluate(context);
        if (i < 0 || l == null || i >= l.Count || l.Count == 0)
        {
            Value.Write(null,context);
            HasValue.Write(false,context);
            return;
        }
        Value.Write(l[i], context);
        HasValue.Write(true,context);
    }

    public GetRefListElement()
    {
        Value = new ObjectOutput<T>(this);
        HasValue = new ValueOutput<bool>(this);
    }
}
