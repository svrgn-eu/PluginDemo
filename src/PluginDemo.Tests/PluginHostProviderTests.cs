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
                Assert.IsNotNull(pluginHost.Plugin);
                Assert.IsNotNull(pluginHost.Plugin.MetaData);
                Assert.IsNotNull(pluginHost.Plugin.MetaData.Identifier);
            }
        }
        #endregion Creation

        //TODO: add negative tests
        // -missing assembly info in the file
        // wildly named files
        // missing authorinfo
    }
}
