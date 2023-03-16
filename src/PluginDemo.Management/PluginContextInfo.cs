using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PluginDemo.Management
{
    internal class PluginContextInfo
    {
        #region Properties

        public PluginAssemblyLoadContext Context { get; }
        public string Path { get; }

        public AssemblyName AssemblyName { get; private set; }

        #endregion Properties

        #region Construction

        public PluginContextInfo(PluginAssemblyLoadContext Context, string Path)
        { 
            this.Context = Context;
            this.Path = Path;
        }

        #endregion Construction

        #region Methods

        #region SetAssemblyName
        public void SetAssemblyName(AssemblyName NewValue)
        { 
            this.AssemblyName = NewValue;
        }
        #endregion SetAssemblyName

        #endregion Methods
    }
}
