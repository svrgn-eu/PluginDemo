using PluginDemo.Attributes;
using PluginDemo.Implementations.Base;
using PluginDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Implementations.DemoPlugin2
{
    [Author("Author2")]
    public class DemoPlugin2 : BasePlugin
    {
        #region Properties

        #endregion Properties

        #region Construction

        public DemoPlugin2(List<IPluginSetting> Settings)
            : base(Settings)
        {
            var x = Settings;
        }

        #endregion Construction

        #region Methods

        #region SayHello
        public override string SayHello()
        {
            return "DemoPlugin2 yeets Hello!";
        }
        #endregion SayHello

        #endregion Methods
    }
}
