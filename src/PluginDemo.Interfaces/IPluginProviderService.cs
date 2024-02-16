using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Interfaces
{
    public interface IPluginProviderService
    {
        #region Properties

        #region Plugins: Available Plugin Types/Files which are ready to be instantiated
        /// <summary>
        /// Available Plugin Types/Files which are ready to be instantiated
        /// </summary>
        List<IPluginTypeReference> Plugins { get; }
        #endregion Plugins

        List<IPluginConfiguration> Configurations { get; }

        #region Instances: Named Plugin Type Instances
        /// <summary>
        /// Named Plugin Type Instances
        /// </summary>
        Dictionary<string, IPlugin> Instances { get; }
        #endregion Instances

        #endregion Properties

        #region Methods

        void Reload();
        void Unload();
        bool AddInstance(string InstanceName, IPluginIdentifier Identifier, List<IPluginSetting> Settings = null);
        bool Exists(string InstanceName);
        IPlugin GetInstance(string InstanceName);
        void SetConfiguration(List<IPluginConfiguration> Configurations);

        #endregion Methods
    }
}
