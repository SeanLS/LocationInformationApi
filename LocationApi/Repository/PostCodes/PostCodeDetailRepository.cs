using LocationAPI.Models.PostCode;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace LocationAPI.Repository.PostCodes
{
    public class PostCodeDetailRepository : IPostCodeDetailRepository
    {
        //// https://gist.github.com/matthewbednarski/4d15c7f50258b82e2d7e#file-postal-codes-json
        private static string FileName { get; set; }

        private static string FileNameType { get; set; }

        private static string FilePath { get; set; }

        private static string FullPath { get; set; }       

        private static PostalCodeDetail PostalCodes { get; set; }

        private readonly IConfiguration Configuration;

        public PostCodeDetailRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            FileName = Configuration["PostCodeRepository:FileName"];
            FileNameType = Configuration["PostCodeRepository:FileNameType"];
            FilePath = Configuration["PostCodeRepository:FilePath"];

            FullPath = Path.Combine(Environment.CurrentDirectory, FilePath, FileName + "." + FileNameType);
        }

        public async Task<PostalCodeDetail> GetPostCodeDataForLocation(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                var jsonString = File.ReadAllText(FullPath);
                PostalCodes = JsonSerializer.Deserialize<PostalCodeDetail[]>(jsonString).FirstOrDefault(x => string.Equals(x.ISO, code, StringComparison.OrdinalIgnoreCase));
                return await Task.FromResult(PostalCodes);
            }

            return await Task.FromResult<PostalCodeDetail>(null);
        }

        public async Task<IList<LocationNameIsoDetail>> GetLocationListDataList()
        {
            var jsonString = File.ReadAllText(FullPath);
            return await Task.FromResult(JsonSerializer.Deserialize<IList<LocationNameIsoDetail>>(jsonString));
        }
    }
}
