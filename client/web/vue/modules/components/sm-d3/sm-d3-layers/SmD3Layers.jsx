import ApiEquipments from '../../sm-api/sm-resource/Equipments';
import { treeArrayItemAddProps, treeArrayToFlatArray } from '../../_utils/tree_array_tools';
import './style/index.less';
import SmD3SetLayerModal from './SmD3SetLayerModal';

let apiEquipments = new ApiEquipments();

export default {
  name: 'SmD3Layers',
  model: {
    prop: 'value',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    dataSource: { type: Array, default: () => [] },
    value: { Array: Array, default: () => [] },
    title: { type: String, default: '图层' },
    multiple: { type: Boolean, default: true },
    showHeader: { type: Boolean, default: true },
    bordered: { type: Boolean, default: true },
    isSetLayer: { type: Boolean, default: false },
  },
  data() {
    return {
      iValue: [],
      iDataSource: [],
      flatData: [],
      isShowAll: false,
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
    dataSource: {
      handler: function (value, oldValue) {
        let date = treeArrayItemAddProps(value, 'children', [
          { sourceProp: 'name', targetProp: 'title' },
          { sourceProp: 'id', targetProp: 'value' },
          { sourceProp: 'id', targetProp: 'key' },
        ]);
        this.iDataSource = date;
        this.flatData = treeArrayToFlatArray(this.dataSource);
      },
      immediate: true,
    },
  },

  async created() { },

  mounted() { },

  methods: {
    onCheck(checkedKeys) {
      this.iValue = checkedKeys;
      this.$emit('change', this.iValue);
    },

    showAll() {
      this.isShowAll = !this.isShowAll;
      this.iValue = this.isShowAll ? this.flatData.map(item => item.id) : [];
      this.$emit('change', this.iValue);
    },

    //组织机构关联图层
    relate() {
      this.$refs.SmD3SetLayerModal.relate(this.flatData);
    },
  },

  render() {
    return (
      <sc-panel
        class="sm-d3-layers"
        flex={1}
        showHeader={this.showHeader}
        title={this.title}
        bordered={this.bordered}
        showHeaderClose={false}
        onClose={() => {
          this.$emit('close');
        }}
      >
        <div slot="icon" style="display:flex;">
          <si-integral style={{ fontSize: '24px' }} />
        </div>
        {this.dataSource.length > 0 ? (
          <div slot="headerExtraContent" >
            <span class="show-all" onClick={this.showAll}>
              {this.isShowAll ? '取消所有' : '显示所有'}
            </span>
            {this.isSetLayer ?
              <a-Tooltip title='图层设置' placement="topLeft">
                <a-icon type='setting' style='margin-left:10px;' onClick={this.relate} />
              </a-Tooltip>
              : undefined}
          </div>

        ) : (
          undefined
        )}

        {this.dataSource.length ? (
          <a-tree
            checkedKeys={this.iValue}
            checkable
            blockNode
            multiple={this.multiple}
            selectable={false}
            treeData={this.iDataSource}
            onCheck={this.onCheck}
          />
        ) : (
          <span style=" margin-top: 10px; color: #bdbdbd;" >
            无数据
          </span>
        )}
        <SmD3SetLayerModal
          ref="SmD3SetLayerModal"
          axios={this.axios}
          onSuccess={() => { this.$emit('success'); }}
        />
      </sc-panel>
    );
  },
};
