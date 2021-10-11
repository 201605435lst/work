<template>
	<view class="tree-select">
		<view :class="isSimple ? 'simple-value-box' : 'value-box'" @click="onOpen">
			<scroll-view v-if="iSelected && iSelected.length > 0 && iSelected[0].name" :scroll-x="true">
				<view class="selected-list" v-if="iSelected && iSelected.length > 0">
					<view :class="isSimple ? 'simple-selected-item' : 'selected-item'" v-for="item in iSelected" :key="item.id">
						{{ item.name }}
					</view>
				</view>
			</scroll-view>

			<view v-else class="tree-select-placeholder">{{ placeholder }}</view>
			<view class="iconfont icon-Down"></view>
		</view>
		<uni-popup ref="popup" type="bottom" @change="onChange">
			<view v-if="!loading" class="uni-tree-select-view">
				<view class="uni-tree-select-body" v-if="showData.length > 0">
					<scroll-view :scroll-x="true" style="width: 100%">
						<view class="tree-select-title">
							<view
								class="title-item"
								v-for="(item, index) in headTitle"
								:key="item.id"
								@click="() => onTitleChange(item, index)"
							>
								<text
									style="white-space: nowrap"
									:style="{paddingRight: index == headTitle.length - 1 ? '15rpx' : ''}"
									:class="{'title-item-content': parentId == item.parentId}"
								>
									{{ item.name }}
								</text>
								<view v-if="index != headTitle.length - 1" class="iconfont icon-arrow-right" />
							</view>
						</view>
					</scroll-view>
					<scroll-view :scroll-y="true" style="height: 100%">
						<view class="tree-select-list">
							<view class="list-item" v-for="item in showData" :key="item.id">
								<view class="list-item-left" @click="onSelect(item)">
									<view class="list-item-icon">
										<template v-if="multiple">
											<view v-if="item.selected" class="iconfont icon-checked-box" />
											<view v-else class="iconfont icon-unchecked-box" />
										</template>
										<template v-else>
											<view v-if="item.selected" class="iconfont icon-radio-checked" />
											<view v-else class="iconfont icon-radio-unchecked" />
										</template>
									</view>
									<view class="list-item-title" :class="{active: item.selected}">{{ item.name }}</view>
								</view>

								<view v-if="item.children != null || item.parentId != null" class="list-item-right">
									<!-- <view v-if="item.parentId != null" class="iconfont icon-go-forward01" @click="enterToSelect(item, true)" />
								<view v-if="item.children != null && item.parentId != null" class="divider" /> -->
									<view
										v-if="item.children && item.children != null"
										class="iconfont icon-get-back01"
										@click="enterToSelect(item, false)"
									/>
								</view>
							</view>
						</view>
					</scroll-view>
				</view>

				<view v-else class="tree-select-empty">
					<view class="iconfont icon-empty-a" />
					<view class="tree-select-empty-title">暂无数据</view>
				</view>
			</view>
			<view v-else><uniLoading :fullScreen="false" /></view>
		</uni-popup>
	</view>
</template>

<script>
import service from '@/utils/service.js';
import {requestIsSuccess} from '@/utils/util.js';
import {treeArrayToFlatArray} from '@/utils/tree-array-tools.js';
import uniLoading from './uniLoading.vue';

