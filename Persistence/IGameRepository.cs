using System.IO;
using Persistence.Models;

namespace Persistence
{
    public interface IGameRepository
    {
        GameModel LoadGame(string path);

        void SaveGame(GameModel gameModel, string path);
    }
}
