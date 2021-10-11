
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 二维码跟踪记录` ,
  us: `# 二维码跟踪记录` ,
};

export default {
  title: 'SmResourceQrcodeRecord',
  render() {
    return (
      <div id="components-sm-resource-qrcode-record-demo">
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
    