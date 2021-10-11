import {
	ModulesType,
	ApprovalState,
	materialOfBillState,
	interfaceState,
	DailyState,
	InventoryRecordType,
	SafeQualityProblemState,
	SafeQualityRecordType,
	SafeQualityRecordState,
	SafeQualityFilterType,
	MaterialTestingType,
	MaterialTestingStatus,
	MaterialTestState,
	QualityProblemType,
	QualityProblemLevel,
	SafetyRiskLevel,
	UnplannedTaskType,
} from './enum.js';
import config from '../config.js';
import $store from '../store/index.js';
import moment from 'moment';
//获取功能模块标题
export function getModulesTypeTitle(status) {
	let title = '';
	switch (status) {
		// 施工管理
		case ModulesType.ConstructionDispatch:
			title = '派工审批';
			break;
		case ModulesType.ConstructionDaily:
			title = '日志管理';
			break;
		case ModulesType.ConstructionDailyApprove:
			title = '日志审批';
			break;
		//质量管理
		case ModulesType.Interface:
			title = '接口管理';
			break;
		case ModulesType.QualityOfAll:
			title = '全部';
			break;
		case ModulesType.QualityOfChecked:
			title = '我检查的';
			break;
		case ModulesType.QualityOfImproved:
			title = '待我整改';
			break;
		case ModulesType.QualityOfVerified:
			title = '待我验证';
			break;
		case ModulesType.QualityOfSended:
			title = '抄送我的';
			break;
		//安全管理
		case ModulesType.SecurityOfAll:
			title = '全部';
			break;
		case ModulesType.SecurityOfChecked:
			title = '我检查的';
			break;
		case ModulesType.SecurityOfImproved:
			title = '待我整改';
			break;
		case ModulesType.SecurityOfVerified:
			title = '待我验证';
			break;
		case ModulesType.SecurityOfSended:
			title = '抄送我的';
			break;
		//成本管理
		case ModulesType.Cost:
			title = '盈亏分析';
			break;
		case ModulesType.Capital:
			title = '资金管理';
			break;
		case ModulesType.Contract:
			title = '合同管理';
			break;
		case ModulesType.LaborCost:
			title = '人工成本';
			break;
		case ModulesType.OtherCost:
			title = '其他成本';
			break;
		//物资管理
		case ModulesType.MaterialAcceptance:
			title = '物资验收';
			break;
		case ModulesType.MaterialEntryRecords:
			title = '入库管理';
			break;
		case ModulesType.MaterialOutRecords:
			title = '出库管理';
			break;
		case ModulesType.MaterialInventory:
			title = '库存台账';
			break;
		case ModulesType.MaterialOfBill:
			title = '领料单管理';
			break;
		//文件管理
		case ModulesType.File:
			title = '文件';
			break;
		case ModulesType.SharingCenter:
			title = '共享中心';
			break;
		case ModulesType.RecycleBin:
			title = '回收站';
			break;
		default:
			break;
	}
	return title;
}
//接口清单状态
export function getInterfaceStateTitle(status) {
	let title = '';
	switch (status) {
		case interfaceState.UnCheck:
			title = '不合格';
			break;
		case interfaceState.Checking:
			title = '未检查';
			break;
		case interfaceState.Passed:
			title = '合格';
			break;
		default:
			break;
	}
	return title;
}
//质量问题状态
export function getQuality(status) {
	let title = '';
	switch (status) {
		case SafeQualityProblemState.WaitingImprove:
			title = '待整改';
			break;
		case SafeQualityProblemState.WaitingVerify:
			title = '待验证';
			break;
		case SafeQualityProblemState.Improved:
			title = '已通过';
			break;
		default:
			break;
	}
	return title;
}
//质量问题报告记录类型
export function getQualityOfRecordType(status) {
	let title = '';
	switch (status) {
		case SafeQualityRecordType.Improve:
			title = '整改';
			break;
		case SafeQualityRecordType.Verify:
			title = '验证';
			break;
		default:
			break;
	}
	return title;
}
//质量问题报告记录状态
export function getQualityOfRecordState(status) {
	let title = '';
	switch (status) {
		case SafeQualityRecordState.Checking:
			title = '检查中';
			break;
		case SafeQualityRecordState.UnCheck:
			title = '不通过';
			break;
		case SafeQualityRecordState.Passed:
			title = '已通过';
			break;
		default:
			break;
	}
	return title;
}
//施工审批状态
export function getDiarysStateTitle(status) {
	let title = '';
	switch (status) {
		case ApprovalState.ToSubmit:
			title = '待提交';
			break;
		case ApprovalState.OnReview:
			title = '待审批';
			break;
		case ApprovalState.Pass:
			title = '已审批';
			break;
		case ApprovalState.UnPass:
			title = '已驳回';
			break;
		default:
			break;
	}
	return title;
}

