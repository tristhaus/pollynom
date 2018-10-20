﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace PersistenceTest
{
    /// <summary>
    /// Collects tests related to the mock-like <see cref="InMemoryGameRepository"/>.
    /// </summary>
    [TestClass]
    public class InMemoryGameRepositoryTest : GameRepositoryTestBase
    {
        private IGameRepository gameRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryGameRepositoryTest"/> class.
        /// </summary>
        public InMemoryGameRepositoryTest()
        {
            this.gameRepository = new InMemoryGameRepository();
        }

        /// <inheritdoc />
        protected override IGameRepository GameRepository => this.gameRepository;

        /// <summary>
        /// Compare <see cref="GameRepositoryTestBase.ShouldPerformRoundTripSavingLoading"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public new void ShouldPerformRoundTripSavingLoading()
        {
            base.ShouldPerformRoundTripSavingLoading();
        }

        /// <summary>
        /// Compare <see cref="GameRepositoryTestBase.ShouldAllowOverwriting"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public new void ShouldAllowOverwriting()
        {
            base.ShouldAllowOverwriting();
        }

        /// <summary>
        /// Compare <see cref="GameRepositoryTestBase.ShouldThrowForNonExistentFile"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public new void ShouldThrowForNonExistentFile()
        {
            base.ShouldThrowForNonExistentFile();
        }

        /// <summary>
        /// Compare <see cref="GameRepositoryTestBase.ShouldThrowForNonCompatibleFileContent"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        [ExpectedException(typeof(Newtonsoft.Json.JsonSerializationException))]
        public void ShouldThrowForNonCompatibleFileContent()
        {
            // special setup: create bad file
            const string path = @"F:\temp\synchronizedNameOfBadFile.json";
            const string badJson = @"{ ""someKey"": ""someContent""}";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(badJson);
            IGameRepository specialGameRepository = new InMemoryGameRepository(path, buffer);

            base.ShouldThrowForNonCompatibleFileContent(specialGameRepository);
        }
    }
}
