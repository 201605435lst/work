
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-construction-base-worker` ,
  us: `# sm-construction-base-worker` ,
};

export default {
  title: 'SmConstructionBaseWorker',
  render() {
    return (
      <div id="components-sm-construction-base-worker-demo">
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
    