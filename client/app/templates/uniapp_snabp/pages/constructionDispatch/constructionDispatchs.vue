<template>
	<!-- 派工审批与日志审批模块 -->
	<view class="construction-dispatchs">
		<!-- 操作区 -->
		<view class="construction-dispatchs-oprator">
			<view class="construction-dispatchs-oprator-view">
				<view :class="waitApprove ? 'approve-selected' : ''" @tap="onTap(true)">待我审批</view>
				<view :class="waitApprove ? '' : 'approve-selected'" @tap="onTap(false)">我审批的</view>
			</view>
			<view style="display: flex; align-items: center" @click="showDrawer">
				<image :src="require('@/static/BIMAppUI/other/screening.png')" style="width: 50rpx; height: 50rpx" />
			</view>
		</view>
		<!-- 加载中 -->
		<uniLoading v-show="loading" class="uniLoading" />
		<view class="uni-list-empty" v-if="!loadingSwitch && !(listData && listData.length > 0)">
			<view class="iconfont icon-empty-a" />
			<view class="uni-list-empty-title">暂无数据</view>
		</view>
		<!-- 展示区 -->
		<view style="padding-top: 80rpx" v-else>
			<block v-for="(item, index) of listData" :key="index">
				<view class="construction-dispatchs-item">
					<view class="item-content first-line" style="display: flex">
						<view class="item-content">编号：{{ item.code }}</view>
						<view class="item-content">
							<view class="details">
								<text v-if="item.state == ApprovalState.OnReview" @tap="goto(item.id, PageState.Edit)">审批</text>
								<text v-if="item.state == ApprovalState.OnReview">|</text>
								<text @tap="goto(item.id, PageState.View)">详情</text>
							</view>
						</view>
					</view>
					<view class="item-content" style="display: flex">
						状态：
						<view :style="{color: getStateColor(item.state)}">{{ getDiarysStateTitle(item.state) }}</view>
					</view>
					<block v-if="dailyApprove">
						<view class="item-content">填报日期：{{ item.date }}</view>
						<view class="item-content">创建人：{{ item.informant }}</view>
						<view class="item-content">施工人数：{{ item.builderCount }}</view>
					</block>
					<block v-else>
						<view class="item-content">派工单名称：{{ item.name }}</view>
						<view class="item-content">派工专业：{{ item.profession }}</view>
						<view class="item-content">派工时间：{{ item.time }}</view>
						<view class="item-content">提交人：{{ item.creator }}</view>
						<view class="item-content">承包商：{{ item.contractor }}</view>
					</block>
				</view>
			</block>
			<uni-load-more color="#ffffff" v-show="loadNoMore" :status="loadStatus"></uni-load-more>
		</view>

		<!-- 筛选组件 -->
		<constructionDailyDrawer v-if="dailyApprove" ref="TableOperator" @change="onChange" />
		<constructionDispatchDrawer v-else ref="TableOperator" @change="onChange" />
	</view>
</template>

<script>
import constructionDispatchDrawer from './constructionDispatchDrawer.vue';
import constructionDailyDrawer from '../constructionDaily/constructionDailyDrawer.vue';
import {requestIsSuccess, getDiarysStateTitle} from '@/utils/util.js';
import {pagination as paginationConfig} from '@/utils/config.js';
import {PageState, interfaceState, ApprovalState} from '@/utils/enum.js';
import * as apiDispatch from '@/api/construction/dispatch.js';
import * as apiDaily from '@/api/construction/daily.js';
import moment from 'moment';

