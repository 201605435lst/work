<template>
	<view class="uni-user-center">
		<view class="uni-user-center-top">
			<view class="uni-user-header">
				<view class="uni-user-header-box">
					<view class="uni-user-header-info">
						<view class="uni-user-avatar">
							<view class="uni-user-avatar-item">
								<image :src="require('@/static/BIMAppUI/headPortrait.png')" style="width: 80rpx; height: 80rpx" />
							</view>
						</view>
						<view class="uni-user-info">
							<view>
								{{ userInfo.name }}
							</view>
							<view>
								<text>用户名</text>
								：
								{{ userInfo.userName }}
							</view>
							<view style="display: flex">
								<view
									style="
										display: flex;
										white-space: nowrap;
										align-items: center;
										position: relative;
										padding-right: 20rpx;
									"
								>
									<image
										:src="require('@/static/BIMAppUI/other/project.png')"
										style="width: 30rpx; height: 30rpx; position: absolute; left: 14rpx"
									/>
									<treeSelect
										title="项目"
										placeholder="选择项目"
										:isSimple="true"
										:noCancel="true"
										v-model="project"
										action="/api/app/projectManager/getList"
										@input="onProject"
									/>
								</view>
								<view style="display: flex; white-space: nowrap; align-items: center; position: relative">
									<image
										:src="require('@/static/BIMAppUI/other/organization.png')"
										style="width: 30rpx; height: 30rpx; position: absolute; left: 14rpx"
									/>
									<treeSelect
										title="组织机构"
										placeholder="选择组织"
										:isSimple="true"
										:noCancel="true"
										v-model="organization"
										:isCurrentUserOrganizations="true"
										:lazy="true"
										action="/api/app/appOrganization/getList"
										@input="onSelector"
									/>
								</view>
							</view>
						</view>
					</view>
					<view class="uni-user-header-box-right" @tap="onPrompt('userInfo')">
						<image :src="require('@/static/BIMAppUI/personalCenter/QRcode.png')" />
						<view class="iconfont icon-arrow-right info-icon" />
					</view>
				</view>
			</view>
			<view class="uni-user-info-list">
				<view class="uni-user-info-item" @tap="onPrompt('myInformation')">
					<view class="uni-user-info-item-left">
						<text style="padding: 0 10rpx"></text>
						<image :src="require('@/static/BIMAppUI/personalCenter/news.png')" />
						<view class="uni-user-info-item-title">我的信息</view>
					</view>
					<view class="iconfont icon-arrow-right info-icon" />
				</view>
				<view class="uni-user-info-item" @click="getScanning">
					<view class="uni-user-info-item-left">
						<text style="padding: 0 10rpx"></text>
						<image :src="require('@/static/BIMAppUI/personalCenter/scan.png')" />
						<view class="uni-user-info-item-title">扫一扫</view>
					</view>
					<view class="iconfont icon-arrow-right info-icon" />
				</view>
				<view class="uni-user-info-item" @tap="onPrompt('concerning')">
					<view class="uni-user-info-item-left">
						<text style="padding: 0 10rpx"></text>
						<image :src="require('@/static/BIMAppUI/toolBar/matter.png')" />
						<view class="uni-user-info-item-title">关于app</view>
					</view>
					<view class="iconfont icon-arrow-right info-icon" />
				</view>
				<view class="uni-user-info-item" @tap="onPrompt('help')">
					<view class="uni-user-info-item-left">
						<text style="padding: 0 10rpx"></text>
						<image :src="require('@/static/BIMAppUI/personalCenter/test.png')" />
						<view class="uni-user-info-item-title">帮助与反馈</view>
					</view>
					<view class="iconfont icon-arrow-right info-icon" />
				</view>
				<view class="uni-user-info-item" @tap="() => this.$refs.popup.open()">
					<view class="uni-user-info-item-left">
						<text style="padding: 0 10rpx"></text>
						<image :src="require('@/static/BIMAppUI/personalCenter/logOut.png')" />
						<view class="uni-user-info-item-title">退出</view>
					</view>
					<view class="iconfont icon-arrow-right info-icon" />
				</view>
				<uni-popup ref="popup" type="bottom">
					<view class="popup-button">
						<view @tap="onLogOut">退出登录</view>
						<view @tap="closeApp">关闭app</view>
						<view @tap="() => this.$refs.popup.close()">取消</view>
					</view>
				</uni-popup>
			</view>
		</view>
		<view class="pages-index-title">Copyright © 2020 陕西心像信息科技有限公司 | v{{ version }} {{ startupTime }}</view>
	</view>
