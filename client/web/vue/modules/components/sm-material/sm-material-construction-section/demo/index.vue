
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-material-construction-section` ,
  us: `# sm-material-construction-section` ,
};

export default {
  title: 'SmMaterialConstructionSection',
  render() {
    return (
      <div id="components-sm-material-construction-section-demo">
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
    