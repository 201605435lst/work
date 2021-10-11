
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-d-3-modal-select` ,
  us: `# sm-d-3-modal-select` ,
};

export default {
  title: 'SmD3ModalSelect',
  render() {
    return (
      <div id="components-sm-d-3-modal-select-demo">
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
    