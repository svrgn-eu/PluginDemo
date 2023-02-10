using PluginDemo.Implementations.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Implementations.DemoPlugin1
{
    public class DemoPlugin1 : BasePlugin
    {
        public override string SayHello()
        {
            return "DemoPlugin1 yeets Hello!";
        }
    }
}
