using PluginDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Implementations.Base
{
    public abstract class BasePlugin : IPlugin
    {
        #region Properties

        protected List<IPluginSetting> settings;

        #endregion Properties

        #region Construction

        public BasePlugin()
        {
            
        }

        #endregion Construction

        #region Methods

        #region Initialize
        public virtual void Initialize(List<IPluginSetting> Settings)
        {
            this.settings = Settings;
        }
        #endregion Initialize

        public abstract string SayHello();

        #endregion Methods
    }
}
