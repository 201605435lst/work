
<script>
import Basic from './basic.md';
import Approval from './approval.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-construction-plan-with-gantt` ,
  us: `# sm-construction-plan-with-gantt` ,
};

export default {
  title: 'SmConstructionPlanWithGantt',
  render() {
    return (
      <div id="components-sm-construction-plan-with-gantt-demo">
        <md cn={md.cn} us={md.us} />
        <h1> 甘特图结合施工计划详情</h1>
        <Basic />
        <h1> 甘特图结合总体计划详情-审批/查看(无法编辑)</h1>
        <Approval />
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
