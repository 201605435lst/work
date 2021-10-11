<template>
	<view class="pages-fileManage-file-file">
		<!-- 操作区 -->
		<view class="file-top">
			<view v-if="hierarchy.length == 0" class="construction-dispatchs-oprator-view">
				<view :class="waitApprove ? 'approve-selected' : ''" @tap="onTap(true)">我的</view>
				<view :class="waitApprove ? '' : 'approve-selected'" @tap="onTap(false)">我的组织</view>
			</view>
			<view v-else class="construction-dispatchs-oprator-view">
				<view @tap="upperStory">上一层</view>
				<view class="approve-selected">{{ hierarchy[hierarchy.length - 1].name }}</view>
			</view>
			<view style="display: flex; align-items: center">
				<image :src="require('@/static/BIMAppUI/other/transmission.png')" @tap="onTransmission" />
				<image :src="require('@/static/BIMAppUI/other/screening.png')" @tap="scaleDrew = !scaleDrew" />
			</view>
			<uni-search-bar
				style="display: none"
				:class="scaleDrew == null ? '' : scaleDrew ? 'search-scaleDrewDown' : 'search-scaleDrewUp'"
				:radius="100"
				cancelButton="none"
				placeholder="输入文件名"
				@input="search"
			></uni-search-bar>
		</view>
		<view class="increase" @tap="addButton">
			<!-- :style="{animation: scaleDrew ? scaleDrew + ` 1s ease-in-out` : '', animationFillMode: 'forwards'}" -->
			<text>+</text>
		</view>

		<!-- 加载中 -->
		<uniLoading v-show="loading" class="uniLoading" />
		<view class="uni-list-empty" v-if="!loadingSwitch && !(dataSources && dataSources.length > 0)">
			<view class="iconfont icon-empty-a" />
			<view class="uni-list-empty-title">暂无数据</view>
		</view>

		<!-- 展示区 -->
		<view style="padding-top: 100rpx" v-else>
			<wyb-slide-listener @slideUp="onSlide" @slideDown="onSlide">
				<!-- 手势监听的区域 -->
				<block v-for="(item, index) of dataSources" :key="index">
					<view
						class="file-item"
						:style="{backgroundColor: item.selected ? '#f0faff' : '#ffffff'}"
						@tap="selected.length == 0 ? clickFileFolder(item) : ''"
					>
						<view><image :src="getFileTypeIcon(item.type)" /></view>
						<view>
							<view style="display: flex">
								<view>
									{{ item.name }}
								</view>
								<text>{{ item.type != '文件夹' ? item.type : '' }}</text>
							</view>
							<view style="color: #afb3bf">
								{{ item.editTime + ' ' + (item.type != '文件夹' ? getFileSize(item.size) : '') }}
							</view>
						</view>
						<view @tap.stop="selectButton(item)">
							<image v-if="item.selected" :src="require('@/static/BIMAppUI/other/selected.png')" />
							<image v-else :src="require('@/static/BIMAppUI/other/unSelected.png')" />
						</view>
					</view>
				</block>
				<uni-load-more color="#ffffff" v-show="loadNoMore" :status="loadStatus"></uni-load-more>
			</wyb-slide-listener>
		</view>
		<!-- +号点击后底部弹出 -->
		<uni-popup ref="popup" type="bottom" :mask-click="!fileFolderInput">
			<view class="file-popup1">
				<block v-if="popupType == 'add'">
					<text>{{ fileFolderInput ? '新建文件夹' : '上传至云盘' }}</text>
					<block v-if="!fileFolderInput">
						<view @tap="onPopup(1)">
							<image :src="getFileTypeIcon('文件夹')" />
							<text>新建文件夹</text>
						</view>
						<view @tap="onPopup(2)">
							<image :src="getFileTypeIcon('.png')" />
							<text>上传照片</text>
						</view>
						<view @tap="onPopup(3)">
							<image :src="getFileTypeIcon('拍照')" />
							<text>拍照上传</text>
						</view>
						<view @tap="onPopup(4)">
							<image :src="getFileTypeIcon('.mp4')" />
							<text>上传视频</text>
						</view>
						<view @tap="onPopup(5)">
							<image :src="getFileTypeIcon('链接')" />
							<text>上传其他</text>
						</view>
					</block>
					<block v-else>
						<view class="file-fileFolderInput">
							<input type="text" placeholder="输入文件名" v-model="fileFolderInputVal.name" />
							<button class="button1" size="mini" @tap="fileFolderInputCancel">取消</button>
							<button class="button2" size="mini" @tap="fileFolderInputConfirm">确认</button>
						</view>
					</block>
				</block>
			</view>
		</uni-popup>
		<!-- 点击后中间弹出 -->
		<uni-popup ref="popup2" type="dialog">
			<uni-popup-dialog
				mode="base"
				type="info"
				title="是否下载此文件"
				:duration="2000"
				:before-close="true"
				@close="close('下载')"
				@confirm="confirm('下载')"
			></uni-popup-dialog>
		</uni-popup>

		<!-- 点击删除弹出 -->
		<uni-popup ref="popup4" type="dialog">
			<uni-popup-dialog
				mode="base"
				type="info"
				title="是否删除"
				:duration="2000"
				:before-close="true"
				@close="close('删除')"
				@confirm="confirm('删除')"
			></uni-popup-dialog>
		</uni-popup>
		<!-- 顶部提示 -->
		<uni-popup ref="popup3" type="message">
			<uni-popup-message :type="popupMessage.type" :message="popupMessage.message" :duration="2000"></uni-popup-message>
		</uni-popup>
		<!-- 右侧点击底部弹出操作区 -->
		<view v-if="selected.length > 0" class="file-bottom">
			<view @tap="onPopup(6)">
				<image :src="require('@/static/BIMAppUI/other/b1.png')" />
				<text>下载</text>
			</view>
			<view @tap="onPopup(7)">
				<image :src="require('@/static/BIMAppUI/other/b2.png')" />
				<text>删除</text>
			</view>
			<view v-if="selected.length == 1" @tap="onPopup(8)">
				<image :src="require('@/static/BIMAppUI/other/b3.png')" />
				<text>重命名</text>
			</view>
			<view @tap="onPopup(9)">
				<image :src="require('@/static/BIMAppUI/other/b4.png')" />
				<text>发布到</text>
			</view>
		</view>
	</view>
