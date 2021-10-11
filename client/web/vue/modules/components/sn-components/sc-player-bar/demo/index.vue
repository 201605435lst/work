
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sc-player-bar` ,
  us: `# sc-player-bar` ,
};

export default {
  title: 'ScPlayerBar',
  render() {
    return (
      <div id="components-sc-player-bar-demo">
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
    