<!-- 安全问题标记页面 -->
<template>
	<view id="pages-uni-security-mark">
		<!-- 加载中 -->
		<uniLoading v-show="loading" class="uniLoading" />
		<!-- 展示区 -->
		<uni-forms v-show="!loading" ref="form" :value="securityMark" :rules="rules">
			<uni-forms-item label="问题标题" required name="title">
				<textarea
					class="pages-textarea"
					auto-height
					maxlength="-1"
					placeholder="请输入问题标题"
					placeholder-style="color: #c3c1c1; z-index:0"
					v-model="securityMark.title"
					@input="binddata('title', $event.target.value)"
				/>
			</uni-forms-item>
			<uni-forms-item label="问题类型" required name="type">
				<uniPicker
					mode="selector"
					placeholder="请选择问题类型"
					range-key="name"
					groupCode="SafeManager.ProblemType"
					v-model="securityMark.type"
					@input="binddata('typeId', $event)"
				/>
			</uni-forms-item>
			<uni-forms-item label="风险等级" required name="riskLevel">
				<uniPicker
					mode="selector"
					:range="levelRange"
					placeholder="请选择风险等级"
					range-key="name"
					:value="getSafetyRiskLevel(securityMark.riskLevel)"
					@input="binddata('riskLevel', $event)"
				/>
			</uni-forms-item>
			<uni-forms-item label="所属专业" required name="profession">
				<uniPicker
					mode="selector"
					groupCode="Profession"
					placeholder="请选择所属专业"
					v-model="securityMark.profession"
					@input="binddata('profession', $event)"
				/>
			</uni-forms-item>
			<uni-forms-item label="检查时间" required name="checkTime">
				<uniPicker
					mode="date"
					v-model="securityMark.checkTime"
					placeholder="请选择检查时间"
					@input="binddata('checkTime', $event)"
				/>
			</uni-forms-item>
			<uni-forms-item label="限期时间" required name="limitTime">
				<uniPicker
					mode="date"
					v-model="securityMark.limitTime"
					placeholder="请选择限期时间"
					@input="binddata('limitTime', $event)"
				/>
			</uni-forms-item>
			<uni-forms-item label="检查单位" name="checkUnit">
				<treeSelect
					class="uni-form-member-select"
					placeholder="请选择检查单位"
					title="检查单位"
					:disabled="checkUnitDisabled"
					v-model="securityMark.checkUnit"
					:lazy="true"
					action="/api/app/appOrganization/getList"
					@input="
						$event => {
							onCheckUnitNameChange($event);
							binddata('checkUnit', $event);
						}
					"
				/>
			</uni-forms-item>
			<uni-forms-item label="检查单位名称" required name="checkUnitName">
				<textarea
					class="pages-textarea"
					auto-height
					maxlength="-1"
					placeholder="请输入检查单位名称"
					placeholder-style="color: #c3c1c1; z-index:0"
					:disabled="securityMark.checkUnit != null"
					v-model="securityMark.checkUnitName"
					@input="
						$event => {
							if ($event.target.value == '') {
								checkUnitDisabled = false;
							} else {
								checkUnitDisabled = true;
							}
							binddata('checkUnitName', $event);
						}
					"
				/>
			</uni-forms-item>
			<uni-forms-item label="检查人" required name="checker">
				<memberSelect
					v-model="securityMark.checker"
					placeholder="请选择检查人"
					class="uni-form-member-select"
					@input="binddata('checker', $event)"
				></memberSelect>
			</uni-forms-item>

			<uni-forms-item label="责任人" required name="responsibleUser">
				<memberSelect
					v-model="securityMark.responsibleUser"
					placeholder="请选择责任人"
					class="uni-form-member-select"
					@input="binddata('responsibleUser', $event)"
				></memberSelect>
			</uni-forms-item>
			<uni-forms-item label="抄送人" required name="ccUsers">
				<memberSelect
					v-model="securityMark.ccUsers"
					placeholder="请选择抄送人"
					:multiple="true"
					class="uni-form-member-select"
					@input="binddata('ccUsers', $event)"
				></memberSelect>
			</uni-forms-item>
			<uni-forms-item label="验证人" required name="verifier">
				<memberSelect
					v-model="securityMark.verifier"
					placeholder="请选择验证人"
					class="uni-form-member-select"
					@input="binddata('verifier', $event)"
				></memberSelect>
			</uni-forms-item>
			<uni-forms-item label="责任单位" required name="responsibleUnit">
				<textarea
					class="pages-textarea"
					auto-height
					maxlength="-1"
					placeholder="请输入责任单位"
					placeholder-style="color: #c3c1c1; z-index:0"
					v-model="securityMark.responsibleUnit"
					@input="binddata('responsibleUnit', $event.target.value)"
				/>
			</uni-forms-item>
			<uni-forms-item label="责任部门" name="responsibleOrganization">
				<treeSelect
					v-model="securityMark.responsibleOrganization"
					title="责任部门"
					:lazy="true"
					action="/api/app/appOrganization/getList"
					placeholder="请选择责任部门"
					class="uni-form-member-select"
					@input="binddata('responsibleOrganization', $event)"
				></treeSelect>
			</uni-forms-item>
			<!-- <uni-forms-item label="关联模型" name="equipments">
					<treeSelect
						v-model="securityMark.equipments"
						title="关联模型"
						:lazy="true"
						action="/api/app/appOrganization/getList"
						placeholder="请选择关联模型"
						class="uni-form-member-select"
						@input="binddata('equipments', $event)"
					></treeSelect>
				</uni-forms-item> -->
			<uni-forms-item label="问题描述" name="content">
				<listSelect
					:needLabel="false"
					url="/api/app/safeProblemLibrary/getList"
					placeholder="请填写问题描述"
					keySelect="content"
					keyEmit="measures"
					v-model="securityMark.content"
					@input="
						item => {
							securityMark.content = item;
						}
					"
					@keyEmit="
						item => {
							securityMark.suggestion = item;
						}
					"
				/>
			</uni-forms-item>
			<uni-forms-item label="整改意见" name="suggestion">
				<textarea
					class="pages-textarea"
					auto-height
					maxlength="-1"
					placeholder="请输入整改意见"
					placeholder-style="color: #c3c1c1; z-index:0"
					v-model="securityMark.suggestion"
					@input="binddata('suggestion', $event.target.value)"
				/>
			</uni-forms-item>
			<uni-forms-item label="图片上传" name="files">
				<pickerImage style="flex: 1" :url="getUploadUrl()" v-model="securityMark.files" @input="binddata('files', $event)" />
			</uni-forms-item>
			<view class="save-button">
				<button @tap="submit">{{ pageState == PageState.Edit ? '编辑' : '标记' }}</button>
			</view>
		</uni-forms>
	</view>
