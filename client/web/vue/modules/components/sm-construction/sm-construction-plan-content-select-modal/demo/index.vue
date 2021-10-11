
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-construction-plan-content-select-modal` ,
  us: `# sm-construction-plan-content-select-modal` ,
};

export default {
  title: 'SmConstructionPlanContentSelectModal',
  render() {
    return (
      <div id="components-sm-construction-plan-content-select-modal-demo">
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
    