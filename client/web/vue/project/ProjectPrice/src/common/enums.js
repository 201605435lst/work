export const ModalStatus = {
  Hide: 0,
  Add: 1,
  Edit: 2,
  View: 3,
};

// 单页状态
export const PageState = {
  Add: 'add', // 添加模式
  Edit: 'edit', // 编辑模式
  View: 'view', // 查看模式
};

//车站类型
export const StationsType = {
  Station: 1, //车站
  Section: 2, //区间
};

//计划完成状态
export const CompleteState = {
  Finish: 0, // 完成
  UnFinish: 1, // 未完成
  Changed: 2, // 已变更
};

//年月表类型
export const DateReportType = {
  Year: 1, //年表
  Month: 2, //月表
};

//维修类别
export const RepairType = {
  Muster: 1, //集中检修
  Daily: 2, //日常检修
};

//维修项值类型
export const RepairValType = {
  Number: 1,
  Char: 2,
  Table: 3,
};

//成员类型
export const MemberType = {
  Organization: 1, //组织
  Role: 2, //角色
  User: 3, //用户
};

//年月计划类型
export const YearMonthPlanType = {
  Year: 1, //年表
  Month: 2, //月表
  AnnualMonth: 3, //年度月表
};

//添加待选计划类型
export const SelectablePlanType = {
  Year: 1, //年表
  HalfYaer: 2, //半年表
  QuarterYear: 3, //季度表
  Month: 4, //月表
};

export const YearMonthPlanState = {
  UnCommit: 0, //未提交
  UnCheck: 1, //待审核
  Checking: 2, //审核中
  Passed: 3, //审核通过
  UnPassed: 4, //审核不通过
};

// // 天窗类型
export const PlanType = {
  Vertical: 1, //垂直
  General: 2, //综合
  OutOf: 3, //点外
  All: 4, //全部
};


// 天窗计划状态
export const PlanState = {
  NoPublish: 0, //未发布
  Publish: 1, //已发布
  Backout: 2, //已撤销
  Dispatching: 3, //已派工
  Complete: 4, //已完成
  NotIssued: 5, //未下发
  Issued: 6,
};

//维修级别
export const RepairLevel = {
  LevelI: 1, //天窗点内I级维修
  LevelII: 2, //天窗点内II级维修
  LevelIII: 3, //天窗外I级维修
  LevelIv: 4, //天窗点外II级维修
};

// 派工单状态
export const OrderState = {
  UnFinished: 0, //未完成
  Complete: 1, //已完成
  Acceptance: 2, //已验收
};

//计划类型
export const SkylightType = {
  Vertical: 1, //垂直
  General: 2, //综合
  OutOf: 3, //点外
  All: 4, //全部
  Other: 5, //其他
};

// 工作流状态
export const WorkflowState = {
  All: 0, // 所有
  Waiting: 1, // 待审批
  Finished: 2, // 已完成
  Stopped: 3, // 已终止
};

// 用户工作流群组
export const UserWorkflowGroup = {
  Initial: 1, // 【用户】发起的
  Waiting: 2, // 等等【用户】审批的
  Approved: 3, // 【用户】审批过的
  Cc: 4, // 抄送给【用户】的
};

// 工作流节点状态
export const WorkflowStepState = {
  Approved: 1, // 通过
  Rejected: 2, // 驳回
  Stopped: 3, // 终止
};

// 检验状态
export const CheckoutState = {
  Qualified: 0, // 合格
  UnQualified: 1, // 不合格
};

//故障案例 故障状态
export const State = {
  UnSubmitted: 1, //未提交
  Submitted: 2, //已提交
  Pending: 3, //待处理
  UnChecked: 4, //未销记
  CheckedOut: 5, //已销记
};

export const RepairTags = {
  RailwayWired: "RailwayWired", //有限科
  RailwayHighSpeed: "RailwayHighSpeed", //高铁科
};
// 派工作业操作类型
export const SendWorkOperatorType = {
  Finish: 1, // 完成
  Acceptance: 2, // 验收
  Edit: 3, // 编辑
  View: 4, // 详情
};