<template>
	<view class="listSelect">
		<view style="position: relative; width: 100%">
			<textarea
				class="pages-textarea"
				auto-height
				maxlength="-1"
				placeholder-style="color: #c3c1c1; z-index:0"
				:placeholder="disabled ? '' : placeholder"
				:class="disabled ? 'disabledColor' : ''"
				:disabled="disabled"
				v-model="inputVal"
				@input="onInput"
				@tap="toggleSelector"
			/>
			<!-- <view class="IconAdd"><view class="iconfont icon-Down"></view></view> -->
			<!-- <uni-icons class="listSelect-input-arrow" type="arrowdown" size="13" color="#c3c1c1" /> -->
			<view class="listSelect-selector" v-if="showSelector">
				<scroll-view scroll-y="true" class="listSelect-selector-scroll">
					<view class="listSelect-selector-empty" v-if="filterCandidatesLength === 0" @tap="toggleSelector">
						<text>无匹配项</text>
					</view>
					<view
						class="listSelect-selector-item"
						v-for="(item, index) in filterCandidates"
						:key="index"
						@click="onSelectorClick(index)"
					>
						<scroll-view class="basic-col-content" :scroll-x="true">{{ item.keySelect || '--' }}</scroll-view>
					</view>
				</scroll-view>
			</view>
		</view>
	</view>
</template>

<script>
/**
 * listSelect 列表选择输入框
 * @description 列表选择输入框
 * @property {Boolean} disabled = [true|false] 是否禁用
 * @property {Array} dataSource 候选项列表数据
 * @property {String} value 输入框的值
 * @property {String} url 接口url
 * @property {String} keySelect 需要选择展示关键字
 * @property {String} keyEmit 传出需要的关键字值
 * @property {String} placeholder 提示词
 * @event {Function} input 传出input值的事件
 * @event {Function} keyEmit 传出关键字值的事件
 * @example <listSelect :necessary="false" url="/api/app/qualityProblemLibrary/getList" keySelect="content" keyEmit="measures" v-model="formData.content" @input="item=>{formData.content=item}" @keyEmit="item=>{formData.suggestion=item}"/>
 */
import service from '@/utils/service.js';
export default {
	name: 'listSelect',
	props: {
		disabled: {type: Boolean, default: false},
		dataSource: {type: Array, default: () => []},
		value: {type: String, default: ''},
		url: {type: String, default: ''},
		keySelect: {type: String, default: ''},
		keyEmit: {type: String, default: ''},
		placeholder: {type: String, default: '请输入'},
	},
	data() {
		return {
			showSelector: false,
			dataSource_: [],
			inputVal: '',
		};
	},
	computed: {
		filterCandidates() {
			return this.dataSource_.map(item => {
				return {keySelect: item[this.keySelect], keyEmit: item[this.keyEmit]};
			});
		},
		filterCandidatesLength() {
			return this.filterCandidates.length;
		},
	},
	methods: {
		async refresh(url) {
			let response = await this.getList(url, {isAll: true});
			if (response && (response.statusCode === 200 || response.statusCode === 201 || response.statusCode === 204)) {
				this.dataSource_ = response.data.items || response.data;
			}
		},
		getList(url, data) {
			const res = service.request({
				url: url,
				method: 'get',
				data,
			});
			return res;
		},
		toggleSelector() {
			this.showSelector = !this.showSelector;
		},
		onInput(e) {
			this.$emit('input', e.detail.value);
		},
		onSelectorClick(index) {
			this.inputVal = this.filterCandidates[index].keySelect;
			this.showSelector = false;
			this.$emit('input', this.inputVal);
			this.$emit('keyEmit', this.filterCandidates[index].keyEmit);
		},
	},
	mounted() {
		this.inputVal = this.value;
		this.dataSource_ = this.dataSource;
		this.url != '' ? this.refresh(this.url) : '';
	},
	watch: {
		value(newVal) {
			this.inputVal = newVal;
		},
	},
};
</script>

<style lang="scss">
.listSelect {
	display: flex;
	align-items: center;
	width: 100%;
	font-size: 30rpx;
	.basic-col-content {
		flex: 1;
		display: inline-block;
		overflow: hidden;
		white-space: nowrap;
	}
}
.listSelect-input {
	height: 70rpx;
	border-radius: 10rpx;
	padding: 0 20rpx;
	background-color: #ffffff;
	font-size: 28rpx;
}
.disabledColor {
	background-color: #f0f0f0;
}
.listSelect-input-arrow {
	background-color: #ffffff;
	padding: 16rpx 10rpx 10rpx;
	position: absolute;
	right: 0;
	top: 0;
}
.listSelect-selector {
	box-sizing: border-box;
	position: absolute;
	/* top: 42px; */
	left: 0;
	bottom: 0;
	width: 100%;
	background-color: #ffffff;
	border-radius: 6px;
	box-shadow: #dddddd 4px 4px 8px, #dddddd -4px -4px 8px;
	margin-bottom: 88rpx;
	z-index: 5;
}
.listSelect-selector-scroll {
	max-height: 200px;
	box-sizing: border-box;
}
.listSelect-selector-empty,
.listSelect-selector-item {
	display: flex;
	line-height: 72rpx;
	font-size: 24rpx;
	border-bottom: solid 1rpx #dddddd;
	margin: 0px 20rpx;
}

.listSelect-selector-empty:last-child,
.listSelect-selector-item:last-child {
	border-bottom: none;
}
.IconAdd {
	background-color: #ffffff;
	position: absolute;
	right: 10rpx;
	top: 0;
	border-radius: 10rpx;
	height: 100%;
	display: flex;
	align-items: center;
}
.icon-Down {
	color: #c3c1c1;
	font-size: 28rpx;
	padding: 10rpx;
}
</style>
