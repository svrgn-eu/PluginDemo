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

            foreach (IPluginHost pluginHost in hostProvider.Plugins)
            {
                Console.WriteLine(pluginHost.Plugin.SayHello());
            }
        }
    }
}