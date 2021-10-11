<template>
	<view class="tabBar-component" :style="{backgroundColor: backgroundColor, height: height}">
		<block v-for="(item, index) of list" :key="index">
			<view
				class="tabBar-component-item"
				:style="{color: checked_ == item.key ? selectedColor : color, fontSize: fontSize}"
				@tap="onTap(item)"
			>
				<image
					:src="checked_ == item.key ? item.selectedIconPath : item.iconPath"
					style="width: 40%; height: 40%; padding-bottom: 8rpx"
					mode="aspectFit"
				/>
				{{ item.text }}
			</view>
		</block>
	</view>
</template>

<script>
/**
 * tabBar 底部tabBar，同 https://uniapp.dcloud.io/collocation/pages?id=tabbar 属性
 * @description 底部tabBar
 * @property {Array} list=[{pagePath:'页面路径',text:'按钮文字',iconPath:'图片路径',selectedIconPath:'选中时的图片路径',key: '操作key'}] tab 的列表
 * @property {String} color tab 上的文字默认颜色
 * @property {String} selectedColor tab 上的文字选中时的颜色
 * @property {String} backgroundColor tab 的背景色
 * @property {String} height tabBar 默认高度100rpx
 * @property {String} fontSize 文字默认大小
 * @property {String} checked 默认选中的tab地址
 * @example <tabBar/>
 * @example <view style="height: 115rpx;"></view> //被遮挡了在后面添加props里的height+padding值
 */
export default {
	name: 'tabBar-component',
	props: {
		list: {
			type: Array,
			default: () => [
				{
					iconPath: require('static/BIMAppUI/toolBar/home.png'),
					selectedIconPath: require('static/BIMAppUI/toolBar/home-current.png'),
					text: '首页',
					key: 'home',
				},
				// {
				// 	iconPath: require('static/BIMAppUI/toolBar/matter.png'),
				// 	selectedIconPath: require('static/BIMAppUI/toolBar/matter-current.png'),
				// 	text: '待办',
				// 	key: 'matter',
				// },
				{
					iconPath: require('static/BIMAppUI/toolBar/scan.png'),
					selectedIconPath: require('static/BIMAppUI/toolBar/scan-current.png'),
					text: '扫一扫',
					key: 'scan',
				},
				{
					iconPath: require('static/BIMAppUI/toolBar/user.png'),
					selectedIconPath: require('static/BIMAppUI/toolBar/user-current.png'),
					text: '我的',
					key: 'user',
				},
			],
		},
		color: {type: String, default: '#9fbfff'},
		selectedColor: {type: String, default: '#ffffff'},
		backgroundColor: {type: String, default: '#173e7c'},
		height: {type: String, default: '90rpx'},
		fontSize: {type: String, default: '23rpx'},
		checked: {type: String, default: 'home'},
	},
	data() {
		return {
			checked_: this.checked,
			recordKey: '', //记录上一次点击的key
		};
	},
	methods: {
		onTap(item) {
			if (this.checked_ != item.key) {
				this.recordKey = this.checked_;
				this.checked_ = item.key;
				item.key == 'scan' ? (this.checked_ = this.recordKey) : '';
				this.$emit('change', item);
			}
		},
	},
	watch: {
		checked(key) {
			this.checked_ = key;
		},
	},
};
</script>

<style>
.tabBar-component {
	width: 100%;
	display: flex;
	position: fixed;
	bottom: 0;
	padding: 16rpx 0;
	z-index: 90;
}
.tabBar-component-item {
	width: 100%;
	display: flex;
	flex-direction: column;
	justify-content: center;
	align-items: center;
}
</style>
