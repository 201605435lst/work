
<script>
import Basic from './basic.md';
import Approval from './approval.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 施工日志管理` ,
  us: `# 施工日志管理` ,
};

export default {
  title: 'SmConstructionDailys',
  render() {
    return (
      <div id="components-sm-construction-dailys-demo">
        <md cn={md.cn} us={md.us} />
        <Basic />
        <Approval/>
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
    