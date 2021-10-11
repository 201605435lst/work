<!-- 查询模态框 -->
<template>
	<uni-drawer ref="showRight" mode="right">
		<view class="uni-table-operator">
			<view class="uni-table-operator-item">
				<view class="uni-form-item uni-column">
					<view class="uni-form-item uni-form-item-title">文件名称</view>
					<input
						class="uni-form-item-input"
						v-model="queryParams.name"
						placeholder="请输入文件名称"
						placeholder-style="color: #c3c1c1;font-size: 28rpx;"
					/>
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
import uniPicker from '@/components/uniPicker.vue';
export default {
	name: 'materialOutRecordDrawer',
	components: {datePicker, uniPicker},
	data() {
		return {
			queryParams: {
				name: '', //备注/编号
			},
			index: 0,
		};
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},
	methods: {
		onShowDrawer(isShowDrawer = false) {
			let queryParams_ = {
				name: this.queryParams.name,
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
			this.queryParams = {};
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
