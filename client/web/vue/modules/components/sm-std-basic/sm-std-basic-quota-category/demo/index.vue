<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 定额分类
        `,
  us: `# 定额分类
        `,
};

export default {
  category: 'Modules',
  subtitle: '定额分类管理',
  type: 'StdBasic',
  zhType: '标准数据库基础模块',
  title: 'SmStdBasicQuotaCategory',
  render() {
    return (
      <div id="components-sm-std-basic-quota-category-demo">
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
