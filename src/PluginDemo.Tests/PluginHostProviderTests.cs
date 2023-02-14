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
        }
        #endregion Creation
    }
}
