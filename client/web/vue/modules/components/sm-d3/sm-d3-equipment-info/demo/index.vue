<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 设备信息
        `,
  us: `# 设备信息
        `,
};

export default {
  category: 'Modules',
  subtitle: '设备信息',
  type: 'D3',
  zhType: '3D 可视化',
  title: 'SmD3EquipmentInfo',
  render() {
    return (
      <div id="sm-d3-scope-equipments-info-panel-demo">
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
