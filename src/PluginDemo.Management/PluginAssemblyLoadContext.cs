using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace PluginDemo.Management
{
    /// <summary>
    /// this class provides a specified context for loading plugins. It is mainly corresponding to the class used in https://learn.microsoft.com/de-de/dotnet/core/tutorials/creating-app-with-plugin-support
    /// </summary>
    internal class PluginAssemblyLoadContext : AssemblyLoadContext
    {
        #region Properties

        public bool IsUnloading { get; private set; } = false;

        private AssemblyDependencyResolver resolver;

        #endregion Properties

        #region Construction

        public PluginAssemblyLoadContext(string Name, string PluginPath)
            : base(Name, true)
        { 
            this.resolver = new AssemblyDependencyResolver(PluginPath);
            this.Unloading += PluginAssemblyLoadContext_Unloading;
        }
        #endregion Construction

        #region Methods

        #region Load
        protected override Assembly Load(AssemblyName assemblyName)
        {
            Assembly result = default;

            string assemblyPath = this.resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                result = LoadFromAssemblyPath(assemblyPath);
            }

            return result;
        }
        #endregion Load

        #region LoadUnmanagedDll
        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = this.resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }

            return IntPtr.Zero;
        }
        #endregion LoadUnmanagedDll

        #region PluginAssemblyLoadContext_Unloading
        private void PluginAssemblyLoadContext_Unloading(AssemblyLoadContext obj)
        {
            this.IsUnloading = true;
        }
        #endregion PluginAssemblyLoadContext_Unloading

        #endregion Methods
    }
}
