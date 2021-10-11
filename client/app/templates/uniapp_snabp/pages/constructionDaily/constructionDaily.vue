<!-- 安全问题标记页面 -->
<template>
	<view class="uni-construction-daily">
		<!-- 加载中 -->
		<uniLoading v-show="loading" class="uniLoading" />
		<view v-show="!loading">
			<uni-forms ref="form" :value="daily" :rules="rules">
				<uni-forms-item label="日志编号" required name="code">
					<textarea
						class="pages-textarea"
						disabled
						auto-height
						maxlength="-1"
						v-model="daily.code"
						placeholder="自动生成"
						placeholder-style="color: #c3c1c1; z-index:0"
					/>
				</uni-forms-item>
				<uni-forms-item label="填报时间" required name="date">
					<uniPicker
						:disabled="pageState == PageState.View"
						mode="date"
						v-model="daily.date"
						placeholder="请选择填报时间"
						@input="
							$event => {
								currentDate = $event;
							}
						"
					/>
				</uni-forms-item>
				<uni-forms-item label="填报人" required name="informant">
					<memberSelect
						v-model="daily.informant"
						:disabled="pageState == PageState.View"
						placeholder="请选择填报人"
						class="uni-form-member-select"
						@input="binddata('informant', $event)"
					/>
				</uni-forms-item>

				<uni-forms-item label="天气" required name="weathers">
					<textarea
						class="pages-textarea"
						:disabled="pageState == PageState.View"
						auto-height
						maxlength="-1"
						v-model="daily.weathers"
						placeholder="请输入天气情况"
						placeholder-style="color: #c3c1c1; z-index:0"
						@input="binddata('weathers', $event.target.value)"
					/>
				</uni-forms-item>

				<uni-forms-item label="温度" required name="temperature">
					<textarea
						class="pages-textarea"
						:disabled="pageState == PageState.View"
						auto-height
						maxlength="-1"
						v-model="daily.temperature"
						placeholder="请输入温度"
						placeholder-style="color: #c3c1c1; z-index:0"
						@input="binddata('temperature', $event.target.value)"
					/>
				</uni-forms-item>

				<uni-forms-item label="风力风向" required name="windDirection">
					<textarea
						class="pages-textarea"
						:disabled="pageState == PageState.View"
						auto-height
						maxlength="-1"
						v-model="daily.windDirection"
						placeholder="请输入风力风向"
						placeholder-style="color: #c3c1c1; z-index:0"
						@input="binddata('windDirection', $event.target.value)"
					/>
				</uni-forms-item>

				<uni-forms-item label="空气质量" required name="airQuality">
					<textarea
						class="pages-textarea"
						:disabled="pageState == PageState.View"
						auto-height
						maxlength="-1"
						v-model="daily.airQuality"
						placeholder="请输入空气质量状况"
						placeholder-style="color: #c3c1c1; z-index:0"
						@input="binddata('airQuality', $event.target.value)"
					/>
				</uni-forms-item>
				<uni-forms-item label="派工单" name="dispatch">
					<uniPicker
						mode="selector"
						placeholder="请选择派工单"
						:disabled="pageState == PageState.View"
						:range="dispatchList"
						range-key="name"
						v-model="daily.dispatch"
						@input="
							$event => {
								getdailyPlanRltEquipment($event);
								daily.team = $event ? $event.team : '';
								binddata('dispatch', $event);
							}
						"
					/>
				</uni-forms-item>

				<uni-forms-item label="施工班组" name="team">
					<textarea
						class="pages-textarea"
						:disabled="true"
						auto-height
						v-model="daily.team"
						placeholder="--"
						maxlength="-1"
						placeholder-style="color: #c3c1c1; z-index:0"
						@input="binddata('team', $event.target.value)"
					/>
				</uni-forms-item>

				<uni-forms-item label="施工人员" required name="builderCount">
					<textarea
						class="pages-textarea"
						:disabled="pageState == PageState.View"
						auto-height
						v-model="daily.builderCount"
						maxlength="-1"
						placeholder="请输入施工人员"
						placeholder-style="color: #c3c1c1; z-index:0"
						@input="binddata('builderCount', $event.target.value)"
					/>
				</uni-forms-item>

				<uni-forms-item label="施工部位" name="location">
					<textarea
						class="pages-textarea"
						:disabled="pageState == PageState.View"
						auto-height
						v-model="daily.location"
						maxlength="-1"
						placeholder="请输入施工部位"
						placeholder-style="color: #c3c1c1; z-index:0"
						@input="binddata('location', $event.target.value)"
					/>
				</uni-forms-item>

				<uni-forms-item label="施工总结" name="summary">
					<textarea
						class="pages-textarea"
						:disabled="pageState == PageState.View"
						auto-height
						v-model="daily.summary"
						maxlength="-1"
						placeholder="请输入施工总结"
						placeholder-style="color: #c3c1c1; z-index:0"
						@input="binddata('summary', $event.target.value)"
					/>
				</uni-forms-item>
				<view class="daily-table">
					<view class="tab-title">施工任务</view>
					<wyb-table
						:disabled="pageState == PageState.View"
						:headers="planColumns"
						:contents="dailyRltPlan"
						@onCellClick="onCellClick1"
					/>
				</view>
				<view class="daily-table">
					<view class="tab-title">设备信息</view>
					<wyb-table
						:headers="equipmentColumns"
						:contents="dailyRltEquipments"
						:wyb-qRcode-confirm="equipmentsIndex"
						@onCellClick="onCellClick2"
					/>
				</view>
				<view class="daily-table">
					<view class="tab-title">
						<text>临时任务</text>
						<view v-if="pageState != PageState.View" class="iconfont icon-add" @click="onShowPopup" />
					</view>
					<wyb-table :headers="taskColumns" :contents="unplannedTask" @onCellClick="onCellClick3" />
				</view>
				<view class="daily-table">
					<view class="tab-title">
						<text>存在的安全问题</text>
						<view class="iconfont">
							<uniPicker
								v-if="pageState != PageState.View"
								mode="selector"
								:isShowValueBox="false"
								:range="safeProblems"
								range-key="title"
								@input="addSafeProblem"
							/>
						</view>
					</view>
					<wyb-table :headers="problemColumns" :contents="dailyRltSafe" @onCellClick="onCellClick4" />
				</view>
				<view class="daily-table">
					<view class="tab-title">
						<text>存在的质量问题</text>
						<view class="iconfont">
							<uniPicker
								v-if="pageState != PageState.View"
								mode="selector"
								:isShowValueBox="false"
								:range="qualityProblems"
								range-key="title"
								@input="addQuatityProblem"
							/>
						</view>
					</view>
					<wyb-table :headers="problemColumns" :contents="dailyRltQuality" @onCellClick="onCellClick5" />
				</view>

				<uni-forms-item label="其他内容" name="remark">
					<textarea
						class="pages-textarea"
						:disabled="pageState == PageState.View"
						auto-height
						v-model="daily.remark"
						maxlength="-1"
						@input="binddata('remark', $event.detail.value)"
						placeholder="请输入其他内容"
						placeholder-style="color: #c3c1c1; display:flex; align-items: center "
					/>
				</uni-forms-item>
				<uni-forms-item label="图片上传" required="" name="files">
					<pickerImage
						:disabled="pageState == PageState.View"
						style="flex: 1"
						:url="getUploadUrl()"
						v-model="daily.files"
						@input="binddata('files', $event)"
					/>
				</uni-forms-item>
			</uni-forms>
			<view v-if="pageState != PageState.View" class="save-button-box"><button class="save-button" @tap="submit">保存</button></view>
			<constructionDailyPopup ref="ConstructionDailyPopup" @change="onTaskChange" />
		</view>
	</view>
