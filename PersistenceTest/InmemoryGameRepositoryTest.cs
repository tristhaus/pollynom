using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace PersistenceTest
{
    [TestClass]
    public class InmemoryGameRepositoryTest : GameRepositoryTestBase
    {
        private IGameRepository gameRepository;

        public InmemoryGameRepositoryTest()
        {
            this.gameRepository = new InmemoryGameRepository();
        }

        protected override IGameRepository GameRepository
        {
            get
            {
                return this.gameRepository;
            }
        }

        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public new void ShouldPerformRoundTripSavingLoading()
        {
            base.ShouldPerformRoundTripSavingLoading();
        }

        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        public new void ShouldAllowOverwriting()
        {
            base.ShouldAllowOverwriting();
        }

        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public new void ShouldThrowForNonExistentFile()
        {
            base.ShouldThrowForNonExistentFile();
        }

        [TestMethod]
        [TestCategory(TestInfrastructure.TestCategories.UnitTest)]
        [ExpectedException(typeof(Newtonsoft.Json.JsonSerializationException))]
        public void ShouldThrowForNonCompatibleFileContent()
        {
            // special setup: create bad file
            const string path = @"C:\temp\synchronizedNameOfBadFile.json";
            const string badJson = @"{ ""someKey"": ""someContent""}";
            IGameRepository specialGameRepository = new InmemoryGameRepository(path, badJson);

            base.ShouldThrowForNonCompatibleFileContent(specialGameRepository);
        }
    }
}
