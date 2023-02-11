using PluginDemo.Interfaces;
using PluginDemo.Management;
using System;

namespace PluginDemo.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPluginHostProvider hostProvider = new PluginHostProvider();
            //TODO: use dependency injection
            //TODO: call hostProvider.Reload() when something has changed in the Plugin (sub)folders, file system watcher

            foreach (IPluginHost pluginHost in hostProvider.Plugins)
            {
                Console.WriteLine(pluginHost.Plugin.SayHello());
            }
        }
    }
}