
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-costmanagement-other-cost` ,
  us: `# sm-costmanagement-other-cost` ,
};

export default {
  title: 'SmCostmanagementOtherCost',
  render() {
    return (
      <div id="components-sm-costmanagement-other-cost-demo">
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
    