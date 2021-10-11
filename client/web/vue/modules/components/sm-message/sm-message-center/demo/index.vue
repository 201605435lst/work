
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 消息中心` ,
  us: `# 消息中心` ,
};

export default {
  title: 'SmMessageCenter',
  render() {
    return (
      <div id="components-sm-message-center-demo">
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
    