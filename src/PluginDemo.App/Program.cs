using PluginDemo.Interfaces;
using PluginDemo.Management;
using System;

namespace PluginDemo.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            IPluginHostProvider hostProvider = new PluginHostProvider();
        }
    }
}