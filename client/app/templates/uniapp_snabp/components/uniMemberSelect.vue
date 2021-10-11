<template>
	<view class="uni-member-select">
		<view class="member-value-box" @click="onOpen">
			<scroll-view class="value-scroll-view" v-if="iSelected && iSelected.length > 0 && iSelected[0].name != null" scroll-x="true">
				<view class="member-selected-list" v-if="iSelected && iSelected.length > 0">
					<view class="member-selected-item" v-for="item in iSelected" :key="item.id">{{ item.name }}</view>
				</view>
			</scroll-view>

			<view v-else class="tree-select-placeholder">{{ placeholder }}</view>
			<view class="iconfont icon-Down"></view>
		</view>
		<uni-popup ref="popup" type="bottom" @change="onChange">
			<view class="uni-member-popup">
				<view class="uni-member-popup-body">
					<view class="member-pupup-title">
						<view v-if="multiple" @tap="onAllSelect">
							<view v-if="!allSelect" class="iconfont icon-unchecked-box" />
							<view v-else class="iconfont icon-checked-box" />
						</view>
						<scroll-view :scroll-x="true">
							<view style="display: flex">
								<view
									class="title-item"
									v-for="(item, index) in headTitle"
									:key="item.id"
									@click="() => onTitleChange(item, index)"
								>
									<text style="white-space: nowrap" :class="{'title-item-content': parentId == item.parentId}">
										{{ item.name }}
									</text>
									<view v-if="index != headTitle.length - 1" class="iconfont icon-arrow-right" />
								</view>
							</view>
						</scroll-view>
					</view>
					<view class="uni-member-select-box" v-if="!loading">
						<scroll-view :scroll-y="true" style="height: 100%">
							<view v-if="organizations.length !== 0 || users.length !== 0">
								<view class="list-item" v-for="item in organizations" :key="item.id">
									<view class="list-item-left">
										<view class="list-item-icon"><view class="iconfont icon-organization" /></view>
										<view class="list-item-title">{{ item.name }}</view>
									</view>

									<view v-if="item.children != null || item.parentId != null" class="list-item-right">
										<view class="list-item-right-icon" @click="enterToSelect(item, false)">
											<view class="iconfont icon-get-back01" />
										</view>
									</view>
								</view>
								<view class="list-item" v-for="item in users" :key="item.id">
									<view class="list-item-left">
										<view class="list-item-icon"><view class="iconfont icon-user1" /></view>
										<view class="list-item-title" :class="{active: item.selected}">{{ item.name }}</view>
									</view>

									<view class="list-item-right" @click="onSelected(item)">
										<template v-if="multiple">
											<view v-if="!item.selected" class="iconfont icon-unchecked-box" />
											<view v-else class="iconfont icon-checked-box" />
										</template>
										<template v-else>
											<view v-if="!item.selected" class="iconfont icon-radio-unchecked" />
											<view v-else class="iconfont icon-radio-checked" />
										</template>
									</view>
								</view>
							</view>
							<view v-else class="tree-select-empty">
								<view class="iconfont icon-empty-a" />
								<view class="tree-select-empty-title">暂无数据,点击返回</view>
							</view>
						</scroll-view>
					</view>

					<view v-else class="uni-loading-more-page"><uni-load-more status="loading"></uni-load-more></view>
				</view>
			</view>
		</uni-popup>
	</view>
</template>

<script>
import {checkToken, requestIsSuccess} from '@/utils/util.js';
import * as apiSystem from '@/api/system.js';

export default {
	name: 'uniMemberSelect',
	model: {prop: 'value', event: 'input'},
	props: {
		value: {
			type: [Object, Array],
		}, //父组件传进来的值
		multiple: {
			type: Boolean,
			default: false,
		}, //是否为多选
		disabled: {
			type: Boolean,
			default: false,
		}, //是否禁用
		placeholder: {
			type: String,
			default: '请选择',
		}, //输入框默认值
	},
	data() {
		return {
			loading: false,
			keyword: '',
			filter: '',
			organizations: [],
			users: [],
			iSelected: [],
			headTitle: [{name: '通讯录', parentId: ''}],
			parentId: '',
			allSelect: false, // 是否全部选择
		};
	},

	watch: {
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
					}
				}
			},
			immediate: true,
		},
	},

	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
		this.refresh();
	},

	onLoad: function (option) {},

	methods: {
		//数据更新
		async refresh(organizationId = '') {
			this.users = [];
			this.organizations = [];
			//组织机构数据获取
			this.loading = true;
			if (!this.filter) {
				const organizationResponse = await apiSystem.getOrganizationList({
					name: '',
					parentId: organizationId,
					isAll: true,
				});
				if (requestIsSuccess(organizationResponse)) {
					this.organizations = organizationResponse.data.items;
				}
			}

			//所选组织机构下的用户获取
			if (organizationId || this.filter) {
				const userResponse = await apiSystem.getUserList({
					organizationId: organizationId,
					isAll: true,
					filter: this.filter,
				});
				if (requestIsSuccess(userResponse) && userResponse.data.totalCount > 0) {
					this.users = userResponse.data.items.map(item => {
						return {
							...item,
							selected: this.iSelected.length > 0 && this.iSelected.find(_item => _item.id === item.id) ? true : false,
						};
					});
				}
			}
			this.loading = false;
		},

		onTitleChange(item, index) {
			if (this.parentId == item.parentId) return;
			this.refresh(item.parentId);
			this.headTitle.splice(index + 1, this.headTitle.length - index - 1);
			this.parentId = item.parentId;
		},

		onOpen() {
			if (!this.disabled) {
				this.flatData = [];
				this.showData = [];
				this.$refs.popup.open();
				this.refresh();
				this.headTitle = [{name: '通讯录', parentId: ''}];
				this.parentId = '';
			}
		},

		onChange(event) {
			let _this = this;
			if (event.show) {
				_this.users.map(item => {
					let target = _this.iSelected.find(_item => _item.id == item.id);
					if (target) {
						item.selected = true;
					}
				});
			} else {
				this.users = [];
				this.organizations = [];
				this.$emit('input', this.multiple ? this.iSelected : this.iSelected[0]);
			}
		},

		//上下级操作
		async enterToSelect(item) {
			this.parentId = item.id;
			this.refresh(item.id);
			this.headTitle.push({name: item.name, parentId: item.id});
		},
		//选中全部操作
		onAllSelect() {
			this.allSelect = !this.allSelect;
			this.users.map(item => {
				item.selected = !this.allSelect;
			});
			this.users.map(item => {
				this.onSelected(item);
			});
		},
		//选中操作
		onSelected(item) {
			item.selected = !item.selected;
			if (!this.multiple) {
				this.iSelected = [];
				this.users.map(_item => {
					if (_item.id !== item.id) {
						_item.selected = false;
					}
				});
			}
			if (this.iSelected.length > 0) {
				this.users.map(data => {
					let target = this.iSelected.find(_item => _item.id == data.id);
					if (!target && data.selected) {
						this.iSelected.push(data);
					} else if (target && !data.selected) {
						target.selected = item.selected;
						let index = this.iSelected.findIndex(item => item.id === target.id);
						if (index > -1) {
							this.iSelected.slice(index, 1);
						}
					}
				});
			} else {
				this.iSelected = this.users.filter(item => item.selected);
			}
			this.iSelected = this.iSelected.filter(item => item.selected);
			this.$emit('input', this.multiple ? this.iSelected : this.iSelected[0]);
		},
	},
};
</script>

