<script>
import Basic from './basic.md';
import Multiple from './multiple.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 定额分类定额选择树
        `,
  us: `# 定额分类定额选择树
        `,
};

export default {
  category: 'Modules',
  subtitle: '定额分类定额选择树',
  type: 'Basic',
  zhType: '数据库基础模块',
  title: 'SmStdBasicQuotaTreeSelect',
  render() {
    return (
      <div id="components-sm-std-basic-quota-tree-select-demo">
        <md cn={md.cn} us={md.us} />
        <Basic />
        <Multiple />
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
