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

﻿using System.IO;
using Persistence.Models;

namespace Persistence
{
    /// <summary>
    /// A disk-based implementation of <see cref="IGameRepository"/>.
    /// </summary>
    public class OnDiskGameRepository : IGameRepository
    {
        private readonly Serializer<GameModel> serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnDiskGameRepository"/> class.
        /// </summary>
        public OnDiskGameRepository()
        {
            this.serializer = new Serializer<GameModel>();
        }

        /// <inheritdoc />
        public void SaveGame(GameModel gameModel, string path)
        {
            byte[] buffer = this.serializer.Serialize(gameModel);

            using (var fileStream = File.Open(path, FileMode.Create))
            {
                fileStream.Write(buffer, 0, (int)buffer.Length);
                fileStream.Flush();
            }
        }

        /// <inheritdoc />
        public GameModel LoadGame(string path)
        {
            using (var fileStream = File.Open(path, FileMode.Open))
            {
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, (int)fileStream.Length);

                return this.serializer.Deserialize(buffer);
            }
        }
    }
}
