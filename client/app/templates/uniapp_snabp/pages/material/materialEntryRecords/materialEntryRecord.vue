<!-- 安全问题标记页面 -->
<template>
	<view id="pages-uni-entry-record">
		<uni-forms ref="form" :value="entryRecord" :rules="rules">
			<uni-forms-item label="仓库名称" required name="partition">
				<treeSelect
					action="/api/app/partition/getTreeList"
					placeholder="请选择仓库"
					title="库存位置"
					:disabled="pageState == PageState.View"
					v-model="entryRecord.partition"
					@input="binddata('partition', $event.detail.value)"
				/>
			</uni-forms-item>
			<uni-forms-item label="入库时间" required name="time">
				<uniPicker
					mode="date"
					:disabled="pageState == PageState.View"
					v-model="entryRecord.time"
					placeholder="请选择入库时间"
					@input="binddata('time', $event)"
				/>
			</uni-forms-item>
			<uni-forms-item label="录入人" required name="creator">
				<memberSelect
					v-model="entryRecord.creator"
					:disabled="pageState == PageState.View"
					placeholder="请选择录入人"
					class="uni-form-member-select"
					@input="binddata('creator', $event)"
				></memberSelect>
			</uni-forms-item>

			<uni-forms-item label="备注" name="remark">
				<textarea
					class="pages-textarea"
					auto-height
					maxlength="-1"
					:disabled="pageState == PageState.View"
					placeholder="请输入状况原因"
					placeholder-style="color: #c3c1c1; z-index:0"
					v-model="entryRecord.remark"
					@input="binddata('remark', $event.target.value)"
				/>
			</uni-forms-item>

			<uni-forms-item label="图片上传" name="entryRecordRltFiles">
				<pickerImage
					style="flex: 1"
					:url="getUploadUrl()"
					:disabled="pageState == PageState.View"
					v-model="entryRecord.entryRecordRltFiles"
					@input="binddata('entryRecordRltFiles', $event)"
				/>
			</uni-forms-item>
			<view>
				<view class="entry-record-add">
					<view style="font-size: 28rpx">入库材料</view>
					<view class="entry-record-add-button" v-if="pageState != PageState.View">
						<text style="font-size: 28rpx">扫码添加</text>
						<view class="iconfont icon-scan1" @tap="addMaterial" />
					</view>
				</view>
				<wyb-table :headers="materialColumns" :contents="materials" @onCellClick="onCellClick1" />
			</view>
			<view>
				<view class="entry-record-add">
					<view style="font-size: 28rpx">跟踪构件</view>
					<view class="entry-record-add-button" v-if="pageState != PageState.View">
						<text style="font-size: 28rpx">扫码添加</text>
						<view class="iconfont icon-scan1" @tap="addComponent" />
					</view>
				</view>
				<wyb-table :headers="componentColumns" :contents="components" @onCellClick="onCellClick2" />
			</view>
		</uni-forms>
		<view class="save-button">
			<button v-show="pageState == PageState.Add" @tap="submit">提交</button>
		</view>
	</view>
</template>

<script>
import {requestIsSuccess, getUploadUrl, showToast} from '@/utils/util.js';
import {PageState} from '@/utils/enum.js';
import uniPicker from '@/components/uniPicker.vue';
import memberSelect from '@/components/uniMemberSelect.vue';
import treeSelect from '@/components/treeSelect.vue';
import pickerImage from '@/components/pickerImage.vue';
import * as apiEntryRecord from '@/api/material/entryRecord.js';
import * as apiMaterial from '@/api/material/material.js';
import * as apiComponentCategory from '@/api/stdBasic/componentCategory.js';
import moment from 'moment';

