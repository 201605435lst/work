
<script>
import Basic from './basic.md';
import Approval from './approval.md';
import Edit from './edit.md';
import View from './view.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 施工日志单页` ,
  us: `# 施工日志单页` ,
};

export default {
  title: 'SmConstructionDaily',
  render() {
    return (
      <div id="components-sm-construction-daily-demo">
        <md cn={md.cn} us={md.us} />
        <Basic />
        <Edit/>
        <View/>
        <Approval/>
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
    