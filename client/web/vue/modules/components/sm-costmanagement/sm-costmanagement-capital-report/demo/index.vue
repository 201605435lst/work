
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-costmanagement-capital-report` ,
  us: `# sm-costmanagement-capital-report` ,
};

export default {
  title: 'SmCostmanagementCapitalReport',
  render() {
    return (
      <div id="components-sm-costmanagement-capital-report-demo">
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
    