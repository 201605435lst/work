<script>
import Basic from './basic.md';
import Static from './static.md';
import Approve from './approve.md';
import Approve2 from './approve2.md';
import Preview from './preview.md';
import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 文件管理
        `,
  us: `# 文件管理
        `,
};

export default {
  category: 'Modules',
  subtitle: '文件管理',
  type: 'File',
  zhType: '文件2',
  title: 'SmFileManage',
  render() {
    return (
      <div id="components-sm-file-manage-demo">
        <md cn={md.cn} us={md.us} />
        <Basic />
        <h1>静态文件</h1>
        <Static/>
        <h1>三维会审</h1>
        <Approve/>
        <h1>检验批</h1>
        <Approve2/>
        <h1>文件预览</h1>
        <Preview/>
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
