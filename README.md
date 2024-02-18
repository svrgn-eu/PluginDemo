[TOC]

# About

This demo project is designated to show how to implement a flexible Plugin System in C#. It uses the theory behind https://discoverdot.net/projects/dotnetcore-plugins and https://learn.microsoft.com/de-de/dotnet/core/tutorials/creating-app-with-plugin-support and tries to solve some of the detailed blockers for productive or expected use.

Basically, the demo project and implementation builds a base of available plugins by scanning the subfolder "Plugins". Each Plugin is contained in an own subfolder, which enables various advantages:

- use of different versions of a plugin is supported
- each plugin brings it's own dependencies

Those two, quite simple achievements are key to not being in "dependency hell", although it sounds like a cozy place, it isn't. 



## Out of scope

Due to the demo character, the following features are out of scope:

- Plugin unloading while runtime
- Plugin change or reloading while runtime

Those two features can be enabled by using a file system watcher and some reload logic though, should not be far from here.

# How to use

In the main program, you can see the straightforward use case;

```csharp
IPluginProviderService hostProvider = new PluginProviderService();  // create a new instance of the Plugin Host Provider Service

// create instances of existing types in "./Plugins/*"
hostProvider.AddInstance("DemoPlugin1Instance1", new PluginIdentifier("PluginDemo.Implementations.DemoPlugin1", Version.Parse("0.1.0.0")), new List<IPluginSetting>() { new PluginSetting("", "") });
hostProvider.AddInstance("DemoPlugin1Instance1-1", new PluginIdentifier("PluginDemo.Implementations.DemoPlugin1", Version.Parse("0.1.0.0")), new List<IPluginSetting>() { new PluginSetting("", "") });
hostProvider.AddInstance("DemoPlugin1Instance2", new PluginIdentifier("PluginDemo.Implementations.DemoPlugin2", Version.Parse("0.1.0.0")), new List<IPluginSetting>() { new PluginSetting("", "") });

//TODO: use dependency injection
//TODO: call hostProvider.Reload() when something has changed in the Plugin (sub)folders, file system watcher

foreach (IPlugin plugin in hostProvider.Instances.Values)  // iterate through all instances
{
    Console.WriteLine(plugin.SayHello());  // actual Plugin Method / Reference
}
```

For the useage, we need an IPluginProviderService instance which serves as access to the plugin functions, it will build up a type reference while being constructed. 

After that, we basically create instances of the known plugin types. 

In the end, we can iterate through all the instances and call their methods, defined in the IPlugin interface. For a more sophisticated plugin, of course a or several more specialized interface(s) is needed.

