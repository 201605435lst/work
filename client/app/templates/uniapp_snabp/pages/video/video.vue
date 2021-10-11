<template>
	<view>
		<video style="width: 100vw; height: 100vh; display: block" autoplay @error="videoErrorCallback" :src="url"></video>
	</view>
</template>

<script>
export default {
	data() {
		return {
			url: '',
			videoContext: null,
		};
	},
	onReady() {
		this.videoContext = uni.createVideoContext('myVideo');
		this.videoContext.requestFullScreen();
	},
	onLoad(option) {
		if (option.url.indexOf('//') == -1) {
			this.url = 'http://' + option.url;
		} else {
			this.url = 'http:' + option.url;
		}
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},
	methods: {
		videoErrorCallback() {
			uni.showToast({
				title: '视频出错了',
				duration: 2000,
			});
		},
	},
};
</script>
