
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-d-3-safe` ,
  us: `# sm-d-3-safe` ,
};

export default {
  title: 'SmD3Safe',
  render() {
    return (
      <div id="components-sm-d-3-safe-demo">
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
    