export default {
	name: 'treeSelect',
	model: {prop: 'value', event: 'input'},
	components: {uniLoading},
	props: {
		value: {type: [Object, Array]},
		multiple: {type: Boolean, default: false},
		disabled: {type: Boolean, default: false},
		noCancel: {type: Boolean, default: false}, //单击后是否可以取消选中状态
		treeData: {type: Array, default: () => []},
		placeholder: {type: String, default: '请选择'},
		action: {type: [String, Function, Object, Promise], default: ''}, //获取数据的地址或者执行方法
		lazy: {type: Boolean, default: false}, //是否懒加载子节点，需与 load 方法结合使用
		title: {type: String, default: '顶级'}, //标题
		isSimple: {type: Boolean, default: false}, //是否简单模式
		isCurrentUserOrganizations: {type: Boolean, default: false}, //是否数据当前用户所属组织机构
		// groupCode: {type: String, default: ''}, //字典标识
	},
	data() {
		return {
			showData: [], //页面要显示的数据
			flatData: [], //扁平数组
			iSelected: [], //已选数据
			showPopup: false, //是否显示popup组件
			loading: false,
			headTitle: [{name: this.title, parentId: ''}],
			parentId: '',
		};
	},
	watch: {
		treeData: {
			handler: function (val, nVal) {
				this.onHandleData(val);
				if (val.length === 0) {
					this.refresh();
				}
			},
			immediate: true,
		},
		value: {
			handler: function (val, nVal) {
				if (this.multiple) {
					this.iSelected =
						val && val.length > 0
							? val.map(item => {
									return {
										...item,
										selected: true,
									};
							  })
							: [];
				} else {
					if (val) {
						this.iSelected = [val];
						this.iSelected.selected = true;
					} else {
						this.iSelected = [];
					}
				}
			},
			immediate: true,
		},
	},
	created() {},
	methods: {
		//数据源获取
		async refresh(parentId = '', isFirst = true) {
			let _this = this;
			if (!_this.action) return;
			let response = null;
			this.loading = true;
			if (typeof _this.action === 'string') {
				response = await service.request({
					url: `${
						isFirst && this.isCurrentUserOrganizations ? '/api/app/appOrganization/getCurrentUserOrganizations' : _this.action
					}`,
					method: 'get',
					data: {parentId, isCurrent: this.isCurrentUserOrganizations, isAll: true, groupCode: this.groupCode},
				});
				_this.getDataArray(response);
			} else {
				_this.action.then(res => {
					response = res;
					_this.getDataArray(response);
				});
			}
		},

		getDataArray(response) {
			if (requestIsSuccess(response)) {
				// console.log(response);
				if (response.data && response.data.length > 0) {
					this.onHandleData(response.data);
				} else {
					this.onHandleData(response.data.items);
				}
				this.loading = false;
			}
		},

		//数据处理
		onHandleData(data) {
			if (data && Array.isArray(data)) {
				let _this = this;
				if (_this.lazy) {
					_this.showData = [];
					data.map(item => {
						if (!_this.flatData.find(_item => _item.id == item.id)) {
							_this.flatData.push(item);
						}
					});
					_this.showData = data.map(item => {
						return {
							...item,
							selected: _this.iSelected.length > 0 && _this.iSelected.find(_item => _item.id === item.id) ? true : false,
						};
					});
				} else {
					_this.flatData = treeArrayToFlatArray(data);
					_this.flatData = _this.flatData.map(item => {
						return {
							...item,
							children: item.children && item.children.length == 0 ? null : item.children,
							selected: false,
						};
					});
					_this.flatData.map(item => {
						if (item.parentId == null) {
							_this.showData.push({
								...item,
								selected: _this.iSelected.length > 0 && _this.iSelected.find(_item => _item.id === item.id) ? true : false,
							});
						}
					});
				}
			}
		},

		//选中操作
		onSelect(item) {
			let _this = this;
			item.selected = this.noCancel ? true : !item.selected;
			let target = this.flatData.find(_item => _item.id == item.id);
			if (target) {
				target.selected = item.selected;
			}
			if (_this.multiple) {
				if (_this.iSelected.length > 0) {
					_this.showData.map(data => {
						let target = _this.iSelected.find(_item => _item.id == data.id);
						if (!target && data.selected) {
							_this.iSelected.push(data);
						} else if (target && !data.selected) {
							target.selected = item.selected;
							let index = _this.iSelected.findIndex(item => item.id === target.id);
							if (index > -1) {
								_this.iSelected.slice(index, 1);
							}
						}
					});
				} else {
					_this.iSelected = _this.flatData.filter(item => item.selected);
				}
				_this.iSelected = _this.iSelected.filter(item => item.selected);
			} else {
				_this.iSelected = [];
				_this.flatData.map(_item => {
					if (_item.id !== item.id) {
						_item.selected = false;
					}
				});
				_this.showData.map(_item => {
					if (_item.id !== item.id) {
						_item.selected = false;
					}
				});
				_this.iSelected = _this.flatData.filter(item => item.selected);
			}
			this.$emit('input', this.multiple ? this.iSelected : this.iSelected[0]);
		},

		onOpen() {
			if (!this.disabled) {
				this.flatData = [];
				this.showData = [];
				this.$refs.popup.open();
				this.refresh();
				this.headTitle = [{name: this.title, parentId: ''}];
				this.parentId = '';
			}
		},

		//标题切换
		onTitleChange(item, index) {
			let _this = this;
			if (_this.parentId == item.parentId) return;
			this.showData = [];
			if (_this.lazy) {
				_this.refresh(item.parentId, index == 0 ? true : false);
			} else {
				console.log(this.flatData);
				console.log(item);
				_this.flatData.map(_item => {
					if (item.parentId) {
						if (_item.parentId === item.parentId) {
							_this.showData.push(_item);
						}
					} else {
						if (_item.parentId == null) {
							_this.showData.push(_item);
						}
					}
				});
			}
			_this.headTitle.splice(index + 1, _this.headTitle.length - index - 1);
			_this.parentId = item.parentId;
		},

		onChange(event) {
			let _this = this;
			if (event.show) {
				_this.showData.map(item => {
					let target = _this.iSelected.find(_item => _item.id == item.id);
					if (target) {
						item.selected = true;
					}
				});
			} else {
				this.flatData = [];
				this.showData = [];
				this.$emit('input', this.multiple ? this.iSelected : this.iSelected[0]);
			}
		},

		//上下级操作
		enterToSelect(item) {
			this.showData = [];
			let _this = this;
			if (_this.lazy) {
				_this.refresh(item.id, false);
			} else {
				_this.flatData.map(_item => {
					if (_item.parentId === item.id) {
						_this.showData.push(_item);
					}
				});
			}
			this.parentId = item.id;
			this.headTitle.push({name: item.name, parentId: item.id});
			this.showData.map(item => {
				let target = this.iSelected.find(_item => _item.id == item.id);
				if (target) {
					item.selected = true;
				}
			});
		},
	},
};
</script>

