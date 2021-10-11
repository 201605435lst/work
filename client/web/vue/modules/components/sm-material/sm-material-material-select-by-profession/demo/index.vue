<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 材料选择框
        `,
  us: `# 材料选择框
        `,
};

export default {
  category: 'Modules',
  subtitle: '材料选择框',
  type: 'Material',
  zhType: '基础',
  title: 'SmMaterialMaterialSelectByProfession',
  render() {
    return (
      <div id="components-sm-material-material-select-by-profession-demo">
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
