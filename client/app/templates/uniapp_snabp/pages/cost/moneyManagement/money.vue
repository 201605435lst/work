<!-- 资金信息录入 -->
<template>
	<view id="pages-uni-money">
		<uni-forms ref="form" :value="money" :rules="rules">
			<uni-forms-item label="资金类别" required name="type">
				<uniPicker
					mode="selector"
					groupCode="CostmanagementCapitalCategory"
					placeholder="请选择资金类别"
					range-key="name"
					:disabled="pageState == PageState.View"
					v-model="money.type"
					@input="binddata('type', $event)"
				/>
				<!-- <treeSelect
					action="/api/app/appDataDictionary/getValues"
					groupCode="CostmanagementCapitalCategory"
					placeholder="请选择资金类别"
					title="资金类别"
					:disabled="pageState == PageState.View"
					v-model="money.type"
					@input="binddata('type', $event)"
				/> -->
			</uni-forms-item>
			<uni-forms-item label="收款单位" required name="payee">
				<uniPicker
					mode="selector"
					placeholder="请选择收款单位"
					groupCode="CostmanagementCostPayee"
					:disabled="pageState == PageState.View"
					range-key="name"
					v-model="money.payee"
					@input="binddata('payee', $event)"
				/>
			</uni-forms-item>

			<uni-forms-item label="应收(万元)" required name="receivable">
				<textarea
					class="pages-textarea"
					:disabled="pageState == PageState.View"
					auto-height
					maxlength="-1"
					v-model="money.receivable"
					placeholder="请输入应收金额"
					placeholder-style="color: #c3c1c1; z-index:0"
					@input="binddata('receivable', $event.target.value)"
				/>
			</uni-forms-item>

			<uni-forms-item label="已收(万元)" required name="received">
				<textarea
					class="pages-textarea"
					:disabled="pageState == PageState.View"
					auto-height
					maxlength="-1"
					v-model="money.received"
					placeholder="请输入已收金额"
					placeholder-style="color: #c3c1c1; z-index:0"
					@input="binddata('received', $event.target.value)"
				/>
			</uni-forms-item>

			<uni-forms-item label="应付(万元)" required name="due">
				<textarea
					class="pages-textarea"
					:disabled="pageState == PageState.View"
					auto-height
					maxlength="-1"
					v-model="money.due"
					placeholder="请输入应付金额"
					placeholder-style="color: #c3c1c1; z-index:0"
					@input="binddata('due', $event.target.value)"
				/>
			</uni-forms-item>

			<uni-forms-item label="已付(万元)" required name="paid">
				<textarea
					class="pages-textarea"
					:disabled="pageState == PageState.View"
					auto-height
					maxlength="-1"
					v-model="money.paid"
					placeholder="请输入已付金额"
					placeholder-style="color: #c3c1c1; z-index:0"
					@input="binddata('paid', $event.target.value)"
				/>
			</uni-forms-item>

			<uni-forms-item label="时间" required name="date">
				<uniPicker
					mode="date"
					:disabled="pageState == PageState.View"
					v-model="money.date"
					placeholder="请选择时间"
					@input="binddata('date', $event)"
				/>
			</uni-forms-item>

			<uni-forms-item label="备注" name="remark">
				<textarea
					class="pages-textarea"
					:disabled="pageState == PageState.View"
					auto-height
					maxlength="-1"
					v-model="money.remark"
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
import {checkToken, requestIsSuccess, showToast, setTimeRange} from '@/utils/util.js';
import {PageState} from '@/utils/enum.js';
import uniPicker from '@/components/uniPicker.vue';
import * as apiMoney from '@/api/costManagement/money.js';
import treeSelect from '@/components/treeSelect.vue';
import moment from 'moment';

export default {
	name: 'money',
	components: {
		uniPicker,
		treeSelect,
	},
	data() {
		return {
			PageState,
			id: '',
			pageState: PageState.Add,
			money: {
				type: '',
				payee: '',
				receivable: '',
				received: '',
				due: '',
				paid: '',
				date: '',
				remark: '',
			},
			rules: {
				// 对name字段进行必填验证
				type: {rules: [{required: true, errorMessage: '请选择资金类型'}]},
				payee: {rules: [{required: true, errorMessage: '请选择收款单位'}]},
				receivable: {rules: [{required: true, errorMessage: '请输入应收金额'}]},
				received: {rules: [{required: true, errorMessage: '请输入已收金额'}]},
				due: {rules: [{required: true, errorMessage: '请输入应付金额'}]},
				paid: {rules: [{required: true, errorMessage: '请输入已付金额'}]},
				date: {rules: [{required: true, errorMessage: '请选择时间'}]},
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
		async refresh() {
			if (!this.id) return;
			const response = await apiMoney.get(this.id);
			if (requestIsSuccess(response)) {
				let _money = response.data;
				this.money = {
					..._money,
					date: moment(_money.date).format('YYYY-MM-DD'),
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
						typeId: res.type ? res.type.id : '',
						payeeId: res.payee ? res.payee.id : '',
					};

					if (!setTimeRange(data.date, {compare: '>'}) && !this.$store.state.isSubmit) {
						this.$store.commit('SetIsSubmit', true);
						let response;
						if (_this.pageState == PageState.Add) {
							response = await apiMoney.create(data);
						} else if (_this.pageState == PageState.Edit) {
							response = await apiMoney.update({...data, id: _this.money.id});
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
#pages-uni-money {
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

	.pages-uni-money-item {
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
