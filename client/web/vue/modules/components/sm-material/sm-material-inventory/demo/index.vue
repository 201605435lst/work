
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 库存管理` ,
  us: `# 库存管理` ,
};

export default {
  title: 'SmMaterialInventory',
  render() {
    return (
      <div id="components-sm-material-inventory-demo">
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
    