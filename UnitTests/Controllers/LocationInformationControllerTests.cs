using LocationAPI.Controllers;
using LocationAPI.Manager;
using LocationAPI.Models;
using LocationAPI.Models.Phone;
using LocationAPI.Models.PostCode;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using LocationAPI.Mappings;
using System.Collections.Generic;

namespace UnitTests.Controllers
{
    // TODO - Coverage tool
    // https://dvoituron.com/2019/03/08/azure-devops-and-the-code-coverage/
    // https://stackoverflow.com/questions/32369664/does-visual-studio-have-code-coverage-for-unit-tests#:~:text=You%20can%20use%20the%20OpenCover,from%20here%20(release%20notes).
    // https://www.code4it.dev/blog/code-coverage-vs-2019-coverlet
    // https://docs.microsoft.com/en-us/azure/devops/pipelines/test/review-code-coverage-results?view=azure-devops
    [TestFixture]
    public class LocationInformationControllerTests
    {
        protected Mock<ILocationDetailsManager> mockLocationDetailsManager;

        protected Mock<ILogger<LocationInformationController>> mockLogger;

        protected IMapper mapper;

        protected LocationInformationController LocationInformationController { get; set; }

        private const string InvalidCode = "Z1";

        private const string ValidCode = "US";

        [OneTimeSetUp]
        public void Setup()
        {
            mockLocationDetailsManager = new Mock<ILocationDetailsManager>();
            mockLogger = new Mock<ILogger<LocationInformationController>>();

            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg => { cfg.AddProfile(new MappingProfile()); });
            mapper = mockMapper.CreateMapper();

            LocationInformationController = new LocationInformationController(mockLogger.Object, mapper, mockLocationDetailsManager.Object);
        }

        [Test]
        public async Task GetNotFoundResultWhenLocationCodeIsEmptyAndResturns400BadRequest()
        {
            // Arrange
            mockLocationDetailsManager.Setup(x => x.GetLocationDetails(It.IsAny<string>()));

            // Act
            var result = await LocationInformationController.Get(string.Empty);
            var statusCodeActionResult = (IStatusCodeActionResult)result;

            // Assert
            Assert.IsNotNull(statusCodeActionResult);
            Assert.That((HttpStatusCode)statusCodeActionResult.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [TestCase("Z")]
        [TestCase("ZZ1")]
        public async Task GetBadRequestResultWhenLocationCodeIsNotExpectedIsoFormatAndResturns400BadRequest(string code)
        {
            // Arrange
            mockLocationDetailsManager.Setup(x => x.GetLocationDetails(It.IsAny<string>()));

            // Act
            var result = await LocationInformationController.Get(code);
            var statusCodeActionResult = (IStatusCodeActionResult)result;

            // Assert
            Assert.IsNotNull(statusCodeActionResult);
            Assert.That((HttpStatusCode)statusCodeActionResult.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task GetNotFoundResultWhenLocationCodeIsInvalidAndResturns404NotFound()
        {
            // Arrange
            mockLocationDetailsManager.Setup(x => x.GetLocationDetails(InvalidCode)).ReturnsAsync((LocationDetails)null);

            // Act
            var result = await LocationInformationController.Get(InvalidCode);
            var statusCodeActionResult = (IStatusCodeActionResult)result;

            // Assert
            Assert.IsNotNull(statusCodeActionResult);
            Assert.That((HttpStatusCode)statusCodeActionResult.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetOkResultWhenLocationCodeIsValidAndResturns200Ok()
        {
            // Arrange
            var phoneNumberMetadata = Builder<PhoneNumberMetadata>.CreateNew().Build();
            var postalCodeDetail = Builder<PostalCodeDetail>.CreateNew().Build();
            var locationDetails = Builder<LocationDetails>
                                    .CreateNew()
                                    .With(x => x.PhoneNumberMetadata = phoneNumberMetadata)
                                    .With(x => x.PostalCodes = postalCodeDetail)
                                    .Build();


            mockLocationDetailsManager.Setup(x => x.GetLocationDetails(ValidCode)).ReturnsAsync(locationDetails);

            // Act
            var result = await LocationInformationController.Get(ValidCode);
            var statusCodeActionResult = (IStatusCodeActionResult)result;
            var okObjectResult = result as OkObjectResult;

            // Assert
            Assert.IsNotNull(statusCodeActionResult);
            Assert.That((HttpStatusCode)statusCodeActionResult.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.IsInstanceOf<LocationDetailsViewModel>(okObjectResult.Value);
        }

        [Test]
        public async Task GetCountryListResultsAndResturns200Ok()
        {
            // Arrange
            var countryLocationNameIsoDetail = Builder<LocationNameIsoDetail>.CreateListOfSize(1).Build();
            mockLocationDetailsManager.Setup(x => x.GetLocationList()).ReturnsAsync(countryLocationNameIsoDetail);

            // Act
            var result = await LocationInformationController.GetList();
            var statusCodeActionResult = (IStatusCodeActionResult)result;
            var okObjectResult = result as OkObjectResult;

            // Assert
            Assert.IsNotNull(statusCodeActionResult);
            Assert.That((HttpStatusCode)statusCodeActionResult.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.IsInstanceOf<IList<LocationNameIsoDetailViewModel>>(okObjectResult.Value);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            mockLogger.VerifyAll();
            mockLocationDetailsManager.VerifyAll();
        }
    }
}