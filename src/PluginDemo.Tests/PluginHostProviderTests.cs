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
            foreach (IPluginHost pluginHost in provider.Plugins)
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
    }
}
