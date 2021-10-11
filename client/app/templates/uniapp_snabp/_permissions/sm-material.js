import Common from './common';
const {Create, Update, Delete, Detail, Export, Dot, SetFlow, Flow, Approval, Import, Accept} = Common;

const GroupName = 'AbpMaterial';

const GroupNameSuppliers = GroupName + Dot + 'Suppliers';
const Suppliers = {
	Default: GroupNameSuppliers,
	Create: GroupNameSuppliers + Dot + Create,
	Update: GroupNameSuppliers + Dot + Update,
	Detail: GroupNameSuppliers + Dot + Detail,
	Delete: GroupNameSuppliers + Dot + Delete,
	Export: GroupNameSuppliers + Dot + Export,
};

// const GroupNameMaterials = GroupName + Dot + 'Materials';
// const Materials = {
//   Default: GroupNameMaterials,
//   Create: GroupNameMaterials + Dot + Create,
//   Update: GroupNameMaterials + Dot + Update,
//   Detail: GroupNameMaterials + Dot + Detail,
//   Delete: GroupNameMaterials + Dot + Delete,
//   Export: GroupNameMaterials + Dot + Export,
// };
const GroupNameConstructionTeam = GroupName + Dot + 'ConstructionTeams';
const ConstructionTeams = {
	Default: GroupNameConstructionTeam,
	Create: GroupNameConstructionTeam + Dot + Create,
	Update: GroupNameConstructionTeam + Dot + Update,
	Detail: GroupNameConstructionTeam + Dot + Detail,
	Delete: GroupNameConstructionTeam + Dot + Delete,
	Export: GroupNameConstructionTeam + Dot + Export,
	Import: GroupNameConstructionTeam + Dot + Import,
};
const GroupNameConstructionSections = GroupName + Dot + 'ConstructionSections';
const ConstructionSections = {
	Default: GroupNameConstructionSections,
	Create: GroupNameConstructionSections + Dot + Create,
	Update: GroupNameConstructionSections + Dot + Update,
	Detail: GroupNameConstructionSections + Dot + Detail,
	Delete: GroupNameConstructionSections + Dot + Delete,
};

const GroupNameInquires = GroupName + Dot + 'Inquires';
const Inquires = {
	Default: GroupNameInquires,
	Detail: GroupNameInquires + Dot + Detail,
	Export: GroupNameInquires + Dot + Export,
};

const GroupNameUsePlans = GroupName + Dot + 'UsePlans';
const UsePlans = {
	Default: GroupNameUsePlans,
	Create: GroupNameUsePlans + Dot + Create,
	Update: GroupNameUsePlans + Dot + Update,
	Detail: GroupNameUsePlans + Dot + Detail,
	Delete: GroupNameUsePlans + Dot + Delete,
	Export: GroupNameUsePlans + Dot + Export,
	Approval: GroupNameUsePlans + Dot + Approval,
	Flow: GroupNameUsePlans + Dot + Flow,
};

