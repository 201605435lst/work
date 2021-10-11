export const ModalStatus = {
  Hide: 0, // 隐藏
  Add: 1, // 添加
  Edit: 2, // 编辑
  View: 3, // 详情
};
//采购方式
export const PurchaseListTypeStatus = {
  MonthPurchasing: 1, //按月采购
  CentralPurchasing: 2, //集中采购
  MinorPurchasing: 3, //零星采购
};
//任务类型
export const UnplannedTaskType = {
  TemporaryDuty: 1, //临时任务
  OtherDuty: 2, //其他任务
};
//问题状态
export const SafeProblemState = {
  WaitingImprove: 1, //待整改
  WaitingVerifie: 2, //待审查
  Improved: 3, //已整改
};
//验证结果
export const SafeRecordState = {
  Checking: 1, //检查中
  NotPass: 2, //不通过
  Passed: 3, //通过
};
/// 问题记录类型，分为验证、整改
export const SafeRecordType = {
  Improve: 1, //整改记录
  Verify: 2, //验证记录
};
//安全问题类型
export const SafeFilterType = {
  All: 1, //全部
  MyChecked: 2, //我检查的
  MyWaitingImprove: 3, //待我整改
  MyWaitingVerify: 4, //待我验证
  CopyMine: 5, //抄送我的
};
export const MaterialType = {
  AutoCompute: 1, //辅助材料
  Appliance: 2, //使用器具
  Mechanical: 3, //使用机械
  SafetyArticle: 4, //安全防护用品
};
//接口标记类型
export const InterfaceFlagType = {
  InterfaceFlag: 1, //接口标记
  InterfaceFlagReform: 2, //接口整改
};
//构件跟踪预设状态
export const ComponentTrackTypeType = {
  Reserved: 1, //已预设
  NoReserved: 2, //未预设
};
//材料检测类型
export const MaterialAcceptanceTypeEnable = {
  Inspect: 1, //送检
  SelfInspection: 2, //自检
};
// 单页状态
export const PageState = {
  Add: 'add', // 添加模式
  Edit: 'edit', // 编辑模式
  View: 'view', // 查看模式
  Approval: 'approval', // 审批模式
};

//车站类型
export const StationsType = {
  Station: 0, //车站
  Section: 1, //区间
};

//机密类型
export const SecurityType = {
  secret: 1, //绝密
  confidential: 2, //机密
  common: 3, //普通
};

//线路类型
export const RailwayType = {
  Uniline: 0, //单线
  DoubleLine: 1, //复线
};

//线路方向
export const RailwayDirection = {
  Up: 0, //上行
  Down: 1, //下行
  UpAndDown: 2, //上下行
};

//构件跟踪
export const NodeType = {
  CheckOut: 1, //检验
  PutStorage: 2, //入库
  OutStorage: 3, //出库
  ToTest: 4, //到场检验
  Install: 5, //安装
  Alignment: 6, //调试
};
//机房状态
export const InstallationSiteState = {
  Using: 1, //在用
  Building: 2, //在建
};

//位置类型
export const InstallationSiteLocationType = {
  RailwayOuter: 1, //非沿线
  SectionInner: 2, //沿线区间
  StationInner: 3, //沿线站内
  Other: 4, //其它
};

//使用类别
export const InstallationSiteUseType = {
  Private: 1, //独用
  Share: 2, //共用
};

//计划完成状态
export const CompleteState = {
  Finish: 0, // 完成
  UnFinish: 1, // 未完成
  Changed: 2, // 已变更
};

// 计划完成状态
export const PlanFinishState = {
  UnFinish: 0, // 未完成
  Complete: 1, // 已完成
  All: 2, // 全部
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
  Key: 3, //重点检修
  Other: 4,
};

//维修项值类型
export const RepairValType = {
  Number: 1,
  Char: 2,
  Table: 3,
};
//验收结果
export const MaterialAcceptanceTestStatus = {
  ForAcceptance: 1, //待验收
  Approved: 2, //已验收
};
//检测结果
export const TestingResultType = {
  Qualified: 1, //合格
  Disqualification: 2, //不合格
};
//成员类型
export const MemberType = {
  Organization: 1, //组织
  Role: 2, //角色
  User: 3, //用户
  DynamicType: 1000, // 定义一个动态标签，用来记录所以动态组织的类型
  DynamicOrgLevel1: 21, // 动态组织1
  DynamicOrgLevel2: 22, // 动态组织2
  DynamicOrgLevel3: 23, // 动态组织3
  DynamicOrgLevel4: 24, // 动态组织4
  DynamicOrgLevel5: 25, // 动态组织5
  DynamicOrgLevel6: 26, // 动态组织6
  DynamicOrgLevel7: 27, // 动态组织7
  DynamicOrgLevel8: 28, // 动态组织8
  DynamicOrgLevel9: 29, // 动态组织9
};

