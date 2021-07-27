using LocationAPI.Models.PostCode;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LocationAPI.Repository.PostCodes
{
    public interface IPostCodeDetailRepository
    {
        public Task<PostalCodeDetail> GetPostCodeDataForLocation(string code);

        public Task<IList<LocationNameIsoDetail>> GetLocationListDataList();
    }
}
