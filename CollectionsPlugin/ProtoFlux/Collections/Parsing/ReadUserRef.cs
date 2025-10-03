using FrooxEngine;
using FrooxEngine.ProtoFlux;
using ProtoFlux.Core;
using ProtoFlux.Runtimes.Execution;

namespace CollectionsPlugin.ProtoFlux.Collections.Parsing;

[NodeCategory("Collections/Parsing")]
[NodeName("Read UserRef")]
public class ReadUserRef : ObjectFunctionNode<FrooxEngineContext,User>
{
    public readonly ObjectInput<UserRef> UserRef;

    protected override User Compute(FrooxEngineContext context)
    {
        UserRef userRef = UserRef.Evaluate(context);
        if (userRef == null || userRef.Target == null)
        {
            return null;
        }

        return userRef.Target;
    }
}