//年月计划类型
export const YearMonthPlanType = {
  Year: 1, //年表
  Month: 2, //月表
  AnnualMonth: 3, //年度月表
};
//工作汇报类型
export const ReportType = {
  DayReport: 1, //周报
  WeekReport: 2, //周报
  MonthReport: 3, //月报
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
  UnPassed: 4, //审核驳回
};

// // 天窗类型
export const PlanType = {
  Vertical: 1, //垂直
  General: 2, //综合
  OutOf: 3, //点外
  All: 4, //全部
};

// 计划内容类型
export const WorkContentType = {
  MonthYearPlan: 1, //年月计划
  OtherPlan: 2, //其他计划
};

// 天窗计划状态
export const PlanState = {
  UnDispatching: 1, //未派工
  Dispatching: 2, //已派工
  NotIssued: 3, //未下发
  Issued: 4, //已下发
  Complete: 5, //已完成
  // Submited: 6, //已提交
  Waitting: 6, //审批中
  UnSubmited: 7, //待提交
  Revoke: 8, //已撤销,
  Adopted: 9, //已批复
  UnAdopted: 10, //未批复
  Backed: 11, //已退回
  OrderCancel: 12, //命令号取消
  NaturalDisasterCancel: 13, //自然灾害取消
  OtherReasonCancel: 14, //自然灾害取消
};
// 仓库是否启用
export const StoreHouseEnable = {
  All: '', // 所有
  Enable: true, // 是
  Unable: false, // 否
};
//维修级别
export const RepairLevel = {
  LevelI: 1, //天窗点内I级维修
  LevelII: 2, //天窗点内II级维修
  LevelIII: 3, //天窗外I级维修
  LevelIv: 4, //天窗点外II级维修
};

//维修周期单位
export const RepairPeriodUnit = {
  Other: 1, //其他
  Year: 2, //年
  Month: 3, //月
};

// 派工单状态
export const OrderState = {
  UnFinished: 0, //未完成
  Complete: 1, //已完成
  Acceptance: 2, //已验收
  OrderCancel: 3, //命令取消
  NaturalDisasterCancel: 4, //自然灾害取消
  OtherReasonCancel: 5, //其他原因取消
};

//天窗类型
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
  Rejected: 4, // 已退回
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

// 测试项类型
export const StandTestType = {
  Number: 1,
  String: 2,
  Excel: 3,
};

// 派工作业操作类型
export const SendWorkOperatorType = {
  Finish: 1, // 完成
  Acceptance: 2, // 验收
  Edit: 3, // 编辑
  View: 4, // 详情
};

// 栏目是否启用
export const CategoryEnable = {
  All: '', // 所有
  Enable: true, // 是
  Unable: false, // 否
};
// 设备是否出库入库
export const StoreEquipmentTransferTypeEnable = {
  Import: 1, // 入库
  Export: 2, // 出库
};
// oss 服务类型
export const OssServerType = {
  Aliyun: 'aliyun', // 阿里云
  MinIO: 'minio', // minio
  AmazonS3: 'amazons3', // 亚马逊s3
};

// 文件管理-->文件表格的类型
export const ResourceTableType = {
  Organization: 1, //我的组织
  Mine: 2, // 我的
  ShareCenter: 3, // 共享中心
  Delete: 4, // 回收站
  Static: 5, // 回收站
  Approve: 6, //审批
  DoneApprove:7,//已审批
};

//设备 安装位置类型
export const InstallationSiteType = {
  Station: 1, //车站
  Section: 2, //区间
  Other: 3, //其他
};

//设备 设备运行状态
export const RunningState = {
  PrimaryUse: 1, //主用
  SealUp: 2, //封存
  Standby: 3, //备用
  Scrap: 4, //报废
};

//题库 题目类型
export const QuestionType = {
  SingleChoice: 1, //单选
  MultipleChoice: 2, //多选
  TrueOrFalseQuestions: 3, //判断
  GapFilling: 4, //填空
  ShortAnswerQuestion: 5, //简答
};

