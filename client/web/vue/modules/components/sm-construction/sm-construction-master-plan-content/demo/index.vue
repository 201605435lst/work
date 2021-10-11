
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-construction-master-plan-content` ,
  us: `# sm-construction-master-plan-content` ,
};

export default {
  title: 'SmConstructionMasterPlanContent',
  render() {
    return (
      <div id="components-sm-construction-master-plan-content-demo">
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
