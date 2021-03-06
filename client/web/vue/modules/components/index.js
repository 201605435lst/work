/* @remove-on-es-build-begin */
// this file is not used if use https://github.com/ant-design/babel-plugin-import

const ENV = process.env.NODE_ENV;
if (
  ENV !== 'production' &&
  ENV !== 'test' &&
  typeof console !== 'undefined' &&
  console.warn &&
  typeof window !== 'undefined'
) {
  console.warn(
    'You are using a whole package of SnWebModule, ' +
      'please use https://www.npmjs.com/package/babel-plugin-import to reduce app bundle size.',
  );
}
/* @remove-on-es-build-end */
import { default as version } from './version';

/* components */
import { default as ScTest } from './sn-components/sc-test';

// temp
import { default as SmNamespaceModule } from './sm-namespace/sm-namespace-module';

/* modules */

//module-basic
import { default as SmBasicStations } from './sm-basic/sm-basic-stations';
import { default as SmBasicStationSelect } from './sm-basic/sm-basic-station-select';
import { default as SmBasicRailwayTreeSelect } from './sm-basic/sm-basic-railway-tree-select';
import { default as SmBasicRailways } from './sm-basic/sm-basic-railways';
import { default as SmBasicInstallationSites } from './sm-basic/sm-basic-installation-sites';
import { default as SmBasicInstallationSiteCascader } from './sm-basic/sm-basic-installation-site-cascader';
import { default as SmBasicInstallationSiteSelect } from './sm-basic/sm-basic-installation-site-select';
import { default as SmBasicStationCascader } from './sm-basic/sm-basic-station-cascader';
import { default as SmBasicScopeCascader } from './sm-basic/sm-basic-scope-cascader';
import { default as SmBasicScopeSelect } from './sm-basic/sm-basic-scope-select';

//module-std-basic
import { default as SmStdBasicManufacturers } from './sm-std-basic/sm-std-basic-manufacturers';
import { default as SmStdBasicManufacturerSelect } from './sm-std-basic/sm-std-basic-manufacturer-select';
import { default as SmStdBasicStandardEquipments } from './sm-std-basic/sm-std-basic-standard-equipments';
import { default as SmStdBasicProductCategoryTreeSelect } from './sm-std-basic/sm-std-basic-product-category-tree-select';
import { default as SmStdBasicComponentCategoryTreeSelect } from './sm-std-basic/sm-std-basic-component-category-tree-select';
import { default as SmStdBasicRepairGroupSelect } from './sm-std-basic/sm-std-basic-repair-group-select';
import { default as SmStdBasicRepairItems } from './sm-std-basic/sm-std-basic-repair-items';
import { default as SmStdBasicProductCategory } from './sm-std-basic/sm-std-basic-product-category';
import { default as SmStdBasicComponentCategory } from './sm-std-basic/sm-std-basic-component-category';
import { default as SmStdBasicRepairGroupTreeSelect } from './sm-std-basic/sm-std-basic-repair-group-tree-select';
import { default as SmStdBasicInfluenceRanges } from './sm-std-basic/sm-std-basic-influence-ranges';
import { default as SmStdBasicWorkAttention } from './sm-std-basic/sm-std-basic-work-attention';
import { default as SmStdBasicWorkAttentionTreeSelect } from './sm-std-basic/sm-std-basic-work-attention-tree-select';
import { default as SmStdBasicMvdProperty } from './sm-std-basic/sm-std-basic-mvd-property';
import { default as SmStdBasicMvdCategory } from './sm-std-basic/sm-std-basic-mvd-category';
import { default as SmStdBasicQuotaCategory } from './sm-std-basic/sm-std-basic-quota-category';
import { default as SmStdBasicQuota } from './sm-std-basic/sm-std-basic-quota';
import { default as SmStdBasicQuotaCategoryTreeSelect } from './sm-std-basic/sm-std-basic-quota-category-tree-select';
import { default as SmStdBasicQuotaTreeSelect } from './sm-std-basic/sm-std-basic-quota-tree-select';
import { default as SmStdBasicIndividualProject } from './sm-std-basic/sm-std-basic-individual-project';
import { default as SmStdBasicProjectItem } from './sm-std-basic/sm-std-basic-project-item';
import { default as SmStdBasicProjectItemListSelect } from './sm-std-basic/sm-std-basic-project-item-list-select';
import { default as SmStdBasicIndividualProjectTreeSelect } from './sm-std-basic/sm-std-basic-individual-project-tree-select';
import { default as SmStdBasicProcessTemplate } from './sm-std-basic/sm-std-basic-process-template';
import { default as SmStdBasicComputerCode } from './sm-std-basic/sm-std-basic-computer-code';
import { default as SmStdBasicComputerCodeSelect } from './sm-std-basic/sm-std-basic-computer-code-select';
import { default as SmStdBasicBasePriceSelect } from './sm-std-basic/sm-std-basic-base-price-select';
import { default as SmStdBasicProcessTemplateTreeSelect } from './sm-std-basic/sm-std-basic-process-template-tree-select';
import { default as SmStdBasicMvdPropertyTreeSelect } from './sm-std-basic/sm-std-basic-mvd-property-tree-select';

/* module-bpm ??????????????? */
import { default as SmBpmFormDesign } from './sm-bpm/sm-bpm-form-design';
import { default as SmBpmFlowDesign } from './sm-bpm/sm-bpm-flow-design';
import { default as SmBpmWorkflowTemplate } from './sm-bpm/sm-bpm-workflow-template';
import { default as SmBpmWorkflowTemplates } from './sm-bpm/sm-bpm-workflow-templates';
import { default as SmBpmWorkflows } from './sm-bpm/sm-bpm-workflows';
import { default as SmBpmChartStatistics } from './sm-bpm/sm-bpm-chart-statistics';
import { default as SmBpmWorkflowView } from './sm-bpm/sm-bpm-workflow-view';
import { default as SmBpmWorkflowTemplatesSelect } from './sm-bpm/sm-bpm-workflow-templates-select';

