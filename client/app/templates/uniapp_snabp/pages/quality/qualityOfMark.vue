<!-- 质量标记页面 -->
<template>
	<view id="pages-uni-quality-mark">
		<!-- 加载中 -->
		<uniLoading v-show="loading" class="uniLoading" />
		<!-- 展示区 -->
		<uni-forms v-show="!loading" ref="form" :value="formData" :rules="rules">
			<uni-forms-item label="问题标题" required name="title">
				<textarea
					class="pages-textarea"
					auto-height
					maxlength="-1"
					placeholder="请输入问题标题"
					placeholder-style="color: #c3c1c1; z-index:0"
					v-model="formData.title"
					@input="binddata('title', $event.target.value)"
				/>
			</uni-forms-item>
			<uni-forms-item label="问题等级" required name="level">
				<uniPicker
					mode="selector"
					:range="levelState"
					placeholder="请选择问题等级"
					range-key="name"
					:value="getQualityProblemLevel(formData.level)"
					@input="binddata('level', $event)"
				/>
			</uni-forms-item>
			<uni-forms-item label="问题类型" required name="type">
				<uniPicker
					mode="selector"
					:range="typeState"
					placeholder="请选择问题类型"
					range-key="name"
					:value="getQualityProblemType(formData.type)"
					@input="binddata('type', $event)"
				/>
			</uni-forms-item>
			<uni-forms-item label="所属专业" required name="profession">
				<uniPicker
					mode="selector"
					groupCode="Profession"
					placeholder="请选择所属专业"
					v-model="formData.profession"
					@input="binddata('profession', $event)"
				/>
			</uni-forms-item>
			<uni-forms-item label="检查时间" required name="checkTime">
				<uniPicker mode="date" :value="formData.checkTime" placeholder="请选择检查时间" @input="binddata('checkTime', $event)" />
			</uni-forms-item>
			<uni-forms-item label="限期时间" required name="limitTime">
				<uniPicker mode="date" :value="formData.limitTime" placeholder="请选择限期时间" @input="binddata('limitTime', $event)" />
			</uni-forms-item>
			<uni-forms-item label="检查单位" name="checkUnit">
				<treeSelect
					class="uni-form-member-select"
					placeholder="请选择检查单位"
					title="检查单位"
					:disabled="checkUnitDisabled"
					v-model="formData.checkUnit"
					:lazy="true"
					action="/api/app/appOrganization/getList"
					@input="
						$value => {
							onCheckUnitNameChange($value);
							binddata('checkUnit', $value);
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
					:disabled="formData.checkUnit != null"
					v-model="formData.checkUnitName"
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
			<uni-forms-item label="责任单位" required name="responsibleUnit">
				<textarea
					class="pages-textarea"
					auto-height
					maxlength="-1"
					placeholder="请输入责任单位"
					placeholder-style="color: #c3c1c1; z-index:0"
					v-model="formData.responsibleUnit"
					@input="binddata('responsibleUnit', $event.target.value)"
				/>
			</uni-forms-item>
			<uni-forms-item label="检查人" required name="checker">
				<memberSelect
					class="uni-form-member-select"
					placeholder="请选择检查人"
					v-model="formData.checker"
					@input="
						$event => {
							binddata('checker', $event);
						}
					"
				/>
			</uni-forms-item>
			<uni-forms-item label="责任人" required name="responsibleUser">
				<memberSelect
					class="uni-form-member-select"
					placeholder="请选择责任人"
					v-model="formData.responsibleUser"
					@input="
						$event => {
							binddata('responsibleUser', $event);
						}
					"
				/>
			</uni-forms-item>
			<uni-forms-item label="抄送人" required name="ccUsers">
				<memberSelect
					class="uni-form-member-select"
					placeholder="请选择抄送人"
					v-model="formData.ccUsers"
					:multiple="true"
					@input="
						$event => {
							binddata('ccUsers', $event);
						}
					"
				/>
			</uni-forms-item>
			<uni-forms-item label="验证人" required name="verifier">
				<memberSelect
					class="uni-form-member-select"
					placeholder="请选择验证人"
					v-model="formData.verifier"
					@input="
						$event => {
							binddata('verifier', $event);
						}
					"
				/>
			</uni-forms-item>
			<uni-forms-item label="责任部门" name="responsibleOrganization">
				<treeSelect
					class="uni-form-member-select"
					placeholder="请选择责任部门"
					title="责任部门"
					v-model="formData.responsibleOrganization"
					:lazy="true"
					action="/api/app/appOrganization/getList"
					@input="
						$event => {
							binddata('responsibleOrganization', $event);
						}
					"
				/>
			</uni-forms-item>
			<uni-forms-item label="问题描述" name="content">
				<listSelect
					url="/api/app/qualityProblemLibrary/getList"
					placeholder="请填写问题描述"
					keySelect="content"
					keyEmit="measures"
					v-model="formData.content"
					@input="
						item => {
							formData.content = item;
						}
					"
					@keyEmit="
						item => {
							formData.suggestion = item;
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
					v-model="formData.suggestion"
					@input="binddata('suggestion', $event.target.value)"
				/>
			</uni-forms-item>

			<uni-forms-item label="图片上传" name="files">
				<pickerImage :url="getUploadUrl()" v-model="formData.files" @input="binddata('files', $event)" />
			</uni-forms-item>
			<view class="save-button">
				<button @tap="submit">{{ pageState == PageState.Edit ? '编辑' : '标记' }}</button>
			</view>
		</uni-forms>
	</view>
</template>

<script>
import pickerImage from '@/components/pickerImage.vue';
import memberSelect from '@/components/uniMemberSelect.vue';
import treeSelect from '@/components/treeSelect.vue';
import {getUploadUrl, requestIsSuccess, showToast, getQualityProblemType, getQualityProblemLevel, throttle} from '@/utils/util.js';
import {SafeQualityRecordState, SafeQualityProblemState, PageState} from '@/utils/enum.js';
import uniPicker from '@/components/uniPicker.vue';
import * as apiQuality from '@/api/quality/quality.js';
import listSelect from '@/components/listSelect.vue';
import moment from 'moment';

export default {
	name: 'qualityMark',
	components: {
		uniPicker,
		listSelect,
		pickerImage,
		memberSelect,
		treeSelect,
	},
	data() {
		return {
			PageState: PageState,
			checkUnitDisabled: false,
			levelState: [
				{name: '重大质量事故', key: 1},
				{name: '一般质量事故', key: 2},
				{name: '质量问题', key: 3},
			],
			typeState: [
				{name: 'A', key: 1},
				{name: 'B', key: 2},
				{name: 'C', key: 3},
			],
			SafeQualityRecordState,
			pageState: PageState.Add, // 标记页状态
			editId: '',
			loading: true,
			formData: {
				title: '',
				level: '',
				type: '',
				profession: '',
				checkTime: '',
				limitTime: '',
				checkUnit: {},
				checkUnitName: '',
				responsibleUnit: '',
				checker: {},
				responsibleUser: {},
				ccUsers: [],
				verifier: {},
				responsibleOrganization: {},
				// equipments: [],
				content: '',
				suggestion: '',
				files: [],
			},
			rules: {
				title: {rules: [{required: true, errorMessage: '请填写问题标题'}]},
				level: {rules: [{required: true, errorMessage: '请选择问题等级'}]},
				type: {rules: [{required: true, errorMessage: '请选择问题类型'}]},
				profession: {rules: [{required: true, errorMessage: '请选择专业'}]},
				checkTime: {rules: [{required: true, errorMessage: '请选择检查时间'}]},
				limitTime: {rules: [{required: true, errorMessage: '请选择限期时间'}]},
				responsibleUnit: {rules: [{required: true, errorMessage: '请填写责任单位'}]},
				checker: {rules: [{required: true, errorMessage: '请选择检查人'}]},
				responsibleUser: {rules: [{required: true, errorMessage: '请选择责任人'}]},
				ccUsers: {rules: [{required: true, errorMessage: '请选择抄送人'}]},
				verifier: {rules: [{required: true, errorMessage: '请选择验证人'}]},
				checkUnitName: {rules: [{required: true, errorMessage: '请输入检查单位'}]},
			},
		};
	},

	onLoad(option) {
		this.pageState = option.pageState;
		this.editId = option.id;
		this.loading = true;
		this.refresh();
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},
	mounted() {
		this.loading = false;
	},
	methods: {
		getUploadUrl,
		getQualityProblemLevel,
		getQualityProblemType,
		throttle,

		onCheckUnitNameChange(value) {
			if (value) {
				this.formData.checkUnitName = value.name;
			} else {
				this.formData.checkUnitName = '';
			}
		},

		async refresh() {
			if (!this.editId) return;
			let resData = await apiQuality.get(this.editId);
			if (requestIsSuccess(resData)) {
				resData = resData.data;
				resData = {
					...resData,
					ccUsers: resData.ccUsers.map(e => {
						let ccUser = e.ccUser;
						return {...ccUser};
					}),
					files:
						resData.files && resData.files.length > 0
							? resData.files.map(item => {
									return item.file;
							  })
							: [],
					checkUnits: resData.checkUnits && resData.checkUnits.length > 0 ? resData.checkUnits[0].checkUnit : {},
					checkTime: moment(resData.checkTime).format('YYYY-MM-DD'),
					limitTime: moment(resData.limitTime).format('YYYY-MM-DD'),
				};
				this.formData = resData;
				console.log(this.formData);
			}
		},
		submit() {
			this.$refs.form
				.submit()
				.then(async res => {
					res = {
						...res,
						professionId: res.profession.id,
						checkUnitId: res.checkUnit ? res.checkUnit.id : '',
						state: SafeQualityProblemState.WaitingImprove,
						checkerId: res.checker.id,
						responsibleUserId: res.responsibleUser.id,
						verifierId: res.verifier.id,
						responsibleOrganizationId: res.responsibleOrganization.id,
						ccUsers: res.ccUsers
							? res.ccUsers.map(item => {
									return {ccUserId: item.id};
							  })
							: [],
						equipments: res.equipments
							? res.equipments.map(item => {
									return {equipmentId: item.id};
							  })
							: [],
						files: res.files
							? res.files.map(item => {
									return {fileId: item.id};
							  })
							: [],
						content: this.formData.content,
						suggestion: this.formData.suggestion,
					};
					if (!this.$store.state.isSubmit) {
						this.$store.commit('SetIsSubmit', true);
						if (this.pageState == PageState.Add) {
							let response = await apiQuality.create(res);
							if (requestIsSuccess(response)) {
								showToast(`标记成功`);
							} else {
								showToast('标记失败', false);
							}
						} else if (this.pageState == PageState.Edit) {
							res.id = this.formData.id;
							res.code = this.formData.code;
							let response = await apiQuality.update(res);
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
		binddata(name, value) {
			if (name == 'type') {
				this.formData.type = value.key;
			} else if (name == 'level') {
				this.formData.level = value.key;
			}
			this.$refs.form.setValue(name, value);
		},
	},
};
</script>

<style lang="scss">
page {
	background-color: #f9f9f9;
}

#pages-uni-quality-mark {
	background-color: #f9f9f9;
	padding: 20rpx;
	margin-bottom: 120rpx;
	.uni-form-member-select {
		flex: 1;
	}
	.pages-uni-quality-mark-item {
		padding: 0 20rpx;
		flex: 1;
		height: 70rpx;
		background-color: #ffffff;
		border-radius: 10rpx;
		font-size: 28rpx;
		overflow: hidden;
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
