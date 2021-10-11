
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 接口报告` ,
  us: `# 接口报告` ,
};

export default {
  title: 'SmTechnologyInterfaceReport',
  render() {
    return (
      <div id="components-sm-technology-interface-report-demo">
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
    