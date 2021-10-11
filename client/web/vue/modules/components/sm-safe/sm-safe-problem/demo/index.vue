
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-safe-problem` ,
  us: `# sm-safe-problem` ,
};

export default {
  title: 'SmSafeProblem',
  render() {
    return (
      <div id="components-sm-safe-problem-demo">
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
    