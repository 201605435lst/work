<template>
	<view id="pages-user-concerning">
		<view class="concerning-top">
			<image :src="require('@/static/BIMAppUI/appIcon/c.png')" style="width: 150rpx; height: 150rpx" />
			<text>版本 {{ version }}</text>
		</view>
		<view class="concerning-major">
			<view @tap="onPrompt('去评分')">
				<text>去评分</text>
				<view class="iconfont icon-arrow-right info-icon" />
			</view>
			<view @tap="onPrompt('检查新版本')">
				<text>检查新版本</text>
				<view class="iconfont icon-arrow-right info-icon" />
			</view>
			<view @tap="onPrompt('新功能介绍')">
				<text>新功能介绍</text>
				<view class="iconfont icon-arrow-right info-icon" />
			</view>
			<view @tap="onPrompt('服务协议')">
				<text>服务协议</text>
				<view class="iconfont icon-arrow-right info-icon" />
			</view>
			<view @tap="onPrompt('隐私政策')">
				<text>隐私政策</text>
				<view class="iconfont icon-arrow-right info-icon" />
			</view>
			<view @tap="onPrompt('版权信息')">
				<text>版权信息</text>
				<view class="iconfont icon-arrow-right info-icon" />
			</view>
		</view>
		<view class="pages-index-title">Copyright © 2020 陕西心像信息科技有限公司 | v{{ version }} {{ startupTime }}</view>
	</view>
</template>

<script>
import moment from 'moment';
export default {
	data() {
		return {
			version: '1.0.0',
			startupTime: '',
		};
	},
	mounted() {
		this.startupTime = moment().format('YYYY-MM-DD');
		// #ifdef APP-PLUS
		this.version = plus.runtime.version;
		// #endif
	},
	methods: {
		onPrompt(key) {
			switch (key) {
				case 'a1':
					uni.showToast({
						icon: 'none',
						title: `${key}...`,
						duration: 2000,
					});
					break;
				default:
					uni.showToast({
						icon: 'none',
						title: `${key}...`,
						duration: 2000,
					});
					break;
			}
		},
	},
};
</script>

<style lang="scss">
page {
	background-color: #f2f2f6;
}
#pages-user-concerning {
	.concerning-top {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		padding: 60rpx 0;
		text {
			font-size: 26rpx;
			color: #6b6b6b;
		}
	}
	.concerning-major {
		> view {
			display: flex;
			justify-content: space-between;
			align-items: center;
			background-color: #ffffff;
			margin-bottom: 4rpx;
			padding: 20rpx 10rpx;
			padding-left: 20rpx;
			color: #191919;
			&:active {
				background: #d5d5d5;
			}
			> view {
				font-size: 50rpx;
				color: #b2b2b2;
			}
		}
	}
	.pages-index-title {
		width: 100vw;
		font-size: 20rpx;
		color: darkgray;
		text-align: center;
		position: absolute;
		bottom: 25rpx;
	}
}
</style>
