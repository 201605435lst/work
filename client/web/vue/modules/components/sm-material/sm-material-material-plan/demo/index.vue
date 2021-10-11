
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 用料计划` ,
  us: `# 用料计划` ,
};

export default {
  title: 'SmMaterialMaterialPlan',
  render() {
    return (
      <div id="components-sm-material-material-plan-demo">
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
    