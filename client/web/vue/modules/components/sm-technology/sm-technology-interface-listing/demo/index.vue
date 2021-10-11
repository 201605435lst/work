
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 接口清单` ,
  us: `# 接口清单` ,
};

export default {
  title: 'SmTechnologyInterfaceListing',
  render() {
    return (
      <div id="components-sm-technology-interface-listing-demo">
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
    