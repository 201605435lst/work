<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 库存管理模态框
        `,
  us: `# 库存管理模态框
        `,
};

export default {
  category: 'Modules',
  subtitle: '库存管理模态框',
  type: 'Material',
  zhType: '物资管理',
  title: 'SmMaterialInventoryModal',
  render() {
    return (
      <div id="sm-material-inventory-modal-demo">
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
