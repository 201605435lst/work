
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-d-3-quality` ,
  us: `# sm-d-3-quality` ,
};

export default {
  title: 'SmD3Quality',
  render() {
    return (
      <div id="components-sm-d-3-quality-demo">
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
    