using LocationAPI.Repository.Phone;
using NUnit.Framework;
using PhoneNumbers;
using System;
using System.Threading.Tasks;
using static LocationAPI.Models.Phone.NumberTypeEnum;
using System.Linq;

namespace IntegrationTests.Repositories
{
    [TestFixture]
    public class PhoneNumberMetadataRepositoryTests
    {
        protected static PhoneNumberUtil PhoneNumberUtil { get; set; }

        protected PhoneNumberMetadataRepository phoneNumberMetadataRepository;

        private const string InvalidLocationCode = "Z1";

        private const string ValidLocationCode = "US";

        [OneTimeSetUp]
        public void Setup()
        {
            PhoneNumberUtil = PhoneNumberUtil.GetInstance();
            phoneNumberMetadataRepository = new PhoneNumberMetadataRepository();
        }


        [Test]
        public async Task ReturnNullWhenUsingEmptyLocationCodeForGetPhoneNumerDetails()
        {
            // Act
            var result = await phoneNumberMetadataRepository.GetPhoneNumberMetadataDetails(string.Empty);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task ReturnNullWhenUsingInvalidLocationCodeForGetPhoneNumerDetails()
        {

            // Act
            var result = await phoneNumberMetadataRepository.GetPhoneNumberMetadataDetails(InvalidLocationCode);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task ReturnAValueWhenUsingVvalidLocationCodeForGetPhoneNumerDetails()
        {
            // Arrange
            var expecedResult = PhoneNumberUtil.GetMetadataForRegion(ValidLocationCode);
            var expectedNumberType = Enum.GetName(typeof(TypeOfNumber), TypeOfNumber.FixedLine);

            // Act
            var result = await phoneNumberMetadataRepository.GetPhoneNumberMetadataDetails(ValidLocationCode);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.InternationalPrefix);
            Assert.That(result.InternationalPrefix, Is.EqualTo(expecedResult.InternationalPrefix));
            Assert.IsNotEmpty(result.NationalPrefix);
            Assert.That(result.NationalPrefix, Is.EqualTo(expecedResult.NationalPrefix));
            Assert.IsNotNull(result.GeneralDesc);
            Assert.IsNotEmpty(result.GeneralDesc.NationalNumberPattern);
            Assert.That(result.GeneralDesc.NationalNumberPattern, Is.EqualTo(expecedResult.GeneralDesc.NationalNumberPattern));

            Assert.GreaterOrEqual(result.PhoneNumberFormats.Count, 1);
            Assert.That(result.PhoneNumberFormats[0].Type.Equals(expectedNumberType));
            Assert.IsNotEmpty(result.PhoneNumberFormats[0].ExampleNumber);
            Assert.IsNotEmpty(result.PhoneNumberFormats[0].NationalNumberPattern);

            // Should be excluding VOIP as is has empty data
            Assert.That(result.PhoneNumberFormats.Count, Is.EqualTo(3));
            Assert.IsNull(result.PhoneNumberFormats.FirstOrDefault(x => x.Type == Enum.GetName(typeof(TypeOfNumber), TypeOfNumber.Voip)));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
        }
    }
}