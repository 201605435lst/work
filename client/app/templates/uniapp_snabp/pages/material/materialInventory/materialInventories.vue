<template>
	<view class="uni-inventories">
		<!-- 操作区 -->
		<view class="uni-inventories-oprator">
			<view style="display: flex; align-items: center" @click="showDrawer">
				<image :src="require('@/static/BIMAppUI/other/screening.png')" style="width: 50rpx; height: 50rpx" />
			</view>
		</view>
		<!-- 加载中 -->
		<uniLoading v-show="loading" class="uniLoading" />
		<view class="uni-list-empty" v-if="!(inventories && inventories.length > 0)">
			<view class="iconfont icon-empty-a" />
			<view class="uni-list-empty-title">暂无数据</view>
		</view>
		<!-- 展示区 -->
		<view style="padding-top: 80rpx" v-else>
			<view class="uni-inventories-item" v-for="item in inventories" :key="item.id">
				<view class="item-content first-line">
					<view class="inventories-item-code">材料名称： {{ item.materialName || '--' }}</view>
					<view class="inventories-item-operator"><text @click="onView(item)">详情</text></view>
				</view>
				<view class="omit-content">材料规格： {{ item.materialSpec || '--' }}</view>
				<view class="omit-content">登记时间： {{ item.entryTime || '--' }}</view>
				<view class="omit-content">库存数量：{{ item.amount || '--' }}</view>
				<view class="omit-content">库存地点： {{ item.partitionName || '--' }}</view>
				<view class="omit-content">供应商：{{ item.supplierName || '--' }}</view>
			</view>
			<uni-load-more color="#ffffff" :status="loadStatus"></uni-load-more>
		</view>
		<materialInventorieDrawer id="materialInventorieDrawer" ref="TableOperator" @change="onChange" />
	</view>
</template>

<script>
import materialInventorieDrawer from './materialInventorieDrawer.vue';
import {pagination as paginationConfig, tips as tipsConfig} from '@/utils/config.js';
import {checkToken, requestIsSuccess} from '@/utils/util.js';
import {PageState} from '@/utils/enum.js';
import * as apiMaterialInventory from '@/api/material/inventory.js';
import moment from 'moment';

export default {
	name: 'materialInventories',
	components: {
		materialInventorieDrawer,
	},
	data() {
		return {
			loading: false,
			pageIndex: 1,
			queryParams: {
				sTime: '',
				eTime: '',
				keyword: '',
				partitionId: '',
				maxResultCount: paginationConfig.defaultPageSize,
			},
			inventories: [],
			loadStatus: 'more',
		};
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
		this.refresh(true, true);
	},

	onLoad(option) {
		if (option.queryParams) {
			let _queryParams = JSON.parse(option.queryParams);
			this.queryParams = {
				..._queryParams,
				maxResultCount: paginationConfig.defaultPageSize,
			};
		}
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
				this.inventories = [];
				this.loading = true;
			}

			//页面是否重置
			if (reset) {
				this.pageIndex = 1;
			}
			this.loadStatus = 'loading';
			const response = await apiMaterialInventory.getList({
				skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
				...this.queryParams,
			});
			if (requestIsSuccess(response) && response.data && response.data.items.length >= 0) {
				if (response.data.items.length < 10) {
					this.loadStatus = 'noMore';
				} else {
					this.loadStatus = 'more';
				}
				let array = response.data.items.map(item => {
					return {
						...item,
						entryTime: moment(item.entryTime).format('YYYY-MM-DD'),
						materialName: item.material ? item.material.name : '',
						materialSpec: item.material ? item.material.spec : '',
						partitionName: item.partition ? item.partition.name : '',
						supplierName: item.supplier ? item.supplier.name : '',
					};
				});
				this.inventories = this.inventories.concat(array);
			}
			this.loading = false;
		},

		onChange(queryParams) {
			this.queryParams = {
				...this.queryParams,
				...queryParams,
			};
			this.refresh(true, true);
		},

		//详情页面
		onView(item) {
			uni.navigateTo({
				url: `./materialInventory?id=${item.id}`,
			});
		},
		showDrawer() {
			this.$refs.TableOperator.onShowDrawer(true);
		},
	},
};
</script>

<style lang="scss">
page {
	background-color: #0e2a58;
}
.uni-inventories {
	display: flex;
	flex-direction: column;
	height: 100%;
}

.uni-list-empty {
	flex: 1;
	display: flex;
	flex-direction: column;
	align-items: center;
	justify-content: center;
	font-size: 28rpx;
	color: #c3c1c1;
	height: 100vh;
}

.icon-empty-a {
	font-size: 120rpx;
	margin-bottom: 20rpx;
}

.uni-inventories-oprator {
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
}

.uni-inventories-list {
	flex: 1;
}

.uni-inventories-item {
	padding: 20rpx;
	border-bottom: solid 1rpx #e4e4e4;
	font-size: 26rpx;
	color: #8d8b8b;
	background-color: #f9f9f9;
	margin-top: 25rpx;
	padding: 20rpx 30rpx;
	border-radius: 20rpx;
}

.first-line {
	display: flex;
	justify-content: space-between;
}

.item-content {
	margin-bottom: 10rpx;
	overflow: hidden;
	white-space: nowrap;
	text-overflow: ellipsis;
	display: flex;
}

.inventories-item-code {
	color: #000000;
	flex: 1;
	overflow: hidden;
	white-space: nowrap;
	text-overflow: ellipsis;
}

.inventories-item-time {
	width: 160rpx;
	text-align: right;
	margin-left: 10rpx;
}

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
.icon-filter {
	display: flex;
	align-items: center;
	font-size: 50rpx;
}

.icon-filter-title {
	font-size: 28rpx;
	margin-left: 10rpx;
}
.uni-inventories #materialInventorieDrawer ::v-deep .uni-transition {
	left: -310rpx !important;
}

.inventories-item-operator {
	width: 160rpx;
	display: flex;
	justify-content: flex-end;
	align-items: center;
	color: #1890ff;
	font-size: 28rpx;
}

.divider {
	background: #dfdfdf;
	height: 24rpx;
	margin: 0 20rpx;
	padding: 1rpx;
}
</style>
