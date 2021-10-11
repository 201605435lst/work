import { requestIsSuccess } from '../../_utils/utils';
import { treeArrayItemAddProps } from '../../_utils/tree_array_tools';
import ApiDispatchTemplate from '../../sm-api/sm-construction/ApiDispatchTemplate';
import { dropdownStyle } from '../../_utils/config';

let apiDispatchTemplate = new ApiDispatchTemplate();

export default {
  name: 'SmConstructionDispatchTemplateSelect',
  model: {
    prop: 'value',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    value: { default: undefined },
    disabled: { type: Boolean, default: false },
    placeholder: { type: String, default: '请选择' },
    treeCheckable: { type: Boolean, default: false },
    allowClear: { type: Boolean, default: true },
    mode: { type: String, default: 'default' },
  },
  data() {
    return {
      dispatchTemplates: [], // 列表数据源
      iValue: undefined,
    };
  },
  computed: {},
  watch: {
    value: {
      handler: function (val, oldVal) {
        this.iValue = val;
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
      apiDispatchTemplate = new ApiDispatchTemplate(this.axios);
    },
    async refresh() {
      let response = await apiDispatchTemplate.getAllList({
        isAll: true,
      });
      if (requestIsSuccess(response)) {
        let _dispatchTemplates = treeArrayItemAddProps(response.data.items, 'children', [
          { sourceProp: 'name', targetProp: 'title' },
          { sourceProp: 'id', targetProp: 'value' },
          { sourceProp: 'id', targetProp: 'key' },
        ]);
        this.dispatchTemplates = _dispatchTemplates;
      }
    },

    // 搜索过滤
    filterOption(input, option) {
      return (
        option.componentOptions.children[0].text.toLowerCase().indexOf(input.toLowerCase()) >= 0
      );
    },
  },
  render() {
    return (
      <a-select
        dropdownStyle={dropdownStyle}
        showSearch
        filterOption={this.filterOption}
        disabled={this.disabled}
        allowClear={this.allowClear}
        options={this.dispatchTemplates}
        value={this.iValue}
        mode={this.mode}
        onChange={(value, option) => {
          let data = option && option.data && option.data.props ? option.data.props : null;
          this.iValue = value;
          this.$emit('input', value, data);
          this.$emit('change', value, data);
        }}
        placeholder={this.disabled ? '' : this.placeholder}
        style="width: 100%"
      ></a-select>
    );
  },
};
