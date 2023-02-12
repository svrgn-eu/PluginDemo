using Newtonsoft.Json;

namespace PluginDemo.Helpers.Serialization
{
    internal class SerializerFactory
    {
        #region Methods

        #region CreateSerializer
        internal static JsonSerializer CreateSerializer()
        {
            JsonSerializer result = new JsonSerializer();
            result.TypeNameHandling = TypeNameHandling.All;  //serializes type names
            result.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;  //ignore circular references as we will have a bunch of them (nature of the network)
            result.Formatting = Formatting.Indented;
            result.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
            result.NullValueHandling = NullValueHandling.Ignore;
            result.MissingMemberHandling = MissingMemberHandling.Ignore;
            result.ObjectCreationHandling = ObjectCreationHandling.Auto;
            return result;
        }
        #endregion CreateSerializer

        #endregion Methods
    }
}
