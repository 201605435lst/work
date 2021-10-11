
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-material-purchase-plan-select` ,
  us: `# sm-material-purchase-plan-select` ,
};

export default {
  title: 'SmMaterialPurchasePlanSelect',
  render() {
    return (
      <div id="components-sm-material-purchase-plan-select-demo">
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
    