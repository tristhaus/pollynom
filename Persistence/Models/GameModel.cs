using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace Persistence.Models
{
    /// <summary>
    /// Represents a game for persistence.
    /// </summary>
    [JsonObject(ItemRequired = Required.Always)]
    public class GameModel : IEquatable<GameModel>
    {
        private List<string> expressionStrings = new List<string>();
        private List<DotModel> dotModels = new List<DotModel>();

        /// <summary>
        /// Gets or sets the collection of dots contained in the game.
        /// </summary>
        [JsonProperty("ExpressionStrings", Required = Required.Always)]
        public List<string> ExpressionStrings
        {
            get
            {
                return this.expressionStrings;
            }

            set
            {
                this.expressionStrings = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of dots contained in the game.
        /// </summary>
        [JsonProperty("DotModels", Required = Required.Always)]
        public List<DotModel> DotModels
        {
            get
            {
                return this.dotModels;
            }

            set
            {
                this.dotModels = value;
            }
        }

        /// <inheritdoc />
        public bool Equals(GameModel other)
        {
            if (this.expressionStrings.Count != other.expressionStrings.Count
                || !this.expressionStrings.Zip(other.expressionStrings, (a, b) => { return a.Equals(b); }).All(x => x))
            {
                return false;
            }

            if (this.dotModels.Count != other.dotModels.Count
                || !this.dotModels.Zip(other.dotModels, (a, b) => { return a.Equals(b); }).All(x => x))
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            GameModel other = obj as GameModel;
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

                foreach (var expressionString in this.expressionStrings)
                {
                    finalHash = finalHash * 23 + expressionString.GetHashCode();
                }

                foreach (var dotModel in this.dotModels)
                {
                    finalHash = finalHash * 23 + dotModel.GetHashCode();
                }

                return finalHash;
            }
        }
    }
}