export default {
	name: 'materialEntryRecordMaterial',
	components: {
		uniPicker,
		memberSelect,
		treeSelect,
		pickerImage,
	},
	data() {
		return {
			PageState,
			id: '',
			pageState: PageState.Add,
			entryRecord: {
				partition: {},
				creator: {},
				time: '',
				remark: '',
				entryRecordRltMaterials: [],
				entryRecordRltQRCodes: [],
				entryRecordRltFiles: [],
			},
			materials: [],
			components: [],
			rules: {
				// 对name字段进行必填验证
				partitionId: {
					rules: [
						{
							required: true,
							errorMessage: '请选择仓库',
						},
					],
				},
				creatorId: {
					rules: [
						{
							required: true,
							errorMessage: '请选择录入人',
						},
					],
				},
				time: {
					rules: [
						{
							required: true,
							errorMessage: '请选择入库时间',
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
		materialColumns() {
			let materialColumn = [
				{
					label: '名称',
					key: 'name',
				},
				{
					label: '规格',
					key: 'spec',
				},
				{
					label: '厂商',
					key: 'supplierName',
				},
				{
					label: '价格',
					key: 'price',
				},
				{
					label: '入库量',
					key: 'count',
				},
			];
			this.pageState == PageState.View
				? materialColumn
				: materialColumn.push({
						label: '操作',
						key: 'wyb-delete',
				  });
			return materialColumn;
		},
		componentColumns() {
			let componentColumn = [
				{
					label: '构件名称',
					key: 'componentName',
				},
				{
					label: '单位',
					key: 'unit',
				},
			];
			this.pageState == PageState.View
				? componentColumn
				: componentColumn.push({
						label: '操作',
						key: 'wyb-delete',
				  });
			return componentColumn;
		},
	},

	async onLoad(option) {
		this.pageState = option.pageState;
		this.id = option.id;
		if (this.pageState == PageState.View) {
			this.refresh();
		}
		let _this = this;
		uni.$on('addMaterial', function (data) {
			data.supplierName = data.supplier.name;
			if (
				!_this.materials.find(
					item => item.materialId === data.materialId && item.supplierId === data.supplierId && item.price === data.price,
				)
			) {
				_this.materials.push(data);
			}
		});
	},

	methods: {
		getUploadUrl,
		//获取详情
		async refresh() {
			if (!this.id) return;
			const response = await apiEntryRecord.get(this.id);
			if (requestIsSuccess(response)) {
				let _entryRecord = response.data;
				this.entryRecord = {
					..._entryRecord,
					time: moment(_entryRecord.time).format('YYYY-MM-DD'),
					entryRecordRltFiles:
						_entryRecord.entryRecordRltFiles && _entryRecord.entryRecordRltFiles.length > 0
							? _entryRecord.entryRecordRltFiles.map(item => {
									return item.file;
							  })
							: [],
				};
				this.materials =
					_entryRecord.entryRecordRltMaterials && _entryRecord.entryRecordRltMaterials.length > 0
						? _entryRecord.entryRecordRltMaterials.map(item => {
								return {
									materialId: item.materialId,
									name: item.material ? item.material.name : '',
									spec: item.material ? item.material.spec : '',
									unit: item.material ? item.material.unit : '',
									count: item.count,
									price: item.price,
									supplierName: item.supplier ? item.supplier.name : '',
								};
						  })
						: [];
				this.components =
					_entryRecord.entryRecordRltQRCodes && _entryRecord.entryRecordRltQRCodes.length > 0
						? _entryRecord.entryRecordRltQRCodes.map(item => {
								return {
									componentCategory: item.componentCategory,
									componentName: item.componentCategory ? item.componentCategory.name : '',
									unit: item.componentCategory && item.componentCategory.unit ? item.componentCategory.unit : '',
								};
						  })
						: [];
			}
		},
		// 扫码添加材料
		addMaterial() {
			let _this = this;
			uni.scanCode({
				success: async res => {
					let result = JSON.parse(res.result);
					if (result && result.key === 'material' && result.value) {
						let id = result.value;
						if (!_this.materials.find(item => item.materialId === id)) {
							let response = await apiMaterial.get(id);
							if (requestIsSuccess(response) && response.data) {
								uni.navigateTo({
									url: `./materialEntryRecordMaterial?&data=${JSON.stringify(response.data)}`,
								});
							} else {
								showToast('材料获取失败', false);
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

		// 扫码跟踪构件
		addComponent() {
			let _this = this;
			uni.scanCode({
				success: async res => {
					let result = JSON.parse(res.result);
					if (result && result.key === 'equipment' && result.value) {
						let qrCode = result.value;
						if (!_this.components.find(item => item.qrCode === qrCode)) {
							let array = qrCode.split('@');
							if (array.length >= 2 && array[0]) {
								let response = await apiComponentCategory.getByCode(array[0]);
								if (requestIsSuccess(response) && response.data) {
									let target = {
										componentCategory: response.data,
										componentCategoryId: response.data.id,
										qrCode: qrCode,
										componentName: response.data.name,
										unit: response.data.unit ? response.data.unit : '',
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
			console.log(_this.components);
		},

		cancel() {
			uni.navigateBack();
		},

		submit() {
			let _this = this;
			if (_this.materials.length == 0) {
				showToast('入库材料不能为空', false);
				return;
			} else {
				_this.$refs.form
					.submit()
					.then(async res => {
						let data = {
							...res,
							partitionId: res.partition.id,
							creatorId: res.creator.id,
							entryRecordRltFiles:
								res.entryRecordRltFiles.length > 0
									? res.entryRecordRltFiles.map(item => {
											return {
												fileId: item.id,
											};
									  })
									: [],
							entryRecordRltMaterials: _this.materials,
							entryRecordRltQRCodes:
								this.components.length > 0
									? _this.components.map(item => {
											return {
												qrCode: item.qrCode,
											};
									  })
									: [],
						};

						if (_this.pageState == PageState.Add) {
							if (!this.$store.state.isSubmit) {
								this.$store.commit('SetIsSubmit', true);
								let response = await apiEntryRecord.create(data);
								if (requestIsSuccess(response)) {
									showToast('操作成功');
								} else {
									showToast('操作失败', false);
								}
							}
						}
					})
					.catch(err => {
						console.log('表单错误信息：', err);
					});
			}
		},
		//表格操作1
		onCellClick1(e) {
			if (!e.checkType && e.checkType != 'multiple' && e.key == 'wyb-delete') {
				this.materials.splice(e.contentIndex, 1);
			}
		},
		//表格操作2
		onCellClick2(e) {
			if (!e.checkType && e.checkType != 'multiple' && e.key == 'wyb-delete') {
				this.components.splice(e.contentIndex, 1);
			}
		},
	},
};
</script>

<style lang="scss">
page {
	background-color: #f9f9f9;
}
#pages-uni-entry-record {
	padding: 20rpx;
	margin-bottom: 120rpx;
	background-color: #f9f9f9;
	.uni-form-item {
		display: flex;
		height: 80rpx;
		align-items: center;
		justify-content: space-between;
		padding: 0 10rpx;
	}

	.uni-form-item-input {
		padding: 0 20rpx;
		flex: 1;
		height: 70rpx;
		background-color: #ffffff;
		border-radius: 10rpx;
		font-size: 28rpx;
	}

	.uni-form-member-select {
		flex: 1;
	}

	.pages-uni-entry-record-item {
		flex: 1;
		height: 70rpx;
		background-color: #ffffff;
		border-radius: 10rpx;
	}

	.entry-record-add {
		display: flex;
		justify-content: space-between;
		align-items: center;
		margin: 40rpx 0 20rpx 0;
	}

	.entry-record-add-button {
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
