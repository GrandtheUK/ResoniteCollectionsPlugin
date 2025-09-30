using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists.Contains;

[NodeCategory("Collections/Lists")]
[NodeOverload("Collections.Lists.Contains")]
[NodeName("Object List Contains")]
public class ObjectListContains<T> : VoidNode<FrooxEngineContext>
{
    public readonly ObjectInput<ISyncList> List;
    public readonly ObjectInput<T> Value;
    public readonly ValueOutput<bool> Contains;

    protected override void ComputeOutputs(FrooxEngineContext context)
    {
        ISyncList list = List.Evaluate(context);
        T val = Value.Evaluate(context);
        
        if (list == null)
        {
            Contains.Write(false,context);
        }

        switch (list.GetType())
        {
            case SyncFieldList<T> l:
                Contains.Write(l.Contains(val),context);
                break;
            default:
                Contains.Write(false,context);
                break;
        } 
    }
    public ObjectListContains()
    {
        Contains = new ValueOutput<bool>(this);
    }
}