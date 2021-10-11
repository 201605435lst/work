<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 工作汇报单页
        `,
  us: `# 工作汇报单页
        `,
};

export default {
  category: 'Modules',
  subtitle: '工作汇报单页',
  type: 'Report',
  zhType: '工作汇报单页',
  title: 'SmReport',
  render() {
    return (
      <div id="sm-report-demo">
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