</template>

<script>
import {requestIsSuccess, getUploadUrl, getSafetyRiskLevel, showToast} from '@/utils/util.js';
import {PageState, SafetyRiskLevel, SafeQualityProblemState} from '@/utils/enum.js';
import uniPicker from '@/components/uniPicker.vue';
import dictionaryPicker from '@/components/dictionaryPicker.vue';
import memberSelect from '@/components/uniMemberSelect.vue';
import treeSelect from '@/components/treeSelect.vue';
import pickerImage from '@/components/pickerImage.vue';
import listSelect from '@/components/listSelect.vue';
import * as apiSafeProblem from '@/api/safe/safeProblem.js';
import * as apiSystem from '@/api/system.js';
import moment from 'moment';

export default {
	name: 'securityOfMark',
	components: {
		uniPicker,
		dictionaryPicker,
		memberSelect,
		treeSelect,
		pickerImage,
		listSelect,
	},
	data() {
		return {
			SafetyRiskLevel,
			levelRange: [
				{
					name: '特别重大事故',
					key: 1,
				},
				{
					name: '重大事故',
					key: 2,
				},
				{
					name: '较大事故',
					key: 3,
				},
				{
					name: '一般事故',
					key: 3,
				},
			],
			loading: true,
			checkUnitDisabled: false,
			PageState,
			getSafetyRiskLevel,
			id: '',
			pageState: PageState.Add,
			securityMark: {
				title: '',
				date: '',
				typeId: '',
				checker: {},
				checkTime: '',
				limitTime: '',
				checkUnit: null,
				checkUnitName: '',
				responsibleUser: {},
				verifier: {},
				responsibleUnit: '',
				responsibleOrganization: {},
				suggestion: '',
				content: '',
				// equipments: [],
				ccUsers: [],
				files: [],
				riskLevel: SafetyRiskLevel,
			},
			rules: {
				// 对name字段进行必填验证
				title: {
					rules: [
						{
							required: true,
							errorMessage: '请输入标题',
						},
					],
				},
				riskLevel: {
					rules: [
						{
							required: true,
							errorMessage: '请选择风险等级',
						},
					],
				},
				typeId: {
					rules: [
						{
							required: true,
							errorMessage: '请选择类型',
						},
					],
				},
				checkUnitName: {
					rules: [
						{
							required: true,
							errorMessage: '请输入检查单位',
						},
					],
				},
				checker: {
					rules: [
						{
							required: true,
							errorMessage: '请选择检测人',
						},
					],
				},
				responsibleUser: {
					rules: [
						{
							required: true,
							errorMessage: '请选择责任人',
						},
					],
				},
				responsibleUnit: {
					rules: [
						{
							required: true,
							errorMessage: '请输入责任单位',
						},
					],
				},
				verifier: {
					rules: [
						{
							required: true,
							errorMessage: '请选择验证人',
						},
					],
				},
				ccUsers: {
					rules: [
						{
							required: true,
							errorMessage: '请选择抄送人',
						},
					],
				},
				checkTime: {
					rules: [
						{
							required: true,
							errorMessage: '请选择检测时间',
						},
					],
				},
				limitTime: {
					rules: [
						{
							required: true,
							errorMessage: '请选择限期时间',
						},
					],
				},
			},
		};
	},

	async onLoad(option) {
		this.pageState = option.pageState;
		this.id = option.id;
		this.loading = true;
		this.refresh();
	},

	methods: {
		requestIsSuccess,
		submit() {
			this.$refs.form
				.submit()
				.then(async res => {
					let data = {
						...res,
						typeId: res.type.id,
						checkerId: res.checker.id,
						checkUnitId: res.checkUnit ? res.checkUnit.id : '',
						verifierId: res.verifier.id,
						professionId: res.profession.id,
						responsibleOrganizationId: res.responsibleOrganization.id,
						responsibleUserId: res.responsibleUser.id,
						state: SafeQualityProblemState.WaitingImprove,
						files:
							res.files.length > 0
								? res.files.map(item => {
										return {
											fileId: item.id,
										};
								  })
								: [],
						ccUsers:
							res.ccUsers.length > 0
								? res.ccUsers.map(item => {
										return {
											ccUserId: item.id,
										};
								  })
								: [],
						// equipments:
						// 	res.equipments.length > 0
						// 		? res.equipments.map(item => {
						// 				return {
						// 					equipmentId: item.id,
						// 				};
						// 		  })
						// 		: [],
					};
					if (!this.$store.state.isSubmit) {
						this.$store.commit('SetIsSubmit', true);
						if (this.pageState == PageState.Add) {
							let response = await apiSafeProblem.create(data);
							if (requestIsSuccess(response)) {
								showToast(`标记成功`);
							} else {
								showToast('标记失败', false);
							}
						} else if (this.pageState == PageState.Edit) {
							let response = await apiSafeProblem.update({
								id: this.id,
								...data,
							});
							if (requestIsSuccess(response)) {
								showToast(`编辑成功`);
							} else {
								showToast('编辑失败', false);
							}
						}
					}
				})
				.catch(err => {
					console.log('表单错误信息：', err);
				});
		},
		getUploadUrl,

		onCheckUnitNameChange(value) {
			console.log(value);
			if (value) {
				this.securityMark.checkUnitName = value.name;
			} else {
				this.securityMark.checkUnitName = '';
			}
		},

		binddata(name, value) {
			if (name == 'riskLevel') {
				this.securityMark.riskLevel = value.key;
			}
			this.$refs.form.setValue(name, value);
		},

		async refresh() {
			if (!this.id) return;
			const response = await apiSafeProblem.get(this.id);
			if (requestIsSuccess(response)) {
				let _securityMark = response.data;
				this.securityMark = {
					..._securityMark,
					checkTime: moment(_securityMark.checkTime).format('YYYY-MM-DD'),
					limitTime: moment(_securityMark.limitTime).format('YYYY-MM-DD'),
					files:
						_securityMark.files && _securityMark.files.length > 0
							? _securityMark.files.map(item => {
									return item.file;
							  })
							: [],
					checkUnit: _securityMark.checkUnit ? _securityMark.checkUnit : null,
					ccUsers:
						_securityMark.ccUsers && _securityMark.ccUsers.length > 0 ? _securityMark.ccUsers.map(item => item.ccUser) : [],
					// equipments:
					// 	_securityMark.equipments && _securityMark.equipments.length > 0
					// 		? _securityMark.equipments.map(item => item.equipment)
					// 		: [],
				};
			}
		},
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},
	mounted() {
		this.loading = false;
	},
};
</script>

<style lang="scss">
page {
	background-color: #f9f9f9;
}

#pages-uni-security-mark {
	padding: 20rpx;
	margin-bottom: 120rpx;
	background-color: #f9f9f9;
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
	.pages-uni-security-mark-item {
		padding: 0 20rpx;
		flex: 1;
		height: 70rpx;
		background-color: #ffffff;
		border-radius: 10rpx;
		font-size: 28rpx;
		display: flex;
		align-items: center;
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
		background-color: #1890ff;
	}
}
</style>
