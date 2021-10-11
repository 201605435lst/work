<!-- 查询模态框 -->
<template>
	<uni-drawer ref="showRight" mode="right">
		<view class="uni-table-operator">
			<view class="uni-table-operator-item">
				<view class="uni-form-item uni-column">
					<view class="uni-form-item uni-form-item-title">施工队</view>
					<input
						placeholder="请输入施工队"
						placeholder-style="color: #c3c1c1;font-size: 28rpx;"
						class="uni-form-item-input"
						v-model="queryParams.constructionTeam"
					/>
				</view>
				<view class="uni-form-item uni-column">
					<view class="uni-form-item uni-form-item-title">施工区段</view>
					<treeSelect
						class="sm-input-class"
						placeholder="请选择施工区段"
						title="施工区段"
						action="/api/app/section/getTreeList"
						v-model="queryParams.section"
					/>
				</view>
				<view class="uni-form-item uni-column">
					<view class="uni-form-item uni-form-item-title">领料状态</view>
					<uniPicker
						mode="selector"
						placeholder="请选择领料状态"
						class="sm-input-class"
						:range="stateList"
						range-key="name"
						v-model="queryParams.state"
					/>
				</view>
				<view class="uni-form-item uni-column">
					<view class="uni-form-item uni-form-item-title">日期选择</view>
					<view class="uni-time-picker">
						<datePicker placeholder="开始时间" style="flex: 1" mode="date" v-model="queryParams.startTime" />
						<view style="width: 30rpx; text-align: center">-</view>
						<datePicker placeholder="结束时间" dateRange="end" style="flex: 1" mode="date" v-model="queryParams.endTime" />
					</view>
				</view>
			</view>
			<view class="uni-table-operator-buttons">
				<button class="uni-table-operator-button" @tap="onReset" size="mini">重置</button>
				<button
					class="uni-table-operator-button"
					@tap="onShowDrawer(false)"
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
import treeSelect from '@/components/treeSelect.vue';
import datePicker from '@/components/datePicker.vue';
import * as apiSection from '@/api/construction/section.js';
import moment from 'moment';
export default {
	name: 'qualityDrawer',
	components: {uniPicker, datePicker, treeSelect},
	props: {},
	data() {
		return {
			conList: [],
			stateList: [
				{name: '待提交', key: 1},
				{name: '待审核', key: 2},
				{name: '已通过', key: 3},
			],
			queryParams: {
				constructionTeam: '',
				section: {},
				state: '',
				startTime: '',
				endTime: '',
			},
		};
	},
	mounted() {
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
			let Qstate = this.queryParams.state;
			let queryParams = {
				constructionTeam: this.queryParams.constructionTeam,
				sectionId: this.queryParams.section.id ? this.queryParams.section.id : '',
				state: Qstate && Qstate.key ? Qstate.key : '',
				startTime: this.queryParams.startTime ? moment(this.queryParams.startTime).startOf('d').format('YYYY-MM-DD HH:mm:ss') : '',
				endTime: this.queryParams.endTime ? moment(this.queryParams.endTime).endOf('d').format('YYYY-MM-DD HH:mm:ss') : '',
			};
			if (isShowDrawer) {
				this.$refs['showRight'].open();
			} else {
				this.$refs['showRight'].close();
				this.$emit('change', queryParams);
			}
		},

		//条件重置
		onReset() {
			this.queryParams = {
				constructionTeam: '',
				section: {},
				state: '',
				startTime: '',
				endTime: '',
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
