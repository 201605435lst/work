
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 量差分析(具体需求不确定)` ,
  us: `# 量差分析(具体需求不确定)` ,
};

export default {
  title: 'SmMaterialGapAnalysis',
  render() {
    return (
      <div id="components-sm-material-gap-analysis-demo">
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
    