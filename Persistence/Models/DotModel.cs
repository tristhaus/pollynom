using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Persistence.Models
{
    /// <summary>
    /// Represents a dot for persistence.
    /// </summary>
    [JsonObject(ItemRequired = Required.Always)]
    public class DotModel
    {
        /// <summary>
        /// Enumerates the kinds of dots available.
        /// </summary>
        public enum DotKind
        {
            /// <summary>
            /// A good dot that the player wants to hit.
            /// </summary>
            Good,

            /// <summary>
            /// A bad dot that the player does not want to hit.
            /// </summary>
            Bad,
        }

        /// <summary>
        /// Gets or sets the kind of the dot.
        /// </summary>
        [JsonProperty("Kind", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public DotModel.DotKind Kind { get; set; }

        /// <summary>
        /// Gets or sets x-coordinate of the dot.
        /// </summary>
        [JsonProperty("X", Required = Required.Always)]
        public double X { get; set; }

        /// <summary>
        /// Gets or sets y-coordinate of the dot.
        /// </summary>
        [JsonProperty("Y", Required = Required.Always)]
        public double Y { get; set; }
    }
}
