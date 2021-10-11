using AutoMapper;
using SnAbp.Alarm.Dtos;

namespace SnAbp.Alarm
{
    public class AlarmApplicationAutoMapperProfile : Profile
    {
        public AlarmApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<Entities.Alarm, AlarmDto>();
            CreateMap<AlarmDto, Entities.Alarm>();
        }
    }
}