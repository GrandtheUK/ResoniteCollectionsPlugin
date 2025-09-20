using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Lists
{
    [NodeCategory("Collections/Lists")]
    public class ListLength : VoidNode<FrooxEngineContext>
    {
        public readonly ObjectInput<ISyncList> List;
        public readonly ValueOutput<int> Length;

        override protected void ComputeOutputs(FrooxEngineContext context)
        {
            ISyncList l = List.Evaluate(context);
            Length.Write(l.Count, context);
        }

        public ListLength()
        {
            Length = new ValueOutput<int>(this);
        }
    }
}
