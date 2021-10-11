
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sc-gantt` ,
  us: `# sc-gantt` ,
};

export default {
  title: 'ScGantt',
  render() {
    return (
      <div id="components-sc-gantt-demo">
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
    