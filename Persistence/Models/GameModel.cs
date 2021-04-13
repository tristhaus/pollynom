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
        [JsonProperty("Id", Required = Required.Always)]
        public Guid Id { get; set; }

        [JsonProperty("Signature", Required = Required.Always)]
        public string Signature { get; set; }

        /// <summary>
        /// Gets or sets the collection of dots contained in the game.
        /// </summary>
        [JsonProperty("ExpressionStrings", Required = Required.Always)]
        public List<string> ExpressionStrings { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the collection of dots contained in the game.
        /// </summary>
        [JsonProperty("DotModels", Required = Required.Always)]
        public List<DotModel> DotModels { get; set; } = new List<DotModel>();

        /// <inheritdoc />
        public bool Equals(GameModel other)
        {
            if (this.Id != other.Id
                || this.Signature != other.Signature)
            {
                return false;
            }

            if (this.ExpressionStrings.Count != other.ExpressionStrings.Count
                || !this.ExpressionStrings.Zip(other.ExpressionStrings, (a, b) => { return a.Equals(b); }).All(x => x))
            {
                return false;
            }

            if (this.DotModels.Count != other.DotModels.Count
                || !this.DotModels.Zip(other.DotModels, (a, b) => { return a.Equals(b); }).All(x => x))
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

                foreach (var expressionString in this.ExpressionStrings)
                {
                    finalHash = finalHash * 23 + expressionString.GetHashCode();
                }

                foreach (var dotModel in this.DotModels)
                {
                    finalHash = finalHash * 23 + dotModel.GetHashCode();
                }

                return finalHash;
            }
        }
    }
}
