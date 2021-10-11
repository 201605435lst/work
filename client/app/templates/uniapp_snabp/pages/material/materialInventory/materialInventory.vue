<template>
	<view class="uni-inventory">
		<view class="uni-inventory-body" v-show="!loading">
			<view class="uni-inventory-item">
				<view class="uni-inventory-item-title">物资类别：</view>
				<view class="uni-inventory-item-content">
					{{ inventory && inventory.material && inventory.material.type ? inventory.material.type.name : '无关联信息' }}
				</view>
			</view>
			<view class="uni-inventory-item">
				<view class="uni-inventory-item-title">材料名称：</view>
				<view class="uni-inventory-item-content">
					{{ inventory && inventory.material ? inventory.material.name : '无关联信息' }}
				</view>
			</view>
			<view class="uni-inventory-item">
				<view class="uni-inventory-item-title">规格型号：</view>
				<view class="uni-inventory-item-content">
					{{ inventory && inventory.material ? inventory.material.spec : '无关联信息' }}
				</view>
			</view>
			<view class="uni-inventory-item">
				<view class="uni-inventory-item-title">库存数量：</view>
				<view class="uni-inventory-item-content">{{ inventory ? inventory.amount : '' }}</view>
			</view>
			<view class="uni-inventory-item">
				<view class="uni-inventory-item-title">库存位置：</view>
				<view class="uni-inventory-item-content">
					{{ inventory && inventory.partition ? inventory.partition.name : '无关联信息' }}
				</view>
			</view>
			<view class="uni-inventory-item">
				<view class="uni-inventory-item-title">供应商：</view>
				<view class="uni-inventory-item-content">
					{{ inventory && inventory.supplier ? inventory.supplier.name : '无关联信息' }}
				</view>
			</view>
			<view class="uni-inventory-item">
				<view class="uni-inventory-item-title">登记时间：</view>
				<view class="uni-inventory-item-content">
					{{ inventory && inventory.entryTime ? moment(inventory.entryTime).format('YYYY-MM-DD hh:mm:ss') : '' }}
				</view>
			</view>
			<!-- <view class="uni-inventory-item">
				<view class="uni-inventory-item-title">附件上传：</view>
				<pickerImage
					v-if="inventory && inventory.files && inventory.files.length > 0"
					class="uni-inventory-item-content"
					url="http://localhost:8091/api/app/fileFile/uploadForApp"
					:value="inventory.files"
					:disabled="true"
				/>
				<view v-else class="uni-inventory-item-content">暂无附件信息</view>
			</view> -->

			<view class="segmented-control">
				<uni-segmented-control
					style="font-size: 28rpx"
					:current="current"
					:values="recordInfo"
					@clickItem="onChangeRecordInfo"
					styleType="text"
					activeColor="#1890ff"
				></uni-segmented-control>
				<view class="content">
					<view v-show="current === InventoryRecordType.Entry">
						<wyb-table :headers="entryColumns" :contents="entryDatas" />
					</view>
					<view v-show="current === InventoryRecordType.Out">
						<wyb-table :headers="outColumns" :contents="outRecords" />
					</view>
				</view>
			</view>
		</view>
		<uniLoading v-show="loading" style="height: 100%; display: flex; align-items: center; justify-content: center" />
	</view>
</template>

<script>
import {checkToken, requestIsSuccess, getInventoryRecordTypeTitle} from '@/utils/util.js';
import {InventoryRecordType} from '@/utils/enum.js';
import pickerImage from '@/components/pickerImage.vue';
import * as apiMaterialInventory from '@/api/material/inventory.js';
import moment from 'moment';

export default {
	name: 'materialInventory',
	components: {
		pickerImage,
	},
	data() {
		return {
			InventoryRecordType,
			loading: false,
			id: null,
			inventory: {},
			entryDatas: [], //入库记录
			outRecords: [], //出库记录
			current: InventoryRecordType.Entry, //默认入库
			recordInfo: ['入库记录', '出库记录'],
			QRCode: [],
		};
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},

	computed: {
		entryColumns() {
			return [
				{
					label: '序号',
					key: 'index',
				},
				{
					label: '入库量',
					key: 'count',
				},
				{
					label: '入库时间',
					key: 'time',
				},
				{
					label: '登记人',
					key: 'creator',
				},
			];
		},

		outColumns() {
			return [
				{
					label: '序号',
					key: 'index',
				},
				{
					label: '出库量',
					key: 'count',
				},
				{
					label: '出库时间',
					key: 'time',
				},
				{
					label: '登记人',
					key: 'creator',
				},
			];
		},
	},

	onLoad(option) {
		this.id = option.id;
		this.refresh();
	},

	methods: {
		getInventoryRecordTypeTitle,
		moment,
		async refresh() {
			if (!this.id) return;
			this.loading = true;
			const response = await apiMaterialInventory.get(this.id);
			if (requestIsSuccess(response) && response.data) {
				this.inventory = response.data;
				this.entryDatas =
					response.data.entryRecords && response.data.entryRecords.length > 0
						? response.data.entryRecords.map((item, index) => {
								return {
									index: index + 1,
									count: item.count ? item.count : '',
									time:
										item.entryRecord && item.entryRecord.time ? moment(item.entryRecord.time).format('YYYY-MM-DD') : '',
									creator: item.entryRecord && item.entryRecord.creator ? item.entryRecord.creator.name : '',
								};
						  })
						: [];
				this.outRecords =
					response.data.outRecords && response.data.outRecords.length > 0
						? response.data.outRecords.map((item, index) => {
								return {
									index: index + 1,
									count: item.count ? item.count : '',
									time: item.outRecord.time ? moment(item.outRecord.time).format('YYYY-MM-DD') : '',
									creator: item.outRecord && item.outRecord.creator ? item.outRecord.creator.name : '',
								};
						  })
						: [];
				this.loading = false;
			}
		},

		//出入库记录信息tab页切换
		onChangeRecordInfo(event) {
			this.current = event.currentIndex;
		},
	},
};
</script>

<style>
page {
	background-color: #f9f9f9;
}

.uni-inventory {
	font-size: 28rpx;
}

.uni-inventory-body {
	padding: 20rpx;
	height: 100%;
}

.uni-inventory-item {
	display: flex;
	height: 60rpx;
	align-items: center;
	justify-content: space-between;
}

.uni-inventory-item-title {
	width: 150rpx;
	color: #1a1a1a;
	font-weight: 600;
	text-align-last: justify;
}

.uni-inventory-item-content {
	flex: 1;
	color: #949494;
}

.segmented-control {
	margin-top: 20rpx;
}

.segmented-control__text.data-v-064e9cd1 {
	font-size: 28rpx !important;
}
.uni-buttons {
	display: flex;
	justify-content: flex-end;
	width: 100%;
	position: fixed;
	bottom: 0;
	z-index: 90;
	background-color: #f9f9f9;
}
.footer-cancel-button {
	font-size: 28rpx;
	margin: 10rpx 0;
	height: 60rpx;
	display: flex;
	align-items: center;
	color: #1890ff;
}
</style>
