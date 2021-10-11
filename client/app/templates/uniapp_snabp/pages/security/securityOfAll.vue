<!-- 安全问题库管理页面 -->
<template>
	<view id="uni-safe-problems">
		<!-- 操作区 -->
		<view class="uni-safe-problems-oprator">
			<view style="display: flex; align-items: center" @tap="showDrawer">
				<image :src="require('@/static/BIMAppUI/other/screening.png')" style="width: 50rpx; height: 50rpx" />
			</view>
			<view
				style="display: flex; align-items: center"
				v-if="filterType == SafeQualityFilterType.All"
				class="quality-mark"
				@click="onAdd"
			>
				<image :src="require('@/static/BIMAppUI/other/add.png')" style="width: 40rpx; height: 40rpx" />
			</view>
		</view>
		<!-- 加载中 -->
		<uniLoading v-show="loading" class="uniLoading" />
		<view class="uni-list-empty" v-if="!(safeProblems && safeProblems.length > 0)">
			<view class="iconfont icon-empty-a" />
			<view class="uni-list-empty-title">暂无数据</view>
		</view>
		<!-- 展示区 -->
		<view style="padding-top: 80rpx" v-else>
			<view class="uni-safe-problems-item" v-for="item in safeProblems" :key="item.id">
				<view class="item-content first-line">
					<view class="safe-problems-item-title">标题：{{ item.title }}</view>
					<view class="securities-item-operator">
						<view
							style="display: flex; align-items: center"
							v-if="filterType != SafeQualityFilterType.All || filterType != SafeQualityFilterType.CopyMine"
						>
							<text
								v-if="item.state == SafeQualityProblemState.WaitingImprove && filterType == SafeQualityFilterType.MyChecked"
								@click="onEdit(item)"
							>
								编辑
							</text>
							<text v-if="filterType == SafeQualityFilterType.MyWaitingImprove" @click="onImprove(item)">整改</text>
							<text v-if="filterType == SafeQualityFilterType.MyWaitingVerify" @click="onVerify(item)">验证</text>
							<view
								v-if="
									(item.state == SafeQualityProblemState.WaitingImprove &&
										filterType == SafeQualityFilterType.MyWaitingImprove) ||
									(item.state == SafeQualityProblemState.WaitingVerify &&
										filterType == SafeQualityFilterType.MyWaitingVerify) ||
									(item.state == SafeQualityProblemState.WaitingImprove && filterType == SafeQualityFilterType.MyChecked)
								"
								class="divider"
							></view>
						</view>

						<text class="securities-item-view" @click="onView(item.id)">详情</text>
					</view>
				</view>
				<view class="item-content">
					状态：
					<view :style="{color: getStateColor(item.state)}">{{ getProblemStateTitle(item.state) }}</view>
				</view>
				<view class="omit-content">类型：{{ item.type }}</view>
				<view class="omit-content">检查时间：{{ item.checkTime }}</view>
				<view class="omit-content">限定时间：{{ item.limitTime }}</view>
				<view class="omit-content">检查人：{{ item.checker }}</view>
				<view class="omit-content">责任人：{{ item.responsibleUser }}</view>
				<view class="omit-content">责任单位：{{ item.responsibleUnit }}</view>
			</view>
			<uni-load-more color="#ffffff" :status="loadStatus"></uni-load-more>
		</view>
		<securityDrawer ref="TableOperator" @change="onChange" />
	</view>
</template>

<script>
import securityDrawer from './securityDrawer.vue';
import {pagination as paginationConfig, tips as tipsConfig} from '@/utils/config.js';
import {checkToken, requestIsSuccess, getProblemStateTitle, setNavigationBarTitle} from '@/utils/util.js';
import {PageState, SafeQualityProblemState, SafeQualityFilterType} from '@/utils/enum.js';
import * as apiSafeProblem from '@/api/safe/safeProblem.js';
import moment from 'moment';

