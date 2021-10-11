
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-construction-plan-content-select` ,
  us: `# sm-construction-plan-content-select` ,
};

export default {
  title: 'SmConstructionPlanContentSelect',
  render() {
    return (
      <div id="components-sm-construction-plan-content-select-demo">
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
    