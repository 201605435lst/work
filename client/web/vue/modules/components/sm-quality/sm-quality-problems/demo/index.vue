
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 质量问题` ,
  us: `# 质量问题` ,
};

export default {
  title: 'SmQualityProblems',
  render() {
    return (
      <div id="components-sm-quality-problems-demo">
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
    