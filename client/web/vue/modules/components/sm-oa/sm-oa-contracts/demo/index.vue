<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 合同管理
        `,
  us: `# 合同管理
        `,
};

export default {
  category: 'Modules',
  subtitle: '合同管理',
  type: 'Oa',
  zhType: '合同管理',
  title: 'SmOaContracts',
  render() {
    return (
      <div id="sm-oa-contracts-demo">
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
