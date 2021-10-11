<!-- 人员选择，附件上传 -->
<template>
	<view class="Box">
		<!-- 前缀文本 -->
		<view v-if="need_PrefixText" :class="necessary ? 'prefixText' : 'prefixTexts'">
			<view v-if="necessary" class="iconfont icon-required"></view>
			<view style="width: 154rpx">{{ Prefix_text }}</view>
		</view>
		<memberSelect
			v-if="mode == 'People'"
			:disabled="disabled"
			:value="value_"
			class="uni-form-member-select"
			:placeholder="'请选择' + Prefix_text"
			@input="selectDirectors"
		/>
		<memberSelect
			v-else-if="mode == 'Peoples'"
			:disabled="disabled"
			:value="value_"
			multiple="true"
			class="uni-form-member-select"
			:placeholder="'请选择' + Prefix_text"
			@input="selectDirectors"
		/>
		<treeSelect
			v-else-if="mode == 'treeSelect'"
			v-model="value_"
			:title="Prefix_text"
			:lazy="true"
			action="/api/app/appOrganization/getList"
			:placeholder="'请选择' + Prefix_text"
			@input="selectDirectors"
		/>
		<!-- 附件上传 -->
		<pickerImage
			v-else-if="mode == 'Annex'"
			style="flex: 1"
			:url="getUploadUrl()"
			:value="Files"
			@input="
				value => {
					Files = value;
				}
			"
		/>
		<!-- 输入框 -->
		<input
			v-else
			:type="type"
			:placeholder="disabled ? '' : '请输入' + Prefix_text"
			placeholder-style="color: #c3c1c1;"
			class="input_class uni-input"
			:class="disabled ? 'disabledColor' : ''"
			:disabled="disabled"
			:value="value"
			@input="inputContent"
		/>
	</view>
</template>

<script>
import memberSelect from '@/components/uniMemberSelect.vue';
import pickerImage from '@/components/pickerImage.vue';
import {getUploadUrl} from '@/utils/util.js';
import treeSelect from '@/components/treeSelect.vue';
export default {
	components: {memberSelect, pickerImage, treeSelect},
	props: {
		need_PrefixText: {type: Boolean, default: true}, //是否需要前缀文本
		Prefix_text: {type: String, default: ''}, // 前缀文本
		necessary: {type: Boolean, default: true}, // 是否必填项
		disabled: {type: Boolean, default: false}, // 是否禁用
		value: {type: [String, Array, Object, Number], default: () => []}, // 内容
		type: {type: String, default: 'text'}, // input类型
		mode: {type: String, default: ''}, // 人员选择：单:People,多:Peoples；附件上传：Annex；备注：Remark；
		files: {type: Array, default: () => []}, // 文件内容
	},
	data() {
		return {
			value_: [],
			Files: [],
		};
	},
	methods: {
		getUploadUrl,
		selectDirectors(value) {
			console.log(value);
			this.value_ = value;
			this.$emit('change', value || '');
		},
		inputContent(e) {
			this.$emit('change', e.target.value || '');
		},
	},
	mounted() {
		this.value_ = this.value;
		this.Files = this.files;
	},
	watch: {
		value: function (value) {
			this.value_ = value;
		},
		Files(e) {
			this.$emit('change', e);
		},
	},
};
</script>

<style scoped>
.Box {
	display: flex;
	padding: 10rpx;
	align-items: center;
	font-size: 32rpx;
}
.prefixText,
.prefixTexts {
	display: flex;
	align-items: center;
	margin-right: 20rpx;
}
.prefixTexts {
	margin-left: 36rpx;
}

.input_class {
	width: 100%;
	height: 50rpx;
	padding: 10rpx;
	border-radius: 10rpx;
	background-color: #ffffff;
}
.icon-required {
	left: 10rpx;
	color: red;
	font-size: 26rpx;
}
.Annex {
	flex: 1;
	background-color: #ecececdc !important;
	height: 70rpx;
	border-radius: 10rpx;
	display: flex;
	justify-content: center;
	align-items: center;
	color: #757575;
}
.Annex_image {
	width: 50rpx;
	height: 50rpx;
	z-index: 2;
}
.uni-form-member-select {
	width: 528rpx;
	font-size: 32rpx;
}
.member-value-box {
	height: 70rpx !important;
}
.disabledColor {
	background-color: #f0f0f0;
}
</style>
