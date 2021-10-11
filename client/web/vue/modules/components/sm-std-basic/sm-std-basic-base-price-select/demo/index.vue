<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 基价选择单页
        `,
  us: `# 基价选择单页
        `,
};

export default {
  category: 'Modules',
  subtitle: '标签选择单页',
  type: 'StdBasic',
  zhType: '标签选择单页',
  title: 'SmStdBasicBasePriceSelect',
  
  render() {
    return (
      <div id="components-sm-std-basic-base-price-select-demo">
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
