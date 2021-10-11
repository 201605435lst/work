
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 供应商管理` ,
  us: `# 供应商管理` ,
};

export default {
  title: 'SmMaterialSuppliers',
  render() {
    return (
      <div id="components-sm-material-suppliers-demo">
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
    