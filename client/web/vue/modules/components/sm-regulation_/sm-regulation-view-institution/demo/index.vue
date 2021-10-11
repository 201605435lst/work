<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 查看制度
        `,
  us: `# 查看制度
        `,
};

export default {
  category: 'Modules',
  subtitle: '查看制度',
  type: 'Regulation',
  zhType: '查看制度',
  title: 'SmRegulationViewInstitution',
  
  render() {
    return (
      <div id="components-sm-regulation-view-institution-demo">
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
