import { requestIsSuccess } from '../../_utils/utils';
import { treeArrayItemAddProps, treeArrayToFlatArray } from '../../_utils/tree_array_tools';
import ApiDataDictionary from '../../sm-api/sm-system/DataDictionary';
import { dropdownStyle } from '../../_utils/config';
let apiDataDictionary = new ApiDataDictionary();

export default {
  name: 'SmSystemDataDictionaryTreeSelect',
  props: {
    axios: { type: Function, default: null },
    value: { type: [Array, String] },
    groupCode: { type: String, default: null },
    disabled: { type: Boolean, default: false },
    multiple: { type: Boolean, default: false }, //是否多选，默认单选
    allowClear: { type: Boolean, default: true },
    placeholder: { type: String, default: '请选择' },
    treeCheckStrictly: { type: Boolean, default: false }, //父子级是否严格
    ignore: { type: [Array, String] },// 需要忽略的类型，通过逗号分隔
    size: { type: String, default: 'default' },// 组件的尺寸
  },
  data() {
    return {
      dataDictonaries: [], // 列表数据源
      iValue: this.multiple ? [] : null,
    };
  },
  computed: {
    dataResource() {
      if (this.ignore instanceof String) {
        // 单个key值忽略
        return this.dataDictonaries.filter(a => a.key != this.ignore);
      } else
        return this.dataDictonaries;
    },
  },
  watch: {
    value: {
      handler(nVal, oVal) {
        this.iValue=nVal;
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
      apiDataDictionary = new ApiDataDictionary(this.axios);
    },

    async refresh() {
      this.dataDictonaries = [];
      let response = await apiDataDictionary.getValues({ groupCode: this.groupCode });
      this.$emit('res', response.data.length);//返回值
      if (requestIsSuccess(response)) {
        let _dics = treeArrayItemAddProps(response.data, 'children', [
          { sourceProp: 'name', targetProp: 'title' },
          { sourceProp: 'id', targetProp: 'value' },
          { sourceProp: 'id', targetProp: 'key' },
        ]);
        this.dataDictonaries = _dics;
      }
    },
  },
  render() {
    return (
      <a-tree-select
        showCheckedStrategy="SHOW_ALL"
        dropdownStyle={dropdownStyle}
        disabled={this.disabled}
        tree-checkable={this.multiple}
        allowClear={this.allowClear}
        treeNodeFilterProp="title"
        treeCheckStrictly={this.treeCheckStrictly}
        treeData={this.dataResource}
        size={this.size}
        value={
          this.multiple && this.iValue ? this.iValue.map(item => {
            return {
              label: this.dataResource.find(_item => _item.id === item) ? this.dataResource.find(_item => _item.id === item).name : '',
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
