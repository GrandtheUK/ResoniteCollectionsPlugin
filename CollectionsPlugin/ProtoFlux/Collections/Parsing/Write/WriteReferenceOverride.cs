using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Parsing.Write;

[NodeCategory("Collections/Parsing")]
[NodeName("Write Override")]
[NodeOverload("Collections.Parsing.Write")]
public class WriteReferenceOverride<T> : ActionNode<FrooxEngineContext>
    where T : class, IWorldElement, new()
{
    public readonly ObjectInput<ReferenceUserOverride<T>.Override> Override;
    public readonly ObjectInput<T> Value;
    public Continuation OnSuccess;
    public Continuation OnFailed;

    protected override IOperation Run(FrooxEngineContext context)
    {
        ReferenceUserOverride<T>.Override o = Override.Evaluate(context);
        if (o == null)
        {
            return OnFailed.Target;
        }

        o.Value.Target = Value.Evaluate(context);
        return OnSuccess.Target;
    }
}