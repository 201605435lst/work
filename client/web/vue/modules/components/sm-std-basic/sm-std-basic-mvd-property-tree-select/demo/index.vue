<script>
import Basic from './basic.md';
import Multiple from './multiple.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 信息交换模板属性选择树
        `,
  us: `# 信息交换模板属性选择树
        `,
};

export default {
  category: 'Modules',
  subtitle: '信息交换模板属性选择树',
  type: 'Basic',
  zhType: '数据库基础模块',
  title: 'SmStdBasicMvdPropertyTreeSelect',
  render() {
    return (
      <div id="components-sm-std-basic-mvd-property-tree-select-demo">
        <md cn={md.cn} us={md.us} />
        <Basic />
        <Multiple />
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
