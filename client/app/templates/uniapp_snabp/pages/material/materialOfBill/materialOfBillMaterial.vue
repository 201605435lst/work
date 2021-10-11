<template>
	<view id="pages-uni-entry-record-material">
		<uni-forms ref="form" :value="material" :rules="rules">
			<uni-forms-item label="材料名称" name="name">
				<input class="uni-form-item-input" placeholder="--" style="padding: 0 20rpx" disabled v-model="material.name" />
			</uni-forms-item>
			<uni-forms-item label="规格型号" name="spec">
				<input class="uni-form-item-input" placeholder="--" style="padding: 0 20rpx" disabled v-model="material.spec" />
			</uni-forms-item>
			<uni-forms-item label="库存位置" required name="partition">
				<uniPicker
					mode="selector"
					placeholder="请选择库存位置"
					:value="material.partitionName"
					:range="partitionRange"
					rangeKey="partitionName"
					@input="onPartition"
				/>
			</uni-forms-item>
			<uni-forms-item label="单位" name="unit">
				<input class="uni-form-item-input" placeholder="--" style="padding: 0 20rpx" disabled v-model="material.unit" />
			</uni-forms-item>
			<uni-forms-item label="库存数量" name="amount">
				<input class="uni-form-item-input" placeholder="--" style="padding: 0 20rpx" disabled v-model="material.amount" />
			</uni-forms-item>
			<uni-forms-item label="领取数量" required name="count">
				<input
					class="uni-form-item-input"
					style="padding: 0 20rpx"
					type="number"
					v-model="material.count"
					@input="binddata('count', $event.detail.value)"
					placeholder="请输入入库数量"
					placeholder-style="color: #c3c1c1;"
				/>
			</uni-forms-item>
			<uni-forms-item label="价格" required name="price">
				<input
					class="uni-form-item-input"
					style="padding: 0 20rpx"
					type="number"
					v-model="material.price"
					@input="binddata('price', $event.detail.value)"
					placeholder="请输入价格"
					placeholder-style="color: #c3c1c1;"
				/>
			</uni-forms-item>

			<uni-forms-item label="备注" name="remark">
				<input
					class="uni-form-item-input"
					style="padding: 0 20rpx"
					type="text"
					v-model="material.remark"
					@input="binddata('remark', $event.detail.value)"
					placeholder="请输入备注"
					placeholder-style="color: #c3c1c1;"
				/>
			</uni-forms-item>
			<view class="save-button">
				<button @tap="submit">确定</button>
			</view>
		</uni-forms>
	</view>
</template>

<script>
import {requestIsSuccess, showToast} from '@/utils/util.js';
import uniPicker from '@/components/uniPicker.vue';
import * as apiInventory from '@/api/material/inventory.js';

export default {
	name: 'materialEntryRecord',
	components: {uniPicker},
	data() {
		return {
			material: {
				id: '',
				name: '',
				spec: '',
				partitionName: '',
				supplierName: '',
				unit: '',
				amount: '',
				count: '',
				price: '',
				remark: '',
			},
			partitionRange: [],
			rules: {
				// 对name字段进行必填验证
				price: {
					rules: [
						{
							required: true,
							errorMessage: '请输入价格',
						},
					],
				},
				count: {
					rules: [
						{
							required: true,
							errorMessage: '请输入入库数量',
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
	computed: {},

	async onLoad(option) {
		let data = JSON.parse(option.data);
		this.material = {...this.material, ...data};
		this.refresh();
	},

	methods: {
		async refresh() {
			const response = await apiInventory.getListByMaterialId(this.material.id);
			if (requestIsSuccess(response) && response.data && response.data.length > 0) {
				let responses = response.data.map((item, index) => {
					return {
						...item.material,
						inventoryId: item.id,
						amount: item.amount,
						price: item.price,
						partitionName: item.partition.name,
						supplierName: item.supplier.name,
						count: '',
					};
				});
				this.material = responses[0];
				this.partitionRange = responses;
			}
		},

		submit() {
			let _this = this;
			this.$refs.form
				.submit()
				.then(async res => {
					if (res.count <= 0) {
						showToast('领取数量不能小于等于0', false);
					} else if (res.count > this.material.amount) {
						showToast('领取数量不能大于库存数量', false);
					} else if (res.price <= 0) {
						showToast('价格不能小于等于0', false);
					} else {
						uni.$emit('addMaterial', {
							...this.material,
							...res,
							partitionName: this.material.partitionName,
							materialId: _this.material.id,
						});
						uni.navigateBack();
					}
				})
				.catch(err => {
					uni.showToast({
						icon: 'none',
						title: err,
					});
				});
		},
		onPartition(e) {
			this.material = e;
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
#pages-uni-entry-record-material {
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

	.uni-form-member-select {
		flex: 1;
	}

	.icon-required {
		font-size: 18px;
		color: red;
		position: absolute;
		left: 10rpx;
	}
	.uni-entry-record-item {
		flex: 1;
		height: 70rpx;
		background-color: #ffffff;
		border-radius: 10rpx;
	}

	.entry-record-add {
		display: flex;
		justify-content: space-between;
		margin-bottom: 10rpx;
	}

	.entry-record-add-button {
		display: flex;
		align-items: center;
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