</template>

<script>
import {pagination as paginationConfig} from '@/utils/config.js';
import {requestIsSuccess, getFileSize, getFileTypeIcon} from '@/utils/util.js';
import wybSlideListener from '@/components/wyb-slide-listener/wyb-slide-listener.vue';
import * as apiFileManage from '@/api/file/fileManage.js';
import * as apiFolder from '@/api/file/folder.js';
import moment from 'moment';
export default {
	components: {wybSlideListener},
	data() {
		return {
			inputVal: '',
			waitApprove: true, //顶部选择的待我审批选中状态
			scaleDrew: null, //搜索按钮的动画为1
			dataSources: [], //数据源
			skipCount: 0, // 跳过数
			hierarchy: [], //显示文件夹层级
			popupType: 'add', //弹出类型
			selected: [], //选中的列表内容
			singleFileUrl: '', //单个下载文件的url
			fileFolderInput: false, //显示新建文件夹输入框
			fileFolderInputVal: {
				organizationId: null,
				name: '',
				staticKey: '',
			},
			//新建文件夹输入框内容
			popupMessage: {
				type: 'success',
				message: '下载成功',
			},
			loading: true,
			loadingSwitch: false,
			loadNoMore: true,
			loadStatus: 'more',
			queryParams: {
				id: '00000000-0000-0000-0000-000000000000',
				type: 2,
				isMine: true,
				staticKey: '', // 静态文件夹key值
				size: 10,
				page: 0,
				name: '',
			},
			pageIndex: 1,
			maxResultCount: paginationConfig.defaultPageSize,
		};
	},
	methods: {
		getFileSize,
		getFileTypeIcon,
		onLoad(args) {
			this.queryParams.isMine = true;
			this.refresh(true, true);
		},
		//列表数据查询refresh
		async refresh(reset = true, isClearData = false) {
			//页面是否重置
			if (reset) {
				this.pageIndex = 1;
			}
			//是否清空数据源
			if (isClearData) {
				this.dataSources = [];
				this.selected = [];
			}
			this.loadStatus = 'loading';
			const response = await apiFileManage.getResourceList({
				...this.queryParams,
				page: (this.pageIndex - 1) * this.maxResultCount,
				size: this.maxResultCount,
			});
			if (requestIsSuccess(response)) {
				let response_ = response.data.items;
				if (response_.length < 10) {
					this.loadStatus = 'noMore';
				} else {
					this.loadStatus = 'more';
				}
				let listData = response_.map(item => {
					return {
						...item,
						selected: false,
						editTime: item.editTime ? moment(item.editTime).format('YYYY-MM-DD HH:mm:ss') : '--',
					};
				});
				this.dataSources = this.dataSources.concat(listData);
				this.loading = false;
				this.loadingSwitch = false;
			}
		},
		/**跳转到文件传输页 */
		onTransmission() {
			uni.navigateTo({
				url: '../../../pages/fileManage/file/fileTransfer',
			});
		},

		/**增加文件夹等操作 */
		onPopup(key) {
			switch (key) {
				case 1:
					// console.log('新建文件夹');
					this.Popup1();
					break;
				case 2:
					console.log('上传照片');
					break;
				case 3:
					console.log('拍照上传');
					break;
				case 4:
					console.log('上传视频');
					break;
				case 5:
					console.log('上传其他');
					break;
				case 6:
					// console.log('下载');
					this.Popup6();
					break;
				case 7:
					// console.log('删除');
					this.Popup7();
					break;
				case 8:
					console.log('重命名');
					break;
				case 9:
					console.log('发布到');
					break;
				default:
					break;
			}
		},
		/** Popup调用函数-开始 */
		//1.新建文件夹
		Popup1() {
			this.fileFolderInput = true;
		},
		//6.下载
		Popup6() {
			this.selected.forEach(item => {
				let FP = uni.getStorageSync('fileServerEndPoint') || '';
				let FPurl = FP && item.url ? 'http:' + FP + item.url : '';
				uni.downloadFile({
					url: FPurl,
					success: res => {
						if (res.statusCode === 200) {
							var tempFilePath = res.tempFilePath;
							uni.saveFile({
								tempFilePath: tempFilePath,
								success: function (res_) {
									var savedFilePath = res_.savedFilePath;
									console.log(savedFilePath);
								},
								fail: err => {
									this.Tips('保存失败', 'error');
								},
							});
						}
					},
					fail: err => {
						this.Tips('失败请重新下载', 'error');
					},
				});
				this.Tips('下载中...', 'success');
			});
		},
		//7.删除
		Popup7() {
			this.$refs.popup4.open();
		},
		/**结束 */

		/**顶部选择 */
		onTap(state) {
			//点击后，请求数据时不能再点击
			if (!this.loadingSwitch) {
				if (state) {
					this.queryParams = {
						type: 2,
						isMine: true,
						staticKey: '', // 静态文件夹key值
					};
				} else {
					this.queryParams = {
						id: '00000000-0000-0000-0000-000000000000',
						type: 1,
						staticKey: '', // 静态文件夹key值
					};
				}
				this.waitApprove = state ? true : false;
				this.loadingSwitch = true;
				this.refresh(true, true);
			}
		},
		onSlide(e) {
			let type = e.type;
			switch (type) {
				case 'slideUp':
					// console.log('我上滑了~');
					this.scaleDrew ? (this.scaleDrew = false) : '';
					break;
				case 'slideDown':
					// console.log('我下滑了~');
					this.scaleDrew ? (this.scaleDrew = false) : '';
					break;
			}
		},

		/**列表文件被点击 */
		clickFileFolder(item) {
			let FP = uni.getStorageSync('fileServerEndPoint') || '';
			let FPurl = FP && item.url ? 'http:' + FP + item.url : '';
			let FPurl_ = FP && item.url ? FP + item.url : '';

			if (['文件夹'].indexOf(item.type) != -1) {
				this.hierarchy.push(item);
				this.queryParams.id = item.id;
				this.loadingSwitch = true;
				this.refresh(true, true);
			} else if (['.mp4', '.rm', '.avi', '.mov', '.mpg', '.wmv', '.rmvb'].indexOf(item.type) != -1) {
				//视频
				uni.navigateTo({
					url: `../../video/video?url=${FPurl_}`,
				});
			} else if (['.png', '.jpg', '.bmp', '.tif', '.gif'].indexOf(item.type) != -1) {
				//图片
				uni.previewImage({urls: [FPurl]});
			} else {
				//其他文件下载
				this.singleFileUrl = FPurl;
				this.$refs.popup2.open();
			}
		},
		/**点击上一层 */
		upperStory() {
			this.hierarchy.pop();
			if (this.hierarchy.length != 0) {
				this.queryParams.id = this.hierarchy[this.hierarchy.length - 1].id;
			} else {
				this.queryParams.id = '00000000-0000-0000-0000-000000000000';
			}
			this.loadingSwitch = true;
			this.refresh(true, true);
		},
		/**列表加号按钮 */
		addButton() {
			this.popupType = 'add';
			this.$refs.popup.open();
		},
		/**列表右侧选择按钮 */
		selectButton(item) {
			this.popupType = 'circle';
			let id = '';
			if (item.selected) {
				item.selected = false;
				id = item.id;
			} else {
				item.selected = true;
			}
			if (id) {
				for (let i = 0; i < this.selected.length; i++) {
					const element = this.selected[i];
					element.id == id ? this.selected.splice(i, 1) : '';
				}
			} else {
				this.selected.push(item);
			}
		},
		/**列表item其他文件点击下载 */
		confirm(state) {
			if (state == '下载') {
				let url = this.singleFileUrl;
				uni.downloadFile({
					url: url,
					success: res => {
						if (res.statusCode === 200) {
							var tempFilePath = res.tempFilePath;
							uni.saveFile({
								tempFilePath: tempFilePath,
								success: function (res_) {
									var savedFilePath = res_.savedFilePath;
									uni.showToast({
										icon: 'none',
										title: '文件已保存：' + savedFilePath,
										duration: 2000,
									});
									setTimeout(() => {
										uni.openDocument({filePath: savedFilePath});
										//打开文档查看
									}, 1000);
								},
								fail: err => {
									this.Tips('保存失败', 'error');
								},
							});
							this.Tips('下载成功', 'success');
						}
					},
					fail: err => {
						this.Tips('失败请重新下载', 'error');
					},
				});
				this.close('下载');
			} else if (state == '删除') {
				this.selected.forEach(async (item, index) => {
					let response_ = await apiFileManage.deleteFile({type: item.type == '文件夹' ? 'folders' : 'files', id: item.id});
					if (requestIsSuccess(response_)) {
						this.selected.splice(index, 1);
					}
				});
				this.close('删除');
				this.refresh(true, true);
			}
		},
		close(state) {
			if (state == '下载') {
				this.$refs.popup2.close();
			} else if (state == '删除') {
				this.$refs.popup4.close();
			}
		},
		/**顶部弹出提示函数 */
		Tips(message = '下载成功', type = 'success') {
			this.popupMessage = {
				type: type,
				message: message,
			};
			this.$refs.popup3.open();
		},
		/**新建文件夹名称确认取消 */
		async fileFolderInputConfirm() {
			let parentId = this.queryParams.id;
			let _ = await apiFolder.create({
				...this.fileFolderInputVal,
				parentId: parentId,
			});
			this.$refs.popup.close();
			this.refresh(true, true);
			this.Tips('新建成功', 'success');
			this.fileFolderInputCancel();
		},
		fileFolderInputCancel() {
			this.fileFolderInput = false;
			this.fileFolderInputVal = {
				organizationId: null,
				name: '',
				staticKey: '',
			};
		},
		/**搜索栏搜索 */
		search(val) {
			this.queryParams.name = val;
			this.refresh(true, true);
		},
	},
	//页面显示
	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
	},
	//页面触底操作
	onReachBottom() {
		if (this.loadStatus === 'more') {
			this.loadNoMore = true;
			this.pageIndex += 1;
			this.refresh(false);
		}
	},
};
</script>

