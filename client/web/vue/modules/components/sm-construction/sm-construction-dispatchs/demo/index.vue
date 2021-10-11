<script>
import Basic from './basic.md';
import Approval from './approval.md';
import Query from './query.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 派工管理`,
  us: `# sm-construction-dispatchs`,
};

export default {
  title: 'SmConstructionDispatchs',
  render() {
    return (
      <div id="components-sm-construction-dispatchs-demo">
        <md cn={md.cn} us={md.us} />
        <h1>派工单管理</h1>
        <Basic />
        <br/>
        <h1>派工单审批</h1>
        <Approval />
        <h1>派工单查询</h1>
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
