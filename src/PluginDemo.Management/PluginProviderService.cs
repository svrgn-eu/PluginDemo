using PluginDemo.Attributes;
using PluginDemo.Helpers;
using PluginDemo.Implementations.Base;
using PluginDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;

namespace PluginDemo.Management
{
    public class PluginProviderService : IPluginProviderService
    {
        #region Properties

        public List<IPluginTypeReference> Plugins { get; private set; }

        public List<IPluginConfiguration> Configurations { get; private set; }

        public Dictionary<string, IPlugin> Instances { get; private set; }

        #endregion Properties

        #region Construction

        public PluginProviderService() 
        {
            this.Plugins = new List<IPluginTypeReference>();
            this.Configurations = new List<IPluginConfiguration>();
            this.Instances = new Dictionary<string, IPlugin>();
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
            this.Plugins = GetPlugins(plugInAssemblies);
            
            //TODO: load config (also save it somewhere)
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

        #region GetPlugins: returns a list of IPlugin Types in the passed assemblies
        /// <summary>
        /// returns a list of IPlugin Types in the passed assemblies
        /// </summary>
        /// <param name="assemblies">the assemblies with IPlugin child Types</param>
        /// <returns>a list of IPlugin Types</returns>
        private List<IPluginTypeReference> GetPlugins(List<Assembly> assemblies)
        {
            List<IPluginTypeReference> result = default;
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
                result = new List<IPluginTypeReference>();
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

                    //IPlugin resultPart = Activator.CreateInstance(pluginType) as IPlugin;
                    IPluginTypeReference resultPart = new PluginTypeReference(pluginType, metaData);
                    //resultPart.SetMetaData(metaData);

                    result.Add(resultPart);
                }
            }

            return result;
        }
        #endregion GetPlugins

        #region AddInstance
        public bool AddInstance(string InstanceName, IPluginIdentifier Identifier, List<IPluginSetting> Settings = null)
        {
            bool result = false;

            if (!this.Exists(InstanceName))
            {
                IPluginTypeReference source = this.Plugins.Where(x => x.MetaData.Identifier.Equals(Identifier)).FirstOrDefault();
                if (source != null)
                {
                    IPlugin newInstance = Activator.CreateInstance(source.PluginType, Settings) as IPlugin;  //TODO: make better
                    this.Instances.Add(InstanceName, newInstance);
                    this.AddConfiguration(Identifier, InstanceName, Settings);
                    result = true;
                }
                else
                { 
                    //TODO: error, plugin host not loaded
                }
            }
            else 
            { 
                //TODO: error, instance is already existing
            }

            return result;
        }
        #endregion AddInstance

        #region AddConfiguration
        private void AddConfiguration(IPluginIdentifier Identifier, string InstanceName, List<IPluginSetting> Settings)
        {
            IPluginConfiguration newConfig = new PluginConfiguration(InstanceName, Identifier);  //TODO: use DI
            if (Settings != null)
            {
                foreach (IPluginSetting setting in Settings)
                {
                    newConfig.AddSetting(setting);
                }
            }
            this.Configurations.Add(newConfig);
        }
        #endregion AddConfiguration

        #region Exists
        public bool Exists(string InstanceName)
        {
            bool result = false;

            result = this.Instances.ContainsKey(InstanceName);

            return result;
        }
        #endregion Exists

        #region GetInstance
        public IPlugin GetInstance(string InstanceName)
        {
            IPlugin result = default;

            if (this.Exists(InstanceName))
            {
                result = this.Instances[InstanceName];
            }

            return result;
        }
        #endregion GetInstance

        #region SetConfiguration: replaces the current configuration by the given one. Any instance of a Plugin will be discarded as consistency cannot be ensured.
        /// <summary>
        /// replaces the current configuration by the given one. Any instance of a Plugin will be discarded as consistency cannot be ensured.
        /// </summary>
        /// <param name="Configurations"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetConfiguration(List<IPluginConfiguration> Configurations)
        {
            //replace config
            this.Configurations.Clear();

            // throw away old instances
            this.Instances.Clear();

            // create new ones
            if (Configurations != null)
            {
                foreach (IPluginConfiguration config in Configurations)
                {
                    if (this.Plugins.Any(x => x.MetaData.Identifier.Equals(config.Identifier)))
                    {
                        IPluginTypeReference pluginTypeReference = this.Plugins.Where(x => x.MetaData.Identifier.Equals(config.Identifier)).FirstOrDefault();
                        if (pluginTypeReference != null)
                        {
                            IPlugin newInstance = Activator.CreateInstance(pluginTypeReference.PluginType, config.Settings) as IPlugin;  //TODO: make better
                            this.Instances.Add(config.InstanceName, newInstance);
                            this.Configurations.Add(config);
                        }
                    }
                    else
                    { 
                        //TODO: error handling: config is for a plugin which is not loaded / available
                    }
                }
            }
            else
            { 
                //TODO: notification that no configs have been given
            }
        }
        #endregion SetConfiguration

        #endregion Methods
    }
}
