<template>
	<view><web-view :webview-styles="webviewStyles" :src="url" @message="getMessage"></web-view></view>
</template>

<script>
export default {
	data() {
		return {
			webviewStyles: {
				progress: {
					color: '#FF3333'
				}
			},
			url: ''
		};
	},

	onLoad: function(option) {
		//option为object类型，会序列化上个页面传递的参数
		// this.url = option.url;
		// console.log(option.url); //打印出上个页面传递的参数。
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},
	methods: {}
};
</script>

<style></style>
