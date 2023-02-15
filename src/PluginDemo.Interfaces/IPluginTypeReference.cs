using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Interfaces
{
    public interface IPluginTypeReference
    {
        #region Properties

        Type PluginType { get; }
        IPluginMetaData MetaData { get; }

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
