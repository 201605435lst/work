<!-- 施工日志提交审批 -->
<template>
	<view class="uni-construction-daily-submit">
		<uni-forms ref="form" :value="constructionDaily" :rules="rules">
			<uni-forms-item label="审批流程" required name="workFlow">
				<uniPicker
					mode="selector"
					:range="workFlows"
					placeholder="请选择审批流程"
					range-key="name"
					:value="workFlow"
					@input="binddata('workFlow', $event)"
				/>
			</uni-forms-item>
		</uni-forms>
		<view class="construction-daily-button-box"><button class="save-button" @tap="submit">提交</button></view>
	</view>
</template>

<script>
import {requestIsSuccess, showToast} from '@/utils/util.js';
import uniPicker from '@/components/uniPicker.vue';
import * as apiDaily from '@/api/construction/daily.js';
import * as apiBpm from '@/api/bpm.js';

export default {
	name: 'constructionDailySubmit',
	components: {
		uniPicker,
	},
	data() {
		return {
			workFlows: [], //审批流程数据源
			id: '', //施工日志id
			constructionDaily: {
				workFlow: {},
			},
			rules: {
				workFlow: {
					rules: [
						{
							required: true,
							errorMessage: '请输入标题',
						},
					],
				},
			},
		};
	},

	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},

	async onLoad(option) {
		this.id = option.id;
		console.log(option);
		console.log(this.id);
		this.refresh();
	},

	methods: {
		requestIsSuccess,
		submit() {
			this.$refs.form
				.submit()
				.then(async res => {
					if (!this.$store.state.isSubmit) {
						this.$store.commit('SetIsSubmit', true);
						let response = await apiDaily.createWorkFlow(this.id, res.workFlow.id);
						if (requestIsSuccess(response)) {
							showToast('提交成功');
						} else {
							showToast('提交失败', false);
						}
					}
				})
				.catch(err => {
					console.log('表单错误信息：', err);
				});
		},

		//获取工作流模板数据
		async refresh() {
			let response = await apiBpm.getList({
				select: true,
				forCurrentUser: true,
				isAll: true,
			});
			if (requestIsSuccess(response) && response.data && response.data.items && response.data.items.length > 0) {
				this.workFlows = response.data.items;
			}
		},
	},
};
</script>

<style>
page {
	background-color: #f9f9f9;
}

.uni-construction-daily-submit {
	padding: 20rpx;
	background-color: #f9f9f9;
}

.uni-form-item {
	display: flex;
	height: 80rpx;
	align-items: center;
	justify-content: space-between;
	padding: 0 10rpx;
}

.uni-form-member-select {
	flex: 1;
}

.uni-construction-daily-submit .save-button {
	background-color: #1890ff;
	margin: 10px;
	color: #ffffff;
	border-radius: 100rpx;
}

.construction-daily-button-box {
	width: 100%;
	position: fixed;
	z-index: 999;
	bottom: 0;
	left: 0;
	background-color: #f9f9f9;
}

.icon-required {
	font-size: 18px;
	color: red;
	position: absolute;
	left: 10rpx;
}
</style>
