<script>
import Basic from './basic.md';
import Select from './select.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 3D 可视化
        `,
  us: `# 3D 可视化
        `,
};

export default {
  category: 'Modules',
  subtitle: '3D 可视化',
  type: 'D3',
  zhType: '3D 可视化',
  title: 'SmD3',
  render() {
    return (
      <div id="sm-d3-demo">
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
