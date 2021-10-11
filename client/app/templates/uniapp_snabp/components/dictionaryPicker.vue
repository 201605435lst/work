<template>
	<picker
		style="width: 100%"
		@change="onChange($event, dictionaries)"
		:disabled="disabled"
		:value="index"
		:range="dictionaries"
		:range-key="'name'"
	>
		<view v-if="!!value" class="uni-data-picker">{{ dictionaries[index].name }}</view>
		<view v-else class="uni-data-picker" style="color: #c3c1c1">{{ placeholder }}</view>
	</picker>
</template>

<script>
import {ModulesType} from '../utils/enum.js';
import {requestIsSuccess, checkToken} from '../utils/util.js';
import * as apiSystem from '@/api/system.js';

export default {
	name: 'dictionaryPicker',
	model: {prop: 'value', event: 'input'},
	props: {
		axios: {type: Function, default: null},
		value: {type: String, default: null},
		groupCode: {type: String, default: null},
		disabled: {type: Boolean, default: false},
		placeholder: {type: String, default: '请选择'},
	},
	data() {
		return {
			dictionaries: [],
			index: 0,
		};
	},
	created() {
		this.refresh();
	},
	methods: {
		async refresh() {
			const response = await apiSystem.getValues({groupCode: this.groupCode});
			if (requestIsSuccess(response) && response.data && response.data.length > 0) {
				this.dictionaries = response.data;
				if (this.value) {
					this.index = this.dictionaries.findIndex(item => item.id === this.value);
				}
			}
		},
		onChange(event, storage) {
			this.index = event.target.value;
			this.$emit('input', storage[this.index].id);
		},
	},
};
</script>

<style>
.uni-data-picker {
	height: 70rpx;
	padding: 0 20rpx;
	background-color: #ffffff;
	line-height: 70rpx;
	font-size: 30rpx;
}
</style>
