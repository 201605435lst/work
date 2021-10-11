<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 注意事项模态框
        `,
  us: `# 注意事项模态框
        `,
};

export default {
  category: 'Modules',
  subtitle: '注意事项模态框',
  type: 'StdBasic',
  zhType: '标准库基础模块',
  title: 'SmStdBasicWorkAttentionModal',
  render() {
    return (
      <div id="sm-std-basic-work-attention-modal-demo">
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
