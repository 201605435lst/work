<!-- 质量问题标记详情页面 -->
<template>
	<view class="uni-quality-details">
		<!-- 加载中 -->
		<uniLoading v-show="loading" class="uniLoading" />
		<!-- 基本信息 -->
		<view class="quality-details-information">
			<uni-row class="basic-row" :gutter="16">
				<block v-for="(item, index) of dataList" :key="index">
					<uni-col :span="24">
						<view class="basic-col">
							<view class="basic-col-title">{{ item.name }}</view>
							<scroll-view class="basic-col-content" :scroll-x="true">{{ item.value || '--' }}</scroll-view>
						</view>
					</uni-col>
				</block>
				<uni-col :span="6">
					<view class="basic-col">
						<view class="basic-col-title">问题附件：</view>
						<text v-if="!(qualityOfDetails.files && qualityOfDetails.files.length > 0)">-</text>
					</view>
				</uni-col>
				<uni-col :span="18">
					<pickerImage :disabled="true" :url="getUploadUrl()" :value="qualityOfDetails.files" />
				</uni-col>
			</uni-row>
		</view>
		<!-- 整改,验证记录 -->
		<block v-if="qualityOfDetails.recordList && qualityOfDetails.recordList.length > 0">
			<block v-for="(item, index) of qualityOfDetails.recordList" :key="index">
				<view class="record-title">
					{{ getQualityOfRecordType(item.type) }}记录
					<text :style="{color: getStateColor(item.state)}">（{{ getQualityOfRecordState(item.state) }}）</text>
				</view>
				<view class="quality-Details-information" style="padding: 20rpx">
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
								<scroll-view class="basic-col-content" :scroll-x="true">{{ item.user || '--' }}</scroll-view>
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
			</block>
		</block>
	</view>
</template>

<script>
import {
	requestIsSuccess,
	getQualityOfRecordType,
	getQualityOfRecordState,
	getFileUrl,
	getQualityProblemType,
	getUploadUrl,
} from '@/utils/util.js';
import {PageState, SafeQualityProblemState} from '@/utils/enum.js';
import datePicker from '@/components/datePicker.vue';
import dictionaryPicker from '@/components/dictionaryPicker.vue';
import * as apiQuality from '@/api/quality/quality.js';
import pickerImage from '@/components/pickerImage.vue';
import moment from 'moment';

export default {
	name: 'qualityOfDetails',
	components: {datePicker, dictionaryPicker, pickerImage},
	data() {
		return {
			src: require('@/static/thumbs/problem.png'),
			loading: true,
			PageState,
			id: '',
			pageState: PageState.Add,
			qualityOfDetails: {
				name: '',
				date: '',
				typeId: '',
				remark: '',
				money: '',
				contractRltFiles: [],
			},
		};
	},
	computed: {
		dataList() {
			return [
				{name: '问题标题：', value: this.qualityOfDetails.title},
				{name: '问题类型：', value: this.qualityOfDetails.type},
				{name: '检查时间：', value: this.qualityOfDetails.checkTime},
				{name: '限定时间：', value: this.qualityOfDetails.limitTime},
				{name: '检查人：', value: this.qualityOfDetails.checker},
				{name: '责任人：', value: this.qualityOfDetails.responsibleUser},
				{name: '抄送人：', value: this.qualityOfDetails.ccUsers},
				{name: '验证人：', value: this.qualityOfDetails.verifier},
				{name: '检查单位：', value: this.qualityOfDetails.checkUnitName},
				{name: '责任单位：', value: this.qualityOfDetails.responsibleUnit},
				{name: '责任部门：', value: this.qualityOfDetails.responsibleOrganization},
				{name: '关联模型：', value: '--'},
				{name: '问题描述：', value: this.qualityOfDetails.content},
				{name: '整改意见：', value: this.qualityOfDetails.suggestion},
			];
		},
	},

	onLoad(option) {
		this.pageState = option.pageState;
		this.id = option.id;
		this.refresh();
	},

	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},

	methods: {
		getFileUrl,
		getUploadUrl,
		getQualityOfRecordType,
		getQualityOfRecordState,
		async refresh() {
			this.loading = true;
			if (!this.id) return;
			const response = await apiQuality.get(this.id);
			const recordList = await apiQuality.getRecordList(this.id);
			if (requestIsSuccess(response)) {
				let _response = response.data;
				let _recordList = recordList.data.map(item => {
					return {
						...item,
						user: item.user && item.user.name ? item.user.name : '',
						time: moment(item.time).format('YYYY-MM-DD'),
						files: item.files.length > 0 ? item.files.map(item_ => item_.file) : [],
					};
				});
				this.qualityOfDetails = {
					..._response,
					type: getQualityProblemType(_response.type),
					checkTime: moment(_response.checkTime).format('YYYY-MM-DD'),
					limitTime: moment(_response.limitTime).format('YYYY-MM-DD'),
					checker: _response.checker && _response.checker.name ? _response.checker.name : '',
					verifier: _response.verifier && _response.verifier.name ? _response.verifier.name : '',
					responsibleUser: _response.responsibleUser && _response.responsibleUser.name ? _response.responsibleUser.name : '',
					responsibleOrganization:
						_response.responsibleOrganization && _response.responsibleOrganization.name
							? _response.responsibleOrganization.name
							: '',
					ccUsers: _response.ccUsers.length > 0 ? _response.ccUsers.map(item => item.ccUser.name).join('/') : '暂无人员',
					files: _response.files.length > 0 ? _response.files.map(item => item.file) : [],
					recordList: _recordList,
				};
			}
			this.loading = false;
		},
		// 获取记录状态颜色
		getStateColor(state) {
			let color = '';
			switch (state) {
				case SafeQualityProblemState.WaitingImprove:
					color = '#ff5500';
					break;
				case SafeQualityProblemState.WaitingVerify:
					color = '#ff55ff';
					break;
				case SafeQualityProblemState.Improved:
					color = '#42c32d';
					break;
			}

			return color;
		},
		previewImage(imgSrc) {
			uni.previewImage({
				urls: [getFileUrl(imgSrc.file.url)],
			});
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
}

.improve-record-content {
	border-top: solid 1rpx gray;
}

.basic-col {
	display: flex;
	padding-bottom: 30rpx;
	white-space: nowrap;
	text-overflow: ellipsis;
	overflow: hidden;
}
.basic-col-img > image {
	height: 100rpx;
	width: 100rpx;
	margin: 0 10rpx;
}

.basic-col-title {
	min-width: 150rpx;
	color: #1a1a1a;
	font-weight: 600;
	text-align-last: justify;
}
.basic-col-title-item {
	overflow: hidden;
	white-space: nowrap;
	text-overflow: ellipsis;
}
.basic-col-title-item > text {
	padding: 0 10rpx 5rpx 0;
}
.uni-quality-details {
	font-size: 26rpx;
	color: #656565;
}

.quality-details-information {
	padding: 20rpx;
}
.basic-col-content {
	flex: 1;
	display: inline-block;
	overflow: hidden;
	white-space: nowrap;
}
</style>
