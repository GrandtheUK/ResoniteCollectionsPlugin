using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists;

[NodeCategory("Collections/Lists")]
[NodeName("List Events")]
public class ListEvents: VoidNode<FrooxEngineContext>
{
    public Call OnAdded;
    public Call OnRemoving;
    public readonly GlobalRef<ISyncList> List;
    public readonly ObjectOutput<ISyncMember> Value;
    private ObjectStore<ISyncList> _list;

    private ObjectStore<SyncListElementsEvent> _added;
    private ObjectStore<SyncListElementsEvent> _removing;
    private HashSet<NodeContextPath> _currentlyFiring = new HashSet<NodeContextPath>();

    public override bool CanBeEvaluated => false;

    private void OnListChanged(ISyncList value, FrooxEngineContext context)
    {
        ISyncList l = _list.Read(context);
        if (l != null)
        {
            l.ElementsAdded -= _added.Read(context);
            l.ElementsRemoving -= _removing.Read(context);
        }
        if (value != null)
        {
            NodeContextPath path = context.CaptureContextPath();
            context.GetEventDispatcher(out ExecutionEventDispatcher<FrooxEngineContext> dispatcher);
            SyncListElementsEvent added = (list, startIndex, count) =>
            {
                lock (_currentlyFiring)
                {
                    if (_currentlyFiring.Contains(path))
                        return;
                    while (count > 0)
                    {
                        dispatcher.ScheduleEvent(path,
                            ListOnElementsAdded, list.GetElement(startIndex));
                        count -= 1;
                        startIndex += 1;
                    }
                }
            };
            SyncListElementsEvent removing = (list, startIndex, count) =>
            {
                lock (_currentlyFiring)
                {
                    if (_currentlyFiring.Contains(path))
                        return;
                    while (count > 0)
                    {
                        dispatcher.ScheduleEvent(path,
                            ListOnElementsRemoving, list.GetElement(startIndex));
                        count -= 1;
                        startIndex += 1;
                    }
                }
            };

            value.ElementsAdded += added;
            value.ElementsRemoving += removing;
            
            _added.Write(added,context);
            _removing.Write(removing,context);
            _list.Write(value,context);
        }
        else
        {
            _added.Clear(context);
            _removing.Clear(context);
            _list.Clear(context);
        }
    }
    
    void ListOnElementsAdded(FrooxEngineContext context, object args)
    {
        NodeContextPath path = context.CaptureContextPath();
        try
        {
            lock (_currentlyFiring)
                _currentlyFiring.Add(path);
            Value.Write((ISyncMember)args,context);
            OnAdded.Execute(context);
        }
        finally
        {
            lock (_currentlyFiring)
                _currentlyFiring.Remove(path);
        }
    }
    
    void ListOnElementsRemoving(FrooxEngineContext context, object args)
    {
        NodeContextPath path = context.CaptureContextPath();
        try
        {
            lock (_currentlyFiring)
                _currentlyFiring.Add(path);
            Value.Write((ISyncMember)args,context);
            OnRemoving.Execute(context);
        }
        finally
        {
            lock (_currentlyFiring)
                _currentlyFiring.Remove(path);
        }
    }

    public ListEvents()
    {
        List = new GlobalRef<ISyncList>(this,0);
        Value = new ObjectOutput<ISyncMember>(this);
    }
}