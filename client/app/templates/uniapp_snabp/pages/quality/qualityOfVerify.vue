<!-- 质量问题验证页面 -->
<template>
	<view id="pages-uni-quality-verify">
		<uni-forms ref="form" :value="formData" :rules="rules">
			<uni-forms-item label="问题标题">
				<textarea
					class="pages-textarea"
					disabled
					auto-height
					v-model="formData.title"
					maxlength="-1"
					placeholder="请输入问题标题"
					placeholder-style="color: #c3c1c1; z-index:0"
				/>
			</uni-forms-item>
			<uni-forms-item label="验证时间" required name="time">
				<uniPicker mode="date" :placeholder="'请选择验证时间'" v-model="formData.time" @input="binddata('time', $event)" />
			</uni-forms-item>
			<uni-forms-item label="验证人" required name="userId">
				<memberSelect
					:placeholder="'请选择验证人'"
					v-model="formData.userId"
					class="sm-input-class"
					@input="
						$event => {
							binddata('userId', $event);
						}
					"
				/>
			</uni-forms-item>
			<uni-forms-item label="验证内容" required name="content">
				<textarea
					class="pages-textarea"
					auto-height
					maxlength="-1"
					placeholder="请输入验证内容"
					placeholder-style="color: #c3c1c1; z-index:0"
					v-model="formData.content"
					@input="binddata('content', $event.target.value)"
				/>
			</uni-forms-item>
			<uni-forms-item label="附件上传" name="files">
				<pickerImage :url="getUploadUrl()" v-model="formData.files" @input="binddata('files', $event)" />
			</uni-forms-item>
		</uni-forms>
		<view class="save-button">
			<button
				style="background-color: #de3636; margin-right: 20rpx"
				@tap="
					() => {
						isPassed = false;
						submit();
					}
				"
			>
				不通过
			</button>
			<button
				style="background-color: #28bf49"
				@tap="
					() => {
						isPassed = true;
						submit();
					}
				"
			>
				通过
			</button>
		</view>
	</view>
</template>

<script>
import pickerImage from '@/components/pickerImage.vue';
import memberSelect from '@/components/uniMemberSelect.vue';
import {getUploadUrl, requestIsSuccess, showToast} from '@/utils/util.js';
import {SafeQualityRecordState, SafeQualityRecordType} from '@/utils/enum.js';
import uniPicker from '@/components/uniPicker.vue';
import * as apiQuality from '@/api/quality/quality.js';

export default {
	name: 'qualityVerify',
	components: {
		uniPicker,
		pickerImage,
		memberSelect,
	},
	data() {
		return {
			SafeQualityRecordState,
			isPassed: true,
			formData: {
				title: '',
				qualityProblemId: '',
				type: '',
				state: '',
				userId: {},
				time: '',
				content: '',
				files: [],
			},
			rules: {
				userId: {
					rules: [{required: true, errorMessage: '请选择验证人'}],
				},
				time: {
					rules: [{required: true, errorMessage: '请选择验证时间'}],
				},
				content: {
					rules: [{required: true, errorMessage: '请填写验证内容'}],
				},
			},
		};
	},

	onLoad(option) {
		let optionData = JSON.parse(option.data);
		this.formData.title = optionData.title;
		this.formData.qualityProblemId = optionData.id;
	},

	methods: {
		getUploadUrl,
		submit() {
			let this_ = this;
			uni.showModal({
				title: '提示',
				content: '是否确认当前选择',
				success: function (ress) {
					if (ress.confirm) {
						this_.$refs.form
							.submit()
							.then(async res => {
								if (!this_.$store.state.isSubmit) {
									res = {
										...this_.formData,
										files: res.files.map(item => {
											return {fileId: item.id};
										}),
										userId: res.userId.id,
										time: res.time,
										content: res.content,
										type: SafeQualityRecordType.Verify,
										state: this_.isPassed ? SafeQualityRecordState.Passed : SafeQualityRecordState.UnCheck,
									};
									if (!this_.$store.state.isSubmit) {
										this_.$store.commit('SetIsSubmit', true);
										let response = await apiQuality.createRecord(res);
										if (requestIsSuccess(response)) {
											showToast(this_.isPassed ? `验证已通过` : `验证不通过`);
										} else {
											showToast('操作失败', false);
										}
									}
								}
							})
							.catch(err => {
								console.log('表单错误信息：', err);
							});
					}
				},
			});
		},
		// 返回
		onCancel() {
			uni.navigateBack();
		},
		binddata(name, value) {
			this.$refs.form.setValue(name, value);
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

#pages-uni-quality-verify {
	font-size: 28rpx;
	padding: 20rpx;
	margin-bottom: 120rpx;
	.verify-uni-form-item-input {
		padding: 0 20rpx;
		flex: 1;
		height: 70rpx;
		background-color: #ffffff;
		border-radius: 10rpx;
		font-size: 28rpx;
	}

	.uni-form-member-select {
		flex: 1;
	}
	.verify-uni-form-item-input {
		padding: 0 20rpx;
		flex: 1;
		height: 70rpx;
		background-color: #ffffff;
		border-radius: 10rpx;
	}

	.uni-form-member-select {
		flex: 1;
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
