
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-construction-dispatch-select` ,
  us: `# sm-construction-dispatch-select` ,
};

export default {
  title: 'SmConstructionDispatchSelect',
  render() {
    return (
      <div id="components-sm-construction-dispatch-select-demo">
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
    