<style lang="scss">
.value-box {
	background-color: #ffffff;
	color: #c3c1c1;
	height: 70rpx;
	border-radius: 10rpx;
	font-size: 28rpx;
	padding: 0 20rpx;
	display: flex;
	align-items: center;
	justify-content: space-between;
}

.simple-value-box {
	color: #c3c1c1;
	height: 70rpx;
	border-radius: 10rpx;
	font-size: 28rpx;
	padding: 0 20rpx;
	display: flex;
	align-items: center;
	justify-content: space-between;
}
.tree-select {
	width: 100%;

	.selected-item {
		white-space: nowrap;
		display: inline-block;
		background-color: #1890ff;
		color: #ffffff;
		border-radius: 40rpx;
		padding: 4rpx 10rpx;
		margin: 0 0 0 10rpx;
		font-size: 28rpx;
		white-space: nowrap;
	}

	.simple-selected-item {
		white-space: nowrap;
		display: inline-block;
		color: #ffffff;
		border-radius: 40rpx;
		padding: 4rpx 10rpx;
		margin: 0 0 0 10rpx;
		font-size: 28rpx;
		white-space: nowrap;
	}
}
.icon-Down {
	color: #c3c1c1;
	font-size: 28rpx;
}
.tree-select-placeholder {
	color: #c3c1c1;
}

.selected-list {
	display: flex;
}

.uni-tree-select-body {
	background-color: #ffffff;
	padding-top: 20rpx;
}

.tree-select-title {
	display: flex;
	align-items: center;
	padding: 10rpx 20rpx;
	.title-item {
		display: flex;
		align-items: center;

		.title-item-content {
			color: #1890ff;
		}
		.icon-arrow-right {
			font-size: 40rpx;
			color: #c5c5c5;
		}
	}
}

.tree-select-list {
	background-color: #ffffff;
	height: 500rpx;
}

.list-item {
	display: flex;
	align-items: center;
	background-color: #f7f7f7;
	margin-bottom: 15rpx;
	height: 70rpx;
	justify-content: space-between;
}

.list-item-left {
	display: flex;
	align-items: center;
	flex: 1;
	overflow: hidden;
	white-space: nowrap;
	text-overflow: ellipsis;
	margin-right: 20rpx;
}

.list-item-icon {
	display: flex;
	margin: 0 20rpx;
	align-items: center;
	justify-content: center;
}

.list-item-right {
	display: flex;
	align-items: center;
	width: 160rpx;
	justify-content: flex-end;
	padding-right: 20rpx;
}

.list-item-title {
	font-size: 24rpx;
	text-overflow: ellipsis;
	white-space: nowrap;
	overflow: hidden;
}

.icon-get-back01 {
	font-size: 28rpx;
	color: #1890ff;
}

.icon-go-forward01 {
	font-size: 28rpx;
	color: #1890ff;
}

.icon-checked-box {
	color: #1890ff;
	font-size: 40rpx;
}

.icon-unchecked-box {
	color: #c5c5c5;
	font-size: 40rpx;
}

.icon-radio-checked {
	color: #1890ff;
	font-size: 40rpx;
}

.icon-radio-unchecked {
	color: #c5c5c5;
	font-size: 40rpx;
}

.tree-select-empty {
	height: 500rpx;
	background-color: #ffffff;
	border-radius: 10rpx 10rpx 0 0;
	display: flex;
	flex-direction: column;
	align-items: center;
	justify-content: center;
	color: #a5a5a5;
}

.icon-empty-a {
	font-size: 100rpx;
	margin-bottom: 20rpx;
}

.active {
	color: #1890ff;
}

.divider {
	background: #dfdfdf;
	height: 24rpx;
	width: 1rpx;
	margin: 0 20rpx;
	padding: 1rpx;
}
</style>
