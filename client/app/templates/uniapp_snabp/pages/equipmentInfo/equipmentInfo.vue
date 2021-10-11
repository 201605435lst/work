<template>
	<view class="equipmentInfo">
		<tabs :tabTop="tabTop" @orderTap="changeTab" />

		<view class="" v-show="navIndex==0">
			<equipmentForm :title="'系统属性'" :options="defaultProperty" v-show="defaultProperty.length > 0" />

			<equipmentForm v-for="(item,index) in extendProperty" :title="item.mvdCategoryName" :key="index"
				:options="item.properties" />
		</view>

		<view class="" v-show="navIndex==1">
			<equipmentTrack :options="equipmentTrackInfos" />
		</view>
	</view>
</template>

<script>
	import tabs from './tabs.vue'
	import equipmentForm from './equipmentForm.vue'
	import equipmentTrack from './equipmentTrack.vue'
	import * as apiEquipmentInfo from '@/api/equipmentInfo/equipmentInfo.js';
	import * as apiEquipmentTrack from '@/api/equipmentInfo/equipmentTrack.js';
	import {
		getEquipmentTrackType
	} from '../../utils/util.js'
	import {
		requestIsSuccess
	} from '@/utils/util.js';
	import _ from 'lodash';
	import moment from 'moment';

	export default {
		components: {
			tabs,
			equipmentForm,
			equipmentTrack
		},
		data() {
			return {
				navIndex: 0,
				equipmentProperty: [],
				tabTop: ['设备属性', '履历信息'],
				installationEquipmentId: null,
				equipmentTrackInfos: []
			}
		},

		onLoad(eve) {
			// console.log(eve)
			this.refresh(eve.qRCode)
			// let code = "GP-02 02 11@39fe3ae4-f089-28fb-1c3b-bc4095be427f"
			// this.refresh(code)
		},

		onShow() {
			//获取token判断是否登录状态
			this.$checkToken();
		},
		computed: {
			//基础属性
			defaultProperty() {
				var aaa=this.equipmentProperty.filter(x => x.type === 1)
				console.log(aaa)
				return aaa;
			},

			//扩展属性
			extendProperty() {
				const groups = _
					.chain(this.equipmentProperty.filter(x => x.type === 2))
					.groupBy("mvdCategoryId")
					.map((value, key) => ({
						mvdCategoryId: key,
						mvdCategoryName: value[0] && value[0].mvdCategory ? value[0].mvdCategory.name : "扩展属性",
						properties: value,
					}))
					.value();

				return groups;
			},
		},

		methods: {
			changeTab(value) {
				this.navIndex = value.e
				//加载设备履历信息
				if (value.e === 1) {
					this.getTrackInfo();
				} else {
					this.equipmentTrackInfos = []
				}
			},

			//获取履历信息
			async getTrackInfo() {
				this.equipmentTrackInfos = []
				let installationEquipmentId = this.installationEquipmentId;

				let trackResponse = await apiEquipmentTrack.getByInstallationEquipmentId({
					installationEquipmentId
				})
				console.log(trackResponse)
				if (requestIsSuccess(trackResponse) && trackResponse.data) {
					trackResponse.data.map(item => {
						// console.log(item)
						let params = {
							title: getEquipmentTrackType(item.nodeType).title,
							content: item.content,
							time: moment(item.time).format("yyyy-MM-DD HH:mm:ss"),
							userName: item.userName,
							userType: getEquipmentTrackType(item.nodeType).userType,
							timeType: getEquipmentTrackType(item.nodeType).timeType
						}
						this.equipmentTrackInfos.push(params)
					})
				}
			},


			//获取设备属性信息
			async refresh(QRCode) {
				let trackResponse = await apiEquipmentTrack.getInstallationEquipmentId({
					QRCode
				})

				console.log(trackResponse.data)
				//获取设备属性信息
				if (requestIsSuccess(trackResponse)) {
					this.installationEquipmentId = trackResponse.data
					let equipmentInfoResponse = await apiEquipmentInfo.getList({
						equipmentId: this.installationEquipmentId
					})
					console.log(equipmentInfoResponse)
					if (requestIsSuccess(equipmentInfoResponse)) {
						this.equipmentProperty = equipmentInfoResponse.data
					}
				}
			}
		}
	}
</script>

<style lang="scss">
	page {
		background-color: #f2f2f2;
	}

	.equipmentInfo {}
</style>
