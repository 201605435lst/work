<template>
	<!-- //盈亏分析 -->
	<view id="pages-cost">
		<web-view src="../../static/echartsHTML/cost.html">暂不支持小程序,请app查看...</web-view>
	</view>
</template>

<script>
import {requestIsSuccess} from '@/utils/util.js';
import * as apiCost from '@/api/cost/cost.js';
export default {
	data() {
		return {
			chartData: {
				categories: [],
				series: [
					{
						name: '合同额',
						data: [],
					},
					{
						name: '总支出',
						data: [],
					},
				],
			},
		};
	},
	onLoad(args) {
		this.refresh();
	},
	methods: {
		async refresh() {
			// 获取统计信息
			let response = await apiCost.getStatistics();
			if (requestIsSuccess(response) && response.data) {
				response = response.data;
				let series = [
					{
						name: '合同额',
						data: response.contractAmount,
					},
					{
						name: '总支出',
						data: response.totalExpenditure,
					},
				];
				this.chartData.categories = response.dates;
				this.chartData.series = series;
			}
		},
	},
	mounted() {
		setTimeout(() => {
			this.chartType = 'column';
		}, 5000);
	},
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},
};
</script>

<style lang="scss">
page {
	background-color: #f9f9f9;
}
#pages-cost {
	padding-top: 40rpx;
	.charts-box {
		width: 100%;
		height: 300px;
		.charts-box-text {
			display: flex;
			flex-direction: column;
			align-items: center;
			> text:nth-child(2) {
				font-size: 25rpx;
				color: #a0a0a0;
			}
		}
		.cost-switch-image {
			padding: 40rpx;
			image {
				padding-right: 10rpx;
			}
		}
	}
}
</style>
