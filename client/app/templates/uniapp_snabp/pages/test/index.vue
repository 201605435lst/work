<template>
	<view class="content">
		<view class="row b-b image">
			<text class="tit">头像</text>
			<pickerImage
				:edit="true"
				v-model="selected"
				url="http://localhost:8091/api/app/fileFile/uploadForApp"
				@change="onChange"
			></pickerImage>
		</view>
	</view>
</template>

<script>
import pickerImage from '@/components/pickerImage.vue';
export default {
	components: {pickerImage},
	data() {
		return {
			selected: [
				{
					id: '39fc6605-2260-25eb-bff6-2d70f7be8d53',
					url: '/2021/05/39fc6605-2260-25eb-bff6-2d70f7be8d53.jpg',
					name: 'FQhWt8z5G2Gy125d9ff204f0e7f6934504d05cab412b',
				},
			],
		};
	},
	methods: {
		onChange(value) {
			console.log(value);
		},
	},
};
</script>

<style lang="scss">
page {
	padding-top: 16upx;
}
.row {
	display: flex;
	align-items: center;
	position: relative;
	padding: 0 30upx;
	height: 110upx;
	background: #fff;

	.tit {
		flex-shrink: 0;
		width: 160upx;
		font-size: 30upx;
	}
	&.m-t {
		margin-top: 16upx;
	}
	.input {
		flex: 1;
		font-size: 30upx;
	}
}

.image {
	height: 140upx;
}
.action {
	display: flex;
	flex-direction: row;
	align-items: center;
	position: absolute;
	right: 0;
}

.uni-icon {
	font-size: 38upx;
	padding-right: 30upx;
	color: #303133;
}

.add-btn {
	display: flex;
	align-items: center;
	justify-content: center;
	width: 690upx;
	height: 80upx;
	margin: 60upx auto;
	color: #fff;
	border-radius: 10upx;
	box-shadow: 1px 2px 5px rgba(219, 63, 96, 0.4);
}
</style>
