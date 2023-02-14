using PluginDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Implementations.Base
{
    public abstract class BasePlugin : IPlugin
    {
        #region Properties

        public IPluginMetaData MetaData { get; private set; }

        #endregion Properties

        #region Methods

        public abstract string SayHello();

        #region SetMetaData
        public void SetMetaData(IPluginMetaData Data)
        {
            this.MetaData = Data;
        }
        #endregion SetMetaData

        #endregion Methods
    }
}
