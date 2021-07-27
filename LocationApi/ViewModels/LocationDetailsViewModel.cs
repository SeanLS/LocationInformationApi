using LocationAPI.Models.Phone;
using LocationAPI.Models.PostCode;

namespace LocationAPI.Models
{
    public class LocationDetailsViewModel
    {
        public PostalCodeDetailViewModel PostalCodes { get; set; }
        public PhoneNumberMetadataViewModel PhoneNumberMetadata { get; set; }
    }
}
