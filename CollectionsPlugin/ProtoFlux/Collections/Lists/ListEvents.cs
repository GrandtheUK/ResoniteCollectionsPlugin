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

    public struct ListEventData(
        ISyncMember Value,
        in ISyncList syncList,
        in int startIndex,
        in int count)
    {
        public ISyncMember Value = Value;
        public readonly ISyncList syncList = syncList;
        public readonly int startIndex = startIndex;
        public readonly int count = count;
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
            SyncListElementsEvent added = (list, startIndex, count) => dispatcher.ScheduleEvent(path,
                ListOnElementsAdded, new ListEventData(null,list,startIndex,count));
            SyncListElementsEvent removed = (list, startIndex, count) => dispatcher.ScheduleEvent(path,
                ListOnElementsRemoved, new ListEventData(null,list,startIndex,count));
            Action<IChangeable> changed = (changed) => dispatcher.ScheduleEvent(path, ValueOnChanged, changed);
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
        ListEventData eventData = (ListEventData)args;
        for (int i = eventData.startIndex; i < eventData.count; i++)
        {
            ISyncMember elem = eventData.syncList.GetElement(i);
            eventData.Value = elem;
            WriteEventData(in eventData, context);
            OnAdded.Execute(context);
        }
    }
    
    void ListOnElementsRemoved(FrooxEngineContext context,object args)
    {
        ListEventData eventData = (ListEventData)args;
        for (int i = eventData.startIndex; i < eventData.count; i++)
        {
            ISyncMember elem = eventData.syncList.GetElement(i);
            eventData.Value = elem;
            WriteEventData(in eventData, context);
            OnRemoved.Execute(context);
        }
        
    }
        
    void ValueOnChanged(FrooxEngineContext context,object args)
    {
        Value.Write((ISyncMember)args,context);
        OnChanged.Execute(context);
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