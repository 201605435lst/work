
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-construction-base-equipment-team` ,
  us: `# sm-construction-base-equipment-team` ,
};

export default {
  title: 'SmConstructionBaseEquipmentTeam',
  render() {
    return (
      <div id="components-sm-construction-base-equipment-team-demo">
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
    