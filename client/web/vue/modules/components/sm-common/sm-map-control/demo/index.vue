
<script>
import Basic from './basic.md';
import Mark from './mark.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-map-control` ,
  us: `# sm-map-control` ,
};

export default {
  title: 'SmMapControl',
  render() {
    return (
      <div id="components-sm-map-control-demo">
        <md cn={md.cn} us={md.us} />
        <Mark />
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
    