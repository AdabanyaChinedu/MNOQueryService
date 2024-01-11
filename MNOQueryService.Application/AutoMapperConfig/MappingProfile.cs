using AutoMapper;
using MNOQueryService.Domain.Entities;

namespace EventService.Application.AutoMapperConfig
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            this.CreateMap<Country, CountryDto>().ReverseMap();
            this.CreateMap<NetworkOperator, OperatorDto>().ReverseMap();
        }
    }
}
