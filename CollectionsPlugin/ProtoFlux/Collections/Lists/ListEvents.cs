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
    public Call OnRemoved;
    public readonly ObjectOutput<ISyncMember> Value;
    public readonly GlobalRef<ISyncList> List;
    private ObjectStore<ISyncList> _list;

    private void OnListChanged(ISyncList value, FrooxEngineContext context)
    {
        ISyncList list = _list.Read(context);
        if (list != null)
        {
            list.ElementsAdded -= ListOnElementsAdded;
            list.ElementsRemoved -= ListOnElementsRemoved;
        }

        void ListOnElementsAdded(ISyncList syncList, int startIndex, int count)
        {
            for (int i = startIndex; i < count; i++)
            {
                ISyncMember elem = syncList.GetElement(i);
                Value.Write(elem, context);
            }
            OnAdded.Execute(context);
        }

        void ListOnElementsRemoved(ISyncList syncList, int startIndex, int count)
        {
            for (int i = startIndex; i < count; i++)
            {
                ISyncMember elem = syncList.GetElement(i);
                Value.Write(elem, context);
            }
            OnRemoved.Execute(context);
        }

        if (value != null)
        {
            value.ElementsAdded += ListOnElementsAdded;
            value.ElementsRemoved += ListOnElementsRemoved;
        }
        _list.Write(value,context);
    }
    public ListEvents()
    {
        List = new GlobalRef<ISyncList>(this,0);
        Value = new ObjectOutput<ISyncMember>(this);
    }

    public ISyncList Read(FrooxEngineContext context) => List.Read(context);

    public bool Write(ISyncList value, FrooxEngineContext context) => List.Write(value, context);
}