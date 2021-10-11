
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-safe-rlt-quality-modal-select` ,
  us: `# sm-safe-rlt-quality-modal-select` ,
};

export default {
  title: 'SmSafeRltQualityModalSelect',
  render() {
    return (
      <div id="components-sm-safe-rlt-quality-modal-select-demo">
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
    