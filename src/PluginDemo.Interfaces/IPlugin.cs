using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Interfaces
{
    public interface IPlugin
    {
        //TODO: create child interfaces for specialized plugins which provide more info (standard set) and specialized functions / methods
        #region Properties

        #endregion Properties

        #region Methods

        void Initialize(List<IPluginSetting> Settings);

        string SayHello();

        #endregion Methods
    }
}
