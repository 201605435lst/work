using AutoMapper;

using SnAbp.File;
using SnAbp.Material.Dtos;
using SnAbp.Material.Entities;

namespace SnAbp.Material
{
    public class MaterialApplicationAutoMapperProfile : Profile
    {
        public MaterialApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */


            CreateMap<Supplier, SupplierDto>();
            CreateMap<SupplierDto, Supplier>();
            CreateMap<Supplier, SupplierSimpleDto>();
            CreateMap<Supplier, SupplierUpdateDto>();
            CreateMap<Supplier, SupplierCreateDto>();

            CreateMap<SupplierRltAccessory, SupplierRltAccessoryDto>();
            CreateMap<SupplierRltAccessory, SupplierRltAccessoryCreateDto>();

            CreateMap<SupplierRltContacts, SupplierRltContactsDto>();
            CreateMap<SupplierRltContacts, SupplierRltContactsCreateDto>();
            CreateMap<SupplierRltContacts, SupplierRltContactsUpdateDto>();

            CreateMap<Partition, PartitionDto>();
            CreateMap<PartitionDto, Partition>();

            CreateMap<PartitionCreateDto, Partition>();
            CreateMap<Partition, PartitionCreateDto>();

            //入库记录
            CreateMap<EntryRecord, EntryRecordDto>();
            CreateMap<EntryRecordDto, EntryRecord>();
            CreateMap<EntryRecord, EntryRecordDetailDto>();
            CreateMap<EntryRecord, EntryRecordCreateDto>();
            CreateMap<EntryRecordCreateDto, EntryRecord>();
            CreateMap<EntryRecordRltFile, EntryRecordRltFileDto>();
            CreateMap<EntryRecordRltFile, EntryRecordRltFileCreateDto>();
            CreateMap<EntryRecordRltMaterial, EntryRecordRltMaterialDto>();
            CreateMap<EntryRecordRltMaterial, EntryRecordRltMaterialCreateDto>();
            CreateMap<EntryRecordRltQRCode, EntryRecordRltQRCodeDto>();
            CreateMap<EntryRecordRltQRCode, EntryRecordRltQRCodeDetailDto>();
            CreateMap<EntryRecordRltQRCode, EntryRecordRltQRCodeCreateDto>();

            //出库记录
            CreateMap<OutRecord, OutRecordDto>();
            CreateMap<OutRecordDto, OutRecord>();
            CreateMap<OutRecord, OutRecordCreateDto>();
            CreateMap<OutRecordCreateDto, OutRecord>();
            CreateMap<OutRecordRltFile, OutRecordRltFileDto>();
            CreateMap<OutRecordRltFile, OutRecordRltFileCreateDto>();
            CreateMap<OutRecordRltMaterial, OutRecordRltMaterialDto>();
            CreateMap<OutRecordRltMaterial, OutRecordRltMaterialCreateDto>();
            CreateMap<OutRecordRltQRCode, OutRecordRltQRCodeDto>();
            CreateMap<OutRecordRltQRCode, OutRecordRltQRCodeCreateDto>();

            //库存管理
            CreateMap<Inventory, InventoryDto>();
            CreateMap<InventoryDto, Inventory>();
            CreateMap<InventoryCreateDto, Inventory>();
            CreateMap<Inventory, InventoryCreateDto>();
            CreateMap<Inventory, InventoryDetailDto>();



            // 物资合同
            CreateMap<MaterialContractDto, Contract>();
            CreateMap<Contract, MaterialContractDto>();
            CreateMap<Contract, MaterialContractCreateDto>();
            CreateMap<MaterialContractCreateDto, Contract>();

            CreateMap<MaterialContractRltFileDto, ContractRltFile>();
            CreateMap<ContractRltFile, MaterialContractRltFileDto>();
            CreateMap<FileDomainDto, File.Entities.File>();
            CreateMap<File.Entities.File, FileDomainDto>();

