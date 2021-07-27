using AutoMapper;
using System.Collections.Generic;
using LocationAPI.Models;
using LocationAPI.Models.Phone;
using LocationAPI.Models.PostCode;

namespace LocationAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<LocationDetails, LocationDetailsViewModel>();

            CreateMap<PostalCodeDetail, PostalCodeDetailViewModel>()
                .ForMember(dest => dest.TerritoryOrCountry, source => source.MapFrom(source => source.Country));

            CreateMap<LocationNameIsoDetail, LocationNameIsoDetailViewModel>()
                .ForMember(dest => dest.TerritoryOrCountry, source => source.MapFrom(source => source.Country));

            CreateMap<NationalNumberPatternBase, NationalNumberPatternBaseViewModel>();

            CreateMap<PhoneNumberFormatBase, PhoneNumberFormatBaseViewModel>();

            CreateMap<PhoneNumberMetadata, PhoneNumberMetadataViewModel>();
        }
    }
}
