using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists.Contains;

[NodeCategory("Collections/Lists")]
[NodeOverload("Collections.Lists.Contains")]
[NodeName("List Contains")]
[ContinuouslyChanging]
public class RefListContains<T> : VoidNode<FrooxEngineContext> where T: class, IWorldElement
{
    public readonly ObjectInput<SyncRefList<T>> List;
    public readonly ObjectInput<T> Value;

    public readonly ValueOutput<bool> Contains;

    protected override void ComputeOutputs(FrooxEngineContext context)
    {
        SyncRefList<T> list = List.Evaluate(context);
        T val = Value.Evaluate(context);
        
        if (list == null || list.Count == 0)
        {
            Contains.Write(false,context);
            return;
        }

        Contains.Write(list.Contains(val),context);
    }

    public RefListContains()
    {
        Contains = new ValueOutput<bool>(this);
    }
}