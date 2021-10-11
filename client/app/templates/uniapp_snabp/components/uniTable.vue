<template>
	<view class="uni-table">
		<view class="uni-table-body">
			<view class="uni-table-column" :style="{width: 100 / columns.length + '%'}" v-for="(item, index1) in columns" :key="index1">
				<view class="uni-table-column-title" :style="{width: item.hasOwnProperty('width') ? item.width : ''}">
					{{ item.title }}
				</view>
				<view
					class="uni-table-column-content"
					:style="{width: item.hasOwnProperty('width') ? item.width : ''}"
					v-for="(_item, index2) in iDataSource"
					:key="index2"
				>
					<view class="uni-table-column-input" v-if="item.operateKey == 'select' && selectData && selectData.length > 0">
						<uni-combox
							v-if="!disabled"
							:candidates="getCandidates(item.key)"
							placeholder="请选择"
							:value="_item[item.key]"
							@input="
								value => {
									onSelect(index2, item.key, value);
								}
							"
						></uni-combox>
						<view class="column-content" v-else style="height: 46rpx; display: flex; align-items: center">
							{{ _item[item.key] ? _item[item.key] : '无' }}
						</view>
					</view>
					<view class="uni-table-column-input" v-else-if="item.operateKey == 'input'">
						<input
							style="font-size: 12px; height: 23px"
							:value="_item[item.key]"
							:type="item.inputType"
							:disabled="disabled || item.disabled || _item.disabled"
							@input="
								event => {
									onInput(index2, item.key, event.target.value);
								}
							"
							:placeholder="item.placeholder || '请输入'"
							placeholder-style="color: #c3c1c1; text-align: center;"
						/>
					</view>
					<view class="uni-table-column-scanning" v-else-if="item.operateKey == 'scanCode' && !disabled">
						<view class="iconfont icon-scanning" @tap="onScanCode(index2)" />
					</view>

					<view class="column-content" v-else>{{ _item.hasOwnProperty(item.key) ? _item[item.key] : '' }}</view>
					<view v-if="item.key == 'operators' && !disabled" class="uni-table-td">
						<view class="iconfont icon-ashbin" @tap="onOperator(index2)"></view>
					</view>
				</view>
			</view>
		</view>
		<view v-if="iDataSource.length == 0" class="uni-table-body">
			<view class="uni-table-empty">
				<view class="iconfont icon-empty-a" />
				<view class="uni-table-empty-content">暂无数据</view>
			</view>
		</view>
	</view>
</template>

<script>
import {showToast} from '@/utils/util.js';

export default {
	name: 'uniTable',
	props: {
		columns: {type: Array, default: () => []}, //[{key:'index',title:'序号',operateKey:'select',width:'40rpx',inputType:'number'}]
		dataSource: {type: Array, default: () => []},
		disabled: {type: Boolean, default: false},
		selectData: {type: Array, default: () => []}, //[{columnKey:'name',list:[]}]
	},
	data() {
		return {
			iDataSource: [],
			isShow: false,
		};
	},
	watch: {
		dataSource: {
			handler: function (val, nVal) {
				this.iDataSource = val;
			},
			immediate: true,
		},
	},
	computed: {
		columnsKeys() {
			let newArray = this.columns.map(item => item.key);
			return newArray;
		},
	},
	created() {},
	methods: {
		getCandidates(key) {
			let index = this.selectData.findIndex(x => x.columnKey && x.columnKey == key);
			let newArray = [];
			newArray = this.selectData[index].list;
			return newArray;
		},

		onOperator(index) {
			this.iDataSource.splice(index, 1);
			this.$emit('change', this.iDataSource);
		},
		onInput(index, key, value) {
			this.iDataSource[index][key] = value;
			this.$emit('change', this.iDataSource);
			this.$emit('recordChange', index, value);
		},
		onSelect(index, key, value) {
			this.iDataSource[index][key] = value;
			this.$emit('change', this.iDataSource);
		},
		// 扫码功能
		onScanCode(index) {
			uni.scanCode({
				success: res => {
					let result = JSON.parse(res.result);
					if (result) {
						this.$emit('scan', result, this.iDataSource[index]);
					}
				},
				fail: err => {
					uni.showToast({
						icon: 'none',
						title: 'err',
						duration: 1000,
					});
				},
			});
		},
	},
};
</script>

<style>
.uni-table {
	color: #7d7d7d;
	font-size: 24rpx;
	border: solid 1rpx #dad6d6;
	border-radius: 5rpx;
	/* background-color: #ffffff; */
}

.uni-table-body {
	display: flex;
	justify-content: space-around;
}

.uni-table-column-title {
	text-overflow: ellipsis;
	white-space: nowrap;
	overflow: hidden;
	padding: 10rpx 0;
	color: #636363;
	font-size: 24rpx;
	font-weight: 500;
	text-align: center;
}

.column-content {
	text-overflow: ellipsis;
	white-space: nowrap;
	overflow: hidden;
	padding: 0 10rpx;
}

.uni-table-column-content {
	text-overflow: ellipsis;
	white-space: nowrap;
	overflow: hidden;
	padding: 10rpx 0;
	height: 46rpx;
	display: flex;
	align-items: center;
	justify-content: center;
}

.uni-table-column-input {
	background-color: white;
	border-radius: 10rpx;
	padding: 0 8rpx;
	border: solid 1rpx #d8d8d8;
}

.uni-table-select {
	background-color: #ffff;
	padding: 20rpx 0;
	min-height: 300rpx;
	border-radius: 10rpx 10rpx 0 0;
}

.icon-ashbin {
	font-size: 44rpx;
	color: red;
}

.uni-table-empty {
	display: flex;
	/* flex-direction: column; */
	justify-content: center;
	align-items: center;
	height: 120rpx;
	color: #c3c1c1;
}

.icon-empty-a {
	font-size: 50rpx;
	margin-right: 20rpx;
}
.uni-table-column-scanning {
	width: 100%;
	text-align: center;
	color: #1890ff;
}
.icon-scanning {
	font-size: 48rpx;
}
</style>
