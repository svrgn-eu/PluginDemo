using PluginDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Management
{
    public class PluginTypeReference : IPluginTypeReference
    {
        #region Properties

        public Type PluginType { get; private set; }
        public IPluginMetaData MetaData { get; private set; }

        #endregion Properties

        #region Construction

        public PluginTypeReference(Type PluginType, IPluginMetaData MetaData) 
        { 
            this.PluginType = PluginType;
            this.MetaData = MetaData;
        }

        #endregion Construction

        #region Methods

        #endregion Methods
    }
}
