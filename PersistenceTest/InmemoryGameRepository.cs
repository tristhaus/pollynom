using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Persistence;
using Persistence.Models;

namespace PersistenceTest
{
    /// <summary>
    /// A memory-based implementation of <see cref="IGameRepository"/> for testing purposes.
    /// </summary>
    public class InmemoryGameRepository : IGameRepository
    {
        private static readonly JsonSerializerSettings SettingsForJson = new JsonSerializerSettings()
        {
            CheckAdditionalContent = true,
            MissingMemberHandling = MissingMemberHandling.Error,
            DefaultValueHandling = DefaultValueHandling.Include,
            ObjectCreationHandling = ObjectCreationHandling.Auto,
        };

        private Dictionary<string, string> fileSystemStub;

        /// <summary>
        /// Initializes a new instance of the <see cref="InmemoryGameRepository"/> class.
        /// </summary>
        public InmemoryGameRepository()
        {
            this.fileSystemStub = new Dictionary<string, string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InmemoryGameRepository"/> class.
        /// Accepts the storage of content under the given path.
        /// </summary>
        /// <param name="path">The storage key under which to store content.</param>
        /// <param name="content">The content to store.</param>
        internal InmemoryGameRepository(string path, string content)
            : this()
        {
            this.fileSystemStub[path] = content;
        }

        /// <inheritdoc />
        public GameModel LoadGame(string path)
        {
            if (!this.fileSystemStub.ContainsKey(path))
            {
                throw new FileNotFoundException("file not found in " + this.GetType().ToString(), path);
            }

            return JsonConvert.DeserializeObject<GameModel>(this.fileSystemStub[path], SettingsForJson);
        }

        /// <inheritdoc />
        public void SaveGame(GameModel gameModel, string path)
        {
            this.fileSystemStub[path] = JsonConvert.SerializeObject(gameModel, SettingsForJson);
        }
    }
}
