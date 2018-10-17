using Newtonsoft.Json;

namespace Persistence.Models
{
    /// <summary>
    /// Represents a dot for persistence.
    /// </summary>
    [JsonObject(ItemRequired = Required.Always)]
    public class DotModel
    {
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
