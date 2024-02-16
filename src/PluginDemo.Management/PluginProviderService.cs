using PluginDemo.Attributes;
using PluginDemo.Helpers;
using PluginDemo.Implementations.Base;
using PluginDemo.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Text;

namespace PluginDemo.Management
{
    public class PluginProviderService : IPluginProviderService
    {
        #region Properties

        public List<IPluginTypeReference> Plugins { get; private set; }

        public List<IPluginConfiguration> Configurations { get; private set; }

        public Dictionary<string, IPlugin> Instances { get; private set; }

        private List<PluginContextInfo> pluginContextInfos;
        #endregion Properties

        #region Construction

        public PluginProviderService() 
        {
            this.pluginContextInfos = new List<PluginContextInfo>();
            this.Plugins = new List<IPluginTypeReference>();
            this.Configurations = new List<IPluginConfiguration>();
            this.Instances = new Dictionary<string, IPlugin>();

            this.Reload();  // load all the plugins in the specified sub-directory
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

            this.pluginContextInfos = this.CreateLoadContexts();
            this.ScanForCorrectPluginAssemlyNames(this.pluginContextInfos);
            this.LoadPlugInAssemblies(this.pluginContextInfos);
            this.Plugins = this.GetPlugins(this.pluginContextInfos);
            if (this.Plugins is null)
            {
                this.Plugins = new List<IPluginTypeReference>();  //re-create if no pluginContextInfos were submitted
            }

            //TODO: load config (also save it somewhere)
        }
        #endregion Reload

        #region Unload
        public void Unload()
        {
            //unload all contexts
            foreach (PluginContextInfo pluginContextInfo in this.pluginContextInfos)
            {
                pluginContextInfo.Context.Unload();
            }

            // clear all instance variables
            this.Plugins?.Clear();
            this.Configurations?.Clear();
            this.Instances?.Clear();
        }
        #endregion Unload

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

        #region CreateLoadContexts
        private List<PluginContextInfo> CreateLoadContexts()
        {
            List<PluginContextInfo> result = default;

            DirectoryInfo pluginPath = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Plugins"));
            string[] pluginDirectories = Directory.GetDirectories(pluginPath.FullName);

            if (pluginDirectories != null)
            {
                result = new List<PluginContextInfo>();
                foreach (string pluginDirectory in pluginDirectories)
                {
                    PluginAssemblyLoadContext newContext = new PluginAssemblyLoadContext("SomeName", pluginDirectory);
                    PluginContextInfo newInfo = new PluginContextInfo(newContext, pluginDirectory);
                    result.Add(newInfo);
                }
            }

            return result;
        }
        #endregion CreateLoadContexts

        #region ScanForCorrectPluginAssemlyNames: scan all DLLs and see, which types they do export. If it matches to what we are looking for, then load it
        /// <summary>
        /// scan all DLLs and see, which types they do export. If it matches to what we are looking for, then load it
        /// </summary>
        /// <param name="contexts">a list of PluginContextIngo objects to use</param>
        /// <returns>true, when the contexts where not null</returns>
        private bool ScanForCorrectPluginAssemlyNames(List<PluginContextInfo> contexts)
        {
            //scan all DLLs and see, which types they do export. If it matches to what we are looking for, then load it
            bool result = false;

            string interfaceName = "IPlugin";  //TODO: set centrally / build reference for different INterface Types for different types of plugins

            if (contexts != null)
            {
                result = true;
                foreach (PluginContextInfo context in contexts)
                {
                    string[] dllFiles = Directory.GetFiles(context.Path, "*.dll");

                    if (dllFiles != null && dllFiles.Length > 0)
                    {
                        foreach (string potentialPlugin in dllFiles)
                        {
                            if (!context.Context.IsUnloading)
                            {
                                Assembly assembly = context.Context.LoadFromAssemblyPath(potentialPlugin);
                                Type[] types = assembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.Name == interfaceName)).ToArray();
                                if (types != null && types.Length.Equals(1))
                                {
                                    //TODO: do not use the base assembly!

                                    //workaround
                                    if (assembly.FullName.ToLower().Contains("base"))
                                    {
                                        continue;
                                    }

                                    //TODO: set this in the context object
                                    context.SetAssemblyName(assembly.GetName());

                                }
                                else if (types != null && types.Length > 1)
                                {
                                    // TODO: type was implemented more than once in that dll, eventual problem, needs to be handled
                                }
                                else
                                {
                                    // interface was just not implemented in that dll, potentially a dependency only.
                                }
                            }
                            else
                            { 
                                //TODO: error / Warning that the context is unloading at the very moment
                            }
                        }
                    }
                }
            }

