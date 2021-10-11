
<script>
import Basic from './basic.md';
import MyIinitial from './my_initial.md';
import MyManage from './my_manage.md';
import MyCc from './my_cc.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 任务管理` ,
  us: `# 任务管理` ,
};

export default {
  category: 'Modules',
  subtitle: '任务管理',
  type: 'Task',
  zhType: '任务管理',
  title: 'SmTaskTasks',
  render() {
    return (
      <div id="sm-task-tasks-demo">
        <md cn={md.cn} us={md.us} />
        <Basic />
        <MyIinitial />
        <MyManage />
        <MyCc />
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
    