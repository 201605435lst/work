<template>
	<view id="pages-user-concerning">
		<view class="concerning-title">
			<view>热点问题</view>
			<view class="concerning-title-image" @tap="onFeedback">
				<image :src="require('@/static/BIMAppUI/personalCenter/news2.png')" />
				意见反馈
			</view>
		</view>
		<view class="concerning-major">
			<block v-for="(item, index) of questionData" :key="index">
				<view @tap="onPrompt(item.url)">
					<text>{{ item.value }}</text>
					<view class="iconfont icon-arrow-right info-icon" />
				</view>
			</block>
		</view>
	</view>
</template>

<script>
export default {
	data() {
		return {
			questionData: [
				{value: '什么是‘标记’功能？', url: '../../pages/user/concerning'},
				{value: '发送失败怎么办？', url: '../../pages/user/concerning'},
				{value: '如何迁移记录？', url: '../../pages/user/concerning'},
				{value: '质量安全相关问题？', url: '../../pages/user/concerning'},
			],
		};
	},
	methods: {
		onFeedback() {
			uni.showToast({
				icon: 'none',
				title: `意见反馈...`,
				duration: 2000,
			});
		},
		onPrompt(url) {
			uni.navigateTo({
				url: url,
			});
		},
	},
};
</script>

<style lang="scss">
page {
	background-color: #f2f2f6;
}
#pages-user-concerning {
	.concerning-title {
		padding: 20rpx;
		font-size: 28rpx;
		color: #444444;
		display: flex;
		justify-content: space-between;
		align-items: center;
		.concerning-title-image {
			display: flex;
			justify-content: space-between;
			align-items: center;
			image {
				width: 30rpx;
				height: 30rpx;
				padding-right: 10rpx;
				padding-top: 6rpx;
			}
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
}
</style>
