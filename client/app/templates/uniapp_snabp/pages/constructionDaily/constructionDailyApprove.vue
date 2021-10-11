<template>
	<view id="construction-constructionDailyApprove">
		<!-- 加载中 -->
		<uniLoading v-show="loading" class="uniLoading" />
		<view v-show="!loading">
			<view class="basic-row" v-for="(item, index) of listData" :key="index">
				<text class="basic-col-title minW140">{{ item.name }}</text>
				<text class="basic-col-content">{{ listDataSource[item.key] || '--' }}</text>
			</view>

			<text>施工任务</text>
			<wyb-table :headers="dailyRltPlanColumns" :contents="tableDataSource.dailyRltPlan" />
			<text>设备信息</text>
			<wyb-table :headers="equipmentsColumns" :contents="tableDataSource.equipments" />
			<text>临时任务</text>
			<wyb-table :headers="unplannedTaskColumns" :contents="tableDataSource.unplannedTask" />
			<text>存在的安全问题</text>
			<wyb-table :headers="dailyRltSafeColumns" :contents="tableDataSource.dailyRltSafe" />
			<text>存在的质量问题</text>
			<wyb-table :headers="dailyRltSafeColumns" :contents="tableDataSource.dailyRltQuality" />

			<text class="underscore">其他内容</text>
			<view class="basic-row">
				<text class="basic-col-title">施工现场照片：</text>
				<pickerImage v-if="dailyRltFiles.length > 0" :disabled="true" :url="getUploadUrl()" v-model="dailyRltFiles" />
				<text v-else>--</text>
			</view>
			<view class="basic-row" :style="{marginBottom: pageState == PageState.View ? '120rpx' : ''}">
				<text class="basic-col-title">其他内容：</text>
				<text class="basic-col-content">{{ dataSource.remark || '--' }}</text>
			</view>
			<view v-if="pageState != PageState.View" class="basic-row" style="margin-bottom: 120rpx">
				<text class="basic-col-title" style="max-width: 140rpx; min-width: 140rpx">
					审批意见
					<text style="color: #df0000; font-size: 28rpx">*</text>
				</text>
				<textarea maxlength="-1" auto-height placeholder="请输入审批意见" v-model="content" />
			</view>

			<view v-if="pageState != PageState.View" class="save-button">
				<button style="background-color: #de3636; margin-right: 20rpx" @tap="submit(ApprovalState.UnPass)">驳回</button>
				<button style="background-color: #28bf49" @tap="submit(ApprovalState.Pass)">通过</button>
			</view>
		</view>
	</view>
</template>

