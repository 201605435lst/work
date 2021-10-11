<template>
	<view id="construction-constructionDispatch">
		<!-- 加载中 -->
		<uniLoading v-show="loading" class="uniLoading" />
		<view v-show="!loading" style="padding: 20rpx">
			<view class="basic-row" v-for="(item, index) of listData" :key="index">
				<text
					class="basic-col-title"
					:style="{
						maxWidth: item.name.length < 6 ? '140rpx' : '138rpx',
						minWidth: item.name.length < 6 ? '140rpx' : '138rpx',
					}"
				>
					{{ item.name }}
				</text>
				<text
					class="basic-col-content"
					:style="{display: index >= listData.length - 2 ? 'flex' : '', alignItems: index >= listData.length - 2 ? 'center' : ''}"
				>
					{{ listDataSource[item.key] || '--' }}
				</text>
			</view>
			<text>施工任务</text>
			<wyb-table :headers="planContentsColumns" :contents="tableDataSource.planContents" />
			<text>待安装设备</text>
			<wyb-table :headers="equipmentsColumns" :contents="tableDataSource.equipments" />
			<text>待安装材料</text>
			<wyb-table :headers="materialsColumns" :contents="tableDataSource.materials" />
			<view class="basic-row">
				<text class="basic-col-title minW140">补充说明：</text>
				<text class="basic-col-content">{{ dataSource.extraDescription || '--' }}</text>
			</view>
			<text class="underscore">其他内容</text>
			<view class="basic-row">
				<text class="basic-col-title">是否需要大型设备吊装：</text>
				<view>
					<label class="raido">
						<radio disabled="true" :checked="dataSource.isNeedLargeEquipment" style="transform: scale(0.7)" />
						是
					</label>
					<label class="radio">
						<radio disabled="true" :checked="!dataSource.isNeedLargeEquipment" style="transform: scale(0.7)" />
						否
					</label>
				</view>
			</view>
			<view class="basic-row">
				<text class="basic-col-title">是否涉及空调围蔽拆除：</text>
				<view>
					<label class="raido">
						<radio disabled="true" :checked="dataSource.isDismantle" style="transform: scale(0.7)" />
						是
					</label>
					<label class="radio">
						<radio disabled="true" :checked="!dataSource.isDismantle" style="transform: scale(0.7)" />
						否
					</label>
				</view>
			</view>
			<view class="basic-row">
				<text class="basic-col-title">是否高空作业：</text>
				<view>
					<label class="raido">
						<radio disabled="true" :checked="dataSource.isHighWork" style="transform: scale(0.7)" />
						是
					</label>
					<label class="radio">
						<radio disabled="true" :checked="!dataSource.isHighWork" style="transform: scale(0.7)" />
						否
					</label>
				</view>
			</view>
			<view class="basic-row">
				<text class="basic-col-title minW140">大型设备吊装器具：</text>
				<text class="basic-col-content alignItems">{{ dataSource.largeEquipment || '--' }}</text>
			</view>
			<view class="basic-row">
				<text class="basic-col-title minW140">处理方式：</text>
				<text class="basic-col-content">{{ dataSource.process || '--' }}</text>
			</view>
			<view class="basic-row">
				<text class="basic-col-title minW138">安全风险源：</text>
				<text class="basic-col-content alignItems">{{ dataSource.riskSources || '--' }}</text>
			</view>
			<view class="basic-row">
				<text class="basic-col-title minW138">安全防护措施：</text>
				<view class="alignItems">
					<checkbox-group>
						<label class="radio">
							<checkbox
								disabled="true"
								:checked="
									dataSource.safetyMeasure && dataSource.safetyMeasure.length > 0
										? dataSource.safetyMeasure.includes(1)
										: false
								"
								style="transform: scale(0.7)"
							/>
							内部培训
						</label>
						<label class="radio">
							<checkbox
								disabled="true"
								:checked="
									dataSource.safetyMeasure && dataSource.safetyMeasure.length > 0
										? dataSource.safetyMeasure.includes(2)
										: false
								"
								style="transform: scale(0.7)"
							/>
							自身防护
						</label>
						<label class="radio">
							<checkbox
								disabled="true"
								:checked="
									dataSource.safetyMeasure && dataSource.safetyMeasure.length > 0
										? dataSource.safetyMeasure.includes(3)
										: false
								"
								style="transform: scale(0.7)"
							/>
							无安全隐患
						</label>
					</checkbox-group>
				</view>
			</view>
			<view class="basic-row">
				<text class="basic-col-title minW138">计划恢复时间：</text>
				<text class="basic-col-content alignItems">{{ dataSource.recoveryTime || '--' }}</text>
			</view>
			<view class="basic-row">
				<text class="basic-col-title minW138">工序控制类型：</text>
				<view class="alignItems">
					<checkbox-group>
						<label class="radio" v-for="(item, index) of ['关键工序', '一般工序', '隐蔽', '旁站', '其他']" :key="index">
							<checkbox
								:checked="
									dataSource.controlType && dataSource.controlType.length > 0
										? dataSource.controlType.includes(index + 1)
										: false
								"
								disabled="true"
								style="transform: scale(0.7)"
							/>
							{{ item }}
						</label>
					</checkbox-group>
				</view>
			</view>
			<view class="basic-row">
				<text class="basic-col-title minW140">材料附件：</text>
				<pickerImage v-if="dispatchRltFiles.length > 0" :disabled="true" :url="getUploadUrl()" v-model="dispatchRltFiles" />
				<text v-else>--</text>
			</view>
			<view class="basic-row" :style="{marginBottom: pageState == PageState.View ? '120rpx' : ''}">
				<text class="basic-col-title minW140">其他事宜：</text>
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
				<button style="background-color: #de3636" @tap="submit(ApprovalState.UnPass)">驳回</button>
				<button style="background-color: #28bf49" @tap="submit(ApprovalState.Pass)">通过</button>
			</view>
		</view>
	</view>
