
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 施工审批管理` ,
  us: `# 施工审批管理` ,
};

export default {
  title: 'SmScheduleApprovals',
  render() {
    return (
      <div id="components-sm-schedule-approvals-demo">
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
    