</template>

<script>
import {
	requestIsSuccess,
	getUploadUrl,
	showToast,
	getProblemStateTitle,
	getQualityProblemType,
	getUnplannedTaskType,
	getCode,
} from '@/utils/util.js';
import {NodeType, PageState} from '@/utils/enum.js';
import uniPicker from '@/components/uniPicker.vue';
import memberSelect from '@/components/uniMemberSelect.vue';
import pickerImage from '@/components/pickerImage.vue';
import constructionDailyPopup from './constructionDailyPopup.vue';
import * as apiDispatch from '@/api/construction/dispatch.js';
import * as apiQuality from '@/api/quality/quality.js';
import * as apiSafe from '@/api/safe/safeProblem.js';
import * as apiDaily from '@/api/construction/daily.js';

import moment from 'moment';

export default {
	name: 'securityOfMark',
	components: {
		uniPicker,
		memberSelect,
		pickerImage,
		constructionDailyPopup,
	},
	data() {
		return {
			urlCol: [
				{
					type: 'route',
					key: 'url',
				},
				{
					type: 'http',
					key: 'link',
				},
			],
			textInput: true, //清空时暂停操作
			loading: true, //页面加载动画
			dispatchList: [], //派工单数据
			currentDate: '', //当前填报日期
			dailyRltPlan: [], //施工任务
			dailyRltEquipments: [], //设备信息
			equipments: [], //设备信息安装确认
			equipmentsIndex: [],
			unplannedTask: [], //临时任务
			dailyRltSafe: [], //安全问题
			dailyRltQuality: [], //质量问题
			safeProblems: [], //安全问题数据源
			qualityProblems: [], //质量问题数据源
			PageState,
			NodeType,
			id: '',
			pageState: PageState.Add,
			daily: {
				code: '',
				date: '',
				dispatch: {},
				informant: {},
				weathers: '',
				temperature: '',
				airQuality: '',
				windDirection: '',
				location: '',
				summary: '',
				team: '',
				builderCount: '',
				files: [],
				remark: '',
			},
			rules: {
				// 进行必填验证
				date: {
					rules: [
						{
							required: true,
							errorMessage: '请选择填报日期',
						},
					],
				},
				informant: {
					rules: [
						{
							required: true,
							errorMessage: '请选择填报人',
						},
					],
				},
				weathers: {
					rules: [
						{
							required: true,
							errorMessage: '请输入天气',
						},
					],
				},
				temperature: {
					rules: [
						{
							required: true,
							errorMessage: '请输入温度',
						},
					],
				},
				windDirection: {
					rules: [
						{
							required: true,
							errorMessage: '请输入风力风向',
						},
					],
				},
				airQuality: {
					rules: [
						{
							required: true,
							errorMessage: '请输入空气质量',
						},
					],
				},
				builderCount: {
					rules: [
						{
							required: true,
							errorMessage: '请输入施工人员',
						},
					],
				},
				files: {
					rules: [
						{
							required: true,
							errorMessage: '请上传现场施工照片',
						},
					],
				},
			},
		};
	},

	computed: {
		planColumns() {
			let planColumns = [
				{
					label: '任务/设备',
					key: 'nameCategory',
				},
				{
					label: '未完成/总量',
					key: 'quantityAndUnFinished',
				},
			];
			this.pageState == PageState.View
				? planColumns
				: planColumns.push({
						label: '当天完成',
						key: 'wybNumberBox',
						width: 222,
				  });
			planColumns.push({label: '当天完成量', key: 'currentCount'});
			planColumns.push({label: '累计完成量', key: 'sumCountPercent'});
			return planColumns;
		},
		equipmentColumns() {
			return [
				{
					label: '设备名称',
					key: 'name',
				},
				{
					label: '安装确认',
					key: 'wyb-QRcode',
				},
				{
					label: '设备分类',
					key: 'componentCategoryName',
				},
				{
					label: '规格型号',
					key: 'spec',
				},
				{
					label: '计量单位',
					key: 'unit',
				},
			];
		},
		taskColumns() {
			let taskColumns = [
				{
					label: '任务类型',
					key: 'taskTypeName',
				},
				{
					label: '任务说明',
					key: 'content',
				},
			];
			this.pageState == PageState.View
				? taskColumns
				: taskColumns.push({
						label: '操作',
						key: 'wyb-delete',
				  });
			return taskColumns;
		},
		problemColumns() {
			let problemColumns = [
				{
					label: '问题类型',
					key: 'typeName',
				},
				{
					label: '问题名称',
					key: 'title',
				},
				{
					label: '问题发起人',
					key: 'checkerName',
				},
				{
					label: '当前状态',
					key: 'stateName',
				},
			];
			this.pageState == PageState.View
				? problemColumns
				: problemColumns.push({
						label: '操作',
						key: 'wyb-delete',
				  });
			return problemColumns;
		},
	},

	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},

	async onLoad(option) {
		this.pageState = option.pageState;
		this.id = option.id;
		uni.setNavigationBarTitle({
			title: this.pageState != PageState.View ? '施工日志填报' : '施工日志详情',
		});
		this.refresh();
		this.getData();
	},
	watch: {
		daily: {
			handler(Obj) {
				let builderCount = this.$conformToRegular(Obj.builderCount, 5);
				if (!builderCount && this.textInput) {
					this.textInput = false;
					setTimeout(() => {
						this.daily.builderCount = '';
						this.textInput = true;
					}, 50);
				}
			},
			deep: true,
		},
	},

	methods: {
		requestIsSuccess,
		getUnplannedTaskType,
		//获取数据
		async getData() {
			let response = await apiDispatch.getList({isAll: true, passed: true, isForDaily: true});
			if (requestIsSuccess(response)) {
				this.dispatchList = response.data.items;
			}
			let safeResponse = await apiSafe.getWaitingImproveList({isAll: true});
			if (requestIsSuccess(safeResponse)) {
				this.safeProblems = safeResponse.data.items;
			}
			let qualityResponse = await apiQuality.getWaitingImproveList({isAll: true});
			if (requestIsSuccess(qualityResponse)) {
				this.qualityProblems = qualityResponse.data.items;
			}
		},

		//当天完成量数据更新
		recordChange(index, value) {
			let target = this.dailyRltPlan[index];
			if (target && value) {
				if (parseFloat(value) > target.unFinished) {
					uni.showToast({
						icon: 'none',
						title: '当天完成量不能大于未完成量',
						duration: 2000,
					});
					target.count = target.unFinished;
				} else {
					target.currentCount = `${((parseFloat(value) / target.quantity) * 100).toFixed(1)}%`;
					let count = parseFloat(value) + parseFloat(target.sumCount);
					target.sumCountPercent = `${((count / target.quantity) * 100).toFixed(1)}%`;
					target.count = parseFloat(value);
				}
			}
		},

		//设备安装跟踪确认
		async onScan(data, record) {
			let componentRltQRCodeId = '';
			if (data.value) {
				let array = data.value.split('@');
				if (array.length == 2 && array[1]) {
					componentRltQRCodeId = array[1];
				}
			}
			let result = {
				componentRltQRCodeId,
				nodeType: NodeType.Install,
				content: this.daily.dispatch ? this.daily.dispatch.name : '',
				time: this.daily.date,
				userId: this.daily.informant ? this.daily.informant.id : '',
				userName: this.daily.informant ? this.daily.informant.name : '',
				installationEquipmentId: record.id,
			};
			equipments.push(result);
			let response = await apiDaily.createTrackRecord(result);
			if (requestIsSuccess(response)) {
				uni.showToast({
					icon: 'none',
					title: '设备跟踪信息已录入',
					duration: 2000,
				});
			}
		},

		//获取施工任务以及设备信息
		async getdailyPlanRltEquipment(dispatch) {
			let response = await apiDispatch.get(dispatch.id);
			if (requestIsSuccess(response)) {
				this.dailyRltPlan = [];
				this.dailyRltEquipments = [];
				let data = response.data;
				if (data && data.dispatchRltPlanContents && data.dispatchRltPlanContents.length > 0) {
					let planContents = data.dispatchRltPlanContents;
					planContents.map(item => {
						if (item.planContent) {
							let data = {};
							if (
								item.planContent.planMaterials &&
								item.planContent.planMaterials &&
								item.planContent.planMaterials.length > 0
							) {
								item.planContent.planMaterials.map(async _item => {
									let _sumCount = 0;
									let response = await apiDaily.getDailyRltPlanMaterial(_item.id);
									if (requestIsSuccess(response)) {
										_sumCount = response.data;
									}
									let _nameCategory = `${item.planContent.name}/${
										_item.componentCategoryName ? _item.componentCategoryName : '无'
									}`;
									let unFinished = _item.quantity - _sumCount;
									let _quantityUnit = `${unFinished} / ${_item.quantity}`;
									data = {
										id: _item.id,
										planMaterialId: _item.id,
										name: item.planContent.name,
										nameCategory: _nameCategory,
										quantity: _item.quantity,
										unFinished: unFinished,
										quantityAndUnFinished: _quantityUnit,
										count: 0,
										wybNumberBox: 0,
										currentCount: '0.0%',
										sumCount: _sumCount,
										sumCountPercent: `${((_sumCount / _item.quantity) * 100).toFixed(1)}%` || '0.0%',
										disabled: false,
									};
									this.dailyRltPlan.push(data);
									if (_item.planMaterialRltEquipments && _item.planMaterialRltEquipments.length > 0) {
										_item.planMaterialRltEquipments.map((x, index) => {
											if (x.equipment) {
												this.dailyRltEquipments.push({
													...x.equipment,
													equipmentId: x.id,
													componentCategoryName:
														x.equipment.componentCategory && x.equipment.componentCategory.name
															? x.equipment.componentCategory.name
															: '--',
													unit: x.equipment.componentCategory ? x.equipment.componentCategory.unit : '--',
													spec: x.equipment.productCategory ? x.equipment.productCategory.name : '--',
												});
												//设备状态为5则是已安装
												if (x.equipment.state && x.equipment.state == 5) {
													this.equipmentsIndex.push(index);
												}
											}
										});
									}
								});
							}
						}
					});
				}
			}
		},

		//添加安全问题
		addSafeProblem(value) {
			if (!this.dailyRltSafe.find(x => x.id === value.id)) {
				this.dailyRltSafe.push({
					...value,
					typeName: value && value.type ? value.type.name : '',
					checkerName: value && value.checker ? value.checker.name : '',
					stateName: getProblemStateTitle(value.state),
				});
			}
		},

		//添加质量问题
		addQuatityProblem(value) {
			if (!this.dailyRltQuality.find(x => x.id === value.id)) {
				this.dailyRltQuality.push({
					...value,
					typeName: getQualityProblemType(value.type),
					checkerName: value && value.checker ? value.checker.name : '',
					stateName: getProblemStateTitle(value.state),
				});
			}
		},

		//数据提交
		submit() {
			this.$refs.form
				.submit()
				.then(async res => {
					let data = {
						...res,
						informantId: res.informant ? res.informant.id : '',
						dailyRltFiles:
							res.files.length > 0
								? res.files.map(item => {
										return {
											fileId: item.id,
										};
								  })
								: [],
						unplannedTask: this.unplannedTask,
						dailyRltSafe:
							this.dailyRltSafe.length > 0
								? this.dailyRltSafe.map(item => {
										return {
											safeProblemId: item.id,
										};
								  })
								: [],
						dailyRltQuality:
							this.dailyRltQuality.length > 0
								? this.dailyRltQuality.map(item => {
										return {
											qualityProblemId: item.id,
										};
								  })
								: [],
						dispatchId: res.dispatch ? res.dispatch.id : '',
						dailyRltPlan: this.dailyRltPlan,
					};
					let isSubmit = 1; //用于表格中完成量判断
					if (data.dailyRltPlan && data.dailyRltPlan.length > 0) {
						for (const item of data.dailyRltPlan) {
							if (item.wybNumberBox != 0 && item.wybNumberBox > item.unFinished) {
								uni.showToast({
									icon: 'none',
									title: '当天完成-不能大于未完成量',
									duration: 2000,
								});
								isSubmit += 1;
								return;
							} else if (item.wybNumberBox != 0 && item.wybNumberBox < 0) {
								uni.showToast({
									icon: 'none',
									title: '当天完成-不能小于0',
									duration: 2000,
								});
								isSubmit += 1;
								return;
							}
						}
						data.dailyRltPlan.map(item => {
							item.count = Number(item.wybNumberBox) + item.count;
						});
					}
					if (isSubmit == 1) {
						if (!this.$store.state.isSubmit) {
							this.$store.commit('SetIsSubmit', true);
							if (this.equipments.length > 0) {
								this.equipments.forEach(async item => {
									let response_ = await apiDaily.createTrackRecord(item);
									if (requestIsSuccess(response_)) {
										console.log('设备确认成功');
									}
								});
							}
							let response;
							if (this.pageState == PageState.Add) {
								response = await apiDaily.create(data);
							} else if (this.pageState == PageState.Edit) {
								response = await apiDaily.update({
									id: this.id,
									...data,
								});
							}
							if (requestIsSuccess(response)) {
								showToast('操作成功');
							} else {
								showToast('操作失败', false);
							}
						}
					}
				})
				.catch(err => {
					console.log('表单错误信息：', err);
				});
		},
		getUploadUrl,

		cancel() {
			this.currentDate = ''; //当前填报日期
			this.dailyRltPlan = []; //施工任务
			this.dailyRltEquipments = []; //设备信息
			this.unplannedTask = []; //临时任务
			this.dailyRltSafe = []; //安全问题
			this.dailyRltQuality = []; //质量问题
			this.id = '';
			this.pageState = PageState.Add;
			daily = {
				code: '',
				date: '',
				dispatch: {},
				informant: {},
				weathers: '',
				temperature: '',
				airQuality: '',
				windDirection: '',
				location: '',
				summary: '',
				team: '',
				builderCount: '',
				files: [],
				remark: '',
			};
			uni.navigateBack();
		},
		async refresh() {
			if (this.id) {
				const response = await apiDaily.get(this.id);
				if (requestIsSuccess(response)) {
					let _daily = response.data;
					let dailyRltPlan = _daily.dailyRltPlan;
					if (dailyRltPlan && dailyRltPlan.length > 0) {
						dailyRltPlan.map(async item => {
							let _sumCount = 0;
							let countResponse = await apiDaily.getDailyRltPlanMaterial(item.planMaterialId);
							if (requestIsSuccess(countResponse)) {
								_sumCount = countResponse.data;
							}
							let _nameCategory = `${
								item.planMaterial && item.planMaterial.planContent
									? item.planMaterial && item.planMaterial.planContent.name + '/'
									: ''
							}
												 ${item.planMaterial && item.planMaterial.ComponentCategoryName ? item.planMaterial.ComponentCategoryName : '无'}`;
							let unFinished = item.planMaterial.quantity - item.count;
							let _quantityAndUnFinished = `${unFinished}/${item.planMaterial ? item.planMaterial.quantity : ''}`;
							this.dailyRltPlan.push({
								...item,
								wybNumberBox: 0,
								quantity: item.planMaterial ? item.planMaterial.quantity : 0,
								nameCategory: _nameCategory,
								quantityAndUnFinished: _quantityAndUnFinished,
								sumCount: _sumCount,
								unFinished: unFinished,
								currentCount: '0.0%',
								sumCountPercent:
									item.count && item.planMaterial && item.planMaterial.quantity
										? `${(((_sumCount + item.count) / item.planMaterial.quantity) * 100).toFixed(1)}%`
										: '0.0%',
							});
							if (
								item.planMaterial &&
								item.planMaterial.planMaterialRltEquipments &&
								item.planMaterial.planMaterialRltEquipments.length > 0
							) {
								item.planMaterial.planMaterialRltEquipments.map((_item, index) => {
									if (_item.equipment) {
										this.dailyRltEquipments.push({
											..._item.equipment,
											componentCategoryName: _item.equipment.componentCategory
												? _item.equipment.componentCategory.name
												: '--',
											spec: _item.equipment.productCategory ? _item.equipment.productCategory.name : '--',
											unit: _item.equipment.componentCategory ? _item.equipment.componentCategory.unit : '--',
										});
										//设备状态为5则是已安装
										if (_item.equipment.state && _item.equipment.state == 5) {
											this.equipmentsIndex.push(index);
										}
									}
								});
							}
						});
					}
					this.daily = {
						..._daily,
						date: _daily.date ? moment(_daily.date).format('YYYY-MM-DD') : '',
						files:
							_daily && _daily.dailyRltFiles && _daily.dailyRltFiles.length > 0
								? _daily.dailyRltFiles.map(item => item.file)
								: [],
					};
					this.dailyRltQuality =
						_daily && _daily.dailyRltQuality && _daily.dailyRltQuality.length > 0
							? _daily.dailyRltQuality.map(item => {
									return {
										...item.qualityProblem,
										typeName: getQualityProblemType(item.qualityProblem.type),
										checkerName: item.qualityProblem.checker ? item.qualityProblem.checker.name : '',
										stateName: getProblemStateTitle(item.qualityProblem.state),
									};
							  })
							: [];
					this.dailyRltSafe =
						_daily && _daily.dailyRltSafe && _daily.dailyRltSafe.length > 0
							? _daily.dailyRltSafe.map(item => {
									return {
										...item.safeProblem,
										typeName: item.safeProblem.type ? item.safeProblem.type.name : '',
										checkerName: item.safeProblem.checker ? item.safeProblem.checker.name : '',
										stateName: getProblemStateTitle(item.safeProblem.state),
									};
							  })
							: [];
					this.unplannedTask =
						_daily.unplannedTask && _daily.unplannedTask.length > 0
							? _daily.unplannedTask.map(item => {
									return {
										...item,
										taskTypeName: getUnplannedTaskType(item.taskType),
									};
							  })
							: [];
				}
			} else {
				this.daily.code = getCode('RZ');
			}
			this.loading = false;
		},

		//弹出模态框
		onShowPopup() {
			this.$refs.ConstructionDailyPopup.open();
		},

		//添加临时任务
		onTaskChange(data) {
			this.unplannedTask.push(data);
		},
		// 扫码确认设备信息
		getScanning(equipmentId, contentIndex, Code, name) {
			let _this = this;
			uni.scanCode({
				success: async res => {
					//判断二维码类型
					let result = JSON.parse(res.result);
					let componentRltQRCodeId = '',
						componentCategoryQRCode = '';
					if (result && result.key == 'equipment' && result.value) {
						let qrCode = result.value;
						let array = qrCode.split('@');
						if (array.length == 2 && array[1]) {
							componentCategoryQRCode = array[0];
							componentRltQRCodeId = array[1];
							if (componentCategoryQRCode == Code) {
								if (!_this.equipments.find(item => item.componentRltQRCodeId === componentRltQRCodeId)) {
									let result = {
										componentRltQRCodeId,
										name: name,
										nodeType: NodeType.Install,
										content: this.daily.dispatch ? this.daily.dispatch.name : '',
										time: this.daily.date,
										userId: this.daily.informant ? this.daily.informant.id : '',
										userName: this.daily.informant ? this.daily.informant.name : '',
										installationEquipmentId: equipmentId,
									};
									_this.equipments.push(result);
									_this.equipmentsIndex.push(contentIndex);
								} else if (_this.equipments.find(item => item.componentRltQRCodeId === componentRltQRCodeId)) {
									let text = '';
									_this.equipments.map(item => {
										item.componentRltQRCodeId === componentRltQRCodeId ? (text = item.name) : '';
									});
									showToast(`二维码已被设备${text}安装确认,等待提交`, false);
								}
							} else {
								showToast(`二维码分类不正确`, false);
							}
						}
					} else {
						showToast('扫描的二维码类型不正确', false);
					}
				},
				fail: err => {
					showToast('信息获取失败', false);
				},
			});
		},
		//表格操作1
		onCellClick1(e) {
			if (!e.checkType && e.checkType != 'multiple' && e.key == 'wybNumberBox') {
				let data = e.lineData;
				data.currentCount = `${((data.wybNumberBox / data.quantity) * 100).toFixed(1)}%`;
				this.dailyRltPlan[e.contentIndex] = data;
			}
		},
		//表格操作2
		onCellClick2(e) {
			if (!e.checkType && e.checkType != 'multiple' && e.key == 'wyb-QRcode') {
				//设备状态为5则是已安装
				if (e.lineData && this.pageState != PageState.View) {
					if (!(e.lineData.state && e.lineData.state == 5)) {
						let Code =
							e.lineData.componentCategory && e.lineData.componentCategory.code ? e.lineData.componentCategory.code : '';
						this.getScanning(e.lineData.id, e.contentIndex, Code, e.lineData.name);
					}
				}
			}
		},
		//表格操作3
		onCellClick3(e) {
			if (!e.checkType && e.checkType != 'multiple' && e.key == 'wyb-delete') {
				this.unplannedTask.splice(e.contentIndex, 1);
			}
		},
		//表格操作4
		onCellClick4(e) {
			if (!e.checkType && e.checkType != 'multiple' && e.key == 'wyb-delete') {
				this.dailyRltSafe.splice(e.contentIndex, 1);
			}
		},
		//表格操作5
		onCellClick5(e) {
			if (!e.checkType && e.checkType != 'multiple' && e.key == 'wyb-delete') {
				this.dailyRltQuality.splice(e.contentIndex, 1);
			}
		},
	},
};
</script>

