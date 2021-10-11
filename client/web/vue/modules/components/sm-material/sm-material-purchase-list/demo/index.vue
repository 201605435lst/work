
<script>
import Basic from './basic.md';
import Approval from './approval.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 采购清单` ,
  us: `# 采购清单` ,
};

export default {
  title: 'SmMaterialPurchaseList',
  render() {
    return (
      <div id="components-sm-material-purchase-list-demo">
        <md cn={md.cn} us={md.us} />
        <Basic />
        <Approval />
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
    