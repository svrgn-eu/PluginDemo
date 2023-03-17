using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Interfaces
{
    public interface IPlugin
    {
        #region Properties

        #endregion Properties

        #region Methods

        void Initialize(List<IPluginSetting> Settings);

        string SayHello();

        #endregion Methods
    }
}
