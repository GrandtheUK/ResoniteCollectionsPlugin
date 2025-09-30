using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists.Contains;

[NodeCategory("Collections/Lists")]
[NodeOverload("Collections.Lists.Contains")]
[NodeName("Value List Contains")]
public class ValueListContains<T> : VoidNode<FrooxEngineContext> where T: unmanaged
{
    public readonly ObjectInput<SyncFieldList<T>> List;
    public readonly ObjectInput<T> Value;

    public readonly ValueOutput<bool> Contains;

    protected override void ComputeOutputs(FrooxEngineContext context)
    {
        SyncFieldList<T> list = List.Evaluate(context);
        T val = Value.Evaluate(context);
        
        if (list == null)
        {
            Contains.Write(false,context);
        }

        Contains.Write(list.Contains(val),context);
    }
    public ValueListContains()
    {
        Contains = new ValueOutput<bool>(this);
    }
}