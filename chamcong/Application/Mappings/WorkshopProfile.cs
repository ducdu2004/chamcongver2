using AutoMapper;
using chamcong.Application.DTOs;
using chamcong.Domain.Entities;

namespace chamcong.Application.Mappings
{
    public class WorkshopProfile : Profile
    {
        public WorkshopProfile()
        {
            CreateMap<Batch, BatchDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.ProductName : ""))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MapStatus(src.Status)));

            CreateMap<Batch, AdminBatchDto>()
                .IncludeBase<Batch, BatchDto>();

            CreateMap<Bundle, BundleDto>();
        }

        private string MapStatus(int status)
        {
            return status switch
            {
                0 => "New",
                1 => "InProgress",
                2 => "Completed",
                _ => "Unknown"
            };
        }
    }
}
