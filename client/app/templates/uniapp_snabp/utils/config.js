/**
 * 分页器配置项
 */
export const pagination = {
  // 默认分页大小
  defaultPageSize: 10,
  // 分页下拉框显示格式
  buildOptionText: props => `${props.value} 条/页`,
  // 总数显示格式
  showTotal: x => `共 ${x} 条`,
};