import SmD3ScopeEquipments from '../sm-d3-scope-equipments/SmD3ScopeEquipments';
import { requestIsSuccess, parseScope, scopeIsContains } from '../../_utils/utils';
import { ScopeType } from '../../_utils/enum';
import ApiEquipment from '../../sm-api/sm-resource/Equipments';
import ApiOrganizationRltLayer from '../../sm-api/sm-resource/OrganizationRltLayer';
import SmD3Layers from '../sm-d3-layers';
import d3Manager from '../sm-d3/src/d3Manager';

import './style/index.less';

let apiEquipment = new ApiEquipment();
let apiOrganizationRltLayer = new ApiOrganizationRltLayer();

export default {
  name: 'SmD3Equipments',
  model: {
    prop: 'value',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false }, //面板是否弹出
    multiple: { type: Boolean, default: false }, //是否支持多选
    isShowLayer: { type: Boolean, default: false }, //是否显示图层组件
    scopeCode: { type: String, default: null },
    equipment: { type: Object, default: null }, //设备
    value: { type: Array, default: () => [] }, //组件传入的值 ['室内@F1`0`1', '室内@LJJC1`0`6']
    hiddens: { type: Array, default: () => [] }, //组件传入的值 ['室内@F1`0`1', '室内@LJJC1`0`6']240
    height: { type: String, default: null },
    position: {
      type: Object,
      default: () => {
        return { left: '20px', top: '20px', bottom: '20px' };
      },
    },
  },
  data() {
    return {
      iValue: [],
      selectedKeys: [],
      equipments: [],
      iVisible: false,
      groups: [],
      layers: [],
    };
  },

  computed: {
    currentUserName: () => {
      return localStorage.getItem('currentUserName');
    },
    title: function () {
      let title = '请选择（M + 左键多选）';
      if (this.value.length) {
        title += `：(${this.value.length ? this.value[this.value.length - 1] : ''}/${this.value.length
        })`;
      }
      return title;
    },
    stationName: function () {
      if (this.scopeCode) {
        let code = parseScope(this.scopeCode);
        if (code.type === ScopeType.Station) {
          return code.name;
        }
      }
      return null;
    },
  },

  watch: {
    value: {
      handler: function (value, oldValue) {
        this.iValue = this.value;
        this.initAxios();
        this.initGroup();
      },
      immediate: true,
    },
    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
      },
      immediate: true,
    },
    scopeCode: {
      handler: function (value, oldValue) {
        this.onScopeCodeChanged(value);
      },
    },
    selectedKeys: {
      handler: function (value, oldValue) {
        let names = this.layers.filter(x => value.indexOf(x.id) > -1).map(x => x.name);
        d3Manager.snBusinessHelper.setIsolateByLayers(this.stationName, names, true);
      },
    },
  },

  async created() {
    this.initAxios();
    this.initGroup();
  },

  mounted() { },

  methods: {
    initAxios() {
      apiEquipment = new ApiEquipment(this.axios);
      apiOrganizationRltLayer = new ApiOrganizationRltLayer(this.axios);
    },
    async initGroup() {
      this.iValue = this.value;
      let response = await apiEquipment.getScopesByGroupAndName({
        equipmentGroupAndNames: this.value,
      });
      if (requestIsSuccess(response)) {
        // 获取设备范围
        let scopes = JSON.parse(JSON.stringify(response.data));
        let groups = [
          {
            scopeCode: this.scopeCode,
            equipments: [],
          },
        ];

        for (let scopeItem of scopes) {
          let groupAndName = `${scopeItem.equipmentGroupName}@${scopeItem.equipmentName}`;

          // 再次验证设备范围
          if (this.iValue.indexOf(groupAndName) > -1) {
            // 找现有 group 判断是否在目标范围内
            // let targetGroup = groups.find(target =>  scopeItem.scopeCode.indexOf(target.scopeCode) > -1);
            let targetGroup = groups.find(target =>
              scopeIsContains(target.scopeCode, scopeItem.scopeCode),
            );
            if (targetGroup) {
              // 已找到：加设备
              targetGroup.equipments.push(groupAndName);
              // }
            } else {
              // 没找到，新建组
              groups.push({
                scopeCode: scopeItem.scopeCode,
                equipments: [groupAndName],
              });
            }
          }
        }
        groups = groups.sort((a, b) => {
          let sa = parseScope(a.scopeCode);
          let sb = parseScope(b.scopeCode);
          if (sa && sa.type && sb && sb.type) {
            return sa.type - sb.type;
          }
          return false;
        });
        this.groups = [...groups];
      }
    },
    async onScopeCodeChanged(scopeCodeString) {
      if (scopeCodeString) {
        // 重新载入 layaer
        if (this.stationName && d3Manager.snBusinessHelper) {
          let layers = d3Manager.snBusinessHelper.getLayers(this.stationName) || [];
          this.layers = layers.map(x => ({ id: x._id, name: x._name }));
        }
      } else {
        // 清空 layers
        this.layers = [];
      }

      this.getLayerIds();
    },

    async getLayerIds() {
      let organizationId = localStorage.getItem('OrganizationId');
      let response = await apiOrganizationRltLayer.getLayerIdsByOrganizationId(organizationId);
      if (requestIsSuccess(response)) {
        this.selectedKeys = response.data;
      }
    },
  },

  render() {
    return (
      <sc-panel
        class="sm-d3-equipments"
        title={this.title}
        bordered
        borderedRadius
        visible={this.iVisible}
        position={this.position}
        bodyFlex
        width={'260px'}
        // height={this.height}
        animate="leftEnter"
        forceRender
        // resizable
        onClose={visible => {
          this.iVisible = visible;
          this.$emit('close', this.iVisible);
        }}
      >
        <a-icon slot="icon" type="menu-unfold" />

        <span
          slot="title"
          title={this.value.length ? this.value[this.value.length - 1] : '请选择'}
          class="title"
        >
          {this.value.length
            ? `已选(1/${this.value.length})：${this.value[this.value.length - 1]}`
            : ['请选择', <span style="color: #c1c1c1">（M + 左键多选）</span>]}
        </span>

        {this.groups.map(group => {
          return (
            <SmD3ScopeEquipments
              axios={this.axios}
              showHeader={this.groups.length > 1}
              scopeCode={group.scopeCode}
              key={group.scopeCode}
              value={group.equipments}
              hiddens={this.hiddens}
              // multiple={this.multiple}
              onFlyTo={data => {
                this.$emit('flyTo', data);
              }}
              onChange={(value, equipments) => {
                equipments.map(item => {
                  if (this.equipments.find(x => x.id === item.id) == null) {
                    this.equipments.push(item);
                  }
                });
                let _iValue = this.iValue;
                for (let item of group.equipments) {
                  _iValue.splice(_iValue.indexOf(item), 1);
                }
                for (let item of value) {
                  _iValue.push(item);
                }
                this.$emit('change', _iValue, equipments);
              }}
              // onChange={value => {
              //   let _iValue = this.iValue;
              //   for (let item of group.equipments) {
              //     _iValue.splice(_iValue.indexOf(item), 1);
              //   }
              //   for (let item of value) {
              //     _iValue.push(item);
              //   }

              //   this.$emit('change', _iValue);
              // }}
              onVisibleChange={data => {
                this.$emit('visibleChange', data);
              }}
              onClose={() => {
                for (let item of group.equipments) {
                  this.iValue.splice(this.iValue.indexOf(item), 1);
                }
                this.$emit('change', this.iValue);
              }}
            />
          );
        })}
        {this.isShowLayer ? (
          <SmD3Layers
            bordered={false}
            axios={this.axios}
            dataSource={this.layers}
            value={this.selectedKeys}
            isSetLayer={this.currentUserName === 'admin'}
            onChange={values => {
              this.selectedKeys = values;
              this.$emit('layerChange', values);
            }}
            onSuccess={() => this.getLayerIds()}
          />
        ) : (
          undefined
        )}
      </sc-panel>
    );
  },
};
