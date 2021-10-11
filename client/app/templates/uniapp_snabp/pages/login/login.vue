<template>
	<view id="uni-login" class="uni-login-top">
		<view class="uni-wave uni-login">
			<image
				:src="require('@/static/BIMAppUI/loginPage/setUp.png')"
				class="uni-login-train"
				mode="widthFix"
				@tap="() => this.$refs.popup.open()"
			/>
			<view class="uni-title">
				<!-- <view>
					<image :src="require('@/static/BIMAppUI/loginPage/train.png')" class="uni-title-image" />
				</view> -->
				<!-- BIM+GIS城市轨道交通信息集成管理云平台app -->
				<text v-if="$appBIMGIS" style="margin-top: 20rpx">
					BIM+GIS城市轨道交通\n
					<text style="font-size: 45rpx">信息集成管理云平台</text>
				</text>
				<!-- BIM接触网数字孪生管理应用平台app -->
				<text v-else style="margin-top: 20rpx">
					BIM接触网数字孪生\n
					<text style="font-size: 45rpx">管理应用平台</text>
				</text>
			</view>
			<form class="uni-login-form">
				<view class="uni-form-item uni-column input-bg">
					<image :src="require('@/static/BIMAppUI/loginPage/user.png')" class="image-user" />
					<input
						class="uni-input"
						name="userName"
						placeholder="请输入用户名"
						placeholder-style="margin-left:10rpx;color: #c3c1c1;"
						v-model="userName"
					/>
				</view>
				<view class="uni-form-item uni-column input-bg">
					<image :src="require('@/static/BIMAppUI/loginPage/password.png')" class="image-password" />
					<input
						class="uni-input"
						name="password"
						:password="true"
						placeholder="请输入密码"
						placeholder-style="margin-left:10rpx;color: #cccccc;"
						v-model="password"
					/>
				</view>
				<!-- 登录 -->
				<view class="uni-btn-v">
					<button @tap="onSubmit">
						登
						<text style="padding: 0 10rpx"></text>
						录
					</button>
				</view>
			</form>
			<uni-popup ref="popup" type="bottom">
				<uni-popup-dialog
					:customMade="true"
					:value="remoteServiceBaseUrl"
					type="success"
					mode="input"
					title="地址"
					placeholder="请输入服务器地址"
					:duration="2000"
					:before-close="true"
					@close="close"
					@confirm="confirm"
				></uni-popup-dialog>
			</uni-popup>
			<view class="uni-form-item uni-column remoteService-base-url">
				<!-- <view>
					<text>注册</text>
					|
					<text>找回密码</text>
				</view> -->
				<view>
					<text>帮助</text>
					|
					<text>隐私</text>
					|
					<text>条款</text>
				</view>
				<view class="pages-index-title_">Copyright © 2020 陕西心像信息科技有限公司</view>
			</view>
		</view>
	</view>
</template>

<script>
import {requestIsSuccess} from '../../utils/util.js';
import index from '../index/index';

