
<script>
import Basic from './basic.md';
import Custom from './custom.md';
import Bind from './bind.md';
import Directory from './directory.md';
import Multiple from './multiple.md';
import Theme from './theme.md';
import View from './view.md';
import ViewDownload from './viewdownload.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# sm-file-upload` ,
  us: `# sm-file-upload` ,
};

export default {
  title: 'SmFileUpload',
  render() {
    return (
      <div id="components-sm-file-upload-demo">
        <md cn={md.cn} us={md.us} />
        <div class="upload-container">
          <Basic class="span" />
          <Custom class="span" />
          <Bind class="span" />
          <Directory class="span" />
          <Multiple class="span" />
          <Theme class="span" />
          <ViewDownload class="span" />
          <View class="span" />
        </div>
        <api>
          <CN slot="cn" />
          <US />
        </api>
      </div>
    );
  },
};
</script>

<style>
.upload-container{
  display: flex;
  flex-wrap: wrap;  
}
.span{
  width:50%;
  padding: 10px;
}
</style>
    