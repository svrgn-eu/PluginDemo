using PluginDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Management
{
    public class PluginHost : IPluginHost
    {
        #region Properties

        private IPlugin plugin;

        #endregion Properties

        #region Construction

        public PluginHost(IPlugin Plugin) 
        { 
            this.plugin = Plugin;
        }

        #endregion Construction

        #region Methods

        #endregion Methods
    }
}
