<template>
	<view id="pages-materialOfBill">
		<!-- 加载中 -->
		<uniLoading v-show="loading" class="uniLoading" />
		<!-- 展示区 -->
		<uni-forms v-show="!loading" ref="form" :value="formData" :rules="rules">
			<uni-forms-item label="施工队" required name="constructionTeam">
				<textarea
					class="pages-textarea"
					auto-height
					maxlength="-1"
					placeholder="请输入施工队"
					:disabled="pageState == PageState.View"
					placeholder-style="color: #c3c1c1; z-index:0"
					v-model="formData.constructionTeam"
					@input="binddata('remark', $event.detail.value)"
				/>
			</uni-forms-item>
			<uni-forms-item label="施工区段" required name="section">
				<treeSelect
					class="uni-form-member-select"
					placeholder="请选择施工区段"
					title="施工区段"
					:disabled="pageState == PageState.View"
					action="/api/app/section/getTreeList"
					v-model="formData.section"
					@input="binddata('section', $event)"
				/>
			</uni-forms-item>
			<uni-forms-item label="领料日期" required name="time">
				<uniPicker
					:disabled="pageState == PageState.View"
					mode="date"
					:placeholder="'请选择领料日期'"
					class="sm-input-class"
					v-model="formData.time"
					@input="binddata('time', $event)"
				/>
			</uni-forms-item>
			<uni-forms-item label="领料人" required name="userId">
				<memberSelect
					:placeholder="'请选择领料人'"
					:disabled="true"
					:value="formData"
					@change="binddata('userId', $event.creatorId)"
					class="sm-input-class sm-input-disabled"
				/>
			</uni-forms-item>

			<uni-forms-item label="附件上传" required name="materialOfBillRltAccessories">
				<pickerImage
					:disabled="pageState == PageState.View"
					:url="getUploadUrl()"
					v-model="formData.materialOfBillRltAccessories"
					@input="binddata('materialOfBillRltAccessories', $event)"
				/>
			</uni-forms-item>
			<uni-forms-item label="备注" name="remark">
				<textarea
					class="pages-textarea"
					auto-height
					maxlength="-1"
					placeholder="请输入施工队"
					:disabled="pageState == PageState.View"
					placeholder-style="color: #c3c1c1; z-index:0"
					v-model="formData.remark"
					@input="binddata('remark', $event.detail.value)"
				/>
			</uni-forms-item>

			<view class="wllb">
				<view class="iconfont icon-required"></view>
				<view class="wllbs">
					<view>物料列表:</view>
					<view v-if="pageState != PageState.View">
						<text>扫码添加</text>
						<view class="iconfont icon-scan1 scan1" @tap="addMaterial" />
					</view>
				</view>
			</view>
			<wyb-table :headers="columns" :contents="materials" @onCellClick="onCellClick" />
			<view v-if="pageState != PageState.View" class="save-button">
				<button style="margin-right: 20rpx" @tap="onClick('prData')">保存</button>
				<button @tap="onClick('submit')">提交</button>
			</view>
		</uni-forms>
	</view>
</template>

<script>
import {requestIsSuccess, showToast, getUploadUrl} from '@/utils/util.js';
import {PageState} from '@/utils/enum.js';
import pickerImage from '@/components/pickerImage.vue';
import memberSelect from '@/components/uniMemberSelect.vue';
import datePicker from '@/components/datePicker.vue';
import dictionaryPicker from '@/components/dictionaryPicker.vue';
import treeSelect from '@/components/treeSelect.vue';
import moment from 'moment';

import uniDatetimePicker from '@/uni_modules/uni-datetime-picker/components/uni-datetime-picker/uni-datetime-picker.vue';

import * as apiMaterialOfBill from '@/api/material/materialOfBill.js';
import * as apiMaterial from '@/api/material/material.js';
import uniPicker from '@/components/uniPicker.vue';
import * as apiSection from '@/api/construction/section.js';

