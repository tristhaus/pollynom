﻿using System.Collections.Generic;

using Newtonsoft.Json;

namespace Persistence.Models
{
    /// <summary>
    /// Represents a game for persistence.
    /// </summary>
    [JsonObject(ItemRequired = Required.Always)]
    public class GameModel
    {
        private List<DotModel> dotModels = new List<DotModel>();

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
    }
}