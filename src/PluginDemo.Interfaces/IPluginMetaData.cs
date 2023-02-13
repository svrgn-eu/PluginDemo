using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Interfaces
{
    public interface IPluginMetaData
    {
        #region Properties

        IPluginIdentifier Identifier { get; }
        string Author { get; }

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
