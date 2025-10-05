using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists;

[NodeCategory("Collections/Lists/Experimental")]
[NodeName("List Events")]
public class ListEvents: VoidNode<FrooxEngineContext>
{
    public Call OnAdded;
    public Call OnRemoved;
    public Call OnChanged;
    public readonly GlobalRef<ISyncList> List;
    public readonly ObjectOutput<ISyncMember> Value;
    private ObjectStore<ISyncList> _list;

    private ObjectStore<SyncListElementsEvent> _added;
    private ObjectStore<SyncListElementsEvent> _removed;
    private ObjectStore<Action<IChangeable>> _changed;
    private HashSet<NodeContextPath> _currentlyFiring = new HashSet<NodeContextPath>();

    public struct ListEvent(
        in ISyncList syncList,
        in int startIndex,
        in int count)
    {
        public readonly ISyncList syncList = syncList;
        public readonly int startIndex = startIndex;
        public readonly int count = count;
    }

    public struct ListEventData(ISyncMember Value)
    {
        public readonly ISyncMember Value = Value;
    }

    private void WriteEventData(in ListEventData eventData, FrooxEngineContext context)
    {
        Value.Write(eventData.Value,context);
    }

    private void OnListChanged(ISyncList value, FrooxEngineContext context)
    {
        ISyncList list = _list.Read(context);
        if (list != null)
        {
            list.ElementsAdded -= _added.Read(context);
            list.ElementsRemoved -= _removed.Read(context);
            list.Changed -= _changed.Read(context);
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
                }
                dispatcher.ScheduleEvent(path,
                    ListOnElementsAdded, new ListEvent(list, startIndex, count));
            };
            SyncListElementsEvent removed = (list, startIndex, count) =>
            {
                lock (_currentlyFiring)
                {
                    if (_currentlyFiring.Contains(path))
                        return;
                }

                dispatcher.ScheduleEvent(path,
                    ListOnElementsRemoved, new ListEvent(list, startIndex, count));
            };
            Action<IChangeable> changed = (changed) =>
            {
                lock (_currentlyFiring)
                {
                    if (_currentlyFiring.Contains(path))
                        return;
                }
                dispatcher.ScheduleEvent(path, ValueOnChanged, changed);
            };
            value.ElementsAdded += added;
            value.ElementsRemoved += removed;
            value.Changed += changed;
            
            _added.Write(added,context);
            _removed.Write(removed,context);
            _changed.Write(changed,context);
            _list.Write(value,context);
        }
        else
        {
            _added.Clear(context);
            _removed.Clear(context);
            _changed.Clear(context);
            _list.Clear(context);
        }
    }
    
    void ListOnElementsAdded(FrooxEngineContext context,object args)
    {
        NodeContextPath path = context.CaptureContextPath();
        try
        {
            lock (_currentlyFiring)
                _currentlyFiring.Add(path);
            ListEvent eventData = (ListEvent)args;
            for (int i = eventData.startIndex; i < eventData.count; i++)
            {
                ISyncMember elem = eventData.syncList.GetElement(i);
                ListEventData data = new ListEventData(elem);
                WriteEventData(in data, context);
                OnAdded.Execute(context);
            }
        }
        finally
        {
            lock (_currentlyFiring)
                _currentlyFiring.Remove(path);
        }
    }
    
    void ListOnElementsRemoved(FrooxEngineContext context,object args)
    {
        NodeContextPath path = context.CaptureContextPath();
        try
        {
            lock (_currentlyFiring)
                _currentlyFiring.Add(path);
            ListEvent eventData = (ListEvent)args;
            for (int i = eventData.startIndex; i < eventData.count; i++)
            {
                ISyncMember elem = eventData.syncList.GetElement(i);
                ListEventData data = new ListEventData(elem);
                WriteEventData(in data, context);
                OnRemoved.Execute(context);
            }
        }
        finally
        {
            lock (_currentlyFiring)
                _currentlyFiring.Remove(path);
        }
    }
        
    void ValueOnChanged(FrooxEngineContext context,object args)
    {
        NodeContextPath path = context.CaptureContextPath();
        try
        {
            lock (_currentlyFiring)
                _currentlyFiring.Add(path);
            ListEventData data = new ListEventData((ISyncMember)args);
            OnChanged.Execute(context);
        }
        finally
        {
            lock (_currentlyFiring)
                _currentlyFiring.Remove(path);
        }
    }

    protected override void ComputeOutputs(FrooxEngineContext context)
    {
        Value.Write(default,context);
    }

    public ListEvents()
    {
        List = new GlobalRef<ISyncList>(this,0);
        Value = new ObjectOutput<ISyncMember>(this);
    }
}