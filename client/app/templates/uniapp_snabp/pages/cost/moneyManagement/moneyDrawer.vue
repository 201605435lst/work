<!-- 查询模态框 -->
<template>
	<uni-drawer ref="showRight" mode="right">
		<view class="uni-table-operator">
			<view class="uni-table-operator-item">
				<view class="uni-form-item uni-column">
					<view class="uni-form-item uni-form-item-title">资金类别</view>
					<uniPicker
						mode="selector"
						groupCode="CostmanagementCapitalCategory"
						placeholder="请选择资金类别"
						range-key="name"
						v-model="queryParams.type"
					/>
				</view>
				<view class="uni-form-item uni-column">
					<view class="uni-form-item uni-form-item-title">收款单位</view>
					<uniPicker
						mode="selector"
						placeholder="请选择收款单位"
						groupCode="CostmanagementCostPayee"
						range-key="name"
						v-model="queryParams.payee"
					/>
				</view>
				<view class="uni-form-item uni-column">
					<view class="uni-form-item uni-form-item-title">费用周期</view>
					<view class="uni-time-picker">
						<datePicker style="flex: 1" mode="date" placeholder="开始时间" v-model="queryParams.startTime" />
						<view style="width: 30rpx; text-align: center">-</view>
						<datePicker style="flex: 1" mode="date" placeholder="结束时间" v-model="queryParams.endTime" />
					</view>
				</view>
			</view>
			<view class="uni-table-operator-buttons">
				<button class="uni-table-operator-button" @click="onReset" size="mini">重置</button>
				<button
					class="uni-table-operator-button"
					@click="onShowDrawer(false)"
					style="background-color: #1890ff; color: #ffffff"
					size="mini"
				>
					查询
				</button>
			</view>
		</view>
	</uni-drawer>
</template>

<script>
import uniPicker from '@/components/uniPicker.vue';
import datePicker from '@/components/datePicker.vue';
import moment from 'moment';
export default {
	name: 'moneyDrawer',
	components: {uniPicker, datePicker},
	props: {},
	data() {
		return {
			queryParams: {
				type: '', //合同类型
				payee: '', //付款单位id
				startTime: '', //开始时间
				endTime: '', //结束时间
			},
		};
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},
	watch: {
		queryParams: {
			handler(newName) {
				if (newName.startTime && newName.endTime) {
					let startTime = moment(newName.startTime).format('X');
					let endTime = moment(newName.endTime).format('X');
					if (startTime > endTime) {
						uni.showToast({
							icon: 'none',
							title: `结束时间不能小于开始时间`,
							duration: 1500,
						});
						this.queryParams.endTime = '';
					}
				}
			},
			deep: true,
		},
	},

	methods: {
		onShowDrawer(isShowDrawer = false) {
			let queryParams_ = {
				typeId: this.queryParams.type ? this.queryParams.type.id : '',
				payeeId: this.queryParams.payee ? this.queryParams.payee.id : '',
				startTime: this.queryParams.startTime ? moment(this.queryParams.startTime).format('YYYY-MM-DD HH:mm:ss') : '',
				endTime: this.queryParams.endTime ? moment(this.queryParams.endTime).endOf('d').format('YYYY-MM-DD HH:mm:ss') : '',
			};
			if (isShowDrawer) {
				this.$refs['showRight'].open();
			} else {
				this.$refs['showRight'].close();
				this.$emit('change', queryParams_);
			}
		},

		//条件重置
		onReset() {
			this.queryParams = {
				type: '', //合同类型
				payee: '', //付款单位id
				startTime: '', //开始时间
				endTime: '', //结束时间
			};
		},
	},
};
</script>

<style>
.uni-table-operator {
	background-color: #f9f9f9;
	display: flex;
	flex-direction: column;
	height: 100%;
	justify-content: space-between;
}

.uni-table-operator-item {
	padding: 20rpx;
	font-size: 28rpx;
}

.uni-form-item {
	margin-bottom: 10rpx;
}

.uni-form-item-title {
	margin-left: 10rpx;
}

.uni-form-item-input {
	font-size: 28rpx;
	background-color: #ffffff;
	padding: 0 20rpx;
	border-radius: 10rpx;
	height: 70rpx;
	display: flex;
	align-items: center;
}

.uni-time-picker {
	display: flex;
	align-items: center;
}
</style>
