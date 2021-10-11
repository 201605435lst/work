
<script>
import Basic from './basic.md';
import Approval from './approval.md';
import Query from './query.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-construction-master-plan` ,
  us: `# sm-construction-master-plan` ,
};

export default {
  title: 'SmConstructionMasterPlan',
  render() {
    return (
      <div id="components-sm-construction-master-plan-demo">
        <md cn={md.cn} us={md.us} />
        <h1>总体计划编制</h1>
        <Basic />
        <br/>
        <h1>总体计划审批</h1>
        <Approval />
        <h1>总体计划查询</h1>
        <Query />
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
