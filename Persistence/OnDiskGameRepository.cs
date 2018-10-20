using System.IO;
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