// ????????????
import { default as SmCostmanagementPeopleCost } from './sm-costmanagement/sm-costmanagement-people-cost';
import { default as SmCostmanagementOtherCost } from './sm-costmanagement/sm-costmanagement-other-cost';
import { default as SmCostmanagementMoneyList } from './sm-costmanagement/sm-costmanagement-money-list';
import { default as SmCostmanagementCapitalReport } from './sm-costmanagement/sm-costmanagement-capital-report';
import { default as SmCostmanagementContract } from './sm-costmanagement/sm-costmanagement-contract';
import { default as SmCostmanagementBreakevenAnalysis } from './sm-costmanagement/sm-costmanagement-breakeven-analysis';

/* module-cms ???????????????????????? */
import { default as SmCmsCategories } from './sm-cms/sm-cms-categories';
import { default as SmCmsCategory } from './sm-cms/sm-cms-category';
import { default as SmCmsArticles } from './sm-cms/sm-cms-articles';
import { default as SmCmsArticle } from './sm-cms/sm-cms-article';
import { default as SmCmsCategoryRltArticles } from './sm-cms/sm-cms-category-rlt-articles';

/* module-cr-plan ???????????????????????? */
import { default as SmCrPlanOtherPlans } from './sm-cr-plan/sm-cr-plan-other-plans';
import { default as SmCrPlanOtherPlan } from './sm-cr-plan/sm-cr-plan-other-plan';
import { default as SmCrPlanSkylightPlans } from './sm-cr-plan/sm-cr-plan-skylight-plans';
import { default as SmCrPlanSkylightPlan } from './sm-cr-plan/sm-cr-plan-skylight-plan';
import { default as SmCrPlanPlanChanges } from './sm-cr-plan/sm-cr-plan-plan-changes';
import { default as SmCrPlanPlanChange } from './sm-cr-plan/sm-cr-plan-plan-change';
import { default as SmCrPlanAddSelectablePlanModal } from './sm-cr-plan/sm-cr-plan-add-selectable-plan-modal';
import { default as SmCrPlanPlanTodos } from './sm-cr-plan/sm-cr-plan-plan-todos';
import { default as SmCrPlanSentedWorkOrders } from './sm-cr-plan/sm-cr-plan-sented-work-orders';
import { default as SmCrPlanSendingWorks } from './sm-cr-plan/sm-cr-plan-sending-works';
import { default as SmCrPlanSendingWork } from './sm-cr-plan/sm-cr-plan-sending-work';
import { default as SmCrPlanSendingWorksFinished } from './sm-cr-plan/sm-cr-plan-sending-works-finished';
import { default as SmCrPlanOtherWorks } from './sm-cr-plan/sm-cr-plan-other-works';
import { default as SmCrPlanMaintenanceRecords } from './sm-cr-plan/sm-cr-plan-maintenance-records';
import { default as SmCrPlanMaintenanceRecord } from './sm-cr-plan/sm-cr-plan-maintenance-record';
import { default as SmCrPlanMaintenanceWork } from './sm-cr-plan/sm-cr-plan-maintenance-work';
import { default as SmCrPlanMaintenanceWorkCheck } from './sm-cr-plan/sm-cr-plan-maintenance-work-check';
import { default as SmCrPlanYearMonthAlterRecord } from './sm-cr-plan/sm-cr-plan-year-month-alter-record';
import { default as SmCrPlanCooperateWork } from './sm-cr-plan/sm-cr-plan-cooperate-work';

/* module-cr-plan ???????????? */
import { default as SmCrPlanMonthPlanRecords } from './sm-cr-plan/sm-cr-plan-month-plan-records';
/* module-cr-plan ???????????? */
import { default as SmCrPlanYearPlanRecords } from './sm-cr-plan/sm-cr-plan-year-plan-records';
/* module-cr-plan ???????????? */
import { default as SmCrPlanYearPlan } from './sm-cr-plan/sm-cr-plan-year-plan';
/* module-cr-plan ???????????? */
import { default as SmCrPlanMonthPlan } from './sm-cr-plan/sm-cr-plan-month-plan';
/* module-cr-plan ?????????????????? */
import { default as SmCrPlanMonthOfYearPlan } from './sm-cr-plan/sm-cr-plan-month-of-year-plan';
/* module-cr-plan ????????????????????? */
import { default as SmCrPlanYearMonthChange } from './sm-cr-plan/sm-cr-plan-year-month-change';

/* module-cr-statistics ????????????    */
import { default as SmCrStatisticsDashboard } from './sm-cr-statistics/sm-cr-statistics-dashboard';
import { default as SmCrStatisticsPlanState } from './sm-cr-statistics/sm-cr-statistics-plan-state';
import { default as SmCrStatisticsPlanTrack } from './sm-cr-statistics/sm-cr-statistics-plan-track';

/* module-dashboard ???????????? */
import { default as SmDashboard } from './sm-dashboard/sm-dashboard';
import { default as SmDashboardListCard } from './sm-dashboard/sm-dashboard-list-card';

/* module-file ??????????????????2 */
import { default as SmFileManage } from './sm-file/sm-file-manage';
import { default as SmFileMigration } from './sm-file/sm-file-migration';
import { default as SmFileOss } from './sm-file/sm-file-oss';
import { default as SmFileManageModal } from './sm-file/sm-file-manage-modal';
import { default as SmFileManageSelect } from './sm-file/sm-file-manage-select';
import { default as SmFileTextEditor } from './sm-file/sm-file-text-editor';
import { default as SmFileUpload } from './sm-file/sm-file-upload';

/* module-problem ??????????????? */
import { default as SmProblemProblemCategories } from './sm-problem/sm-problem-problem-categories';
import { default as SmProblemProblemCategoryTreeSelect } from './sm-problem/sm-problem-problem-category-tree-select';
import { default as SmProblemProblems } from './sm-problem/sm-problem-problems';

