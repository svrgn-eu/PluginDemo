using PluginDemo.Common.Windows;
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
    public class PluginHostProviderTests
    {
        #region Creation
        [TestMethod]
        public void Creation()
        {
            IPluginHostProvider provider = new PluginHostProvider();

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
            IPluginHostProvider provider = new PluginHostProvider();
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
            IPluginHostProvider provider = new PluginHostProvider();
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
            IPluginHostProvider provider = new PluginHostProvider();

            string srcFilename = "AdditionalPlugins/PluginDemo.Implementations.DemoPlugin1-0.1.0-EXTRA.dll";
            string destFilename = "Plugins/PluginDemo.Implementations.DemoPlugin1-0.1.0-EXTRA.dll";

            if (File.Exists(destFilename))
            {
                //file is locked and cannot be deleted
                File.Delete(destFilename);
            }

            int pluginTypesBefore = provider.Plugins.Count;

            File.Copy(srcFilename, destFilename);
            Thread.Sleep(500);
            provider.Reload();
            int pluginTypesAfterCopy = provider.Plugins.Count;
            /*
            File.Delete(destFilename);
            Thread.Sleep(500);
            provider.Reload();
            int pluginTypesAfterDeletation = provider.Plugins.Count;
            */
            Assert.AreEqual(2, pluginTypesBefore);
            Assert.AreEqual(3, pluginTypesAfterCopy);
            //Assert.AreEqual(2, pluginTypesAfterDeletation);
        }
        #endregion AddPlugin

        //TODO: what to do with plugins which are deleted while an active instance is created? Mark? - update: cannot be deleted as it is in access

        #region AddPluginWithFileWatcher
        [TestMethod]
        public void AddPluginWithFileWatcher()
        {
            IPluginHostProvider provider = new PluginHostProvider();
            IDirectoryChangedWatcherService watcher = new DirectoryChangedWatcherService("Plugins");
            int lastNumberOfPlugins = 0;
            watcher.ContentChanged += (x, e) => { provider.Reload(); lastNumberOfPlugins = provider.Plugins.Count; };  //reload avalable plgins when something has changed, omit reloading later in the code

            string srcFilename = "AdditionalPlugins/PluginDemo.Implementations.DemoPlugin1-0.1.0-EXTRA.dll";
            string destFilename = "Plugins/PluginDemo.Implementations.DemoPlugin1-0.1.0-EXTRA.dll";

            if (File.Exists(destFilename))
            {
                //file is locked and cannot be deleted
                File.Delete(destFilename);
            }

            int pluginTypesBefore = provider.Plugins.Count;

            File.Copy(srcFilename, destFilename);
            //wait for watcher
            Thread.Sleep(500);
            int pluginTypesAfterCopy = lastNumberOfPlugins;
            
            /*
            File.Delete(destFilename);
            Thread.Sleep(500);
            int pluginTypesAfterDeletation = provider.Plugins.Count;
            */
            Assert.AreEqual(2, pluginTypesBefore);
            Assert.AreEqual(3, pluginTypesAfterCopy);
            //Assert.AreEqual(2, pluginTypesAfterDeletation);
        }
        #endregion AddPluginWithFileWatcher
    }
}
