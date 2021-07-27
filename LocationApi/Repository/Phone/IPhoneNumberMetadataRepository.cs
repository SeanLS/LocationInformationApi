using LocationAPI.Models.Phone;
using System.Threading.Tasks;

namespace LocationAPI.Repository.Phone
{
    public interface IPhoneNumberMetadataRepository
    {
        public Task<PhoneNumberMetadata> GetPhoneNumberMetadataDetails(string counryCode);
    }
}
