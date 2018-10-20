using System.Collections.Generic;
using System.IO;
using Persistence;
using Persistence.Models;

namespace PersistenceTest
{
    /// <summary>
    /// A memory-based implementation of <see cref="IGameRepository"/> for testing purposes.
    /// </summary>
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly Serializer<GameModel> serializer;

        private Dictionary<string, byte[]> fileSystemStub;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryGameRepository"/> class.
        /// </summary>
        public InMemoryGameRepository()
        {
            this.serializer = new Serializer<GameModel>();
            this.fileSystemStub = new Dictionary<string, byte[]>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryGameRepository"/> class.
        /// Accepts the storage of content under the given path.
        /// </summary>
        /// <param name="path">The storage key under which to store content.</param>
        /// <param name="buffer">The content to store.</param>
        internal InMemoryGameRepository(string path, byte[] buffer)
            : this()
        {
            this.fileSystemStub[path] = buffer;
        }

        /// <inheritdoc />
        public GameModel LoadGame(string path)
        {
            if (!this.fileSystemStub.ContainsKey(path))
            {
                throw new FileNotFoundException("file not found in " + this.GetType().ToString(), path);
            }

            return this.serializer.Deserialize(this.fileSystemStub[path]);
        }

        /// <inheritdoc />
        public void SaveGame(GameModel gameModel, string path)
        {
            this.fileSystemStub[path] = this.serializer.Serialize(gameModel);
        }
    }
}