<script>
import {PageState, ApprovalState} from '@/utils/enum.js';
import {
	requestIsSuccess,
	showToast,
	getUploadUrl,
	getUnplannedTaskType,
	getProblemStateTitle,
	getQualityProblemType,
} from '@/utils/util.js';
import * as apiDaily from '@/api/construction/daily.js';
import pickerImage from '@/components/pickerImage.vue';
import moment from 'moment';
export default {
	components: {pickerImage},
	data() {
		return {
			ApprovalState,
			PageState,
			id: '',
			loading: true, //页面加载动画
			pageState: '',
			dataSource: [],
			listData: [
				{name: '日志编号:', key: 'code'},
				{name: '填报日期:', key: 'date'},
				{name: '填报人:', key: 'informant'},
				{name: '天气:', key: 'weathers'},
				{name: '温度:', key: 'temperature'},
				{name: '风力风向:', key: 'windDirection'},
				{name: '空气质量:', key: 'airQuality'},
				{name: '施工班组:', key: 'team'},
				{name: '施工人员:', key: 'builderCount'},
				{name: '施工部位:', key: 'location'},
				{name: '施工总结:', key: 'summary'},
			],
			listDataSource: {}, //列表数据源
			tableDataSource: {dailyRltPlan: [], equipments: [], unplannedTask: [], dailyRltSafe: [], dailyRltQuality: []}, //表格数据源
			dailyRltFiles: [], //材料附件
			content: '', //审批意见
		};
	},
	computed: {
		dailyRltPlanColumns() {
			return [
				{label: '任务名称', key: 'name'},
				{label: '单位', key: 'unit'},
				{label: '工程量', key: 'quantity'},
				{label: '当天完成', key: 'count'},
				{label: '当天完成量', key: 'currentCount'},
				{label: '累计完成量', key: 'sumCount'},
			];
		},
		equipmentsColumns() {
			return [
				{label: '设备名称', key: 'name'},
				{label: '设备分类', key: 'componentCategoryName'},
				{label: '规格型号', key: 'spec'},
				{label: '计量单位', key: 'unit'},
				{label: '工程量', key: 'quantity'},
			];
		},
		unplannedTaskColumns() {
			return [
				{label: '任务类型', key: 'taskType'},
				{label: '任务说明', key: 'content'},
			];
		},
		dailyRltSafeColumns() {
			return [
				{label: '问题类型', key: 'type'},
				{label: '问题名称', key: 'title'},
				{label: '问题发起人', key: 'checker'},
				{label: '问题状态', key: 'state'},
			];
		},
	},
	onLoad(option) {
		this.pageState = option.pageState;
		this.id = option.id;
		this.refresh(option.id);
	},
	methods: {
		getUploadUrl,
		async refresh(id) {
			let response = await apiDaily.get(id);

			if (requestIsSuccess(response)) {
				let resData = response.data;
				this.dataSource = resData;
				// 获取列表数据源
				this.listDataSource = {
					code: resData.code,
					date: moment(resData.date).format('YYYY-MM-DD'),
					informant: resData.informant && resData.informant.userName ? resData.informant.userName : '',
					weathers: resData.weathers,
					temperature: resData.temperature,
					windDirection: resData.windDirection,
					airQuality: resData.airQuality,
					team: resData.team,
					builderCount: resData.builderCount,
					location: resData.location,
					summary: resData.summary,
				};
				// 获取表格数据源
				//施工任务
				if (resData.dailyRltPlan && resData.dailyRltPlan.length > 0) {
					for (const item of resData.dailyRltPlan) {
						let item_ = item.planMaterial;
						let res = await apiDaily.getDailyRltPlanMaterial(item.planMaterialId);
						let _sumCount = 0;
						if (requestIsSuccess(res)) {
							_sumCount = res.data;
						}
						let quantity = item_.quantity;
						let count = item.count;
						let currentCount = Number((count / quantity) * 100).toFixed(2) + '%';
						let sumCount = Number(((count + _sumCount) / quantity) * 100).toFixed(2) + '%';

						this.tableDataSource.dailyRltPlan.push({
							name: item_.planContent ? item_.planContent.name : '--',
							unit: item_.unit || '--',
							quantity: item_.quantity || '--',
							count: item.count,
							currentCount: currentCount,
							sumCount: sumCount,
						});
					}
					console.log(this.tableDataSource.dailyRltPlan);
				}
				// 设备信息
				let equipments = [];
				resData.dailyRltPlan && resData.dailyRltPlan.length > 0
					? resData.dailyRltPlan.map(item => {
							let item_ = item.planMaterial.planMaterialRltEquipments;
							item_ && item_.length > 0
								? item_.map(_item_ => {
										equipments.push(_item_.equipment);
								  })
								: '';
					  })
					: '';
				this.tableDataSource.equipments = equipments.map(item => {
					//spec,unit获取是根据F:\snabp\client\web\vue\modules\components\sm-construction\sm-construction-daily的249行处获得
					let spec = item.productCategory ? item.productCategory.name : '';
					let unit = item.componentCategory && item.componentCategory.unit ? item.componentCategory.unit : '--';
					let name = item.componentCategory && item.componentCategory.name ? item.componentCategory.name : '--';
					return {
						name: item.name || '--',
						spec: spec || '--',
						unit: unit || '--',
						quantity: item.quantity || '--',
						componentCategoryName: name || '--',
					};
				});
				// 临时任务
				this.tableDataSource.unplannedTask =
					resData.unplannedTask && resData.unplannedTask.length > 0
						? resData.unplannedTask.map(item => {
								return {
									taskType: getUnplannedTaskType(item.taskType) || '--',
									content: item.content || '--',
								};
						  })
						: [];
				// 存在的安全问题
				this.tableDataSource.dailyRltSafe =
					resData.dailyRltSafe && resData.dailyRltSafe.length > 0
						? resData.dailyRltSafe.map(item => {
								let item_ = item.safeProblem;
								return {
									type: item_.type && item_.type.name ? item_.type.name : '--',
									title: item_.title || '--',
									checker: item_.checker && item_.checker.name ? item_.checker.name : '--',
									state: getProblemStateTitle(item_.state) || '--',
								};
						  })
						: [];
				// 存在的质量问题
				this.tableDataSource.dailyRltQuality =
					resData.dailyRltQuality && resData.dailyRltQuality.length > 0
						? resData.dailyRltQuality.map(item => {
								let item_ = item.qualityProblem;
								return {
									type: getQualityProblemType(item_.type) || '--',
									title: item_.title || '--',
									checker: item_.checker && item_.checker.name ? item_.checker.name : '--',
									state: getProblemStateTitle(item_.state) || '--',
								};
						  })
						: [];
				// 获取材料附件
				this.dailyRltFiles = resData.dailyRltFiles.map(item => {
					return item.file;
				});
			}
			this.loading = false;
		},
		async submit(state) {
			if (this.content != '') {
				let formData = {
					planId: this.id,
					content: this.content,
					status: state == ApprovalState.Pass ? ApprovalState.Pass : state == ApprovalState.UnPass ? ApprovalState.UnPass : '',
				};
				console.log(formData);
				uni.showModal({
					title: '提示',
					content: '是否确认当前选择',
					success: async ress => {
						if (ress.confirm) {
							if (!this.$store.state.isSubmit) {
								this.$store.commit('SetIsSubmit', true);
								let response = await apiDaily.process(formData);
								if (requestIsSuccess(response)) {
									showToast(state == ApprovalState.Pass ? '已通过' : state == ApprovalState.UnPass ? '已驳回' : '');
								} else {
									showToast('操作失败', false);
								}
							}
						}
					},
				});
			} else {
				uni.showToast({
					icon: 'none',
					title: '请填写审批意见',
					duration: 2000,
				});
			}
		},
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},
};
</script>

