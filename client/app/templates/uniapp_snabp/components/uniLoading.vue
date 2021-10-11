<template>
	<view class="uni-loading-more-page" :class="fullScreen ? '' : 'fullScreen'"><uni-load-more status="loading"></uni-load-more></view>
</template>
<script>
export default {
	props: {
		fullScreen: {type: Boolean, default: true}, //是否全屏
	},
};
</script>
<style>
.uni-loading-more-page {
	background-color: #f2f2f6;
	display: flex;
	align-items: center;
	justify-content: center;
	min-height: 600rpx;
	height: 100vh;
	width: 100vw;
}
.fullScreen {
	max-height: 600rpx;
}
</style>
