import { requestIsSuccess, getNodeTypeTitle } from '../../_utils/utils';
import { NodeType } from '../../_utils/enum';
import ApiComponentTrackRecord from '../../sm-api/sm-resource/ComponentTrackRecord';
let apiComponentTrackRecord = new ApiComponentTrackRecord();
import moment from 'moment';
import './style';
export default {
  name: 'SmResourceQrcodeRecord',
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
      qrCodeRecords: [],
      loading: false,
      iValue: null,
      disabledPerson: false,
      histories: [],
    };
  },
  computed: {

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
    showPerson: {
      handler: function (value) {
        this.disabledPerson = value;
      },
      immediate: true,
    },

  },
  async created() {
    this.initAxios();
  },

  methods: {
    initAxios() {
      apiComponentTrackRecord = new ApiComponentTrackRecord(this.axios);
    },
    async refresh() {
      this.loading = true;
      let response = null;
      if (this.iValue) {
        response = await apiComponentTrackRecord.get(this.iValue);
        if (requestIsSuccess(response) && response.data) {
          this.qrCodeRecords = response.data;
        }
        this.loading = false;
      }
    },
    getIconByName(type) {
      let icon = null;
      switch (type) {
      case NodeType.CheckOut:
        icon = <a-icon style={{ color: '#858585' }} type="check-circle" />;
        break;
      case NodeType.PutStorage:
        icon = <a-icon style={{ color: 'rgb(68 199 111/1)' }} type="home" />;
        break;
      case NodeType.OutStorage:
        icon = <a-icon style={{ color: '#cb4949' }} type="inbox" />;
        break;
      case NodeType.ToTest:
        icon = <a-icon style={{ color: '#61ccd0' }} type="safety" />;
        break;
      case NodeType.Install:
        icon = <a-icon style={{ color: '#b7ae39' }} type="control" />;
        break;
      case NodeType.Alignment:
        icon = <a-icon style={{ color: '#52ad26' }} type="zoom-in" />;
        break;
      }
      return icon;
    },
  },
  render() {
    return (
      <div
        class={{ "sm-resource-qrcode-record": true, 'sm-resource-qrcode-record-mini': this.size === 'small' }}
        style={{ width: this.width instanceof String ? this.width + 'px' : this.width }}
      >
        <a-steps direction="vertical" size={this.size} class={{ 'title-icon': this.size === 'small' }}>
          {
            this.qrCodeRecords.map(item => {
              let name=item && item.user ?item.user.name:'';
              let title = name + '  ' + moment(item.time).format('YYYY-MM-DD') + '   ' + getNodeTypeTitle(item.nodeType);
              return (
                <a-step title={
                  <div class="title">
                    <a-tooltip placement="bottomLeft" title={title}>
                      <div class="title-left" >{title}</div>
                    </a-tooltip>
                    <div>
                      {/* <div class="title-middle"></div>
                      <div class="title-right">
                        <span class="title-right-personInfo">{('人员：' + item.creatorName)}</span>
                        <span class="title-right-showInfo">{('人员：' + item.creatorName)}</span>
                      </div> */}
                    </div>
                  </div>
                }>
                  <span slot="icon">{this.getIconByName(item.nodeType)}</span>
                </a-step>
              );
            })
          }
        </a-steps>
      </div>
    );
  },
};
