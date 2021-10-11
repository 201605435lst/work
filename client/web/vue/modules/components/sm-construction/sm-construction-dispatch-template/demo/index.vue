<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 派工单模板管理`,
  us: `# sm-construction-dispatch-template`,
};

export default {
  title: 'SmConstructionDispatchTemplate',
  render() {
    return (
      <div id="components-sm-construction-dispatch-template-demo">
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
