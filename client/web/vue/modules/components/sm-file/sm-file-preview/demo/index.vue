
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-file-preview` ,
  us: `# sm-file-preview` ,
};

export default {
  title: 'SmFilePreview',
  render() {
    return (
      <div id="components-sm-file-preview-demo">
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
    