/* module-resource ?????????????????? */
import { default as SmResourceEquipments } from './sm-resource/sm-resource-equipments';
import { default as SmResourceStoreHouse } from './sm-resource/sm-resource-store-house';
import { default as SmResourceStoreImportAltExport } from './sm-resource/sm-resource-store-import-alt-export';
import { default as SmResourceStoreEquipmentTransfer } from './sm-resource/sm-resource-store-equipment-transfer';
import { default as SmResourceStoreEquipments } from './sm-resource/sm-resource-store-equipments';
import { default as SmResourceStoreEquipmentsTests } from './sm-resource/sm-resource-store-equipments-tests';
import { default as SmResourceCodeGeneration } from './sm-resource/sm-resource-code-generation';
import { default as SmResourceStoreEquipmentsTest } from './sm-resource/sm-resource-store-equipments-test';
import { default as SmResourceStoreEquipmentsSelect } from './sm-resource/sm-resource-store-equipments-select';
import { default as SmResourceStoreEquipmentRecord } from './sm-resource/sm-resource-store-equipment-record';
import { default as SmResourceEquipmentSelect } from './sm-resource/sm-resource-equipment-select';
import { default as SmResourceEquipmentGroup } from './sm-resource/sm-resource-equipment-group';
import { default as SmResourceEquipmentGroupTreeSelect } from './sm-resource/sm-resource-equipment-group-tree-select';
import { default as SmResourceStoreHouseTreeSelect } from './sm-resource/sm-resource-store-house-tree-select';

/* module-system  ??????????????????    */
import { default as SmSystemMemberModal } from './sm-system/sm-system-member-modal';
import { default as SmSystemMemberSelect } from './sm-system/sm-system-member-select';
import { default as SmSystemUsers } from './sm-system/sm-system-users';
import { default as SmSystemUserSelect } from './sm-system/sm-system-user-select';
import { default as SmSystemUserChangePassword } from './sm-system/sm-system-user-change-password';
import { default as SmSystemOrganizationUserSelect } from './sm-system/sm-system-organization-user-select';
import { default as SmSystemRoles } from './sm-system/sm-system-roles';
import { default as SmSystemOrganizations } from './sm-system/sm-system-organizations';
import { default as SmSystemOrganizationTree } from './sm-system/sm-system-organization-tree';
import { default as SmSystemOrganizationTreeSelect } from './sm-system/sm-system-organization-tree-select';
import { default as SmSystemDataDictionaryTreeSelect } from './sm-system/sm-system-data-dictionary-tree-select';
import { default as SmSystemDataDictionaries } from './sm-system/sm-system-data-dictionaries';

/* module-oa   OA ??????  */
import { default as SmOaDutySchedule } from './sm-oa/sm-oa-duty-schedule';
import { default as SmOaContract } from './sm-oa/sm-oa-contract';
import { default as SmOaContracts } from './sm-oa/sm-oa-contracts';
import { default as SmOaSeals } from './sm-oa/sm-oa-seals';
import { default as SmOaSealsSelectModal } from './sm-oa/sm-oa-seals-select';

/* module-report   Report ??????  */
import { default as SmReports } from './sm-report/sm-reports';
import { default as SmReport } from './sm-report/sm-report';

/* module-exam   ????????????????????????  */
import { default as SmExamQuestion } from './sm-exam/sm-exam-question';
// import { default as SmExamCategories } from './sm-exam/sm-exam-categories'

/*module-exam ??????????????????*/

import { default as SmExamExamPaperTemplate } from './sm-exam/sm-exam-exam-paper-template';
import { default as SmExamKnowledgePoints } from './sm-exam/sm-exam-knowledge-points';
import { default as SmExamCategories } from './sm-exam/sm-exam-categories';
import { default as SmExamPapers } from './sm-exam/sm-exam-papers';

/*module-emerg ??????????????????????????????*/
import { default as SmEmergFault } from './sm-emerg/sm-emerg-fault';
import { default as SmEmergFaults } from './sm-emerg/sm-emerg-faults';
import { default as SmEmergPlans } from './sm-emerg/sm-emerg-plans';
import { default as SmEmergPlan } from './sm-emerg/sm-emerg-plan';

/*module-area ??????????????????*/
import { default as SmArea } from './sm-common/sm-area-module';
import { default as SmCommonSelect } from './sm-common/sm-common-select';

/*module-import ??????????????????*/
import { default as SmImport } from './sm-import/sm-import-basic';
import { default as SmImportModal } from './sm-import/sm-import-modal';

/*module-import ?????????????????? */
import { default as SmImportTemplate } from './sm-common/sm-import-template-module';
import { default as SmMessage } from './sm-message/sm-message-base';
import { default as SmMessageNotice } from './sm-message/sm-message-notice';
import { default as SmMessageCenter } from './sm-message/sm-message-center';

/*module-export ?????????????????? */
import { default as SmExport } from './sm-common/sm-export-module';
import { default as SmQrcode } from './sm-common/sm-qrcode'; // ?????????
import { default as SmQrcodeConfig } from './sm-common/sm-qrcode-config'; // ?????????
/*SmD3*/

import { default as SmD3 } from './sm-d3/sm-d3';
import { default as SmD3ScopeEquipments } from './sm-d3/sm-d3-scope-equipments';
import { default as SmD3Equipments } from './sm-d3/sm-d3-equipments';
import { default as SmD3EquipmentInfo } from './sm-d3/sm-d3-equipment-info';
import { default as SmD3EquipmentInfoFiles } from './sm-d3/sm-d3-equipment-info-files';
import { default as SmD3EquipmentInfoProperties } from './sm-d3/sm-d3-equipment-info-properties';
import { default as SmD3TerminalLink } from './sm-d3/sm-d3-terminal-link';
import { default as SmD3EmergFaults } from './sm-d3/sm-d3-emerg-faults';
import { default as SmD3CableCores } from './sm-d3/sm-d3-cable-cores';
import { default as SmD3Alarms } from './sm-d3/sm-d3-alarms';
import { default as SmD3StationSlider } from './sm-d3/sm-d3-station-slider';
import { default as SmD3Layers } from './sm-d3/sm-d3-layers';
import { default as SmD3Interface } from './sm-d3/sm-d3-interface';
import { default as SmD3ConstructionProgress } from './sm-d3/sm-d3-construction-progress';
import { default as SmD3ModalSelect } from './sm-d3/sm-d3-modal-select';

// ????????????
import { default as SmProjectProject } from './sm-project/sm-project-project';
import { default as SmProjectProjectSelect } from './sm-project/sm-project-project-select';
import { default as SmProjectProjects } from './sm-project/sm-project-projects';
import { default as SmProjectArchives } from './sm-project/sm-project-archives';
import { default as SmProjectDossier } from './sm-project/sm-project-dossier';
import { default as SmProjectDossierCatrgotyTreeSelect } from './sm-project/sm-project-dossier-catrgoty-tree-select';
import { default as SmProjectArchivesCatrgotyTreeSelect } from './sm-project/sm-project-archives-catrgoty-tree-select';
import { default as SmProjectActionTreeSelect } from './sm-project/sm-project-action-tree-select';
import { default as SmProjectUploadModal } from './sm-project/sm-project-upload-modal';

