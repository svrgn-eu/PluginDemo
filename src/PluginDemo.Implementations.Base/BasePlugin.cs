using PluginDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Implementations.Base
{
    public abstract class BasePlugin : IPlugin
    {
        #region Properties

        #endregion Properties

        #region Methods

        public abstract string SayHello();

        #endregion Methods

    }
}
