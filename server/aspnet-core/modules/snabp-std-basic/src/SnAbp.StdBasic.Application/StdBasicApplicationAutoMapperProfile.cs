using AutoMapper;
using SnAbp.StdBasic.Dtos;
using SnAbp.StdBasic.Dtos.Model;
using SnAbp.StdBasic.Dtos.Model.ModelMVD;
using SnAbp.StdBasic.Dtos.Repair.RepairItem;
using SnAbp.StdBasic.Entities;
using SnAbp.StdBasic.TempleteModel;

namespace SnAbp.StdBasic
{
    public class StdBasicApplicationAutoMapperProfile : Profile
    {
        public StdBasicApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            // Category
            CreateMap<ComponentCategory, ComponentCategoryDto>();
            CreateMap<ComponentCategoryDto, ComponentCategory>();
            CreateMap<ComponentCategoryRltMVDProperty, ComponentCategoryRltMVDPropertyDto>();

            CreateMap<ProductCategory, ProductCategoryDto>();
            CreateMap<ProductCategoryDto, ProductCategory>();
            CreateMap<ProductCategory, ProductCategorySimpleDto>();
            CreateMap<ProductCategorySimpleDto, ProductCategory>();
            CreateMap<ProductCategory, ProductCategoryTemplate>();
            CreateMap<ProductCategoryTemplate, ProductCategory>();
            CreateMap<ProductCategoryRltMVDProperty, ProductCategoryRltMVDPropertyDto>();

            // Manufacturer
            CreateMap<Manufacturer, ManufacturerDto>();
            CreateMap<Manufacturer, ManufacturerSimpleDto>();

            CreateMap<ManufacturerDto, Manufacturer>();
            CreateMap<ManufacturerUpdateDto, Manufacturer>();
            CreateMap<Manufacturer, ManufacturerUpdateDto>();
            CreateMap<Manufacturer, ManufacturerCreateDto>();
            CreateMap<Manufacturer, ManufactureTemplate>();
            CreateMap<ManufactureTemplate, Manufacturer>();

            CreateMap<EquipmentControlType, EquipmentControlTypeDto>();
            CreateMap<EquipmentControlType, EquipmentControlTypeSimpleDto>();
            CreateMap<EquipmentControlTypeDto, ComponentCategory>();
            CreateMap<ComponentCategory, EquipmentControlTypeCreateDto>();
            CreateMap<ComponentCategory, ComponentCategoryTemplate>();
            CreateMap<ComponentCategoryTemplate, ComponentCategory>();


            CreateMap<ModelTerminal, StandardEquipmentTerminalDto>();
            CreateMap<StandardEquipmentTerminalDto, ModelTerminal>();
            CreateMap<ModelTerminal, StandardEquipmentTerminalCreateDto>();
            CreateMap<ModelTerminal, StandardEquipmentTerminalUpdateDto>();

            CreateMap<Model, StandardEquipmentDto>();
            CreateMap<StandardEquipmentDto, Model>();
            CreateMap<Model, StandardEquipmentDetailDto>();
            CreateMap<Model, StandardEquipmentCreateDto>();
            CreateMap<Model, StandardEquipmentUpdateDto>();
            CreateMap<Model, StandardEquipmentModel>();
            CreateMap<StandardEquipmentModel, Model>();
            CreateMap<ModelFile, ModelFileDto>();
            CreateMap<ModelFileDto, ModelFile>();
            CreateMap<RevitConnector, RevitConnectorTemplate>();
            CreateMap<RevitConnectorTemplate, RevitConnector>();
            CreateMap<ModelRltMVDProperty, ModelRltMVDPropertyDto>();
            CreateMap<RevitConnector, ModelFileRltConnectorDto>();
            CreateMap<ModelFileRltConnectorDto, RevitConnector>();

            // Repair
            CreateMap<RepairItem, RepairItemDto>();
            CreateMap<RepairItem, RepairItemDetailDto>();
            CreateMap<RepairItemDto, RepairItem>();
            CreateMap<RepairItemCreateDto, RepairItem>();
            CreateMap<RepairItemUpdateDto, RepairItem>();
            CreateMap<RepairItemCreateDto, RepairItemDto>();
            CreateMap<RepairItemUpdateDto, RepairItemDto>();
            CreateMap<RepairItemRltComponentCategory, RepairItemRltComponentCategoryDto>();
            CreateMap<RepairItemRltComponentCategoryDto, RepairItemRltComponentCategory>();
            CreateMap<RepairItem, RepairItemUpdateSimpleDto>();
            CreateMap<RepairItemUpdateSimpleDto, RepairItem>();
            CreateMap<RepairTestItem, RepairTestItemDto>();
            CreateMap<RepairTestItem, RepairTestItemCreateDto>();
            CreateMap<RepairTestItem, RepairTestItemUpdateDto>();
            CreateMap<RepairTestItemDto, RepairTestItem>();
            CreateMap<RepairTestItemCreateDto, RepairTestItem>();
            CreateMap<RepairTestItemCreateDto, RepairTestItemDto>();
            CreateMap<RepairTestItemUpdateDto, RepairTestItem>();
            CreateMap<RepairTestItemSimpleDto, RepairTestItem>();
            CreateMap<RepairTestItem, RepairTestItemSimpleDto>();

            //RepairGroup
            CreateMap<RepairGroupDto, RepairGroup>();
            CreateMap<RepairGroup, RepairGroupDto>();

            CreateMap<RepairGroup, RepairGroupSimpleDto>();
            CreateMap<RepairGroupSimpleDto, RepairGroup>();
            //ComponentCategoryRltProductCategory
            CreateMap<ComponentCategoryRltProductCategory, ComponentCategoryRltProductCategoryDto>();
            CreateMap<ComponentCategoryRltProductCategoryDto, ComponentCategoryRltProductCategory>();

