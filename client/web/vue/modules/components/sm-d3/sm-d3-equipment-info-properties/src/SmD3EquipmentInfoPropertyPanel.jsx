import './SmD3EquipmentInfoPropertyPanel.less';
import ApiEquipmentProperty from '../../../sm-api/sm-resource/EquipmentProperty';
import { requestIsSuccess, getCableLayTypeTile } from '../../../_utils/utils';
import SmD3EquipmentPropertyModal from './SmD3EquipmentPropertyModal';
import { tips as tipsConfig } from '../../../_utils/config';
import { EquipmentPropertyType } from '../../../_utils/enum';

let apiEquipmentProperty = new ApiEquipmentProperty();

export default {
  name: 'SmD3EquipmentInfoPropertyPanel',
  model: {
    prop: 'value',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    value: { type: Array, default: () => [] },
    title: { type: String, default: '标题' },
    equipmentId: { type: String, default: null },
    equipmentPropertyType: { type: Number, default: 1 },
    defaultCollapseShow: { type: Boolean, default: false },
  },
  data() {
    return {
      isShowFile: true,
      isShowEdit: false,
      iValue: null,
      product: null,//库存标准设备
      collapseShow: false,//折叠
    };
  },

  computed: {},

  watch: {
    value: {
      handler: function (value, oldValue) {
        this.iValue = value;
      },
      immediate: true,
    },
  },

  async created() {
    this.collapseShow = this.defaultCollapseShow;
    this.initAxios();
  },

  mounted() { },

  methods: {
    initAxios() {
      apiEquipmentProperty = new ApiEquipmentProperty(this.axios);
    },

    add() {
      this.$refs.SmD3EquipmentPropertyModal.add(this.equipmentId);
    },

    edit(record) {
      this.$refs.SmD3EquipmentPropertyModal.edit(this.equipmentId, record);
    },

    delete(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiEquipmentProperty.delete(record.id);
            if (requestIsSuccess(response)) {
              _this.$emit('success');
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() { },
      });
    },
    showDetial() {
      this.$emit("detial");// 提交线芯信息点击
    },

  },

  render() {
    return (
      <div class="sm-d3-equipment-info-properties-panel">

        <sc-panel visible={true} title="Flex 1" flex={1} resizable={false} bordered={false} showHeaderClose={false}>

          <span slot="title" >
            <a style={{ marginRight: '4px' }}>
              <a-icon
                type={this.collapseShow ? "down" : "right"}
                onClick={() => { this.collapseShow = !this.collapseShow; }}
              />
            </a>
            {this.title}
          </span>

          {this.equipmentPropertyType === EquipmentPropertyType.Extend ?
            <span slot="headerExtraContent" class="addButton" onClick={this.add}>
              <a-icon type="plus" />
            </span>
            : undefined}
          <div style={{ display: !this.collapseShow ? 'none' : 'block' }}>
            {this.value.length > 0 ? this.value.map(item => {
              return <div class="item">
                <div class="label">{item.name}</div>

                <div class="text">
                  {item.name === '铺设类型' ? getCableLayTypeTile(parseInt(item.value)) : item.value}
                  {item.name === '皮长公里' ? (item.value ? ' (m)' : '') : null}
                </div>
                {
                  this.equipmentPropertyType !== EquipmentPropertyType.Default ? <div class="edit">

                    {this.equipmentPropertyType !== EquipmentPropertyType.CableProperty ?
                      <a-icon type="minus" onClick={() => this.delete(item)} />
                      : undefined
                    }
                    {item.name === '芯数' ?
                      <span
                        style="width: auto;
                            padding: 0px 2px;
                            border-radius: 2px;
                            "
                        onClick={() => this.showDetial()} >详情</span> : null}
                    <a-icon
                      type="edit"
                      theme="filled"
                      onClick={() => { this.edit(item); }}
                    />
                  </div> : undefined
                }

              </div>;
            }) :
              <span style="color: rgb(189, 189, 189); margin: 5px;">无数据</span>
            }
          </div>
        </sc-panel>
        <SmD3EquipmentPropertyModal
          ref="SmD3EquipmentPropertyModal"
          axios={this.axios}
          onSuccess={() => {
            this.$emit('success');
          }}
        />
      </div>
    );
  },
};
