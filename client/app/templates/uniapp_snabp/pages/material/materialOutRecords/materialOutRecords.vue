<template>
	<view class="uni-material-out-records">
		<!-- 操作区 -->
		<view class="uni-material-out-records-oprator">
			<view style="display: flex; align-items: center" @click="showDrawer">
				<image :src="require('@/static/BIMAppUI/other/screening.png')" style="width: 50rpx; height: 50rpx" />
			</view>
			<view style="display: flex; align-items: center" @click="onAdd">
				<image :src="require('@/static/BIMAppUI/other/add.png')" style="width: 40rpx; height: 40rpx" />
			</view>
		</view>
		<!-- 加载中 -->
		<uniLoading v-show="loading" class="uniLoading" />
		<view class="uni-list-empty" v-if="!(outRecords && outRecords.length > 0)">
			<view class="iconfont icon-empty-a" />
			<view class="uni-list-empty-title">暂无数据</view>
		</view>
		<!-- 展示区 -->
		<view style="padding-top: 80rpx" v-else>
			<view class="uni-material-out-records-item" v-for="item in outRecords" :key="item.id">
				<view class="out-records-item-content first-line">
					<view class="out-records-item-code">编号： {{ item.code || '--' }}</view>
					<view class="out-record-item-operator"><text @click="onView(item)">详情</text></view>
				</view>
				<view class="omit-content">仓库位置： {{ item.partitionName || '--' }}</view>
				<view class="omit-content">出库时间： {{ item.time || '--' }}</view>
				<view class="omit-content">登记人： {{ item.creatorName || '--' }}</view>
				<view class="omit-content">项目：{{ item.projectName || '--' }}</view>
			</view>
			<uni-load-more color="#ffffff" :status="loadStatus"></uni-load-more>
		</view>
		<materialOutRecordDrawer ref="TableOperator" @change="onChange" />
	</view>
</template>

<script>
import materialOutRecordDrawer from './materialOutRecordDrawer.vue';
import {pagination as paginationConfig, tips as tipsConfig} from '@/utils/config.js';
import {checkToken, requestIsSuccess} from '@/utils/util.js';
import {PageState} from '@/utils/enum.js';
import * as apiOutRecord from '@/api/material/outRecord.js';
import moment from 'moment';

export default {
	name: 'materialOutRecords',
	components: {
		materialOutRecordDrawer,
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
			outRecords: [],
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
				this.outRecords = [];
				this.loading = true;
			}

			//页面是否重置
			if (reset) {
				this.pageIndex = 1;
			}
			this.loadStatus = 'loading';
			const response = await apiOutRecord.getList({
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
						time: moment(item.time).format('YYYY-MM-DD'),
						projectName: item.project ? item.project.name : '',
						creatorName: item.creator ? item.creator.name : '',
						partitionName: item.partition ? item.partition.name : '',
					};
				});
				this.outRecords = this.outRecords.concat(array);
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
				url: `./materialOutRecord?id=${item.id}&pageState=${PageState.View}`,
			});
		},
		showDrawer() {
			this.$refs.TableOperator.onShowDrawer(true);
		},

		//添加页面跳转
		onAdd() {
			uni.navigateTo({
				url: `./materialOutRecord?pageState=${PageState.Add}`,
			});
		},
	},
};
</script>

<style lang="scss">
page {
	background-color: #0e2a58;
}
.uni-material-out-records {
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

.uni-material-out-records-oprator {
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

.uni-material-out-records-list {
	flex: 1;
}

.uni-material-out-records-item {
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

.out-records-item-content {
	margin-bottom: 10rpx;
	overflow: hidden;
	white-space: nowrap;
	text-overflow: ellipsis;
	display: flex;
}

.out-records-item-code {
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

.icon-filter-title {
	font-size: 28rpx;
	margin-left: 10rpx;
}
::v-deep .uni-transition {
	left: -310rpx !important;
}

.out-record-item-operator {
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

.icon-scanning {
	font-size: 48rpx;
}
</style>
