<template>
	<view class="upload-content">
		<block v-for="(item, index) in iValue" :key="index">
			<view class="upload-item">
				<image
					v-if="['.jpg', '.png', '.jpeg'].indexOf(item.type) != -1"
					class="upload-img"
					:src="getFileUrl(item.url) || require('../static/BIMAppUI/fileTypeIcon/png_jpg_bmp.png')"
					mode="aspectFill"
					@click="previewImage(index)"
				></image>
				<image v-else class="upload-img2" :src="getFileTypeIcon(item.type)" mode="aspectFill" @tap="downloadFile"></image>
				<image
					v-if="!disabled"
					class="upload-del-btn"
					@click="delImage(index)"
					src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACYAAAAmCAYAAACoPemuAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6RjNBODgzQjUwNDM5MTFFOUJDMjlGN0UwRTJGMjVCNjQiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6RjNBODgzQjYwNDM5MTFFOUJDMjlGN0UwRTJGMjVCNjQiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDpGM0E4ODNCMzA0MzkxMUU5QkMyOUY3RTBFMkYyNUI2NCIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDpGM0E4ODNCNDA0MzkxMUU5QkMyOUY3RTBFMkYyNUI2NCIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/PuYQTIAAAAHNSURBVHjazJgxasMwFIbjnsD4BDmCLxDQDZrNQ5bcILlBvXvpDRLIWmiHFkyH2t0LyVbwEkPpbE/eYlUCGYQax096UuIffkIgij7ryU/vyZuYy2e+Z56K70R8lsJcOfPn5AriMCvmZ2YKdMW8EQ/hRA9iEorwXlpZtObMRySQ6kysvrFWloHUEIcmUBuHUDLccmxQskErt7wyVLdy00tQ4Q2g5De2V9kNwWjffiNDA33f/42i6E13Qj5uNptlwJD+SyODuaqqqi/KVBTFOxSKA7VtW/NxSZI8AcbE2nuraZpvKgSBk6G4drsdBOwogz1CwAghH/JEl+BUKJ1VltPHHjoIAoeE4l53YFqb+RKcBSgqyqXhtxEKZwkKB3YOThYCqjPuCDoHZwGK3mGLtdPp5LmqTI1Dqe4p26EkNqA4iE6eg2z+iQ0okyQMATvYgLIIt9Y6knTyFBIudHqIq3BpmkL60VJ9O0sXZY8MBwSLtdNGEAQ/i8XiVXcz83EcEPDbuq/fzMdYWt+6GTmMsX2rh9q3TtsxNrzXhKt1rwg6rR1Dhdjqo7QMlWOvoWTF4imxQMRF7eaL8L5ohmwrLgDB8pCQc8DlcG7y538CDABJNGPqfaJgfgAAAABJRU5ErkJggg=="
					mode="scaleToFill"
				></image>
				<view class="upload-progress" v-if="item.progress < 100">{{ item.progress }}%</view>
			</view>
		</block>
		<view v-if="!disabled" class="upload-add-btn" @click="chooseImage"></view>
	</view>
</template>

