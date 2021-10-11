
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 配合作业` ,
  us: `# 配合作业` ,
};

export default {
  title: 'SmCrPlanCooperateWork',
  category: 'Modules',
  subtitle: '配合作业',
  type: 'CrPlan',
  zhType: '配合作业',
  render() {
    return (
      <div id="components-sm-cr-plan-cooperate-work-demo">
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
    