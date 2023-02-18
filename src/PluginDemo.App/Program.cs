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
            IPluginProviderService hostProvider = new PluginProviderService();
            hostProvider.AddInstance("DemoPlugin1Instance1", new PluginIdentifier("PluginDemo.Implementations.DemoPlugin1", Version.Parse("0.1.0.0")));
            hostProvider.AddInstance("DemoPlugin1Instance1-1", new PluginIdentifier("PluginDemo.Implementations.DemoPlugin1", Version.Parse("0.1.0.0")));
            hostProvider.AddInstance("DemoPlugin1Instance2", new PluginIdentifier("PluginDemo.Implementations.DemoPlugin2", Version.Parse("0.1.0.0")));
            //TODO: use dependency injection
            //TODO: call hostProvider.Reload() when something has changed in the Plugin (sub)folders, file system watcher

            foreach (IPlugin plugin in hostProvider.Instances.Values)
            {
                Console.WriteLine(plugin.SayHello());
            }
        }
    }
}