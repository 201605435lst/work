
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 质量问题库` ,
  us: `# 质量问题库` ,
};

export default {
  title: 'SmQualityProblemLibraries',
  render() {
    return (
      <div id="components-sm-quality-problem-libraries-demo">
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
    