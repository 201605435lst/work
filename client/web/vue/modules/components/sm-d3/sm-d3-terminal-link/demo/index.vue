<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 端子配线图
        `,
  us: `# 端子配线图
        `,
};

export default {
  category: 'Modules',
  subtitle: '端子配线图',
  type: 'D3',
  zhType: '3D 可视化',
  title: 'SmD3TerminalLink',
  render() {
    return (
      <div id="sm-d3-terminal-link-demo">
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
