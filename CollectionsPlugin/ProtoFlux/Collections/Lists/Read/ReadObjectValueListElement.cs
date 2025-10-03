using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists.Read;

[NodeCategory("Collections/Lists")]
[NodeName("ReadList Element")]
[NodeOverload("Collections.Lists.Read")]
public class ReadObjectValueListElement<T> : VoidNode<FrooxEngineContext>
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

    public ReadObjectValueListElement()
    {
        Value = new ObjectOutput<T>(this);
        HasValue = new ValueOutput<bool>(this);
    }
}