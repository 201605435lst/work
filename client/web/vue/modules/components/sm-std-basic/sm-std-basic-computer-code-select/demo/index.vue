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
  subtitle: '电算代号选择框',
  type: 'StdBasic',
  zhType: '电算代号选择框',
  title: 'SmStdBasicComputerCodeSelect',
  
  render() {
    return (
      <div id="components-sm-std-basic-computer-code-select-demo">
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
