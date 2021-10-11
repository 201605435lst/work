const SmConstructionBaseWorker = {
  category: 'Modules',
  type: 'ConstructionBase',
  title: 'SmConstructionBaseWorker',
  subtitle: '工种管理',
  demos: [
    {
      path: 'sm-construction-base-worker',
      component: () => import('@/components/sm-construction-base/sm-construction-base-worker/demo/index.vue'),
    },
    {
      path: 'sm-construction-base-worker-cn',
      component: () => import('@/components/sm-construction-base/sm-construction-base-worker/demo/index.vue'),
    },
  ],
};
const SmConstructionBaseEquipmentTeam = {
  category: 'Modules',
  type: 'ConstructionBase',
  title: 'SmConstructionBaseEquipmentTeam',
  subtitle: '设备台班管理',
  demos: [
    {
      path: 'sm-construction-base-equipment-team',
      component: () => import('@/components/sm-construction-base/sm-construction-base-equipment-team/demo/index.vue'),
    },
    {
      path: 'sm-construction-base-equipment-team-cn',
      component: () => import('@/components/sm-construction-base/sm-construction-base-equipment-team/demo/index.vue'),
    },
  ],
};
const SmConstructionBaseMaterial = {
  category: 'Modules',
  type: 'ConstructionBase',
  title: 'SmConstructionBaseMaterial',
  subtitle: '工程量清单管理',
  demos: [
    {
      path: 'sm-construction-base-material',
      component: () => import('@/components/sm-construction-base/sm-construction-base-material/demo/index.vue'),
    },
    {
      path: 'sm-construction-base-material-cn',
      component: () => import('@/components/sm-construction-base/sm-construction-base-material/demo/index.vue'),
    },
  ],
};
const SmConstructionBaseProcedure = {
  category: 'Modules',
  type: 'ConstructionBase',
  title: 'SmConstructionBaseProcedure',
  subtitle: '施工工序管理',
  demos: [
    {
      path: 'sm-construction-base-procedure/:id',
      component: () => import('@/components/sm-construction-base/sm-construction-base-procedure/demo/index.vue'),
    },
    {
      path: 'sm-construction-base-procedure-cn/:id',
      component: () => import('@/components/sm-construction-base/sm-construction-base-procedure/demo/index.vue'),
    },
  ],
};
const SmConstructionBaseSubItem = {
  category: 'Modules',
  type: 'ConstructionBase',
  title: 'SmConstructionBaseSubItem',
  subtitle: '分部分项管理',
  demos: [
    {
      path: 'sm-construction-base-sub-item',
      component: () => import('@/components/sm-construction-base/sm-construction-base-sub-item/demo/index.vue'),
    },
    {
      path: 'sm-construction-base-sub-item-cn',
      component: () => import('@/components/sm-construction-base/sm-construction-base-sub-item/demo/index.vue'),
    },
  ],
};
const SmConstructionBaseSubItemSelect = {
  category: 'Modules',
  type: 'ConstructionBase',
  title: 'SmConstructionBaseSubItemSelect',
  subtitle: '分部分项管理选择框',
  demos: [
    {
      path: 'sm-construction-base-sub-item-select',
      component: () => import('@/components/sm-construction-base/sm-construction-base-sub-item-select/demo/index.vue'),
    },
    {
      path: 'sm-construction-base-sub-item-select-cn',
      component: () => import('@/components/sm-construction-base/sm-construction-base-sub-item-select/demo/index.vue'),
    },
  ],
};
const SmConstructionBaseSubItemSelectTree = {
  category: 'Modules',
  type: 'ConstructionBase',
  title: 'SmConstructionBaseSubItemSelectTree',
  subtitle: '分部分项选择树',
  demos: [
    {
      path: 'sm-construction-base-sub-item-select-tree',
      component: () => import('@/components/sm-construction-base/sm-construction-base-sub-item-select-tree/demo/index.vue'),
    },
    {
      path: 'sm-construction-base-sub-item-select-tree-cn',
      component: () => import('@/components/sm-construction-base/sm-construction-base-sub-item-select-tree/demo/index.vue'),
    },
  ],
};
// 编制分部分项
const SmConstructionBaseDrawSubItem = {
  category: 'Modules',
  type: 'ConstructionBase',
  title: 'SmConstructionBaseDrawSubItem',
  subtitle: '编制分部分项',
  demos: [
    {
      path: 'sm-construction-base-draw-sub-item',
      component: () => import('@/components/sm-construction-base/sm-construction-base-draw-sub-item/demo/index'),
    },
    {
      path: 'sm-construction-base-draw-sub-item-cn',
      component: () => import('@/components/sm-construction-base/sm-construction-base-draw-sub-item/demo/index'),
    },
  ],
};
// 先注释了,不知道后面要不要用……
// const SmConstructionBaseProcedureRltSub = {
//   category: 'Modules',
//   type: 'ConstructionBase',
//   title: 'SmConstructionBaseProcedureRltSub',
//   subtitle: '工序关联管理',
//   demos: [
//     {
//       path: 'sm-construction-base-procedure-rlt-sub',
//       component: () => import('@/components/sm-construction-base/sm-construction-base-procedure-rlt-sub/demo/index.vue'),
//     },
//     {
//       path: 'sm-construction-base-procedure-rlt-sub-cn',
//       component: () => import('@/components/sm-construction-base/sm-construction-base-procedure-rlt-sub/demo/index.vue'),
//     },
//   ],
// };



