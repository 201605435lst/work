<!-- 查询模态框 -->
<template>
	<uni-drawer ref="showRight" mode="right">
		<view class="uni-table-operator">
			<view class="uni-table-operator-item">
				<view class="uni-form-item uni-column">
					<view class="uni-form-item uni-form-item-title">关键字</view>
					<input
						class="uni-form-item-input"
						v-model="queryParams.keyword"
						placeholder="请输入备注/编号"
						placeholder-style="color: #c3c1c1;font-size: 28rpx;"
					/>
				</view>
				<view class="uni-form-item uni-column">
					<view class="uni-form-item uni-form-item-title">库存位置</view>
					<treeSelect
						action="/api/app/partition/getTreeList"
						placeholder="请选择库存位置"
						title="库存位置"
						v-model="queryParams.partitionId"
					/>
				</view>
				<view class="uni-form-item uni-column">
					<view class="uni-form-item uni-form-item-title">登记时间</view>
					<view class="uni-time-picker">
						<datePicker style="flex: 1" mode="date" placeholder="开始时间" v-model="queryParams.sTime" />
						<view style="width: 30rpx; text-align: center">-</view>
						<datePicker style="flex: 1" mode="date" placeholder="结束时间" v-model="queryParams.eTime" />
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
import datePicker from '@/components/datePicker.vue';
import treeSelect from '@/components/treeSelect.vue';
import moment from 'moment';
export default {
	name: 'materialEntryRecordDrawer',
	components: {datePicker, treeSelect},
	props: {},
	data() {
		return {
			queryParams: {
				partitionId: {}, //库存地点
				keyword: '', //材料名称
				sTime: '', //开始时间
				eTime: '', //结束时间
			},
			index: 0,
		};
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},
	watch: {
		queryParams: {
			handler(newName) {
				if (newName.sTime && newName.eTime) {
					let sTime = moment(newName.sTime).format('X');
					let eTime = moment(newName.eTime).format('X');
					if (sTime > eTime) {
						uni.showToast({
							icon: 'none',
							title: `结束时间不能小于开始时间`,
							duration: 1500,
						});
						this.queryParams.eTime = '';
					}
				}
			},
			deep: true,
		},
	},

	methods: {
		onShowDrawer(isShowDrawer = false) {
			let queryParams_ = {
				keyword: this.queryParams.keyword,
				partitionId: this.queryParams.partitionId && this.queryParams.partitionId.id ? this.queryParams.partitionId.id : '',
				sTime: this.queryParams.sTime ? moment(moment(this.queryParams.sTime)).format('YYYY-MM-DD HH:mm:ss') : '',
				eTime: this.queryParams.eTime ? moment(this.queryParams.eTime).endOf('d').format('YYYY-MM-DD HH:mm:ss') : '',
			};
			console.log(queryParams_);
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
				partitionId: {}, //库存地点
				keyword: '', //材料名称
				sTime: '', //开始时间
				eTime: '', //结束时间
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
