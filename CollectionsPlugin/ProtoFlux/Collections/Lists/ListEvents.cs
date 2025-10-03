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
    private ObjectStore<ISyncMember> _elem;

    private void OnListChanged(ISyncList value, FrooxEngineContext context)
    {
        ISyncList list = _list.Read(context);
        if (list != null)
        {
            list.ElementsAdded -= ListOnElementsAdded;
            list.ElementsRemoved -= ListOnElementsRemoved;
            list.Changed -= ValueOnChanged;
        }

        void ListOnElementsAdded(ISyncList syncList, int startIndex, int count)
        {
            for (int i = startIndex; i < count; i++)
            {
                ISyncMember elem = syncList.GetElement(i);
                _elem.Write(elem,context);
                Compute(context);
                _elem.Write(default,context);
            }
            OnAdded.Execute(context);
        }

        void ListOnElementsRemoved(ISyncList syncList, int startIndex, int count)
        {
            for (int i = startIndex; i < count; i++)
            {
                ISyncMember elem = syncList.GetElement(i);
                _elem.Write(elem,context);
                Compute(context);
                _elem.Write(default,context);
            }
            OnRemoved.Execute(context);
        }
        
        void ValueOnChanged(IChangeable obj)
        {
            _elem.Write((ISyncMember)obj,context);
            Compute(context);
            _elem.Write(default,context);
            OnChanged.Execute(context);
        }

        if (value != null)
        {
            value.ElementsAdded += ListOnElementsAdded;
            value.ElementsRemoved += ListOnElementsRemoved;
            value.Changed += ValueOnChanged;
        }
        _list.Write(value,context);
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