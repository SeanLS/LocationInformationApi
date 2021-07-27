using LocationAPI.Manager;
using LocationAPI.Models.Phone;
using LocationAPI.Models.PostCode;
using LocationAPI.Repository.Phone;
using LocationAPI.Repository.PostCodes;
using FizzWare.NBuilder;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace UnitTests.Managers
{
    [TestFixture]
    public class LocationDetailsManagerTests
    {
        protected Mock<IPostCodeDetailRepository> mockPostCodeRepository;

        protected Mock<IPhoneNumberMetadataRepository> mockPhoneNumberMetadataRepository;

        protected LocationDetailsManager LocationDetailsManager { get; set; }

        private const string InvalidCode = "Z1";

        private const string ValidCode = "US";

        [OneTimeSetUp]
        public void Setup()
        {
            mockPostCodeRepository = new Mock<IPostCodeDetailRepository>();
            mockPhoneNumberMetadataRepository = new Mock<IPhoneNumberMetadataRepository>();
            LocationDetailsManager = new LocationDetailsManager(mockPostCodeRepository.Object, mockPhoneNumberMetadataRepository.Object);
        }

        [Test]
        public async Task GetNullResultWhenLocationCodeIsEmpty()
        {
            // Act
            var result = await LocationDetailsManager.GetLocationDetails(string.Empty);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetNullResultWhenLocationCodeIsInValid()
        {
            // Arrange
            mockPostCodeRepository.Setup(x => x.GetPostCodeDataForLocation(InvalidCode)).ReturnsAsync((PostalCodeDetail)null);
            mockPhoneNumberMetadataRepository.Setup(x => x.GetPhoneNumberMetadataDetails(InvalidCode)).ReturnsAsync((PhoneNumberMetadata)null);

            // Act
            var result = await LocationDetailsManager.GetLocationDetails(InvalidCode);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetValidResultWhenLocationCodeValid()
        {
            // Arrange
            var nationalNumberPatternBase = Builder<NationalNumberPatternBase>.CreateNew().Build();
            var phoneNumberFormatBaseList = Builder<PhoneNumberFormatBase>.CreateListOfSize(1).Build();
            var expectedPhoneNumberUtilResult = Builder<PhoneNumberMetadata>.CreateNew()
                                                .With(x => x.GeneralDesc = nationalNumberPatternBase)
                                                .With(x => x.PhoneNumberFormats = phoneNumberFormatBaseList)
                                                .Build();

            mockPhoneNumberMetadataRepository.Setup(x => x.GetPhoneNumberMetadataDetails(ValidCode)).ReturnsAsync(expectedPhoneNumberUtilResult);

            var expectedPostCodeResult = Builder<PostalCodeDetail>.CreateNew().With(x => x.Country = ValidCode).Build();
            mockPostCodeRepository.Setup(x => x.GetPostCodeDataForLocation(ValidCode)).ReturnsAsync(expectedPostCodeResult);

            // Act
            var result = await LocationDetailsManager.GetLocationDetails(ValidCode);

            // Assert
            Assert.IsNotNull(result);

            Assert.IsNotNull(result.PhoneNumberMetadata);
            Assert.IsNotEmpty(result.PhoneNumberMetadata.InternationalPrefix);
            Assert.That(result.PhoneNumberMetadata.InternationalPrefix, Is.EqualTo(expectedPhoneNumberUtilResult.InternationalPrefix));
            Assert.IsNotEmpty(result.PhoneNumberMetadata.NationalPrefix);
            Assert.That(result.PhoneNumberMetadata.NationalPrefix, Is.EqualTo(expectedPhoneNumberUtilResult.NationalPrefix));
            Assert.IsNotNull(result.PhoneNumberMetadata.GeneralDesc);
            Assert.IsNotEmpty(result.PhoneNumberMetadata.GeneralDesc.NationalNumberPattern);
            Assert.That(result.PhoneNumberMetadata.GeneralDesc.NationalNumberPattern, Is.EqualTo(expectedPhoneNumberUtilResult.GeneralDesc.NationalNumberPattern));

            Assert.GreaterOrEqual(result.PhoneNumberMetadata.PhoneNumberFormats.Count, 1);
            Assert.That(result.PhoneNumberMetadata.PhoneNumberFormats[0].Type, Is.EqualTo(expectedPhoneNumberUtilResult.PhoneNumberFormats[0].Type));
            Assert.That(result.PhoneNumberMetadata.PhoneNumberFormats[0].ExampleNumber, Is.EqualTo(expectedPhoneNumberUtilResult.PhoneNumberFormats[0].ExampleNumber));
            Assert.That(result.PhoneNumberMetadata.PhoneNumberFormats[0].NationalNumberPattern, Is.EqualTo(expectedPhoneNumberUtilResult.PhoneNumberFormats[0].NationalNumberPattern));

            Assert.IsNotNull(result.PostalCodes);
            Assert.That(result.PostalCodes.Equals(expectedPostCodeResult));
            Assert.That(result.PostalCodes.Country, Is.EqualTo(ValidCode));
            Assert.That(result.PostalCodes.Country, Is.EqualTo(expectedPostCodeResult.Country));
            Assert.That(result.PostalCodes.Format, Is.EqualTo(expectedPostCodeResult.Format));
            Assert.That(result.PostalCodes.ISO, Is.EqualTo(expectedPostCodeResult.ISO));
            Assert.That(result.PostalCodes.Note, Is.EqualTo(expectedPostCodeResult.Note));
            Assert.That(result.PostalCodes.Regex, Is.EqualTo(expectedPostCodeResult.Regex));
            mockPostCodeRepository.Verify(x => x.GetPostCodeDataForLocation(ValidCode), Times.Once);
        }

        [Test]
        public async Task GetCountryLocationList()
        {
            // Arrange
            var expectedResult = Builder<LocationNameIsoDetail>.CreateListOfSize(3).Build();
            mockPostCodeRepository.Setup(x => x.GetLocationListDataList()).ReturnsAsync(expectedResult);

            // Act
            var result = await LocationDetailsManager.GetLocationList();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result[0].ISO, Is.EqualTo(expectedResult[0].ISO));
            Assert.That(result[0].Country, Is.EqualTo(expectedResult[0].Country));
            Assert.That(result[1].ISO, Is.EqualTo(expectedResult[1].ISO));
            Assert.That(result[1].Country, Is.EqualTo(expectedResult[1].Country));
            Assert.That(result[2].ISO, Is.EqualTo(expectedResult[2].ISO));
            Assert.That(result[2].Country, Is.EqualTo(expectedResult[2].Country));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            mockPostCodeRepository.VerifyAll();
        }
    }
}