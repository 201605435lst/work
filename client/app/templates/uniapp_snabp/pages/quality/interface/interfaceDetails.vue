<template>
	<view class="pages-interface-interfaceDetails">
		<block v-if="detailsData.length > 0">
			<view class="details" v-for="(item, index) in detailsData" :key="index">
				<view>
					<view>检查人员：{{ item.marker ? item.marker.name : '--' }}</view>
					<view>土建单位：{{ item.builder ? item.builder.name : '--' }}</view>
					<view>接口状况：{{ item.markType == 2 ? '合格' : item.markType == 3 ? '不合格' : '未检查' }}</view>
					<view>状况原因：{{ item.reason || '--' }}</view>

					<view class="image-css" v-if="item.markFiles.length > 0">
						<view>检查图片：</view>
						<view style="display: flex; flex-wrap: wrap">
							<block v-for="(z, i) in item.markFiles" :key="i">
								<view v-if="z.type == 1">
									<image :src="getFileUrl(z.markFile.url)" mode="aspectFill" @tap="previewImage(index, i)"></image>
								</view>
							</block>
						</view>
					</view>
					<block v-if="item.markType == 3">
						<view>整改人员：{{ item.reformer ? item.reformer.name : '--' }}</view>
						<view>整改时间：{{ item.reformDate ? item.reformDate : '--' }}</view>
						<view>整改意见：{{ item.reformExplain || '--' }}</view>

						<view class="image-css" v-if="item.markFiles.length > 0">
							<view>整改图片：</view>
							<view style="display: flex; flex-wrap: wrap">
								<block v-for="(z, i) in item.markFiles" :key="i">
									<view v-if="z.type == 2">
										<image :src="getFileUrl(z.markFile.url)" mode="aspectFill" @tap="previewImage(index, i)"></image>
									</view>
								</block>
							</view>
						</view>
					</block>
				</view>
				<view>
					<image :src="require('@/static/BIMAppUI/other/time.png')" class="time-img" />
					<view>{{ item.markDate }}</view>
				</view>
			</view>
		</block>
		<view v-else class="uni-list-empty">
			<view class="iconfont icon-empty-a" />
			<view class="uni-list-empty-title">暂无记录</view>
		</view>
	</view>
</template>

<script>
import * as apiConstructInterfaceInfo from '@/api/quality/constructInterfaceInfo.js';
import {getFileUrl, requestIsSuccess} from '@/utils/util.js';
import moment from 'moment';
export default {
	data() {
		return {
			detailsData: [], // 详情数据
		};
	},
	onLoad(e) {
		this.refresh(e.id);
	},
	methods: {
		getFileUrl,
		button() {
			uni.navigateBack();
		},
		async refresh(id) {
			let response = await apiConstructInterfaceInfo.getInterfanceReform({
				constructInterfaceId: id,
			});
			if (requestIsSuccess(response)) {
				this.detailsData = response.data.map(item => {
					return {
						...item,
						markDate: item.markDate ? moment(item.markDate).format('YYYY-MM-DD HH:mm:ss') : '--',
						reformDate: item.reformDate ? moment(item.reformDate).format('YYYY-MM-DD HH:mm:ss') : '--',
					};
				});
			}
		},
		//预览图片
		previewImage: function (index, i) {
			let urls = [];
			this.detailsData[index].markFiles.forEach(item => {
				urls.push(getFileUrl(item.markFile.url));
			});
			uni.previewImage({
				current: i,
				urls: urls,
			});
		},
	},
};
</script>

<style lang="scss">
.pages-interface-interfaceDetails {
	padding: 30rpx 0;
	.details {
		display: flex;
		flex-direction: column;
		color: #3b3b3b;
		font-size: 28rpx;
		padding: 0 30rpx;
		> view:nth-child(1) {
			margin-left: 18rpx;
			padding-left: 30rpx;
			padding-top: 20rpx;
			border-left: 4rpx solid #cccccc;
			background-color: ghostwhite;
			> view {
				padding-bottom: 20rpx;
			}
		}
		> view:nth-child(2) {
			display: flex;
			align-items: center;
			.time-img {
				width: 40rpx;
				height: 40rpx;
				padding-right: 10rpx;
			}
		}
		.image-css {
			display: flex;
		}
		.image-css > view:nth-child(1) {
			white-space: nowrap;
		}
		.image-css > view:nth-child(2) > view {
			width: 70rpx;
			height: 90rpx;
			margin: 0 8rpx 10rpx;
			border: 1rpx solid #f1f1f1;
		}
		.image-css > view:nth-child(2) image {
			width: 100%;
			height: 100%;
		}
	}
	.uni-list-empty {
		width: 100vw;
		height: 100vh;
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		font-size: 30rpx;
		color: dimgrey;
	}
	.icon-empty-a {
		font-size: 120rpx;
		margin-bottom: 20rpx;
	}
}
</style>
