<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 月表记录
        `,
  us: `# 月表记录
        `,
};

export default {
  category: 'Modules',
  subtitle: '月表记录',
  type: 'CrPlan',
  zhType: '系统',
  title: 'SmCrPlanMonthPlanRecords',
  render() {
    return (
      <div id="components-sm-cr-plan-month-plan-records-demo">
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

<style></style>