export default {
	name: 'materialOfBill',
	components: {
		datePicker,
		dictionaryPicker,
		uniDatetimePicker,
		uniPicker,
		pickerImage,
		memberSelect,
		treeSelect,
	},
	data() {
		return {
			PageState,
			loading: true,
			conList: [],
			conIndex: [0, 0],
			delRow: [],
			contract: {},
			isTap: false,
			pageState: '', //当前页面状态
			formData: {
				id: '',
				name: '',
				constructionTeam: '',
				section: {},
				creatorId: '',
				sectionId: '',
				time: '',
				userName: '',
				materialOfBillRltAccessories: [],
				materialOfBillRltMaterials: [],
				remark: '',
				state: 1,
			},
			isnew: true, // 是否是录入页
			inputError: '', // input为空
			pageindex: 0,
			materials: [], // 表格里传出的数据源
			rules: {
				constructionTeam: {
					rules: [{required: true, errorMessage: '请选择施工队'}],
				},
				section: {
					rules: [{required: true, errorMessage: '请选择施工区段'}],
				},
				time: {
					rules: [{required: true, errorMessage: '请选择领料日期'}],
				},
				userId: {
					rules: [{required: true, errorMessage: '请选择领料人'}],
				},
				materialOfBillRltAccessories: {
					rules: [{required: true, errorMessage: '请上传附件'}],
				},
			},
		};
	},
	computed: {
		columns() {
			let materialColumn = [
				{label: '名称', key: 'name'},
				{label: '规格', key: 'spec'},
				{label: '库存地点', key: 'partitionName'},
				{label: '库存量', key: 'amount'},
				{label: '领料量', key: 'count'},
			];
			this.pageState == PageState.View ? materialColumn : materialColumn.push({label: '操作', key: 'wyb-delete'});

			return materialColumn;
		},
	},
	onLoad(option) {
		this.pageState = option.pageState;
		option.id != 'undefined' ? (this.formData.id = option.id) : '';
		let userData = uni.getStorageSync('userInfo') || {};
		this.formData.creatorId = userData.id;
		this.formData.userName = userData.name;
		this.refresh(option.id);
		let _this = this;
		uni.$on('addMaterial', function (data) {
			console.log(data);
			_this.materials.push(data);
		});
	},

	methods: {
		getUploadUrl,
		// 获取内容
		async refresh(id) {
			// 获取领料单内容
			if (id != 'undefined') {
				this.isnew = false;
				const pickingData = await apiMaterialOfBill.get(id);
				if (requestIsSuccess(pickingData) && pickingData.data) {
					let dataSources = pickingData.data.materialOfBillRltMaterials;
					dataSources.forEach(e => {
						let inv = e.inventory;
						if (inv) {
							let dataSource = {
								inventoryId: e.inventoryId,
								materialOfBillId: e.materialOfBillId,
								count: e.count || '--',
								name: inv.material && inv.material.name ? inv.material.name : '--',
								spec: inv.material && inv.material.spec ? inv.material.spec : '--',
								partitionName: inv.partition && inv.partition.name ? inv.partition.name : '--',
								amount: inv.amount ? inv.amount : '--',
							};
							this.materials.push(dataSource);
						}
					});
					if (pickingData.data.materialOfBillRltAccessories.length > 0) {
						let Accessories = [];
						pickingData.data.materialOfBillRltAccessories.forEach(e => {
							if (e.file) {
								Accessories.push(e.file);
							}
						});
						pickingData.data.materialOfBillRltAccessories = Accessories;
					}
					pickingData.data.time = moment(pickingData.data.time).format('YYYY-MM-DD');
					this.formData = pickingData.data;
				}
			}
			this.formData.name = this.formData.userName;
			this.loading = false;
		},
		// 扫码功能
		onAdd() {
			uni.scanCode({
				success: res => {
					uni.navigateTo({
						url: '../infoDisplay/userInfo',
					});
				},
				fail: err => {
					// console.log(err);
				},
			});
		},
		// 保存提交数据
		async onClick(type) {
			this.$refs.form
				.submit()
				.then(async res => {
					this.isTap = true;
					if (this.materials.length > 0) {
						res = {
							...res,
							state: 1,
							id: this.formData.id,
							sectionId: res.section.id,
							userId: this.formData.creatorId,
							materialOfBillRltAccessories: res.materialOfBillRltAccessories.map(item => {
								return {
									fileId: item.id,
									materialOfBillId: this.formData.id,
								};
							}),
							materialOfBillRltMaterials: this.materials ? this.materials : this.dataSources,
						};
						type == 'submit' ? (res.state = 2) : '';
						if (!this.$store.state.isSubmit) {
							this.$store.commit('SetIsSubmit', true);
							if (this.isnew) {
								let response = await apiMaterialOfBill.create(res);
								if (requestIsSuccess(response)) {
									showToast(type == 'submit' ? '提交成功' : '保存成功');
								} else {
									showToast(type == 'submit' ? '提交失败' : '保存失败', false);
								}
							} else {
								let response = await apiMaterialOfBill.update(res);
								if (requestIsSuccess(response)) {
									showToast(type == 'submit' ? '提交成功' : '保存成功');
								} else {
									showToast(type == 'submit' ? '提交失败' : '保存失败', false);
								}
							}
						}
					} else {
						showToast('物料列表不能为空', false);
					}
				})
				.catch(err => {
					console.log('表单错误信息：', err);
				});
		},
		binddata(name, value) {
			this.$refs.form.setValue(name, value);
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
									url: `./materialOfBillMaterial?&data=${JSON.stringify(response.data)}`,
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
		//表格操作
		onCellClick(e) {
			if (!e.checkType && e.checkType != 'multiple' && e.key == 'wyb-delete') {
				this.materials.splice(e.contentIndex, 1);
			}
		},
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},
};
</script>

<style lang="scss">
page {
	background-color: #f9f9f9;
}
::v-deep #pages-materialOfBill uni-member-select .member-value-box {
	background-color: #f0f0f0 !important;
}

