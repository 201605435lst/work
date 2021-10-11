<template>
  <view class="uni-user-select">
    <view class="uni-user-select-box">
      <!-- <view class="uni-search">
				<view class="search-icon"><icon type="search" size="14" /></view>
				<input class="uni-input" type="text" placeholder="请输入用户名/姓名/邮箱" placeholder-style="color:#c3c1c1;" v-model="filter" @input="onChange" />
				<view class="search-icon" v-if="isClear" @click="onClear"><icon type="clear" size="14" /></view>
			</view> -->
      <!-- 
			<view class="uni-all">
				<radio class="uni-radio" value="r1" :checked="false" color="#1890ff" size="14" />
				<text>全部</text>
			</view> -->

			<view class="uni-list">
				<view class="list-item" v-for="item in organizations" :key="item.id">
					<view class="list-item-left">
						<view class="list-item-icon"><view class="iconfont icon-organization" /></view>
						<view class="list-item-title">{{ item.name }}</view>
					</view>

					<view v-if="item.children != null || item.parentId != null" class="list-item-right">
						<view v-if="item.parentId != null" class="iconfont icon-get-back01" @click="enterToSelect(item, true)"/>
						<view v-if="item.parentId != null" class="divider" />
						<view  class="iconfont icon-go-forward01" @click="enterToSelect(item, false)"/>
					</view>
				</view>
				<view class="list-item" v-for="item in users" :key="item.id">
					<view class="list-item-left">
						<view class="list-item-icon"><view class="iconfont icon-user1" /></view>
						<view class="list-item-title" :class="{ active: item.selected }">{{ item.name }}</view>
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
		</view>

		<view class="uni-user-select-button-box">
			<view class="uni-user-select-buttons">
				<button class="uni-user-select-button-cancel" @click="onClose" size="mini">取消</button>
				<button class="uni-user-select-button-confirm" @click="onConfirm" size="mini" style="background-color: #1890ff;color: #FFFFFF;">确认</button>
			</view>
		</view>
	</view>
</template>

<script>
import { checkToken, requestIsSuccess } from "@/utils/util.js";
import * as apiSystem from "@/api/system.js";

export default {
	name: 'memberSelect',
	data() {
		return {
			keyword: '',
			multiple: false,
			filter: '',
			isClear: false,
			organizations: [],
			users: [],
			selectedData: []
		};
	},

	watch: {
		selected: {
			handler: function(val, nVal) {
				this.selectedData =
					val && val.length < 0
						? val.map(item => {
								return {
									...item,
									selected: true
								};
						  })
						: [];
			},
			immediate: true
		}
	},

	onShow() {
		//获取token判断是否登录状态
		this.$checkToken();
		this.refresh();
	},

	onLoad: function(option) {
		//option为object类型，会序列化上个页面传递的参数
		this.keyword = option.keyword;
		this.multiple = option.multiple;
	},

	methods: {
		//数据更新
		async refresh(organizationId = '') {
			this.users = [];
			this.organizations = [];
			//组织机构数据获取
			if (!this.filter) {
				const organizationResponse = await apiSystem.getOrganizationList({ name: '', parentId: organizationId, isAll: true });
				if (requestIsSuccess(organizationResponse)) {
					this.organizations = organizationResponse.data.items;
				}
			}

			//所选组织机构下的用户获取
			if (organizationId || this.filter) {
				const userResponse = await apiSystem.getUserList({ organizationId: organizationId, isAll: true, filter: this.filter });
				if (requestIsSuccess(userResponse) && userResponse.data.totalCount > 0) {
					this.users = userResponse.data.items.map(item => {
						return {
							...item,
							selected: this.selectedData.length > 0 && this.selectedData.find(_item => _item.id === item.id) ? true : false
						};
					});
				}
			}
		},

		onChange(event) {
			this.filter = event.detail.value;
			this.refresh();
			if (event.detail.value) {
				this.isClear = true;
			} else {
				this.isClear = false;
			}
		},

		onClear() {
			this.filter = '';
			this.refresh();
			this.isClear = false;
		},

		//上下级操作
		async enterToSelect(item, isTop) {
			if (isTop) {
				this.refresh(item.parent && item.parent.parentId ? item.parent.parentId : '');
			} else {
				this.refresh(item.id);
			}
		},

		//取消操作
		onClose() {
			uni.navigateBack();
		},

		//确认操作
		onConfirm() {
			uni.$emit('select', this.keyword, this.selectedData);
			uni.navigateBack();
			this.selectedData = [];
		},

		//选中操作
		onSelected(item) {
			item.selected = !item.selected;
			if (!this.multiple) {
				this.selectedData = [];
				this.users.map(_item => {
					if (_item.id !== item.id) {
						_item.selected = false;
					}
				});
			}
			if (this.selectedData.length > 0) {
				this.users.map(item => {
					if (!this.selectedData.find(_item => _item.id == item.id) && item.selected) {
						this.selectedData.push(item);
					}
				});
			} else {
				this.selectedData = this.users.filter(item => item.selected);
			}
			this.selectedData = this.selectedData.filter(item => item.selected);
		}
	}
};
</script>

<style>
.uni-user-select {
  height: 100%;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

.uni-search {
  display: flex;
  align-items: center;
  margin: 10rpx;
  height: 80rpx;
  background-color: #f7f7f7;
  border-radius: 40rpx;
}

.search-icon {
  margin: 0 20rpx;
  width: 28rpx;
}

.uni-input {
  font-size: 28rpx;
  flex: 1;
}

.uni-radio {
  margin: 0 20rpx;
}

.uni-all {
  display: flex;
  align-items: center;
  background-color: #f7f7f7;
  height: 80rpx;
  font-size: 28rpx;
  color: #5a5a5a;
  margin: 10rpx;
  border-radius: 40rpx;
}

.list-item-title {
  font-size: 28rpx;
  text-overflow: ellipsis;
  white-space: nowrap;
  overflow: hidden;
}

.icon-get-back01 {
	margin-right: 20rpx;
	font-size: 28rpx;
	color: #1890ff;
}

.icon-go-forward01 {
	margin-right: 20rpx;
	font-size: 28rpx;
	color: #1890ff;
}

.list-item {
  display: flex;
  align-items: center;
  background-color: #f7f7f7;
  margin-bottom: 10rpx;
  height: 80rpx;
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
  margin-right: 20rpx;
}

.list-item-right {
  display: flex;
  align-items: center;
  width: 160rpx;
  justify-content: flex-end;
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
</style>
