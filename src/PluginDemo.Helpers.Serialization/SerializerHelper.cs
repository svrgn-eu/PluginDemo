using Newtonsoft.Json;
using System.IO;

namespace PluginDemo.Helpers.Serialization
{
    public static class SerializerHelper
    {
        #region Serialize
        public static string Serialize<T>(T neuralNetwork)
        {
            string result;
            using (TextWriter textWriter = new StringWriter())
            {
                JsonSerializer jsonSerializer = SerializerFactory.CreateSerializer();
                jsonSerializer.Serialize(textWriter, neuralNetwork);
                result = textWriter.ToString();
            }
            return result;
        }
        #endregion Serialize

        #region Deserialize
        public static T Deserialize<T>(string JsonInput)
        {
            T result = default;
            using (JsonReader jsonReader = new JsonTextReader(new StringReader(JsonInput)))
            {
                JsonSerializer jsonSerializer = SerializerFactory.CreateSerializer();
                var intermediateResult = jsonSerializer.Deserialize(jsonReader, typeof(T));
                result = (T)intermediateResult;
            }

            return result;
        }
        #endregion Deserialize
    }
}