//施工日志状态
export function getDailyStateTile(status) {
	let title = '';
	switch (status) {
		case DailyState.ToSubmit:
			title = '待提交';
			break;
		case DailyState.OnReview:
			title = '审核中';
			break;
		case DailyState.Pass:
			title = '已通过';
			break;
		case DailyState.UnPass:
			title = '未通过';
			break;
		default:
			break;
	}
	return title;
}

//领料单管理
export function getMaterial(status) {
	let title = '';
	switch (status) {
		case materialOfBillState.UnCheck:
			title = '待提交';
			break;
		case materialOfBillState.Checking:
			title = '待审核';
			break;
		case materialOfBillState.Passed:
			title = '已通过';
			break;
		default:
			break;
	}
	return title;
}

//材料类型
export function getInventoryRecordTypeTitle(status) {
	let title = '';
	switch (status) {
		case InventoryRecordType.Entry:
			title = '入库信息';
			break;
		case InventoryRecordType.Out:
			title = '出库信息';
			break;
		default:
			break;
	}
	return title;
}

//安全问题类型
export function getProblemStateTitle(status) {
	let title = '';
	switch (status) {
		case SafeQualityProblemState.WaitingImprove:
			title = '待整改';
			break;
		case SafeQualityProblemState.WaitingVerify:
			title = '待验证';
			break;
		case SafeQualityProblemState.Improved:
			title = '已整改';
			break;
		default:
			break;
	}
	return title;
}

export function requestIsSuccess(response) {
	return response && (response.statusCode === 200 || response.statusCode === 201 || response.statusCode === 204);
}

export function timeFix() {
	const time = new Date();
	const hour = time.getHours();
	return hour < 9 ? '早上好' : hour <= 11 ? '上午好' : hour <= 13 ? '中午好' : hour < 20 ? '下午好' : '晚上好';
}

// 生产 Guid
export function CreateGuid() {
	function S4() {
		return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
	}
	return S4() + S4() + '-' + S4() + '-' + S4() + '-' + S4() + '-' + S4() + S4() + S4();
}

export function checkToken() {
	//获取token判断是否登录状态
	let token = uni.getStorageSync('token');
	if (!token) {
		uni.reLaunch({
			url: '../login/login',
		});
	}
}

/**
 * 获取一个文件的完整路径
 * 如：/upload/xx.img -> http://www.xxx.com/api + /upload/xx.img
 * 如：http://xxx.com/avatarl.jpg -> http://xxx.com/avatarl.jpg
 * @param {*} url
 */
export function getFileUrl(url) {
	let fileServerEndPoint = 'http:' + uni.getStorageSync('fileServerEndPoint');
	return url.indexOf('http') > -1 ? url : fileServerEndPoint + url;
}

export function getUploadUrl() {
	let url = uni.getStorageSync('remoteServiceBaseUrl') + '/api/app/fileFile/uploadForApp';
	return url;
}

export function getFileExt(url) {
	var arr = url.split('.');
	return arr.length > 1 ? arr[arr.length - 1] : null;
}

