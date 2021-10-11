<!-- 安全问题数据查询模态框 -->
<template>
	<uni-drawer ref="showRight" mode="right">
		<view class="uni-table-operator">
			<view class="uni-table-operator-item">
				<view class="uni-form-item uni-column">
					<view class="uni-form-item uni-form-item-title">问题标题</view>
					<input
						class="uni-form-item-input"
						v-model="queryParams.title"
						placeholder="请输入问题标题"
						placeholder-style="color: #c3c1c1;font-size: 28rpx;"
					/>
				</view>
				<view class="uni-form-item uni-column">
					<view class="uni-form-item uni-form-item-title">问题类型</view>
					<uniPicker
						placeholder="请选择问题类型"
						mode="selector"
						class="sm-input-class"
						groupCode="SafeManager.ProblemType"
						v-model="queryParams.typeId"
					/>
				</view>
				<view class="uni-form-item uni-column">
					<view class="uni-form-item uni-form-item-title">检查时间</view>
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
import dictionaryPicker from '@/components/dictionaryPicker.vue';
import uniPicker from '@/components/uniPicker.vue';
import datePicker from '@/components/datePicker.vue';
import moment from 'moment';

export default {
	name: 'securityDrawer',
	components: {dictionaryPicker, datePicker, uniPicker},
	props: {},
	data() {
		return {
			queryParams: {
				title: '', //问题标题
				typeId: '', //问题类型
				startTime: '', //开始时间
				endTime: '', //结束时间
			},
			index: 0,
			markTypeRange: [
				{name: 'A', markType: 1},
				{name: 'B', markType: 2},
				{name: 'C', markType: 3},
			],
		};
	},
	watch: {
		visible: {
			handler: function (val, oldVal) {
				this.iVisible = val;
			},
			immediate: true,
		},
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
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},

	methods: {
		//开始日期
		onStartTimeChange(value) {
			this.queryParams.startTime = value;
		},

		//结束日期
		onEndTimeChange(value) {
			this.queryParams.endTime = value;
		},

		onShowDrawer(isShowDrawer = false) {
			if (isShowDrawer) {
				this.$refs['showRight'].open();
			} else {
				this.$refs['showRight'].close();
				this.queryParams_ = {
					title: this.queryParams.title != '' ? this.queryParams.title : '', //问题标题
					typeId: this.queryParams.typeId != '' ? this.queryParams.typeId.id : '',
					startTime: this.queryParams.startTime
						? moment(this.queryParams.startTime).startOf('d').format('YYYY-MM-DD HH:mm:ss')
						: '',
					endTime: this.queryParams.endTime ? moment(this.queryParams.endTime).endOf('d').format('YYYY-MM-DD HH:mm:ss') : '',
				};
				this.$emit('change', this.queryParams_);
			}
		},

		//条件重置
		onReset() {
			this.queryParams = {
				title: '', //问题标题
				typeId: '', //问题类型
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

.uni-table-operator-buttons {
	display: flex;
}

.uni-table-operator-button {
	width: 100%;
	border-radius: 0;
}
</style>
