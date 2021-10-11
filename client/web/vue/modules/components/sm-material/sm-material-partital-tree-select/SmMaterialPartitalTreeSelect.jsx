import { requestIsSuccess } from '../../_utils/utils';
import { treeArrayItemAddProps, treeArrayToFlatArray } from '../../_utils/tree_array_tools';
import ApiPartition from '../../sm-api/sm-material/Partition';
import { dropdownStyle } from '../../_utils/config';
let apiPartition = new ApiPartition();

export default {
  name: 'SmMaterialPartitalTreeSelect',
  model: {
    prop: 'value',
    event: ['input', 'change'],
  },
  props: {
    axios: { type: Function, default: null },
    value: { type: [Array, String] },
    disabled: { type: Boolean, default: false },
    multiple: { type: Boolean, default: false }, //是否多选，默认单选
    allowClear: { type: Boolean, default: true },
    placeholder: { type: String, default: '请选择' },
    treeCheckStrictly: { type: Boolean, default: false }, //父子级是否严格
    size: { type: String, default: 'default' },
  },
  data() {
    return {
      partitions: [], // 列表数据源
      iValue: this.multiple ? [] : null,
    };
  },
  computed: {},
  watch: {
    value: {
      handler(nVal, oVal) {
        this.iValue = nVal;
      },
      immediate: true,
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiPartition = new ApiPartition(this.axios);
    },

    async refresh() {
      this.partitions = [];
      let response = await apiPartition.getTreeList();
      this.$emit('res', response.data.length);//返回值
      if (requestIsSuccess(response)) {
        let _partitions = treeArrayItemAddProps(response.data, 'children', [
          { sourceProp: 'name', targetProp: 'title' },
          { sourceProp: 'id', targetProp: 'value' },
          { sourceProp: 'id', targetProp: 'key' },
        ]);
        this.partitions = _partitions;
      }
    },
  },
  render() {
    return (
      <a-tree-select
        showCheckedStrategy="SHOW_ALL"
        dropdownStyle={dropdownStyle}
        disabled={this.disabled}
        size={this.size}
        tree-checkable={this.multiple}
        allowClear={this.allowClear}
        treeNodeFilterProp="title"
        treeCheckStrictly={this.treeCheckStrictly}
        treeData={this.partitions}
        value={
          this.multiple && this.iValue ? this.iValue.map(item => {
            return {
              label: this.partitions.find(item => item.id === item) ? this.partitions.find(item => item.id === item).name : '',
              value: item,
            };
          }) : this.iValue}
        onChange={value => {
          let _value = this.multiple && value ? value.map(item => item.value) : value;
          this.iValue = _value;
          this.$emit('input', _value);
          this.$emit('change', _value);
        }}
        placeholder={this.disabled ? '' : this.placeholder}
        style="width: 100%"
      ></a-tree-select>
    );
  },
};
