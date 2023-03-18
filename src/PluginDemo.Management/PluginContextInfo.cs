using PluginDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PluginDemo.Management
{
    /// <summary>
    /// this class provides a collection of contextual objects around a Plugin. This is needed to ensure to have the right dependencies for the affected plugin dlls at hand, also being able to unload them.
    /// </summary>
    internal class PluginContextInfo
    {
        #region Properties

        public PluginAssemblyLoadContext Context { get; }
        public string Path { get; }
        public Assembly Assembly { get; private set; }

        public AssemblyName AssemblyName { get; private set; }
        public Type PluginType { get; private set; }

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

        #region SetAssembly
        public void SetAssembly(Assembly NewValue)
        {
            this.Assembly = NewValue;
        }
        #endregion SetAssembly

        #region SetPluginType
        public void SetPluginType(Type NewValue)
        { 
            this.PluginType = NewValue;
        }
        #endregion SetPluginType

        #endregion Methods
    }
}
