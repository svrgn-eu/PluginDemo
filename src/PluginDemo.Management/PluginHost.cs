using PluginDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Management
{
    public class PluginHost : IPluginHost
    {
        #region Properties

        public IPlugin Plugin { get; private set; }

        #endregion Properties

        #region Construction

        public PluginHost(IPlugin Plugin) 
        { 
            this.Plugin = Plugin;
        }

        #endregion Construction

        #region Methods

        #endregion Methods
    }
}