export default {
	components: {constructionDispatchDrawer, constructionDailyDrawer},
	data() {
		return {
			interfaceState,
			ApprovalState,
			dailyApprove: '', //是否是日志审批
			pageIndex: 1,
			PageState,
			loading: true,
			listData: [],
			loadStatus: 'more',
			loadNoMore: true,
			loadingSwitch: false,
			queryParams: {
				markType: '',
				approval: true,
				waiting: true,
				maxResultCount: paginationConfig.defaultPageSize,
			},
			waitApprove: true, //顶部选择的待我审批选中状态
		};
	},
	onLoad(option) {
		let dailyApprove = JSON.parse(option.dailyApprove);
		dailyApprove ? uni.setNavigationBarTitle({title: '日志审批'}) : uni.setNavigationBarTitle({title: '派工审批'});
		this.dailyApprove = dailyApprove;
	},

	//页面显示
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
		this.refresh(true, true);
	},

	//页面触底操作
	onReachBottom() {
		if (this.loadStatus === 'more') {
			this.loadNoMore = true;
			this.pageIndex += 1;
			this.refresh(false);
		}
	},

	methods: {
		getDiarysStateTitle,

		//列表数据查询refresh
		async refresh(reset = true, isClearData = false) {
			//是否清空数据源
			if (isClearData) {
				this.listData = [];
				this.loading = true;
			}

			//页面是否重置
			if (reset) {
				this.pageIndex = 1;
			}
			this.loadStatus = 'loading';

			if (this.dailyApprove) {
				//日志审批数据查询
				const response = await apiDaily.getList({
					skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
					...this.queryParams,
				});
				if (requestIsSuccess(response) && response.data && response.data.items) {
					if (response.data.items.length < 10) {
						this.loadStatus = 'noMore';
					} else {
						this.loadStatus = 'more';
					}
					let listData = response.data.items.map(item => {
						return {
							id: item.id,
							code: item.code,
							state: item.status,
							date: moment(item.date).format('YYYY-MM-DD'),
							informant: item.informant ? item.informant.userName : '',
							builderCount: item.builderCount,
						};
					});
					this.listData = this.listData.concat(listData);
					this.loading = false;
					this.loadingSwitch = false;
				}
			} else {
				//派工审批数据查询
				const response = await apiDispatch.getList({
					skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
					...this.queryParams,
				});
				if (requestIsSuccess(response) && response.data && response.data.items.length >= 0) {
					if (response.data.items.length < 10) {
						this.loadStatus = 'noMore';
					} else {
						this.loadStatus = 'more';
					}
					let listData = response.data.items.map(item => {
						return {
							id: item.id,
							code: item.code,
							state: item.state,
							name: item.name,
							time: moment(item.time).format('YYYY-MM-DD'),
							profession: item.profession,
							creator: item.creator ? item.creator.name : '',
							contractor: item.contractor ? item.contractor.name : '',
						};
					});
					this.listData = this.listData.concat(listData);
					this.loading = false;
					this.loadingSwitch = false;
				}
			}
		},

		//状态颜色获取
		getStateColor(state) {
			let color = '';
			switch (state) {
				case ApprovalState.ToSubmit:
					color = '#ff007f';
					break;
				case ApprovalState.OnReview:
					color = '#ff55ff';
					break;
				case ApprovalState.Pass:
					color = '#42c32d';
					break;
				case ApprovalState.UnPass:
					color = '#ff0000';
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

		//页面跳转
		goto(id, pageState) {
			console.log(id);
			uni.navigateTo({
				url: `${
					this.dailyApprove
						? `../constructionDaily/constructionDailyApprove?id=${id}&pageState=${pageState}`
						: `./constructionDispatch?id=${id}&pageState=${pageState}`
				}`,
			});
		},
		onTap(state) {
			//点击后，请求数据时不能再点击
			if (!this.loadingSwitch) {
				this.waitApprove = state ? true : false;
				this.queryParams.waiting = state ? true : false;
				this.listData = [];
				this.loadingSwitch = true;
				this.refresh();
			}
		},
	},
};
</script>

<style lang="scss">
page {
	background-color: #0e2a58;
}
.construction-dispatchs {
	.construction-dispatchs-oprator {
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
		font-size: 28rpx;
		.construction-dispatchs-oprator-view {
			height: 100%;
			display: flex;
			view {
				margin: 0 20rpx;
				height: 100%;
				line-height: 80rpx;
				color: #adadad;
			}
			.approve-selected {
				border-bottom: 4rpx solid #37a3ff;
				color: #ffffff;
			}
		}
		.icon-filter {
			display: flex;
			align-items: center;
			font-size: 50rpx;
			.icon-filter-title {
				font-size: 28rpx;
				margin-left: 10rpx;
			}
		}
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
		.icon-empty-a {
			font-size: 120rpx;
			margin-bottom: 20rpx;
		}
	}
	.approvals-tian {
		color: #ffffff;
		background-color: #1890ff;
		text-align: center;
		border-radius: 20rpx;
		padding: 10rpx 20rpx;
	}
	.construction-dispatchs {
		display: flex;
		flex-direction: column;
		height: 100%;
	}

	.construction-dispatchs-item {
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
		align-items: center;
		justify-content: space-between;
	}
	.first-line > view {
		margin: 0 !important;
	}

	.item-content {
		margin-bottom: 10rpx;
		overflow: hidden;
		white-space: nowrap;
		text-overflow: ellipsis;
	}
	.approvals-item-code {
		color: #ffffff;
		padding: 5rpx 20rpx;
		font-size: 36rpx;
		background-color: #1890ff;
		border-radius: 15rpx;
	}
	.details {
		display: flex;
		align-items: center;
		margin-left: 10rpx;
	}
	.details > text {
		padding: 12rpx;
	}
	.details > text:nth-child(1) {
		color: #1890ff;
	}
	.icon-add {
		font-size: 40rpx;
	}
}
</style>
