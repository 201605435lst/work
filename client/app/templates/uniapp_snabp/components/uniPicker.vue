<template>
	<view class="uni-picker-box" style="width: 100%">
		<!-- 单列单项选择 -->
		<picker
			class="uniPicker-picker"
			v-if="mode == 'selector'"
			mode="selector"
			:class="disabled ? 'sm-input-disabled' : ''"
			:range="range_"
			:range-key="rangeKey_"
			:disabled="disabled"
			@change="onSelector"
		>
			<view v-if="isShowValueBox">
				<view class="box-picker" v-if="!val" style="color: #c3c1c1">{{ placeholder }}</view>
				<view class="box-picker" v-else>{{ val }}</view>
				<view class="iconfont icon-Down"></view>
			</view>
			<view class="oprator-icon" v-else><view class="iconfont icon-add"></view></view>
		</picker>
		<!-- 单列多项选择 -->
		<picker
			class="uniPicker-picker"
			v-if="mode == 'multipleSelector'"
			mode="selector"
			:class="disabled ? 'sm-input-disabled' : ''"
			:range="range_"
			:range-key="rangeKey_"
			:disabled="disabled"
			@change="onSelector"
		>
			<view v-if="isShowValueBox">
				<view class="box-picker" v-if="!val" style="color: #c3c1c1">{{ placeholder }}</view>
				<view class="box-picker" v-else>{{ val }}</view>
				<view class="iconfont icon-Down"></view>
			</view>
			<view class="oprator-icon" v-else><view class="iconfont icon-add"></view></view>
		</picker>
		<!-- 多列选择 -->
		<picker
			class="uniPicker-picker"
			v-else-if="mode == 'multiSelector'"
			mode="multiSelector"
			:class="disabled ? 'sm-input-disabled' : ''"
			:range="range_"
			:range-key="rangeKey_"
			:disabled="disabled"
			@change="onMultiSelector"
		>
			<view v-if="isShowValueBox">
				<view class="box-picker" v-if="!val" style="color: #c3c1c1">{{ placeholder }}</view>
				<view class="box-picker" v-else>{{ val }}</view>
				<view class="iconfont icon-Down"></view>
			</view>
			<view class="oprator-icon" v-else><view class="iconfont icon-add"></view></view>
		</picker>
		<!-- 日期选择 -->
		<picker
			v-else-if="mode == 'date'"
			mode="date"
			@change="onDate"
			:value="val"
			:class="disabled ? 'sm-input-disabled' : ''"
			:disabled="disabled"
		>
			<view v-if="isShowValueBox">
				<view class="box-picker" v-if="!val" style="color: #c3c1c1">{{ placeholder }}</view>
				<view class="box-picker" v-else>{{ val }}</view>
				<view class="iconfont icon-Down"></view>
			</view>
			<view class="oprator-icon" v-else><view class="iconfont icon-add"></view></view>
		</picker>
	</view>
</template>

<script>
/**
 * uniPicker 前缀文本
 * @description 前缀文本,可与框组合
 * @property {Boolean} disabled = [true|false] 是否禁用
 * @property {String} mode = [selector(单列单项选择)|multipleSelector(单列多项选择)|multiSelector(多列选择器)|date(日期选择器)] 前缀文本内容
 * @property {Number,Array,String} value value的值表示选择了range中的第几个（下标从 0 开始）;表示选中的日期，格式为"YYYY-MM-DD"
 * @property {Array / Array＜Object＞} range mode为selector或multiSelector时，range有效;列表内容
 * @property {String} groupCode 数据字典标识
 * @property {String} placeholder 提示词
 * @event {Function} change 选择后触发 change 事件
 * @example
 * import uniPicker from '@/components/uniPicker.vue';
 * components: {uniPicker},
 * <uniPicker mode="selector" class="sm-input-class" :range="markTypeRange" range-key="value" @change=" e => {formData.markType = e.markType;}"/>
 */
import * as apiSystem from '@/api/system.js';
export default {
	name: 'uniPicker',
	model: {prop: 'value', event: 'input'},
	props: {
		disabled: {type: Boolean, default: false},
		mode: {type: String, default: ''},
		value: {type: [Number, Array, String, Object], default: ''},
		range: {type: Array, default: () => []},
		rangeKey: {type: String, default: ''},
		groupCode: {type: String, default: ''},
		placeholder: {type: String, default: '请选择'},
		isShowValueBox: {type: Boolean, default: true},
	},
	data() {
		return {
			val: '',
			range_: [],
			rangeKey_: '',
		};
	},
	methods: {
		onSelector(e) {
			if (this.groupCode == '') {
				this.val = this.range_[e.detail.value][this.rangeKey_];
				this.$emit('input', this.range_[e.detail.value]);
			} else if (this.groupCode != '') {
				this.val = this.range_[e.detail.value][this.rangeKey_];
				this.$emit('input', this.range_[e.detail.value]);
			}
		},
		onMultiSelector(e) {
			let event = e.detail.value;
			let L = this.range_[0][event[0]];
			let R = this.range_[1][event[1]];
			event[0] == 0 ? (L = '') : '';
			event[1] == 0 ? (R = '') : '';
			this.$emit('input', [L, R]);
		},
		onDate(e) {
			this.val = e.detail.value;
			this.$emit('input', e.detail.value);
		},
	},
	async mounted() {
		if (typeof this.value == 'string') {
			this.val = this.value;
		} else if (this.value && this.value.name) {
			this.val = this.value.name;
		}
		this.range_ = this.range;
		this.rangeKey_ = this.rangeKey;
		if (this.groupCode != '') {
			const groupCodeData = await apiSystem.getValues({groupCode: this.groupCode});
			this.range_ = groupCodeData.data;
			this.rangeKey_ = 'name';
		}
	},
	watch: {
		value(val) {
			if (typeof val == 'string') {
				this.val = val;
			} else if (val && val.name) {
				this.val = val.name;
			}
		},
		range(val) {
			this.range_ = val;
		},
	},
};
</script>

<style lang="scss" scoped>
.uni-picker-box {
	.box-picker {
		height: 70rpx;
		padding: 0 20rpx;
		background-color: #ffffff;
		line-height: 70rpx;
		font-size: 28rpx;
		border-radius: 10rpx;
		overflow: auto;
	}
	.uniPicker-picker {
		position: relative;
	}
	.icon-Down {
		font-size: 28rpx;
		color: #c3c1c1;
		position: absolute;
		right: 20rpx;
		line-height: 70rpx;
		top: 0;
	}
	.icon-add {
		font-size: 40rpx;
		color: #1890ff;
	}
	.oprator-icon {
		height: 70rpx;
		width: 70rpx;
		display: flex;
		align-items: center;
		justify-content: flex-end;
	}
}
</style>
