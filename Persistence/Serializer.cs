using System;
using Newtonsoft.Json;

namespace Persistence
{
    /// <summary>
    /// Collects model serialization logic and settings.
    /// </summary>
    /// <typeparam name="T">The type to de/serialize.</typeparam>
    public class Serializer<T>
        where T : class
    {
        private static readonly System.Text.Encoding Encoding = System.Text.Encoding.UTF8;

        private static readonly JsonSerializerSettings SettingsForJson = new JsonSerializerSettings()
        {
            CheckAdditionalContent = true,
            MissingMemberHandling = MissingMemberHandling.Error,
            DefaultValueHandling = DefaultValueHandling.Include,
            ObjectCreationHandling = ObjectCreationHandling.Auto,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="Serializer{T}"/> class.
        /// </summary>
        public Serializer()
        {
            var attributesOfT = typeof(T).GetCustomAttributes(typeof(JsonObjectAttribute), true);
            if (attributesOfT.Length == 0)
            {
                throw new ArgumentException($"T does not have JsonObjectAttribute");
            }
        }

        /// <summary>
        /// Serialize a model.
        /// </summary>
        /// <param name="model">The model to serialize.</param>
        /// <returns>A buffer containing the serialized model.</returns>
        public byte[] Serialize(T model)
        {
            string content = JsonConvert.SerializeObject(model, SettingsForJson);
            return Encoding.GetBytes(content);
        }

        /// <summary>
        /// Deserialize a model.
        /// </summary>
        /// <param name="buffer">The bytes containing the serialized model.</param>
        /// <returns>A deserialized model.</returns>
        public T Deserialize(byte[] buffer)
        {
            string content = Encoding.GetString(buffer);
            return JsonConvert.DeserializeObject<T>(content, SettingsForJson);
        }
    }
}
