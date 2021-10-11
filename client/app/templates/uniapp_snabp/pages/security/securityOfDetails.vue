<!-- 安全问题标记详情页面 -->
<template>
	<view class="uni-security-details">
		<!-- 基本信息 -->
		<view class="security-information">
			<uni-row class="basic-row" :gutter="16">
				<uni-col :span="24">
					<view class="basic-col">
						<view class="basic-col-title">问题标题：</view>
						<scroll-view class="basic-col-content" :scroll-x="true">{{ problem.title || '--' }}</scroll-view>
					</view>
				</uni-col>
				<uni-col :span="24">
					<view class="basic-col">
						<view class="basic-col-title">问题类型：</view>
						<scroll-view class="basic-col-content" :scroll-x="true">{{ problem.type ? problem.type.name : '--' }}</scroll-view>
					</view>
				</uni-col>
				<uni-col :span="24">
					<view class="basic-col">
						<view class="basic-col-title">检查时间：</view>
						<scroll-view class="basic-col-content" :scroll-x="true">{{ problem.checkTime || '--' }}</scroll-view>
					</view>
				</uni-col>
				<uni-col :span="24">
					<view class="basic-col">
						<view class="basic-col-title">限期时间：</view>
						<scroll-view class="basic-col-content" :scroll-x="true">{{ problem.limitTime || '--' }}</scroll-view>
					</view>
				</uni-col>
				<uni-col :span="24">
					<view class="basic-col">
						<view class="basic-col-title">检查人：</view>
						<scroll-view class="basic-col-content" :scroll-x="true">
							{{ problem.checker ? problem.checker.name : '--' }}
						</scroll-view>
					</view>
				</uni-col>
				<uni-col :span="24">
					<view class="basic-col">
						<view class="basic-col-title">责任人：</view>
						<scroll-view class="basic-col-content" :scroll-x="true">
							{{ problem.responsibleUser ? problem.responsibleUser.name : '--' }}
						</scroll-view>
					</view>
				</uni-col>
				<uni-col :span="24">
					<view class="basic-col">
						<view class="basic-col-title">抄送人：</view>
						<scroll-view class="basic-col-content" :scroll-x="true">{{ problem.ccUsers || '--' }}</scroll-view>
					</view>
				</uni-col>
				<uni-col :span="24">
					<view class="basic-col">
						<view class="basic-col-title">验证人：</view>
						<scroll-view class="basic-col-content" :scroll-x="true">
							{{ problem.verifier ? problem.verifier.name : '--' }}
						</scroll-view>
					</view>
				</uni-col>
				<uni-col :span="24">
					<view class="basic-col">
						<view class="basic-col-title">检查单位：</view>
						<scroll-view class="basic-col-content" :scroll-x="true">{{ problem.checkUnitName || '--' }}</scroll-view>
					</view>
				</uni-col>
				<uni-col :span="24">
					<view class="basic-col">
						<view class="basic-col-title">责任单位：</view>
						<scroll-view class="basic-col-content" :scroll-x="true">{{ problem.responsibleUnit || '--' }}</scroll-view>
					</view>
				</uni-col>
				<uni-col :span="24">
					<view class="basic-col">
						<view class="basic-col-title">责任部门：</view>
						<scroll-view class="basic-col-content" :scroll-x="true">
							{{ problem.responsibleOrganization ? problem.responsibleOrganization.name : '--' }}
						</scroll-view>
					</view>
				</uni-col>
				<uni-col :span="24">
					<view class="basic-col">
						<view class="basic-col-title">关联模型：</view>
						--
					</view>
				</uni-col>
				<uni-col :span="24">
					<view class="basic-col-suggestion">
						<view class="basic-col-title">问题描述：</view>
						<scroll-view class="basic-col-content" :scroll-x="true">{{ problem.content || '--' }}</scroll-view>
					</view>
				</uni-col>
				<uni-col :span="24">
					<view class="basic-col">
						<view class="basic-col-title">整改意见：</view>
						<scroll-view class="basic-col-content" :scroll-x="true">{{ problem.suggestion || '--' }}</scroll-view>
					</view>
				</uni-col>
				<uni-col :span="6">
					<view class="basic-col">
						<view class="basic-col-title">问题附件：</view>
						<text v-if="!(problem.files && problem.files.length > 0)">-</text>
					</view>
				</uni-col>
				<uni-col :span="18">
					<pickerImage :disabled="true" :url="getUploadUrl()" :value="problem.files" />
				</uni-col>
			</uni-row>
		</view>
		<!-- 整改记录 -->
		<view v-for="(item, index) in recordList" :key="index">
			<view class="record-title">
				{{ getQualityOfRecordType(item.type) }}记录&nbsp;&nbsp;(
				<view :style="{color: getStateColor(item.state)}">{{ getQualityOfRecordState(item.state) }}</view>
				)
			</view>
			<view class="security-information">
				<uni-row class="basic-row" :gutter="16">
					<uni-col :span="12">
						<view class="basic-col">
							<view class="basic-col-title">{{ getQualityOfRecordType(item.type) }}时间：</view>
							<scroll-view class="basic-col-content" :scroll-x="true">{{ item.time || '--' }}</scroll-view>
						</view>
					</uni-col>
					<uni-col :span="12">
						<view class="basic-col">
							<view class="basic-col-title">{{ getQualityOfRecordType(item.type) }}人：</view>
							<scroll-view class="basic-col-content" :scroll-x="true">{{ item.user ? item.user.name : '--' }}</scroll-view>
						</view>
					</uni-col>
					<uni-col :span="24">
						<view class="basic-col">
							<view class="basic-col-title">{{ getQualityOfRecordType(item.type) }}内容：</view>
							<scroll-view class="basic-col-content" :scroll-x="true">{{ item.content || '--' }}</scroll-view>
						</view>
					</uni-col>
					<uni-col :span="5">
						<view class="basic-col">
							<view class="basic-col-title">{{ getQualityOfRecordType(item.type) }}附件：</view>
						</view>
					</uni-col>
					<uni-col :span="19">
						<pickerImage :disabled="true" :url="getUploadUrl()" :value="item.files" />
						<text v-if="!(item.files && item.files.length > 0)">--</text>
					</uni-col>
				</uni-row>
			</view>
		</view>
	</view>
