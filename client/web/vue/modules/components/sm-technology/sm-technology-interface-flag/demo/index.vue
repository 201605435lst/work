
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 接口标记` ,
  us: `# 接口标记` ,
};

export default {
  title: 'SmTechnologyInterfaceFlag',
  render() {
    return (
      <div id="components-sm-technology-interface-flag-demo">
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
    