<script>
import Basic from './basic.md';
import Approval from './approval.md';
import Material from './material.md';
import Small from './small.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-technology-material-plan`,
  us: `# sm-technology-material-plan`,
};

export default {
  title: 'SmTechnologyMaterialPlan',
  render() {
    return (
      <div id="components-sm-technology-material-plan-demo">
        <md cn={md.cn} us={md.us} />
        <h1> 用料计划台账</h1>
        <Basic />
        <h1> 用料计划审批</h1>
        <br/>
        <Approval />
        <h1>物资模块调用（展示数据）</h1>
        <br/>
        <Material/>
        <h1>其他弹窗调用</h1>
        <br/>
        <Small/>
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
