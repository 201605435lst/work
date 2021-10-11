<template>
	<view class="uni-user-info-panel">
		<view class="uni-user-info-panel-title">
			<text>铁路BIM施工运维系统</text>
			<text style="font-size: 30rpx">用户信息</text>
		</view>
		<view class="uni-user-info-panel-content">
			<view class="uni-user-info-box">
				<block v-for="(item, index) of userInfo" :key="index">
					<view class="uni-user-info-item">
						<view class="uni-user-info-left">{{ item.title }}</view>
						<view class="uni-user-info-right">{{ item.val }}</view>
					</view>
				</block>
			</view>
		</view>
	</view>
</template>

<script>
import {requestIsSuccess} from '@/utils/util.js';
import * as apiSystem from '@/api/system.js';
export default {
	data() {
		return {
			userData: {},
			organizationsName: '',
		};
	},
	computed: {
		userInfo() {
			return [
				{title: '姓名', val: this.userData.name || '--'},
				{title: '电话', val: this.userData.phoneNumber || '--'},
				{title: '邮箱', val: this.userData.email || '--'},
				{title: '职位', val: this.userData.positionName || '--'},
				{title: '组织机构', val: this.organizationsName ? this.organizationsName : this.userData.organizationsName},
			];
		},
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},
	onLoad(option) {
		if (option.organizationsName) {
			this.organizationsName = option.organizationsName;
		}
		this.refresh(option.id);
	},
	methods: {
		async refresh(id) {
			let response = await apiSystem.get(id);
			let response_ = await apiSystem.getCurrentUserOrganizations(id);
			if (requestIsSuccess(response)) {
				let organizationsName = [];
				if (requestIsSuccess(response_)) {
					organizationsName = response_.data;
				}
				let resData = response.data;
				let userData = {
					...resData,
					positionName: resData.position && resData.position.name ? resData.position.name : '--',
					organizationsName: organizationsName,
					organizationsName:
						organizationsName && organizationsName.length > 0
							? organizationsName.map(item => (item.name ? item.name : '--')).join('/')
							: '--',
				};
				this.userData = userData;
			}
		},
	},
};
</script>

<style lang="scss">
page {
	background-color: #f2f2f6;
}
.uni-user-info-panel {
	height: 100%;
	display: flex;
	flex-direction: column;
	justify-content: center;
	align-items: center;
}

.uni-user-info-panel-title {
	display: flex;
	flex-direction: column;
	justify-content: center;
	align-items: center;
	font-size: 36rpx;
	color: #565555;
	font-weight: 400;
	flex: 1;
}

.uni-user-info-panel-content {
	flex: 3;
	width: 90%;
}

.uni-user-info-box {
	font-size: 28rpx;
	box-shadow: 0 0 6rpx #ff9800;
	> view:nth-last-child(1) {
		border-bottom: 0;
	}
}

.uni-user-info-item {
	display: flex;
	border-bottom: solid 1rpx #e6e4e4;
	height: 60rpx;
	align-items: center;
}

.uni-user-info-left {
	width: 160rpx;
	border-right: solid 1rpx #e6e4e4;
	height: 100%;
	display: flex;
	align-items: center;
	justify-content: center;
	font-weight: 600;
	color: #676767;
}

.uni-user-info-right {
	flex: 1;
	height: 100%;
	display: flex;
	align-items: center;
	padding-left: 10rpx;
	color: #676767;
	overflow: auto;
	white-space: nowrap;
}
</style>
