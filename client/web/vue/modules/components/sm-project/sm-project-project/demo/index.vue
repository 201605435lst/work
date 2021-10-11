<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 项目管理单页
        `,
  us: `# 项目管理单页
        `,
};

export default {
  category: 'Modules',
  subtitle: '项目管理单页',
  type: 'Project',
  zhType: '项目管理单页',
  title: 'SmProjectModule',
  render() {
    return (
      <div id="components-sm-project-basic-demo">
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
  h3{
    margin-bottom: 10px;
  }
</style>
