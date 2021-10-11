<template>
	<view style="height: 85vh">
		<view class="uni-home" v-show="showPage == 'home'">
			<swiper
				:indicator-dots="true"
				indicator-color="#ebebeb"
				indicator-active-color="#c0c6c9"
				autoplay="true"
				interval="3000"
				circular="true"
				class="uni-home-swiper"
			>
				<swiper-item v-for="item in swipers" :key="item" class="uni-home-swiper-item">
					<image :src="item" style="width: 100%; height: 100%" />
				</swiper-item>
			</swiper>
			<view v-show="vP(getPermissions(), permissionsSmConstruction.GroupName)" class="uni-item">
				<text class="item-title">施工管理</text>
				<GridList :column="5" :gridList="technicalGridList" class="GridList" />
			</view>
			<view v-show="vP(getPermissions(), permissionsSmQuality.GroupName)" class="uni-item">
				<text class="item-title">质量管理</text>
				<GridList :column="5" :gridList="qualityGridList" safeOrQuality="quality" />
			</view>
			<view v-show="vP(getPermissions(), permissionsSmSafe.GroupName)" class="uni-item">
				<text class="item-title">安全管理</text>
				<GridList :column="5" :gridList="securityGridList" safeOrQuality="safe" />
			</view>
			<view v-if="!$appBIMGIS" v-show="vP(getPermissions(), permissionsSmCostManagement.GroupName)" class="uni-item">
				<text class="item-title">成本管理</text>
				<GridList :column="5" :gridList="costGridList" />
			</view>
			<view v-show="vP(getPermissions(), permissionsSmMaterial.GroupName)" class="uni-item">
				<text class="item-title">物资管理</text>
				<GridList :column="5" :gridList="materialList" />
			</view>
			<view v-if="!$appBIMGIS && !$packagingMode" v-show="vP(getPermissions(), permissionsSmFile.GroupName)" class="uni-item">
				<text class="item-title">文件管理</text>
				<GridList :column="5" :gridList="fileManage" />
			</view>
		</view>
		<!-- <matter ref="onShow" v-show="showPage == 'matter'" /> -->
		<user ref="onShow" v-show="showPage == 'user'" />

		<tabBar @change="onChange" />
		<view style="height: 114rpx"></view>
	</view>
</template>

<script>
import GridList from '@/components/gridList.vue';
import {requestIsSuccess, timeFix, getPermissions, vP} from '@/utils/util.js';
import {ModulesType, SafeQualityFilterType} from '@/utils/enum.js';
import memberSelect from '@/components/uniMemberSelect.vue';
import tabBar from '@/components/tabBar.vue';
import matter from '../matter/matter';
import user from '../user/user';
import * as apiQualityProblem from '@/api/quality/quality.js';
import * as apiSafeProblem from '@/api/safe/safeProblem.js';
import permissionsSmMaterial from '@/_permissions/sm-material.js';
import * as permissionsSmConstruction from '@/_permissions/sm-construction.js';
import permissionsSmQuality from '@/_permissions/sm-quality.js';
import permissionsSmSafe from '@/_permissions/sm-safe.js';
import permissionsSmTechnology from '@/_permissions/sm-technology.js';
import permissionsSmCostManagement from '@/_permissions/sm-costmanagement.js';
import permissionsSmFile from '@/_permissions/sm-file.js';