<style lang="scss">
.uni-member-select {
	width: 100%;
	font-size: 28rpx;
}

.member-value-box {
	background-color: #ffffff;
	color: #c3c1c1;
	height: 70rpx;
	padding: 0 20rpx;
	border-radius: 10rpx;
	display: flex;
	align-items: center;
	justify-content: space-between;
}

.value-scroll-view {
	padding-right: 10rpx;
	display: inline-block;
	overflow: hidden;
	white-space: nowrap;
}

.icon-Down {
	color: #c3c1c1;
	font-size: 28rpx;
}

.tree-select-placeholder {
	color: #c3c1c1;
}

.member-selected-list {
	/* display: -webkit-box; */
	display: flex;
}

.member-selected-item {
	display: inline-block;
	background-color: #1890ff;
	color: #ffffff;
	border-radius: 40rpx;
	padding: 4rpx 10rpx;
	margin: 0 0 0 10rpx;
	font-size: 28rpx;
	white-space: nowrap;
}

.list-item-title {
	font-size: 28rpx;
	text-overflow: ellipsis;
	white-space: nowrap;
	overflow: hidden;
}

.icon-go-forward01 {
	/* margin-right: 20rpx; */
	font-size: 28rpx;
	color: #1890ff;
}

.uni-member-popup-body {
	background-color: #ffffff;
	/* border-radius: 20rpx 20rpx 0 0; */
	padding-top: 20rpx;
}

.member-pupup-title {
	display: flex;
	align-items: center;
	padding: 10rpx 80rpx;
	padding-left: 30rpx;
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

.uni-member-popup {
	.uni-member-select-box {
		height: 500rpx;
		overflow: auto;
	}

	.uni-loading-more-page {
		background-color: #f2f2f6;
		display: flex;
		align-items: center;
		justify-content: center;
		border-radius: 10rpx 10rpx 0 0;
		height: 500rpx;
	}
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

.divider {
	background: #dfdfdf;
	height: 24rpx;
	width: 1rpx;
	padding: 1rpx;
	/* margin-right: 20rpx; */
}

.list-item-right {
	display: flex;
	align-items: center;
	width: 160rpx;
	justify-content: flex-end;

	.list-item-right-icon {
		width: 50rpx;
		display: flex;
		justify-content: center;
		margin-right: 20rpx;
		.icon-get-back01 {
			font-size: 28rpx;
			color: #1890ff;
		}
	}
}

.list-item-right-organization {
	justify-content: space-around;
}

.radio-icon {
	height: 44rpx;
	width: 44rpx;
	background-color: white;
	border-radius: 50%;
	border: solid 1rpx #d6d5d5;
}

.icon-checked-box {
	color: #1890ff;
	font-size: 44rpx;
	margin-right: 20rpx;
}

.icon-unchecked-box {
	color: #c5c5c5;
	font-size: 44rpx;
	margin-right: 20rpx;
}

.icon-radio-checked {
	color: #1890ff;
	font-size: 44rpx;
	margin-right: 20rpx;
}

.icon-radio-unchecked {
	color: #c5c5c5;
	font-size: 44rpx;
	margin-right: 20rpx;
}

.icon-organization {
	font-size: 40rpx;
	color: #ff0a0e;
}

.icon-user1 {
	font-size: 40rpx;
	color: #1890ff;
}

.active {
	color: #1890ff;
}

.uni-user-select-buttons {
	width: 300rpx;
	display: flex;
	justify-content: space-between;
	margin-bottom: 14rpx;
}

.uni-user-select-button-box {
	display: flex;
	justify-content: flex-end;
}

.tree-select-empty {
	min-height: 500rpx;
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
</style>
