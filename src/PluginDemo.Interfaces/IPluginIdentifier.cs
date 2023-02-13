using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Interfaces
{
    public interface IPluginIdentifier
    {
        #region Properties

        string Name { get; }
        Version Version { get; }

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
