
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 接口管理` ,
  us: `# 接口管理` ,
};

export default {
  title: 'SmD3Interface',
  render() {
    return (
      <div id="components-sm-d3-interface-demo">
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
    