// 显示消息提示框并返回上一界面/type:0是显示后退出，1是不退出
export function showToast(title, type = true) {
	if (type == false) {
		uni.showToast({
			icon: 'none',
			title: title,
			duration: 2000,
		});
		$store.commit('SetIsSubmit', false);
	} else {
		$store.commit('SetIsSubmit', true);
		uni.showToast({
			icon: 'none',
			title: title,
			duration: 10000,
		});
		setTimeout(() => {
			uni.hideToast();
			uni.navigateBack();
			//防止多次点击
			setTimeout(() => {
				$store.commit('SetIsSubmit', false);
			}, 500);
		}, 2000);
	}
}
//质量安全设置当前页面的标题
export function setNavigationBarTitle(type) {
	if (type == SafeQualityFilterType.All) {
		uni.setNavigationBarTitle({
			title: '全部',
		});
	} else if (type == SafeQualityFilterType.MyChecked) {
		uni.setNavigationBarTitle({
			title: '我检查的',
		});
	} else if (type == SafeQualityFilterType.MyWaitingImprove) {
		uni.setNavigationBarTitle({
			title: '待我整改',
		});
	} else if (type == SafeQualityFilterType.MyWaitingVerify) {
		uni.setNavigationBarTitle({
			title: '待我验证',
		});
	} else if (type == SafeQualityFilterType.CopyMine) {
		uni.setNavigationBarTitle({
			title: '抄送我的',
		});
	}
}

//物资验证检测类型
export function getMaterialTestingType(status) {
	let title = '';
	switch (status) {
		case MaterialTestingType.Inspect:
			title = '送检';
			break;
		case MaterialTestingType.SelfInspection:
			title = '自检';
			break;
		default:
			break;
	}
	return title;
}

//物资验证检测状态
export function getMaterialTestingStatus(status) {
	let title = '';
	switch (status) {
		case MaterialTestingStatus.ForAcceptance:
			title = '待验收';
			break;
		case MaterialTestingStatus.Approved:
			title = '已验收';
			break;
		default:
			break;
	}
	return title;
}

//物资验证检测结果状态
export function getMaterialTestState(status) {
	let title = '';
	switch (status) {
		case MaterialTestState.Qualified:
			title = '合格';
			break;
		case MaterialTestState.Disqualification:
			title = '不合格';
			break;
		default:
			break;
	}
	return title;
}
//问题标记类型
export function getQualityProblemType(status) {
	let title = '';
	switch (status) {
		case QualityProblemType.A:
			title = 'A类';
			break;
		case QualityProblemType.B:
			title = 'B类';
			break;
		case QualityProblemType.C:
			title = 'C类';
			break;
		default:
			break;
	}
	return title;
}
//问题标记等级
export function getQualityProblemLevel(status) {
	let title = '';
	switch (status) {
		case QualityProblemLevel.Great:
			title = '重大质量事故';
			break;
		case QualityProblemLevel.General:
			title = '一般质量事故';
			break;
		case QualityProblemLevel.Minor:
			title = '质量问题';
			break;
		default:
			break;
	}
	return title;
}

//安全问题库风险等级
export function getSafetyRiskLevel(status) {
	let title = '';
	switch (status) {
		case SafetyRiskLevel.Especially:
			title = '特别重大事故';
			break;
		case SafetyRiskLevel.Great:
			title = '重大事故';
			break;
		case SafetyRiskLevel.Larger:
			title = '较大事故';
			break;
		case SafetyRiskLevel.General:
			title = '一般事故';
			break;
		default:
			break;
	}
	return title;
}

export function getEquipmentTrackType(type) {
	let title = '';
	let userType = '';
	let timeType = '';
	switch (type) {
		case 1:
			title = '验收';
			userType = '验收人';
			timeType = '验收时间';
			break;
		case 2:
			title = '入库';
			userType = '录入人';
			timeType = '入库时间';
			break;
		case 3:
			title = '出库';
			userType = '录入人';
			timeType = '出库时间';
			break;
		case 4:
			title = '到场检验';
			userType = '操作人';
			timeType = '检验时间';
			break;
		case 5:
			title = '安装';
			userType = '操作人';
			timeType = '安装时间';
			break;
		case 6:
			title = '调试';
			userType = '操作人';
			timeType = '调试时间';
			break;
		default:
			title = '未定义';
			userType = '未定义';
			break;
	}
	return {
		title,
		userType,
		timeType,
	};
}
/**
 * @desc 函数防抖
 * @param func 函数
 * @param wait 延迟执行毫秒数
 * @param immediate true 表立即执行，false 表非立即执行
 */
