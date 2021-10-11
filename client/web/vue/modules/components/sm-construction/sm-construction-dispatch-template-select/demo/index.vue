<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 派工模板选择框
        `,
  us: `# 派工模板选择框
        `,
};

export default {
  category: 'Modules',
  subtitle: '派工模板选择框',
  type: 'Construction',
  zhType: '内容系统',
  title: 'SmConstructionDispatchTemplateSelect',
  render() {
    return (
      <div id="components-sm-construction-dispatch-template-select-demo">
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
