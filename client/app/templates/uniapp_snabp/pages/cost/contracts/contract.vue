<!-- 合同信息页面 -->
<template>
	<view id="pages-uni-contract">
		<uni-forms ref="form" :value="contract" :rules="rules">
			<uni-forms-item label="合同名称" required name="name">
				<textarea
					class="pages-textarea"
					:disabled="pageState == PageState.View"
					auto-height
					maxlength="-1"
					v-model="contract.name"
					placeholder="请输入合同名称"
					placeholder-style="color: #c3c1c1; z-index:0"
					@input="binddata('name', $event.target.value)"
				/>
			</uni-forms-item>

			<uni-forms-item label="合同日期" required name="date">
				<uniPicker
					mode="date"
					:disabled="pageState == PageState.View"
					v-model="contract.date"
					placeholder="请选择合同日期"
					@input="binddata('date', $event)"
				/>
			</uni-forms-item>

			<uni-forms-item label="合同类型" required name="type">
				<uniPicker
					mode="selector"
					placeholder="请选择合同类型"
					groupCode="CostmanagementContract"
					:disabled="pageState == PageState.View"
					range-key="name"
					v-model="contract.type"
					@input="binddata('type', $event)"
				/>
			</uni-forms-item>

			<uni-forms-item label="合同金额(万元)" required name="money">
				<textarea
					class="pages-textarea"
					:disabled="pageState == PageState.View"
					auto-height
					maxlength="-1"
					v-model="contract.money"
					placeholder="请输入合同金额"
					placeholder-style="color: #c3c1c1; z-index:0"
					@input="binddata('money', $event.target.value)"
				/>
			</uni-forms-item>

			<uni-forms-item label="图片上传" name="contractRltFiles">
				<text v-if="pageState == PageState.View && contract.contractRltFiles.length == 0">--</text>
				<pickerImage
					v-else
					style="flex: 1"
					:url="getUploadUrl()"
					:disabled="pageState == PageState.View"
					v-model="contract.contractRltFiles"
					@input="binddata('contractRltFiles', $event)"
				/>
			</uni-forms-item>

			<uni-forms-item label="备注" name="remark">
				<textarea
					class="pages-textarea"
					:disabled="pageState == PageState.View"
					auto-height
					maxlength="-1"
					v-model="contract.remark"
					placeholder="请输入备注"
					placeholder-style="color: #c3c1c1; z-index:0"
					@input="binddata('remark', $event.target.value)"
				/>
			</uni-forms-item>
		</uni-forms>
		<view class="save-button">
			<button v-if="pageState != PageState.View" @tap="submit">提交</button>
			<button v-else @tap="cancel">返回</button>
		</view>
	</view>
</template>

<script>
import {checkToken, requestIsSuccess, showToast, getUploadUrl, getCode, setTimeRange} from '@/utils/util.js';
import {PageState} from '@/utils/enum.js';
import pickerImage from '@/components/pickerImage.vue';
import uniPicker from '@/components/uniPicker.vue';
import * as apiContract from '@/api/costManagement/contract.js';
import moment from 'moment';

export default {
	name: 'contract',
	components: {uniPicker, pickerImage},
	data() {
		return {
			PageState,
			id: '',
			pageState: PageState.Add,
			contract: {
				name: '',
				date: '',
				type: {},
				remark: '',
				money: '',
				contractRltFiles: [],
			},
			rules: {
				type: {rules: [{required: true, errorMessage: '请选择合同类型'}]},
				name: {rules: [{required: true, errorMessage: '请输入合同名称'}]},
				money: {rules: [{required: true, errorMessage: '请输入合同金额'}]},
				date: {rules: [{required: true, errorMessage: '请选择合同日期'}]},
			},
		};
	},
	onLoad(option) {
		this.pageState = option.pageState;
		this.id = option.id;
		this.refresh();
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},

	methods: {
		getUploadUrl,
		async refresh() {
			if (!this.id) return;
			const response = await apiContract.get(this.id);
			if (requestIsSuccess(response)) {
				let _contract = response.data;
				this.contract = {
					..._contract,
					date: moment(_contract.date).format('YYYY-MM-DD'),
					contractRltFiles:
						_contract.contractRltFiles && _contract.contractRltFiles.length > 0
							? _contract.contractRltFiles.map(item => {
									return item.file;
							  })
							: [],
				};
			}
		},
		cancel() {
			uni.navigateBack();
		},

		submit() {
			let _this = this;
			_this.$refs.form
				.submit()
				.then(async res => {
					let data = {
						...res,
						code: _this.pageState == PageState.Add ? getCode('HT') : _this.contract.id,
						typeId: res.type ? res.type.id : '',
						contractRltFiles:
							res.contractRltFiles && res.contractRltFiles.length > 0
								? res.contractRltFiles.map(item => {
										return {fileId: item.id};
								  })
								: [],
					};
					if (!setTimeRange(data.date, {compare: '>'}) && !this.$store.state.isSubmit) {
						this.$store.commit('SetIsSubmit', true);
						let response;
						if (_this.pageState == PageState.Add) {
							response = await apiContract.create(data);
						} else if (_this.pageState == PageState.Edit) {
							response = await apiContract.update({...data, id: _this.contract.id});
						}
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
	},
};
</script>

<style lang="scss">
page {
	background-color: #f9f9f9;
}
#pages-uni-contract {
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

	.uni-form-item-input {
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

	.pages-uni-contract-item {
		flex: 1;
		height: 70rpx;
		background-color: #ffffff;
		border-radius: 10rpx;
	}

	.entry-record-add {
		display: flex;
		justify-content: space-between;
		align-items: center;
		margin: 40rpx 0 20rpx 0;
	}

	.entry-record-add-button {
		display: flex;
		align-items: center;
	}

	.icon-scan1 {
		font-size: 50rpx;
		color: #1890ff;
		margin-left: 20rpx;
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
