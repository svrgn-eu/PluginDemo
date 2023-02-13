﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Interfaces
{
    public interface IPluginHostProvider
    {
        #region Properties

        List<IPluginHost> Plugins { get; }
        List<IPluginConfiguration> Configurations { get; }

        #endregion Properties

        #region Methods

        void Reload();

        #endregion Methods
    }
}
