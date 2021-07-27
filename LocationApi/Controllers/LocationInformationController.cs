using LocationAPI.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AutoMapper;
using LocationAPI.Models;
using System.Collections.Generic;
using LocationAPI.Models.PostCode;

namespace LocationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationInformationController : ControllerBase
    {
        protected ILocationDetailsManager locationDetailsManager;

        private readonly ILogger<LocationInformationController> _logger;

        private readonly IMapper _mapper;

        public LocationInformationController(ILogger<LocationInformationController> logger, IMapper mapper, ILocationDetailsManager locationDetailsManager)
        {
            _logger = logger;
            _mapper = mapper;
            this.locationDetailsManager = locationDetailsManager;
        }

        //// https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-5.0
        //// https://nordicapis.com/ultimate-guide-to-30-api-documentation-solutions/
        //// https://pronovix.com/blog/free-and-open-source-api-documentation-tools?platform=hootsuite

        /// <summary>
        /// Gets a details for a specific location
        /// </summary>
        /// <param name="code"></param>
        /// <returns>A LocationDetailsViewModel object</returns>
        /// <response code="400">If the code format is unsupported</response>
        /// <response code="404">If the code is not recognised</response>       
        /// <response code="200">If the code returns data</response>    
        [Produces("application/json")]
        [HttpGet("Code/{code}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(LocationDetailsViewModel), StatusCodes.Status200OK)]
        [ResponseCache(CacheProfileName = "Default30ResponseCacheLocationAnyNoStoreFalse")]
        public async Task<IActionResult> Get(string code)
        {
            if (string.IsNullOrEmpty(code) || code.Length > 2 || code.Length < 2)
            {
                return StatusCode(StatusCodes.Status400BadRequest, null);
            }

            var locationDetails = await locationDetailsManager.GetLocationDetails(code);

            if (locationDetails == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, null);
            }
                        
            return new OkObjectResult(_mapper.Map<LocationDetailsViewModel>(locationDetails));
        }

        [Produces("application/json")]
        [HttpGet()]
        [ProducesResponseType(typeof(LocationNameIsoDetailViewModel), StatusCodes.Status200OK)]
        [ResponseCache(CacheProfileName= "Default30ResponseCacheLocationAnyNoStoreFalse")]
        public async Task<IActionResult> GetList()
        {
            var locationList = await locationDetailsManager.GetLocationList();
            return new OkObjectResult(_mapper.Map<IList<LocationNameIsoDetail>, IList<LocationNameIsoDetailViewModel>>(locationList));
        }
    }
}
