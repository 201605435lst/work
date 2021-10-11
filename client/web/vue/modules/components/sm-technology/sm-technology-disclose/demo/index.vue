<script>
import Basic from './basic.md';
import Disclose from './disclose.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 三维交底` ,
  us: `# 三维交底` ,
};

export default {
  title: 'SmTechnologyDisclose',
  render() {
    return (
      <div id="components-sm-technology-disclose-demo">
        <md cn={md.cn} us={md.us} />
        <h1>技术交底</h1>
        <Basic />
        <h1>安全技术交底</h1>
        <Disclose />
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
