
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-construction-base-procedure-rlt-sub` ,
  us: `# sm-construction-base-procedure-rlt-sub` ,
};

export default {
  title: 'SmConstructionBaseProcedureRltSub',
  render() {
    return (
      <div id="components-sm-construction-base-procedure-rlt-sub-demo">
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
    