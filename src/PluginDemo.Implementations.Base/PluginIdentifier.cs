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

        #region Methods

        #region Equals
        public override bool Equals(object obj)
        {
            bool result = false;

            if (obj is IPluginIdentifier) 
            {
                IPluginIdentifier comparedObject = (IPluginIdentifier)obj;
                result = (this.Name.Equals(comparedObject.Name)) && (this.Version.Equals(comparedObject.Version));
            }
            return result;
        }
        #endregion Equals

        #endregion Methods
    }
}
