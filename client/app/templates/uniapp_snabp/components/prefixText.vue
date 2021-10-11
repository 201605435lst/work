<template>
	<!-- 前缀文本 -->
	<view :class="necessary ? 'necessary-true' : 'necessary-false'">
		<view v-if="necessary" class="iconfont icon-required prefixText-icon"></view>
		<view style="width: 150rpx">{{ label }}</view>
	</view>
</template>

<script>
/**
 * prefixText 前缀文本
 * @description 前缀文本,可与框组合
 * @property {String} label 前缀文本内容
 * @property {Boolean} necessary = [true|false] 是否必填项
 * @example
 * import prefixText from '@/components/prefixText.vue';
 * components: {prefixText},
 * <prefixText label="前缀文本" :necessary="false"/>
 */
export default {
	name: 'prefixText',
	props: {
		label: {type: String, default: ''},
		necessary: {type: Boolean, default: true},
	},
};
</script>

<style>
.necessary-true,
.necessary-false {
	display: flex;
	align-items: center;
	margin-right: 20rpx;
}
.necessary-false {
	margin-left: 36rpx;
}
.prefixText-icon {
	left: 10rpx;
	color: red;
	font-size: 26rpx;
}
</style>
