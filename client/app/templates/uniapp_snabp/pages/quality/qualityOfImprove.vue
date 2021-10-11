<!-- 质量问题整改页面 -->
<template>
	<view id="pages-uni-quality-improve">
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
			<uni-forms-item label="整改时间" required name="time">
				<uniPicker mode="date" placeholder="请选择整改时间" v-model="formData.time" @input="binddata('time', $event)" />
			</uni-forms-item>
			<uni-forms-item label="责任人" required name="userId">
				<memberSelect
					class="uni-form-member-select"
					placeholder="请选择责任人"
					v-model="formData.userId"
					style="flex: 1"
					@input="
						$event => {
							binddata('userId', $event);
						}
					"
				/>
			</uni-forms-item>
			<uni-forms-item label="整改内容" name="content">
				<textarea
					class="pages-textarea"
					auto-height
					maxlength="-1"
					placeholder="请输入整改内容"
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
			<button @tap="submit">整改</button>
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
	name: 'qualityImprove',
	components: {
		uniPicker,
		pickerImage,
		memberSelect,
	},
	data() {
		return {
			SafeQualityRecordState,
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
					rules: [
						{
							required: true,
							errorMessage: '请选择责任人',
						},
					],
				},
				time: {
					rules: [
						{
							required: true,
							errorMessage: '请选择整改时间',
						},
					],
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
			this.$refs.form
				.submit()
				.then(async res => {
					res = {
						...this.formData,
						files: res.files.map(item => {
							return {fileId: item.id};
						}),
						userId: res.userId.id,
						time: res.time,
						content: res.content && res.content.length > 0 ? res.content : '',
						type: SafeQualityRecordType.Improve,
						state: SafeQualityRecordState.Checking,
					};
					if (!this.$store.state.isSubmit) {
						this.$store.commit('SetIsSubmit', true);
						let response = await apiQuality.createRecord(res);
						if (requestIsSuccess(response)) {
							showToast('整改成功');
						} else {
							showToast('整改失败', false);
						}
					}
				})
				.catch(err => {
					console.log('表单错误信息：', err);
				});
		},
		// 返回
		onCancel() {
			uni.navigateBack();
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

#pages-uni-quality-improve {
	font-size: 28rpx;
	padding: 20rpx;
	margin-bottom: 120rpx;
	.improve-uni-form-item-input {
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
	.pages-uni-quality-improve-item {
		flex: 1;
		height: 70rpx;
		background-color: #ffffff;
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
