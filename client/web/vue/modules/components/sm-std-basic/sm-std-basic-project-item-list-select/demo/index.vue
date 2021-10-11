<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 工程工项列表选择框
        `,
  us: `# 工程工项列表选择框
        `,
};

export default {
  category: 'Modules',
  subtitle: '工程工项列表选择框',
  type: 'StdBasic',
  zhType: '基础',
  title: 'SmStdBasicProjectItemListSelect',
  render() {
    return (
      <div id="components-sm-std-basic-project-item-list-select-demo">
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
