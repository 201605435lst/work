<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 图层面板
        `,
  us: `# 图层面板
        `,
};

export default {
  category: 'Modules',
  subtitle: '图层面板',
  type: 'D3',
  zhType: '3D 可视化',
  title: 'SmD3Layers',
  render() {
    return (
      <div id="sm-d3-layers-demo">
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
