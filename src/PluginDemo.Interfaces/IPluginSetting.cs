using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Interfaces
{
    public interface IPluginSetting
    {
        #region Properties

        string Name { get; }
        Type Type { get; }
        object Value { get; }

        #endregion Properties
    }
}
