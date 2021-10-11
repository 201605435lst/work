
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 材料类别选择框` ,
  us: `# 材料类别选择框` ,
};

export default {
  title: 'SmMaterialMaterialTypeSelect',
  render() {
    return (
      <div id="components-sm-material-material-type-select-demo">
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
    