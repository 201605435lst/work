<script>
import Basic from './basic.md';

import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';

const md = {
  cn: `# 信息交换模板属性管理
        `,
  us: `# 信息交换模板属性管理
        `,
};

export default {
  category: 'Modules',
  subtitle: '信息交换模板属性管理',
  type: 'StdBasic',
  zhType: '标准库基础模块',
  title: 'SmStdBasicMvdProperty',
  render() {
    return (
      <div id="components-sm-std-basic-mvd-property-demo">
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

<style>
/* #components-badge-demo .ant-badge:not(.ant-badge-not-a-wrapper) {
  margin-right: 20px;
}
#components-badge-demo .head-example {
  width: 42px;
  height: 42px;
  border-radius: 4px;
  background: #eee;
  display: inline-block;
  vertical-align: middle;
}
#components-badge-demo .ant-badge-not-a-wrapper:not(.ant-badge-status) {
  margin-right: 8px;
} */
</style>
