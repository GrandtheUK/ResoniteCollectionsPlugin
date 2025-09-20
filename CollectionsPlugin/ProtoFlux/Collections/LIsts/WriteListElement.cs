using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists 
{
    [NodeCategory("Collections/Lists")]
    [NodeName("Write List Element")]
    [NodeOverload("Collections.Lists.WriteListElement")]
    public class WriteValueListElement<T> : ActionNode<FrooxEngineContext> where T: unmanaged
    {
        public readonly ObjectInput<SyncFieldList<T>> List;
        public readonly ValueInput<int> Index;
        public readonly ValueInput<T> Value;
        public readonly Continuation Sucess;
        public readonly Continuation Failure;
        public readonly Continuation NotFound;
        
        protected override IOperation Run(FrooxEngineContext context)
        {
            SyncFieldList<T> l = List.Evaluate(context);
            int i = Index.Evaluate(context);
            if (l == null)
            {
                return NotFound.Target;
            }

            if (i < 0 || i > l.Count)
            {
                return Failure.Target;
            }
            
            l[i] = Value.Evaluate(context);
            return Sucess.Target;
        }
    }
    [NodeCategory("Collections/Lists")]
    [NodeName("Write List sElement")]
    [NodeOverload("Collections.Lists.WriteListElement")]
    public class WriteObjectListElement<T> : ActionNode<FrooxEngineContext> where T: class, IWorldElement
    {
        public readonly ObjectInput<SyncRefList<T>> List;
        public readonly ValueInput<int> Index;
        public readonly ObjectInput<T> Object;
        public readonly Continuation Sucess;
        public readonly Continuation Failure;
        public readonly Continuation NotFound;
        

        protected override IOperation Run(FrooxEngineContext context)
        {
            SyncElementList<SyncRef<T>> l = List.Evaluate(context);
            int i = Index.Evaluate(context);
            if (l == null)
            {
                return NotFound.Target;
            }

            if (i < 0 || i > l.Count)
            {
                return Failure.Target;
            }
            
            l[i].Target = Object.Evaluate(context);
            return Sucess.Target;
        }
    }
}
