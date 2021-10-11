<template>
	<view class="uni-schedule-approvals">
		<!-- 操作区 -->
		<view class="uni-schedule-approvals-oprator">
			<view style="display: flex; align-items: center" @click="showDrawer">
				<image :src="require('@/static/BIMAppUI/other/screening.png')" style="width: 50rpx; height: 50rpx" />
			</view>
		</view>
		<!-- 加载中 -->
		<uniLoading v-show="loading" class="uniLoading" />
		<view class="uni-list-empty" v-if="!(dataSources && dataSources.length > 0)">
			<view class="iconfont icon-empty-a" />
			<view class="uni-list-empty-title">暂无数据</view>
		</view>
		<!-- 展示区 -->
		<view style="padding-top: 80rpx" v-else>
			<block v-for="(item, index) of dataSources" :key="index">
				<view class="uni-schedule-approvals-item">
					<view class="item-content" style="justify-content: space-between">
						<view>{{ item.code }}</view>
						<view style="color: #1890ff; padding: 6rpx; font-size: 28rpx" @tap="onOpen(item.id)">跟踪录入</view>
					</view>
					<view class="item-content">
						状态：
						<view style="color: #42c32d">{{ getMaterialTestingStatus(item.testingStatus) }}</view>
					</view>
					<view class="omit-content">检测类型：{{ getMaterialTestingType(item.testingType) }}</view>
					<view class="omit-content">检测机构：{{ item.testingOrganization.name }}</view>
					<view class="item-content">
						{{ item.userName ? '送检人：' + item.userName : '' }}
					</view>
					<view class="omit-content">报告时间：{{ item.receptionTime }}</view>
				</view>
			</block>
			<uni-load-more color="#ffffff" v-show="loadNoMore" :status="loadStatus"></uni-load-more>
		</view>
		<materialAcceptanceDrawer ref="TableOperator" @change="onChange" />
	</view>
</template>

<script>
import materialAcceptanceDrawer from './materialAcceptanceDrawer.vue';
import {requestIsSuccess, getMaterialTestingType, getMaterialTestingStatus} from '@/utils/util.js';
import {MaterialTestingStatus} from '@/utils/enum.js';
import {pagination as paginationConfig} from '@/utils/config.js';
import moment from 'moment';

import * as apiMaterialAcceptance from '@/api/material/materialAcceptance.js';

export default {
	components: {materialAcceptanceDrawer},
	data() {
		return {
			pageIndex: 1,
			loading: true,
			queryParams: {
				testingStatus: MaterialTestingStatus.Approved,
				keyword: '',
				testingType: '',
				startTime: '',
				endTime: '',
				maxResultCount: paginationConfig.defaultPageSize,
			},
			dataSources: [],
			skipCount: 0, // 跳过数
			delete_: [], // 删除组
			canLoaded: 1, // 是否可以加载
			loadStatus: 'more',
			loadNoMore: true,
		};
	},

	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
		this.refresh(true, true);
	},

	methods: {
		getMaterialTestingType,
		getMaterialTestingStatus,
		async refresh(reset = true, isClearData = false) {
			//是否清空数据源
			if (isClearData) {
				this.dataSources = [];
				this.loading = true;
			}
			//页面是否重置
			if (reset) {
				this.pageIndex = 1;
			}
			const response = await apiMaterialAcceptance.getList({
				skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
				...this.queryParams,
			});
			console.log(response);
			if (requestIsSuccess(response) && response.data && response.data.items) {
				if (response.data.items.length < 10) {
					this.loadStatus = 'noMore';
				} else {
					this.loadStatus = 'more';
				}
				// 获取数据列表
				response.data.items.forEach(item => {
					item.receptionTime = moment(item.receptionTime).format('YYYY-MM-DD');
					this.dataSources.push(item);
				});
				this.loading = false;
			}
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

		//领料单填报页面跳转
		onOpen(id) {
			uni.navigateTo({
				url: `./materialAcceptanceOfUpdate?id=${id}`,
			});
		},
	},
	onReachBottom() {
		if (this.loadStatus === 'more') {
			this.loadNoMore = true;
			this.pageIndex += 1;
			this.refresh(false);
		}
	},
};
</script>

<style lang="scss">
page {
	background-color: #0e2a58;
}
.approvals-tian {
	color: #ffffff;
	background-color: #1890ff;
	text-align: center;
	border-radius: 20rpx;
	padding: 10rpx 20rpx;
}
.uni-schedule-approvals {
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

.uni-schedule-approvals-oprator {
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

.uni-schedule-approvals-item {
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
	display: flex;
	align-items: center;
}

.approvals-item-code {
	color: #ffffff;
	padding: 5rpx 20rpx;
	font-size: 36rpx;
	background-color: #1890ff;
	border-radius: 15rpx;
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

.Trash {
	width: 100%;
	text-align: end;
}
.Trash uni-icons {
	padding: 10rpx;
}
</style>
