﻿using PluginDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Implementations.Base
{
    public abstract class BasePlugin : IPlugin
    {
        #region Properties

        #endregion Properties

        #region Construction

        public BasePlugin(List<IPluginSetting> Settings) 
        {
            var x = Settings;
        }

        #endregion Construction

        #region Methods

        public abstract string SayHello();

        #endregion Methods
    }
}
