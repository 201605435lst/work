import Common from './common';
const { Create, Update, Delete, Dot, Import, Export, Detail, CreateType, UpdateOrder } = Common;

const GroupName = 'AbpStdBasic';

const GroupNameComponentCategories = GroupName + Dot + 'ComponentCategories';
const ComponentCategories = {
  Default: GroupNameComponentCategories,
  Create: GroupNameComponentCategories + Dot + Create,
  Update: GroupNameComponentCategories + Dot + Update,
  Delete: GroupNameComponentCategories + Dot + Delete,
  Detail: GroupNameComponentCategories + Dot + Detail,
  Import: GroupNameComponentCategories + Dot + Import,
  Export: GroupNameComponentCategories + Dot + Export,
};

const GroupNameProductCategories = GroupName + Dot + 'ProductCategories';
const ProductCategories = {
  Default: GroupNameProductCategories,
  Create: GroupNameProductCategories + Dot + Create,
  Update: GroupNameProductCategories + Dot + Update,
  Delete: GroupNameProductCategories + Dot + Delete,
  Detail: GroupNameProductCategories + Dot + Detail,
  Import: GroupNameProductCategories + Dot + Import,
  Export: GroupNameProductCategories + Dot + Export,
};

const GroupNameWorkAttention = GroupName + Dot + 'WorkAttention';
const WorkAttention = {
  Default: GroupNameWorkAttention,
  Create: GroupNameWorkAttention + Dot + Create,
  Update: GroupNameWorkAttention + Dot + Update,
  Delete: GroupNameWorkAttention + Dot + Delete,
  Detail: GroupNameWorkAttention + Dot + Detail,
  CreateType: GroupNameWorkAttention + Dot + CreateType,
};

const GroupNameManufactures = GroupName + Dot + 'Manufactures';
const Manufactures = {
  Default: GroupNameManufactures,
  Create: GroupNameManufactures + Dot + Create,
  Update: GroupNameManufactures + Dot + Update,
  Delete: GroupNameManufactures + Dot + Delete,
  Detail: GroupNameManufactures + Dot + Detail,
  Import: GroupNameManufactures + Dot + Import,
  Export: GroupNameManufactures + Dot + Export,
};

const GroupNameStandardEquipments = GroupName + Dot + 'StandardEquipments';
const StandardEquipments = {
  Default: GroupNameStandardEquipments,
  Create: GroupNameStandardEquipments + Dot + Create,
  Update: GroupNameStandardEquipments + Dot + Update,
  Delete: GroupNameStandardEquipments + Dot + Delete,
  Detail: GroupNameStandardEquipments + Dot + Detail,
  Import: GroupNameStandardEquipments + Dot + Import,
  Export: GroupNameStandardEquipments + Dot + Export,
};

const GroupNameTerminals = GroupName + Dot + 'Terminals';
const Terminals = {
  Default: GroupNameTerminals,
  Create: GroupNameTerminals + Dot + Create,
  Update: GroupNameTerminals + Dot + Update,
  Delete: GroupNameTerminals + Dot + Delete,
  Detail: GroupNameTerminals + Dot + Detail,
};

const GroupNameRepairGroup = GroupName + Dot + 'RepairGroup';
const RepairGroup = {
  Default: GroupNameRepairGroup,
  Create: GroupNameRepairGroup + Dot + Create,
  Update: GroupNameRepairGroup + Dot + Update,
  Delete: GroupNameRepairGroup + Dot + Delete,
  Detail: GroupNameRepairGroup + Dot + Detail,
};

const GroupNameRepairItems = GroupName + Dot + 'RepairItems';
const RepairItems = {
  Default: GroupNameRepairItems,
  Create: GroupNameRepairItems + Dot + Create,
  Update: GroupNameRepairItems + Dot + Update,
  Delete: GroupNameRepairItems + Dot + Delete,
  Detail: GroupNameRepairItems + Dot + Detail,
  CreateTagMigration: GroupNameRepairItems + Dot + 'CreateTagMigration',
};

const GroupNameRepairTestItems = GroupName + Dot + 'RepairTestItems';
const RepairTestItems = {
  Default: GroupNameRepairTestItems,
  Create: GroupNameRepairTestItems + Dot + Create,
  Update: GroupNameRepairTestItems + Dot + Update,
  Delete: GroupNameRepairTestItems + Dot + Delete,
  Detail: GroupNameRepairTestItems + Dot + Detail,
  UpdateOrder: GroupNameRepairTestItems + Dot + UpdateOrder,
  Upgrade: GroupNameRepairTestItems + Dot + 'Upgrade',
};

const GroupNameInfluenceRanges = GroupName + Dot + 'InfluenceRanges';
const InfluenceRanges = {
  Default: GroupNameInfluenceRanges,
  Create: GroupNameInfluenceRanges + Dot + Create,
  Update: GroupNameInfluenceRanges + Dot + Update,
  Delete: GroupNameInfluenceRanges + Dot + Delete,
  Detail: GroupNameInfluenceRanges + Dot + Detail,
};

const GroupNameMVDCategory = GroupName + Dot + 'MVDCategory';
const MVDCategory = {
  Default: GroupNameMVDCategory,
  Create: GroupNameMVDCategory + Dot + Create,
  Update: GroupNameMVDCategory + Dot + Update,
  Delete: GroupNameMVDCategory + Dot + Delete,
  Detail: GroupNameMVDCategory + Dot + Detail,
  Import: GroupNameMVDCategory + Dot + Import,
  Export: GroupNameMVDCategory + Dot + Export,
};