export default {
	components: {index},
	data() {
		return {
			userName: 'admin',
			password: '1q2w3E*',
			remoteServiceBaseUrl: '',
			$appBIMGIS: this.$appBIMGIS, //app项目打包时选择项
		};
	},
	watch: {
		remoteServiceBaseUrl(val) {
			uni.setStorageSync('remoteServiceBaseUrl', val);
		},
	},
	methods: {
		async onSubmit() {
			if (this.userName == '' || this.password == '') {
				uni.showToast({
					icon: 'none',
					title: '请将信息填写完整',
					duration: 2000,
				});
			} else if (this.remoteServiceBaseUrl == '') {
				uni.showToast({
					icon: 'none',
					title: '请配置服务器访问地址',
					duration: 2000,
				});
			} else {
				uni.showLoading({
					title: '登陆中...',
				});
				//获取OpenIdConfig
				// const resOpenIdConfig = await apiLogin.getOpenIdConfig();
				// if (!requestIsSuccess(resOpenIdConfig)) {
				// 	uni.showToast({
				// 		icon: 'none',
				// 		title: '获取用户配置失败',
				// 		duration: 2000
				// 	});
				// 	uni.hideLoading();
				// 	return false;
				// }
				// 获取getJwks
				// const resJwks = await apiLogin.getJwks(resOpenIdConfig.data.jwks_uri);
				// if (!requestIsSuccess(resJwks)) {
				// 	uni.showToast({
				// 		icon: 'none',
				// 		title: '获取 Jwks 失败',
				// 		duration: 2000
				// 	});
				// 	uni.hideLoading();
				// 	return false;
				// }

				// 登录
				let data = {
					userName: this.userName,
					password: this.password,
					remoteServiceBaseUrl: this.remoteServiceBaseUrl,
				};
				const resToken = await this.$store.dispatch('login', data);
				if (requestIsSuccess(resToken) && resToken.data && resToken.data.access_token) {
					await this.$store.dispatch('getInfo');
					uni.setStorageSync('remoteServiceBaseUrl', this.remoteServiceBaseUrl);
					uni.hideLoading();
					uni.reLaunch({url: `/pages/index/index?loginSuccessful=${true}`});
				} else {
					uni.showToast({
						icon: 'none',
						title: '用户名密码或地址有误',
						duration: 3000,
					});
				}
			}
		},
		//地址框输入输出
		close() {
			this.$refs.popup.close();
			let remoteServiceBaseUrl = this.remoteServiceBaseUrl;
			this.remoteServiceBaseUrl = '';
			setTimeout(() => {
				this.remoteServiceBaseUrl = remoteServiceBaseUrl;
			}, 1);
		},
		confirm(value) {
			this.remoteServiceBaseUrl = value;
			if (value == '0') {
				this.remoteServiceBaseUrl = 'http://localhost:8091';
			} else if (value == '1') {
				this.remoteServiceBaseUrl = 'http://172.16.1.22:8140';
			} else if (value == '2') {
				this.remoteServiceBaseUrl = 'http://172.16.1.22:8170';
			} else if (value == '3') {
				this.remoteServiceBaseUrl = 'http://172.16.1.220:8140';
			} else if (value == '4') {
				this.remoteServiceBaseUrl = 'http://172.16.1.220:8170';
			} else if (value == '5') {
				this.remoteServiceBaseUrl = 'http://39.99.132.76:8140';
			} else if (value == '6') {
				this.remoteServiceBaseUrl = 'http://39.99.132.76:8170';
			}
			this.$refs.popup.close();
		},
	},
	mounted() {
		const remoteServiceBaseUrl = uni.getStorageSync('remoteServiceBaseUrl');
		if (remoteServiceBaseUrl) {
			this.remoteServiceBaseUrl = remoteServiceBaseUrl;
		} else {
			this.remoteServiceBaseUrl = this.$appBIMGIS ? 'http://39.99.132.76:8140' : 'http://39.99.132.76:8170';
		}
	},
};
</script>

<style lang="scss">
#uni-login {
	background-color: #0e2a58;
	color: white;
	position: relative;
	width: 100%;
	height: 100vh;
	overflow: hidden;
	.uni-wave {
		position: absolute;
		bottom: 0;
		left: 0;
		height: 100%;
		width: 100%;
	}

	.uni-login {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		text-align: center;
	}

	.uni-form-item {
		display: flex;
		margin: 40rpx 0;
		align-items: center;
		border-radius: 20rpx;
	}

	.image-user,
	.image-password {
		width: 60rpx;
		height: 60rpx;
		padding: 15rpx;
		padding-left: 30rpx;
		color: #cccccc;
	}
	.title {
		width: 120rpx;
		color: #cccccc;
		margin-right: 28rpx;
		text-align: center;
		font-size: 28rpx;
		letter-spacing: 4rpx;
	}

	.uni-input {
		flex: 1;
		/* background-color: #e0e0e0; */
		background-color: #173e7c;
		color: #cccccc;
		border-radius: 100rpx;
		font-size: 36rpx;
		padding: 0 10rpx;
		text-align: left;
		height: 60rpx;
	}
	.remoteService-base-url {
		width: 100%;
		font-size: 20rpx;
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
		position: absolute;
		bottom: 0;
		left: 0;
		color: #cccccc;
		margin: 20rpx 0;
	}
	.remoteService-base-url text {
		font-size: 24rpx;
		padding: 0 10rpx;
	}
	.remoteService-base-url > view {
		padding: 5rpx 0;
	}

	.uni-title {
		font-size: 60rpx;
		letter-spacing: 4rpx;
		margin-bottom: 100rpx;
		color: #ffffff;
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		position: relative;
		top: -130rpx;
		> view {
			width: 200rpx;
			height: 200rpx;
			border: 1rpx solid #ffffff;
			border-radius: 50%;
			display: flex;
			align-items: center;
			justify-content: center;
			overflow: hidden;
			.uni-title-image {
				height: 70%;
				position: relative;
				right: -20rpx;
				top: 6rpx;
			}
		}
	}

	.uni-btn-v {
		margin-top: 90rpx;
		button {
			color: #ffffff;
			background-color: #4b97f9;
		}
	}
	.remoteService-base-url > input {
		background-color: #ffffff00;
		color: #cccccc;
		font-size: 32rpx;
		text-align: center;
	}
	.remoteService-base-url > input:hover {
		background-color: #ffffff21;
	}
	.input-bg {
		background-color: #173e7c;
	}
	.uni-login-train {
		width: 50rpx;
		position: absolute;
		top: 80rpx;
		right: 30rpx;
	}
	.uni-login-form {
		margin: 0 116rpx;
		width: 90vw;
		position: relative;
		top: -100rpx;
	}
	.pages-index-title_ {
		font-size: 20rpx;
		color: darkgray;
		text-align: center;
	}
}
</style>
