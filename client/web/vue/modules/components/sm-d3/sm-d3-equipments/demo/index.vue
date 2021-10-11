<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 设备列表
        `,
  us: `# 设备列表
        `,
};

export default {
  category: 'Modules',
  subtitle: '设备列表',
  type: 'D3',
  zhType: '3D 可视化',
  title: 'SmD3ScopeEquipments',
  render() {
    return (
      <div id="sm-d3-equipments-demo">
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
