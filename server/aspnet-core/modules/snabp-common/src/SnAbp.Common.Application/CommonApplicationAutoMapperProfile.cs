using AutoMapper;
using SnAbp.Common.Dtos;
using SnAbp.Common.Entities;

namespace SnAbp.Common
{
    public class CommonApplicationAutoMapperProfile : Profile
    {
        public CommonApplicationAutoMapperProfile()
        {
            CreateMap<Area, AreaDto>();
            CreateMap<QRCode, QRCodeDto>();
            CreateMap<QRCodeDto,QRCode >();
        }
    }
}