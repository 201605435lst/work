<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 制度审核
        `,
  us: `# 制度审核
        `,
};

export default {
  category: 'Modules',
  subtitle: '制度审核',
  type: 'Regulation',
  zhType: '制度审核',
  title: 'SmRegulationAuditedInstitution',
  
  render() {
    return (
      <div id="components-sm-regulation-audited-institution-demo">
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
