<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 签章选择
        `,
  us: `# 签章选择
        `,
};

export default {
  category: 'Modules',
  subtitle: '签章选择',
  type: 'Oa',
  zhType: '签章选择',
  title: 'SmOaSealsSelect',
  render() {
    return (
      <div>
        <md cn={md.cn} us={md.us} />
        <div id="select-baseic">
          <Basic />
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
h3 {
  color: #0d1a26;
  font-family: Avenir, -apple-system, BlinkMacSystemFont, Segoe UI, PingFang SC, Hiragino Sans GB,
    Microsoft YaHei, Helvetica Neue, Helvetica, Arial, sans-serif, Apple Color Emoji, Segoe UI Emoji,
    Segoe UI Symbol;
  font-variant: tabular-nums;
  margin: 1.6em 0 0.6em;
  font-weight: 500;
  clear: both;
}
</style>
