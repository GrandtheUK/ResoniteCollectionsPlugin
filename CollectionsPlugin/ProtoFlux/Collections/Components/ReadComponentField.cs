using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Components;

[NodeCategory("Collections/Components")]
[NodeName("Read Component Field")]
[ContinuouslyChanging]
public class ReadComponentField<T> : VoidNode<FrooxEngineContext>
{
    public readonly ObjectInput<Component> Component;
    public readonly ObjectInput<string> FieldName;
    public readonly ObjectOutput<IField<T>> Field;
    public readonly ValueOutput<bool> HasField;

    protected override void ComputeOutputs(FrooxEngineContext context)
    {
        Component? c = Component.Evaluate(context);
        string s = FieldName.Evaluate(context);
        if (c == null || s == null || s.Length == 0)
        {
            Field.Write(null,context);
            HasField.Write(false,context);
            return;
        }
        IField<T> f = c.TryGetField<T>(s);
        if (f == null)
        {
            Field.Write(null,context);
            HasField.Write(false,context);
            return;
        }
        Field.Write(f, context);
        HasField.Write(true, context);
    }

    public ReadComponentField()
    {
        Field = new ObjectOutput<IField<T>>(this);
        HasField = new ValueOutput<bool>(this);
    }
}