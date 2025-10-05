using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Parsing.New;

[NodeCategory("Collections/Parsing")]
[NodeName("New Override")]
[NodeOverload("Collections.Parsing.New")]
public class NewReferenceOverride<T> : ActionNode<FrooxEngineContext>
    where T: class, IWorldElement, new()
{
    public readonly ObjectInput<UserRef> UserRef;
    public readonly ObjectInput<T> Value;
    public readonly ObjectOutput<ReferenceUserOverride<T>.Override> Override;

    public Continuation Next;
    
    protected override IOperation Run(FrooxEngineContext context)
    {
        ReferenceUserOverride<T>.Override newOverride = new ReferenceUserOverride<T>.Override();
        UserRef user = UserRef.Evaluate(context,new UserRef());
        string id = user.LinkedCloudId ?? user.LinkedMachineId;
        newOverride.User.SetFromIdAuto(id);
        newOverride.Value.Target = Value.Evaluate(context);
        Override.Write(newOverride,context);
        return Next.Target;
    }
    
    public NewReferenceOverride()
    {
        Override = new ObjectOutput<ReferenceUserOverride<T>.Override>(this);
    }
}