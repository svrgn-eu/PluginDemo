using PluginDemo.Implementations.Base;
using PluginDemo.Interfaces;
using PluginDemo.Management;
using System;

namespace PluginDemo.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
        }
    }
}