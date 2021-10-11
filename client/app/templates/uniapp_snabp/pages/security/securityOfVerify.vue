<!-- 安全问题标记页面 -->
<template>
	<view id="pages-uni-security-verify">
		<uni-forms ref="form" :value="securityVerify" :rules="rules">
			<uni-forms-item label="问题标题" name="title">
				<textarea
					class="pages-textarea"
					disabled
					auto-height
					v-model="securityVerify.title"
					maxlength="-1"
					placeholder="请输入问题标题"
					placeholder-style="color: #c3c1c1; z-index:0"
				/>
			</uni-forms-item>
			<uni-forms-item label="验证时间" required name="time">
				<uniPicker
					style="flex: 1"
					mode="date"
					v-model="securityVerify.time"
					placeholder="请选择验证时间"
					@input="binddata('time', $event)"
				/>
			</uni-forms-item>
			<uni-forms-item label="验证人" required name="user">
				<memberSelect
					v-model="securityVerify.user"
					placeholder="请选择验证人"
					class="uni-form-member-select"
					@input="binddata('user', $event)"
				></memberSelect>
			</uni-forms-item>
			<uni-forms-item label="验证内容" name="content">
				<textarea
					class="pages-textarea"
					auto-height
					maxlength="-1"
					placeholder="请输入验证内容"
					placeholder-style="color: #c3c1c1; z-index:0"
					v-model="securityVerify.content"
					@input="binddata('content', $event.target.value)"
				/>
			</uni-forms-item>
			<uni-forms-item label="图片上传" name="files">
				<pickerImage style="flex: 1" :url="getUploadUrl()" v-model="securityVerify.files" @input="binddata('files', $event)" />
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
import {requestIsSuccess, getUploadUrl, showToast} from '@/utils/util.js';
import {SafeQualityRecordType, SafeQualityRecordState} from '@/utils/enum.js';
import uniPicker from '@/components/uniPicker.vue';
import dictionaryPicker from '@/components/dictionaryPicker.vue';
import memberSelect from '@/components/uniMemberSelect.vue';
import treeSelect from '@/components/treeSelect.vue';
import pickerImage from '@/components/pickerImage.vue';
import * as apiSafeProblem from '@/api/safe/safeProblem.js';

export default {
	name: 'securityVerify',
	components: {uniPicker, dictionaryPicker, memberSelect, treeSelect, pickerImage},
	data() {
		return {
			isPassed: true,
			securityVerify: {
				title: '',
				time: '',
				content: '',
				user: {},
				files: [],
			},
			rules: {
				user: {
					rules: [
						{
							required: true,
							errorMessage: '请选择验证人',
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
		this.securityVerify = {
			id: option.id,
			title: option.title,
			time: '',
			content: '',
			user: {},
			files: [],
		};
	},

	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
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
									let data = {
										safeProblemId: this_.securityVerify.id,
										type: SafeQualityRecordType.Verify,
										state: this_.isPassed ? SafeQualityRecordState.Passed : SafeQualityRecordState.UnCheck,
										time: res.time,
										content: res.content,
										userId: res.user ? res.user.id : '',
										files:
											res.files.length > 0
												? res.files.map(item => {
														return {
															fileId: item.id,
														};
												  })
												: [],
									};
									if (!this_.$store.state.isSubmit) {
										this_.$store.commit('SetIsSubmit', true);
										let response = await apiSafeProblem.createRecord(data);
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
	},
};
</script>

<style lang="scss">
page {
	background-color: #f9f9f9;
}
#pages-uni-security-verify {
	font-size: 28rpx;
	padding: 20rpx;
	margin-bottom: 120rpx;
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
