<script>
import Basic from './basic.md';
import BpmNode from './bpmNode.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 任务节点
        `,
  us: `# 任务节点
        `,
};

export default {
  category: 'Modules',
  subtitle: '任务节点',
  type: 'Common',
  zhType: '任务节点',
  title: 'SmFlowBase',
  render() {
    return (
      <div id="components-sm-flow-base">
        <md cn={md.cn} us={md.us} />
        <h3>任务节点</h3>
        <Basic />
        <h3>工作流节点</h3>
        <BpmNode/>
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
  h3{
    margin-bottom: 10px;
  }
</style>
