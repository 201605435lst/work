
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 施工日志查看` ,
  us: `# 施工日志查看` ,
};

export default {
  title: 'SmScheduleDiaryLog',
  render() {
    return (
      <div id="components-sm-schedule-diary-log-demo">
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
    