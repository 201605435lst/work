<template>
	<view class="uni-schedule-approvals">
		<!-- 操作区 -->
		<view class="uni-schedule-approvals-oprator">
			<view style="display: flex; align-items: center" @click="showDrawer">
				<image :src="require('@/static/BIMAppUI/other/screening.png')" style="width: 50rpx; height: 50rpx" />
			</view>
			<view style="display: flex; align-items: center" @tap="onOpen(PageState.Add)">
				<image :src="require('@/static/BIMAppUI/other/add.png')" style="width: 40rpx; height: 40rpx" />
			</view>
		</view>
		<!-- 加载中 -->
		<uniLoading v-show="loading" class="uniLoading" />
		<view class="uni-list-empty" v-if="!(PickingDatas && PickingDatas.length > 0)">
			<view class="iconfont icon-empty-a" />
			<view class="uni-list-empty-title">暂无数据</view>
		</view>
		<!-- 展示区 -->
		<view style="padding-top: 80rpx" v-else>
			<block v-for="(item, index) of PickingDatas" :key="index">
				<block v-if="delete_.indexOf(index) == -1 ? 1 : 0">
					<view class="uni-schedule-approvals-item" @tap="onOpen(item.state == 1 ? PageState.Edit : PageState.View, item.id)">
						<view class="item-content first-line">
							<view class="item-content">
								状态：
								<view :style="{color: getStateColor(item.state)}">{{ getMaterial(item.state) }}</view>
							</view>
							<view class="approvals-item-time">{{ item.time }}</view>
						</view>
						<view class="omit-content">施工队：{{ item.constructionTeam }}</view>
						<view class="omit-content">施工区段：{{ item.section.name }}</view>
						<view class="omit-content" style="position: relative">
							{{ item.userName ? '领料人：' + item.userName : '' }}
							<view class="Trash" @tap.stop="deleteRow(item.id, index)">
								<uni-icons v-if="item.state == 1" type="trash" size="18" color="#ee0000"></uni-icons>
							</view>
						</view>
					</view>
				</block>
			</block>
			<uni-load-more color="#ffffff" v-show="loadNoMore" :status="loadStatus"></uni-load-more>
		</view>
		<materialOfBillDrawer ref="TableOperator" @change="onChange" />
	</view>
</template>

<script>
import materialOfBillDrawer from './materialOfBillDrawer.vue';
import {getMaterial, requestIsSuccess} from '@/utils/util.js';
import {materialOfBillState, PageState} from '@/utils/enum.js';
import {pagination as paginationConfig} from '@/utils/config.js';
import moment from 'moment';

import * as apiMaterialOfBill from '@/api/material/materialOfBill.js';

export default {
	name: 'materialOfBills',
	components: {materialOfBillDrawer},
	data() {
		return {
			PageState,
			pageIndex: 1,
			loading: true,
			queryParams: {
				constructionTeam: '',
				sectionId: '',
				state: '',
				startTime: '',
				endTime: '',
				maxResultCount: paginationConfig.defaultPageSize,
			},
			materialOfBillState,
			PickingDatas: [],
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
		getMaterial,
		async refresh(reset = true, isClearData = false) {
			//是否清空数据源
			if (isClearData) {
				this.PickingDatas = [];
				this.loading = true;
			}
			//页面是否重置
			if (reset) {
				this.pageIndex = 1;
			}
			const response = await apiMaterialOfBill.getList({
				skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
				...this.queryParams,
			});
			if (requestIsSuccess(response) && response.data && response.data.items.length >= 0) {
				if (response.data.items.length < 10) {
					this.loadStatus = 'noMore';
				} else {
					this.loadStatus = 'more';
				}
				// 获取数据列表
				response.data.items.forEach(e => {
					e.time = moment(e.time).format('YYYY-MM-DD');
					this.PickingDatas.push(e);
				});
				this.loading = false;
			}
		},

		getStateColor(state) {
			let color = '';
			switch (state) {
				case materialOfBillState.Passed:
					color = '#42c32d';
					break;
				case materialOfBillState.Checking:
					color = '#ff55ff';
					break;
				case materialOfBillState.UnCheck:
					color = '#ff5500';
					break;
			}

			return color;
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
		onOpen(pageState, id) {
			uni.navigateTo({
				url: `./materialOfBill?pageState=${pageState}&id=${id}`,
			});
		},
		// 待提交删除
		async deleteRow(e, i) {
			await apiMaterialOfBill.remove(e);
			this.delete_.push(i);
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

<style>
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
	overflow: hidden;
	white-space: nowrap;
	text-overflow: ellipsis;
	display: flex;
}

.approvals-item-code {
	color: #ffffff;
	padding: 5rpx 20rpx;
	font-size: 36rpx;
	background-color: #1890ff;
	border-radius: 15rpx;
}

.approvals-item-time {
	display: flex;
	align-items: center;
	width: 160rpx;
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

.Trash {
	position: absolute;
	right: 0;
}
.Trash uni-icons {
	padding: 10rpx;
}
::v-deep .uni-transition {
	left: -310rpx !important;
}
</style>
