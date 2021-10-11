
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 工作流图表统计` ,
  us: `# 工作流图表统计` ,
};

export default {
  title: 'SmBpmChartStatistics',
  category: 'Modules',
  subtitle: '工作流图表统计',
  type: 'Bpm',
  zhType: '工作流图表统计',
  render() {
    return (
      <div id="components-sm-bpm-chart-statistics-demo">
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
    