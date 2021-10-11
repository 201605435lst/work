const SmBpmFormDesign = {
  category: 'Modules',
  type: 'Bpm',
  title: 'SmBpmFormDesign',
  subtitle: '表单设计',
  demos: [
    {
      path: 'sm-bpm-form-design',
      component: () => import('@/components/sm-bpm/sm-bpm-form-design/demo/index.vue'),
    },
    {
      path: 'sm-bpm-form-design-cn',
      component: () => import('@/components/sm-bpm/sm-bpm-form-design/demo/index.vue'),
    },
  ],
};
const SmBpmFlowDesign = {
  category: 'Modules',
  type: 'Bpm',
  title: 'SmBpmFlowDesign',
  subtitle: '流程设计',
  demos: [
    {
      path: 'sm-bpm-flow-design',
      component: () => import('@/components/sm-bpm/sm-bpm-flow-design/demo/index.vue'),
    },
    {
      path: 'sm-bpm-flow-design-cn',
      component: () => import('@/components/sm-bpm/sm-bpm-flow-design/demo/index.vue'),
    },
  ],
};
const SmBpmWorkflowTemplate = {
  category: 'Modules',
  type: 'Bpm',
  title: 'SmBpmWorkflowTemplate',
  subtitle: '工作流模板单页',
  demos: [
    {
      path: 'sm-bpm-workflow-template',
      component: () => import('@/components/sm-bpm/sm-bpm-workflow-template/demo/index.vue'),
    },
    {
      path: 'sm-bpm-workflow-template-cn',
      component: () => import('@/components/sm-bpm/sm-bpm-workflow-template/demo/index.vue'),
    },
  ],
};
const SmBpmWorkflowTemplates = {
  category: 'Modules',
  type: 'Bpm',
  title: 'SmBpmWorkflowTemplates',
  subtitle: '工作流模板管理',
  demos: [
    {
      path: 'sm-bpm-workflow-templates',
      component: () => import('@/components/sm-bpm/sm-bpm-workflow-templates/demo/index.vue'),
    },
    {
      path: 'sm-bpm-workflow-templates-cn',
      component: () => import('@/components/sm-bpm/sm-bpm-workflow-templates/demo/index.vue'),
    },
  ],
};
const SmBpmWorkflows = {
  category: 'Modules',
  type: 'Bpm',
  title: 'SmBpmWorkflows',
  subtitle: '工作流管理',
  demos: [
    {
      path: 'sm-bpm-workflows',
      component: () => import('@/components/sm-bpm/sm-bpm-workflows/demo/index.vue'),
    },
    {
      path: 'sm-bpm-workflows-cn',
      component: () => import('@/components/sm-bpm/sm-bpm-workflows/demo/index.vue'),
    },
  ],
};

const SmBpmChartStatistics = {
  category: 'Modules',
  type: 'Bpm',
  title: 'SmBpmChartStatistics',
  subtitle: '工作流图表统计',
  demos: [
    {
      path: 'sm-bpm-chart-statistics',
      component: () => import('@/components/sm-bpm/sm-bpm-chart-statistics/demo/index.vue'),
    },
    {
      path: 'sm-bpm-chart-statistics-cn',
      component: () => import('@/components/sm-bpm/sm-bpm-chart-statistics/demo/index.vue'),
    },
  ],
};
const SmBpmWorkflowView = {
  category: 'Modules',
  type: 'Bpm',
  title: 'SmBpmWorkflowView',
  subtitle: '工作流流程查看',
  demos: [
    {
      path: 'sm-bpm-workflow-view',
      component: () => import('@/components/sm-bpm/sm-bpm-workflow-view/demo/index.vue'),
    },
    {
      path: 'sm-bpm-workflow-view-cn',
      component: () => import('@/components/sm-bpm/sm-bpm-workflow-view/demo/index.vue'),
    },
  ],
};
const SmBpmWorkflowTemplatesSelect = {
  category: 'Modules',
  type: 'Bpm',
  title: 'SmBpmWorkflowTemplatesSelect',
  subtitle: '工作流树选择',
  demos: [
    {
      path: 'sm-bpm-workflow-templates-select',
      component: () => import('@/components/sm-bpm/sm-bpm-workflow-templates-select/demo/index.vue'),
    },
    {
      path: 'sm-bpm-workflow-templates-select-cn',
      component: () => import('@/components/sm-bpm/sm-bpm-workflow-templates-select/demo/index.vue'),
    },
  ],
};
export const modules = {
  SmBpmFormDesign,
  SmBpmFlowDesign,
  SmBpmWorkflowTemplate,
  SmBpmWorkflowTemplates,
  SmBpmWorkflows,
  SmBpmChartStatistics,
  SmBpmWorkflowView,
  SmBpmWorkflowTemplatesSelect,
};
export const demo = [
  ...SmBpmFormDesign.demos,
  ...SmBpmFlowDesign.demos,
  ...SmBpmWorkflowTemplate.demos,
  ...SmBpmWorkflowTemplates.demos,
  ...SmBpmWorkflows.demos,
  ...SmBpmChartStatistics.demos,
  ...SmBpmWorkflowView.demos,
  ...SmBpmWorkflowTemplatesSelect.demos,
];
