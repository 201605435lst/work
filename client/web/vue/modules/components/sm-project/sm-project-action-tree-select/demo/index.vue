<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 下拉菜单
        `,
  us: `# 下拉菜单
        `,
};

export default {
  category: 'Modules',
  subtitle: '下拉菜单',
  type: 'Project',
  zhType: '下拉菜单',
  title: 'SmProjectActionTreeSelect',
  render() {
    return (
      <div id="components-sm-project-action-tree-select-demo">
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