export function debounce(func, wait, immediate) {
	var timeout;

	return function () {
		var context = this;
		var args = arguments;

		if (timeout) clearTimeout(timeout);
		if (immediate) {
			var callNow = !timeout;
			timeout = setTimeout(function () {
				timeout = null;
			}, wait);
			if (callNow) func.apply(context, args);
		} else {
			timeout = setTimeout(function () {
				func.apply(context, args);
			}, wait);
		}
	};
}
/**
 * @desc 函数节流
 * @param func 函数
 * @param wait 延迟执行毫秒数
 * @param type 1 表时间戳版，2 表定时器版
 */
export function throttle(func, wait, type) {
	if (type === 1) {
		var previous = 0;
	} else if (type === 2) {
		var timeout;
	}

	return function () {
		var context = this;
		var args = arguments;
		if (type === 1) {
			var now = Date.now();

			if (now - previous > wait) {
				func.apply(context, args);
				previous = now;
			}
		} else if (type === 2) {
			if (!timeout) {
				timeout = setTimeout(function () {
					timeout = null;
					func.apply(context, args);
				}, wait);
			}
		}
	};
}

//总体计划审批状态-临时任务类型
export function getUnplannedTaskType(status) {
	let title = '';
	switch (status) {
		case UnplannedTaskType.TemporaryDuty:
			title = '临时任务';
			break;
		case UnplannedTaskType.OtherDuty:
			title = '其他任务';
			break;
		default:
			break;
	}
	return title;
}

/**
 * 是否拥有权限
 * @param {Array} allPermissions 所有权限数组
 * @param {String} permission 当前权限组
 */
export function vP(allPermissions, permission) {
	if (permission instanceof Array) {
		for (let item of permission) {
			if (allPermissions && allPermissions.find(x => x === item) != null) {
				return true;
			}
		}
	} else if (typeof permission === 'string') {
		if (allPermissions && allPermissions.find(x => x === permission) != null) {
			return true;
		}
	}
	return false;
}

//获取权限
export function getPermissions() {
	let rst = [];
	let string = JSON.stringify(uni.getStorageSync('permissions'));
	if (string) {
		rst = JSON.parse(string);
	}
	return rst;
}

/**
 * 生成编码Code: xx + 时间
 * @param {String} str 头部名称
 */
export function getCode(str) {
	let num = '';
	let date = moment().format('YYYY-MM-DD-HH-mm-ss');
	for (let item of date) {
		item != '-' ? (num = num + item) : '';
	}
	let code = str + '-' + num;
	return code;
}

/**
 * 弹出系统等待对话框
 * @param {boolean} isShow 是否显示等待对话框
 */
export function showWaiting(isShow = true) {
	// #ifdef APP-PLUS
	isShow
		? plus.nativeUI.showWaiting('加载中...', {
				width: '100%',
				height: '100%',
				color: '#7f7f7f',
				style: 'black',
				background: '#f2f2f6',
		  })
		: plus.nativeUI.closeWaiting();
	// #endif
}

/**
 * 判断选择时间是否存在于此范围
 * @function setTimeRange()
 * @param {String} date 所选时间
 * @param {String} before 在此时间之前，包含
 * @param {String} after 在此时间之后，包含
 * @param {String} compare 与当前时间进行比较
 */
export function setTimeRange(date, options) {
	let result = false;
	let Time = moment(date).format('YYYY-MM-DD');
	let currentTime = moment().format('YYYY-MM-DD');
	Time = moment(Time).format('X');
	currentTime = moment(currentTime).format('X');
	if (options.before) {
		let isBefore = moment(options.before).format('YYYY-MM-DD');
		isBefore = moment(isBefore).format('X');
		Time <= isBefore ? (result = true) : (result = false);
	}
	if (options.after) {
		let iAfter = moment(options.after).format('YYYY-MM-DD');
		iAfter = moment(iAfter).format('X');
		Time >= iAfter ? (result = true) : (result = false);
	}
	if (options.before && options.after) {
		Time <= isBefore && Time >= iAfter ? (result = true) : (result = false);
	}
	if (options.compare) {
		let compare = options.compare;
		if (compare == '>') {
			Time > currentTime ? (result = true) : (result = false);
		} else if (compare == '<') {
			Time < currentTime ? (result = true) : (result = false);
		}
		if (result) {
			uni.showToast({
				icon: 'none',
				title: `时间不能${compare == '>' ? '大于' : compare == '<' ? '小于' : ''}当前时间`,
				duration: 2000,
			});
		}
	}
	return result;
}

