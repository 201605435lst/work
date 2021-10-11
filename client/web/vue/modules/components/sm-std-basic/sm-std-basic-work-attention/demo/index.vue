<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 作业注意事项管理
        `,
  us: `# 作业注意事项管理
        `,
};

export default {
  category: 'Modules',
  subtitle: '作业注意事项管理',
  type: 'StdBasic',
  zhType: '标准库基础模块',
  title: 'SmStdBasicWorkAttention',
  render() {
    return (
      <div id="components-sm-std-basic-work-attention-demo">
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

