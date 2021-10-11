<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 文件管理
        `,
  us: `# 文件管理
        `,
};

export default {
  category: 'Modules',
  subtitle: '文件管理',
  type: 'Project',
  zhType: '文件管理',
  title: 'SmProjectDossier',
  render() {
    return (
      <div id="components-sm-project-dossier-basic-demo">
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
