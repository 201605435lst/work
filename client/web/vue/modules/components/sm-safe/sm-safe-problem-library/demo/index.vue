
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-safe-problem-library` ,
  us: `# sm-safe-problem-library` ,
};

export default {
  title: 'SmSafeProblemLibrary',
  render() {
    return (
      <div id="components-sm-safe-problem-library-demo">
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
    