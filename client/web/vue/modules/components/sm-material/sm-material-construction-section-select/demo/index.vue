
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-material-construction-section-select` ,
  us: `# sm-material-construction-section-select` ,
};

export default {
  title: 'SmMaterialConstructionSectionSelect',
  render() {
    return (
      <div id="components-sm-material-construction-section-select-demo">
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
    