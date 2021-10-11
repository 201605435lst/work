//页面状态
export const PageState = {
	Add: 0, //添加
	Edit: 1, //编辑
	View: 2, //查看
};

//模块分类
export const ModulesType = {
	// 技术管理
	ConstructionDispatch: 0, //派工审批
	ConstructionDaily: 1, //派工日志
	ConstructionDailyApprove: 2, //日志审批

	//质量管理
	Interface: 3, //接口管理
	QualityOfAll: 4, //全部
	QualityOfChecked: 5, //我检查的
	QualityOfImproved: 6, //待整改的
	QualityOfVerified: 7, //待我验证
	QualityOfSended: 8, //抄送我的

	//安全管理
	SecurityOfAll: 9, //全部
	SecurityOfChecked: 10, //我检查的
	SecurityOfImproved: 11, //待整改的
	SecurityOfVerified: 12, //待我验证
	SecurityOfSended: 13, //抄送我的

	//成本管理
	Cost: 14, //盈亏分析
	Capital: 15, //资金管理
	Contract: 16, //合同管理
	LaborCost: 17, //人工成本
	OtherCost: 18, //其他成本

	//物资管理
	MaterialAcceptance: 19, //物资验收
	MaterialEntryRecords: 20, //入库管理
	MaterialOutRecords: 21, //出库管理
	MaterialInventory: 22, //库存台账
	MaterialOfBill: 23, //领料单管理

	//文件管理
	File: 24, //文件
	SharingCenter: 25, //共享中心
	RecycleBin: 26, //回收站
};
//接口清单状态
export const interfaceState = {
	Checking: 1, //未检查
	Passed: 2, //合格
	UnCheck: 3, //不合格
};
//派工审批状态
export const ApprovalState = {
	All: 0, //全部
	ToSubmit: 1, //待提交
	OnReview: 2, //待审批
	Pass: 3, //已审批
	UnPass: 4, //已驳回
};

//施工日志状态
export const DailyState = {
	All: 0, //全部
	ToSubmit: 1, //待提交
	OnReview: 2, //审核中
	Pass: 3, //已通过
	UnPass: 4, //未通过
};

//领料单管理
export const materialOfBillState = {
	UnCheck: 1, //待提交
	Checking: 2, //待审核
	Passed: 3, //已通过
};

//材料类型
export const MaterialType = {
	AutoCompute: 1, //辅助材料
	Appliance: 2, //使用器具
	Mechanical: 3, //使用机械
	SafetyArticle: 4, //安全防护用品
};

//材料类型
export const InventoryRecordType = {
	Entry: 0, //入库信息
	Out: 1, //出库信息
};

//质量安全问题状态
export const SafeQualityProblemState = {
	WaitingImprove: 1, //待整改
	WaitingVerify: 2, //待审查,待验证
	Improved: 3, //已整改
};

//质量安全问题数据过滤条件
export const SafeQualityFilterType = {
	All: 1, //全部
	MyChecked: 2, //我检查的
	MyWaitingImprove: 3, //待我整改
	MyWaitingVerify: 4, //待我验证
	CopyMine: 5, //	抄送我的
};

//质量问题报告记录类型
export const SafeQualityRecordType = {
	Improve: 1, //整改
	Verify: 2, //验证
};

//质量问题报告记录状态
export const SafeQualityRecordState = {
	Checking: 1, //检查中
	UnCheck: 2, //不通过
	Passed: 3, //已通过
};
//物资验证检测类型
export const MaterialTestingType = {
	Inspect: 1, //送检
	SelfInspection: 2, //自检
};
//物资验证检测状态
export const MaterialTestingStatus = {
	ForAcceptance: 1, //待验收
	Approved: 2, //已验收
};
//物资验证检测结果状态
export const MaterialTestState = {
	Qualified: 1, //合格
	Disqualification: 2, //不合格
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
//总体计划审批状态-临时任务类型
export const UnplannedTaskType = {
	TemporaryDuty: 1, //临时任务
	OtherDuty: 2, //其他任务
};
//构件跟踪节点类型
export const NodeType = {
	CheckOut: 1, //检验
	PutStorage: 2, //入库
	OutStorage: 3, //出库
	ToTest: 4, //到场检验
	Install: 5, //安装
	Alignment: 6, //调试
};
