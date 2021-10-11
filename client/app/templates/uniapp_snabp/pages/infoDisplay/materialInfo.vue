<template>
	<view class="uni-user-info-panel">
		<view class="uni-user-info-panel-title">
			<text>铁路BIM施工运维系统</text>
			<text style="font-size: 30rpx">材料信息</text>
		</view>
		<view class="uni-user-info-panel-content">
			<view class="uni-user-info-box">
				<block v-for="(item, index) of materialInfo" :key="index">
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
import * as apiMaterial from '@/api/material/material.js';
import * as apiSystem from '@/api/system.js';
export default {
	data() {
		return {
			materialData: {},
			organizationsName: '',
		};
	},
	computed: {
		materialInfo() {
			return [
				{title: '材料名称', val: this.materialData.name || '--'},
				{title: '材料类别', val: this.materialData.typeName || '--'},
				{title: '规格型号', val: this.materialData.spec || '--'},
				{title: '计量单位', val: this.materialData.unit || '--'},
				{title: '材料价格', val: this.materialData.price || '--'},
				{title: '备注', val: this.materialData.remark || '--'},
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
			const response = await apiMaterial.get(id);
			const response_ = await apiSystem.getValues({groupCode: 'MaterialType'});
			if (requestIsSuccess(response)) {
				let materialData = response.data;
				if (requestIsSuccess(response_)) {
					let types = response_.data,
						typeName = '--';
					for (const item of types) {
						if (item.id == materialData.typeId) {
							typeName = item.name;
						}
					}
					this.materialData = {
						...materialData,
						typeName: typeName,
					};
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