/**
 * 字符串的正则匹配
 * 用于输入框判断
 * @function conformToRegular()
 * @param {String} str 内容
 * @param {String} regular 正则
 * @param {String} isType 是否是此类型
 */
export function conformToRegular(str = '', regular) {
	if (str) {
		let rule;
		switch (regular) {
			case 1:
				/** 数字 */
				rule = /^[0-9]*$/;
				break;

			case 2:
				/** n位的数字 */
				rule = /^\d{n}$/;
				break;

			case 3:
				/** 至少n位的数字 */
				rule = /^\d{n,}$/;
				break;

			case 4:
				/** m-n位的数字 */
				rule = /^\d{m,n}$/;
				break;

			case 5:
				/** 零和非零开头的数字 */
				rule = /^(0|[1-9][0-9]*)$/;
				break;

			case 6:
				/** 非零开头的最多带两位小数的数字 */
				rule = /^([1-9][0-9]*)+(\.[0-9]{1,2})?$/;
				break;

			case 7:
				/** 带1-2位小数的正数或负数 */
				rule = /^(\-)?\d+(\.\d{1,2})$/;
				break;

			case 8:
				/** 正数、负数、和小数 */
				rule = /^(\-|\+)?\d+(\.\d+)?$/;
				break;

			case 9:
				/** 有两位小数的正实数 */
				rule = /^[0-9]+(\.[0-9]{2})?$/;
				break;

			case 10:
				/** 非零的正整数*/
				rule = /^[1-9]\d*$/;
				// rule = /^([1-9][0-9]*){1,3}$/;
				// rule = /^\+?[1-9][0-9]*$/;
				break;

			case 11:
				/** 非零的负整数 */
				rule = /^\-[1-9][]0-9"*$/;
				// rule = /^-[1-9]\d*$/;
				break;

			case 12:
				/** 非负整数 */
				rule = /^\d+$/;
				// rule = /^[1-9]\d*|0$/;
				break;

			case 13:
				/** 非负浮点数 */
				rule = /^\d+(\.\d+)?$/;
				// rule = /^[1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0$/;
				break;

			case 14:
				/** 正浮点数 */
				rule = /^[1-9]\d*\.\d*|0\.\d*[1-9]\d*$/;
				// rule = /^[1-9]\d*\.\d*|0\.\d*[1-9]\d*$ 或 ^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$/;
				break;

			case 15:
				/** 浮点数 */
				rule = /^(-?\d+)(\.\d+)?$/;
				// rule = /^-?([1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0)$/;
				break;

			case 16:
				/** 汉字 */
				rule = /^[\u4e00-\u9fa5]{0,}$/;
				break;

			case 17:
				/** 英文和数字 */
				rule = /^[A-Za-z0-9]+$/;
				// rule = /^[A-Za-z0-9]{4,40}$/;
				break;

			case 18:
				/** 长度为3-20的所有字符 */
				rule = /^.{3,20}$/;
				break;

			case 19:
				/** 由26个英文字母组成的字符串 */
				rule = /^[A-Za-z]+$/;
				break;

			case 20:
				/** 由数字和26个英文字母组成的字符串 */
				rule = /^[A-Za-z0-9]+$/;
				break;

			case 21:
				/** 由数字、26个英文字母或者下划线组成的字符串 */
				rule = /^\w+$/;
				// rule = /^\w{3,20}$/;
				break;

			case 22:
				/** 中文、英文、数字包括下划线 */
				rule = /^[\u4E00-\u9FA5A-Za-z0-9_]+$/;
				break;

			case 23:
				/** Email地址 */
				rule = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
				break;

			case 24:
				/** 域名 */
				rule = /[a-zA-Z0-9][-a-zA-Z0-9]{0,62}(\.[a-zA-Z0-9][-a-zA-Z0-9]{0,62})+\.?/;
				break;

			case 25:
				/** 手机号码 */
				rule = /^(13[0-9]|14[5|7]|15[0|1|2|3|4|5|6|7|8|9]|18[0|1|2|3|5|6|7|8|9])\d{8}$/;
				break;

			case 26:
				/** 帐号是否合法(字母开头，允许5-16字节，允许字母数字下划线) */
				rule = /^[a-zA-Z][a-zA-Z0-9_]{4,15}$/;
				break;

			case 27:
				/** 密码(以字母开头，长度在6~18之间，只能包含字母、数字和下划线) */
				rule = /^[a-zA-Z]\w{5,17}$/;
				break;

			case 28:
				/** 强密码(必须包含大小写字母和数字的组合，不能使用特殊字符，长度在 8-10 之间) */
				rule = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])[a-zA-Z0-9]{8,10}$/;
				break;

			case 29:
				/** 强密码(必须包含大小写字母和数字的组合，可以使用特殊字符，长度在8-10之间) */
				rule = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,10}$/;
				break;

			default:
				break;
		}
		return rule.test(str);
	} else {
		return false;
	}
}

