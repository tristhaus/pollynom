/*
 * This file is part of PollyNom.
 * 
 * PollyNom is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * PollyNom is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with PollyNom.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

﻿using System;
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
