using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace PluginDemo.Management
{
    internal class PluginAssemblyLoadContext : AssemblyLoadContext
    {
        #region Properties

        #endregion Properties

        #region Construction

        public PluginAssemblyLoadContext(string Name)
            : base(Name, true)
        { 
        
        }

        #endregion Construction

        #region Methods

        #region Load
        protected override Assembly Load(AssemblyName assemblyName)
        {
            return LoadFromAssemblyName(assemblyName);
        }
        #endregion Load

        #endregion Methods
    }
}