<style lang="scss">
page {
	background-color: #f9f9f9;
}
#construction-constructionDailyApprove {
	font-size: 28rpx;
	color: #656565;
	padding: 20rpx;
	.basic-row {
		display: flex;
		padding: 10rpx 0;
		.basic-col-title {
			color: #525252;
			font-weight: bold;
			text-align-last: justify;
			margin-right: 15rpx;
			display: inline-block;
		}
		.basic-col-content {
			display: inline-block;
		}
		.raido {
			padding: 0 10rpx;
		}
		textarea {
			width: 100%;
			height: 110rpx;
			padding: 10rpx;
			border: 1rpx solid #cccccc;
			border-radius: 20rpx;
			background-color: white;
		}
	}
	.save-button {
		width: 100%;
		display: flex;
		position: fixed;
		z-index: 90;
		bottom: 0;
		left: 0;
		background-color: #f9f9f9;
		padding: 20rpx 0;
	}
	.save-button > button {
		color: #ffffff;
		width: 100%;
		border-radius: 100rpx;
		margin: 0 20rpx;
	}
	.underscore {
		background: linear-gradient(90deg, grey 66%, transparent 0) repeat-x;
		background-size: 1rpx 1rpx;
		background-position: 0 60rpx;
	}
}
#construction-constructionDailyApprove > view > text {
	color: #40a7fc;
	padding: 15rpx 0;
	display: block;
}
::v-deep #construction-constructionDailyApprove .basic-row textarea {
	padding: 10rpx 20rpx;
	border-radius: 10rpx;
}
::v-deep #construction-constructionDailyApprove .uni-loading-more-page {
	background-color: #f2f2f6;
}
</style>