import { default as SmTaskTasks } from './sm-task/sm-task-tasks';
import { default as SmTaskTask } from './sm-task/sm-task-task';
import { default as SmTaskCalendars } from './sm-task/sm-task-calendars';
import { default as SmTaskCalendar } from './sm-task/sm-task-calendar';

import { default as SnMapAmap } from './sn-components/sn-map-amap';
// ?????????
import { default as PlayerBar } from './sn-components/sc-player-bar';
// sc?????????
import { default as ScGantt } from './sn-components/sc-gantt';
import { default as SmMapControl } from './sm-common/sm-map-control';

// ????????????
import { default as SmAlarms } from './sm-alarm/sm-alarms';

//????????????????????????
import { default as SmSafeProblem } from './sm-safe/sm-safe-problem';
import { default as SmSafeProblemLibrary } from './sm-safe/sm-safe-problem-library';
import { default as SmSafeSpeechVideo } from './sm-safe/sm-safe-speech-video';
import { default as SmSafeRltQualityModalSelect } from './sm-safe/sm-safe-rlt-quality-modal-select';

//????????????????????????
import { default as SmQualityProblems } from './sm-quality/sm-quality-problems';
import { default as SmQualityProblemLibraries } from './sm-quality/sm-quality-problem-libraries';

//????????????
import { default as SmRegulationInstitution } from './sm-regulation_/sm-regulation-institution';
import { default as SmRegulationLabelTreeSelect } from './sm-regulation_/sm-regulation-label-tree-select';
import { default as SmRegulationViewInstitution } from './sm-regulation_/sm-regulation-view-institution';
import { default as SmRegulationAuditedInstitution } from './sm-regulation_/sm-regulation-audited-institution';

// ????????????
import { default as SmVideo } from './sm-common/sm-video';
import { default as SmFlowBase } from './sm-common/sm-flow-base';
// ?????????
import { default as SmGantt } from './sm-common/sm-gantt';

/* module-material ?????????????????? */
import { default as SmMaterialSuppliers } from './sm-material/sm-material-suppliers';
import { default as SmMaterialSupplier } from './sm-material/sm-material-supplier';
import { default as SmMaterialMaterialSelectModal } from './sm-material/sm-material-material-select-modal';

import { default as SmSchedules } from './sm-schedule/sm-schedule-schedules';
import { default as SmSchedule } from './sm-schedule/sm-schedule-schedule';
import { default as SmSchedulesSelect } from './sm-schedule/sm-schedule-schedules-select';
import { default as SmApprovals } from './sm-schedule/sm-schedule-approvals';
import { default as SmApproval } from './sm-schedule/sm-schedule-approval';
import { default as SmApprovalTable } from './sm-schedule/sm-schedule-approval-table';
import { default as SmScheduleDiary } from './sm-schedule/sm-schedule-diary';
import { default as SmScheduleDiarys } from './sm-schedule/sm-schedule-diarys';
import { default as SmScheduleDiaryLog } from './sm-schedule/sm-schedule-diary-log';
import { default as SmScheduleDiarysStatistics } from './sm-schedule/sm-schedule-diarys-statistics';

// ????????????
// ????????????
import { default as SmMaterialPartital } from './sm-material/sm-material-partital';
import { default as SmMaterialPartitalTreeSelect } from './sm-material/sm-material-partital-tree-select';
import { default as SmMaterialMaterial } from './sm-material/sm-material-material';
import { default as SmMaterialMaterialTypeSelect } from './sm-material/sm-material-material-type-select';
import { default as SmMaterialMaterialSelect } from './sm-material/sm-material-material-select';
import { default as SmMaterialInventory } from './sm-material/sm-material-inventory';
import { default as SmMaterialEntryRecords } from './sm-material/sm-material-entry-records';
import { default as SmMaterialOutRecords } from './sm-material/sm-material-out-records';
import { default as SmMaterialPurchase } from './sm-material/sm-material-purchase';
import { default as SmMaterialContract } from './sm-material/sm-material-contract';
import { default as SmMaterialConstructionSection } from './sm-material/sm-material-construction-section';
import { default as SmMaterialConstructionTeam } from './sm-material/sm-material-construction-team';
import { default as SmMaterialInquire } from './sm-material/sm-material-inquire';
import { default as SmMaterialMaterialPlan } from './sm-material/sm-material-material-plan';
import { default as SmMaterialGapAnalysis } from './sm-material/sm-material-gap-analysis';
import { default as SmMaterialAcceptance } from './sm-material/sm-material-acceptance';
import { default as SmMaterialPurchaseList } from './sm-material/sm-material-purchase-list';
import { default as SmMaterialPurchaseListSelect } from './sm-material/sm-material-purchase-list-select';
import { default as SmMaterialPurchasePlan } from './sm-material/sm-material-purchase-plan';
import { default as SmMaterialMaterialPlanSelect } from './sm-material/sm-material-material-plan-select';
import { default as SmMaterialPurchasePlanSelect } from './sm-material/sm-material-purchase-plan-select';
// ????????????
import { default as SmTechnologyDisclose } from './sm-technology/sm-technology-disclose'; // ????????????
import { default as SmTechnologyInterfaceFlag } from './sm-technology/sm-technology-interface-flag'; //
import { default as SmTechnologyInterfaceReport } from './sm-technology/sm-technology-interface-report'; //
import { default as SmTechnologyInterfaceListing } from './sm-technology/sm-technology-interface-listing'; //
import { default as SmTechnologyQuantity } from './sm-technology/sm-technology-quantity'; //
import { default as SmTechnologyMaterialPlan } from './sm-technology/sm-technology-material-plan'; //

// ???????????????
import { default as SmMaterialMaterialOfbill } from './sm-material/sm-material-material-ofbill';

