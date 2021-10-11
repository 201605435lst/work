<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 项目选择框
        `,
  us: `# 项目选择框
        `,
};

export default {
  category: 'Modules',
  subtitle: '项目选择框',
  type: 'Project',
  zhType: '基础',
  title: 'SmProjectProjectSelect',
  render() {
    return (
      <div id="components-sm-project-project-select-demo">
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
