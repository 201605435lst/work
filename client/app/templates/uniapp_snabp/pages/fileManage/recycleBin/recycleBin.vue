<template>
	<view class="pages-fileManage-file-fileTransfer">
		<!-- 操作区 -->
		<view class="file-top">
			<view class="construction-dispatchs-oprator-view">
				<uni-search-bar :radius="100" cancelButton="none" placeholder="输入文件名" @input="search"></uni-search-bar>
			</view>
		</view>
		<!-- 加载中 -->
		<uniLoading v-show="loading" class="uniLoading" />
		<view class="uni-list-empty" v-if="!loadingSwitch && !(dataSources && dataSources.length > 0)">
			<view class="iconfont icon-empty-a" />
			<view class="uni-list-empty-title">暂无数据</view>
		</view>
		<!-- 展示区 -->
		<view style="padding-top: 80rpx" v-else>
			<view class="fileTransfer-list-top">
				<text>文件数：{{ totalCount }}</text>
				<text style="color: #21c4f5">清空</text>
			</view>
			<block v-for="(item, index) of dataSources" :key="index">
				<view class="file-item" @tap="clickFileFolder(item)">
					<view><image :src="getFileTypeIcon(item.type)" /></view>
					<view>
						<view style="display: flex">
							<view>
								{{ item.name }}
							</view>
							<text>{{ item.type != '文件夹' ? item.type : '' }}</text>
						</view>
						<view style="color: #afb3bf">
							{{ item.editTime + ' ' + (item.type != '文件夹' ? getFileSize(item.size) : '') }}
						</view>
					</view>
					<view @tap.stop="selectButton(item)">
						<image v-if="item.selected" :src="require('@/static/BIMAppUI/other/selected.png')" />
						<image v-else :src="require('@/static/BIMAppUI/other/unSelected.png')" />
					</view>
				</view>
			</block>
			<uni-load-more color="#ffffff" v-show="loadNoMore" :status="loadStatus"></uni-load-more>
		</view>
	</view>
</template>

<script>
import {pagination as paginationConfig} from '@/utils/config.js';
import {requestIsSuccess, getFileSize, getFileTypeIcon} from '@/utils/util.js';
import * as apiFile from '@/api/file/file.js';
import moment from 'moment';
export default {
	data() {
		return {
			waitApprove: true, //顶部选中状态
			dataSources: [], //数据源
			totalCount: 0, //文件数量
			loading: true,
			loadingSwitch: false,
			loadNoMore: true,
			loadStatus: 'more',
			queryParams: {
				id: '00000000-0000-0000-0000-000000000000',
				type: 1,
				isDelete: true,
				staticKey: '', // 静态文件夹key值
				size: 10,
				page: 0,
			},
			pageIndex: 1,
			maxResultCount: paginationConfig.defaultPageSize,
		};
	},
	onLoad(args) {
		this.refresh(true, true);
	},
	methods: {
		getFileSize,
		getFileTypeIcon,
		//列表数据查询refresh
		async refresh(reset = true, isClearData = false) {
			//页面是否重置
			if (reset) {
				this.pageIndex = 1;
			}
			//是否清空数据源
			if (isClearData) {
				this.dataSources = [];
				this.selected = [];
			}
			this.loadStatus = 'loading';
			const response = await apiFile.getResourceList({
				...this.queryParams,
				page: (this.pageIndex - 1) * this.maxResultCount,
				size: this.maxResultCount,
			});
			if (requestIsSuccess(response)) {
				let response_ = response.data.items;
				this.totalCount = response.data.totalCount;
				if (response_.length < 10) {
					this.loadStatus = 'noMore';
				} else {
					this.loadStatus = 'more';
				}
				let listData = response_.map(item => {
					return {
						...item,
						selected: false,
						editTime: item.editTime ? moment(item.editTime).format('YYYY-MM-DD HH:mm:ss') : '--',
					};
				});
				this.dataSources = this.dataSources.concat(listData);
				this.loading = false;
				this.loadingSwitch = false;
			}
		},
		/**顶部选择 */
		onTap(state) {
			//点击后，请求数据时不能再点击
			if (!this.loadingSwitch) {
				if (state) {
					this.queryParams = {
						type: 2,
						isMine: true,
						staticKey: '', // 静态文件夹key值
					};
				} else {
					this.queryParams = {
						id: '00000000-0000-0000-0000-000000000000',
						type: 1,
						staticKey: '', // 静态文件夹key值
					};
				}
				this.waitApprove = state ? true : false;
				this.loadingSwitch = true;
				this.refresh(true, true);
			}
		},
		/**搜索栏搜索 */
		search(val) {
			console.log(val);
		},
	},
};
</script>

<style lang="scss">
page {
	background-color: #0e2a58;
}
.pages-fileManage-file-fileTransfer {
	.file-top {
		position: fixed;
		z-index: 20;
		left: 0;
		right: 0;
		top: 0;
		display: flex;
		justify-content: space-between;
		align-items: center;
		min-height: 80rpx;
		max-height: 80rpx;
		color: #202020;
		padding: 0 30rpx;
		border-radius: 15rpx;
		.construction-dispatchs-oprator-view {
			width: 100%;
			height: 100%;
			display: flex;
			font-size: 28rpx;
			justify-content: space-around;
			> uni-search-bar {
				width: 100%;
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
	.file-item {
		display: flex;
		padding: 30rpx;
		background-color: #ffffff;
		border-radius: 20rpx;
		min-height: 80rpx;
		max-height: 80rpx;
		margin: 20rpx;
		position: relative;
		> view:nth-child(1) {
			min-width: 80rpx;
			max-width: 80rpx;
			margin-right: 30rpx;
			> image {
				width: 100%;
				height: 100%;
			}
		}
		> view:nth-child(2) {
			width: 100%;
			display: flex;
			flex-direction: column;
			font-size: 24rpx;
			justify-content: space-between;
			overflow: hidden;
			text-overflow: ellipsis;
			padding-right: 30rpx;
			> view:nth-child(1) {
				> view {
					overflow: hidden; /*必须结合的属性,当内容溢出元素框时发生的事情*/
					text-overflow: ellipsis; /*可以用来多行文本的情况下，用省略号“…”隐藏超出范围的文本 。*/
					display: -webkit-box; /*必须结合的属性 ，将对象作为弹性伸缩盒子模型显示 。*/
					-webkit-line-clamp: 1; /*用来限制在一个块元素显示的文本的行数。*/
					-webkit-box-orient: vertical; /*必须结合的属性 ，设置或检索伸缩盒对象的子元素的排列方式 。*/
				}
			}
		}
		> view:nth-child(3) {
			position: absolute;
			z-index: 5;
			top: 60rpx;
			right: 30rpx;
			width: 30rpx;
			height: 30rpx;
			> image {
				position: absolute;
				width: 100%;
				height: 100%;
			}
		}
	}
	.fileTransfer-list-top {
		display: flex;
		justify-content: space-between;
		color: #ffffff;
		font-size: 24rpx;
		padding: 10rpx 30rpx;
	}
}
</style>
