
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 构件跟踪二维码生成` ,
  us: `#  构件跟踪二维码生成` ,
};

export default {
  title: 'SmComponentQrCodeGenerate',
  render() {
    return (
      <div id="sm-component-qr-code-generate-demo">
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
    