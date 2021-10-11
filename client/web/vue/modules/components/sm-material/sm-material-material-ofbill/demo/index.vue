
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 领料单管理` ,
  us: `# 领料单管理` ,
};

export default {
  title: 'SmMaterialMaterialOfbill',
  render() {
    return (
      <div id="components-sm-material-material-ofbill-demo">
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
    