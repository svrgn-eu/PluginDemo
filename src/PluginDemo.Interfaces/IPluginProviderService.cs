using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Interfaces
{
    public interface IPluginProviderService
    {
        #region Properties

        List<IPluginTypeReference> Plugins { get; }
        List<IPluginConfiguration> Configurations { get; }
        Dictionary<string, IPlugin> Instances { get; }

        #endregion Properties

        #region Methods

        void Reload();
        bool AddInstance(string InstanceName, IPluginIdentifier Identifier, List<IPluginSetting> Settings = null);
        bool Exists(string InstanceName);
        IPlugin GetInstance(string InstanceName);
        void SetConfiguration(List<IPluginConfiguration> Configurations);

        #endregion Methods
    }
}
