
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-construction-daily-template` ,
  us: `# sm-construction-daily-template` ,
};

export default {
  title: 'SmConstructionDailyTemplate',
  render() {
    return (
      <div id="components-sm-construction-daily-template-demo">
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
    