            //InfluenceRange
            CreateMap<InfluenceRange, InfluenceRangeDto>();
            CreateMap<InfluenceRangeDto, InfluenceRange>();
            CreateMap<InfluenceRange, InfluenceRangeCreateDto>();
            CreateMap<InfluenceRangeCreateDto, InfluenceRange>();
            CreateMap<InfluenceRange, InfluenceRangeUpdateDto>();
            CreateMap<InfluenceRangeUpdateDto, InfluenceRange>();

            //WorkAttention
            CreateMap<WorkAttention, WorkAttentionDto>();
            CreateMap<WorkAttentionDto, WorkAttention>();
            CreateMap<WorkAttention, WorkAttentionUpdateDto>();
            CreateMap<WorkAttentionUpdateDto, WorkAttention>();
            CreateMap<WorkAttention, WorkAttentionCreateDto>();
            CreateMap<WorkAttentionCreateDto, WorkAttention>();


            //MVD
            CreateMap<MVDCategory, MVDCategoryDto>();
            CreateMap<MVDCategoryDto, MVDCategory>();
            CreateMap<MVDCategoryDto, MVDCategoryExportDto>();
            CreateMap<MVDCategory, MVDCategoryTemplate>();
            CreateMap<MVDCategoryTemplate, MVDCategory>();

            CreateMap<MVDProperty, MVDPropertyDto>();
            CreateMap<MVDPropertyDto, MVDPropertyExportDto>();
            CreateMap<MVDPropertyDto, MVDProperty>();
            CreateMap<MVDProperty, MVDPropertyTemplate>();
            CreateMap<MVDPropertyTemplate, MVDProperty>();

            //ProjectItem
            CreateMap<ProjectItem, ProjectItemDto>();
            CreateMap<ProjectItemDto, ProjectItem>();
            CreateMap<ProjectItem, ProjectItemTemplate>();
            CreateMap<ProjectItemTemplate, ProjectItem>();

            CreateMap<IndividualProject, IndividualProjectDto>();
            CreateMap<IndividualProjectDto, IndividualProject>();
            CreateMap<IndividualProject, IndividualProjectTemplate>();
            CreateMap<IndividualProjectTemplate, IndividualProject>();

            CreateMap<ProcessTemplate, ProcessTemplateDto>();
            CreateMap<ProcessTemplateDto, ProcessTemplate>();
            CreateMap<ProcessTemplate, ProcessTemplateTemplate>();
            CreateMap<ProcessTemplateTemplate, ProcessTemplate>();

            CreateMap<ProjectItemRltComponentCategory, ProjectItemRltComponentCategoryDto>();
            CreateMap<ProjectItemRltComponentCategoryDto, ProjectItemRltComponentCategory>();

            CreateMap<ProjectItemRltIndividualProject, ProjectItemRltIndividualProjectDto>();
            CreateMap<ProjectItemRltIndividualProjectDto, ProjectItemRltIndividualProject>();

            CreateMap<ProjectItemRltProcessTemplate, ProjectItemRltProcessTemplateDto>();
            CreateMap<ProjectItemRltProcessTemplateDto, ProjectItemRltProcessTemplate>();

            CreateMap<ProjectItemRltProductCategory, ProjectItemRltProductCategoryDto>();
            CreateMap<ProjectItemRltProductCategoryDto, ProjectItemRltProductCategory>();

            CreateMap<BasePrice, BasePriceDto>();
            CreateMap<BasePriceDto, BasePrice>();
            CreateMap<BasePrice, BasePriceTemplate>();
            CreateMap<BasePriceTemplate, BasePrice>();

            CreateMap<ComputerCode, ComputerCodeDto>();
            CreateMap<ComputerCodeDto, ComputerCode>();
            CreateMap<ComputerCode, ComputerCodeTemplate>();
            CreateMap<ComputerCodeTemplate, ComputerCode>();

            CreateMap<Quota, QuotaDto>();
            CreateMap<QuotaDto, Quota>();
            CreateMap<Quota, QuotaTemplate>();
            CreateMap<QuotaTemplate, Quota>();

            CreateMap<QuotaItem, QuotaItemDto>();
            CreateMap<QuotaItemDto, QuotaItem>();
            CreateMap<QuotaItem, QuotaItemTemplate>();
            CreateMap<QuotaItem, QuotaItemEditDto>();
            CreateMap<QuotaItemEditDto, QuotaItem>();
            CreateMap<QuotaItemTemplate, QuotaItem>();

            CreateMap<QuotaCategory, QuotaCategoryDto>();
            CreateMap<QuotaCategoryDto, QuotaCategory>();
            CreateMap<QuotaCategory, QuotaCategoryTemplate>();
            CreateMap<QuotaCategoryTemplate, QuotaCategory>();

            CreateMap<ComponentCategoryRltMaterial, ComponentCategoryRltMaterialDto>();
            CreateMap<ComponentCategoryRltMaterialDto, ComponentCategoryRltMaterial>();

            CreateMap<ComponentCategoryRltQuota, ComponentCategoryRltQuotaDto>();
            CreateMap<ComponentCategoryRltQuotaDto, ComponentCategoryRltQuota>();

            CreateMap<ProductCategoryRltMaterial, ProductCategoryRltMaterialDto>();
            CreateMap<ProductCategoryRltMaterialDto, ProductCategoryRltMaterial>();

            CreateMap<ProductCategoryRltQuota, ProductCategoryRltQuotaDto>();
            CreateMap<ProductCategoryRltQuotaDto, ProductCategoryRltQuota>();

        }
    }
}