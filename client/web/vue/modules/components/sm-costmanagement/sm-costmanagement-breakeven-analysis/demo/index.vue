
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-costmanagement-breakeven-analysis` ,
  us: `# sm-costmanagement-breakeven-analysis` ,
};

export default {
  title: 'SmCostmanagementBreakevenAnalysis',
  render() {
    return (
      <div id="components-sm-costmanagement-breakeven-analysis-demo">
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
    