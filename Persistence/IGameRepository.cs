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

﻿using Persistence.Models;

namespace Persistence
{
    /// <summary>
    /// Defines operations related to storing and retrieving <see cref="GameModel"/>.
    /// </summary>
    public interface IGameRepository
    {
        /// <summary>
        /// Load a <see cref="GameModel"/> from storage.
        /// </summary>
        /// <param name="path">The path/storage key to the game information storage.</param>
        /// <returns>The game information.</returns>
        GameModel LoadGame(string path);

        /// <summary>
        /// Save a <see cref="GameModel"/> to storage.
        /// </summary>
        /// <param name="gameModel">The information to persist.</param>
        /// <param name="path">The path/storage key to the game information storage.</param>
        void SaveGame(GameModel gameModel, string path);
    }
}
