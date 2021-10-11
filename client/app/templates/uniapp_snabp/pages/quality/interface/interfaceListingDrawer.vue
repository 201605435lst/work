<!-- 查询模态框 -->
<template>
	<uni-drawer ref="showRight" mode="right">
		<view class="uni-table-operator">
			<view class="uni-table-operator-item">
				<view class="uni-form-item uni-column">
					<view class="uni-form-item uni-form-item-title">专业</view>
					<uniPicker
						placeholder="请选择专业"
						mode="selector"
						class="sm-input-class"
						groupCode="Profession"
						v-model="queryParams.professionId"
					/>
				</view>
				<view class="uni-form-item uni-column">
					<view class="uni-form-item uni-form-item-title">土建单位</view>
					<uniPicker
						placeholder="请选择土建单位"
						mode="selector"
						class="sm-input-class"
						groupCode="ConstructionUnit"
						v-model="queryParams.builderId"
					/>
				</view>
				<view class="uni-form-item uni-column">
					<view class="uni-form-item uni-form-item-title">检查情况</view>
					<uniPicker
						placeholder="请选择检查情况"
						mode="selector"
						class="sm-input-class"
						:range="stateList"
						range-key="name"
						v-model="queryParams.markType"
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
import uniPicker from '@/components/uniPicker.vue';
import datePicker from '@/components/datePicker.vue';
export default {
	name: 'qualityDrawer',
	components: {uniPicker, datePicker},
	props: {},
	data() {
		return {
			conList: [],
			stateList: [
				{name: '未检查', key: 1},
				{name: '合格', key: 2},
				{name: '不合格', key: 3},
			],
			queryParams: {
				professionId: {},
				builderId: {},
				markType: '',
			},
		};
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},

	methods: {
		onShowDrawer(isShowDrawer = false) {
			let queryParams_ = {
				professionId: this.queryParams.professionId.id ? this.queryParams.professionId.id : '',
				builderId: this.queryParams.builderId.id ? this.queryParams.builderId.id : '',
				markType: this.queryParams.markType.key ? this.queryParams.markType.key : '',
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
				professionId: '',
				builderId: '',
				markType: '',
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
	padding: 0 10rpx;
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
