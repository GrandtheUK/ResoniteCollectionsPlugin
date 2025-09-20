using FrooxEngine;
using Elements.Core;

namespace ExamplePlugin.Components.PluginTemplate
{
    [Category("PluginTemplate")]
    public class MyComponent : Component
    {
        public readonly Sync<bool> WeirdToggle;

        protected override void OnCommonUpdate()
        {
            base.OnCommonUpdate();
            if (WeirdToggle.Value == false)
            {
                RunInSeconds(2, (() => WeirdToggle.Value = true));
            }
        }

        public MyComponent()
        {
            WeirdToggle = new Sync<bool>();
        }
    }
}

