<!-- 安全问题标记页面 -->
<template>
	<view id="pages-uni-security-improve">
		<uni-forms ref="form" :value="securityImprove" :rules="rules">
			<uni-forms-item label="问题标题" name="title">
				<textarea
					class="pages-textarea"
					disabled
					auto-height
					v-model="securityImprove.title"
					maxlength="-1"
					placeholder="请输入问题标题"
					placeholder-style="color: #c3c1c1; z-index:0"
					@input="binddata('title', $event.target.value)"
				/>
			</uni-forms-item>
			<uni-forms-item label="整改时间" required name="time">
				<uniPicker
					style="flex: 1"
					mode="date"
					v-model="securityImprove.time"
					placeholder="请选择整改时间"
					@input="binddata('time', $event)"
				/>
			</uni-forms-item>
			<uni-forms-item label="责任人" required name="user">
				<memberSelect
					v-model="securityImprove.user"
					placeholder="请选择责任人"
					class="uni-form-member-select"
					@input="binddata('user', $event)"
				></memberSelect>
			</uni-forms-item>
			<uni-forms-item label="整改内容" name="content">
				<textarea
					class="pages-textarea"
					auto-height
					maxlength="-1"
					placeholder="请输入整改内容"
					placeholder-style="color: #c3c1c1; z-index:0"
					v-model="securityImprove.content"
					@input="binddata('content', $event.target.value)"
				/>
			</uni-forms-item>
			<uni-forms-item label="图片上传" name="files">
				<pickerImage style="flex: 1" :url="getUploadUrl()" v-model="securityImprove.files" @input="binddata('files', $event)" />
			</uni-forms-item>
		</uni-forms>
		<view class="save-button">
			<button @tap="submit">整改</button>
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
	name: 'securityImprove',
	components: {uniPicker, dictionaryPicker, memberSelect, treeSelect, pickerImage},
	data() {
		return {
			SafeQualityRecordType,
			SafeQualityRecordState,
			securityImprove: {
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
		this.securityImprove = {
			id: option.id,
			title: option.title,
			time: '',
			content: '',
			user: {},
			files: [],
		};
	},

	methods: {
		getUploadUrl,
		submit() {
			this.$refs.form
				.submit()
				.then(async res => {
					let data = {
						safeProblemId: this.securityImprove.id,
						type: SafeQualityRecordType.Improve,
						state: SafeQualityRecordState.Checking,
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
					if (!this.$store.state.isSubmit) {
						this.$store.commit('SetIsSubmit', true);
						let response = await apiSafeProblem.createRecord(data);
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
#pages-uni-security-improve {
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
