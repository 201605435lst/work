<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 电缆埋深
        `,
  us: `# 电缆埋深
        `,
};

export default {
  category: 'Modules',
  subtitle: '电缆埋深',
  type: 'D3',
  zhType: '3D 可视化',
  title: 'SmD3CableLocation',
  render() {
    return (
      <div id="sm-d3-cable-location-demo">
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
