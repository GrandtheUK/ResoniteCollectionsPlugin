using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists
{
    [Category("Collections/Lists")]
    [NodeOverload("Collections.Lists.GetListElement")]
    public class GetValueListElement<T> : VoidNode<FrooxEngineContext> where T: unmanaged
    {
        public readonly ObjectInput<SyncFieldList<T>> List;
        public readonly ValueInput<int> Index;
        public readonly ValueOutput<T> Value;
        public readonly ValueOutput<bool> HasValue;

        protected override void ComputeOutputs(FrooxEngineContext context)
        {
            SyncFieldList<T> l = List.Evaluate(context);
            int i = Index.Evaluate(context,0);
            if (i < 0 || i>=l.Count || l == null) {
                Value.Write(default,context);
                HasValue.Write(false,context);
                return;
            }
            Value.Write(l[i],context);
            HasValue.Write(true,context);
        }

        public GetValueListElement()
        {
            this.Value = new ValueOutput<T>(this);
            this.HasValue = new ValueOutput<bool>(this);
        }
    }

    [NodeCategory("Collections/Lists")]
    [NodeOverload("Collections.Lists.GetListElement")]
    public class GetObjectListElement<T> : VoidNode<FrooxEngineContext> where T: class, IWorldElement
    {
        public readonly ObjectInput<SyncRefList<T>> List;
        public readonly ValueInput<int> Index;
        public readonly ObjectOutput<T> Value;
        public readonly ValueOutput<bool> HasValue;

        protected override void ComputeOutputs(FrooxEngineContext context)
        {
            SyncRefList<T> l = List.Evaluate(context);
            int i = Index.Evaluate(context);
            if (i < 0 || i>=l.Count || l == null) {
                Value.Write(null,context);
                HasValue.Write(false,context);
                return;
            }
            T val = l[i];
            Value.Write(val,context);
            HasValue.Write(true,context);
        }
        public GetObjectListElement()
        {
            this.Value = new ObjectOutput<T>(this);
            this.HasValue = new ValueOutput<bool>(this);
        }
    }
}

