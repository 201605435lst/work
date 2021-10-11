
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 物资查询` ,
  us: `# 物资查询` ,
};

export default {
  title: 'SmMaterialInquire',
  render() {
    return (
      <div id="components-sm-material-inquire-demo">
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
    