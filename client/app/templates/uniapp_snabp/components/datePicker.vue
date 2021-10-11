<template>
	<picker style="width: 100%" mode="date" :value="value" @change="onChange" :disabled="disabled">
		<view v-if="!!value" class="uni-date-picker">{{ value }}</view>
		<view v-else class="uni-date-picker" style="color: #c3c1c1">{{ placeholder }}</view>
	</picker>
</template>

<script>
export default {
	name: 'datePicker',
	model: {prop: 'value', event: 'input'},
	props: {
		value: {type: String, default: null},
		mode: {type: String, default: 'selector'},
		disabled: {type: Boolean, default: false},
		placeholder: {type: String, default: '请选择'},
	},
	data() {
		return {};
	},
	created() {},
	methods: {
		onChange(event) {
			console.log(event);
			this.$emit('input', event.detail.value);
		},
	},
};
</script>

<style>
.uni-date-picker {
	height: 70rpx;
	padding: 0 20rpx;
	background-color: #ffffff;
	line-height: 70rpx;
	white-space: nowrap;
	text-overflow: ellipsis;
	overflow: hidden;
	border-radius: 10rpx;
}
</style>
