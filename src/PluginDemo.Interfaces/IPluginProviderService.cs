using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Interfaces
{
    public interface IPluginHostProvider
    {
        #region Properties

        List<IPluginTypeReference> Plugins { get; }
        List<IPluginConfiguration> Configurations { get; }
        Dictionary<string, IPlugin> Instances { get; }

        #endregion Properties

        #region Methods

        void Reload();
        bool AddInstance(string InstanceName, IPluginIdentifier Identifier);
        bool Exists(string InstanceName);
        IPlugin GetInstance(string InstanceName);

        #endregion Methods
    }
}
