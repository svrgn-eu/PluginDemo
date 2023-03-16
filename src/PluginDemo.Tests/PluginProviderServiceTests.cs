using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using PluginDemo.Common.Implementations.Windows;
using PluginDemo.Common.Interfaces.Windows;
using PluginDemo.Helpers.Serialization;
using PluginDemo.Implementations.Base;
using PluginDemo.Interfaces;
using PluginDemo.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginDemo.Tests
{
    [TestClass]
    public class PluginProviderServiceTests
    {
        #region Creation
        [TestMethod]
        public void Creation()
        {
            IPluginProviderService provider = new PluginProviderService();

            Assert.IsNotNull(provider);
            Assert.IsTrue(provider.Plugins.Count > 0);
            foreach (IPluginTypeReference pluginHost in provider.Plugins)
            {
                Assert.IsNotNull(pluginHost.PluginType);
                Assert.IsNotNull(pluginHost.MetaData);
                Assert.IsNotNull(pluginHost.MetaData.Identifier);
            }
        }
        #endregion Creation

        //TODO: add negative tests
        // -missing assembly info in the file
        // wildly named files
        // missing authorinfo

        #region AddInstance
        [TestMethod]
        public void AddInstance()
        {
            IPluginProviderService provider = new PluginProviderService();
            IPluginIdentifier identifier = new PluginIdentifier("PluginDemo.Implementations.DemoPlugin1", Version.Parse("0.1.0.0"));
            bool wasSuccessfullyAdded = provider.AddInstance("NewInstance01", identifier);
            bool doesInstanceExist = provider.Exists("NewInstance01");

            Assert.IsNotNull(provider);
            Assert.IsTrue(wasSuccessfullyAdded);
            Assert.IsTrue(doesInstanceExist);
        }
        #endregion AddInstance

        #region GetInstance
        [TestMethod]
        public void GetInstance()
        {
            IPluginProviderService provider = new PluginProviderService();
            IPluginIdentifier identifier = new PluginIdentifier("PluginDemo.Implementations.DemoPlugin1", Version.Parse("0.1.0.0"));
            bool wasSuccessfullyAdded = provider.AddInstance("NewInstance01", identifier);
            bool doesInstanceExist = provider.Exists("NewInstance01");
            IPlugin instance = provider.GetInstance("NewInstance01");

            Assert.IsNotNull(provider);
            Assert.IsTrue(wasSuccessfullyAdded);
            Assert.IsTrue(doesInstanceExist);
            Assert.IsNotNull(instance);
        }
        #endregion GetInstance

        #region AddPlugin
        [TestMethod]
        public void AddPlugin()
        {
            IPluginProviderService provider = new PluginProviderService();

            int pluginTypesBefore = provider.Plugins.Count;

            //File.Copy(srcFilename, destFilename);
            //Thread.Sleep(500);
            provider.Reload();
            int pluginTypesAfterCopy = provider.Plugins.Count;

            Assert.AreEqual(2, pluginTypesBefore);
            Assert.AreEqual(2, pluginTypesAfterCopy);
        }
        #endregion AddPlugin

        //TODO: what to do with plugins which are deleted while an active instance is created? Mark? - update: cannot be deleted as it is in access

        #region AddPluginWithFileWatcher
        [TestMethod]
        public void AddPluginWithFileWatcher()
        {
            string srcFilename = "AdditionalPlugins/PluginDemo.Implementations.DemoPlugin1-0.1.0-EXTRA.dll";
            string destFilename = "Plugins/PluginDemo.Implementations.DemoPlugin1-0.1.0-EXTRA.dll";

            IPluginProviderService provider = new PluginProviderService();
            IDirectoryChangedWatcherService watcher = new DirectoryChangedWatcherService();
            watcher.SetPath("Plugins");
            int lastNumberOfPlugins = 0;
            watcher.ContentChanged += (x, e) => { provider.Reload(); lastNumberOfPlugins = provider.Plugins.Count; };  //reload avalable plgins when something has changed, omit reloading later in the code

            int pluginTypesBefore = provider.Plugins.Count;

            //File.Copy(srcFilename, destFilename);
            //wait for watcher
            //Thread.Sleep(500);
            //int pluginTypesAfterCopy = lastNumberOfPlugins;
            int pluginTypesAfterCopy = provider.Plugins.Count;

            Assert.AreEqual(2, pluginTypesBefore);
            Assert.AreEqual(2, pluginTypesAfterCopy);
        }
        #endregion AddPluginWithFileWatcher

        #region ExportConfiguration
        [TestMethod]
        public void ExportConfiguration()
        {
            IPluginProviderService provider = new PluginProviderService();
            IPluginIdentifier identifier01 = new PluginIdentifier("PluginDemo.Implementations.DemoPlugin1", Version.Parse("0.1.0.0"));
            IPluginIdentifier identifier02 = new PluginIdentifier("PluginDemo.Implementations.DemoPlugin2", Version.Parse("0.1.0.0"));
            IPluginSetting setting01 = new PluginSetting("SomeSetting", 666);
            IPluginSetting setting02 = new PluginSetting("SomeOtherSetting", "String Text");

            bool wasSuccessfullyAdded01 = provider.AddInstance("NewInstance01", identifier01, new List<IPluginSetting>() { setting01, setting02 });
            bool wasSuccessfullyAdded02 = provider.AddInstance("NewInstance02", identifier02);
            bool doesInstance01Exist = provider.Exists("NewInstance01");
            bool doesInstance02Exist = provider.Exists("NewInstance02");


            List<IPluginConfiguration> exportedConfiguration = provider.Configurations;

            string configurationJson = SerializerHelper.Serialize(exportedConfiguration);

            Assert.IsNotNull(provider);
            Assert.IsTrue(wasSuccessfullyAdded01);
            Assert.IsTrue(wasSuccessfullyAdded02);
            Assert.IsTrue(doesInstance01Exist);
            Assert.IsTrue(doesInstance02Exist);
            Assert.IsNotNull(configurationJson);
            Assert.IsFalse(string.IsNullOrWhiteSpace(configurationJson));
        }
        #endregion ExportConfiguration

        #region LoadConfiguration
        [DataTestMethod]
        [DataRow("{\"$type\":\"System.Collections.Generic.List`1[[PluginDemo.Interfaces.IPluginConfiguration, PluginDemo.Interfaces]], System.Private.CoreLib\",\"$values\":[{\"$type\":\"PluginDemo.Implementations.Base.PluginConfiguration, PluginDemo.Implementations.Base\",\"Settings\":{\"$type\":\"System.Collections.Generic.List`1[[PluginDemo.Interfaces.IPluginSetting, PluginDemo.Interfaces]], System.Private.CoreLib\",\"$values\":[]},\"Identifier\":{\"$type\":\"PluginDemo.Implementations.Base.PluginIdentifier, PluginDemo.Implementations.Base\",\"Name\":\"PluginDemo.Implementations.DemoPlugin1\",\"Version\":\"0.1.0.0\"},\"InstanceName\":\"NewInstance01\"},{\"$type\":\"PluginDemo.Implementations.Base.PluginConfiguration, PluginDemo.Implementations.Base\",\"Settings\":{\"$type\":\"System.Collections.Generic.List`1[[PluginDemo.Interfaces.IPluginSetting, PluginDemo.Interfaces]], System.Private.CoreLib\",\"$values\":[]},\"Identifier\":{\"$type\":\"PluginDemo.Implementations.Base.PluginIdentifier, PluginDemo.Implementations.Base\",\"Name\":\"PluginDemo.Implementations.DemoPlugin2\",\"Version\":\"0.1.0.0\"},\"InstanceName\":\"NewInstance02\"}]}")]
        [DataRow("{\"$type\":\"System.Collections.Generic.List`1[[PluginDemo.Interfaces.IPluginConfiguration, PluginDemo.Interfaces]], System.Private.CoreLib\",\"$values\":[{\"$type\":\"PluginDemo.Implementations.Base.PluginConfiguration, PluginDemo.Implementations.Base\",\"Settings\":{\"$type\":\"System.Collections.Generic.List`1[[PluginDemo.Interfaces.IPluginSetting, PluginDemo.Interfaces]], System.Private.CoreLib\",\"$values\":[{\"$type\":\"PluginDemo.Implementations.Base.PluginSetting, PluginDemo.Implementations.Base\",\"Name\":\"SomeSetting\",\"Type\":\"System.Int32, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e\",\"Value\":666},{\"$type\":\"PluginDemo.Implementations.Base.PluginSetting, PluginDemo.Implementations.Base\",\"Name\":\"SomeOtherSetting\",\"Type\":\"System.String, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e\",\"Value\":\"String Text\"}]},\"Identifier\":{\"$type\":\"PluginDemo.Implementations.Base.PluginIdentifier, PluginDemo.Implementations.Base\",\"Name\":\"PluginDemo.Implementations.DemoPlugin1\",\"Version\":\"0.1.0.0\"},\"InstanceName\":\"NewInstance01\"},{\"$type\":\"PluginDemo.Implementations.Base.PluginConfiguration, PluginDemo.Implementations.Base\",\"Settings\":{\"$type\":\"System.Collections.Generic.List`1[[PluginDemo.Interfaces.IPluginSetting, PluginDemo.Interfaces]], System.Private.CoreLib\",\"$values\":[]},\"Identifier\":{\"$type\":\"PluginDemo.Implementations.Base.PluginIdentifier, PluginDemo.Implementations.Base\",\"Name\":\"PluginDemo.Implementations.DemoPlugin2\",\"Version\":\"0.1.0.0\"},\"InstanceName\":\"NewInstance02\"}]}")]
        public void LoadConfiguration(string ConfigJson)
        {
            IPluginProviderService provider = new PluginProviderService();
            List <IPluginConfiguration> configurations = SerializerHelper.Deserialize<List<IPluginConfiguration>>(ConfigJson);    
            provider.SetConfiguration(configurations);

            Assert.IsNotNull(provider);
            Assert.AreNotEqual(0, provider.Instances.Count);
        }
        #endregion LoadConfiguration
    }
}