<style lang="scss">
page {
	background-color: #f9f9f9;
}

.uni-construction-daily {
	padding: 20rpx;
	margin-bottom: 120rpx;
	background-color: #f9f9f9;
}

.uni-form-item {
	display: flex;
	height: 80rpx;
	align-items: center;
	justify-content: space-between;
	padding: 0 10rpx;
}

.uni-form-member-select {
	flex: 1;
}

.uni-construction-daily .save-button {
	background-color: #1890ff;
	margin: 10px;
	color: #ffffff;
	border-radius: 100rpx;
}

.save-button-box {
	width: 100%;
	position: fixed;
	z-index: 90;
	bottom: 0;
	left: 0;
	background-color: #f9f9f9;
}

.icon-required {
	font-size: 18px;
	color: red;
	position: absolute;
	left: 10rpx;
}

.uni-construction-daily .uni-construction-daily-item {
	padding: 0 20rpx;
	flex: 1;
	height: 70rpx;
	background-color: #ffffff;
	border-radius: 10rpx;
	font-size: 28rpx;
	display: flex;
	align-items: center;
	z-index: 0;
}

.daily-table {
	margin-bottom: 20rpx;
}

.tab-title {
	height: 60rpx;
	display: flex;
	align-items: center;
	justify-content: space-between;
	color: #1890ff;
	> text {
		white-space: nowrap;
	}
}

.icon-add {
	font-size: 40rpx;
}
</style>
