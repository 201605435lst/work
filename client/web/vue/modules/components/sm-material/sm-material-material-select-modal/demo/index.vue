
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-material-material-select-modal` ,
  us: `# sm-material-material-select-modal` ,
};

export default {
  title: '材料选择弹窗',
  render() {
    return (
      <div id="components-sm-material-material-select-modal-demo">
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
