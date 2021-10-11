
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-material-construction-team` ,
  us: `# sm-material-construction-team` ,
};

export default {
  title: 'SmMaterialConstructionTeam',
  render() {
    return (
      <div id="components-sm-material-construction-team-demo">
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
    