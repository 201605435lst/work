<template>
	<view class="equipment-form">
		<view class=" equipment-form-item">
			<view class="equipment-form-title">
				{{title}}
			</view>
			<view class="equipment-form-divider" />
			<view class="equipment-form-content">
				<view class="" v-for="(item,index) in options" :key="index">
					<view class="equipment-form-content-item">
						<view style="width: 20%;">
							{{item.name}}:
						</view>
						<view class="equipment-form-content-input">
							{{item.value}}
						</view>
					</view>
				</view>
			</view>
		</view>
	</view>
</template>

<script>
	export default {
		name: 'equipmentForm',
		props: {
			title: "",
			options: {
				type: Array,
				default: () => []
			}
		},
		data() {
			return {}
		},
		methods: {

		}
	}
</script>

<style>
	page {
		background-color: #f9f9f9;
	}

	.equipment-form-title {
		font-size: 30upx;
		width: 95%;
		margin: auto;
		margin-top: 10px;
		color: #0062d0;
	}

	.equipment-form-divider {
		border-bottom: solid 1px #c9c9c9;
		margin-top: 5px;
	}

	.equipment-form-content {
		width: 95%;
		margin: auto;
	}

	.equipment-form-content-item {
		display: flex;
		font-size: 28upx;
		margin: 8px;
	}

	.equipment-form-content-input {
		width: 80%;
		background: #dedede;
		padding: 3px 5px;
		border-radius: 3px;
	}
</style>