//故障案例 故障状态
export const State = {
  UnSubmitted: 1, //未提交
  Submitted: 2, //已提交
  Pending: 3, //待处理
  UnChecked: 4, //未销记
  CheckedOut: 5, //已销记
};
export const DiaryState = {
  ToSubmit: 1, //未提交
  OnReview: 2, //审核中
  Pass: 3, //审核通过
};
//故障案例 来源
export const Source = {
  History: 1, //历史记录
  System: 2, //系统登记
};

//应急预案关联人员 处理状态
export const Group = {
  Launched: 1, //由我发起
  Waiting: 2, //待我处理
  Handled: 3, //我已处理
  Cc: 4, //抄送给我
};

//服务年限单位
export const ServiceLifeUnit = {
  Year: 1, //年
  Month: 2, //月
  Day: 3, //天
};

//库存设备 设备状态
export const StoreEquipmentState = {
  UnActived: 1, //未激活
  OnService: 2, //已安装
  WaitForTest: 3, //待检测
  Spare: 4, //备用
  Scrap: 5, //报废
};

//检测单 设备状态
export const StoreEquipmentTestState = {
  All: '', // 所有
  Qualified: true, // 合格
  Unqualified: false, // 不合格
};

// 设备类型
export const EquipmentType = {
  // [Description("默认")]
  Default: 1,
  // [Description("电缆")]
  Cable: 2,
};

/// <summary>
/// 电缆类型 1:电缆芯，2:光缆芯
/// </summary>
export const CableCoreType = {
  // [Description("电缆芯")]
  Electric: 1,

  // [Description("光缆芯")]
  Optical: 2,
};

//设备 设备上下道
export const EquipmentServiceRecordType = {
  Install: 1, // 安装
  UnInstall: 2, // 拆除
};

//设备属性分类
export const EquipmentPropertyType = {
  Default: 1, // 默认
  Extend: 2, // 扩展
  CableProperty: 3, //电缆特性
};
// 范围类型
export const ScopeType = {
  // 组织范围
  Organization: 1,
  // 线路范围
  Railway: 2,
  // 车站范围
  Station: 3,
  // 安装位置范围
  InstallationSite: 4,
};

// 电缆铺设类型
export const CableLayType = {
  // 管道
  Conduit: 1,
  // 架空
  Overhead: 2,
  // 直埋
  Bury: 3,
  // 室内槽道及竖井
  InnerChannelFlow: 4,
  // 室外槽道
  OuterChannelFlow: 5,
};

export const CableLocationDirection = {
  //[Description("水平方向")]
  Horizontal: 1,
  //[Description("垂直方向")]
  Vertical: 2,
  //[Description("直线距离")]
  Straight: 3,
};

export const CameraFlyState = {
  Stoped: 1,
  Pause: 2,
  Flying: 3,
};

export const RepairTags = {
  RailwayWired: 'RailwayWired', //有限科
  RailwayHighSpeed: 'RailwayHighSpeed', //高铁科
};

//数据字典 维修项类型key
export const RepairTagKeys = {
  RailwayWired: 'RepairTag.RailwayWired', //有线科
  RailwayHighSpeed: 'RepairTag.RailwayHighSpeed', //高铁科
};

export const RelateRailwayType = {
  SINGLELINK: 0, //单线
  UPLINK: 1, //上行
  DOWNLINK: 2, //下行
  UPANDDOWN: 3, //上下行
};

export const SealType = {
  Personal: 0, //个人
  Company: 1, //单位
};
export const ProjectState = {
  Building: 1, //建设中
  Finshed: 2, //竣工
  Acceptance: 3, //验收
  Stop: 4, //终止
  WaitSurvey: 5, //待勘察
  NoStart: 6, //未开始
  Start: 7, //未开始
};

export const BuildUnitType = {
  Supplier: 1, //供应商
  Owner: 2, //业主
};

export const AlarmLevel = {
  Emergency: 1, // 紧急告警
  Important: 2, // 重要告警
  Normal: 3, // 一般告警
  PreAlarm: 4, // 预警告警
};

export const BpmMessageType = {
  Notice: 0, // 通知类型
  Approval: 1, //通知类
  Cc: 2, // 抄送接收
};

// 任务状态
export const TaskGroup = {
  Initial: 1, //我发起的
  Manage: 2, // 我负责的
  Cc: 3, // 我参与的
};

// 优先级
export const PriorityType = {
  ImportantUrgent: 1, //重要紧急
  ImportantNoUrgent: 2, //重要不紧急
  NoImportantUrgent: 3, //不重要紧急
  NoImportantNoUrgent: 4, //不重要不紧急
};

