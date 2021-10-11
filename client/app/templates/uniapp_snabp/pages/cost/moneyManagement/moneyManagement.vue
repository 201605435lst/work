<template>
	<view class="uni-money-management">
		<!-- 操作区 -->
		<view class="uni-money-management-oprator">
			<view style="display: flex; align-items: center" @click="showDrawer">
				<image :src="require('@/static/BIMAppUI/other/screening.png')" style="width: 50rpx; height: 50rpx" />
			</view>
			<view style="display: flex; align-items: center" @click="onAdd">
				<image :src="require('@/static/BIMAppUI/other/add.png')" style="width: 40rpx; height: 40rpx" />
			</view>
		</view>
		<!-- 加载中 -->
		<uniLoading v-show="loading" class="uniLoading" />
		<view class="uni-list-empty" v-if="!(moneyList && moneyList.length > 0)">
			<view class="iconfont icon-empty-a" />
			<view class="uni-list-empty-title">暂无数据</view>
		</view>
		<!-- 展示区 -->
		<view style="padding-top: 80rpx" v-else>
			<view class="uni-money-management-item" v-for="item in moneyList" :key="item.id" @click="onView(item.id)">
				<view class="item-content first-line">
					<view class="money-management-item-code">资金类型： {{ item.type }}</view>
					<view class="item-operator">
						<text @click.stop="onEdit(item.id)">编辑</text>
						<view class="divider"></view>
						<text style="color: #ff0000" @click.stop="remove(item.id)">删除</text>
					</view>
				</view>
				<view class="omit-content">收款单位： {{ item.payee }}</view>
				<view class="omit-content">时间： {{ item.date }}</view>
				<view class="omit-content">应收：{{ item.receivable }}（万元）</view>
				<view class="omit-content">已收：{{ item.received }}（万元）</view>
				<view class="omit-content">应付：{{ item.due }}（万元）</view>
				<view class="omit-content">已付：{{ item.paid }}（万元）</view>
			</view>
			<uni-load-more color="#ffffff" :status="loadStatus"></uni-load-more>
		</view>

		<moneyDrawer ref="TableOperator" @change="onChange" />
	</view>
</template>

<script>
import moneyDrawer from './moneyDrawer.vue';
import {pagination as paginationConfig, tips as tipsConfig} from '@/utils/config.js';
import {checkToken, requestIsSuccess} from '@/utils/util.js';
import {PageState} from '@/utils/enum.js';
import * as apiMoney from '@/api/costManagement/money.js';
import moment from 'moment';

export default {
	name: 'moneyManagement',
	components: {moneyDrawer},
	data() {
		return {
			pageIndex: 1,
			queryParams: {
				typeId: '', //id
				payeeId: '', //付款单位id
				startTime: '',
				endTime: '',
				maxResultCount: paginationConfig.defaultPageSize,
			},
			moneyList: [],
			loading: true,
			loadStatus: 'more',
		};
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
		this.refresh(true, true);
	},

	onReachBottom() {
		if (this.loadStatus === 'more') {
			this.pageIndex += 1;
			this.refresh(false);
		}
	},

	methods: {
		//数据获取
		async refresh(reset = true, isClearData = false) {
			//是否清空数据源
			if (isClearData) {
				this.moneyList = [];
				this.loading = true;
			}

			//页面是否重置
			if (reset) {
				this.pageIndex = 1;
			}
			this.loadStatus = 'loading';
			const response = await apiMoney.getList({
				skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
				...this.queryParams,
			});
			if (requestIsSuccess(response) && response.data && response.data.items) {
				if (response.data.items.length < 10) {
					this.loadStatus = 'noMore';
				} else {
					this.loadStatus = 'more';
				}
				let array = response.data.items.map(item => {
					return {
						...item,
						date: moment(item.date).format('YYYY-MM-DD'),
						type: item.type.name,
						payee: item.payee.name,
						unReceived: item.receivable - item.received,
						unPaid: item.due - item.paid,
					};
				});
				this.moneyList = this.moneyList.concat(array);
			}
			this.loading = false;
		},

		showDrawer() {
			this.$refs.TableOperator.onShowDrawer(true);
		},

		onChange(queryParams) {
			this.queryParams = {
				...this.queryParams,
				...queryParams,
			};
			this.refresh(true, true);
		},

		//添加页面跳转
		onAdd() {
			uni.navigateTo({
				url: `../../../pages/cost/moneyManagement/money?pageState=${PageState.Add}`,
			});
		},

		//编辑页面
		onEdit(id) {
			uni.navigateTo({
				url: `../../../pages/cost/moneyManagement/money?pageState=${PageState.Edit}&id=${id}`,
			});
		},

		//编辑页面
		onView(id) {
			uni.navigateTo({
				url: `../../../pages/cost/moneyManagement/money?pageState=${PageState.View}&id=${id}`,
			});
		},

		// 删除
		async remove(id) {
			if (id) {
				uni.showModal({
					cancelColor: '#f7bd8f',
					confirmColor: '#98d98e',
					content: '确定要删除该条数据吗？',
					success: async res => {
						if (res.confirm) {
							let response = await apiMoney.remove([id]);
							if (requestIsSuccess(response)) {
								uni.showToast({
									icon: 'none',
									title: '删除成功',
									duration: 1000,
								});
								this.refresh(true, true);
							}
						}
					},
				});
			} else {
				return;
			}
		},
	},
};
</script>

<style lang="scss">
page {
	background-color: #0e2a58;
}
.uni-money-management {
	display: flex;
	flex-direction: column;
	height: 100%;
	.uni-money-management-oprator {
		position: fixed;
		left: 0;
		right: 0;
		top: 0;
		display: flex;
		justify-content: space-between;
		align-items: center;
		background-color: #006186;
		height: 80rpx;
		color: #ffffff;
		padding: 0 30rpx;
		border-radius: 15rpx;

		.icon-filter {
			display: flex;
			align-items: center;
			font-size: 50rpx;
		}

		.icon-add {
			font-size: 40rpx;
		}

		.icon-filter-title {
			font-size: 28rpx;
			margin-left: 10rpx;
		}
	}

	.divider {
		background: #dfdfdf;
		height: 24rpx;
		margin: 0 20rpx;
		padding: 1rpx;
	}

	.uni-list-empty {
		flex: 1;
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		font-size: 28rpx;
		color: #c3c1c1;
		.icon-empty-a {
			font-size: 120rpx;
			margin-bottom: 20rpx;
		}
	}

	.uni-money-management-item {
		border-bottom: solid 1rpx #e4e4e4;
		font-size: 26rpx;
		color: #8d8b8b;
		background-color: #f9f9f9;
		margin-top: 25rpx;
		padding: 20rpx 30rpx;
		border-radius: 20rpx;

		.item-content {
			margin-bottom: 10rpx;
			overflow: hidden;
			white-space: nowrap;
			text-overflow: ellipsis;
			display: flex;
			.money-management-item-code {
				color: #000000;
				flex: 1;
				overflow: hidden;
				white-space: nowrap;
				text-overflow: ellipsis;
			}

			.item-operator {
				width: 160rpx;
				display: flex;
				justify-content: flex-end;
				align-items: center;
				color: #1890ff;
				font-size: 28rpx;
			}
		}

		.first-line {
			display: flex;
			justify-content: space-between;
		}
	}
}
</style>
