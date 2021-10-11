
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 构件二维码管理` ,
  us: `# 构件二维码管理` ,
};

export default {
  title: 'SmComponentQrCode',
  render() {
    return (
      <div id="sm-component-qr-code-demo">
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
    