const GroupNameMVDProperty = GroupName + Dot + 'MVDProperty';
const MVDProperty = {
  Default: GroupNameMVDProperty,
  Create: GroupNameMVDProperty + Dot + Create,
  Update: GroupNameMVDProperty + Dot + Update,
  Delete: GroupNameMVDProperty + Dot + Delete,
  Detail: GroupNameMVDProperty + Dot + Detail,
  Import: GroupNameMVDProperty + Dot + Import,
  Export: GroupNameMVDProperty + Dot + Export,
};

const GroupNameProjectItems = GroupName + Dot + 'ProjectItems';
const ProjectItems = {
  Default: GroupNameProjectItems,
  Create: GroupNameProjectItems + Dot + Create,
  Update: GroupNameProjectItems + Dot + Update,
  Delete: GroupNameProjectItems + Dot + Delete,
  Detail: GroupNameProjectItems + Dot + Detail,
  Import: GroupNameProjectItems + Dot + Import,
  Export: GroupNameProjectItems + Dot + Export,
};

const GroupNameIndividualProjects = GroupName + Dot + 'IndividualProjects';
const IndividualProjects = {
  Default: GroupNameIndividualProjects,
  Create: GroupNameIndividualProjects + Dot + Create,
  Update: GroupNameIndividualProjects + Dot + Update,
  Delete: GroupNameIndividualProjects + Dot + Delete,
  Detail: GroupNameIndividualProjects + Dot + Detail,
  Import: GroupNameIndividualProjects + Dot + Import,
  Export: GroupNameIndividualProjects + Dot + Export,
};

const GroupNameProcessTemplates = GroupName + Dot + 'ProcessTemplates';
const ProcessTemplates = {
  Default: GroupNameProcessTemplates,
  Create: GroupNameProcessTemplates + Dot + Create,
  Update: GroupNameProcessTemplates + Dot + Update,
  Delete: GroupNameProcessTemplates + Dot + Delete,
  Detail: GroupNameProcessTemplates + Dot + Detail,
  Import: GroupNameProcessTemplates + Dot + Import,
  Export: GroupNameProcessTemplates + Dot + Export,
};
const GroupNameQuotaCategorys = GroupName + Dot + 'QuotaCategorys';
const QuotaCategorys = {
  Default: GroupNameQuotaCategorys ,
  Create: GroupNameQuotaCategorys  + Dot + Create,
  Update: GroupNameQuotaCategorys  + Dot + Update,
  Delete: GroupNameQuotaCategorys  + Dot + Delete,
  Detail: GroupNameQuotaCategorys  + Dot + Detail,
  Import: GroupNameQuotaCategorys  + Dot + Import,
  Export: GroupNameQuotaCategorys  + Dot + Export,
};

const GroupNameQuotas = GroupName + Dot + 'Quotas';
const Quotas = {
  Default: GroupNameQuotas ,
  Create: GroupNameQuotas  + Dot + Create,
  Update: GroupNameQuotas  + Dot + Update,
  Delete: GroupNameQuotas  + Dot + Delete,
  Detail: GroupNameQuotas  + Dot + Detail,
  Import: GroupNameQuotas  + Dot + Import,
  Export: GroupNameQuotas  + Dot + Export,
};

const GroupNameQuotaItems = GroupName + Dot + 'QuotaItems';
const QuotaItems = {
  Default: GroupNameQuotaItems ,
  Create: GroupNameQuotaItems  + Dot + Create,
  Update: GroupNameQuotaItems  + Dot + Update,
  Delete: GroupNameQuotaItems  + Dot + Delete,
  Detail: GroupNameQuotaItems  + Dot + Detail,
  Import: GroupNameQuotaItems  + Dot + Import,
  Export: GroupNameQuotaItems  + Dot + Export,
};

const GroupNameBasePrices = GroupName + Dot + 'BasePrices';
const BasePrices = {
  Default: GroupNameBasePrices ,
  Create: GroupNameBasePrices  + Dot + Create,
  Update: GroupNameBasePrices  + Dot + Update,
  Delete: GroupNameBasePrices  + Dot + Delete,
  Detail: GroupNameBasePrices  + Dot + Detail,
  Import: GroupNameBasePrices  + Dot + Import,
  Export: GroupNameBasePrices + Dot + Export,
};

const GroupNameComputerCodes = GroupName + Dot + 'ComputerCodes';
const ComputerCodes = {
  Default: GroupNameComputerCodes ,
  Create: GroupNameComputerCodes  + Dot + Create,
  Update: GroupNameComputerCodes  + Dot + Update,
  Delete: GroupNameComputerCodes  + Dot + Delete,
  Detail: GroupNameComputerCodes  + Dot + Detail,
  Import: GroupNameComputerCodes  + Dot + Import,
  Export: GroupNameComputerCodes + Dot + Export,
};

export default {
  GroupName,
  WorkAttention,
  ComponentCategories,
  ProductCategories,
  Manufactures,
  StandardEquipments,
  Terminals,
  RepairGroup,
  RepairItems,
  RepairTestItems,
  InfluenceRanges,
  MVDProperty,
  MVDCategory,
  ProjectItems,
  IndividualProjects,
  ProcessTemplates,
  QuotaCategorys,
  Quotas,
  QuotaItems,
  BasePrices,
  ComputerCodes,
};
