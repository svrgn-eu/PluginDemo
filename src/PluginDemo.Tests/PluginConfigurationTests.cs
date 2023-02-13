using PluginDemo.Helpers.Serialization;
using PluginDemo.Implementations.Base;
using PluginDemo.Interfaces;

namespace PluginDemo.Tests
{
    [TestClass]
    public class PluginConfigurationTests
    {
        #region Serialize
        [TestMethod]
        public void Serialize()
        {
            IPluginConfiguration header = new PluginConfiguration("Instance1", new PluginIdentifier("SomeDemoPlugin", Version.Parse("0.9.0")));
            IPluginSetting setting = new PluginSetting("SomeSetting", 666);
            header.AddSetting(setting);

            string json = SerializerHelper.Serialize<IPluginConfiguration>(header);

            Assert.IsNotNull(json);
            Assert.IsFalse(string.IsNullOrWhiteSpace(json));
        }
        #endregion Serialize

        #region Deserialize
        [DataTestMethod]
        [DataRow("{\"$type\":\"PluginDemo.Implementations.Base.PluginConfiguration, PluginDemo.Implementations.Base\",\"Settings\":{\"$type\":\"System.Collections.Generic.List`1[[PluginDemo.Interfaces.IPluginSetting, PluginDemo.Interfaces]], System.Private.CoreLib\",\"$values\":[{\"$type\":\"PluginDemo.Implementations.Base.PluginSetting, PluginDemo.Implementations.Base\",\"Name\":\"SomeSetting\",\"Type\":\"System.Int32, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e\",\"Value\":666}]},\"Identifier\":{\"$type\":\"PluginDemo.Implementations.Base.PluginIdentifier, PluginDemo.Implementations.Base\",\"Name\":\"SomeDemoPlugin\",\"Version\":\"0.9.0\"},\"InstanceName\":\"Instance1\"}")]
        public void Deserialize(string json)
        {
            IPluginConfiguration config = SerializerHelper.Deserialize<IPluginConfiguration>(json);

            Assert.IsNotNull(config);
            Assert.IsNotNull(config.InstanceName);
            Assert.IsNotNull(config.Identifier);
            Assert.IsNotNull(config.Settings);
        }
        #endregion Deserialize
    }
}
