<!-- 人工成本录入页面 -->
<template>
	<view id="pages-uni-labor-cost">
		<uni-forms ref="form" :value="laborCost" :rules="rules">
			<uni-forms-item label="成本对象" required name="professional">
				<uniPicker
					mode="selector"
					placeholder="请选择成本对象"
					groupCode="Profession"
					:disabled="pageState == PageState.View"
					range-key="name"
					v-model="laborCost.professional"
					@input="binddata('professional', $event)"
				/>
			</uni-forms-item>

			<uni-forms-item label="收款单位" required name="payee">
				<uniPicker
					mode="selector"
					placeholder="请选择收款单位"
					groupCode="CostmanagementCostPayee"
					:disabled="pageState == PageState.View"
					range-key="name"
					v-model="laborCost.payee"
					@input="binddata('payee', $event)"
				/>
			</uni-forms-item>

			<uni-forms-item label="付款时间" required name="date">
				<uniPicker
					mode="date"
					:disabled="pageState == PageState.View"
					v-model="laborCost.date"
					placeholder="请选择付款时间"
					@input="binddata('date', $event)"
				/>
			</uni-forms-item>

			<uni-forms-item label="费用金额(万元)" required name="money">
				<textarea
					class="pages-textarea"
					:disabled="pageState == PageState.View"
					auto-height
					maxlength="-1"
					v-model="laborCost.money"
					placeholder="请输入费用金额"
					placeholder-style="color: #c3c1c1; z-index:0"
					@input="binddata('money', $event.target.value)"
				/>
			</uni-forms-item>

			<uni-forms-item label="备注" name="remark">
				<textarea
					class="pages-textarea"
					:disabled="pageState == PageState.View"
					auto-height
					maxlength="-1"
					v-model="laborCost.remark"
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
import {checkToken, requestIsSuccess, showToast, getUploadUrl, setTimeRange} from '@/utils/util.js';
import {PageState} from '@/utils/enum.js';
import uniPicker from '@/components/uniPicker.vue';
import * as apiLaborCost from '@/api/costManagement/laborCost.js';
import moment from 'moment';

export default {
	name: 'laborCost',
	components: {uniPicker},
	data() {
		return {
			PageState,
			id: '',
			pageState: PageState.Add,
			laborCost: {
				date: '',
				professional: '',
				payee: '',
				remark: '',
				money: '',
			},
			rules: {
				professional: {rules: [{required: true, errorMessage: '请选择成本对象'}]},
				payee: {rules: [{required: true, errorMessage: '请选择付款单位'}]},
				money: {rules: [{required: true, errorMessage: '请输入费用金额'}]},
				date: {rules: [{required: true, errorMessage: '请选择付款时间'}]},
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
			const response = await apiLaborCost.get(this.id);
			if (requestIsSuccess(response)) {
				let _laborCost = response.data;
				this.laborCost = {
					..._laborCost,
					date: moment(_laborCost.date).format('YYYY-MM-DD'),
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
						payeeId: res.payee ? res.payee.id : '',
						professionalId: res.professional ? res.professional.id : '',
					};
					if (!setTimeRange(data.date, {compare: '>'}) && !this.$store.state.isSubmit) {
						this.$store.commit('SetIsSubmit', true);
						let response;
						if (_this.pageState == PageState.Add) {
							response = await apiLaborCost.create(data);
						} else if (_this.pageState == PageState.Edit) {
							response = await apiLaborCost.update({...data, id: _this.laborCost.id});
						}
						if (response && requestIsSuccess(response)) {
							showToast('操作成功');
						} else {
							showToast('操作失败', false);
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
#pages-uni-labor-cost {
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
