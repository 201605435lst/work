
<script>
import Basic from './basic.md';
import Modal from './modal.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-video` ,
  us: `# sm-video` ,
};

export default {
  title: 'SmVideo',
  render() {
    return (
      <div id="components-sm-video-demo">
        <md cn={md.cn} us={md.us} />
        <h1>基础播放器</h1>
        <Basic />
        <h1>预览弹窗</h1>
        <Modal/>
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
    