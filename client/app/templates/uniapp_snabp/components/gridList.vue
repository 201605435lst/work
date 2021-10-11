<template>
	<view class="uni-technical-manage">
		<uni-grid :column="column" :showBorder="false">
			<block v-for="(item, index) in gridList" :key="index">
				<uni-grid-item v-if="item" :key="item.index">
					<view class="grid-item" @click="onClick(item)">
						<view class="badge-count" :class="column > 4 ? 'badge-count_' : ''" v-if="item.count && item.count > 0">
							{{ item.count }}
						</view>
						<view class="grid-item-image"><image class="image-body" :src="item.url" mode="aspectFill"></image></view>
						<view class="grid-item-title">{{ getModulesTypeTitle(item.type) }}</view>
					</view>
				</uni-grid-item>
			</block>
		</uni-grid>
	</view>
</template>

<script>
import {ModulesType, SafeQualityFilterType} from '../utils/enum.js';
import {getModulesTypeTitle, requestIsSuccess} from '../utils/util.js';

export default {
	name: 'gridList',
	props: {
		gridList: {
			type: Array,
			default: () => [],
		},
		column: {
			type: Number,
			default: 3,
		},
		safeOrQuality: {
			type: String,
			default: '',
		},
	},
	data() {
		return {
			ModulesType,
			SafeQualityFilterType,
		};
	},

	methods: {
		getModulesTypeTitle,
		onClick(item) {
			uni.navigateTo({
				url: this.onModuleChange(item.type),
			});
		},

		//判断模块类型进行页面跳转
		onModuleChange(status) {
			let url = '';
			switch (status) {
				// 施工管理
				case ModulesType.ConstructionDispatch:
					url = '../constructionDispatch/constructionDispatchs?dailyApprove=false';
					break;
				case ModulesType.ConstructionDaily:
					url = `../constructionDaily/constructionDailies?approval=false`;
					break;
				case ModulesType.ConstructionDailyApprove:
					url = '../constructionDispatch/constructionDispatchs?dailyApprove=true';
					break;

				//质量管理
				case ModulesType.Interface:
					url = '../quality/interface/interfaceListings';
					break;
				case ModulesType.QualityOfAll:
					url = `../quality/qualityOfAll?type=${SafeQualityFilterType.All}`;
					break;
				case ModulesType.QualityOfChecked:
					url = `../quality/qualityOfAll?type=${SafeQualityFilterType.MyChecked}`;
					break;
				case ModulesType.QualityOfImproved:
					url = `../quality/qualityOfAll?type=${SafeQualityFilterType.MyWaitingImprove}`;
					break;
				case ModulesType.QualityOfVerified:
					url = `../quality/qualityOfAll?type=${SafeQualityFilterType.MyWaitingVerify}`;
					break;
				case ModulesType.QualityOfSended:
					url = `../quality/qualityOfAll?type=${SafeQualityFilterType.CopyMine}`;
					break;

				//安全管理
				case ModulesType.SecurityOfAll:
					url = `../security/securityOfAll?type=${SafeQualityFilterType.All}`;
					break;
				case ModulesType.SecurityOfChecked:
					url = `../security/securityOfAll?type=${SafeQualityFilterType.MyChecked}`;
					break;
				case ModulesType.SecurityOfImproved:
					url = `../security/securityOfAll?type=${SafeQualityFilterType.MyWaitingImprove}`;
					break;
				case ModulesType.SecurityOfVerified:
					url = `../security/securityOfAll?type=${SafeQualityFilterType.MyWaitingVerify}`;
					break;
				case ModulesType.SecurityOfSended:
					url = `../security/securityOfAll?type=${SafeQualityFilterType.CopyMine}`;
					break;

				//成本管理
				case ModulesType.Cost:
					url = '../cost/cost';
					break;
				case ModulesType.Capital:
					uni.navigateTo({
						url: '../cost/moneyManagement/moneyManagement',
					});
					break;
				case ModulesType.Contract:
					url = '../cost/contracts/contracts';
					break;
				case ModulesType.LaborCost:
					url = '../cost/laborCost/laborCosts';
					break;
				case ModulesType.OtherCost:
					url = '../cost/otherCost/otherCosts';
					break;

				//物资管理
				case ModulesType.MaterialOfBill:
					url = '../material/materialOfBill/materialOfBills';
					break;
				case ModulesType.MaterialAcceptance:
					url = '../material/materialAcceptance/materialAcceptances';
					break;
				case ModulesType.MaterialEntryRecords:
					url = '../material/materialEntryRecords/materialEntryRecords';
					break;
				case ModulesType.MaterialOutRecords:
					url = '../material/materialOutRecords/materialOutRecords';
					break;
				case ModulesType.MaterialInventory:
					url = '../material/materialInventory/materialInventories';
					break;

				//文件管理
				case ModulesType.File:
					url = '../fileManage/file/file';
					break;
				case ModulesType.SharingCenter:
					url = '../fileManage/sharingCenter/sharingCenter';
					break;
				case ModulesType.RecycleBin:
					url = '../fileManage/recycleBin/recycleBin';
					break;
				default:
					break;
			}
			return url;
		},
	},
};
</script>

<style>
.uni-search {
	display: flex;
	align-items: center;
	margin: 10rpx;
	height: 80rpx;
	background-color: #f7f7f7;
	border-radius: 10rpx;
}

.badge-count {
	position: absolute;
	height: 40rpx;
	width: 40rpx;
	border-radius: 50%;
	background-color: #ef0000;
	color: #ffffff;
	display: flex;
	justify-content: center;
	align-items: center;
	font-size: 20rpx;
	top: 5%;
	right: 15%;
	z-index: 50;
}
.badge-count_ {
	height: 30rpx;
	width: 30rpx;
}

.search-icon {
	margin: 0 20rpx;
	width: 28rpx;
}

.uni-input {
	font-size: 28rpx;
	flex: 1;
}

.uni-radio {
	margin: 0 20rpx;
}

.uni-all {
	display: flex;
	align-items: center;
	background-color: #f7f7f7;
	height: 80rpx;
	font-size: 56rpx;
	color: #5a5a5a;
	margin: 10rpx;
	border-radius: 10rpx;
}

.grid-item {
	height: 100%;
	display: flex;
	flex-direction: column;
	align-items: center;
	justify-content: center;
	position: relative;
}
.grid-item:active {
	background-color: #173e7c;
}
.grid-item-image {
	display: flex;
	align-items: center;
	justify-content: center;
	border-radius: 50%;
	overflow: hidden;
	width: 50%;
	height: 50%;
}

.grid-item-title {
	margin-top: 10rpx;
	font-size: 28rpx;
	font-family: fantasy;
	font-weight: 600;
	color: #7d7d7d;
}

.image-body {
	height: 100%;
}
</style>
