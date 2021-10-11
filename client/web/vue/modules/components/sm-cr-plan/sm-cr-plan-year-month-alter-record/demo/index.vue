
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 年月表变更记录` ,
  us: `# 年月表变更记录` ,
};

export default {
  title: 'SmCrPlanYearMonthAlterRecord',
  render() {
    return (
      <div id="components-sm-cr-plan-year-month-alter-record-demo">
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
    