::v-deep #pages-materialOfBill .uni-loading-more-page {
	background-color: #f2f2f6;
}
#pages-materialOfBill {
	font-size: 28rpx;
	padding: 20rpx;
	margin-bottom: 120rpx;

	.uni-form-body {
		height: 100%;
	}

	.uni-form-item {
		display: flex;
		height: 80rpx;
		align-items: center;
		justify-content: space-between;
		padding: 0 10rpx;
	}

	.uni-form-item-title {
		width: 150rpx;
		display: flex;
		align-items: center;
	}

	.uni-form-item-input {
		font-size: 28rpx;
		flex: 1;
		background-color: #ffffff;
		height: 60rpx;
		border-radius: 10rpx;
		padding-left: 10rpx;
		display: flex;
		align-items: center;
	}

	.icon-add {
		font-size: 50rpx;
		color: #1890ff;
	}
	.save-button > button {
		background-color: #1890ff;
		color: #ffffff;
		width: 100%;
		border-radius: 100rpx;
	}

	.icon-required {
		font-size: 18px;
		color: red;
		left: 10rpx;
	}

	.wllb {
		display: flex;
		padding: 10rpx 10rpx 0;
		margin: 15rpx 0;
	}
	.wllbs {
		width: 100%;
		display: flex;
		justify-content: space-between;
	}
	.uni-form-member-box {
		flex: 1;
		display: flex;
		align-items: center;
		justify-content: space-between;
		background-color: #ffffff;
		height: 60rpx;
		border-radius: 10rpx;
	}
	.uni-form-member-list {
		display: flex;
	}
	.item-tag {
		background-color: #1890ff;
		color: #ffffff;
		border-radius: 40rpx;
		padding: 4rpx 10rpx;
		margin: 0 0 0 10rpx;
	}
	.member-palacehorder {
		margin-left: 10rpx;
		display: flex;
		align-items: center;
		color: #c3c1c1;
	}
	.wllbs > view {
		display: flex;
		align-items: center;
	}
	.box {
		padding: 15rpx;
	}
	.box .t-th,
	.box .t-td {
		font-size: 25rpx !important;
		padding: 16rpx 0 !important;
		width: 100rpx;
	}
	.scan1 {
		color: #1890ff;
		font-size: 45rpx;
		margin-left: 10rpx;
	}
	.member-value-box {
		height: 70rpx !important;
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
	.materialOfBill-input-class {
		padding: 0 20rpx;
		flex: 1;
		height: 70rpx;
		background-color: #ffffff;
		border-radius: 10rpx;
		font-size: 28rpx;
	}
}
</style>