            CreateMap<ConstructionSection, ConstructionSectionDto>();
            CreateMap<ConstructionSectionDto, ConstructionSection>();
            CreateMap<ConstructionSection, ConstructionSectionCreateDto>();
            CreateMap<ConstructionSectionCreateDto, ConstructionSection>();
            CreateMap<ConstructionSection, ConstructionSectionUpdateDto>();
            CreateMap<ConstructionSectionUpdateDto, ConstructionSection>();


            //采购计划
            CreateMap<PurchasePlan, PurchasePlanExportDto>();
            CreateMap<PurchasePlanDto, PurchasePlan>();
            CreateMap<PurchasePlan, PurchasePlanDto>();
            CreateMap<PurchasePlanDto, PurchasePlan>();
            CreateMap<PurchasePlan, PurchasePlanCreateDto>();
            CreateMap<PurchasePlanCreateDto, PurchasePlan>();
            CreateMap<PurchasePlan, PurchasePlanUpdateDto>();
            CreateMap<PurchasePlanUpdateDto, PurchasePlan>();

            CreateMap<PurchasePlanRltFile, PurchasePlanRltFileDto>();
            CreateMap<PurchasePlanRltFileDto,PurchasePlanRltFile>();
            CreateMap<PurchasePlanRltFile, PurchasePlanRltFileSimpleDto>();
            CreateMap<PurchasePlanRltFileSimpleDto, PurchasePlanRltFile>();
            CreateMap<PurchasePlanRltFile, PurchasePlanRltFileCreateDto>();
            CreateMap<PurchasePlanRltFileCreateDto,PurchasePlanRltFile>();

            CreateMap<PurchasePlanRltFlow, PurchasePlanRltFlowDto>();
            CreateMap<PurchasePlanRltFlowDto,PurchasePlanRltFlow>();
            CreateMap<PurchasePlanRltFlow,PurchasePlanRltFlowSimpleDto>();
            CreateMap<PurchasePlanRltFlowSimpleDto,PurchasePlanRltFlow>();
            CreateMap<PurchasePlanRltFlow, PurchasePlanFlowCreateDto>();
            CreateMap<PurchasePlanFlowCreateDto,PurchasePlanRltFlow>();

            CreateMap<PurchasePlanRltMaterial, PurchasePlanRltMaterialDto>();
            CreateMap<PurchasePlanRltMaterialDto,PurchasePlanRltMaterial>();
            CreateMap<PurchasePlanRltMaterial, PurchasePlanRltMaterialCreateDto>();
            CreateMap<PurchasePlanRltMaterialCreateDto,PurchasePlanRltMaterial>();
            CreateMap<PurchasePlanRltMaterial, PurchasePlanRltMaterialSimpleDto>();
            CreateMap<PurchasePlanRltMaterialSimpleDto, PurchasePlanRltMaterial>();

            //采购清单
            CreateMap<PurchaseList, PurchaseListDto>();
            CreateMap<PurchaseListDto, PurchaseList>();
            CreateMap<PurchaseList, PurchaseListCreateDto>();
            CreateMap<PurchaseListCreateDto, PurchaseList>();
            CreateMap<PurchaseList, PurchaseListUpdateDto>();
            CreateMap<PurchaseListUpdateDto, PurchaseList>();
            CreateMap<PurchaseList, PurchaseListExportsDto>();
            CreateMap<PurchaseListExportsDto, PurchaseList>();
            CreateMap<PurchaseListRltFile, PurchaseListRltFileDto>();
            CreateMap<PurchaseListRltFileDto, PurchaseListRltFile>();
            CreateMap<PurchaseListRltFile, PurchaseListRltFileCreateDto>();
            CreateMap<PurchaseListRltFileCreateDto, PurchaseListRltFile>();

            CreateMap<PurchaseListRltFlow, PurchaseListRltFlowDto>();
            CreateMap<PurchaseListRltFlowDto, PurchaseListRltFlow>();
            CreateMap<PurchaseListRltFlow, PurchaseListRltFlowCreateDto>();
            CreateMap<PurchaseListRltFlowCreateDto, PurchaseListRltFlow>();

