using Persistence.Models;

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
