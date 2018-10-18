using System.IO;
using System.Text;
using Newtonsoft.Json;
using Persistence.Models;

namespace Persistence
{
    /// <summary>
    /// A disk-based implementation of <see cref="IGameRepository"/>.
    /// </summary>
    public class OnDiskGameRepository : IGameRepository
    {
        private static readonly System.Text.Encoding Encoding = System.Text.Encoding.UTF8;

        private static readonly JsonSerializerSettings SettingsForJson = new JsonSerializerSettings()
        {
            CheckAdditionalContent = true,
            MissingMemberHandling = MissingMemberHandling.Error,
            DefaultValueHandling = DefaultValueHandling.Include,
            ObjectCreationHandling = ObjectCreationHandling.Auto,
        };

        /// <inheritdoc />
        public GameModel LoadGame(string path)
        {
            using (var fileStream = File.Open(path, FileMode.Open))
            {
                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, (int)fileStream.Length);
                string content = Encoding.GetString(buffer);

                return JsonConvert.DeserializeObject<GameModel>(content, SettingsForJson);
            }
        }

        /// <inheritdoc />
        public void SaveGame(GameModel gameModel, string path)
        {
            string content = JsonConvert.SerializeObject(gameModel, SettingsForJson);

            using (var fileStream = File.Open(path, FileMode.Create))
            {
                byte[] buffer = Encoding.GetBytes(content);
                fileStream.Write(buffer, 0, (int)buffer.Length);
                fileStream.Flush();
            }
        }
    }
}