</template>

<script>
import treeSelect from '@/components/treeSelect.vue';
import moment from 'moment';
export default {
	data() {
		return {
			currentOrganizations: [],
			userInfo: uni.getStorageSync('userInfo') || {},
			project: uni.getStorageSync('Project') || {}, //所选项目
			organization: uni.getStorageSync('organization') || {}, //所选组织
			version: '1.0.0',
			startupTime: '',
		};
	},
	components: {treeSelect},
	mounted() {
		this.startupTime = moment().format('YYYY-MM-DD');
		// #ifdef APP-PLUS
		this.version = plus.runtime.version;
		// #endif
		let this_ = this;
		uni.$on('#indexShow', function (show = false) {
			if (show) {
				//由index页面onShow判断页面是否已显示
				this_.userInfo = uni.getStorageSync('userInfo') || {};
				this_.project = uni.getStorageSync('Project') || {}; //所选项目
				this_.organization = uni.getStorageSync('organization') || {}; //所选组织
			}
		});
	},
	methods: {
		//切换项目
		onProject(value) {
			this.$store.dispatch('switchProject', value);
			uni.$emit('#qualitySafeShowCount', true);
		},
		//切换组织机构
		onSelector(value) {
			this.$store.dispatch('switchInitOrganization', value);
			uni.$emit('#qualitySafeShowCount', true);
		},

		//待开发提示
		onPrompt(key) {
			switch (key) {
				case 'userInfo':
					uni.navigateTo({
						url: '../../pages/user/userInfo',
					});
					break;
				case 'myInformation':
					let organizationName = '';
					organizationName = this.organization && this.organization.name ? this.organization.name : '--';
					uni.navigateTo({
						url: `../infoDisplay/userInfo?id=${this.$store.state.creatorId}&organizationsName=${organizationName}`,
					});
					break;
				case 'concerning':
					uni.navigateTo({
						url: '../../pages/user/concerning',
					});
					break;
				case 'help':
					uni.navigateTo({
						url: '../../pages/user/help',
					});
					break;
				default:
					uni.showToast({
						icon: 'none',
						title: '待开发中...',
						duration: 2000,
					});
					break;
			}
		},
		// 退出登录
		onLogOut() {
			this.$store.dispatch('logout');
			console.log('返回登录页');
			uni.reLaunch({
				url: '../login/login',
			});
		},
		// 关闭app
		closeApp() {
			// #ifdef APP-PLUS
			plus.runtime.quit();
			// #endif
		},
		// 扫码
		getScanning() {
			uni.scanCode({
				success: res => {
					//判断二维码类型
					let result = JSON.parse(res.result);
					console.log(result);
					if (result && result.key === 'equipment') {
						uni.navigateTo({
							url: `../equipmentInfo/equipmentInfo?qRCode=${result.value}`,
						});
						// } else if (result && result.key === 'user') {
						// 	uni.navigateTo({
						// 		url: `../infoDisplay/userInfo?id=${result.value}`,
						// 	});
					} else if (result && result.key === 'video') {
						uni.navigateTo({
							url: `../video/video?url=${result.value}`,
						});
					} else if (result && result.key === 'material') {
						uni.navigateTo({
							url: `../infoDisplay/materialInfo?id=${result.value}`,
						});
					} else {
						uni.showToast({
							icon: 'none',
							title: '扫描的二维码类型不正确',
							duration: 2000,
						});
					}
				},
				fail: err => {
					uni.showToast({
						icon: 'none',
						title: '信息获取失败',
						duration: 1000,
					});
				},
			});
		},
	},
};
</script>

