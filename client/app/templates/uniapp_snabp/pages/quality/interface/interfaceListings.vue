<template>
	<view id="uni-interface-interfaceListings">
		<!-- 操作区 -->
		<view class="construction-dispatchs-oprator">
			<view class="construction-dispatchs-oprator-view">
				<block v-for="(item, index) of interfaceManagementTypeIds" :key="index">
					<view :class="waitApprove == index ? 'approve-selected' : ''" @tap="onTap(index, item.id)">{{ item.name }}</view>
				</block>
			</view>
			<view style="display: flex; align-items: center" @click="showDrawer">
				<image :src="require('@/static/BIMAppUI/other/screening.png')" style="width: 50rpx; height: 50rpx" />
			</view>
		</view>
		<!-- 加载中 -->
		<uniLoading v-show="loading" class="uniLoading" />
		<view class="uni-list-empty" v-if="!(listData && listData.length > 0)">
			<view class="iconfont icon-empty-a" />
			<view class="uni-list-empty-title">暂无数据</view>
		</view>
		<!-- 展示区 -->
		<view class="uni-schedule-approvals-list" v-else>
			<view style="height: 80rpx"></view>
			<block v-for="(item, index) of listData" :key="index">
				<view class="uni-schedule-approvals-item">
					<view class="item-content first-line">
						<view class="item-content">接口名称：{{ item.name || '--' }}</view>
						<view class="item-content">
							<view class="details">
								<text @tap="goto(item.markType == 3 ? 'modify' : 'Sign', item.id, item.name)">
									{{ item.markType == 3 ? '整改' : '标记' }}
								</text>
								|
								<text @tap="goto('Details', item.id)">详情</text>
							</view>
						</view>
					</view>
					<view class="item-content">
						检查情况：
						<view :style="{color: getStateColor(item.markType)}">{{ getInterfaceStateTitle(item.markType) }}</view>
					</view>
					<view class="omit-content">专业：{{ item.profession || '--' }}</view>
					<view class="omit-content">接口编号：{{ item.code || '--' }}</view>
					<view class="omit-content" style="display: flex">
						接口位置：
						<view v-if="item.position">
							<view class="omit-content" style="margin-bottom: 0">经度：{{ item.position.lon || '--' }}</view>
							<view class="omit-content" style="margin-bottom: 0">纬度：{{ item.position.lat || '--' }}</view>
							<view class="omit-content" style="margin-bottom: 0">高程：{{ item.position.alt || '--' }}</view>
						</view>
						<text v-else>--</text>
					</view>
					<view class="omit-content">土建单位：{{ item.builder || '--' }}</view>
				</view>
			</block>
			<uni-load-more color="#ffffff" v-show="loadNoMore" :status="loadStatus"></uni-load-more>
		</view>
		<interfaceListingDrawer ref="TableOperator" @change="onChange" />
	</view>
</template>

<script>
import interfaceListingDrawer from './interfaceListingDrawer.vue';
import {getInterfaceStateTitle, requestIsSuccess} from '@/utils/util.js';
import {pagination as paginationConfig} from '@/utils/config.js';
import {interfaceState} from '@/utils/enum.js';
import * as apiConstructInterface from '@/api/quality/constructInterface.js';
import * as apiSystem from '@/api/system.js';

export default {
	components: {interfaceListingDrawer},
	data() {
		return {
			interfaceState,
			id: '', //当前选中接口管理类型id
			waitApprove: 0, //顶部选择的土建与系统选中状态
			loadingSwitch: false, //选择后状态
			interfaceManagementTypeIds: [], //接口管理类型。。。
			loading: true,
			listData: [],
			loadStatus: 'more',
			loadNoMore: true,
			pageIndex: 1,
			queryParams: {
				professionId: '',
				builderId: '',
				markType: '',
				maxResultCount: paginationConfig.defaultPageSize,
			},
		};
	},
	onLoad(args) {
		this.getGroupCode();
	},

	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
		this.refresh(true, true);
	},

	onReachBottom() {
		if (this.loadStatus === 'more') {
			this.loadNoMore = true;
			this.pageIndex += 1;
			this.refresh(false, false);
		}
	},

	methods: {
		getInterfaceStateTitle,
		async refresh(reset = true, isClearData = false) {
			console.log(this.id);
			//是否清空数据源
			if (isClearData) {
				this.listData = [];
				this.loading = true;
			}

			//页面是否重置
			if (reset) {
				this.pageIndex = 1;
			}
			const response = await apiConstructInterface.getList({
				skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
				interfaceManagementTypeId: this.id,
				...this.queryParams,
			});
			if (requestIsSuccess(response) && response.data && response.data.items.length >= 0) {
				if (response.data.items.length < 10) {
					this.loadStatus = 'noMore';
				} else {
					this.loadStatus = 'more';
				}
				let array = response.data.items.map(item => {
					let position = JSON.parse(item.position);
					return {
						...item,
						position: position,
						profession: item.profession ? item.profession.name : '',
						constructionSection: item.constructionSection && item.constructionSection.name ? item.constructionSection.name : '',
						builder: item.builder ? item.builder.name : '',
					};
				});
				this.listData = this.listData.concat(array);
				this.loading = false;
				this.loadingSwitch = false;
			}
		},
		/**获取接口管理类型 */
		async getGroupCode() {
			const response = await apiSystem.getValues({groupCode: 'InterfaceManagementType'});
			if (requestIsSuccess(response) && response && response.data.length > 0) {
				this.interfaceManagementTypeIds = response.data.map(item => {
					return {
						id: item.id,
						name: item.name,
					};
				});
				this.id =
					this.interfaceManagementTypeIds && this.interfaceManagementTypeIds.length > 0
						? this.interfaceManagementTypeIds[0].id
						: '';
			}
		},

		getStateColor(state) {
			let color = '';
			switch (state) {
				case interfaceState.Passed:
					color = '#42c32d';
					break;
				case interfaceState.Checking:
					color = '#ff55ff';
					break;
				case interfaceState.UnCheck:
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
		goto(a, b, c) {
			if (a == 'Sign') {
				uni.navigateTo({
					url: `./interfaceSign?id=${b}&name=${c}`,
				});
			} else if (a == 'modify') {
				uni.navigateTo({
					url: `./interfaceModify?id=${b}&name=${c}`,
				});
			} else {
				uni.navigateTo({
					url: `./interfaceDetails?id=${b}`,
				});
			}
		},
		onTap(index, id) {
			//点击后，请求数据时不能再点击
			if (!this.loadingSwitch) {
				this.id = id;
				this.waitApprove = index;
				this.listData = [];
				this.loadingSwitch = true;
				this.refresh(true, false);
			}
		},
	},
};
</script>

<style lang="scss">
page {
	background-color: #0e2a58;
}
#uni-interface-interfaceListings {
	display: flex;
	flex-direction: column;
	height: 100%;
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
			font-size: 22rpx;
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
		align-items: center;
		justify-content: space-between;
	}
	.first-line > view {
		margin: 0 !important;
	}

	.item-content {
		margin-bottom: 15rpx;
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

	.icon-filter {
		display: flex;
		align-items: center;
		font-size: 50rpx;
	}

	.icon-add {
		font-size: 50rpx;
	}

	.icon-filter-title {
		font-size: 28rpx;
		margin-left: 10rpx;
	}
}
</style>