<style lang="scss">
page {
	background-color: #0e2a58;
}
.pages-fileManage-file-file {
	@keyframes scaleDrewUp {
		0% {
			transform: scale(1);
		}
		100% {
			transform: scale(0);
		}
	}
	@keyframes scaleDrewDown {
		0% {
			transform: scale(0);
		}
		100% {
			transform: scale(1);
		}
	}
	.search-scaleDrewUp {
		display: block !important;
		animation: scaleDrewUp 1s ease-in-out;
		animation-fill-mode: forwards;
	}
	.search-scaleDrewDown {
		display: block !important;
		animation: scaleDrewDown 1s ease-in-out;
		animation-fill-mode: forwards;
	}
	.file-top {
		position: fixed;
		z-index: 20;
		left: 0;
		right: 0;
		top: 0;
		display: flex;
		justify-content: space-between;
		align-items: center;
		background-color: #006186;
		min-height: 80rpx;
		max-height: 80rpx;
		color: #ffffff;
		padding: 0 30rpx;
		border-radius: 15rpx;
		.construction-dispatchs-oprator-view {
			height: 100%;
			display: flex;
			font-size: 28rpx;
			view {
				margin: 0 20rpx;
				height: 100%;
				line-height: 80rpx;
				color: #adadad;
			}
			.approve-selected {
				border-bottom: 4rpx solid #37a3ff;
				color: #ffffff;
			}
		}
		image {
			width: 50rpx;
			height: 50rpx;
			margin: 0 10rpx;
		}
		> uni-search-bar {
			width: 100%;
			position: absolute;
			top: 40px;
			left: 0;
		}
	}
	.file-bottom {
		position: fixed;
		z-index: 20;
		left: 0;
		right: 0;
		bottom: 0;
		display: flex;
		justify-content: space-between;
		align-items: center;
		background-color: #006186;
		min-height: 150rpx;
		max-height: 150rpx;
		color: #ffffff;
		padding: 0 70rpx;
		border-radius: 15rpx;
		> view {
			display: flex;
			flex-direction: column;
			justify-content: center;
			align-items: center;
			font-size: 24rpx;
			> image {
				width: 70rpx;
				height: 70rpx;
			}
		}
	}
	.increase {
		width: 100rpx;
		height: 100rpx;
		border-radius: 50%;
		background-image: linear-gradient(to top, #1e3c72 0%, #1e3c72 1%, #2a5298 100%);
		display: flex;
		align-items: center;
		justify-content: center;
		color: #ffffff;
		font-size: 100rpx;
		position: fixed;
		z-index: 20;
		bottom: 100rpx;
		right: 40rpx;
		> text {
			margin-bottom: 0.5rem;
		}
	}
	.uni-list-empty {
		flex: 1;
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		font-size: 28rpx;
		color: #c3c1c1;
		height: 100vh;
		.icon-empty-a {
			font-size: 120rpx;
			margin-bottom: 20rpx;
		}
	}
	.file-item {
		display: flex;
		padding: 30rpx;
		background-color: #ffffff;
		border-radius: 20rpx;
		min-height: 80rpx;
		max-height: 80rpx;
		margin: 20rpx;
		position: relative;
		> view:nth-child(1) {
			min-width: 80rpx;
			max-width: 80rpx;
			margin-right: 30rpx;
			> image {
				width: 100%;
				height: 100%;
			}
		}
		> view:nth-child(2) {
			width: 100%;
			display: flex;
			flex-direction: column;
			font-size: 24rpx;
			justify-content: space-between;
			overflow: hidden;
			text-overflow: ellipsis;
			padding-right: 30rpx;
			> view:nth-child(1) {
				> view {
					overflow: hidden; /*必须结合的属性,当内容溢出元素框时发生的事情*/
					text-overflow: ellipsis; /*可以用来多行文本的情况下，用省略号“…”隐藏超出范围的文本 。*/
					display: -webkit-box; /*必须结合的属性 ，将对象作为弹性伸缩盒子模型显示 。*/
					-webkit-line-clamp: 1; /*用来限制在一个块元素显示的文本的行数。*/
					-webkit-box-orient: vertical; /*必须结合的属性 ，设置或检索伸缩盒对象的子元素的排列方式 。*/
				}
			}
		}
		> view:nth-child(3) {
			position: absolute;
			z-index: 5;
			top: 60rpx;
			right: 30rpx;
			width: 30rpx;
			height: 30rpx;
			> image {
				position: absolute;
				width: 100%;
				height: 100%;
			}
		}
	}
	.file-popup1 {
		background-color: #ffffff;
		min-height: 300rpx;
		max-height: 300rpx;
		border-radius: 20rpx 20rpx 0 0;
		display: flex;
		align-items: center;
		justify-content: space-around;
		> text {
			position: absolute;
			z-index: 10;
			top: 10px;
		}
		> view {
			display: flex;
			flex-direction: column;
			justify-content: center;
			align-items: center;
			font-size: 24rpx;
			> image {
				width: 100rpx;
				height: 100rpx;
			}
		}
		> .file-fileFolderInput {
			flex-direction: inherit;
			align-items: center;
			font-size: 28rpx;
			input {
				border: 1rpx solid #d8d8d8;
				border-radius: 20rpx;
				padding: 10rpx 20rpx;
				min-width: 70vw;
			}
			> button {
				background-color: #00a2ec;
				color: #ffffff;
				position: absolute;
				top: 15rpx;
			}
			> .button1 {
				left: 30rpx;
				background-color: #a0a0a0;
			}
			> .button2 {
				right: 30rpx;
			}
		}
	}
}
</style>
