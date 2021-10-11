<template>
	<view>
		<view class="inv-h-w">
			<view v-for="(item,index) in tabTop" :key="index" :class="['inv-h',tabTopIndex==index?'inv-h-se':'']"
				@click="onToptab(index)">{{item}}</view>
		</view>
		
		<slot></slot>
	</view>
</template>

<script>
	export default {
		name:'tabs',
		data() {
			return {
				tabTopIndex: 0,
			}
		},
		props: {
			tabTop: Array, 
		},
		methods: {
			onToptab(e) {
				this.tabTopIndex = e
				this.$emit('orderTap', {
					e
				}) 
			},
		},
	}
</script>

<style lang="scss">
	page {
		background-color: #F2F2F2;
	}

	.inv-h-w {
		background-color: #FFFFFF;
		height: 100upx;
		display: flex;
	}

	.inv-h {
		font-size: 30upx;
		flex: 1;
		text-align: center;
		color: #C9C9C9;
		height: 100upx;
		line-height: 100upx;
	}

	.inv-h-se {
		color: #5BA7FF;
		border-bottom: 4upx solid #5BA7FF;
	}
</style>
