using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace ExamplePlugin.ProtoFlux.PluginTemplate {
    [NodeCategory("PluginTemplate")]
    [NodeName("My Flux Node")]
    public class MyFluxNode : VoidNode<FrooxEngineContext>
    {
        public readonly ObjectOutput<string> text;

        override protected void ComputeOutputs(FrooxEngineContext context)
        {
            text.Write("Hello World!",context);
        }

        public MyFluxNode()
        {
            this.text = new ObjectOutput<string>(this);
        }
    }
    
    [NodeCategory("PluginTemplate")]
    [NodeName("Create Empty Slot")]
    public class CreateEmptySlot : ActionNode<FrooxEngineContext>
    {
        public ObjectInput<string> Name;

        public ObjectInput<Slot> Parent;

        public ObjectOutput<Slot> Slot;
    
        public Call OnAdded;
    
        protected override IOperation Run(FrooxEngineContext context)
        {
            var parent = Parent.Evaluate(context) ?? context.World.RootSlot;
            var name = Name.Evaluate(context, defaultValue: "EmptyObject");
            Slot.Write(parent.AddSlot(name),context);
            return OnAdded.Target;
        }

        public CreateEmptySlot()
        {
            Slot = new ObjectOutput<Slot>(this);
        }
    }
}
