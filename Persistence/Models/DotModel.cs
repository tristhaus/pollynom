using System;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Persistence.Models
{
    /// <summary>
    /// Represents a dot for persistence.
    /// </summary>
    [JsonObject(ItemRequired = Required.Always)]
    public class DotModel : IEquatable<DotModel>
    {
        /// <summary>
        /// Gets or sets the kind of the dot.
        /// </summary>
        [JsonProperty("Kind", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public DotKind Kind { get; set; }

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

        /// <inheritdoc />
        public bool Equals(DotModel other)
        {
            return this.Kind.Equals(other.Kind)
                && this.X.Equals(other.X)
                && this.Y.Equals(other.Y);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            DotModel other = obj as DotModel;
            if (other == null)
            {
                return false;
            }

            return this.Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                int finalHash = 17;
                finalHash = finalHash * 23 + this.Kind.GetHashCode();
                finalHash = finalHash * 23 + this.X.GetHashCode();
                finalHash = finalHash * 23 + this.Y.GetHashCode();
                return finalHash;
            }
        }
    }
}
