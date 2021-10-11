<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 工作看板`,
  us: `# 工作看板`,
};

export default {
  category: 'Modules',
  subtitle: '工作看板',
  type: 'Dashboard',
  zhType: '面板模块',
  title: 'SmDashboard',
  render() {
    return (
      <div id="components-sm-dashboard-demo">
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
