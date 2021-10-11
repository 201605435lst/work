<template>
	<view class="uni-construction-dailies">
		<view class="uni-construction-dailies-body" style="height: 100%" v-if="!loading">
			<!-- 操作区 -->
			<view class="uni-construction-dailies-oprator" style="z-index: 22">
				<view style="display: flex; align-items: center" @tap="showDrawer">
					<!-- <image :src="require('@/static/BIMAppUI/titleBlock/screening.png')" style="width: 40rpx; height: 40rpx" />
					<view class="icon-filter-title">筛选</view> -->
					<image :src="require('@/static/BIMAppUI/other/screening.png')" style="width: 50rpx; height: 50rpx" />
				</view>
				<view style="display: flex; align-items: center" @tap="onAdd">
					<image :src="require('@/static/BIMAppUI/other/add.png')" style="width: 40rpx; height: 40rpx" />
				</view>
			</view>
			<!-- 展示区 -->
			<view class="uni-construction-dailies-list" v-if="dailies.length > 0">
				<view style="height: 80rpx"></view>
				<view class="uni-construction-dailies-item" v-for="item in dailies" :key="item.id" @tap="() => onView(item)">
					<view class="item-content first-line">
						<view class="dailies-item-title">日志编号：{{ item.code }}</view>
						<view>
							<view
								class="dailies-item-operator"
								v-if="item.status == DailyState.ToSubmit || item.status == DailyState.UnPass"
							>
								<view style="display: flex; align-items: center">
									<text @tap.stop="onEdit(item)">编辑</text>
									<view class="divider"></view>
									<text class="dailies-item-view" @tap.stop="onSubmit(item)">提交审批</text>
								</view>
							</view>
							<text
								v-if="item.status == DailyState.OnReview && queryParams.approval"
								class="dailies-item-view"
								@tap.stop="onProcess(item.id)"
							>
								审批
							</text>
						</view>
					</view>
					<view class="item-content">
						状态：
						<view :style="{color: getStateColor(item.status)}">{{ getDailyStateTile(item.status) }}</view>
					</view>
					<view class="omit-content">创建人：{{ item.informantName || '--' }}</view>
					<view class="omit-content">填报时间：{{ item.date || '--' }}</view>
					<view class="omit-content">施工人数：{{ item.builderCount || '--' }}</view>
					<view style="position: relative; padding-right: 50rpx; overflow: hidden; white-space: nowrap; text-overflow: ellipsis">
						施工部位：{{ item.location || '--' }}
						<view class="Trash" style="top: 0" @tap.stop="remove(item.id)">
							<uni-icons
								v-if="item.status == DailyState.ToSubmit || item.status == DailyState.UnPass"
								type="trash"
								size="18"
								color="#ee0000"
							></uni-icons>
						</view>
					</view>
				</view>
				<uni-load-more color="#ffffff" :status="loadStatus"></uni-load-more>
			</view>

			<view class="uni-list-empty" v-else>
				<view class="iconfont icon-empty-a" />
				<view class="uni-list-empty-title">暂无数据</view>
			</view>
		</view>

		<uniLoading v-else style="height: 100%; display: flex; align-items: center; justify-content: center" />
		<constructionDailyDrawer ref="TableOperator" @change="onChange" />
	</view>
</template>

<script>
import constructionDailyDrawer from './constructionDailyDrawer.vue';
import uniPicker from '@/components/uniPicker.vue';
import {pagination as paginationConfig, tips as tipsConfig} from '@/utils/config.js';
import {checkToken, requestIsSuccess, getDailyStateTile, showToast} from '@/utils/util.js';
import {DailyState, PageState} from '@/utils/enum.js';
import * as apiDaily from '@/api/construction/daily.js';
import * as apiBpm from '@/api/bpm.js';
import moment from 'moment';

export default {
	name: 'constructionDailies',
	components: {
		constructionDailyDrawer,
		uniPicker,
	},
	data() {
		return {
			id: '',
			DailyState,
			pageIndex: 1,
			loading: true,
			queryParams: {
				keywords: '',
				startTime: '',
				endTime: '',
				approval: false,
				maxResultCount: paginationConfig.defaultPageSize,
			},
			dailies: [],
			skipCount: 0, // 跳过数
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

	onLoad(option) {
		this.queryParams.approval = JSON.parse(option.approval);
		uni.setNavigationBarTitle({
			title: !this.queryParams.approval ? '施工日志管理' : '施工日志审批',
		});
	},

	onReachBottom() {
		if (this.loadStatus === 'more') {
			this.pageIndex += 1;
			this.refresh(false);
		}
	},

	methods: {
		getDailyStateTile,
		//数据获取
		async refresh(reset = true, isClearData = false) {
			//是否清空数据源
			if (isClearData) {
				this.dailies = [];
				this.loading = true;
			}

			//页面是否重置
			if (reset) {
				this.pageIndex = 1;
			}
			this.loadStatus = 'loading';
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
				let array = response.data.items.map(item => {
					return {
						...item,
						date: moment(item.date).format('YYYY-MM-DD'),
						informantName: item.informant ? item.informant.name : '',
					};
				});
				this.dailies = this.dailies.concat(array);
			}
			this.loading = false;
		},

		getStateColor(state) {
			let color = '';
			switch (state) {
				case DailyState.ToSubmit:
					color = '#ff007f';
					break;
				case DailyState.OnReview:
					color = '#ff55ff';
					break;
				case DailyState.Pass:
					color = '#42c32d';
					break;
				case DailyState.UnPass:
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

		// 删除
		async remove(id) {
			if (id) {
				uni.showModal({
					cancelColor: '#f7bd8f',
					confirmColor: '#98d98e',
					content: '确定要删除该条数据吗？',
					success: async res => {
						if (res.confirm) {
							let response = await apiDaily.remove(id);
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

		close() {
			this.$refs.popup.close();
		},

		//施工日志填报页面跳转
		onAdd() {
			uni.navigateTo({
				url: `./constructionDaily?pageState=${PageState.Add}`,
			});
		},
		onEdit(item) {
			uni.navigateTo({
				url: `./constructionDaily?id=${item.id}&pageState=${PageState.Edit}`,
			});
		},
		onView(item) {
			uni.navigateTo({
				url: `./constructionDaily?id=${item.id}&pageState=${PageState.View}`,
			});
		},
		onProcess(item) {
			uni.navigateTo({
				url: `./constructionDaily?id=${item.id}&isApproval=true`,
			});
		},
		onSubmit(item) {
			uni.navigateTo({
				url: `./constructionDailySubmit?id=${item.id}`,
			});
		},
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

.uni-construction-dailies {
	display: flex;
	flex-direction: column;
	height: 100%;
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

.uni-construction-dailies-oprator {
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

.uni-construction-dailies-item {
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
	padding: 10rpx 50rpx;
	background-color: #1890ff;
	border-radius: 15rpx;
}

.dailies-item-time {
	display: flex;
	align-items: center;
	width: 160rpx;
	margin-left: 10rpx;
}

.dailies-item-title {
	color: #000000;
	font-size: 28rpx;
	flex: 1;
	overflow: hidden;
	white-space: nowrap;
	text-overflow: ellipsis;
}

.dailies-item-operator {
	width: 160rpx;
	display: flex;
	justify-content: flex-end;
	align-items: center;
	color: #1890ff;
	font-size: 28rpx;
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

.divider {
	background: #dfdfdf;
	height: 24rpx;
	margin: 0 20rpx;
	padding: 1rpx;
}

.Trash {
	position: absolute;
	right: 0;
}
.Trash uni-icons {
	padding: 10rpx;
}
</style>
