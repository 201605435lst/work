
<script>
import Basic from './basic.md';
import CN from './../index.zh-CN.md';
import US from './../index.en-US.md';
const md = {
  cn: `# 日志统计` ,
  us: `# 日志统计` ,
};

export default {
  title: 'SmScheduleDiarysStatistics',
  render() {
    return (
      <div id="components-sm-schedule-diarys-statistics-demo">
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
    