<script>
import {getFileUrl, getFileTypeIcon} from '../utils/util.js';
export default {
	model: {
		prop: 'value',
		event: 'input',
	},
	data() {
		return {
			imageList: [],
			iValue: [],
		};
	},
	props: {
		url: {
			type: String,
			value: '', //上传接口地址
		},
		count: {
			type: Number,
			value: 1, //单次可选择的图片数量
		},
		length: {
			type: Number,
			value: 50, //可上传总数量
		},
		value: {
			type: [Array, String],
			default: () => [], //上传文件信息
		},
		disabled: {
			type: Boolean,
			default: false, //是否禁用
		},
	},
	computed: {
		rduLength() {
			return this.length - this.iValue.length;
		},
	},
	watch: {
		value: {
			handler: function (val, nVal) {
				if (val) {
					this.iValue = val;
				}
			},
			immediate: true,
		},
	},
	mounted() {
		// this.iValue = [];
	},
	methods: {
		getFileUrl,
		getFileTypeIcon,
		//选择图片
		chooseImage: function () {
			uni.chooseImage({
				// count: this.rduLength < this.count ? this.rduLength : this.count, //最多可以选择的图片张数，默认9
				count: 1, //最多可以选择的图片张数，默认9
				sizeType: ['original', 'compressed'], //original 原图，compressed 压缩图，默认二者都有
				// sourceType: ['album'], //album 从相册选图，camera 使用相机，默认二者都有
				success: res => {
					const images = res.tempFilePaths;
					this.uploadFiles(images);
				},
			});
		},
		//上传图片
		async uploadFiles(images) {
			uni.showLoading({
				title: '请稍后..',
				mask: true,
			});
			let uploadUrl;
			try {
				uploadUrl = await this.uploadImage(images[0]);
			} catch (err) {
				console.log(err);
				return;
			}

			if (uploadUrl !== false) {
				images.splice(0, 1);
				this.iValue[this.iValue.length - 1].src = uploadUrl;

				//判断是否需要继续上传
				if (images.length > 0 && this.rduLength > 0) {
					this.uploadFiles(images);
				} else {
					uni.hideLoading();
				}
			} else {
				//上传失败处理
				this.iValue.pop();
				uni.hideLoading();
				uni.showToast({
					title: '上传中出现问题，已终止上传',
					icon: 'none',
					mask: true,
					duration: 2000,
				});
			}
		},
		uploadImage: function (file) {
			let _this = this;
			return new Promise((resolve, reject) => {
				//发送给后端的附加参数
				const formData = {
					thumb_mode: 1,
				};
				const header_ = {
					OrganizationId:
						uni.getStorageSync('organization') && uni.getStorageSync('organization').id
							? uni.getStorageSync('organization').id
							: '',
					ProjectId: uni.getStorageSync('Project').id || '',
					OrganizationTagId: uni.getStorageSync('organizationTagId') || '',
				};
				const uploadTask = uni.uploadFile({
					url: this.url,
					filePath: file,
					name: 'file',
					header: header_,
					formData: formData,
					success(uploadFileResult) {
						if (uploadFileResult.statusCode === 200 && uploadFileResult.data) {
							resolve(uploadFileResult.data);
							let data = JSON.parse(uploadFileResult.data);
							_this.iValue.push({...data, progress: 0});
							_this.$emit('input', _this.iValue);
						} else {
							reject('接口返回错误');
						}
					},
					fail() {
						reject('网络链接错误');
					},
				});
				//上传进度
				uploadTask.onProgressUpdate(progressRes => {
					setTimeout(() => {
						_this.iValue[_this.iValue.length - 1].progress = progressRes.progress;
					}, 2000); //1秒后执行代码
				});
			});
		},
		//删除图片
		delImage: function (index) {
			uni.showModal({
				content: '确定要放弃这个文件么？',
				success: confirmRes => {
					if (confirmRes.confirm) {
						this.iValue.splice(index, 1);
						this.$emit('input', this.iValue);
					}
				},
			});
		},
		//预览图片
		previewImage: function (index) {
			const urls = [];
			this.iValue.forEach(item => {
				urls.push(getFileUrl(item.url));
			});
			uni.previewImage({
				current: urls[index],
				urls: urls,
				indicator: 'number',
			});
		},
		//下载文件
		downloadFile() {
			uni.showToast({
				icon: 'none',
				title: '文件暂不可下载',
				duration: 2000,
			});
		},
	},
};
</script>

<style lang="scss">
.upload-content {
	margin: 10rpx 0;
	z-index: 0;
}
.upload-item {
	position: relative;
	float: left;
	width: 100rpx;
	height: 100rpx;
	margin: 15rpx 20rpx 15rpx 0;
	// &:nth-child(4n) {
	// 	margin-right: 0;
	// }
	.upload-img {
		width: 100%;
		height: 100%;
		border-radius: 8upx;
		border: solid 1rpx #e0e0e0;
	}
	.upload-img2 {
		width: 100%;
		height: 100%;
		border-radius: 8upx;
	}
	.upload-del-btn {
		position: absolute;
		right: -20rpx;
		top: -18rpx;
		width: 30rpx;
		height: 30rpx;
		border-radius: 100px;
		padding: 10rpx;
		z-index: 10;
	}
	.upload-progress {
		position: absolute;
		left: 0;
		top: 0;
		display: flex;
		align-items: center;
		justify-content: center;
		width: 100%;
		height: 100%;
		background-color: rgba(0, 0, 0, 0.4);
		color: #fff;
		font-size: 24rpx;
		border-radius: 8upx;
	}
}
.upload-add-btn {
	margin: 15rpx 15rpx 15rpx 0;
	position: relative;
	float: left;
	width: 100rpx;
	height: 100rpx;
	border-radius: 8upx;
	background: #ffffff;
	border: dashed 1rpx #e0d9d9;
	&:before,
	&:after {
		content: ' ';
		position: absolute;
		top: 50%;
		left: 50%;
		-webkit-transform: translate(-50%, -50%);
		transform: translate(-50%, -50%);
		width: 4upx;
		height: 60upx;
		background-color: #d6d6d6;
	}
	&:after {
		width: 60upx;
		height: 4upx;
	}
	&:active {
		background-color: #f7f7f7;
	}
}
</style>
