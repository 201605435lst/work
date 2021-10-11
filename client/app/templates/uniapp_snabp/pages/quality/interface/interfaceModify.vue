<template>
	<view class="quality-interfaceModify">
		<uni-forms ref="form" :value="formData" :rules="rules">
			<uni-forms-item label="接口名称" required name="name">
				<textarea
					class="pages-textarea"
					auto-height
					disabled
					maxlength="-1"
					placeholder="请输入接口名称"
					placeholder-style="color: #c3c1c1; z-index:0"
					v-model="formData.name"
				/>
			</uni-forms-item>
			<uni-forms-item label="整改人" required name="reformer">
				<memberSelect
					style="flex: 1"
					placeholder="请选择整改人"
					v-model="formData.reformer"
					@input="
						$event => {
							binddata('checker', $event);
						}
					"
				/>
			</uni-forms-item>
			<uni-forms-item label="整改时间" required name="reformDate">
				<uniPicker mode="date" :value="formData.reformDate" @input="binddata('reformDate', $event)" />
			</uni-forms-item>
			<uni-forms-item label="整改意见" required name="reformExplain">
				<textarea
					class="pages-textarea"
					auto-height
					maxlength="-1"
					placeholder="请输入整改意见"
					placeholder-style="color: #c3c1c1; z-index:0"
					v-model="formData.reformExplain"
					@input="binddata('reformExplain', $event.target.value)"
				/>
			</uni-forms-item>
			<uni-forms-item label="附件上传" name="markFiles">
				<pickerImage :url="getUploadUrl()" v-model="formData.markFiles" @input="binddata('markFiles', $event)" />
			</uni-forms-item>
		</uni-forms>
		<view class="save-button">
			<button @tap="submit">整改</button>
		</view>
	</view>
</template>

<script>
import prefixText from '@/components/prefixText.vue';
import uniPicker from '@/components/uniPicker.vue';
import pickerImage from '@/components/pickerImage.vue';
import {showToast, getUploadUrl, requestIsSuccess} from '@/utils/util.js';
import * as apiConstructInterfaceInfo from '@/api/quality/constructInterfaceInfo.js';
import memberSelect from '@/components/uniMemberSelect.vue';
import moment from 'moment';
export default {
	components: {uniPicker, prefixText, pickerImage, memberSelect},
	data() {
		return {
			formData: {
				name: '', // 接口名称
				reformDate: '', // 整改时间
				reformExplain: '', // 整改意见
				markFiles: [], // 整改文件
				reformer: {}, //整改人
				constructInterfaceId: '', // 接口清单id
			},
			rules: {
				reformDate: {
					rules: [{required: true, errorMessage: '请选择整改时间'}],
				},
				reformer: {
					rules: [{required: true, errorMessage: '请选择整改人'}],
				},
				reformExplain: {
					rules: [{required: true, errorMessage: '请选择整改意见'}],
				},
			},
		};
	},
	onLoad(option) {
		this.formData.name = option.name;
		this.formData.reformerId = this.$store.state.creatorId;
		this.formData.constructInterfaceId = option.id;
	},
	methods: {
		getUploadUrl,
		async submit() {
			this.$refs.form
				.submit()
				.then(async res => {
					res = {
						...res,
						markFiles: res.markFiles.map(item => {
							return {
								MarkFileId: item.id,
								type: 2,
							};
						}),
						reformDate: res.reformDate + ` ${moment().format('HH:mm:ss')}`,
						reformerId: res.reformer && res.reformer.id ? res.reformer.id : '',
						constructInterfaceId: this.formData.constructInterfaceId,
					};
					if (!this.$store.state.isSubmit) {
						this.$store.commit('SetIsSubmit', true);
						let response = await apiConstructInterfaceInfo.interfanceReform(res);
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
};
</script>

<style lang="scss">
page {
	background-color: #f9f9f9;
}
.quality-interfaceModify {
	font-size: 28rpx;
	padding: 20rpx;
	margin-bottom: 120rpx;
	.save-button {
		width: 100%;
		display: flex;
		position: fixed;
		z-index: 90;
		bottom: 0;
		left: 0;
		background-color: #f9f9f9;
		padding: 20rpx 0;
		> button {
			color: #ffffff;
			width: 100%;
			border-radius: 100rpx;
			margin: 0 20rpx;
			background-color: #1890ff;
		}
	}
}
</style>
