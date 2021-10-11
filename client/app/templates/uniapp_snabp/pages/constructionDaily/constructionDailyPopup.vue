<template>
	<uni-popup ref="popup" type="bottom" @change="onChange">
		<view class="un-plan-task-draw">
			<view class="un-plan-task-draw-title">--&nbsp;&nbsp;请添加&nbsp;&nbsp;--</view>
			<uni-forms ref="form" :value="task" :rules="rules">
				<uni-forms-item label="任务类型" required name="taskType">
					<uniPicker
						mode="selector"
						:range="taskTypes"
						placeholder="请选择任务类型"
						range-key="name"
						:value="getUnplannedTaskType(task.taskType)"
						@input="binddata('taskType', $event)"
					/>
				</uni-forms-item>
				<uni-forms-item label="任务说明" required name="content">
					<textarea
						class="pages-textarea"
						auto-height
						maxlength="-1"
						placeholder="请输入任务说明"
						placeholder-style="color: #c3c1c1 !important; z-index:0"
						style="max-height: 280rpx; overflow: auto"
						@input="binddata('content', $event.target.value)"
					/>
				</uni-forms-item>
			</uni-forms>
			<view class="save-button-box"><button class="save-button" @tap="confirm">确定</button></view>
		</view>
	</uni-popup>
</template>

<script>
import {getUnplannedTaskType, showToast} from '@/utils/util.js';
import uniPicker from '@/components/uniPicker.vue';
export default {
	name: 'constructionDailyPopup',
	components: {uniPicker},
	props: {},
	data() {
		return {
			task: {
				taskType: '',
				content: '',
			},
			taskTypes: [
				{name: '临时任务', key: 1},
				{name: '其他任务', key: 2},
			],
			rules: {
				// 进行必填验证
				taskType: {
					rules: [
						{
							required: true,
							errorMessage: '请选择任务类型',
						},
					],
				},
				content: {
					rules: [
						{
							required: true,
							errorMessage: '请输入任务说明',
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

	methods: {
		getUnplannedTaskType,

		//打开popup
		open() {
			this.$refs.popup.open();
		},

		binddata(name, value) {
			if (name == 'taskType') {
				this.task.taskType = value.key;
			}
			this.$refs.form.setValue(name, value);
		},
		getFormData() {
			this.$refs.form
				.submit()
				.then(async res => {
					this.$emit('change', {...res, taskTypeName: getUnplannedTaskType(res.taskType)});
				})
				.catch(err => {
					showToast('err');
					console.log('表单错误信息：', err);
				});
		},

		onChange(event) {
			if (event && !event.show) {
				this.task = {
					taskType: '',
					content: '',
				};
			}
		},

		confirm() {
			this.getFormData();
			this.$refs.popup.close();
		},
	},
};
</script>

<style>
.un-plan-task-draw {
	padding: 20rpx;
	background-color: #f9f9f9;
	height: 600rpx;
}

.un-plan-task-draw .un-plan-task-draw-item {
	padding: 0 20rpx;
	flex: 1;
	height: 70rpx;
	background-color: #ffffff;
	border-radius: 10rpx;
	font-size: 28rpx;
	display: flex;
	align-items: center;
}

.un-plan-task-draw .un-plan-task-draw-title {
	text-align: center;
	margin-bottom: 20rpx;
}
</style>
