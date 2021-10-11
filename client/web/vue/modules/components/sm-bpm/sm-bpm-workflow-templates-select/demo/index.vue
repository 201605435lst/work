
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-bpm-workflow-templates-select` ,
  us: `# sm-bpm-workflow-templates-select` ,
};

export default {
  title: 'SmBpmWorkflowTemplatesSelect',
  render() {
    return (
      <div id="components-sm-bpm-workflow-templates-select-demo">
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
    