//任务状态
export const StateType = {
  Processing: 1, //进行中
  Receive: 2, //已结项
  Finshed: 3, //已完成
  Refused: 4, //已驳回
  NoStart: 5, //未启动
  Stop: 6, //已暂停
};

//工作类型
export const WorkType = {
  AutoCompute: 1, //自动计算
  Milestone: 2, //里程碑
  Important: 3, //关键工作
  Unimportant: 4, //非关键工作
};

//计划状态
export const ScheduleState = {
  Processing: 1, //进行中
  Finshed: 2, //已完成
  Refused: 3, //已拒绝
  NoStart: 4, //未启动
  Stop: 5, //已暂停
};

//供应商类型
export const SupplierType = {
  Supplier: 1, //供应商
  Proprietor: 2, //业主
  ConstructionTeam: 3, //施工队
};

//供应商级别
export const SupplierLevel = {
  LevelI: 1, //一级供应商
  LevelII: 2, //二级供应商'
  LevelIII: 3, //三级供应商'
};

//供应商性质
export const SupplierProperty = {
  Unit: 1, //单位
  Personal: 2, //个人
};

//工序类型
export const ProcessType = {
  ManagemenetTask: 1, //管理任务
  ConstructionTask: 2, //施工任务
};
//电算代号类型
export const ComputerCodeType = {
  Artificial: 1, //人工
  Mechanics: 2, //机械
  Material: 3, //材料
};
//供应商性质
export const Category = {
  Auxiliary: 1, //辅助材料
  Appliance: 2, //器具
  Mechanical: 3, //机械
  SafetyArticle: 4, //安全防护用品
};

//模型精细等级
export const ModelDetailLevel = {
  GradeI: 1,
  GradeII: 2,
  GradeIII: 3,
  GradeIv: 4,
};
//审批意见
export const ApprovalIdea = {
  Consent: 1, //同意
  ConsentApply: 2, //同意申请
  Refuse: 3, //拒绝
  RefuseApply: 4, //拒绝申请
};
//接口检查情况
export const MarkType = {
  NoCheck: 1, //未检查
  Qualified: 2, //合格
  NoQualified: 3, //不合格
};
export const ConstructType = {
  Civil: 1, //土建工程
  Electric: 2, //四电工程
};
//配合作业完成情况
export const WorkTicketRltCooperationUnitState = {
  Finish: 1, //完成
  UnFinish: 2, //未完成
};
//问题标记类型
export const QualityProblemType = {
  A: 1, //A类
  B: 2, //B类
  C: 3, //C类
};
//问题标记等级
export const QualityProblemLevel = {
  Great: 1, //重大质量事故
  General: 2, //一般质量事故
  Minor: 3, //质量问题
};
//安全问题库风险等级
export const SafetyRiskLevel = {
  Especially: 1, //特别重大事故
  Great: 2, //重大事故
  Larger: 3, //较大事故
  General: 4, //一般事故
};

// 用料计划流程状态
export const ApprovalStatus = {
  ALL: 0, // 所有数据
  ToSubmit: 1, // 待提交
  OnReview: 2, // 审核中
  Pass: 3, // 审核通过
  UnPass: 4, // 审核未通过
};

// 采购清单状态
export const PurchaseState = {
  ToSubmit: 1, //待提交
  OnReview: 2, //审核中
  Pass: 3, //审核通过
  UnPass: 4, //审核未通过
};

// 采购类型
export const PurchaseListType = {
  ToSubmit: 1, //按月采购
  OnReview: 2, //集中采购
  Pass: 3, //零星采购
};

// 甘特图 item 编辑 标记
export const GanttItemState = {
  UnModify: 1, //未修改
  Edit: 2, //已编辑
  Add: 3, //已添加
  Delete: 4, //已删除
};

//派工单审批状态
export const DispatchState = {
  UnSubmit: 1, //待提交
  OnReview: 2, //审核中
  Pass: 3, //已通过
  UnPass: 4, //已驳回
};

//安全防护措施
export const SafetyMeasure = {
  InternalTraining: 1, //内部培训
  Ownar: 2, //自身装备
  NoSafetyRisk: 3, //现场环境无安全隐患
};

//工序控制类型
export const ControlType = {
  KeyProcess: 1, //关键工序
  GeneralProcess: 2, //一般工序
  HideProcess: 3, //隐蔽
  SideProcess: 4, //旁站
};
