<script>
import Basic from './basic.md';
import Multiple from './multiple.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 分区管理选择框
        `,
  us: `# 分区管理选择框
        `,
};

export default {
  category: 'Modules',
  subtitle: '分区管理选择框',
  type: 'System',
  zhType: '物资管理',
  title: 'SmMaterialPartitalTreeSelect',
  render() {
    return (
      <div id="components-sm-material-partital-tree-select-demo">
        <md cn={md.cn} us={md.us} />
        <Basic />
        <h3>多选</h3>
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
