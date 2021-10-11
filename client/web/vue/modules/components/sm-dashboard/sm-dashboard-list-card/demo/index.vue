<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 卡片列表`,
  us: `# 卡片列表`,
};

export default {
  category: 'Modules',
  subtitle: '卡片列表',
  type: 'Dashboard',
  zhType: '面板模块',
  title: 'SmDashboardListCard',
  render() {
    return (
      <div id="components-sm-dashboard-list-card-demo">
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
