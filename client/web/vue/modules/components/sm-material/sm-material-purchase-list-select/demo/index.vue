
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-material-purchase-list-select` ,
  us: `# sm-material-purchase-list-select` ,
};

export default {
  title: 'SmMaterialPurchaseListSelect',
  render() {
    return (
      <div id="components-sm-material-purchase-list-select-demo">
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
    