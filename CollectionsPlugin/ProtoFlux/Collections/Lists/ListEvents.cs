using Elements.Core;
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
    public Call OnRemoving;
    public readonly GlobalRef<ISyncList> List;
    public readonly ObjectOutput<ISyncMember> Value;
    private ObjectStore<ISyncList> _list;

    private ObjectStore<SyncListElementsEvent> _added;
    private ObjectStore<SyncListElementsEvent> _removing;
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
            list.ElementsRemoved -= _removing.Read(context);
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
            SyncListElementsEvent removing = (list, startIndex, count) =>
            {
                lock (_currentlyFiring)
                {
                    if (_currentlyFiring.Contains(path))
                        return;
                }

                dispatcher.ScheduleEvent(path,
                    ListOnElementsRemoving, new ListEvent(list, startIndex, count));
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
    
    void ListOnElementsAdded(FrooxEngineContext context,object args)
    {
        NodeContextPath path = context.CaptureContextPath();
        try
        {
            lock (_currentlyFiring)
                _currentlyFiring.Add(path);
            ListEvent eventData = (ListEvent)args;
            int count = _list.Read(context).Count;
            List<ISyncMember> list = Pool.BorrowList<ISyncMember>();
            for (int i = 0; i < count; ++i)
            {
                ISyncMember elem = eventData.syncList.GetElement(i);
                if (elem != null && !elem.IsRemoved)
                {
                    ListEventData data = new ListEventData(elem);
                    WriteEventData(in data, context);
                    OnAdded.Execute(context);
                }
            }
            Pool.Return(ref list);
        }
        finally
        {
            lock (_currentlyFiring)
                _currentlyFiring.Remove(path);
        }
    }
    
    void ListOnElementsRemoving(FrooxEngineContext context,object args)
    {
        NodeContextPath path = context.CaptureContextPath();
        try
        {
            lock (_currentlyFiring)
                _currentlyFiring.Add(path);
            ListEvent eventData = (ListEvent)args;
            List<ISyncMember> list = Pool.BorrowList<ISyncMember>();
            for (int i = eventData.startIndex; i < eventData.startIndex + eventData.count; i++)
            {
                if (i < _list.Read(context).Count)
                    list.Add(eventData.syncList.GetElement(i));
            }

            foreach (ISyncMember elem in list)
            {
                ListEventData data = new ListEventData(elem);
                WriteEventData(in data, context);
                OnRemoving.Execute(context);
            }
            Pool.Return(ref list);
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