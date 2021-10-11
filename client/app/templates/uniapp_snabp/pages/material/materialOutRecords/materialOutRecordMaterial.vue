<!-- 安全问题标记页面 -->
<template>
	<view id="pages-uni-out-record-material">
		<uni-forms ref="form" :value="material" :rules="rules">
			<view style="margin: 10rpx 0">库存材料列表:</view>
			<view class="out-record-table-body">
				<view class="uni-table-column" v-for="(item, index1) in columns" :key="index1">
					<view class="out-record-table-column-title" :style="{width: item.hasOwnProperty('width') ? item.width : ''}">
						{{ item.title }}
					</view>
					<view
						class="out-record-table-column-content"
						:style="{width: item.hasOwnProperty('width') ? item.width : ''}"
						v-for="(_item, index2) in inventories"
						:key="index2"
						@click="onClick(_item)"
					>
						<view v-if="item.key === 'index'">
							<view v-if="_item.isSelected" class="iconfont icon-radio-checked"></view>
							<view v-else class="iconfont icon-radio-unchecked"></view>
						</view>
						<view v-else style="max-width: 140rpx">{{ _item.hasOwnProperty(item.key) ? _item[item.key] : '' }}</view>
					</view>
				</view>
			</view>

			<uni-forms-item label="出库数量" required name="count">
				<input
					class="uni-form-item-input"
					style="padding: 0 20rpx"
					type="number"
					v-model="material.count"
					@input="binddata('count', $event.detail.value)"
					placeholder="请输入出库数量"
					placeholder-style="color: #c3c1c1;"
				/>
			</uni-forms-item>

			<uni-forms-item label="备注" name="remark">
				<textarea
					class="pages-textarea"
					auto-height
					maxlength="-1"
					placeholder="请输入备注"
					placeholder-style="color: #c3c1c1; z-index:0"
					v-model="material.remark"
					@input="binddata('remark', $event.target.value)"
				/>
			</uni-forms-item>
			<view class="save-button">
				<button @tap="submit">确定</button>
			</view>
		</uni-forms>
	</view>
</template>

<script>
import {requestIsSuccess} from '@/utils/util.js';
import uniPicker from '@/components/uniPicker.vue';
import * as apiInventory from '@/api/material/inventory.js';

export default {
	name: 'materialEntryRecord',
	components: {uniPicker},
	data() {
		return {
			inventories: [],
			materialId: null,
			material: {
				remark: '',
				count: '',
			},
			rules: {
				count: {
					rules: [
						{
							required: true,
							errorMessage: '请输入出库数量',
						},
					],
				},
			},
		};
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},
	computed: {
		columns() {
			return [
				{title: '#', key: 'index'},
				{title: '名称', key: 'name'},
				{title: '规格', key: 'spec'},
				{title: '库存地点', key: 'partitionName'},
				{title: '库存量', key: 'amount'},
				{title: '价格', key: 'price'},
				{title: '供应商', key: 'supplierName'},
			];
		},
	},

	onLoad(option) {
		let inventories = JSON.parse(option.data);
		// if (this.materialId) {
		// 	this.getInventories();
		// }
		this.inventories = inventories.map(_item_ => {
			return {
				..._item_,
				partitionName: _item_.partition && _item_.partition.name ? _item_.partition.name : '--',
			};
		});
		console.log(this.inventories);
	},

	methods: {
		onClick(item) {
			this.inventories.map(_item => {
				if (_item.isSelected) {
					_item.isSelected = false;
				}
			});
			item.isSelected = !item.isSelected;
		},

		//获取项目数据
		// async getInventories() {
		// 	let response = await apiInventory.getListByMaterialId(this.materialId);
		// 	if (requestIsSuccess(response) && response.data && response.data.length > 0) {
		// 		this.inventories = response.data.map(item => {
		// 			return {
		// 				...item,
		// 				supplierName: item.supplier ? item.supplier.name : '',
		// 				name: item.material ? item.material.name : '',
		// 				spec: item.material ? item.material.spec : '',
		// 				isSelected: false
		// 			};
		// 		});
		// 		if (this.inventories.length == 0) {
		// 			uni.navigateBack();
		// 			uni.showToast({
		// 				icon: 'none',
		// 				title: '当前材料暂未入库'
		// 			});
		// 		}
		// 	}
		// },

		submit() {
			let _this = this;
			this.$refs.form
				.submit()
				.then(async res => {
					let material = this.inventories.find(item => item.isSelected);
					if (material) {
						if (res.count <= 0) {
							uni.showToast({
								icon: 'none',
								title: '出库数量不能小于等于0',
							});
							return;
						}
						if (res.count > material.amount) {
							uni.showToast({
								icon: 'none',
								title: '出库数量不能大于库存数量',
							});
							return;
						}
						uni.$emit('addMaterial', {
							...res,
							...material,
						});

						uni.navigateBack();
					} else {
						uni.showToast({
							icon: 'none',
							title: '出库数量不能小于等于0',
						});
						return;
					}
				})
				.catch(err => {
					uni.showToast({
						icon: 'none',
						title: err,
					});
				});
		},
	},
};
</script>

<style>
page {
	background-color: #f9f9f9;
}
</style>
<style lang="scss">
#pages-uni-out-record-material {
	padding: 20rpx;
	background-color: #f9f9f9;
	margin-bottom: 120rpx;
	.uni-form-item {
		display: flex;
		height: 80rpx;
		align-items: center;
		justify-content: space-between;
		padding: 0 10rpx;
	}

	.uni-form-item-input {
		font-size: 28rpx;
		flex: 1;
		background-color: #ffffff;
		height: 70rpx;
		border-radius: 10rpx;
		display: flex;
		align-items: center;
	}

	.out-record-table-body {
		display: flex;
		justify-content: space-around;
		margin-bottom: 20rpx;
	}

	.out-record-table-column-title {
		white-space: inherit;
		overflow: hidden;
		text-overflow: ellipsis;
		padding: 10rpx;
		color: #696969;
		font-size: 24rpx;
		font-weight: 600;
		text-align: center;
	}

	.out-record-table-column-content {
		padding: 10rpx;
		height: 46rpx;
		display: flex;
		align-items: center;
		justify-content: center;
		color: #757575;
		font-size: 24rpx;
	}

	.uni-entry-record-item {
		flex: 1;
		height: 70rpx;
		background-color: #ffffff;
		border-radius: 10rpx;
	}

	.icon-radio-unchecked {
		font-size: 42rpx;
		color: #d6d6d6;
	}

	.icon-radio-checked {
		font-size: 42rpx;
		color: #1890ff;
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