</template>

<script>
import {PageState, ApprovalState} from '@/utils/enum.js';
import {requestIsSuccess, showToast, getUploadUrl} from '@/utils/util.js';
import * as apiDispatch from '@/api/construction/dispatch.js';
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
				{name: '派工编号:', key: 'code'},
				{name: '施工专业:', key: 'profession'},
				{name: '施工区段:', key: 'dispatchRltSections'},
				{name: '承包商:', key: 'contractor'},
				{name: '施工班组:', key: 'team'},
				{name: '派单日期:', key: 'time'},
				{name: '工序指引:', key: 'dispatchRltStandards'},
				{name: '现场施工员:', key: 'dispatchRltWorkers'},
				{name: '施工人员数量:', key: 'number'},
			],
			listDataSource: {}, //列表数据源
			tableDataSource: {planContents: [], equipments: [], materials: []}, //表格数据源
			dispatchRltFiles: [], //材料附件
			content: '', //审批意见
		};
	},
	computed: {
		planContentsColumns() {
			return [
				{label: '任务名称', key: 'name'},
				{label: '工作内容', key: 'content'},
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
		materialsColumns() {
			return [
				{label: '材料名称', key: 'name'},
				{label: '规格型号', key: 'spec'},
				{label: '计量单位', key: 'unit'},
				{label: '工程数量', key: 'count'},
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
			let response = await apiDispatch.get(id);

			if (requestIsSuccess(response)) {
				let resData = response.data;
				let safetyMeasure = [],
					controlType = [];
				for (const item of resData.safetyMeasure) {
					item != ',' ? safetyMeasure.push(Number(item)) : '';
				}
				for (const item of resData.controlType) {
					item != ',' ? controlType.push(Number(item)) : '';
				}
				this.dataSource = {
					...resData,
					recoveryTime: resData.recoveryTime ? moment(resData.recoveryTime).format('YYYY-MM-DD HH:mm:ss') : '--',
					safetyMeasure: safetyMeasure,
					controlType: controlType,
				};

				// 获取列表数据源
				this.listDataSource = {
					code: resData.code,
					profession: resData.profession,
					dispatchRltSections:
						resData.dispatchRltSections.length > 0
							? resData.dispatchRltSections
									.map(item => (item.section && item.section.name ? item.section.name : ''))
									.join('/')
							: '--',
					contractor: resData.contractor ? resData.contractor.name : '',
					team: resData.team,
					time: resData.time ? moment(resData.time).format('YYYY-MM-DD HH:mm:ss') : '--',
					dispatchRltStandards:
						resData.dispatchRltStandards.length > 0
							? resData.dispatchRltStandards
									.map(item => (item.standard && item.standard.name ? item.standard.name : ''))
									.join('/')
							: '--',
					dispatchRltWorkers:
						resData.dispatchRltWorkers.length > 0
							? resData.dispatchRltWorkers.map(item => (item.worker && item.worker.name ? item.worker.name : '')).join('/')
							: '--',
					number: resData.number,
				};
				// 获取表格数据源
				//施工任务
				this.tableDataSource.planContents =
					resData.dispatchRltPlanContents.length > 0
						? resData.dispatchRltPlanContents.map(item => {
								return {
									name: item.planContent.name,
									content: item.planContent.content,
								};
						  })
						: [];
				// 待安装设备
				let equipments = [];
				resData.dispatchRltPlanContents && resData.dispatchRltPlanContents.length > 0
					? resData.dispatchRltPlanContents.map(item => {
							item.planContent && item.planContent.planMaterials && item.planContent.planMaterials.length > 0
								? item.planContent.planMaterials.map(item_ => {
										item_.planMaterialRltEquipments.map(_item_ => {
											equipments.push(_item_.equipment);
										});
								  })
								: [];
					  })
					: [];
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
				// 待安装材料
				this.tableDataSource.materials =
					resData.dispatchRltMaterials.length > 0
						? resData.dispatchRltMaterials.map(item => {
								return {
									name: item.material && item.material.name ? item.material.name : '--',
									spec: item.material && item.material.spec ? item.material.spec : '--',
									unit: item.material && item.material.unit ? item.material.unit : '--',
									count: item.count,
								};
						  })
						: [];

				// 获取材料附件
				this.dispatchRltFiles = resData.dispatchRltFiles.map(item => {
					return item.file;
				});
			}
			this.loading = false;
		},
		async submit(state) {
			if (this.content != '') {
				let formData = {
					dispatchId: this.id,
					content: this.content,
					state: state == ApprovalState.Pass ? ApprovalState.Pass : state == ApprovalState.UnPass ? ApprovalState.UnPass : '',
				};
				uni.showModal({
					title: '提示',
					content: '是否确认当前选择',
					success: async ress => {
						if (ress.confirm) {
							if (!this.$store.state.isSubmit) {
								this.$store.commit('SetIsSubmit', true);
								let response = await apiDispatch.process(formData);
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
#construction-constructionDispatch {
	font-size: 28rpx;
	color: #656565;
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
	.basic-row > .alignItems {
		display: flex;
		align-items: center;
	}
}
#construction-constructionDispatch > view > text {
	color: #40a7fc;
	padding: 15rpx 0;
	display: block;
}
::v-deep #construction-constructionDispatch .basic-row textarea {
	padding: 10rpx 20rpx;
	border-radius: 10rpx;
}
::v-deep #construction-constructionDispatch .uni-loading-more-page {
	background-color: #f2f2f6;
}
</style>
