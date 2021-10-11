<template>
	<view>
		<l-file ref="lFile" :logo="logo" @up-success="onSuccess"></l-file>
		
		<view class="padding text-center">
			<view class="padding">
				<button @tap="onOpenDoc">预览</button>
			</view>
			<view class="padding">
				<button @tap="onDown">下载到本地</button>
				<view class="text-center padding-sm" @tap="onOpenNameDoc(localPath)">点击打开：{{localPath}}</view>
			</view>
			<view class="padding">
				<button @tap="onUpload">上传</button>
			</view>
			<view class="padding" style="font-size: 26rpx;text-align: center;">
				<text decode>{{tip}}</text>
			</view>
		</view>
	</view>
</template>

<script>
	export default {
		data() {
			this.api = 'http://baidu.com/';
			return {
				logo: 'https://gimg2.baidu.com/image_search/src=http%3A%2F%2Fbpic.588ku.com%2Felement_origin_min_pic%2F00%2F00%2F07%2F155788a6d8a5c42.jpg&refer=http%3A%2F%2Fbpic.588ku.com&app=2002&size=f9999,10000&q=a80&n=0&g=0n&fmt=jpeg?sec=1619847627&t=2da40b583002205c204d980b54b35040',
				localPath: '',
				tip: `
					希望能帮到你\n
					不喜勿喷，不要期待更新，我很懒\n
					`,
			}
		},
		onLoad() {
			this.onLogin();
		},
		methods: {
			onLogin() {
			},
			onOpenNameDoc(path = '') {
				if (!path) {return;}
				/* 打开文件 */
				this.$refs.lFile.open(path);
			},
			onOpenDoc() {
				let url = 'https://dss3.bdstatic.com/70cFv8Sh_Q1YnxGkpoWK1HF6hhy/it/u=2534506313,1688529724&fm=26&gp=0.jpg';
				/* 下载返回临时路径（退出应用失效） */
				this.$refs.lFile.download({url})
				.then(path=>{
					/* 预览 */
					this.$refs.lFile.open(path);
				});
			},
		   /**
		   * 保存到本地
		   * type 非save为临时下载
		   * customName 仅type=save生效 附件自定义名称需带后缀，自定义目录需以/结尾
		   * DownloadOptions 仅type=save生效 可选参数(http://www.html5plus.org/doc/zh_cn/downloader.html#plus.downloader.DownloadOptions)
		   * 默认下载到_downloads/files/ 可通过DownloadOptions自定义
		   */
			onDown() {
				let url = 'http://baobab.kaiyanapp.com/api/v1/playUrl?vid=129764&resourceType=video&editionType=default&source=aliyun&playUrlType=url_oss';
				this.$refs.lFile.download({
					url, //必填，附件网络地址
					type:'save', //选填，非save为临时下载
					customName:'自定义文件名需要带后缀.jpg',
					//...DownloadOptions直接写key:value 
					// 例如：
					method: 'GET'
				})
				.then(path=>{
					this.localPath = path;
				}); 
			},
			
			/* 上传 */
			onUpload() { 
				/**
				 * currentWebview: 当前webview
				 * url：上传接口地址
				 * name：附件key,服务端根据key值获取文件流，默认file,上传文件的key
				 * header: 上传接口请求头
				 */
				this.$refs.lFile.upload({
					// #ifdef APP-PLUS
					// nvue页面使用时请查阅nvue获取当前webview的api，当前示例为vue窗口
					currentWebview: this.$mp.page.$getAppWebview(),
					// #endif
					url: this.api+'/dropbox/document/upload', //替换为你的
					name: 'file',
					header: {  //根据你接口需求自定义
						'Authorization': '',
						'uid': '3871',
						'client': 'app',
						'accountid': 'IMED5274'
					}
					// body参数直接写key,value,如：
					// key1: 'value1',
					// key2: 'value2',
				});
			},
			onSuccess(res) {
				console.log('上传成功回调',JSON.stringify(res));
				uni.showToast({
					title: JSON.stringify(res),
					icon: 'none'
				})
			}
		}
	}
</script>

<style>
.padding-sm {
	padding: 20upx;
}

.padding {
	padding: 30upx;
}
</style>
