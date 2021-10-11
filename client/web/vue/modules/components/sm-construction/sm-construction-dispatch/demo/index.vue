<script>
import Basic from './basic.md';
import Approve from './approve.md';
import Edit from './edit.md';
import View from './view.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 派工管理单页`,
  us: `# 派工管理单页`,
};

export default {
  title: 'SmConstructionDispatch',
  render() {
    return (
      <div id="components-sm-construction-dispatch-demo">
        <md cn={md.cn} us={md.us} />
        <Basic />
        <Edit />
        <View />
        <Approve />
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
