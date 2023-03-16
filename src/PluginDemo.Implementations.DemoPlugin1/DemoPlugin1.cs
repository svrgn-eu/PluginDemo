using PluginDemo.Attributes;
using PluginDemo.Implementations.Base;
using PluginDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Implementations.DemoPlugin1
{
    [Author("Author1")]
    public class DemoPlugin1 : BasePlugin
    {
        #region Properties

        #endregion Properties

        #region Construction

        public DemoPlugin1(List<IPluginSetting> Settings)
            :base(Settings)
        {

        }

        #endregion Construction

        #region Methods

        #region SayHello
        public override string SayHello()
        {
            return "DemoPlugin1 yeets Hello!";
        }
        #endregion SayHello

        #endregion Methods
    }
}
