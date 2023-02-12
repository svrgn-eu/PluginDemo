using PluginDemo.Helpers.Serialization;
using PluginDemo.Implementations.Base;
using PluginDemo.Interfaces;

namespace PluginDemo.Tests
{
    [TestClass]
    public class PluginHeaderTests
    {
        #region Serialize
        [TestMethod]
        public void Serialize()
        {
            IPluginHeader header = new PluginHeader("SomeDemoPlugin", Version.Parse("0.9.0"), "efilnukefesin");
            IPluginSetting setting = new PluginSetting("SomeSetting", 666);
            header.AddSetting(setting);

            string json = SerializerHelper.Serialize<IPluginHeader>(header);

            Assert.IsNotNull(json);
            Assert.IsFalse(string.IsNullOrWhiteSpace(json));
        }
        #endregion Serialize

        #region Deserialize
        [DataTestMethod]
        [DataRow("{\"$type\":\"PluginDemo.Implementations.Base.PluginHeader, PluginDemo.Implementations.Base\",\"Name\":\"SomeDemoPlugin\",\"Version\":\"0.9.0\",\"Author\":\"efilnukefesin\",\"Settings\":{\"$type\":\"System.Collections.Generic.List`1[[PluginDemo.Interfaces.IPluginSetting, PluginDemo.Interfaces]], System.Private.CoreLib\",\"$values\":[{\"$type\":\"PluginDemo.Implementations.Base.PluginSetting, PluginDemo.Implementations.Base\",\"Name\":\"SomeSetting\",\"Type\":\"System.Int32, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e\",\"Value\":666}]}}")]
        public void Deserialize(string json)
        {
            IPluginHeader header = SerializerHelper.Deserialize<IPluginHeader>(json);

            Assert.IsNotNull(header);
            Assert.IsNotNull(header.Name);
            Assert.IsNotNull(header.Version);
            Assert.IsNotNull(header.Author);
            Assert.IsNotNull(header.Settings);
        }
        #endregion Deserialize
    }
}
