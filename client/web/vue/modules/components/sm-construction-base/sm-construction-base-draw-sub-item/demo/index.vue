
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-construction-base-draw-sub-item` ,
  us: `# sm-construction-base-draw-sub-item` ,
};

export default {
  title: 'SmConstructionBaseDrawSubItem',
  render() {
    return (
      <div id="components-sm-construction-base-draw-sub-item-demo">
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
    