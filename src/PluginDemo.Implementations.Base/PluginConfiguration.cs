using PluginDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Implementations.Base
{
    public class PluginConfiguration : IPluginConfiguration
    {
        #region Properties

        public List<IPluginSetting> Settings { get; private set; }

        public IPluginIdentifier Identifier { get; private set; }

        public string InstanceName { get; private set; }

        #endregion Properties

        #region Construction

        public PluginConfiguration(string InstanceName, IPluginIdentifier Identifier)
        { 
            this.InstanceName = InstanceName;
            this.Identifier = Identifier;
            this.Settings = new List<IPluginSetting>(); 
        }

        #endregion Construction

        #region Methods

        #region AddSetting
        public void AddSetting(IPluginSetting Setting)
        {
            this.Settings.Add(Setting);
        }
        #endregion AddSetting

        #endregion Methods

        #region Events

        #endregion Events
    }
}
