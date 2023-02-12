using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Interfaces
{
    public interface IPluginHeader
    {
        #region Properties

        string Name { get; }
        Version Version { get; }
        string Author { get; }
        List<IPluginSetting> Settings { get; }

        #endregion Properties

        #region Methods

        void AddSetting(IPluginSetting Setting);

        #endregion Methods
    }
}
