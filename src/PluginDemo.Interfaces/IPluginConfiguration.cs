using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Interfaces
{
    public interface IPluginConfiguration
    {
        #region Properties

        IPluginIdentifier Identifier { get; }
        string InstanceName { get; }
        List<IPluginSetting> Settings { get; }

        #endregion Properties

        #region Methods

        void AddSetting(IPluginSetting Setting);

        #endregion Methods
    }
}
