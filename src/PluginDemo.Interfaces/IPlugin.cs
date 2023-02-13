using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Interfaces
{
    public interface IPlugin
    {
        #region Properties

        IPluginMetaData MetaData { get; }

        #endregion Properties

        #region Methods

        string SayHello();

        #endregion Methods
    }
}
