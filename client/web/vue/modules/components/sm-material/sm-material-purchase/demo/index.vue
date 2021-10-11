
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 采购计划` ,
  us: `# 采购计划` ,
};

export default {
  title: 'SmMaterialPurchase',
  render() {
    return (
      <div id="components-sm-material-purchase-demo">
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