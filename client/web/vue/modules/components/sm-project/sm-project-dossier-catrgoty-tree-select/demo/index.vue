<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 卷宗管理选择树
        `,
  us: `# 卷宗管理选择树
        `,
};

export default {
  category: 'Modules',
  subtitle: '卷宗管理选择树',
  type: 'Basic',
  zhType: '项目管理模块',
  title: 'SmProjectDossierCatrgotyTreeSelect',
  render() {
    return (
      <div id="components-sm-project-dossier-catrgoty-tree-select-demo">
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
