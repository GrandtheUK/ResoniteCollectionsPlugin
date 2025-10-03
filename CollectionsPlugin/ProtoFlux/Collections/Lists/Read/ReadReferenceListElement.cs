using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists;

[NodeCategory("Collections/Lists")]
[NodeName("Read Reference List Element")]
[NodeOverload("Collections.Lists.Read")]
public class ReadReferenceListElement<T> : VoidNode<FrooxEngineContext> where T : class, IWorldElement
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

    public ReadReferenceListElement()
    {
        Value = new ObjectOutput<T>(this);
        HasValue = new ValueOutput<bool>(this);
    }
}
