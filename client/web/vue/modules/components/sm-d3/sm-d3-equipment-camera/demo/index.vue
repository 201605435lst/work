<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 相机视角
        `,
  us: `# 相机视角
        `,
};

export default {
  category: 'Modules',
  subtitle: '相机视角',
  type: 'D3',
  zhType: '3D 可视化',
  title: 'SmD3EquipmentCamera',
  render() {
    return (
      <div id="sm-d3-equipment-camera-demo">
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
