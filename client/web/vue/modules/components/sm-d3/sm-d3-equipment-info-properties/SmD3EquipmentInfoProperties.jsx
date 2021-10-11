import SmD3EquipmentInfoPropertyPanel from './src/SmD3EquipmentInfoPropertyPanel';
import ApiEquipmentProperty from '../../sm-api/sm-resource/EquipmentProperty';
import { EquipmentPropertyType, EquipmentType } from '../../_utils/enum';
import { requestIsSuccess } from '../../_utils/utils';
import _ from 'lodash';

import './style/index.less';

let apiEquipmentProperty = new ApiEquipmentProperty();

export default {
  name: 'SmD3EquipmentInfoProperties',
  model: {
    prop: 'value',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    value: { type: Object, default: null }, //设备
  },
  data() {
    return {
      iValue: null,
      isShowFile: true,
      equipmentProperty: [],
    };
  },

  computed: {
    //基础属性
    defaultProperty() {
      return this.equipmentProperty.filter(x => x.type === EquipmentPropertyType.Default);
    },

    //扩展属性
    extendProperty() {
      const groups = _
        .chain(this.equipmentProperty.filter(x => x.type === EquipmentPropertyType.Extend))
        .groupBy("mvdCategoryId")
        .map((value, key) => ({
          mvdCategoryId: key,
          mvdCategoryName: value[0] && value[0].mvdCategory ? value[0].mvdCategory.name : "扩展属性",
          properties: value,
        }))
        .value();

      return groups;
    },

    //电缆属性
    cableProperty() {
      return this.equipmentProperty.filter(x => x.type === EquipmentPropertyType.CableProperty);
    },
  },

  watch: {
    value: {
      handler: async function (value, oldValue) {
        this.iValue = value;
        this.initAxios();
        this.refresh();
      },
      immediate: true,
    },
  },

  async created() {
    this.initAxios();
    this.refresh();
  },

  mounted() { },

  methods: {
    initAxios() {
      apiEquipmentProperty = new ApiEquipmentProperty(this.axios);
    },

    async refresh() {
      this.initAxios();
      if (!this.value) return;
      let response = await apiEquipmentProperty.getList({ equipmentId: this.value.id });
      if (requestIsSuccess(response)) {
        this.equipmentProperty = response.data;
      }
    },
  },

  render() {
    return (
      <div class="sm-d3-equipment-info-properties">
        <SmD3EquipmentInfoPropertyPanel
          title="系统属性"
          axios={this.axios}
          value={this.defaultProperty}
          defaultCollapseShow={true}
        />

        {this.value.type == EquipmentType.Cable ?
          <SmD3EquipmentInfoPropertyPanel
            title="电缆特性"
            axios={this.axios}
            equipmentPropertyType={EquipmentPropertyType.CableProperty}
            value={this.cableProperty}
            equipmentId={this.value.id}
            onDetial={() => this.$emit("cableCoreDetial", this.value.id)}
            onSuccess={() => {
              this.refresh();
            }}
          /> : undefined
        }

        {
          this.extendProperty.map(item => {
            return <SmD3EquipmentInfoPropertyPanel
              title={item.mvdCategoryName}
              axios={this.axios}
              equipmentPropertyType={EquipmentPropertyType.Extend}
              value={item.properties}
              equipmentId={this.value.id}
              onSuccess={() => {
                this.refresh();
              }}
            />;
          })
        }

      </div>
    );
  },
};
