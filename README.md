# ResoniteCollectionsPlugin
A [Resonite](https://resonite.com/) Plugin that adds ProtoFlux Collections Support

You may wish to use a mod that exposes Reference Proxies for bags when using this plugin. Currently there are 2 available:
- (BepisLoader) [MoreReferenceProxies by EIA485](https://github.com/EIA485/NeosMoreReferenceProxies)
- (RML) [MoreReferenceProxies by Grand](https://github.com/GrandtheUK/MoreReferenceProxies) (a port of EIA485's BepisLoader mod to RML)

# Installation
1. Place [CollectionsPlugin.dll](https://github.com/GrandtheUK/ResoniteCollectionsPlugin/releases/latest/download/CollectionsPlugin.dll) into your `Libraries` folder. This folder should be located at `C:\Program Files (x86)\Steam\steamapps\common\Resonite\Libraries` by default, create it if it's missing. If Resonite is installed elsewhere, put the Libraries folder there.
2. Add `-LoadAssembly Libraries/CollectionsPlugin.dll` to the launch arguments of Resonite
3. Check the logs to make sure the plugin is loaded. Look for the string `Loaded Extra Assembly:` to see what plugins have been loaded

# Building
Run `dotnet build` in the solution root. If your Resonite install is not in the default location provide the Resonite path with the argument `/p:ResonitePath=/path/to/resonite/install`