using LocationAPI.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using LocationAPI.Models.PostCode;

namespace LocationAPI.Manager
{
    public interface ILocationDetailsManager
    {
        public Task<LocationDetails> GetLocationDetails(string code);

        public Task<IList<LocationNameIsoDetail>> GetLocationList();
    }
}
