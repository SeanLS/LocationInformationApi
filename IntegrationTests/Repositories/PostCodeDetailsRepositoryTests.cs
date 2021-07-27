using LocationAPI.Repository.PostCodes;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace IntegrationTests.Repositories
{
    [TestFixture]
    public class PostCodeDetailsRepositoryTests
    {
        protected PostCodeDetailRepository postCodeRepository;

        private const string InvalidLocationCode = "Z1";

        private const string ValidLocationCode = "US";

        private IConfiguration Configuration;

        [OneTimeSetUp]
        public void Setup()
        {
            // https://stackoverflow.com/questions/64794219/how-to-mock-iconfiguration-getvalue
            var appConfigSettings = new Dictionary<string, string> {
                {"PostCodeRepository:FileName", "postal-codes"},
                {"PostCodeRepository:FileNameType", "json"},
                {"PostCodeRepository:FilePath", "Repository\\PostCodes\\"}
            };

            Configuration = new ConfigurationBuilder().AddInMemoryCollection(appConfigSettings).Build();
            postCodeRepository = new PostCodeDetailRepository(Configuration);
        }

        [Test]
        public async Task ReturnNullWhenLocationCodeIsEmpty()
        {
            // Act
            var result = await postCodeRepository.GetPostCodeDataForLocation(string.Empty);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task ReturnNullWhenLocationCodeIsInvalid()
        {
            // Act
            var result = await postCodeRepository.GetPostCodeDataForLocation(InvalidLocationCode);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task ReturnPostalCodesWhenLocationCodeIsValid()
        {
            // Act
            var result = await postCodeRepository.GetPostCodeDataForLocation(ValidLocationCode);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Country, Is.EqualTo("United States"));
            Assert.That(result.Format, Is.EqualTo("NNNNN (optionally NNNNN-NNNN)"));
            Assert.That(result.ISO, Is.EqualTo("US"));
            Assert.IsNotEmpty(result.Note);
            Assert.That(result.Regex, Is.EqualTo("^\\b\\d{5}\\b(?:[- ]{1}\\d{4})?$"));
        }

        [Test]
        public async Task ReturnCountryLocationList()
        {
            // Act
            var result = await postCodeRepository.GetLocationListDataList();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.GreaterThan(1));
            Assert.That(result.FirstOrDefault(x => x.ISO == "US").Country, Is.EqualTo("United States"));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
        }
    }
}