            return result;
        }
        #endregion ScanForCorrectPluginAssemlyNames

        #region LoadPlugInAssemblies
        private void LoadPlugInAssemblies(List<PluginContextInfo> contexts)
        {
            if (contexts != null)
            {
                foreach (PluginContextInfo context in contexts)
                {
                    if (context.AssemblyName != null)
                    {
                        Assembly pluginAssembly = context.Context.LoadFromAssemblyName(context.AssemblyName);
                        context.SetAssembly(pluginAssembly);
                        context.SetPluginType(pluginAssembly.GetExportedTypes()[0]);  //TODO: think more, make more robust, select only implementors of IPlugin
                    }
                    else
                    { 
                        //TODO: error handling, no plugin dll in the specified dir of that context
                    }
                }
            }
        }
        #endregion LoadPlugInAssemblies

        #region GetPlugins: returns a list of IPlugin Types in the passed assemblies
        /// <summary>
        /// returns a list of IPlugin Types in the passed assemblies
        /// </summary>
        /// <param name="contextInfos">the assemblies with IPlugin child Types</param>
        /// <returns>a list of IPlugin Types</returns>
        private List<IPluginTypeReference> GetPlugins(List<PluginContextInfo> contextInfos)
        {
            List<IPluginTypeReference> result = default;
            List<Type> availableTypes = new List<Type>();

            foreach (PluginContextInfo currentContextInfo in contextInfos)
            {
                try
                {
                    if (currentContextInfo.Assembly != null)
                    {
                        availableTypes.AddRange(currentContextInfo.Assembly.GetTypes());
                    }
                    else
                    {
                        availableTypes.Clear();
                    }
                }
                catch (Exception ex)
                {
                    //TODO: error
                }
            }

            if (availableTypes.Count > 0)
            {
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
                        Type authorattributeType = default;
                        //get author attribute if available
                        foreach (PluginContextInfo pluginContextInfo in this.pluginContextInfos)
                        {
                            if (pluginContextInfo.PluginType.Equals(pluginType))
                            {
                                string nameOfAuthorAttribute = "PluginDemo.Attributes.AuthorAttribute";
                                Assembly authorAttributeAssembly = pluginContextInfo.Context.Assemblies.Where(x => x.GetType(nameOfAuthorAttribute, true) != null).FirstOrDefault();
                                authorattributeType = authorAttributeAssembly.GetType(nameOfAuthorAttribute);
                            }
                        }

                        object[] authorAttributes = pluginType.GetCustomAttributes(authorattributeType, true);
                        string authorName = "anonymous";
                        if (authorAttributes != null && authorAttributes.Length.Equals(1))
                        {
                            var authorAttribute = authorAttributes[0];
                            authorName = (string)authorattributeType.GetProperty("Name").GetValue(authorAttribute);
                        }
                        string assemblyFullname = pluginType.Assembly.FullName;


                        IPluginMetaData metaData = PluginMetaDataHelper.ExtractMetadata(authorName, assemblyFullname);
                        IPluginTypeReference resultPart = new PluginTypeReference(pluginType, metaData);
                        result.Add(resultPart);
                    }
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
                if (this.Plugins != null)
                {
                    IPluginTypeReference source = this.Plugins.Where(x => x.MetaData.Identifier.Equals(Identifier)).FirstOrDefault();
                    if (source != null)
                    {
                        object[] parameters = null;
                        IPlugin newInstance = source.PluginType.Assembly.CreateInstance(source.PluginType.FullName, false, BindingFlags.Default, null, parameters, CultureInfo.InvariantCulture, null) as IPlugin;
                        //IPlugin newInstance = source.PluginType.Assembly.CreateInstance(source.PluginType.FullName, false, BindingFlags.Default, null, parameters, CultureInfo.InvariantCulture, null) as IPlugin;
                        newInstance.Initialize(Settings);
                        this.Instances.Add(InstanceName, newInstance);
                        this.AddConfiguration(Identifier, InstanceName, Settings);
                        result = true;
                    }
                    else
                    { 
                        //TODO: source is null, instance type not listed in plugins
                    }
                }
                else
                {
                    //TODO: error, no plugins loaded
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
                            IPlugin newInstance = Activator.CreateInstance(pluginTypeReference.PluginType) as IPlugin;  //TODO: make better
                            newInstance.Initialize(config.Settings);
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
