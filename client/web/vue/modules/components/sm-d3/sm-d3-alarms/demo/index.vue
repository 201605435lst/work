
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-d3-alarms` ,
  us: `# sm-d3-alarms` ,
};

export default {
  title: 'SmD3Alarms',
  render() {
    return (
      <div id="components-sm-d3-alarms-demo">
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
