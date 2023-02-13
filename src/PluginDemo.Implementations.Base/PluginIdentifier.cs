using PluginDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PluginDemo.Implementations.Base
{
    public class PluginIdentifier : IPluginIdentifier
    {
        #region Properties

        public string Name { get; private set; }

        public Version Version { get; private set; }

        #endregion Properties

        #region Construction

        public PluginIdentifier(string Name, Version Version)
        { 
            this.Name = Name;
            this.Version = Version;
        }

        #endregion Construction
    }
}
