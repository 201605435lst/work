<!-- 质量管理页面 -->
<template>
	<view id="uni-quality">
		<!-- 操作区 -->
		<view class="uni-quality-oprator">
			<view style="display: flex; align-items: center" @tap="showDrawer">
				<image :src="require('@/static/BIMAppUI/other/screening.png')" style="width: 50rpx; height: 50rpx" />
			</view>
			<view v-if="filterType == SafeQualityFilterType.All" class="quality-mark" @tap="onTap('mark')">
				<image :src="require('@/static/BIMAppUI/other/add.png')" style="width: 40rpx; height: 40rpx" />
			</view>
		</view>
		<!-- 加载中 -->
		<uniLoading v-show="loading" class="uniLoading" />
		<view class="uni-list-empty" v-if="!(qualitys && qualitys.length > 0)">
			<view class="iconfont icon-empty-a" />
			<view class="uni-list-empty-title">暂无数据</view>
		</view>
		<!-- 展示区 -->
		<view style="padding-top: 80rpx" v-else>
			<view class="uni-quality-item" v-for="item in qualitys" :key="item.id">
				<view class="item-content first-line">
					<view class="securities-item-title">标题：{{ item.title }}</view>
					<view class="securities-item-operator">
						<block v-if="filterType != SafeQualityFilterType.All && filterType != SafeQualityFilterType.CopyMine">
							<text v-if="filterType == SafeQualityFilterType.MyWaitingImprove" @tap="onTap('improved', item)">整改</text>
							<text v-else-if="filterType == SafeQualityFilterType.MyWaitingVerify" @tap="onTap('verified', item)">验证</text>
							<text
								v-else-if="
									filterType == SafeQualityFilterType.MyChecked && item.state == SafeQualityProblemState.WaitingImprove
								"
								@tap="onTap('checked', item)"
							>
								编辑
							</text>
							<view
								v-if="
									(filterType == SafeQualityFilterType.MyChecked &&
										item.state == SafeQualityProblemState.WaitingImprove) ||
									filterType != SafeQualityFilterType.MyChecked
								"
								class="divider"
							></view>
						</block>
						<text @tap="onTap('details', item)">详情</text>
					</view>
				</view>
				<view class="item-content">
					状态：
					<view :style="{color: getStateColor(item.state)}">{{ getQuality(item.state) }}</view>
				</view>
				<view class="omit-content">类型：{{ getQualityProblemType(item.type) }}</view>
				<view class="omit-content">检查时间：{{ item.checkTime }}</view>
				<view class="omit-content">限定时间：{{ item.limitTime }}</view>
				<view class="omit-content">检查人：{{ item.checker.name }}</view>
				<view class="omit-content">责任人：{{ item.responsibleUser.name }}</view>
				<view class="omit-content">责任单位：{{ item.responsibleUnit }}</view>
			</view>
			<uni-load-more color="#ffffff" v-show="loadNoMore" :status="loadStatus"></uni-load-more>
		</view>
		<qualityDrawer ref="TableOperator" @change="onChange" />
	</view>
</template>

<script>
import qualityDrawer from './qualityDrawer.vue';
import {getQuality, requestIsSuccess, setNavigationBarTitle, getQualityProblemType} from '@/utils/util.js';
import {pagination as paginationConfig} from '@/utils/config.js';
import {SafeQualityProblemState, SafeQualityFilterType, PageState} from '@/utils/enum.js';
import * as apiQuality from '@/api/quality/quality.js';
import moment from 'moment';

export default {
	name: 'quality',
	components: {
		qualityDrawer,
	},
	data() {
		return {
			SafeQualityFilterType,
			SafeQualityProblemState,
			pageIndex: 1,
			loading: true,
			queryParams: {
				title: '', //问题标题
				type: '', //问题类型
				startTime: '', //开始时间
				endTime: '', //结束时间
				maxResultCount: paginationConfig.defaultPageSize,
			},
			loadStatus: 'more',
			loadNoMore: true,
			qualitys: [],
			filterType: 0, //当前页面的标题类型
		};
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
		this.refresh(true, true);
	},

	onLoad(args) {
		setNavigationBarTitle(args.type);
		this.filterType = args.type;
	},

	onReachBottom() {
		if (this.loadStatus === 'more') {
			this.loadNoMore = true;
			this.pageIndex += 1;
			this.refresh(false);
		}
	},

	methods: {
		getQuality,
		getQualityProblemType,
		//数据获取
		async refresh(reset = true, isClearData = false) {
			//是否清空数据源
			if (isClearData) {
				this.qualitys = [];
				this.loading = true;
			}

			//页面是否重置
			if (reset) {
				this.pageIndex = 1;
			}
			this.loadStatus = 'loading';
			const response = await apiQuality.getList({
				filterType: this.filterType,
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
						ccUsers: item.ccUsers.map(e => {
							let ccUser = e.ccUser;
							return {...ccUser};
						}),
						checkTime: moment(item.checkTime).format('YYYY-MM-DD'),
						limitTime: moment(item.limitTime).format('YYYY-MM-DD'),
					};
				});
				this.qualitys = this.qualitys.concat(array);
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
		// 获取状态颜色
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
		// 标记/整改/验证/编辑/详情按钮
		onTap(operation, item) {
			switch (operation) {
				case 'mark':
					uni.navigateTo({
						url: `./qualityOfMark?pageState=${PageState.Add}`,
					});
					break;
				case 'improved':
					uni.navigateTo({
						url: `./qualityOfImprove?data=${JSON.stringify(item)}`,
					});
					break;
				case 'verified':
					uni.navigateTo({
						url: `./qualityOfVerify?data=${JSON.stringify(item)}`,
					});
					break;
				case 'checked':
					uni.navigateTo({
						url: `./qualityOfMark?id=${item.id}&pageState=${PageState.Edit}`,
					});
					break;
				case 'details':
					uni.navigateTo({
						url: `./qualityOfDetails?id=${item.id}`,
					});
					break;
				default:
					break;
			}
		},
	},
};
</script>

<style lang="scss">
page {
	background-color: #0e2a58;
}
#uni-quality {
	display: flex;
	flex-direction: column;
	height: 100%;

	.uni-list-empty {
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

	.uni-quality-oprator {
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

	.quality-item-view {
		color: #1890ff;
		font-size: 30rpx;
	}

	.quality-mark {
		display: flex;
		align-items: center;
	}

	.uni-quality-item {
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

	.quality-item-title {
		font-size: 28rpx;
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

	.icon-filter-title {
		margin-left: 10rpx;
	}
	.securities-item-operator {
		width: 160rpx;
		display: flex;
		justify-content: flex-end;
		align-items: center;
		color: #1890ff;
		font-size: 30rpx;
	}
	.securities-item-title {
		color: #000000;
		font-size: 28rpx;
		flex: 1;
		overflow: hidden;
		white-space: nowrap;
		text-overflow: ellipsis;
	}
	.divider {
		background: #dfdfdf;
		height: 24rpx;
		margin: 0 20rpx;
		padding: 1rpx;
	}
}
</style>
