<script>
import Basic from './basic.md';
import Receive from './receive.md';
import Send from './send.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 工作汇报
        `,
  us: `# 工作汇报
        `,
};

export default {
  category: 'Modules',
  subtitle: '工作汇报',
  type: 'Report',
  zhType: '工作汇报',
  title: 'SmReports',
  render() {
    return (
      <div id="sm-reports-demo">
        <md cn={md.cn} us={md.us} />
        <Basic />
        <Receive />
        <Send />
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
