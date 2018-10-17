using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Persistence;
using Persistence.Models;

namespace PersistenceTest
{
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

        public InmemoryGameRepository()
        {
            this.fileSystemStub = new Dictionary<string, string>();
        }

        internal InmemoryGameRepository(string path, string content)
            : this()
        {
            this.fileSystemStub[path] = content;
        }

        public GameModel LoadGame(string path)
        {
            if (!this.fileSystemStub.ContainsKey(path))
            {
                throw new FileNotFoundException("file not found in " + this.GetType().ToString(), path);
            }

            return JsonConvert.DeserializeObject<GameModel>(this.fileSystemStub[path]);
        }

        public void SaveGame(GameModel gameModel, string path)
        {
            this.fileSystemStub[path] = JsonConvert.SerializeObject(gameModel, SettingsForJson);
        }
    }
}
