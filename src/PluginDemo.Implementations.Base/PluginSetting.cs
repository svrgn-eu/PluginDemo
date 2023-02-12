using PluginDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Implementations.Base
{
    public class PluginSetting : IPluginSetting
    {
        #region Properties

        public string Name { get; private set; }

        public Type Type { get; private set; }

        public object Value { get; private set; }

        #endregion Properties

        #region Construction

        public PluginSetting(string Name, object Value)
        {
            this.Name = Name;
            this.Type = Value.GetType();
            this.Value = Value;
        }

        #endregion Construction

        #region Methods

        #endregion Methods

        #region Events

        #endregion Events
    }
}
