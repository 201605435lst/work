<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 设备文件管理
        `,
  us: `# 设备文件管理
        `,
};

export default {
  category: 'Modules',
  subtitle: '设备文件管理',
  type: 'D3',
  zhType: '3D 可视化',
  title: 'SmD3EquipmentInfoFiles',
  render() {
    return (
      <div id="sm-d3-equipment-info-files-demo">
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
