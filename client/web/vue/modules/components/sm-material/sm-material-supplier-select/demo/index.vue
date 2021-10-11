<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 供应商选择框
        `,
  us: `# 供应商选择框
        `,
};

export default {
  category: 'Modules',
  subtitle: '供应商选择框',
  type: 'Material',
  zhType: '物资管理',
  title: 'SmMaterialSupplierSelect',
  render() {
    return (
      <div id="components-sm-material-supplier-select-demo">
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
