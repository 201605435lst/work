<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 高德地图
        `,
  us: `# 高德地图
        `,
};

export default {
  category: 'Modules',
  subtitle: '高德地图',
  type: 'Components',
  zhType: '高德地图',
  title: 'SnMapAmap',
  render() {
    return (
      <div id="components-sn-map-amap-demo">
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

<style>
  h3{
    margin-bottom: 10px;
  }
</style>
