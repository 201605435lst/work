
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-technology-quantity` ,
  us: `# sm-technology-quantity` ,
};

export default {
  title: 'SmTechnologyQuantity',
  render() {
    return (
      <div id="components-sm-technology-quantity-demo">
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
    