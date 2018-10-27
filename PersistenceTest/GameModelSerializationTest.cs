using System;
using System.Collections.Generic;
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
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void DotSerialization()
        {
            // Arrange
            DotModel model = new DotModel()
            {
                Kind = DotKind.Good,
                X = 1.2,
                Y = 2.3,
            };

            // Act
            string modelJson = JsonConvert.SerializeObject(model);

            // Assert
            Assert.AreEqual(@"{""Kind"":""Good"",""X"":1.2,""Y"":2.3}", modelJson.Replace(" ", string.Empty));
        }

        /// <summary>
        /// Test the deserialization of a <see cref="DotModel"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void DotDeserialization()
        {
            // Arrange
            string modelJson = @"{""Kind"":""Good"",""X"":3.2,""Y"":4.3}";

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
            Assert.AreEqual(expectedModel.Kind, actualModel.Kind);
        }

        /// <summary>
        /// Test the serialization of a filled <see cref="GameModel"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void FilledGameSerialization()
        {
            // Arrange
            GameModel model = new GameModel()
            {
                Id = Guid.Parse("990a6482-3d44-44d1-8dd2-4eb1f86db0e7"),
                Signature = "someSignature",
            };
            model.ExpressionStrings = new List<string>() { "exp(x)", "x^2" };
            model.DotModels.Add(new DotModel()
            {
                Kind = DotKind.Good,
                X = 1.2,
                Y = 2.3,
            });
            model.DotModels.Add(new DotModel()
            {
                Kind = DotKind.Bad,
                X = 4.2,
                Y = 4.3,
            });

            // Act
            string modelJson = JsonConvert.SerializeObject(model);

            // Assert
            Assert.AreEqual(@"{""Id"":""990a6482-3d44-44d1-8dd2-4eb1f86db0e7"",""Signature"":""someSignature"",""ExpressionStrings"":[""exp(x)"",""x^2""],""DotModels"":[{""Kind"":""Good"",""X"":1.2,""Y"":2.3},{""Kind"":""Bad"",""X"":4.2,""Y"":4.3}]}", modelJson.Replace(" ", string.Empty));
        }

        /// <summary>
        /// Test the deserialization of a filled <see cref="GameModel"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void FilledGameDeserialization()
        {
            // Arrange
            string modelJson = @"{""Id"":""990a6482-3d44-44d1-8dd2-4eb1f86db0e7"",""Signature"":""someSignature"",""ExpressionStrings"":[""exp(x)"",""x^2""],""DotModels"":[{""Kind"":""Good"",""X"":-1.4,""Y"":-2.5},{""Kind"":""Bad"",""X"":4.4,""Y"":4.5}]}";

            // Act
            GameModel actualModel = JsonConvert.DeserializeObject<GameModel>(modelJson);

            // Assert
            GameModel expectedModel = new GameModel()
            {
                Id = Guid.Parse("990a6482-3d44-44d1-8dd2-4eb1f86db0e7"),
                Signature = "someSignature",
            };
            expectedModel.ExpressionStrings = new List<string>() { "exp(x)", "x^2" };
            expectedModel.DotModels.Add(new DotModel()
            {
                Kind = DotKind.Good,
                X = -1.4,
                Y = -2.5,
            });
            expectedModel.DotModels.Add(new DotModel()
            {
                Kind = DotKind.Bad,
                X = +4.4,
                Y = +4.5,
            });

            Assert.AreEqual(expectedModel, actualModel);
        }

        /// <summary>
        /// Test the serialization of an empty <see cref="GameModel"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void EmptyGameSerialization()
        {
            // Arrange
            GameModel model = new GameModel()
            {
                Id = Guid.Parse("990a6482-3d44-44d1-8dd2-4eb1f86db0e7"),
                Signature = "someSignature",
            };

            // Act
            string modelJson = JsonConvert.SerializeObject(model);

            // Assert
            Assert.AreEqual(@"{""Id"":""990a6482-3d44-44d1-8dd2-4eb1f86db0e7"",""Signature"":""someSignature"",""ExpressionStrings"":[],""DotModels"":[]}", modelJson.Replace(" ", string.Empty));
        }

        /// <summary>
        /// Test the deserialization of an empty <see cref="GameModel"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public void EmptyGameDeserialization()
        {
            // Arrange
            string modelJson = @"{""Id"":""990a6482-3d44-44d1-8dd2-4eb1f86db0e7"",""Signature"":""someSignature"",""ExpressionStrings"":[],""DotModels"":[]}";

            // Act
            GameModel actualModel = JsonConvert.DeserializeObject<GameModel>(modelJson);

            // Assert
            GameModel expectedModel = new GameModel()
            {
                Id = Guid.Parse("990a6482-3d44-44d1-8dd2-4eb1f86db0e7"),
                Signature = "someSignature",
            };

            Assert.AreEqual(expectedModel.DotModels.Count, actualModel.DotModels.Count);
        }
    }
}
