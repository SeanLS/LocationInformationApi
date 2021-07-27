using AutoMapper;
using FizzWare.NBuilder;
using NUnit.Framework;
using System.Threading.Tasks;
using LocationAPI.Mappings;
using LocationAPI.Models;
using LocationAPI.Models.Phone;
using LocationAPI.Models.PostCode;

namespace UnitTests.Mappers
{
    [TestFixture]
    public class MapperTests
    {
        protected IMapper mapper;

        [OneTimeSetUp]
        public void Setup()
        {
            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            mapper = mockMapper.CreateMapper();
        }

        [Test]
        public async Task LocationDetailsViewModelMapping()
        {
            // Arrange
            var nationalNumberPatternBase = Builder<NationalNumberPatternBase>.CreateNew().Build();
            var phoneNumberFormatBase = Builder<PhoneNumberFormatBase>.CreateListOfSize(1).Build();
            var phoneNumberMetadata = Builder<PhoneNumberMetadata>
                .CreateNew()
                .With(x => x.GeneralDesc = nationalNumberPatternBase)
                .With(x => x.PhoneNumberFormats = phoneNumberFormatBase)
                .Build();
            
            var postalCodeDetail = Builder<PostalCodeDetail>.CreateNew().Build();
            var locationDetails = Builder<LocationDetails>
                .CreateNew()
                .With(x => x.PhoneNumberMetadata = phoneNumberMetadata)
                .With(x => x.PostalCodes = postalCodeDetail)
                .Build();

            // Act
            var result = await Task.Run(() => mapper.Map<LocationDetailsViewModel>(locationDetails));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.PhoneNumberMetadata);
            Assert.IsNotNull(result.PhoneNumberMetadata.GeneralDesc);
            Assert.That(result.PhoneNumberMetadata.InternationalPrefix, Is.EqualTo(locationDetails.PhoneNumberMetadata.InternationalPrefix));
            Assert.That(result.PhoneNumberMetadata.NationalPrefix, Is.EqualTo(locationDetails.PhoneNumberMetadata.NationalPrefix));
            Assert.IsNotNull(result.PhoneNumberMetadata.PhoneNumberFormats);
            Assert.That(result.PhoneNumberMetadata.PhoneNumberFormats[0].ExampleNumber, Is.EqualTo(locationDetails.PhoneNumberMetadata.PhoneNumberFormats[0].ExampleNumber));
            Assert.That(result.PhoneNumberMetadata.PhoneNumberFormats[0].NationalNumberPattern, Is.EqualTo(locationDetails.PhoneNumberMetadata.PhoneNumberFormats[0].NationalNumberPattern));
            Assert.That(result.PhoneNumberMetadata.PhoneNumberFormats[0].Type, Is.EqualTo(locationDetails.PhoneNumberMetadata.PhoneNumberFormats[0].Type));
            Assert.IsNotNull(result.PostalCodes);
            Assert.That(result.PostalCodes.Format, Is.EqualTo(locationDetails.PostalCodes.Format));
            Assert.That(result.PostalCodes.ISO, Is.EqualTo(locationDetails.PostalCodes.ISO));
            Assert.That(result.PostalCodes.Note, Is.EqualTo(locationDetails.PostalCodes.Note));
            Assert.That(result.PostalCodes.Regex, Is.EqualTo(locationDetails.PostalCodes.Regex));
            Assert.That(result.PostalCodes.TerritoryOrCountry, Is.EqualTo(locationDetails.PostalCodes.Country));
            
        }

        [Test]
        public async Task PostalCodeDetailViewModelMapping()
        {
            // Arrange
            var postalCodeDetail = Builder<PostalCodeDetail>.CreateNew().Build();

            // Act
            var result = await Task.Run(() => mapper.Map<PostalCodeDetailViewModel>(postalCodeDetail));

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Format, Is.EqualTo(postalCodeDetail.Format));
            Assert.That(result.ISO, Is.EqualTo(postalCodeDetail.ISO));
            Assert.That(result.Note, Is.EqualTo(postalCodeDetail.Note));
            Assert.That(result.Regex, Is.EqualTo(postalCodeDetail.Regex));
            Assert.That(result.TerritoryOrCountry, Is.EqualTo(postalCodeDetail.Country));
        }

        [Test]
        public async Task NationalNumberPatternBaseViewModelMapping()
        {
            // Arrange
            var nationalNumberPatternBase = Builder<NationalNumberPatternBase>.CreateNew().Build();

            // Act
            var result = await Task.Run(() => mapper.Map<NationalNumberPatternBaseViewModel>(nationalNumberPatternBase));

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.NationalNumberPattern, Is.EqualTo(nationalNumberPatternBase.NationalNumberPattern));
        }

        [Test]
        public async Task PhoneNumberFormatBaseViewModelMapping()
        {
            // Arrange
            var phoneNumberFormatBase = Builder<PhoneNumberFormatBase>.CreateNew().Build();

            // Act
            var result = await Task.Run(() => mapper.Map<PhoneNumberFormatBaseViewModel>(phoneNumberFormatBase));

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.ExampleNumber, Is.EqualTo(phoneNumberFormatBase.ExampleNumber));
            Assert.That(result.NationalNumberPattern, Is.EqualTo(phoneNumberFormatBase.NationalNumberPattern));
            Assert.That(result.Type, Is.EqualTo(phoneNumberFormatBase.Type));
        }

        [Test]
        public async Task PhoneNumberMetadataViewModelMapping()
        {
            // Arrange
            var phoneNumberFormatBase = Builder<PhoneNumberFormatBase>.CreateListOfSize(1).Build();
            var phoneNumberMetadata = Builder<PhoneNumberMetadata>
                .CreateNew()
                .With(x => x.PhoneNumberFormats = phoneNumberFormatBase)
                .Build();

            // Act
            var result = await Task.Run(() => mapper.Map<PhoneNumberMetadataViewModel>(phoneNumberMetadata));

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.GeneralDesc, Is.EqualTo(phoneNumberMetadata.GeneralDesc));
            Assert.That(result.InternationalPrefix, Is.EqualTo(phoneNumberMetadata.InternationalPrefix));
            Assert.That(result.NationalPrefix, Is.EqualTo(phoneNumberMetadata.NationalPrefix));
            Assert.IsNotNull(result.PhoneNumberFormats);
            Assert.That(result.PhoneNumberFormats[0].ExampleNumber, Is.EqualTo(phoneNumberMetadata.PhoneNumberFormats[0].ExampleNumber));
            Assert.That(result.PhoneNumberFormats[0].NationalNumberPattern, Is.EqualTo(phoneNumberMetadata.PhoneNumberFormats[0].NationalNumberPattern));
            Assert.That(result.PhoneNumberFormats[0].Type, Is.EqualTo(phoneNumberMetadata.PhoneNumberFormats[0].Type));
        }
        
        [Test]
        public async Task CountryLocationNameIsoDetailViewModelMapping()
        {
            // Arrange
            var countryLocationNameIsoDetail = Builder<LocationNameIsoDetail>.CreateNew().Build();

            // Act
            var result = await Task.Run(() => mapper.Map<LocationNameIsoDetailViewModel>(countryLocationNameIsoDetail));

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.TerritoryOrCountry, Is.EqualTo(countryLocationNameIsoDetail.Country));
            Assert.That(result.ISO, Is.EqualTo(countryLocationNameIsoDetail.ISO));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
        }
    }
}