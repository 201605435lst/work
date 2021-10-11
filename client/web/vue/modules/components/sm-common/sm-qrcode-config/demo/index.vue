
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-qrcode-config` ,
  us: `# sm-qrcode-config` ,
};

export default {
  title: 'SmQrcodeConfig',
  render() {
    return (
      <div id="components-sm-qrcode-config-demo">
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
    