// ????????????????????????
// ????????????
import { default as SmConstructionBaseWorker } from './sm-construction-base/sm-construction-base-worker';
// ??????????????????
import { default as SmConstructionBaseEquipmentTeam } from './sm-construction-base/sm-construction-base-equipment-team';
// ?????????????????????
import { default as SmConstructionBaseMaterial } from './sm-construction-base/sm-construction-base-material';
// ??????????????????
import { default as SmConstructionBaseProcedure } from './sm-construction-base/sm-construction-base-procedure';
// ??????????????????
import { default as SmConstructionBaseSubItem } from './sm-construction-base/sm-construction-base-sub-item';
// ?????????????????? ?????????
import { default as SmConstructionBaseSubItemSelect } from './sm-construction-base/sm-construction-base-sub-item-select';
// ?????????????????? ?????????
import { default as SmConstructionBaseSubItemSelectTree } from './sm-construction-base/sm-construction-base-sub-item-select-tree';
// ??????????????????
import { default as SmConstructionBaseDrawSubItem } from './sm-construction-base/sm-construction-base-draw-sub-item';
// ??????????????????
import { default as SmConstructionBaseProcedureRltSub } from './sm-construction-base/sm-construction-base-procedure-rlt-sub';
// ????????????
import { default as SmConstructionBaseStandard } from './sm-construction-base/sm-construction-base-standard';
// ???????????? ?????????
import { default as SmConstructionBaseStandardSelect } from './sm-construction-base/sm-construction-base-standard-select';
// ????????????
import { default as SmConstructionBaseSection } from './sm-construction-base/sm-construction-base-section';
// ???????????? ?????????
import { default as SmConstructionBaseSectionSelect } from './sm-construction-base/sm-construction-base-section-select';
// ????????????
import { default as SmConstructionMasterPlan } from './sm-construction/sm-construction-master-plan';
// ???????????????????????????
import { default as SmConstructionMasterPlanContentSelect } from './sm-construction/sm-construction-master-plan-content-select';
// ???????????????????????????
import { default as SmConstructionPlanContentSelect } from './sm-construction/sm-construction-plan-content-select';
// ?????????????????????????????????
import { default as SmConstructionPlanContentSelectModal } from './sm-construction/sm-construction-plan-content-select-modal';
// ????????????
import { default as SmConstructionPlan } from './sm-construction/sm-construction-plan';
// ??????????????????
import { default as SmConstructionPlanContent } from './sm-construction/sm-construction-plan-content';
// ?????????????????? ?????? gantt???
import { default as SmConstructionMasterPlanContentWithGantt } from './sm-construction/sm-construction-master-plan-content-with-gantt';
// ?????????????????? ?????? gantt???
import { default as SmConstructionPlanWithGantt } from './sm-construction/sm-construction-plan-with-gantt';
// ????????????
import { default as SmComponentQrCode } from './sm-componenttrack/sm-component-qr-code';
//?????????????????????
import { default as SmConstructionPlanMaterial } from './sm-construction/sm-construction-plan-material';
import { default as SmComponentQrCodeGenerate } from './sm-componenttrack/sm-component-qr-code-generate';
// ?????????????????????
import { default as SmConstructionDispatchTemplate } from './sm-construction/sm-construction-dispatch-template';
// ?????????????????????
import { default as SmConstructionDispatchTemplateSelect } from './sm-construction/sm-construction-dispatch-template-select';
// ????????????
import { default as SmConstructionDispatch } from './sm-construction/sm-construction-dispatch';
import { default as SmConstructionDispatchs } from './sm-construction/sm-construction-dispatchs';
import { default as SmConstructionDaily } from './sm-construction/sm-construction-daily';
import { default as SmConstructionDailys } from './sm-construction/sm-construction-dailys';
import { default as SmConstructionDispatchSelect } from './sm-construction/sm-construction-dispatch-select';
import { default as SmConstructionDailyView } from './sm-construction/sm-construction-daily-view';
import { default as SmConstructionDailyTemplate } from './sm-construction/sm-construction-daily-template';

