<template>
	<view class="equipmentTrack">
		<view class="container" v-for="(item,index) in options" :key="index">
			<view class="container-head">
				<icon :type="'success'" class="container-head-icon" />
				<!-- <view class=`"iconfont${}"` /> -->
				<view class="container-head-title">
					{{item.title}}
				</view>
			</view>
			<view class="container-content">
				<view class="container-content-item" >
					{{item.userType}}：<text style="color: #007AFF">{{item.userName}}</text><!-- //{{item.userName}} -->
				</view>
				<view class="container-content-item" >
					{{item.timeType}}：{{item.time}}
				</view>
				<view class="container-content-item" >
					{{item.title}}内容：{{item.content}}
				</view>
			</view>
		</view>
	</view>
</template>

<script>
	export default {
		name: "equipmentTrack",
		props: {
			options: {
				type: Array,
				default: () => []
			}
		}
	}
</script>

<style>
	page {
		background-color: #f2f2f2;
	}

	.container {
		margin-top: 30px;
		width: 90%;
		margin: auto;
		margin-top: 10px;
	}

	.container-head {
		display: flex;
		align-items: center;
	}

	.container-head-icon {
		font-size: 30upx;
		width: 8%;
	}

	.container-head-title {
		font-size: 35upx;
		letter-spacing: 2px;
		font-weight: bold;
		margin-bottom: 3px;
	}

	.container-content {
		border-left: dashed 2px #007aff;
		width: 94%;
		margin: auto;
	}

	.container-content-item {
		margin-top: 8px;
		font-size: 30upx;
		width: 90%;
		margin: auto;
		/* letter-spacing: 1px; */
	}
</style>
