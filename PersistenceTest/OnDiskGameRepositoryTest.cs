﻿using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace PersistenceTest
{
    /// <summary>
    /// Collects integration tests related to the <see cref="OnDiskGameRepository"/>.
    /// </summary>
    [TestClass]
    public class OnDiskGameRepositoryTest : GameRepositoryTestBase
    {
        private readonly IGameRepository gameRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnDiskGameRepositoryTest"/> class.
        /// </summary>
        public OnDiskGameRepositoryTest()
        {
            this.gameRepository = new OnDiskGameRepository();
        }

        /// <inheritdoc />
        protected override IGameRepository GameRepository => this.gameRepository;

        /// <summary>
        /// Compare <see cref="GameRepositoryTestBase.ShouldPerformRoundTripSavingLoading"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.IntegrationTest)]
        public new void ShouldPerformRoundTripSavingLoading()
        {
            base.ShouldPerformRoundTripSavingLoading();
        }

        /// <summary>
        /// Compare <see cref="GameRepositoryTestBase.ShouldAllowOverwriting"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.IntegrationTest)]
        public new void ShouldAllowOverwriting()
        {
            base.ShouldAllowOverwriting();
        }

        /// <summary>
        /// Compare <see cref="GameRepositoryTestBase.ShouldThrowForNonExistentFile"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.IntegrationTest)]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public new void ShouldThrowForNonExistentFile()
        {
            base.ShouldThrowForNonExistentFile();
        }

        /// <summary>
        /// Compare <see cref="GameRepositoryTestBase.ShouldThrowForNonCompatibleFileContent"/>.
        /// </summary>
        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.IntegrationTest)]
        [ExpectedException(typeof(Newtonsoft.Json.JsonSerializationException))]
        public void ShouldThrowForNonCompatibleFileContent()
        {
            // special setup: create bad file
            var tempPath = Path.GetTempPath();

            // must be kept in sync with other tests
            const string filename = @"synchronizedNameOfBadFile.json";

            string path = Path.Combine(tempPath, filename);
            const string badJson = @"{ ""someKey"": ""someContent""}";

            System.Text.Encoding encoding = System.Text.Encoding.UTF8;

            using (var fileStream = File.Open(path, FileMode.Create))
            {
                byte[] buffer = encoding.GetBytes(badJson);
                fileStream.Write(buffer, 0, (int)buffer.Length);
                fileStream.Flush();
            }

            base.ShouldThrowForNonCompatibleFileContent(this.GameRepository);
        }
    }
}
