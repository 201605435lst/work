
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-construction-base-section` ,
  us: `# sm-construction-base-section` ,
};

export default {
  title: 'SmConstructionBaseSection',
  render() {
    return (
      <div id="components-sm-construction-base-section-demo">
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