const components = [
  /* ???????????? */
  SmComponentQrCodeGenerate,
  SmComponentQrCode,
  ScTest,

  SmNamespaceModule,

  SmBasicRailways,
  SmBasicRailwayTreeSelect,
  SmBasicStations,
  SmBasicStationSelect,
  SmBasicStationCascader,
  SmBasicInstallationSites,
  SmBasicInstallationSiteCascader,
  SmBasicInstallationSiteSelect,
  SmBasicScopeCascader,
  SmBasicScopeSelect,

  SmStdBasicManufacturers,
  SmStdBasicManufacturerSelect,
  SmStdBasicStandardEquipments,
  SmStdBasicProductCategoryTreeSelect,
  SmStdBasicComponentCategoryTreeSelect,
  SmStdBasicRepairGroupSelect,
  SmStdBasicRepairGroupTreeSelect,
  SmStdBasicInfluenceRanges,
  SmStdBasicWorkAttention,
  SmStdBasicWorkAttentionTreeSelect,
  SmStdBasicRepairItems,
  SmStdBasicProductCategory,
  SmStdBasicComponentCategory,
  SmStdBasicMvdCategory,
  SmStdBasicMvdProperty,
  SmStdBasicQuotaCategory,
  SmStdBasicQuota,
  SmStdBasicComputerCodeSelect,
  SmStdBasicBasePriceSelect,
  SmStdBasicQuotaCategoryTreeSelect,
  SmStdBasicQuotaTreeSelect,
  SmStdBasicIndividualProject,
  SmStdBasicProjectItem,
  SmStdBasicProjectItemListSelect,
  SmStdBasicComputerCode,
  SmBpmFormDesign,
  SmBpmFlowDesign,
  SmBpmWorkflowTemplate,
  SmBpmWorkflowTemplates,
  SmBpmWorkflows,
  SmBpmChartStatistics,
  SmBpmWorkflowView,
  SmBpmWorkflowTemplatesSelect,
  SmStdBasicIndividualProjectTreeSelect,
  SmStdBasicProcessTemplate,
  SmStdBasicProcessTemplateTreeSelect,
  SmStdBasicMvdPropertyTreeSelect,

  //????????????
  SmCostmanagementPeopleCost,
  SmCostmanagementOtherCost,
  SmCostmanagementMoneyList,
  SmCostmanagementCapitalReport,
  SmCostmanagementContract,
  SmCostmanagementBreakevenAnalysis,

  SmCmsCategories,
  SmCmsCategory,
  SmCmsArticles,
  SmCmsArticle,
  SmCmsCategoryRltArticles,

  SmCrPlanOtherPlans,
  SmCrPlanOtherPlan,
  SmCrPlanSkylightPlans,
  SmCrPlanSkylightPlan,
  SmCrPlanMonthPlanRecords,
  SmCrPlanYearPlanRecords,
  SmCrPlanYearPlan,
  SmCrPlanMonthPlan,
  SmCrPlanPlanChanges,
  SmCrPlanPlanChange,
  SmCrPlanAddSelectablePlanModal,
  SmCrPlanPlanChanges,
  SmCrPlanPlanChange,
  SmCrPlanPlanTodos,
  SmCrPlanSentedWorkOrders,
  SmCrPlanSendingWorks,
  SmCrPlanSendingWork,
  SmCrPlanSendingWorksFinished,
  SmCrPlanMonthOfYearPlan,
  SmCrPlanYearMonthChange,
  SmCrPlanOtherWorks,
  SmCrPlanMaintenanceRecords,
  SmCrPlanMaintenanceRecord,
  SmCrPlanMaintenanceWork,
  SmCrPlanMaintenanceWorkCheck,
  SmCrPlanYearMonthAlterRecord,
  SmCrPlanCooperateWork,
  SmCrStatisticsDashboard,
  SmCrStatisticsPlanState,
  SmCrStatisticsPlanTrack,

  /* module-dashboard ???????????? */
  SmDashboard,
  SmDashboardListCard,

  SmProblemProblemCategories,
  SmProblemProblemCategoryTreeSelect,
  SmProblemProblems,

  SmResourceEquipments,
  SmResourceStoreHouse,
  SmResourceStoreImportAltExport,
  SmResourceStoreEquipmentTransfer,
  SmResourceStoreEquipments,
  SmResourceStoreEquipmentsTests,
  SmResourceCodeGeneration,
  SmResourceStoreEquipmentsTest,
  SmResourceStoreEquipmentsSelect,
  SmResourceStoreEquipmentRecord,
  SmResourceEquipmentSelect,
  SmResourceEquipmentGroup,
  SmResourceEquipmentGroupTreeSelect,
  SmResourceStoreHouseTreeSelect,

  SmSystemMemberModal,
  SmSystemMemberSelect,
  SmSystemUsers,
  SmSystemUserSelect,
  SmSystemUserChangePassword,
  SmSystemOrganizationUserSelect,
  SmSystemRoles,
  SmSystemOrganizations,
  SmSystemOrganizationTree,
  SmSystemOrganizationTreeSelect,
  SmSystemDataDictionaryTreeSelect,
  SmSystemDataDictionaries,

  SmOaDutySchedule,
  SmOaContract,
  SmOaContracts,
  SmOaSeals,
  SmOaSealsSelectModal,

  // Report
  SmReports,
  SmReport,

  SmFileManage,
  SmFileMigration,
  SmFileOss,
  SmFileManageModal,
  SmFileManageSelect,
  SmFileTextEditor,
  SmFileUpload,

  SmExamQuestion,
  // SmExamCategories,
  SmExamKnowledgePoints,
  SmExamExamPaperTemplate,
  SmExamCategories,
  SmExamPapers,

  SmEmergFault,
  SmEmergFaults,

  SmEmergPlans,
  SmEmergPlan,
  SmArea,
  SmCommonSelect,
  SmImport,
  SmImportModal,
  SmImportTemplate,
  SmExport,
  SmQrcode,
  SmQrcodeConfig,
  SmMessage,
  SmMessageNotice,
  SmMessageCenter,

  //D3??????
  SmD3,
  SmD3ScopeEquipments,
  SmBasicScopeSelect,
  SmD3Equipments,
  SmD3EquipmentInfo,
  SmD3EquipmentInfoFiles,
  SmD3EquipmentInfoProperties,
  SmD3TerminalLink,
  SmD3EmergFaults,
  SmD3CableCores,
  SmD3Alarms,
  SmD3StationSlider,
  SmD3Layers,
  SmD3Interface,
  SmD3ConstructionProgress,
  //????????????
  SmProjectProject,
  SmProjectProjectSelect,
  SmProjectProjects,
  SmProjectArchives,
  SmProjectDossier,
  SmProjectDossierCatrgotyTreeSelect,
  SmProjectArchivesCatrgotyTreeSelect,
  SmProjectActionTreeSelect,
  SmProjectUploadModal,

  SmTaskCalendars,
  SmTaskCalendar,
  SnMapAmap,
  //?????????
  PlayerBar,
  ScGantt,
  SmMapControl,
  SmTaskTasks,
  SmTaskTask,

  // ????????????
  SmAlarms,

  //????????????????????????
  SmSafeProblem,
  SmSafeProblemLibrary,
  SmSafeSpeechVideo,
  SmSafeRltQualityModalSelect,

  //????????????????????????
  SmQualityProblems,
  SmQualityProblemLibraries,

  //????????????
  SmRegulationInstitution,
  SmRegulationLabelTreeSelect,
  SmRegulationViewInstitution,
  SmRegulationAuditedInstitution,

  // ????????????
  SmVideo,
  SmFlowBase,

  // ?????????
  SmGantt,

  //????????????
  SmMaterialSuppliers,
  SmMaterialSupplier,
  SmMaterialPartital,
  SmMaterialPartitalTreeSelect,
  SmMaterialMaterial,
  SmMaterialMaterialTypeSelect,
  SmMaterialMaterialSelect,
  SmMaterialInventory,
  SmMaterialEntryRecords,
  SmMaterialOutRecords,
  SmMaterialPurchase,
  SmMaterialContract,
  SmMaterialConstructionSection,
  SmMaterialConstructionTeam,
  SmMaterialInquire,
  SmMaterialMaterialPlan,
  SmMaterialGapAnalysis,
  SmMaterialAcceptance,
  SmMaterialPurchaseList,
  SmMaterialPurchaseListSelect,
  SmMaterialPurchasePlan,
  SmMaterialMaterialPlanSelect,
  SmMaterialPurchasePlanSelect,
  SmMaterialMaterialSelectModal,

  //????????????
  SmSchedules,
  SmSchedule,
  SmSchedulesSelect,
  SmApprovals,
  SmApproval,
  SmApprovalTable,
  SmScheduleDiarys,
  SmScheduleDiary,
  SmScheduleDiaryLog,
  SmScheduleDiarysStatistics,

  // ????????????
  SmTechnologyDisclose,
  SmTechnologyInterfaceListing,
  SmTechnologyInterfaceFlag,
  SmTechnologyInterfaceReport,
  SmTechnologyQuantity,
  SmTechnologyMaterialPlan,
  // ???????????????
  SmMaterialMaterialOfbill,

  // ??????????????????
  SmConstructionBaseWorker,
  SmConstructionBaseEquipmentTeam,
  SmConstructionBaseMaterial,
  SmConstructionBaseProcedure,
  SmConstructionBaseSubItem,
  SmConstructionBaseSubItemSelect,
  SmConstructionBaseSubItemSelectTree,
  SmConstructionBaseDrawSubItem,
  SmConstructionBaseStandard,
  SmConstructionBaseStandardSelect,
  SmConstructionBaseSection,
  SmConstructionBaseSectionSelect,
  SmConstructionMasterPlan,
  SmConstructionMasterPlanContentSelect,
  SmConstructionPlanContentSelect,
  SmConstructionPlanContentSelectModal,
  SmConstructionMasterPlanContentWithGantt,
  SmConstructionPlanWithGantt,
  SmConstructionPlan,
  SmConstructionPlanContent,
  SmConstructionDispatchTemplate,
  SmConstructionDispatchTemplateSelect,
  SmConstructionDispatch,
  SmConstructionDispatchs,
  SmConstructionDailys,
  SmConstructionDaily,
  SmConstructionDispatchSelect,
  SmConstructionDailyView,
  SmConstructionDailyTemplate,
  // SmConstructionBaseProcedureRltSub,
  //?????????????????????
  SmConstructionPlanMaterial,
];