export default {
	name: 'safe-problems',
	components: {
		securityDrawer,
	},
	data() {
		return {
			SafeQualityProblemState,
			SafeQualityFilterType,
			loading: false,
			pageIndex: 1,
			queryParams: {
				title: '', //问题标题
				typeId: '', //问题类型
				startTime: '', //开始时间
				endTime: '', //结束时间
				maxResultCount: paginationConfig.defaultPageSize,
			},
			safeProblems: [],
			loadStatus: 'more',
			filterType: 0,
		};
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
		this.refresh(true, true);
	},

	onLoad(option) {
		setNavigationBarTitle(option.type);
		this.filterType = option.type;
	},

	onReachBottom() {
		if (this.loadStatus === 'more') {
			this.pageIndex += 1;
			this.refresh(false);
		}
	},

	methods: {
		getProblemStateTitle,

		//获取状态样式
		getStateColor(state) {
			let color = '';
			switch (state) {
				case SafeQualityProblemState.WaitingImprove:
					color = '#ff5500';
					break;
				case SafeQualityProblemState.WaitingVerify:
					color = '#ff55ff';
					break;
				case SafeQualityProblemState.Improved:
					color = '#42c32d';
					break;
			}

			return color;
		},

		//数据获取
		async refresh(reset = true, isClearData = false) {
			//是否清空数据源
			if (isClearData) {
				this.safeProblems = [];
				this.loading = true;
			}

			//页面是否重置
			if (reset) {
				this.pageIndex = 1;
			}
			this.loadStatus = 'loading';
			const response = await apiSafeProblem.getList({
				skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
				filterType: this.filterType,
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
						checkTime: moment(item.checkTime).format('YYYY-MM-DD hh:mm:ss'),
						limitTime: moment(item.limitTime).format('YYYY-MM-DD hh:mm:ss'),
						checker: item.checker ? item.checker.name : '',
						responsibleUser: item.responsibleUser ? item.responsibleUser.name : '',
						type: item.type ? item.type.name : '',
					};
				});
				this.safeProblems = this.safeProblems.concat(array);
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
				url: `../../pages/security/securityOfMark?pageState=${PageState.Add}`,
			});
		},

		//编辑页面跳转
		onEdit(item) {
			uni.navigateTo({
				url: `../../pages/security/securityOfMark?id=${item.id}&pageState=${PageState.Edit}`,
			});
		},

		//详情页面
		onView(id) {
			uni.navigateTo({
				url: `../../pages/security/securityOfDetails?id=${id}`,
			});
		},

		//整改页面
		onImprove(item) {
			uni.navigateTo({
				url: `../../pages/security/securityOfImprove?title=${item.title}&id=${item.id}`,
			});
		},

		//添加页面跳转
		onVerify(item) {
			uni.navigateTo({
				url: `../../pages/security/securityOfVerify?title=${item.title}&id=${item.id}`,
			});
		},
	},
};
</script>

<style lang="scss">
page {
	background-color: #0e2a58;
}
#uni-safe-problems {
	display: flex;
	flex-direction: column;
	height: 100%;

	.securities-item-operator {
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

	.uni-list-empty {
		height: 100%;
		flex: 1;
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		font-size: 30rpx;
		color: #c3c1c1;
	}

	.icon-empty-a {
		font-size: 120rpx;
		margin-bottom: 20rpx;
	}

	.uni-safe-problems-oprator {
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
	}

	.safe-problems-item-view {
		color: #1890ff;
		font-size: 30rpx;
	}

	.security-mark {
		display: flex;
		align-items: center;
	}

	.uni-safe-problems-item {
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
		align-items: center;
	}

	.safe-problems-item-title {
		font-size: 26rpx;
		color: #000000;
		flex: 1;
		overflow: hidden;
		white-space: nowrap;
		text-overflow: ellipsis;
	}

	.icon-filter {
		display: flex;
		align-items: center;
		font-size: 50rpx;
	}

	.icon-mark {
		display: flex;
		align-items: center;
		font-size: 40rpx;
		margin-right: 10rpx;
	}

	.icon-filter-title {
		margin-left: 10rpx;
	}
}
</style>
