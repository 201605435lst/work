<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 维修计划复验
        `,
  us: `# 维修计划复验
        `,
};

export default {
  category: 'Modules',
  subtitle: '维修计划复验',
  type: 'CrPlan',
  zhType: '维修计划复验',
  title: 'SmCrPlanMaintenanceWorkCheck',
  render() {
    return (
      <div id="components-sm-cr-plan-maintenance-work-check-demo">
        <md cn={md.cn} us={md.us} />
        <Basic />
        <api>
          <CN slot="cn" />
          <US />
        </api>
      </div>
    );
  },
};
</script>

<style>
#components-badge-demo .ant-badge:not(.ant-badge-not-a-wrapper) {
  margin-right: 20px;
}
#components-badge-demo .head-example {
  width: 42px;
  height: 42px;
  border-radius: 4px;
  background: #eee;
  display: inline-block;
  vertical-align: middle;
}
#components-badge-demo .ant-badge-not-a-wrapper:not(.ant-badge-status) {
  margin-right: 8px;
}
</style>
