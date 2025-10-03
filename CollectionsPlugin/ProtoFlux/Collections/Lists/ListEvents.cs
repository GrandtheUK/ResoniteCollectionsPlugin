using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists;

[NodeCategory("Collections/Lists")]
[NodeName("List Events")]
public class ListEvents: ObjectFunctionNode<FrooxEngineContext,ISyncMember>
{
    public Call OnAdded;
    public Call OnRemoved;
    public Call OnChanged;
    public readonly GlobalRef<ISyncList> List;
    private ObjectStore<ISyncList> _list;

    private ObjectStore<SyncListElementsEvent> _added;
    private ObjectStore<SyncListElementsEvent> _removed;
    private ObjectStore<Action<IChangeable>> _changed;
    
    private ObjectStore<ISyncMember> _elem;

    private class ListEventData
    {
        public ISyncList syncList;
        public int startIndex;
        public int count;

        public ListEventData(ISyncList syncList, int startIndex, int count)
        {
            this.syncList = syncList;
            this.startIndex = startIndex;
            this.count = count;
        }
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
            ExecutionEventDispatcher<FrooxEngineContext> dispatcher;
            context.GetEventDispatcher(out dispatcher);
            SyncListElementsEvent added = (list, startIndex, count) => dispatcher.ScheduleEvent(path,
                ListOnElementsAdded, new ListEventData(list,startIndex,count));
            SyncListElementsEvent removed = (list, startIndex, count) => dispatcher.ScheduleEvent(path,
                ListOnElementsRemoved, new ListEventData(list,startIndex,count));
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
            _elem.Write(elem,context);
            Compute(context);
            _elem.Write(default,context);
        }
        OnAdded.Execute(context);
    }
    
    void ListOnElementsRemoved(FrooxEngineContext context,object args)
    {
        ListEventData eventData = (ListEventData)args;
        for (int i = eventData.startIndex; i < eventData.count; i++)
        {
            ISyncMember elem = eventData.syncList.GetElement(i);
            _elem.Write(elem,context);
            Compute(context);
            _elem.Write(default,context);
        }
        OnRemoved.Execute(context);
    }
        
    void ValueOnChanged(FrooxEngineContext context,object args)
    {
        _elem.Write((ISyncMember)args,context);
        Compute(context);
        _elem.Write(default,context);
        OnChanged.Execute(context);
    }
    public ListEvents()
    {
        List = new GlobalRef<ISyncList>(this,0);
    }

    public ISyncList Read(FrooxEngineContext context) => List.Read(context);

    public bool Write(ISyncList value, FrooxEngineContext context) => List.Write(value, context);
    protected override ISyncMember Compute(FrooxEngineContext context)
    {
        return _elem.Read(context);
    }
}