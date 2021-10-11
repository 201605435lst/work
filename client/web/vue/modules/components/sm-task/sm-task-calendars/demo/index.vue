<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 任务日历
        `,
  us: `# 任务日历
        `,
};

export default {
  category: 'Modules',
  subtitle: '任务日历',
  type: 'Project',
  zhType: '任务日历',
  title: 'SmTaskCalendars',
  render() {
    return (
      <div id="sm-task-calendars-demo">
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