/**
 * 文件大小计算
 * @function getFileSize()
 * @param {Number} size 文件大小
 */
export function getFileSize(size) {
	var num = 1024.0; //byte
	if (size < num) return size + 'B';
	if (size < Math.pow(num, 2)) return (size / num).toFixed(2) + 'K'; //kb
	if (size < Math.pow(num, 3)) return (size / Math.pow(num, 2)).toFixed(2) + 'M'; //M
	if (size < Math.pow(num, 4)) return (size / Math.pow(num, 3)).toFixed(2) + 'G'; //G
	return (size / Math.pow(num, 4)).toFixed(2) + 'T'; //T
}
/**
 * 由文件类型判断出图标
 * @function getFileTypeIcon()
 * @param {String} type
 */
export function getFileTypeIcon(type) {
	var type_ = '';
	if (['.doc', '.docx'].indexOf(type) != -1) {
		type_ = require('../static/BIMAppUI/fileTypeIcon/doc_docx.png');
	} else if (['文件夹'].indexOf(type) != -1) {
		type_ = require('../static/BIMAppUI/fileTypeIcon/fileFolder.png');
	} else if (['链接'].indexOf(type) != -1) {
		type_ = require('../static/BIMAppUI/fileTypeIcon/link.png');
	} else if (['拍照'].indexOf(type) != -1) {
		type_ = require('../static/BIMAppUI/fileTypeIcon/photograph.png');
	} else if (['.exe'].indexOf(type) != -1) {
		type_ = require('../static/BIMAppUI/fileTypeIcon/exe.png');
	} else if (['.gif'].indexOf(type) != -1) {
		type_ = require('../static/BIMAppUI/fileTypeIcon/gif.png');
	} else if (['.js', '.cs', '.vbs', '.class', '.dll', '.so'].indexOf(type) != -1) {
		type_ = require('../static/BIMAppUI/fileTypeIcon/js_cs_vbs.png');
	} else if (['.mp4', '.rm', '.avi', '.mov', '.mpg', '.wmv', '.rmvb'].indexOf(type) != -1) {
		type_ = require('../static/BIMAppUI/fileTypeIcon/mp4_rm_avi_mov_mpg.png');
	} else if (['.pdf'].indexOf(type) != -1) {
		type_ = require('../static/BIMAppUI/fileTypeIcon/pdf.png');
	} else if (['.png', '.jpg', '.bmp', '.tif'].indexOf(type) != -1) {
		type_ = require('../static/BIMAppUI/fileTypeIcon/png_jpg_bmp.png');
	} else if (['.ppt', '.pptx'].indexOf(type) != -1) {
		type_ = require('../static/BIMAppUI/fileTypeIcon/ppt_pptx.png');
	} else if (['.txt'].indexOf(type) != -1) {
		type_ = require('../static/BIMAppUI/fileTypeIcon/txt.png');
	} else if (['.wav', '.mp3', '.wma'].indexOf(type) != -1) {
		type_ = require('../static/BIMAppUI/fileTypeIcon/wav_mp3_wma.png');
	} else if (['.xlsx', '.xls'].indexOf(type) != -1) {
		type_ = require('../static/BIMAppUI/fileTypeIcon/xlsx_xls.png');
	} else if (['.zip', '.rer', '.arj', '.7z', '.tar', '.gz'].indexOf(type) != -1) {
		type_ = require('../static/BIMAppUI/fileTypeIcon/zip_rer_arj_7z_tar_gz.png');
	} else {
		type_ = require('../static/BIMAppUI/fileTypeIcon/z.png');
	}
	return type_;
}
