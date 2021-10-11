using AutoMapper;
using SnAbp.Resource.Dtos;
using SnAbp.Resource.Entities;
using SnAbp.Resource.TempleteModel;
using SnAbp.StdBasic.Entities;

namespace SnAbp.Resource
{
    public class ResourceApplicationAutoMapperProfile : Profile
    {
        public ResourceApplicationAutoMapperProfile()
        {
            CreateMap<Equipment, EquipmentDetailDto>();
            CreateMap<EquipmentDetailDto, Equipment>();
            CreateMap<Equipment, EquipmentSimpleDto>();
            CreateMap<EquipmentSimpleDto, Equipment>();
            CreateMap<Equipment, EquipmentMiniDto>();
            CreateMap<EquipmentMiniDto, Equipment>();
            CreateMap<EquipmentDto, Equipment>();
            CreateMap<Equipment, EquipmentDto>();
            CreateMap<Equipment, EquipmentModel>();
            CreateMap<EquipmentModel, Equipment>();
            CreateMap<Equipment, EquipmentOfComponentTrackDto>();
            CreateMap<EquipmentOfComponentTrackDto, Equipment>();

            CreateMap<EquipmentProperty, EquipmentPropertyDto>();
            CreateMap<EquipmentPropertyDto, EquipmentProperty>();
            CreateMap<EquipmentProperty, EquipmentPropertyCreateDto>();

            CreateMap<CableExtend, CableExtendDto>();
            CreateMap<CableExtendDto, CableExtend>();

            CreateMap<CableCore, CableCoreDto>();
            CreateMap<CableCoreDto, CableCore>();

            CreateMap<CableLocation, CableLocationDto>();
            CreateMap<CableLocationDto, CableLocation>();

            CreateMap<EquipmentRltFile, EquipmentRltFileDto>();
            CreateMap<EquipmentRltFileDto, EquipmentRltFile>();
            CreateMap<EquipmentRltFile, EquipmentRltFileCreateDto>();

            CreateMap<EquipmentRltOrganization, EquipmentRltOrganizationDto>();
            CreateMap<EquipmentRltOrganizationDto, EquipmentRltOrganization>();

            CreateMap<EquipmentGroup, EquipmentGroupDto>();
            CreateMap<EquipmentGroupDto, EquipmentGroup>();
            CreateMap<EquipmentGroup, EquipmentGroupModel>();
            CreateMap<EquipmentGroupModel, EquipmentGroup>();

            CreateMap<StoreHouse, StoreHouseDto>();
            CreateMap<StoreHouseDto, StoreHouse>();
            CreateMap<StoreHouse, StoreHouseCreateDto>();
            CreateMap<StoreHouseCreateDto, StoreHouse>();
            CreateMap<StoreHouse, StoreHouseSearchDto>();
            CreateMap<StoreHouseSearchDto, StoreHouse>();
            CreateMap<StoreHouse, StoreHouseUpdateDto>();
            CreateMap<StoreHouseUpdateDto, StoreHouse>();

            CreateMap<StoreEquipmentTransfer, StoreEquipmentTransferDto>();
            CreateMap<StoreEquipmentTransferDto, StoreEquipmentTransfer>();
            CreateMap<StoreEquipmentTransferRltEquipment, StoreEquipmentTransferRltEquipmentDto>();
            CreateMap<StoreEquipmentTransferRltEquipmentDto, StoreEquipmentTransferRltEquipment>();
            CreateMap<StoreEquipmentTestRltEquipment, StoreEquipmentTestRltEquipmentDto>();
            CreateMap<StoreEquipment, StoreEquipmentSimpleDto>();
            CreateMap<StoreEquipmentSimpleDto, StoreEquipment>();
            CreateMap<StoreEquipment, StoreEquipmentDto>();
            CreateMap<StoreEquipmentDto, StoreEquipment>();
            CreateMap<EquipmentServiceRecord, EquipmentServiceRecordMiniDto>();

            CreateMap<StoreEquipmentTest, StoreEquipmentTestDto>();
            CreateMap<StoreEquipmentTestDto, StoreEquipmentTest>();

            CreateMap<Manufacturer, StoreEquipmentManufacturerDto>();
            CreateMap<StoreEquipmentManufacturerDto, Manufacturer>();

            CreateMap<ProductCategory, StoreEquipmentProductCategoryDto>();
            CreateMap<StoreEquipmentProductCategoryDto, ProductCategory>();

            CreateMap<ComponentCategory, StoreEquipmentComponentCategoryDto>();
            CreateMap<StoreEquipmentComponentCategoryDto, ComponentCategory>();

            CreateMap<StoreHouse, StoreHouseSimpleDto>();
            CreateMap<StoreHouseSimpleDto, StoreHouse>();

            CreateMap<Terminal, TerminalDto>();
            CreateMap<TerminalDto, Terminal>();

            CreateMap<TerminalLink, TerminalLinkDto>();
            CreateMap<TerminalLinkDto, TerminalLink>();

            CreateMap<TerminalBusinessPath, TerminalBusinessPathDto>();
            CreateMap<TerminalBusinessPathDto, TerminalBusinessPath>();

            CreateMap<TerminalBusinessPathNode, TerminalBusinessPathNodeDto>();
            CreateMap<TerminalBusinessPathNodeDto, TerminalBusinessPathNode>();

            CreateMap<ComponentRltQRCode, ComponentRltQRCodeDto>();
            CreateMap<ComponentRltQRCodeDto, ComponentRltQRCode>();

            CreateMap<ComponentTrackRecord, ComponentTrackRecordDto>();
            CreateMap<ComponentTrackRecordDto, ComponentTrackRecord>();
            CreateMap<ComponentTrackRecord, ComponentTrackRecordCreateDto>();
            CreateMap<ComponentTrackRecordCreateDto, ComponentTrackRecord>();
        }
    }
}