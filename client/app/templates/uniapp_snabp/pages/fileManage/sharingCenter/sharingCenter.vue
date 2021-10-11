<template>
	<view class="pages-fileManage-sharingCenter-sharingCenter">
		<!-- 操作区 -->
		<view class="sharingCenter-top">
			<uni-search-bar
				style="width: 100%"
				:radius="100"
				cancelButton="none"
				placeholder="输入文件名称"
				@input="onInput"
			></uni-search-bar>
			<image :src="require('@/static/BIMAppUI/other/transmission.png')" />
		</view>
	</view>
</template>

<script>
export default {
	data() {
		return {
			inputVal: '',
		};
	},
	methods: {
		onInput(e) {
			console.log(e);
		},
	},
};
</script>

<style lang="scss">
page {
	background-color: #0e2a58;
}
.pages-fileManage-sharingCenter-sharingCenter {
	margin-top: 120rpx;
	.sharingCenter-top {
		position: fixed;
		left: 0;
		right: 0;
		top: 0;
		display: flex;
		justify-content: space-between;
		align-items: center;
		background-color: #006186;
		min-height: 70rpx;
		max-height: 70rpx;
		color: #ffffff;
		padding: 10rpx;
		border-radius: 15rpx;
		> image {
			width: 70rpx;
			height: 60rpx;
			margin: 0 10rpx;
		}
	}
}
</style>
