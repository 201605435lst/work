
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 二维码组件` ,
  us: `# 二维码组件` ,
};

export default {
  title: 'SmQrcode',
  render() {
    return (
      <div id="components-sm-qrcode-demo">
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
    