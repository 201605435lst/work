
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 物资验收` ,
  us: `# 物资验收` ,
};

export default {
  title: 'SmMaterialAcceptance',
  render() {
    return (
      <div id="components-sm-material-acceptance-demo">
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
    