using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Arrays;

[NodeCategory("Collections/Arrays")]
[NodeName("Append Array")]
public class AppendArray<T> : ActionNode<FrooxEngineContext>
    where T: unmanaged, IEquatable<T>
{
    public readonly ObjectInput<SyncArray<T>> Array;
    public readonly ValueInput<T> Value;
    public Continuation OnFailure;
    public Continuation OnSuccess;
    protected override IOperation Run(FrooxEngineContext context)
    {
        SyncArray<T> array = Array.Evaluate(context);
        if (array == null)
        {
            return OnFailure.Target;
        }
        array.Append(Value.Evaluate(context));
        return OnSuccess.Target;
    }
}