using LocationAPI.Models;
using LocationAPI.Repository.Phone;
using LocationAPI.Repository.PostCodes;
using System.Threading.Tasks;
using System.Collections.Generic;
using LocationAPI.Models.PostCode;

namespace LocationAPI.Manager
{
    public class LocationDetailsManager : ILocationDetailsManager
    {
        public IPostCodeDetailRepository postCodeRepository;

        public IPhoneNumberMetadataRepository phoneNumberMetadataRepository;

        public LocationDetailsManager(IPostCodeDetailRepository postCodeRepository, IPhoneNumberMetadataRepository phoneNumberMetadataRepository)
        {
            this.postCodeRepository = postCodeRepository;
            this.phoneNumberMetadataRepository = phoneNumberMetadataRepository;
        }

        public async Task<LocationDetails> GetLocationDetails(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                var phoneNumerDetails = await phoneNumberMetadataRepository.GetPhoneNumberMetadataDetails(code);
                var postCodeDetails = await postCodeRepository.GetPostCodeDataForLocation(code);

                if (phoneNumerDetails == null && postCodeDetails == null)
                {
                    return await Task.FromResult<LocationDetails>(null);
                }

                return new LocationDetails
                {
                    PhoneNumberMetadata = phoneNumerDetails,
                    PostalCodes = postCodeDetails
                };
            }

            return await Task.FromResult<LocationDetails>(null);
        }


        public async Task<IList<LocationNameIsoDetail>> GetLocationList()
        {
            return await postCodeRepository.GetLocationListDataList();
        }
    }
}
