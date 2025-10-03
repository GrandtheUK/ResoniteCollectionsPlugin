using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists.Read;

[NodeCategory("Collections/Lists")]
[NodeName("Read List Element")]
[NodeOverload("Collections.Lists.Read")]
public class ReadValueListElement<T> : VoidNode<FrooxEngineContext> where T: unmanaged
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
    public ReadValueListElement()
    {
        Value = new ValueOutput<T>(this);
        HasValue = new ValueOutput<bool>(this);
    }
}