export default {
	components: {
		GridList,
		memberSelect,
		tabBar,
		matter,
		user,
	},
	data() {
		return {
			apiQualityProblem,
			apiSafeProblem,
			permissionsSmMaterial,
			permissionsSmConstruction,
			permissionsSmQuality,
			permissionsSmSafe,
			permissionsSmTechnology,
			permissionsSmCostManagement,
			permissionsSmFile,
			swipers: [
				require('@/static/BIMAppUI/cycleMap/a1.png'),
				require('@/static/BIMAppUI/cycleMap/a2.png'),
				require('@/static/BIMAppUI/cycleMap/a3.png'),
				require('@/static/BIMAppUI/cycleMap/a4.png'),
				require('@/static/BIMAppUI/cycleMap/a5.png'),
			],
			safeImproveCount: 0,
			safeVerifyCount: 0,
			qualityImproveCount: 0,
			qualityVerifyCount: 0,
			showPage: 'home', //当前显示的页面
			$appBIMGIS: this.$appBIMGIS, //app项目打包时选择项
		};
	},
	onLoad(val) {
		if (val.loginSuccessful) {
			// 登陆成功显示欢迎信息
			setTimeout(() => {
				uni.showToast({
					icon: 'none',
					title: `${timeFix()}，欢迎回来`,
					duration: 2000,
				});
			}, 500);
		}
		let this_ = this;
		uni.$on('#qualitySafeShowCount', async function (show = false) {
			if (show) this_.getShowCount();
		});
	},

	computed: {
		// 施工管理
		technicalGridList() {
			return [
				vP(getPermissions(), permissionsSmConstruction.Dispatch.Approval)
					? {
							type: ModulesType.ConstructionDispatch,
							url: require('@/static/BIMAppUI/functionalArea/constructionDispatch.png'),
					  }
					: null,
				vP(getPermissions(), permissionsSmConstruction.Dailys.Default)
					? {
							type: ModulesType.ConstructionDaily,
							url: require('@/static/BIMAppUI/functionalArea/constructionDaily.png'),
					  }
					: null,
				,
				vP(getPermissions(), permissionsSmConstruction.Dailys.Approve)
					? {
							type: ModulesType.ConstructionDailyApprove,
							url: require('@/static/BIMAppUI/functionalArea/constructionDailyApprove.png'),
					  }
					: null,
			];
		},
		//质量管理
		qualityGridList() {
			return [
				{
					type: ModulesType.Interface,
					url: require('@/static/BIMAppUI/functionalArea/interface.png'),
				},
				{
					type: ModulesType.QualityOfAll,
					url: require('@/static/BIMAppUI/functionalArea/all.png'),
				},
				{
					type: ModulesType.QualityOfChecked,
					url: require('@/static/BIMAppUI/functionalArea/myChecked.png'),
				},
				{
					type: ModulesType.QualityOfImproved,
					url: require('@/static/BIMAppUI/functionalArea/myWaitingImprove.png'),
					count: this.qualityImproveCount,
				},
				{
					type: ModulesType.QualityOfVerified,
					url: require('@/static/BIMAppUI/functionalArea/myWaitingVerify.png'),
					count: this.qualityVerifyCount,
				},
				{
					type: ModulesType.QualityOfSended,
					url: require('@/static/BIMAppUI/functionalArea/copyMine.png'),
				},
			];
		},
		//安全管理
		securityGridList() {
			return [
				{
					type: ModulesType.SecurityOfAll,
					url: require('@/static/BIMAppUI/functionalArea/all.png'),
				},
				{
					type: ModulesType.SecurityOfChecked,
					url: require('@/static/BIMAppUI/functionalArea/myChecked.png'),
				},
				{
					type: ModulesType.SecurityOfImproved,
					url: require('@/static/BIMAppUI/functionalArea/myWaitingImprove.png'),
					count: this.safeImproveCount,
				},
				{
					type: ModulesType.SecurityOfVerified,
					url: require('@/static/BIMAppUI/functionalArea/myWaitingVerify.png'),
					count: this.safeVerifyCount,
				},
				{
					type: ModulesType.SecurityOfSended,
					url: require('@/static/BIMAppUI/functionalArea/copyMine.png'),
				},
			];
		},
		//成本管理
		costGridList() {
			return [
				{
					type: ModulesType.Cost,
					url: require('@/static/BIMAppUI/functionalArea/cost.png'),
				},
				vP(getPermissions(), permissionsSmCostManagement.MoneyLists.Default)
					? {
							type: ModulesType.Capital,
							url: require('@/static/BIMAppUI/functionalArea/capital.png'),
					  }
					: null,
				vP(getPermissions(), permissionsSmCostManagement.Contracts.Default)
					? {
							type: ModulesType.Contract,
							url: require('@/static/BIMAppUI/functionalArea/contract.png'),
					  }
					: null,
				vP(getPermissions(), permissionsSmCostManagement.CostPeoples.Default)
					? {
							type: ModulesType.LaborCost,
							url: require('@/static/BIMAppUI/functionalArea/laborCost.png'),
					  }
					: null,
				vP(getPermissions(), permissionsSmCostManagement.CostOthers.Default)
					? {
							type: ModulesType.OtherCost,
							url: require('@/static/BIMAppUI/functionalArea/otherCost.png'),
					  }
					: null,
			];
		},
		//物资管理
		materialList() {
			return [
				vP(getPermissions(), permissionsSmMaterial.Acceptances.Default)
					? {
							type: ModulesType.MaterialAcceptance,
							url: require('@/static/BIMAppUI/functionalArea/materialAcceptance.png'),
					  }
					: null,
				vP(getPermissions(), permissionsSmMaterial.EntryRecords.Default)
					? {
							type: ModulesType.MaterialEntryRecords,
							url: require('@/static/BIMAppUI/functionalArea/interface.png'), //暂无图标
					  }
					: null,
				vP(getPermissions(), permissionsSmMaterial.OutRecords.Default)
					? {
							type: ModulesType.MaterialOutRecords,
							url: require('@/static/BIMAppUI/functionalArea/constructionDailyApprove.png'), //暂无图标
					  }
					: null,
				vP(getPermissions(), permissionsSmMaterial.Inventories.Default)
					? {
							type: ModulesType.MaterialInventory,
							url: require('@/static/BIMAppUI/functionalArea/materialInventory.png'),
					  }
					: null,
				vP(getPermissions(), permissionsSmMaterial.MaterialOfBill.Default)
					? {
							type: ModulesType.MaterialOfBill,
							url: require('@/static/BIMAppUI/functionalArea/materialOfBill.png'),
					  }
					: null,
			];
		},

		//文件管理
		fileManage() {
			return [
				vP(getPermissions(), permissionsSmFile.FileManager.Default)
					? {
							type: ModulesType.File,
							url: require('@/static/BIMAppUI/functionalArea/constructionDaily.png'),
					  }
					: null,
				vP(getPermissions(), permissionsSmFile.FileManager.Default)
					? {
							type: ModulesType.SharingCenter,
							url: require('@/static/BIMAppUI/functionalArea/copyMine.png'),
					  }
					: null,
				vP(getPermissions(), permissionsSmFile.FileManager.Default)
					? {
							type: ModulesType.RecycleBin,
							url: require('@/static/BIMAppUI/functionalArea/materialOfBill.png'),
					  }
					: null,
			];
		},
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
		if (!uni.getStorageSync('token')) {
			uni.navigateTo({
				url: '../login/login',
			});
		} else {
			this.getShowCount();
			uni.$emit('#indexShow', true);
		}
	},

	methods: {
		vP,
		getPermissions,
		//页面下拉刷新获取质量安全整改验证数据
		async getShowCount() {
			this.qualityImproveCount = await this.getCount('quality', SafeQualityFilterType.MyWaitingImprove);
			this.qualityVerifyCount = await this.getCount('quality', SafeQualityFilterType.MyWaitingVerify);
			this.safeImproveCount = await this.getCount('safe', SafeQualityFilterType.MyWaitingImprove);
			this.safeVerifyCount = await this.getCount('safe', SafeQualityFilterType.MyWaitingVerify);
		},

		//获取待整改、待验证数据
		async getCount(type, safeQualityFilterType) {
			let count = 0;
			let response;
			if (type === 'safe') {
				response = await apiSafeProblem.getList({
					isAll: true,
					filterType: safeQualityFilterType,
				});
			} else if (type === 'quality') {
				response = await apiQualityProblem.getList({
					isAll: true,
					filterType: safeQualityFilterType,
				});
			}
			if (response && requestIsSuccess(response)) {
				count = response.data.totalCount >= 99 ? '+99' : response.data.totalCount;
			}
			return count;
		},
		// 扫码
		getScanning() {
			uni.scanCode({
				success: res => {
					// console.log('人员测页试');
					// uni.navigateTo({
					// 	url: `../infoDisplay/userInfo?id=${'39f95756-fd73-cd27-e4ac-f9e49d711e2b'}`,
					// });
					// 判断二维码类型
					let result = JSON.parse(res.result);
					if (result && result.key === 'equipment') {
						uni.navigateTo({
							url: `../equipmentInfo/equipmentInfo?qRCode=${result.value}`,
						});
						// } else if (result && result.key === 'user') {
						// 	uni.navigateTo({
						// 		url: `../infoDisplay/userInfo?id=${result.value}`,
						// 	});
					} else if (result && result.key === 'video') {
						console.log(result);
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
		onChange(item) {
			switch (item.key) {
				case 'home':
					uni.setNavigationBarTitle({
						title: '铁路BIM施工运维系统',
					});
					this.showPage = item.key;
					break;
				case 'matter':
					uni.setNavigationBarTitle({
						title: '待办事项',
					});
					this.showPage = item.key;
					break;
				case 'scan':
					this.getScanning();
					break;
				case 'user':
					uni.setNavigationBarTitle({
						title: '个人中心',
					});
					this.showPage = item.key;
					break;
				default:
					break;
			}
		},
	},
};
</script>

<style>
page {
	background-color: #0e2a58;
}

.uni-home {
	background-color: #0e2a58;
	padding: 20rpx;
	color: #ffffff;
}

.uni-home .uni-item {
	padding: 16rpx 0;
}

.uni-home .item-title {
	font-size: 32rpx;
	font-weight: 600;
	font-family: fantasy;
	padding-left: 20rpx;
}

.uni-home ::v-deep .uni-technical-manage {
	background-color: #173e7c;
	margin: 20rpx 0;
	padding: 20rpx 0;
	border-radius: 20rpx;
}

.uni-home ::v-deep .grid-item-title {
	color: #ffffff;
	font-weight: initial;
}

.uni-home-swiper {
	height: 366rpx;
	margin: 10rpx;
	border-radius: 20rpx;
	overflow: hidden;
}

.uni-home-swiper-item {
	border-radius: 20rpx;
}
</style>