const install = function(Vue) {
  components.map(component => {
    Vue.use(component);
  });
};

/* istanbul ignore if */
if (typeof window !== 'undefined' && window.Vue) {
  install(window.Vue);
}

export {
  /* ???????????? */
  SmComponentQrCodeGenerate,
  SmComponentQrCode,
  version,
  ScTest,
  SmNamespaceModule,
  //Basic
  SmBasicRailways,
  SmBasicRailwayTreeSelect,
  SmBasicStations,
  SmBasicStationCascader,
  SmBasicStationSelect,
  SmBasicInstallationSites,
  SmBasicInstallationSiteCascader,
  SmBasicInstallationSiteSelect,
  SmBasicScopeCascader,
  SmBasicScopeSelect,
  //StdBasic
  SmStdBasicManufacturers,
  SmStdBasicManufacturerSelect,
  SmStdBasicStandardEquipments,
  SmStdBasicProductCategoryTreeSelect,
  SmStdBasicComponentCategoryTreeSelect,
  SmStdBasicRepairGroupSelect,
  SmStdBasicRepairGroupTreeSelect,
  SmStdBasicInfluenceRanges,
  SmStdBasicWorkAttention,
  SmStdBasicWorkAttentionTreeSelect,
  SmStdBasicRepairItems,
  SmStdBasicProductCategory,
  SmStdBasicComponentCategory,
  SmStdBasicMvdCategory,
  SmStdBasicMvdProperty,
  SmStdBasicQuotaCategoryTreeSelect,
  SmStdBasicQuotaCategory,
  SmStdBasicIndividualProject,
  SmStdBasicProjectItemListSelect,
  SmStdBasicProjectItem,
  SmStdBasicIndividualProjectTreeSelect,
  SmStdBasicProcessTemplateTreeSelect,
  SmStdBasicProcessTemplate,
  SmStdBasicQuota,
  SmStdBasicComputerCode,
  SmStdBasicMvdPropertyTreeSelect,
  SmBpmFormDesign,
  SmBpmFlowDesign,
  SmBpmWorkflowTemplate,
  SmBpmWorkflowTemplates,
  SmBpmWorkflows,
  SmBpmChartStatistics,
  SmBpmWorkflowView,
  SmBpmWorkflowTemplatesSelect,
  //????????????
  SmCostmanagementPeopleCost,
  SmCostmanagementOtherCost,
  SmCostmanagementMoneyList,
  SmCostmanagementCapitalReport,
  SmCostmanagementContract,
  SmCostmanagementBreakevenAnalysis,
  //Cms
  SmCmsCategories,
  SmCmsCategory,
  SmCmsArticles,
  SmCmsArticle,
  SmCmsCategoryRltArticles,
  //CrPlan
  SmCrPlanOtherPlans,
  SmCrPlanOtherPlan,
  SmCrPlanSkylightPlans,
  SmCrPlanSkylightPlan,
  SmCrPlanMonthPlanRecords,
  SmCrPlanYearPlanRecords,
  SmCrPlanYearPlan,
  SmCrPlanMonthPlan,
  SmCrPlanPlanChanges,
  SmCrPlanPlanChange,
  SmCrPlanAddSelectablePlanModal,
  SmCrPlanPlanTodos,
  SmCrPlanSentedWorkOrders,
  SmCrPlanSendingWorks,
  SmCrPlanSendingWork,
  SmCrPlanSendingWorksFinished,
  SmCrPlanMonthOfYearPlan,
  SmCrPlanYearMonthChange,
  SmCrPlanOtherWorks,
  SmCrPlanMaintenanceRecords,
  SmCrPlanMaintenanceRecord,
  SmCrPlanMaintenanceWork,
  SmCrPlanMaintenanceWorkCheck,
  SmCrPlanYearMonthAlterRecord,
  SmCrPlanCooperateWork,
  //Statists
  SmCrStatisticsDashboard,
  SmCrStatisticsPlanState,
  SmCrStatisticsPlanTrack,
  //module-dashboard ????????????
  SmDashboard,
  SmDashboardListCard,
  //Problem
  SmProblemProblemCategories,
  SmProblemProblemCategoryTreeSelect,
  SmProblemProblems,
  //Resource
  SmResourceEquipments,
  SmResourceStoreHouse,
  SmResourceStoreImportAltExport,
  SmResourceStoreEquipmentTransfer,
  SmResourceStoreEquipments,
  SmResourceStoreEquipmentsTests,
  SmResourceCodeGeneration,
  SmResourceStoreEquipmentsTest,
  SmResourceStoreEquipmentsSelect,
  SmResourceStoreEquipmentRecord,
  SmResourceEquipmentGroup,
  SmResourceEquipmentGroupTreeSelect,
  SmResourceStoreHouseTreeSelect,
  //System
  SmSystemMemberModal,
  SmSystemMemberSelect,
  SmSystemUsers,
  SmSystemUserSelect,
  SmSystemUserChangePassword,
  SmSystemOrganizationUserSelect,
  SmSystemRoles,
  SmSystemOrganizations,
  SmSystemOrganizationTree,
  SmSystemOrganizationTreeSelect,
  SmSystemDataDictionaryTreeSelect,
  SmSystemDataDictionaries,
  //oa
  SmOaDutySchedule,
  SmOaContract,
  SmOaContracts,
  SmOaSeals,
  //Report
  SmReports,
  SmReport,
  SmOaSealsSelectModal,
  //File
  SmFileManage,
  SmFileMigration,
  SmFileOss,
  SmFileManageModal,
  SmFileManageSelect,
  SmFileTextEditor,
  SmFileUpload,
  // SmExamCategories,
  SmExamQuestion,
  SmExamKnowledgePoints,
  SmExamExamPaperTemplate,
  SmExamCategories,
  SmExamPapers,
  SmEmergFault,
  SmEmergFaults,
  SmEmergPlans,
  SmEmergPlan,
  SmResourceEquipmentSelect,
  SmArea,
  SmCommonSelect,
  SmImport,
  SmImportModal,
  SmImportTemplate,
  SmExport,
  SmQrcode,
  SmQrcodeConfig,
  SmMessage,
  SmMessageNotice,
  SmMessageCenter,
  SmFlowBase,
  // SmD3
  SmD3,
  SmD3ScopeEquipments,
  SmD3Equipments,
  SmD3EquipmentInfo,
  SmD3EquipmentInfoFiles,
  SmD3EquipmentInfoProperties,
  SmD3TerminalLink,
  SmD3EmergFaults,
  SmD3CableCores,
  SmD3Alarms,
  SmD3StationSlider,
  SmD3Layers,
  SmD3Interface,
  SmD3ConstructionProgress,
  SmD3ModalSelect,
  // project
  SmProjectProject,
  SmProjectProjectSelect,
  SmProjectProjects,
  SmProjectArchives,
  SmProjectDossier,
  SmProjectDossierCatrgotyTreeSelect,
  SmProjectArchivesCatrgotyTreeSelect,
  SmProjectActionTreeSelect,
  SmProjectUploadModal,
  SmTaskCalendars,
  SmTaskCalendar,
  SmTaskTasks,
  SmTaskTask,
  SnMapAmap,
  // ?????????
  PlayerBar,
  ScGantt,
  SmMapControl,
  SmAlarms,
  SmRegulationInstitution,
  SmRegulationLabelTreeSelect,
  SmRegulationAuditedInstitution,
  SmVideo,
  SmGantt,
  //????????????????????????
  SmSafeProblem,
  SmSafeProblemLibrary,
  SmSafeSpeechVideo,
  SmSafeRltQualityModalSelect,

  //????????????????????????
  SmQualityProblems,
  SmQualityProblemLibraries,
  //material
  SmMaterialSuppliers,
  SmMaterialSupplier,
  SmMaterialMaterial,
  SmSchedules,
  SmSchedule,
  SmSchedulesSelect,
  SmApprovals,
  SmApproval,
  SmApprovalTable,
  SmScheduleDiarys,
  SmScheduleDiary,
  SmScheduleDiaryLog,
  SmScheduleDiarysStatistics,
  SmMaterialPartital,
  SmMaterialPartitalTreeSelect,
  SmMaterialMaterialTypeSelect,
  SmMaterialMaterialSelect,
  SmMaterialInventory,
  SmMaterialEntryRecords,
  SmMaterialOutRecords,
  SmMaterialPurchase,
  SmMaterialContract,
  SmMaterialConstructionSection,
  SmMaterialConstructionTeam,
  SmTechnologyDisclose,
  SmTechnologyInterfaceListing,
  SmTechnologyQuantity,
  SmTechnologyInterfaceFlag,
  SmTechnologyInterfaceReport,
  SmTechnologyMaterialPlan,
  SmMaterialInquire,
  SmMaterialMaterialPlan,
  SmMaterialGapAnalysis,
  SmMaterialAcceptance,
  SmMaterialMaterialOfbill,
  SmMaterialPurchaseList,
  SmMaterialPurchaseListSelect,
  SmMaterialPurchasePlan,
  SmMaterialMaterialPlanSelect,
  SmMaterialPurchasePlanSelect,
  SmMaterialMaterialSelectModal,
  // ??????????????????
  SmConstructionBaseWorker,
  SmConstructionBaseEquipmentTeam,
  SmConstructionBaseMaterial,
  SmConstructionBaseProcedure,
  SmConstructionBaseDrawSubItem,
  SmConstructionBaseSubItem,
  SmConstructionBaseSubItemSelect,
  SmConstructionBaseSubItemSelectTree,
  SmConstructionBaseStandard,
  SmConstructionBaseStandardSelect,
  SmConstructionBaseSection,
  SmConstructionBaseSectionSelect,
  SmConstructionMasterPlan,
  SmConstructionMasterPlanContentSelect,
  SmConstructionPlanContentSelect,
  SmConstructionPlanContentSelectModal,
  SmConstructionMasterPlanContentWithGantt,
  SmConstructionPlanWithGantt,
  SmConstructionPlan,
  SmConstructionPlanContent,
  SmConstructionDispatchTemplate,
  SmConstructionDispatchTemplateSelect,
  SmConstructionDispatch,
  SmConstructionDispatchs,
  SmConstructionDailys,
  SmConstructionDispatchSelect,
  SmConstructionDailyView,
  SmConstructionDailyTemplate,
  SmConstructionDaily,
  // SmConstructionBaseProcedureRltSub,
  //??????????????????
  SmConstructionPlanMaterial,
};

export default {
  version,
  install,
};
