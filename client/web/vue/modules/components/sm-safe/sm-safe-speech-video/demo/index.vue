
<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-safe-speech-video` ,
  us: `# sm-safe-speech-video` ,
};

export default {
  title: 'SmSafeSpeechVideo',
  render() {
    return (
      <div id="components-sm-safe-speech-video-demo">
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
    