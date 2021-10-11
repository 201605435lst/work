<template>
	<view id="pages-quality-interfaceSign">
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
			<uni-forms-item label="检查状况" required name="markType">
				<uniPicker
					mode="selector"
					class="sm-input-class"
					:range="markTypeRange"
					:range-key="'name'"
					@input="
						$event => {
							binddata('markType', $event);
						}
					"
				/>
			</uni-forms-item>
			<uni-forms-item label="检查人" required name="marker">
				<memberSelect
					style="flex: 1"
					placeholder="请选择检查人"
					v-model="formData.marker"
					@input="
						$event => {
							binddata('checker', $event);
						}
					"
				/>
			</uni-forms-item>
			<uni-forms-item label="检查时间" required name="markDate">
				<uniPicker mode="date" class="sm-input-class" :value="formData.markDate" @input="binddata('markDate', $event)" />
			</uni-forms-item>
			<uni-forms-item label="土建单位" required name="builder">
				<uniPicker
					mode="selector"
					class="sm-input-class"
					groupCode="ConstructionUnit"
					@input="
						$event => {
							binddata('builder', $event);
						}
					"
				/>
			</uni-forms-item>
			<uni-forms-item label="状况原因" name="reason">
				<textarea
					class="pages-textarea"
					auto-height
					maxlength="-1"
					placeholder="请输入状况原因"
					placeholder-style="color: #c3c1c1; z-index:0"
					v-model="formData.reason"
					@input="binddata('reason', $event.target.value)"
				/>
			</uni-forms-item>
			<uni-forms-item label="附件上传" name="markFiles">
				<pickerImage :url="getUploadUrl()" v-model="formData.markFiles" @input="binddata('markFiles', $event)" />
			</uni-forms-item>
		</uni-forms>
		<view class="save-button">
			<button @tap="submit">标记</button>
		</view>
	</view>
</template>

<script>
import {showToast, requestIsSuccess, getUploadUrl} from '@/utils/util.js';
import uniPicker from '@/components/uniPicker.vue';
import * as apiConstructInterfaceInfo from '@/api/quality/constructInterfaceInfo.js';
import pickerImage from '@/components/pickerImage.vue';
import memberSelect from '@/components/uniMemberSelect.vue';
import moment from 'moment';
export default {
	components: {uniPicker, pickerImage, memberSelect},
	data() {
		return {
			markTypeRange: [
				{name: '合格', markType: 2},
				{name: '不合格', markType: 3},
			],
			formData: {
				name: '', // 接口名称
				markType: '', // 检查状况
				markDate: '', // 检查时间
				builder: '', // 土建单位
				reason: '', // 状况原因
				markFiles: [], // 检查文件

				marker: {}, //检查人
				constructInterfaceId: '', // 接口清单id
			},
			rules: {
				markType: {
					rules: [{required: true, errorMessage: '请选择检查状况'}],
				},
				marker: {
					rules: [{required: true, errorMessage: '请选择检查人'}],
				},
				markDate: {
					rules: [{required: true, errorMessage: '请选择检查时间'}],
				},
				builder: {
					rules: [{required: true, errorMessage: '请选择土建单位'}],
				},
			},
		};
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},
	onLoad(option) {
		this.formData.name = option.name;
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
								type: 1,
							};
						}),
						markDate: res.markDate + ` ${moment().format('HH:mm:ss')}`,
						builderId: res.builder.id,
						markType: res.markType.markType,
						markerId: res.marker && res.marker.id ? res.marker.id : '',
						constructInterfaceId: this.formData.constructInterfaceId,
					};
					if (!this.$store.state.isSubmit) {
						this.$store.commit('SetIsSubmit', true);
						let response = await apiConstructInterfaceInfo.create(res);
						if (requestIsSuccess(response)) {
							showToast('提交成功');
						} else {
							showToast('提交失败', false);
						}
					}
				})
				.catch(err => {
					console.log('表单错误信息：', err);
				});
		},
		binddata(name, value) {
			this.$refs.form.setValue(name, value);
		},
	},
};
</script>

<style lang="scss">
page {
	background-color: #f9f9f9;
}
#pages-quality-interfaceSign {
	font-size: 28rpx;
	padding: 20rpx;
	margin-bottom: 120rpx;
	.pages-quality-interfaceSign-item {
		padding: 0 20rpx;
		flex: 1;
		height: 70rpx;
		background-color: #ffffff;
		border-radius: 10rpx;
		font-size: 28rpx;
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
