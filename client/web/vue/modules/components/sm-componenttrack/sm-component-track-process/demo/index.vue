
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 构件跟踪设置` ,
  us: `# 构件跟踪设置` ,
};

export default {
  title: 'SmComponentTrackProcess',
  render() {
    return (
      <div id="components-sm-component-track-process-demo">
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
    