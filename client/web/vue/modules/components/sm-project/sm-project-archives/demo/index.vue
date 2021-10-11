<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 档案管理
        `,
  us: `# 档案管理
        `,
};

export default {
  category: 'Modules',
  subtitle: '档案管理',
  type: 'Project',
  zhType: '档案管理',
  title: 'SmProjectArchives',
  render() {
    return (
      <div id="components-sm-project-archives-basic-demo">
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

<style>
</style>
