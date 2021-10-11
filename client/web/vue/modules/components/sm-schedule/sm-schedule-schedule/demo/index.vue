
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 施工计划管理单页` ,
  us: `# 施工计划管理单页` ,
};

export default {
  title: 'SmScheduleSchedule',
  render() {
    return (
      <div id="components-sm-schedule-schedule-demo">
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
    