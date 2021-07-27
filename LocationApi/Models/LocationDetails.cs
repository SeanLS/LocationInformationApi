using LocationAPI.Models.Phone;
using LocationAPI.Models.PostCode;

namespace LocationAPI.Models
{
    public class LocationDetails
    {
        public PhoneNumberMetadata PhoneNumberMetadata { get; set; }
        public PostalCodeDetail PostalCodes { get; set; }
    }
}
