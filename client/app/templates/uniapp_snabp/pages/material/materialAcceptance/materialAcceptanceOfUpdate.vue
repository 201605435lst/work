<template>
	<view id="pages-materialAcceptanceOfUpdate">
		<!-- 加载中 -->
		<uniLoading v-show="loading" class="uniLoading" />
		<!-- 基本信息 -->
		<view v-show="!loading" class="quality-details-information">
			<uni-row class="basic-row" :gutter="16">
				<uni-col :span="24">
					<view class="basic-col">
						<view class="basic-col-title">报告编号：</view>
						{{ formData.code || '--' }}
					</view>
				</uni-col>
				<uni-col :span="24">
					<view class="basic-col">
						<view class="basic-col-title">报告日期：</view>
						{{ formData.receptionTime || '--' }}
					</view>
				</uni-col>
				<uni-col :span="24">
					<view class="basic-col">
						<view class="basic-col-title">检测机构：</view>
						{{ formData.testingOrganization ? formData.testingOrganization.name : '--' }}
					</view>
				</uni-col>
				<uni-col :span="24">
					<view class="basic-col">
						<view class="basic-col-title">检测类型：</view>
						{{ getMaterialTestingType(formData.testingType) || '--' }}
					</view>
				</uni-col>
				<uni-col :span="24">
					<view class="basic-col">
						<view class="basic-col-title">备注：</view>
						{{ formData.remark || '--' }}
					</view>
				</uni-col>
			</uni-row>
			<view>
				<view class="acceptance-add"><view>材料列表：</view></view>
				<wyb-table :headers="materialColumns" :contents="materials" />
			</view>
			<view>
				<view class="acceptance-add">
					<view>跟踪构件:</view>
					<view class="acceptance-add-button">
						<text>扫码添加</text>
						<view class="iconfont icon-scan1" @tap="getScanning" />
					</view>
				</view>
				<wyb-table :headers="componentColumns" :contents="components" @onCellClick="onCellClick" />
			</view>
			<view class="save-button">
				<button @click="submit">提交</button>
			</view>
		</view>
	</view>
</template>

<script>
import {requestIsSuccess, getMaterialTestingType, getMaterialTestState, showToast} from '@/utils/util.js';
import * as apiMaterialAcceptance from '@/api/material/materialAcceptance.js';
import * as apiComponentCategory from '@/api/stdBasic/componentCategory.js';
import moment from 'moment';
export default {
	data() {
		return {
			loading: true,
			dataSources: [],
			formData: {},
			materials: [],
			components: [],
		};
	},
	computed: {
		materialColumns() {
			return [
				{label: '名称', key: 'name'},
				{label: '型号/规格', key: 'spec'},
				{label: '类别', key: 'typeName'},
				{label: '数量', key: 'number'},
				{label: '检查结果', key: 'testState'},
			];
		},
		componentColumns() {
			return [
				{label: '构件名称', key: 'name'},
				{label: '单位', key: 'unit'},
				{label: '操作', key: 'wyb-delete'},
			];
		},
	},
	onLoad(args) {
		this.refresh(args.id);
	},
	methods: {
		getMaterialTestingType,
		async refresh(id) {
			const response = await apiMaterialAcceptance.get(id);
			if (requestIsSuccess(response)) {
				let _response = {
					...response.data,
					receptionTime: moment(response.data.receptionTime).format('YYYY-MM-DD'),
				};
				this.formData = _response;
				this.materials =
					_response.materialAcceptanceRltMaterials && _response.materialAcceptanceRltMaterials.length > 0
						? _response.materialAcceptanceRltMaterials.map(item => {
								return {
									materialId: item.materialId,
									typeName: item.material && item.material.type ? item.material.type.name : '',
									name: item.material ? item.material.name : '',
									spec: item.material ? item.material.spec : '',
									number: item.number,
									testState: getMaterialTestState(item.testState),
								};
						  })
						: [];
				this.components =
					_response.materialAcceptanceRltQRCodes && _response.materialAcceptanceRltQRCodes.length > 0
						? _response.materialAcceptanceRltQRCodes.map(item => {
								return {
									qrCode: item.qrCode,
									name: item.componentCategory && item.componentCategory.name ? item.componentCategory.name : '',
									unit: item.componentCategory && item.componentCategory.unit ? item.componentCategory.unit : '',
								};
						  })
						: [];
				this.loading = false;
			}
		},
		async submit() {
			if (this.components && this.components.length >= 0) {
				let submitData = {
					...this.formData,
					materialAcceptanceRltQRCodes: [...this.components],
				};
				if (!this.$store.state.isSubmit) {
					this.$store.commit('SetIsSubmit', true);
					let response = await apiMaterialAcceptance.update(submitData);
					if (requestIsSuccess(response)) {
						showToast('提交成功');
					} else {
						showToast('提交失败', false);
					}
				}
			}
		},
		// 扫码跟踪构件
		getScanning() {
			let _this = this;
			uni.scanCode({
				success: async res => {
					//判断二维码类型
					let result = JSON.parse(res.result);
					if (result && result.key == 'equipment' && result.value) {
						let qrCode = result.value;
						if (!_this.components.find(item => item.qrCode === qrCode)) {
							let array = qrCode.split('@');
							if (array.length >= 2 && array[0]) {
								let response = await apiComponentCategory.getByCode(array[0]);
								if (requestIsSuccess(response) && response.data) {
									let target = {
										qrCode: qrCode,
										name: response.data && response.data.name ? response.data.name : '',
										unit: response.data && response.data.unit ? response.data.unit : '',
									};
									_this.components.push(target);
								}
							}
						} else {
							showToast('材料已添加', false);
						}
					} else {
						showToast('扫描的二维码类型不正确', false);
					}
				},
				fail: err => {
					showToast('信息获取失败', false);
				},
			});
		},
		//表格操作
		onCellClick(e) {
			if (!e.checkType && e.checkType != 'multiple' && e.key == 'wyb-delete') {
				this.components.splice(e.contentIndex, 1);
			}
		},
	},
};
</script>
>

<style lang="scss">
page {
	background-color: #f9f9f9;
}
#pages-materialAcceptanceOfUpdate ::v-deep .uni-col {
	padding: 10rpx;
}
#pages-materialAcceptanceOfUpdate {
	font-size: 28rpx;
	color: #656565;
	padding: 20rpx;
	margin-bottom: 120rpx;
	.basic-col {
		display: flex;
		padding-bottom: 20rpx;
		white-space: nowrap;
		text-overflow: ellipsis;
		overflow: hidden;
	}

	.basic-col-title {
		width: 150rpx;
		color: #1a1a1a;
		font-weight: 600;
	}
	.acceptance-add {
		display: flex;
		justify-content: space-between;
		align-items: center;
		margin: 40rpx 0 20rpx 0;
	}

	.acceptance-add-button {
		display: flex;
		align-items: center;
	}

	.icon-scan1 {
		font-size: 50rpx;
		color: #1890ff;
		margin-left: 20rpx;
	}
	.save-button {
		width: 100%;
		display: flex;
		position: fixed;
		z-index: 90;
		bottom: 0;
		left: 0;
		background-color: #f9f9f9;
		padding: 20rpx 0;
	}
	.save-button > button {
		color: #ffffff;
		width: 100%;
		border-radius: 100rpx;
		margin: 0 20rpx;
		background-color: #1890ff;
	}
}
</style>
