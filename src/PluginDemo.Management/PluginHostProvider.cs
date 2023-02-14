using PluginDemo.Attributes;
using PluginDemo.Helpers;
using PluginDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;

namespace PluginDemo.Management
{
    public class PluginHostProvider : IPluginHostProvider
    {
        #region Properties

        public List<IPluginHost> Plugins { get; private set; }

        public List<IPluginConfiguration> Configurations { get; private set; }

        #endregion Properties

        #region Construction

        public PluginHostProvider() 
        {
            this.Plugins = new List<IPluginHost>();
            this.Configurations = new List<IPluginConfiguration>();
            this.Reload();
        }

        #endregion Construction

        #region Methods

        #region Reload: reloads the plugin list from the file system
        /// <summary>
        /// reloads the plugin list from the file system
        /// </summary>
        public void Reload()
        {
            this.Plugins.Clear();

            this.LoadPlugInDependencies("PluginDemo.Implementations.Base.dll", "PluginDemo.Interfaces.dll", "PluginDemo.Attributes.dll");  //load two dependencies before being able to load the plugins themselves. TODO: specify more detailed and have a dedicated folder(?)
            List<Assembly> plugInAssemblies = this.LoadPlugInAssemblies();
            List<IPlugin> plugIns = GetPlugins(plugInAssemblies);

            foreach (IPlugin plugin in plugIns)
            {
                this.Plugins.Add(new PluginHost(plugin));
            }
        }
        #endregion Reload

        #region LoadPlugInDependencies: pre-loads the dependencies of the plugins (if not loaded anyway) to avoid any missing files when loading the plugin
        /// <summary>
        /// pre-loads the dependencies of the plugins (if not loaded anyway) to avoid any missing files when loading the plugin
        /// </summary>
        /// <param name="AssemblyNames">name of assemblies to load, relative to program executable path</param>
        private void LoadPlugInDependencies(params string[] AssemblyNames)
        {
            string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            List<Assembly> dependencies = new List<Assembly>();
            foreach (string dependencyPath in AssemblyNames)
            {
                string fullPath = Path.Combine(currentDirectory, dependencyPath);
                dependencies.Add(Assembly.LoadFrom(fullPath));
            }
        }
        #endregion LoadPlugInDependencies

        #region LoadPlugInAssemblies
        private List<Assembly> LoadPlugInAssemblies()
        {
            List<Assembly> result = new List<Assembly>();

            DirectoryInfo dInfo = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Plugins"));
            FileInfo[] files = dInfo.GetFiles("*.dll", SearchOption.AllDirectories);

            if (null != files)
            {
                foreach (FileInfo file in files)
                {
                    result.Add(Assembly.LoadFile(file.FullName));
                }
            }

            return result;
        }
        #endregion LoadPlugInAssemblies

        #region GetPlugins: returns a list of IPlugin instances created from the types of the passed assemblies
        /// <summary>
        /// returns a list of IPlugin instances created from the types of the passed assemblies
        /// </summary>
        /// <param name="assemblies">the assemblies with IPlugin child Types</param>
        /// <returns>a list of IPlugin instances</returns>
        private List<IPlugin> GetPlugins(List<Assembly> assemblies)
        {
            List<IPlugin> result = default;
            List<Type> availableTypes = new List<Type>();

            foreach (Assembly currentAssembly in assemblies)
            {
                availableTypes.AddRange(currentAssembly.GetTypes());
            }

            // get a list of objects that implement the IPlugin interface
            List<Type> pluginList = availableTypes.FindAll(delegate (Type t)
            {
                List<Type> interfaceTypes = new List<Type>(t.GetInterfaces());
                return interfaceTypes.Contains(typeof(IPlugin));
            });

            if (pluginList != null && pluginList.Count > 0)
            {
                result = new List<IPlugin>();
                //get metadata
                foreach (Type pluginType in pluginList)
                {
                    //get author attribute if available
                    object[] authorAttributes = pluginType.GetCustomAttributes(typeof(AuthorAttribute), true);
                    AuthorAttribute authorAttribute = null;
                    if (authorAttributes != null && authorAttributes.Length.Equals(1))
                    {
                        authorAttribute = (AuthorAttribute)authorAttributes[0];
                    }
                    string assemblyFullname = pluginType.Assembly.FullName;
                    string authorName;
                    if (authorAttribute != null)
                    {
                        authorName = authorAttribute.Name;
                    }
                    else
                    {
                        authorName = "anonymous";
                    }

                    IPluginMetaData metaData = PluginMetaDataHelper.ExtractMetadata(authorName, assemblyFullname);

                    IPlugin resultPart = Activator.CreateInstance(pluginType) as IPlugin;
                    resultPart.SetMetaData(metaData);

                    result.Add(resultPart);
                }
            }

            return result;
        }
        #endregion GetPlugins

        #endregion Methods
    }
}