<style lang="scss">
page {
	background-color: #0b2a57;
}

.uni-user-center {
	height: 100%;
	/* margin: 0 5px; */
	display: flex;
	flex-direction: column;
	justify-content: space-between;

	.tree-select {
		color: #212121;
	}
}
.uni-user-info {
	display: flex;
	flex-direction: column;
	font-size: 28rpx;
	color: #ffffff;
	padding-left: 30rpx;
	position: relative;
	> view:nth-child(1) {
		font-size: 40rpx;
		padding-bottom: 10rpx;
	}
	> view:nth-child(2) {
		color: #e4e4e4;
		font-size: 26rpx;
	}
	> view:nth-child(3) {
		position: absolute;
		bottom: -75rpx;
	}
}
.uni-user-info-list {
	margin: 40rpx 0;
	.popup-button {
		background-color: #f2f2f6;
		border-radius: 15rpx 15rpx 0 0;
		> view {
			padding: 30rpx;
			text-align: center;
			background-color: #ffffff;
		}
		> view:nth-child(1) {
			border-radius: 15rpx 15rpx 0 0;
			margin-bottom: 5rpx;
		}
		> view:nth-child(2) {
			margin-bottom: 20rpx;
		}
	}
}

.uni-user-header {
	height: 400rpx;
	font-size: 28rpx;
	color: #ffffff;
	display: flex;
	align-items: center;
	justify-content: center;
}

.uni-user-header-box {
	height: 80%;
	width: 100%;
	background-color: #153e7b;
	box-shadow: 0 0 20rpx #79554852;
	display: flex;
	justify-content: center;
	align-items: center;
	.uni-user-header-box-right {
		display: flex;
		align-items: center;
		padding-right: 24rpx;
		> image {
			width: 30rpx;
			height: 30rpx;
			padding-right: 10rpx;
		}
	}
}

.uni-user-header-info {
	display: flex;
	align-items: center;
	margin-left: 30rpx;
	flex: 2;
}

.uni-user-organization {
	flex: 1;
	display: flex;
}

.uni-user-info-item {
	display: flex;
	justify-content: space-between;
	align-items: center;
	padding: 24rpx;
	background-color: #153e7b;
	margin: 10rpx 0;
}

.uni-user-info-item-left {
	display: flex;
	align-items: center;
	justify-content: space-between;
}
.uni-user-info-item-left > image {
	width: 45rpx;
	height: 45rpx;
	border-left: 5rpx solid #ffffff;
	padding-left: 20rpx;
}

.uni-user-info-item-title {
	margin-left: 24rpx;
	color: #ffffff;
}

.info-icon {
	font-size: 50rpx;
	color: #ffffff;
}

.uni-user-avatar {
	height: 135rpx;
	width: 135rpx;
	border-radius: 50%;
	background-color: #348bd4;
	display: flex;
	align-items: center;
	justify-content: center;
	color: #ffffff;
}

.uni-user-avatar-item {
	height: 100rpx;
	width: 100rpx;
	border-radius: 50%;
	background-color: #2c557d;
	display: flex;
	justify-content: center;
	align-items: center;
}

//组织机构组件样式修改
.uni-user-center .uni-user-info ::v-deep .simple-value-box {
	padding: 0;
	border: 1rpx solid #e2e2e2;
	border-radius: 100rpx;
	height: auto;
	font-size: 24rpx;
	padding-left: 38rpx;
	.icon-Down {
		padding-right: 10rpx;
		padding-top: 5rpx;
	}
	.simple-selected-item {
		margin: 0;
		font-size: 24rpx;
		max-width: 165rpx;
		overflow: hidden;
		white-space: nowrap;
		text-overflow: ellipsis;
	}
	.tree-select-placeholder {
		padding: 4rpx 10rpx;
	}
}

.pages-index-title {
	width: 100vw;
	height: 114rpx;
	font-size: 20rpx;
	color: darkgray;
	text-align: center;
	position: absolute;
	bottom: 65rpx;
}
</style>
