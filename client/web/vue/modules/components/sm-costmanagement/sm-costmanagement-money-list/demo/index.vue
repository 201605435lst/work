
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-costmanagement-money-list` ,
  us: `# sm-costmanagement-money-list` ,
};

export default {
  title: 'SmCostmanagementMoneyList',
  render() {
    return (
      <div id="components-sm-costmanagement-money-list-demo">
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
    