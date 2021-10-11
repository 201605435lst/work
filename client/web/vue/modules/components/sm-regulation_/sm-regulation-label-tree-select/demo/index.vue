<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 标签选择单页
        `,
  us: `# 标签选择单页
        `,
};

export default {
  category: 'Modules',
  subtitle: '标签选择单页',
  type: 'Regulation',
  zhType: '标签选择单页',
  title: 'SmRegulationLabelTreeSelect',
  
  render() {
    return (
      <div id="components-sm-regulation-label-tree-select-demo">
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
