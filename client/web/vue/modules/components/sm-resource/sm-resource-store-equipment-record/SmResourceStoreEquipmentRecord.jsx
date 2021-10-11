import { requestIsSuccess, getStoreEquipmentTestPassed, getStoreEquipmentTransferTypeEnable, getNodeTypeTitle } from '../../_utils/utils';
import { StoreEquipmentTestState, StoreEquipmentTransferTypeEnable, NodeType } from '../../_utils/enum';
import moment from 'moment';
import ApiUser from '../../sm-api/sm-system/User';
import ApiStoreEquipment from '../../sm-api/sm-resource/StoreEquipments';
import ApiEquipments from '../../sm-api/sm-resource/EquipmentProperty';
import ApiComponentTrackRecord from '../../sm-api/sm-resource/ComponentTrackRecord';
import { data } from 'autoprefixer';
import './style/index.less';

let apiStoreEquipment = new ApiStoreEquipment();
let apiUser = new ApiUser();
let apiEquipments = new ApiEquipments();
let apiComponentTrackRecord = new ApiComponentTrackRecord();

export default {
  name: 'SmResourceStoreEquipmentRecord',
  props: {
    width: { type: [Number, String], default: '100%' },
    axios: { type: Function, default: null },
    value: { type: String, default: null },//返回值
    iconParameter: { type: Number, default: 1 },//图标显示参数(1|2),第一套图标|第二套图标
    showPerson: { type: Boolean, default: false },//是否显示人员信息
    showInfo: { type: Boolean, default: false },//是否显示详情
    size: { type: String, default: 'default' },
  },
  data() {
    return {
      trackRecords: [],
      loading: false,
      iValue: null,
    };
  },

  watch: {
    value: {
      handler: function (value) {
        this.iValue = value;
        this.initAxios();
        this.refresh();
      },
      immediate: true,
    },
  },
  async created() {
    // this.initAxios();
    // this.refresh();
  },

  methods: {
    initAxios() {
      apiUser = new ApiUser(this.axios);
      apiStoreEquipment = new ApiStoreEquipment(this.axios);
      apiEquipments = new ApiEquipments(this.axios);
      apiComponentTrackRecord = new ApiComponentTrackRecord(this.axios);
    },
    async refresh() {
      this.loading = true;
      let response = null;
      if (this.iValue) {
        response = await apiComponentTrackRecord.getByInstallationEquipmentId(this.iValue);
        if (requestIsSuccess(response) && response.data) {
          this.trackRecords = response.data;
        }
        this.loading = false;
      }
    },
    getIconByName(type) {
      // ToTest: 4, //到场验收
      // CheckOut: 1, //检验
      // PutStorage: 2, //入库
      // OutStorage: 3, //出库
      // Install: 5, //安装
      // Alignment: 6, //调试

      let icon = null;
      switch (type) {
      case NodeType.ToTest:
        icon = <a-icon slot="icon" style={{ color: '#858585' }} type="qrcode" />;
        break;
      case NodeType.CheckOut:
        icon = <a-icon slot="icon" style={{ color: 'rgb(68 199 111/1)' }} type="check-circle" />;
        break;
      case NodeType.PutStorage:
        icon = <a-icon slot="icon" style={{ color: '#cb4949' }} type="import" />;
        break;
      case NodeType.OutStorage:
        icon = <a-icon slot="icon" style={{ color: '#61ccd0' }} type="export" />;
        break;
      case NodeType.Install:
        icon = <a-icon slot="icon" style={{ color: '#b7ae39' }} type="up-square" />;
        break;
      case NodeType.Alignment:
        icon = <a-icon slot="icon" style={{ color: '#52ad26' }} type="check-circle" />;
        break;
      }
      return icon;
    },
  },
  render() {
    return (
      <div
        class={{ "sm-resource-store-equipment-record": true, 'sm-resource-store-equipment-record-mini': this.size === 'small' }}
        style={{ width: this.width instanceof String ? this.width + 'px' : this.width }}
      >
        <a-steps direction="vertical" size={this.size} class={{ 'title-icon': this.size === 'small' }}>
          {
            this.trackRecords.map(item => {
              let title = moment(item.time).format('YYYY-MM-DD HH:mm:ss') + '  ' + getNodeTypeTitle(item.nodeType)
              return (
                <a-step
                  title={
                    <div class="title">
                      {title}
                    </div>
                  }
                >
                  <span slot="description" >{item.content}</span>
                  <template slot="icon">{this.getIconByName(item.nodeType)}</template>
                </a-step>
              );
            })
          }
        </a-steps>
      </div>
    );
  },
};
