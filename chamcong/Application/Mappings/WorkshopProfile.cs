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
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.ProductName : ""));

            CreateMap<Batch, AdminBatchDto>()
                .IncludeBase<Batch, BatchDto>();

            CreateMap<Bundle, BundleDto>();
        }
    }
}
