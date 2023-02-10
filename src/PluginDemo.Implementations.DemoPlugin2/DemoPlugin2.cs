using PluginDemo.Implementations.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Implementations.DemoPlugin2
{
    public class DemoPlugin2 : BasePlugin
    {
        public override string SayHello()
        {
            return "DemoPlugin2 yeets Hello!";
        }
    }
}
