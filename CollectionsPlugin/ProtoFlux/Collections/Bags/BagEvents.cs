using Elements.Core;
using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Bags;

[NodeCategory("Collections/Bags")]
[NodeName("Bag Events")]
public class BagEvents<T> : VoidNode<FrooxEngineContext>
    where T : class, ISyncMember, new()
{
    public override bool CanBeEvaluated => false;

    public readonly GlobalRef<SyncBag<T>> Bag;
    public Call OnAdded;
    public Call OnRemoved;
    public readonly ObjectOutput<T> Value;
    private ObjectStore<SyncBag<T>> _bag;

    private ObjectStore<SyncBagAddEvent<RefID,T>> _added;
    private ObjectStore<SyncBagRemoveEvent<RefID,T>> _removed;
    private HashSet<NodeContextPath> _currentlyFiring = new HashSet<NodeContextPath>();

    public void OnBagChanged(SyncBag<T> value, FrooxEngineContext context)
    {
        SyncBag<T> b = _bag.Read(context);
        if (b != null)
        {
            b.OnElementAdded += _added.Read(context);
            b.OnElementRemoved -= _removed.Read(context);
        }

        if (value != null)
        {
            NodeContextPath path = context.CaptureContextPath();
            context.GetEventDispatcher(out ExecutionEventDispatcher<FrooxEngineContext> dispatcher);

            SyncBagAddEvent<RefID, T> added = (_, _, element, _) =>
            {
                lock (_currentlyFiring)
                {
                    if (_currentlyFiring.Contains(path))
                        return;
                    dispatcher.ScheduleEvent(path,
                        BagOnElementsAdded, element);
                }
            };
            SyncBagRemoveEvent<RefID, T> removed = (_, _, element) =>
            {
                lock (_currentlyFiring)
                {
                    if (_currentlyFiring.Contains(path))
                        return;
                    dispatcher.ScheduleEvent(path,
                        BagOnElementsRemoved, element);
                }
            };

            value.OnElementAdded += added;
            value.OnElementRemoved += removed;
            
            _added.Write(added,context);
            _removed.Write(removed,context);
            _bag.Write(value, context);
        }
        else
        {
            _added.Clear(context);
            _removed.Clear(context);
            _bag.Clear(context);
        }
    }

    private void BagOnElementsAdded(FrooxEngineContext context, object eventData)
    {
        Value.Write((T)eventData,context);
        OnAdded.Execute(context);
    }
    private void BagOnElementsRemoved(FrooxEngineContext context, object eventData)
    {
        Value.Write((T)eventData,context);
        OnRemoved.Execute(context);
    }

    public BagEvents()
    {
        Bag = new GlobalRef<SyncBag<T>>(this, 0);
        Value = new ObjectOutput<T>(this);
    }
}