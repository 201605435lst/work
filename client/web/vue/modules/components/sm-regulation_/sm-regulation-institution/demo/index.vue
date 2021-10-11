<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 全部制度
        `,
  us: `# 全部制度
        `,
};

export default {
  category: 'Modules',
  subtitle: '全部制度',
  type: 'Regulation',
  zhType: '全部制度',
  title: 'SmRegulationInstitution',
  
  render() {
    return (
      <div id="components-sm-regulation-institution-demo">
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