const SmConstructionBaseStandard = {
  category: 'Modules',
  type: 'ConstructionBase',
  title: 'SmConstructionBaseStandard',
  subtitle: '工序规范维护',
  demos: [
    {
      path: 'sm-construction-base-standard',
      component: () => import('@/components/sm-construction-base/sm-construction-base-standard/demo/index.vue'),
    },
    {
      path: 'sm-construction-base-standard-cn',
      component: () => import('@/components/sm-construction-base/sm-construction-base-standard/demo/index.vue'),
    },
  ],
};
const SmConstructionBaseStandardSelect = {
  category: 'Modules',
  type: 'ConstructionBase',
  title: 'SmConstructionBaseStandardSelect',
  subtitle: '工序规范维护-选择框',
  demos: [
    {
      path: 'sm-construction-base-standard-select',
      component: () => import('@/components/sm-construction-base/sm-construction-base-standard-select/demo/index.vue'),
    },
    {
      path: 'sm-construction-base-standard-select-cn',
      component: () => import('@/components/sm-construction-base/sm-construction-base-standard-select/demo/index.vue'),
    },
  ],
};
const SmConstructionBaseSection = {
  category: 'Modules',
  type: 'ConstructionBase',
  title: 'SmConstructionBaseSection',
  subtitle: '施工区段管理',
  demos: [
    {
      path: 'sm-construction-base-section',
      component: () => import('@/components/sm-construction-base/sm-construction-base-section/demo/index.vue'),
    },
    {
      path: 'sm-construction-base-section-cn',
      component: () => import('@/components/sm-construction-base/sm-construction-base-section/demo/index.vue'),
    },
  ],
};
const SmConstructionBaseSectionSelect = {
  category: 'Modules',
  type: 'ConstructionBase',
  title: 'SmConstructionBaseSectionSelect',
  subtitle: '施工区段选择框',
  demos: [
    {
      path: 'sm-construction-base-section-select',
      component: () => import('@/components/sm-construction-base/sm-construction-base-section-select/demo/index.vue'),
    },
    {
      path: 'sm-construction-base-section-select-cn',
      component: () => import('@/components/sm-construction-base/sm-construction-base-section-select/demo/index.vue'),
    },
  ],
};
export const modules = {

  SmConstructionBaseSectionSelect: SmConstructionBaseSectionSelect,
  SmConstructionBaseSection: SmConstructionBaseSection,
  SmConstructionBaseStandardSelect: SmConstructionBaseStandardSelect,
  SmConstructionBaseStandard: SmConstructionBaseStandard,
  SmConstructionBaseWorker: SmConstructionBaseWorker,
  SmConstructionBaseEquipmentTeam: SmConstructionBaseEquipmentTeam,
  SmConstructionBaseMaterial: SmConstructionBaseMaterial,
  SmConstructionBaseProcedure: SmConstructionBaseProcedure,
  SmConstructionBaseSubItem: SmConstructionBaseSubItem,
  SmConstructionBaseSubItemSelect: SmConstructionBaseSubItemSelect,
  SmConstructionBaseSubItemSelectTree: SmConstructionBaseSubItemSelectTree,
  SmConstructionBaseDrawSubItem: SmConstructionBaseDrawSubItem,
  // 工序关联模块先注释掉
  // SmConstructionBaseProcedureRltSub: SmConstructionBaseProcedureRltSub,
};
export const demo = [

  ...SmConstructionBaseSectionSelect.demos,
  ...SmConstructionBaseSection.demos,
  ...SmConstructionBaseStandardSelect.demos,
  ...SmConstructionBaseStandard.demos,
  ...SmConstructionBaseWorker.demos,
  ...SmConstructionBaseEquipmentTeam.demos,
  ...SmConstructionBaseMaterial.demos,
  ...SmConstructionBaseProcedure.demos,
  ...SmConstructionBaseSubItem.demos,
  ...SmConstructionBaseSubItemSelect.demos,
  ...SmConstructionBaseSubItemSelectTree.demos,
  ...SmConstructionBaseDrawSubItem.demos,
  // ...SmConstructionBaseProcedureRltSub.demos,
];
