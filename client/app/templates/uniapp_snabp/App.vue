<script>
export default {
	onLaunch: function () {},
	// onLaunch: function () {this.AndroidCheckUpdate();},
	onShow: function () {
		// 应用启动或从后台进入时判断token
		if (!uni.getStorageSync('token')) {
			uni.reLaunch({url: '../login/login'});
		} else {
			this.$store.dispatch('getInfo');
		}
	},
	onHide: function () {},

	methods: {
		/**
		 * 安卓应用的检测更新实现
		 */
		AndroidCheckUpdate: function () {
			var _this = this;
			var version = 101;
			// console.log(version);
			uni.request({
				//请求地址，设置为自己的服务器链接
				// url: '*************/index.php/System/version', //这是完整域名
				// 也可以将域名写在仓库里面 然后这里面调用 以防换域名的时候来回更改
				url: _this.baseUrl + 'index.php/System/version',
				method: 'POST',
				data: {},
				success: resMz => {
					console.log('版本号信息', resMz.data);
					//返回的版本号
					var server_version = resMz.data.data.version;
					//返回的时间戳
					var currTimeStamp = resMz.data.data.timestamp;
					//返回的状态码 0 是自然更新 1是强制更新
					var status = resMz.data.data.update_status;
					console.log(
						'本地版本号version= ' + version + ',更新码=' + status + ',后台版本号=' + server_version + ',时间戳' + currTimeStamp,
					);
					if (status == 1) {
						//强制更新 又称为热更新  系统出现大bug 必须更新
						_this.MustcheckVersionToLoadUpdate(server_version, version);
					} else if (status == 0) {
						//自然更新
						uni.getStorage({
							key: 'tip_version_update_time',
							success: function (res) {
								var lastTimeStamp = res.data;
								var tipTimeLength = 0;
								console.log('时间间隔', tipTimeLength);
								let cha = lastTimeStamp + tipTimeLength - currTimeStamp;
								console.log('本地时间戳=', lastTimeStamp);
								console.log('时间戳差值', cha);
								if (lastTimeStamp + tipTimeLength > currTimeStamp) {
									//这里不用理会
									console.log('当后台时间戳大于本地时间戳才会进入');
								} else {
									console.log('立即更新');
									//重新设置时间戳
									_this.setStorageForAppVersion(currTimeStamp);
									//进行版本型号的比对 以及下载更新请求
									console.log(server_version, version);
									_this.checkVersionToLoadUpdate(server_version, version);
								}
							},
							fail: function (res) {
								_this.setStorageForAppVersion(currTimeStamp);
							},
						});
					}
				},
				fail: () => {},
				complete: () => {},
			});
		},
		/**
		 * //设置应用版本号对应的缓存信息
		 * @param {Object} currTimeStamp 当前获取的时间戳
		 */
		setStorageForAppVersion: function (currTimeStamp) {
			uni.setStorage({
				key: 'tip_version_update_time',
				data: currTimeStamp,
				success: function () {
					console.log('setStorage-success');
				},
			});
		},
		/**
		 * 进行版本型号的比对 以及下载更新请求 自然更新
		 * @param {Object} server_version 服务器最新 应用版本号
		 * @param {Object} curr_version 当前应用版本号
		 */
		checkVersionToLoadUpdate: function (server_version, curr_version) {
			if (server_version > curr_version) {
				uni.showModal({
					title: '版本更新',
					content: '有新的版本发布，是否立即进行新版本下载？',
					confirmText: '立即更新',
					cancelText: '取消',
					success: function (res) {
						if (res.confirm) {
							uni.showToast({
								icon: 'none',
								mask: true,
								title: '有新的版本发布，程序已启动自动更新。',
								duration: 5000,
							});
							//设置 最新版本apk的下载链接 这是固定的 每次把包放在这个链接里里面即可 由后端制作
							var downloadApkUrl =
								'http://zons.oss-cn-shenzhen.aliyuncs.com/upload/20200616/20200616/159228906014ee22eaba297944c96afdbe5b16c65b.apk';
							console.log(downloadApkUrl);
							plus.runtime.openURL(downloadApkUrl);
						} else if (res.cancel) {
							console.log('下次一定');
						}
					},
				});
			}
		},
		/**
		 * 进行版本型号的比对 以及下载更新请求 自然更新
		 * @param {Object} server_version 服务器最新 应用版本号
		 * @param {Object} curr_version 当前应用版本号
		 */
		MustcheckVersionToLoadUpdate: function (server_version, curr_version) {
			if (server_version > curr_version) {
				uni.showModal({
					title: '版本更新',
					content: '有新的版本发布，检测到您当前为Wifi连接，是否立即进行新版本下载？',
					confirmText: '立即更新',
					showCancel: false,
					success: function (res) {
						if (res.confirm) {
							uni.showToast({
								icon: 'none',
								mask: true,
								title: '有新的版本发布，程序已启动自动更新。',
								duration: 5000,
							});
							//设置 最新版本apk的下载链接 这是固定的
							var downloadApkUrl =
								'http://zons.oss-cn-shenzhen.aliyuncs.com/upload/20200616/20200616/159228906014ee22eaba297944c96afdbe5b16c65b.apk';
							console.log(downloadApkUrl);
							plus.runtime.openURL(downloadApkUrl);
						}
					},
				});
			}
		},
	},
};
</script>

<style>
/*每个页面公共css */
@import './static/icon-self/iconfont.css';
@import './App.css';
</style>
