using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Parsing.Write;

[NodeCategory("Collections/Parsing")]
[NodeName("Write Override")]
[NodeOverload("Collections.Parsing.Write")]
public class WriteValueOverride<T> : ActionNode<FrooxEngineContext>
    where T : unmanaged
{
    public readonly ObjectInput<ValueOverrideBase<T>.Override> Override;
    public readonly ValueInput<T> Value;
    public Continuation OnSuccess;
    public Continuation OnFailed;

    protected override IOperation Run(FrooxEngineContext context)
    {
        ValueOverrideBase<T>.Override o = Override.Evaluate(context);
        if (o == null)
        {
            return OnFailed.Target;
        }

        o.Value.Value = Value.Evaluate(context);
        return OnSuccess.Target;
    }
}