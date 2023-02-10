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

        #endregion Properties

        #region Construction

        public PluginHostProvider() 
        {
            this.Plugins = new List<IPluginHost>();
            this.Reload();
        }

        #endregion Construction

        #region Methods

        #region Reload
        public void Reload()
        {
            this.Plugins.Clear();

            this.LoadPlugInDependencies();  //load two dependencies before being able to load the plugins themselves. TODO: specify more detailed and have a dedicated folder(?)
            List<Assembly> plugInAssemblies = this.LoadPlugInAssemblies();
            List<IPlugin> plugIns = GetPlugIns(plugInAssemblies);

            foreach (IPlugin plugin in plugIns)
            {
                this.Plugins.Add(new PluginHost(plugin));
            }
        }
        #endregion Reload

        #region LoadPlugInAssemblies
        private List<Assembly> LoadPlugInAssemblies()
        {
            List<Assembly> result = new List<Assembly>();

            DirectoryInfo dInfo = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Plugins"));
            FileInfo[] files = dInfo.GetFiles("*.dll");  //TODO: make recursive / subfolder-searching

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

        #region LoadPlugInDependencies
        private void LoadPlugInDependencies()
        {
            string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string[] dependencyPaths = new string[] { "PluginDemo.Implementations.Base.dll", "PluginDemo.Interfaces.dll" };

            List<Assembly> dependencies = new List<Assembly>();
            foreach (string dependencyPath in dependencyPaths)
            {
                string fullPath = Path.Combine(currentDirectory, dependencyPath);
                dependencies.Add(Assembly.LoadFrom(fullPath));
            }
        }
        #endregion LoadPlugInDependencies

        #region GetPlugIns
        private List<IPlugin> GetPlugIns(List<Assembly> assemblies)
        {
            List<Type> availableTypes = new List<Type>();

            foreach (Assembly currentAssembly in assemblies)
            {
                availableTypes.AddRange(currentAssembly.GetTypes());
            }
            // get a list of objects that implement the ICalculator interface AND 
            // have the CalculationPlugInAttribute
            List<Type> pluginList = availableTypes.FindAll(delegate (Type t)
            {
                List<Type> interfaceTypes = new List<Type>(t.GetInterfaces());
                //object[] arr = t.GetCustomAttributes(typeof(CalculationPlugInAttribute), true);
                //return !(arr == null || arr.Length == 0) && interfaceTypes.Contains(typeof(IPlugin));
                return interfaceTypes.Contains(typeof(IPlugin));  //20230210 no attribute used (yet)
            });

            // convert the list of Objects to an instantiated list of ICalculators
            return pluginList.ConvertAll<IPlugin>(delegate (Type t) { return Activator.CreateInstance(t) as IPlugin; });  //todo. use different method for DI
        }
        #endregion GetPlugIns

        #endregion Methods
    }
}
