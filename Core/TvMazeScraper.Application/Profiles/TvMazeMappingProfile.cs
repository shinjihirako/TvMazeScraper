using AutoMapper;
using TvMazeScraper.Application.DTO;
using TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Application.Profiles
{
    public class TvMazeMappingProfile : Profile
    {
        public TvMazeMappingProfile()
        {
            CreateMap<CastDto, CastMember>()
                .ForMember(dest => dest.ExternalPersonId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore())       // DB assigned Guid
                .ForMember(dest => dest.ShowId, opt => opt.Ignore())   // will be set via Show relation
                .ForMember(dest => dest.Show, opt => opt.Ignore());    // avoid circular ref

            CreateMap<CastMember, CastDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalPersonId));

            CreateMap<ShowDto, Show>()
                .ForMember(dest => dest.ExternalShowId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Guid assigned by DB
                .ForMember(dest => dest.CastMembers, opt => opt.MapFrom(src => src.Cast));
              //  .ForMember(dest => dest.Premiered, static opt => opt.MapFrom(src => src.Premiered.HasValue ? DateOnly.FromDateTime(src.Premiered.Value) : default(DateOnly)));


            CreateMap<Show, ShowDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ExternalShowId))
                .ForMember(dest => dest.Cast, opt => opt.MapFrom(src => src.CastMembers));
        }
    }
}