            CreateMap<PurchaseListRltMaterial, PurchaseListRltMaterialDto>();
            CreateMap<PurchaseListRltMaterialDto, PurchaseListRltMaterial>();
            CreateMap<PurchaseListRltMaterial, PurchaseListRltMaterialCreateDto>();
            CreateMap<PurchaseListRltMaterialCreateDto, PurchaseListRltMaterial>();

            CreateMap<PurchaseListRltPurchasePlan, PurchaseListRltPurchasePlanDto>();
            CreateMap<PurchaseListRltPurchasePlanDto, PurchaseListRltPurchasePlan>();
            CreateMap<PurchaseListRltPurchasePlan, PurchaseListRltPurchasePlanCreateDto>();
            CreateMap<PurchaseListRltPurchasePlanCreateDto, PurchaseListRltPurchasePlan>();
            //物资验收表
            CreateMap<MaterialAcceptance, MaterialAcceptanceDto>();
            CreateMap<MaterialAcceptanceDto, MaterialAcceptance>();
            CreateMap<MaterialAcceptance, MaterialAcceptanceCreateDto>();
            CreateMap<MaterialAcceptanceCreateDto, MaterialAcceptance>();
            CreateMap<MaterialAcceptance, MaterialAcceptanceUpdateDto>();
            CreateMap<MaterialAcceptanceUpdateDto, MaterialAcceptance>();

            //物资验收关联表
            CreateMap<MaterialAcceptanceRltMaterial, MaterialAcceptanceRltMaterialDto>();
            CreateMap<MaterialAcceptanceRltMaterialDto, MaterialAcceptanceRltMaterial>();
            CreateMap<MaterialAcceptanceRltMaterial, MaterialAcceptanceRltMaterialCreateDto>();
            CreateMap<MaterialAcceptanceRltMaterialCreateDto, MaterialAcceptanceRltMaterial>();
            CreateMap<MaterialAcceptanceRltFile, MaterialAcceptanceRltFileDto>();
            CreateMap<MaterialAcceptanceRltFileDto, MaterialAcceptanceRltFile>();
            CreateMap<MaterialAcceptanceRltFile, MaterialAcceptanceRltFileCreateDto>();
            CreateMap<MaterialAcceptanceRltFileCreateDto, MaterialAcceptanceRltFile>();
            CreateMap<MaterialAcceptanceRltQRCode, MaterialAcceptanceRltQRCodeDto>();
            CreateMap<MaterialAcceptanceRltQRCodeDto, MaterialAcceptanceRltQRCode>();
            CreateMap<MaterialAcceptanceRltQRCode, MaterialAcceptanceRltQRCodeCreateDto>();
            CreateMap<MaterialAcceptanceRltQRCodeCreateDto, MaterialAcceptanceRltQRCode>();
            CreateMap<MaterialAcceptanceRltPurchase, MaterialAcceptanceRltPurchaseDto>();
            CreateMap<MaterialAcceptanceRltPurchaseDto, MaterialAcceptanceRltPurchase>();

            //领料单
            CreateMap<MaterialOfBill, MaterialOfBillDto>();
            CreateMap<MaterialOfBill, MaterialOfBillCreateDto>();
            CreateMap<MaterialOfBill, MaterialOfBillUpdateDto>();
            CreateMap<MaterialOfBillRltAccessory, MaterialOfBillRltAccessoryDto>();
            CreateMap<MaterialOfBillRltAccessory, MaterialOfBillRltAccessoryCreateDto>();
            CreateMap<MaterialOfBillRltAccessory, MaterialOfBillRltAccessoryUpdateDto>();
            CreateMap<MaterialOfBillRltMaterial, MaterialOfBillRltMaterialDto>();
            CreateMap<MaterialOfBillRltMaterial, MaterialOfBillRltMaterialCreateDto>();
            CreateMap<MaterialOfBillRltMaterial, MaterialOfBillRltMaterialUpdateDto>();

        }
    }
}