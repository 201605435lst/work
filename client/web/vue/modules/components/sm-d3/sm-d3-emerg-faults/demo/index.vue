<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 故障应急
        `,
  us: `# 故障应急
        `,
};

export default {
  category: 'Modules',
  subtitle: '故障应急',
  type: 'D3',
  zhType: '3D 可视化',
  title: 'SmD3EmergFaults',
  render() {
    return (
      <div id="sm-d3-scope-emerg-faults-panel-demo">
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
