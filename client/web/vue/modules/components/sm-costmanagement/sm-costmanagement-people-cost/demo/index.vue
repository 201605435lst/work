
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-costmanagement-people-cost` ,
  us: `# sm-costmanagement-people-cost` ,
};


export default {
  title: 'SmCostmanagementPeopleCost',
  render() {
    return (
      <div id="components-sm-costmanagement-people-cost-demo">
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
    