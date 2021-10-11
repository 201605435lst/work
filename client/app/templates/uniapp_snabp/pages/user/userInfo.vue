<template>
	<!-- 个人信息页 -->
	<view id="pages-user-userInfo" class="pages-user-userInfo-class">
		<view class="userInfo-major">
			<view @tap="onPrompt('修改头像')">
				<text>头像</text>
				<view>
					<image :src="require('@/static/BIMAppUI/headPortrait.png')" style="width: 100rpx; height: 100rpx" />
					<view class="iconfont icon-arrow-right info-icon" :class="isModify ? '' : 'temporarily-hide'" />
				</view>
			</view>
			<view @tap="onPrompt('修改用户名')">
				<text>用户名</text>
				<view>
					<text>{{ userData.userName || '--' }}</text>
					<view class="iconfont icon-arrow-right info-icon" :class="isModify ? '' : 'temporarily-hide'" />
				</view>
			</view>
			<view @tap="onPrompt('name')">
				<text>姓名</text>
				<view>
					<text>{{ userData.name || '--' }}</text>
					<view class="iconfont icon-arrow-right info-icon" />
				</view>
			</view>
			<view @tap="onPrompt('phoneNumber')">
				<text>电话</text>
				<view>
					<text>{{ userData.phoneNumber || '--' }}</text>
					<view class="iconfont icon-arrow-right info-icon" />
				</view>
			</view>
			<view @tap="onPrompt('email')">
				<text>邮箱</text>
				<view>
					<text>{{ userData.email || '--' }}</text>
					<view class="iconfont icon-arrow-right info-icon" />
				</view>
			</view>
			<view @tap="onPrompt('修改职位')">
				<text>职位</text>
				<view>
					<text>{{ userData.position && userData.position.name ? userData.position.name : '--' }}</text>
					<view class="iconfont icon-arrow-right info-icon" :class="isModify ? '' : 'temporarily-hide'" />
				</view>
			</view>
			<view @tap="onPrompt('organization')">
				<text>组织机构</text>
				<view>
					<text>{{ organization && organization.name ? organization.name : '--' }}</text>
					<view class="iconfont icon-arrow-right info-icon" />
				</view>
			</view>
			<!-- <view @tap="onPrompt('弹出二维码名片')">
				<text>二维码名片</text>
				<view>
					<image :src="require('@/static/BIMAppUI/personalCenter/QRcode2.png')" style="width: 40rpx; height: 40rpx" />
					<view class="iconfont icon-arrow-right info-icon" :class="isModify ? '' : 'temporarily-hide'" />
				</view>
			</view> -->
			<treeSelect
				class="user-userInfo-treeSelect"
				ref="treeSelect"
				title="组织机构"
				placeholder="所属组织"
				:isSimple="true"
				v-model="organization"
				:isCurrentUserOrganizations="true"
				:lazy="true"
				action="/api/app/appOrganization/getList"
				@input="onSelector"
			/>
		</view>
		<uni-popup ref="popup" type="dialog">
			<uni-popup-dialog
				mode="input"
				:title="'设置' + titleVal"
				:placeholder="'请输入' + titleVal"
				:duration="2000"
				:before-close="true"
				:value="dialogVal"
				@close="
					() => {
						this.$refs.popup.close();
					}
				"
				@confirm="confirm"
			></uni-popup-dialog>
		</uni-popup>
		<uni-popup ref="popup_" type="message">
			<uni-popup-message type="success" message="设置成功" :duration="2000"></uni-popup-message>
		</uni-popup>
	</view>
</template>

<script>
import {requestIsSuccess} from '@/utils/util.js';
import treeSelect from '@/components/treeSelect.vue';
import * as apiSystem from '@/api/system.js';
export default {
	components: {treeSelect},
	data() {
		return {
			userData: uni.getStorageSync('userInfo') || {},
			organization: uni.getStorageSync('organization') || {},
			isModify: false, //是否可修改
			titleVal: '', //当前弹框标题值
			dialogVal: '',
			userIdData: '', //id获取的用户数据
		};
	},
	onLoad(args) {
		this.refresh();
	},
	methods: {
		async refresh() {
			// let userData = uni.getStorageSync('userInfo') || {};
			// if (userData && userData.id) {
			// 	const response = await apiSystem.get(userData.id);
			// 	if (requestIsSuccess(response)) {
			// 		console.log(response.data);
			// 		// this.userIdData=response.data;
			// 	}
			// }
		},
		onPrompt(key) {
			switch (key) {
				case 'name':
					this.titleVal = '姓名';
					this.dialogVal = this.userData[key];
					this.$refs.popup.open();
					break;
				case 'phoneNumber':
					this.titleVal = '电话';
					this.dialogVal = this.userData[key];
					this.$refs.popup.open();
					break;
				case 'email':
					this.titleVal = '邮箱';
					this.dialogVal = this.userData[key];
					this.$refs.popup.open();
					break;
				case 'organization':
					this.$refs.treeSelect.onOpen();
					break;
				default:
					// uni.showToast({
					// 	icon: 'none',
					// 	title: `当前不可${key}`,
					// 	duration: 2000,
					// });
					break;
			}
		},
		//切换组织机构
		onSelector(value) {
			this.$store.dispatch('switchInitOrganization', value);
			!value ? (this.organization = {}) : '';
		},

		confirm(value) {
			//暂改内存用户数据,后面修改数据库
			let userData = uni.getStorageSync('userInfo') || {};
			switch (this.titleVal) {
				case '姓名':
					userData.name = value;
					break;
				case '电话':
					userData.phoneNumber = value;
					break;
				case '邮箱':
					userData.email = value;
					break;
				default:
					break;
			}
			uni.setStorageSync('userInfo', userData);
			this.userData = uni.getStorageSync('userInfo') || {};

			this.infoUpdate(userData.id, userData);
			this.$refs.popup_.open();
			this.$refs.popup.close();
		},
		async infoUpdate(id, userData) {
			const response = await apiSystem.get_({
				id: id,
				organizationId: uni.getStorageSync('organization').id || '',
			});
			if (requestIsSuccess(response)) {
				let params = {
					...userData,
					organizationId: uni.getStorageSync('organization').id || '',
					concurrencyStamp: response.data.concurrencyStamp,
				};
				const response_ = await apiSystem.update(id, params);
				if (requestIsSuccess(response_)) {
					console.log(response_);
				}
			}
		},
	},
};
</script>

<style lang="scss">
page {
	background-color: #f2f2f6;
}
#pages-user-userInfo {
	.userInfo-major {
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
			.icon-arrow-right {
				font-size: 50rpx;
				color: #b2b2b2;
			}
			> view {
				display: flex;
				justify-content: space-between;
				align-items: center;
				color: #7f7f7f;
			}
		}
		.temporarily-hide {
			position: relative;
			z-index: -100;
		}
		.user-userInfo-treeSelect {
			position: absolute;
			top: -100vh;
		}
	}
}
</style>