const GroupNamePurchases = GroupName + Dot + 'Purchases';
const Purchases = {
	Default: GroupNamePurchases,
	Create: GroupNamePurchases + Dot + Create,
	Update: GroupNamePurchases + Dot + Update,
	Detail: GroupNamePurchases + Dot + Detail,
	Delete: GroupNamePurchases + Dot + Delete,
	Export: GroupNamePurchases + Dot + Export,
	Approval: GroupNamePurchases + Dot + Approval,
	Flow: GroupNamePurchases + Dot + Flow,
	SetFlow: GroupNamePurchases + Dot + SetFlow,
};
const GroupNameAcceptances = GroupName + Dot + 'MaterialAcceptances';
const Acceptances = {
	Default: GroupNameAcceptances,
	Create: GroupNameAcceptances + Dot + Create,
	Update: GroupNameAcceptances + Dot + Update,
	Detail: GroupNameAcceptances + Dot + Detail,
	Delete: GroupNameAcceptances + Dot + Delete,
	Accept: GroupNameAcceptances + Dot + Accept,
	Export: GroupNameAcceptances + Dot + Export,
};
const GroupNameMaterialOfbill = GroupName + Dot + 'MaterialOfBill';
const MaterialOfBill = {
	Default: GroupNameMaterialOfbill,
	Create: GroupNameMaterialOfbill + Dot + Create,
	Update: GroupNameMaterialOfbill + Dot + Update,
	Detail: GroupNameMaterialOfbill + Dot + Detail,
	Delete: GroupNameMaterialOfbill + Dot + Delete,
	Accept: GroupNameMaterialOfbill + Dot + Accept,
	Export: GroupNameMaterialOfbill + Dot + Export,
};
const GroupNameMaterials = GroupName + Dot + 'Materials';
const Materials = {
	Default: GroupNameMaterials,
	Create: GroupNameMaterials + Dot + Create,
	Update: GroupNameMaterials + Dot + Update,
	Detail: GroupNameMaterials + Dot + Detail,
	Delete: GroupNameMaterials + Dot + Delete,
	Export: GroupNameMaterials + Dot + Export,
	ExportCode: GroupNameMaterials + Dot + 'ExportCode',
	GenerateCode: GroupNameMaterials + Dot + 'GenerateCode',
	Synchronize: GroupNameMaterials + Dot + 'Synchronize',
};
const GroupNameEntryRecords = GroupName + Dot + 'EntryRecords';
const EntryRecords = {
	Default: GroupNameEntryRecords,
	Create: GroupNameEntryRecords + Dot + Create,
	Detail: GroupNameEntryRecords + Dot + Detail,
	Export: GroupNameEntryRecords + Dot + Export,
};

const GroupNameOutRecords = GroupName + Dot + 'OutRecords';
const OutRecords = {
	Default: GroupNameOutRecords,
	Create: GroupNameOutRecords + Dot + Create,
	Detail: GroupNameOutRecords + Dot + Detail,
	Export: GroupNameOutRecords + Dot + Export,
};
const GroupNameMaterialPlans = GroupName + Dot + 'MaterialPlans';
const MaterialPlans = {
	Default: GroupNameMaterialPlans,
	GenerateMaterialPlan: GroupNameMaterialPlans + Dot + 'GenerateMaterialPlan',
};
const GroupNameInventories = GroupName + Dot + 'Inventories';
const Inventories = {
	Default: GroupNameInventories,
	Create: GroupNameInventories + Dot + Delete,
	Detail: GroupNameInventories + Dot + Detail,
	Export: GroupNameInventories + Dot + Export,
};
const GroupNamePurchasePlans = GroupName + Dot + 'PurchasePlans';
const PurchasePlans = {
	Default: GroupNamePurchasePlans,
	Create: GroupNamePurchasePlans + Dot + Create,
	Update: GroupNamePurchasePlans + Dot + Update,
	Detail: GroupNamePurchasePlans + Dot + Detail,
	Delete: GroupNamePurchasePlans + Dot + Delete,
	Export: GroupNamePurchasePlans + Dot + Export,
	Approval: GroupNamePurchasePlans + Dot + Approval,
	Flow: GroupNamePurchasePlans + Dot + Flow,
	SetFlow: GroupNamePurchasePlans + Dot + SetFlow,
};
const GroupNamePurchaseLists = GroupName + Dot + 'PurchaseLists';
const PurchaseLists = {
	Default: GroupNamePurchaseLists,
	Create: GroupNamePurchaseLists + Dot + Create,
	Update: GroupNamePurchaseLists + Dot + Update,
	Detail: GroupNamePurchaseLists + Dot + Detail,
	Delete: GroupNamePurchaseLists + Dot + Delete,
	Export: GroupNamePurchaseLists + Dot + Export,
	Approval: GroupNamePurchaseLists + Dot + Approval,
};
export default {
	GroupName,
	Acceptances,
	MaterialOfBill,
	Suppliers,
	Materials,
	ConstructionTeams,
	ConstructionSections,
	Inquires,
	UsePlans,
	Purchases,
	EntryRecords,
	OutRecords,
	Inventories,
	MaterialPlans,
	PurchaseLists,
	PurchasePlans,
};
