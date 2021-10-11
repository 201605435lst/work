<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 问题管理`,
  us: `# 问题管理`,
};

export default {
  category: 'Modules',
  subtitle: '问题管理',
  type: 'Problem',
  zhType: '问题库',
  title: 'SmProblemProblems',
  render() {
    return (
      <div id="components-sm-problem-problems-demo">
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
