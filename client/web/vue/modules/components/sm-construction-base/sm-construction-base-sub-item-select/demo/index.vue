
<script>
import Basic from './basic.md';
import Multiple from './multiple.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-construction-base-sub-item-select` ,
  us: `# sm-construction-base-sub-item-select` ,
};

export default {
  title: 'SmConstructionBaseSubItemSelect',
  render() {
    return (
      <div id="components-sm-construction-base-sub-item-select-demo">
        <md cn={md.cn} us={md.us} />
        <div id="select-baseic">
          <Basic />
        </div>
        <div id="select-signle">
          <Multiple />
        </div>
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
