using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Persistence.Models;

namespace PersistenceTest
{
    /// <summary>
    /// Collects tests related to the JSON de/serialization.
    /// </summary>
    [TestClass]
    public class GameModelSerializationTest
    {
        /// <summary>
        /// Test the serialization of a <see cref="DotModel"/>.
        /// </summary>
        [TestMethod]
        public void DotSerialization()
        {
            // Arrange
            DotModel model = new DotModel()
            {
                X = 1.2,
                Y = 2.3,
            };

            // Act
            string modelJson = JsonConvert.SerializeObject(model);

            // Assert
            Assert.AreEqual(@"{""X"":1.2,""Y"":2.3}", modelJson.Replace(" ", string.Empty));
        }

        /// <summary>
        /// Test the deserialization of a <see cref="DotModel"/>.
        /// </summary>
        [TestMethod]
        public void DotDeserialization()
        {
            // Arrange
            string modelJson = @"{""X"":3.2,""Y"":4.3}";

            // Act
            DotModel actualModel = JsonConvert.DeserializeObject<DotModel>(modelJson);

            // Assert
            DotModel expectedModel = new DotModel()
            {
                X = 3.2,
                Y = 4.3,
            };

            Assert.AreEqual(expectedModel.X, actualModel.X);
            Assert.AreEqual(expectedModel.Y, actualModel.Y);
        }

        /// <summary>
        /// Test the serialization of a filled <see cref="GameModel"/>.
        /// </summary>
        [TestMethod]
        public void FilledGameSerialization()
        {
            // Arrange
            GameModel model = new GameModel();
            model.DotModels.Add(new DotModel()
            {
                X = 1.2,
                Y = 2.3,
            });
            model.DotModels.Add(new DotModel()
            {
                X = 4.2,
                Y = 4.3,
            });

            // Act
            string modelJson = JsonConvert.SerializeObject(model);

            // Assert
            Assert.AreEqual(@"{""DotModels"":[{""X"":1.2,""Y"":2.3},{""X"":4.2,""Y"":4.3}]}", modelJson.Replace(" ", string.Empty));
        }

        /// <summary>
        /// Test the deserialization of a filled <see cref="GameModel"/>.
        /// </summary>
        [TestMethod]
        public void FilledGameDeserialization()
        {
            // Arrange
            string modelJson = @"{""DotModels"":[{""X"":-1.4,""Y"":-2.5},{""X"":4.4,""Y"":4.5}]}";

            // Act
            GameModel actualModel = JsonConvert.DeserializeObject<GameModel>(modelJson);

            // Assert
            GameModel expectedModel = new GameModel();
            expectedModel.DotModels.Add(new DotModel()
            {
                X = -1.4,
                Y = -2.5,
            });
            expectedModel.DotModels.Add(new DotModel()
            {
                X = +4.4,
                Y = +4.5,
            });

            Assert.AreEqual(expectedModel.DotModels.Count, actualModel.DotModels.Count);
            for (int i = 0; i < expectedModel.DotModels.Count; i++)
            {
                Assert.AreEqual(expectedModel.DotModels[i].X, actualModel.DotModels[i].X);
                Assert.AreEqual(expectedModel.DotModels[i].Y, actualModel.DotModels[i].Y);
            }
        }

        /// <summary>
        /// Test the serialization of an empty <see cref="GameModel"/>.
        /// </summary>
        [TestMethod]
        public void EmptyGameSerialization()
        {
            // Arrange
            GameModel model = new GameModel();

            // Act
            string modelJson = JsonConvert.SerializeObject(model);

            // Assert
            Assert.AreEqual(@"{""DotModels"":[]}", modelJson.Replace(" ", string.Empty));
        }

        /// <summary>
        /// Test the deserialization of an empty <see cref="GameModel"/>.
        /// </summary>
        [TestMethod]
        public void EmptyGameDeserialization()
        {
            // Arrange
            string modelJson = @"{""DotModels"":[]}";

            // Act
            GameModel actualModel = JsonConvert.DeserializeObject<GameModel>(modelJson);

            // Assert
            GameModel expectedModel = new GameModel();

            Assert.AreEqual(expectedModel.DotModels.Count, actualModel.DotModels.Count);
            for (int i = 0; i < expectedModel.DotModels.Count; i++)
            {
                Assert.AreEqual(expectedModel.DotModels[i].X, actualModel.DotModels[i].X);
                Assert.AreEqual(expectedModel.DotModels[i].Y, actualModel.DotModels[i].Y);
            }
        }
    }
}
