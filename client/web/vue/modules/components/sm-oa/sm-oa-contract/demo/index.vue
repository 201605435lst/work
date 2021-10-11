<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 合同管理单页
        `,
  us: `# 合同管理单页
        `,
};

export default {
  category: 'Modules',
  subtitle: '合同管理单页',
  type: 'Oa',
  zhType: '合同管理单页',
  title: 'SmOaContract',
  render() {
    return (
      <div id="sm-oa-contract-demo">
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
