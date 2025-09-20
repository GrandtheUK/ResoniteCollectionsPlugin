# ResoniteCollectionsPlugin
A [Resonite](https://resonite.com/) Plugin that does something.

# Screenshots

# Installation
1. Place [CollectionsPlugin.dll](https://github.com/GrandtheUK/ResoniteCollectionsPlugin/releases/latest/download/CollectionsPlugin.dll) and [CollectionsPluginBindings.dll](https://github.com/GrandtheUK/ResoniteCollectionsPlugin/releases/latest/download/CollectionsPluginBindings.dll) into your `Libraries` folder. This folder should be located at `C:\Program Files (x86)\Steam\steamapps\common\Resonite\Libraries` by default, create it if it's missing. If Resonite is installed elsewhere, put the Libraries folder there.
2. Add `-LoadAssembly Libraries/CollectionsPlugin.dll -LoadAssembly Libraries/CollectionsPluginBindings.dll` to the launch arguments of Resonite
3. Check the logs to make sure the plugin is loaded. Look for the string `Loaded Extra Assembly:` to see what plugins have been loaded

# Building
Run `dotnet build` in the solution root. If your Resonite install is not in the default location provide the Resonite path with the argument `/p:ResonitePath=/path/to/resonite/install`

# Using this template

When using this template make sure to update the the solution file, and 2 project files to use your updated plugin and binding plugin names. If renaming the project ensure to clear all binary and object files from the main plugin and the existing bindings before rebuilding. There is a target for cleaning the bindings up that triggers after `dotnet clean`.

Bindings are currently in the gitignore file since they should be regenerated whenever FrooxEngine dlls are updated to ensure compatibility with any changes which can be done at build time instead of being committed to the repository. They may also (currently) contain bindings for official ProtoFlux nodes because of how the official bindings generator works so may not be a good idea to commit these to your plugin repository to keep it clean of official code.