</template>

<script>
import {checkToken, requestIsSuccess, getUploadUrl, getQualityOfRecordType, getQualityOfRecordState} from '@/utils/util.js';
import {PageState, MaterialType, SafeQualityRecordType, SafeQualityRecordState} from '@/utils/enum.js';
import datePicker from '@/components/datePicker.vue';
import dictionaryPicker from '@/components/dictionaryPicker.vue';
import pickerImage from '@/components/pickerImage.vue';
import * as apiSafeProblem from '@/api/safe/safeProblem.js';
import moment from 'moment';

export default {
	name: 'securityOfDetails',
	components: {datePicker, dictionaryPicker, pickerImage},
	data() {
		return {
			src: require('@/static/thumbs/problem.png'),
			SafeQualityRecordType,
			SafeQualityRecordState,
			PageState,
			problem: {},
			pageState: PageState.Add,
			recordList: [],
		};
	},

	onLoad(option) {
		this.refresh(option.id);
	},

	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},

	methods: {
		getUploadUrl,
		getQualityOfRecordType,
		getQualityOfRecordState,

		//获取状态样式
		getStateColor(state) {
			let color = '';
			switch (state) {
				case SafeQualityRecordState.UnCheck:
					color = '#ff5500';
					break;
				case SafeQualityRecordState.Checking:
					color = '#ff55ff';
					break;
				case SafeQualityRecordState.Passed:
					color = '#42c32d';
					break;
			}

			return color;
		},

		async refresh(id) {
			if (!id) return;
			const problemResponse = await apiSafeProblem.get(id);
			if (requestIsSuccess(problemResponse)) {
				let _problem = problemResponse.data;

				this.problem = {
					..._problem,
					checkTime: moment(_problem.checkTime).format('YYYY-MM-DD hh:mm:ss'),
					limitTime: moment(_problem.limitTime).format('YYYY-MM-DD hh:mm:ss'),
					ccUsers: _problem.ccUsers.length > 0 ? _problem.ccUsers.map(item => item.ccUser.name).join('/') : '暂无人员',
					files: _problem.files.length > 0 ? _problem.files.map(item => item.file) : [],
				};
			}
			const response = await apiSafeProblem.getRecordList(id);
			if (requestIsSuccess(response) && response.data && response.data.length > 0) {
				let _recordList = response.data;
				this.recordList = _recordList.map(item => {
					return {
						...item,
						time: moment(item.time).format('YYYY-MM-DD'),
						files: item.files.map(_item => {
							return _item.file;
						}),
					};
				});
			}
		},
	},
};
</script>

<style>
page {
	background-color: #f9f9f9;
}

.record-title {
	color: #1890ff;
	border-bottom: solid 1rpx #dcdcdc;
	padding: 20rpx;
	display: flex;
}

.improve-record-content {
	border-top: solid 1rpx gray;
}

.security-image {
	height: 100rpx;
	width: 100rpx;
	margin: 0 20rpx;
}

.basic-col {
	display: flex;
	padding-bottom: 30rpx;
	white-space: nowrap;
	text-overflow: ellipsis;
	overflow: hidden;
}

.basic-col-suggestion {
	display: flex;
	padding-bottom: 20rpx;
}

.basic-col-title {
	min-width: 150rpx;
	color: #1a1a1a;
	font-weight: 600;
	/* display: flex; */
	/* justify-content:flex-end; */
	text-align-last: justify;
}

.basic-col-content {
	flex: 1;
	display: inline-block;
	overflow: hidden;
	white-space: nowrap;
}

.uni-security-details {
	font-size: 26rpx;
	color: #656565;
}

.security-information {
	padding: 20rpx;
}
</style>
