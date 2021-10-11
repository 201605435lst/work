import { requestIsSuccess } from '../../_utils/utils';
import { treeArrayItemAddProps, treeArrayToFlatArray } from '../../_utils/tree_array_tools';
import ApiProblemCategory from '../../sm-api/sm-problem/ProblemCategory';
import { dropdownStyle } from '../../_utils/config';

let apiProblemCategory = new ApiProblemCategory();

export default {
  name: 'SmProblemProblemCategoryTreeSelect',
  model: {
    prop: 'value',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    value: { type: [Array, String], default: null }, //返回值
    disabled: { type: Boolean, default: false }, //是否禁用
    placeholder: { type: String, default: '请选择' },
    multiple: { type: Boolean, default: false }, //是否多选
    maxTagCount: { type: Number, default: 2 }, //多选状态下最多显示tag数
    allowClear: { type: Boolean, default: true }, //是否清除
    showSearch: { type: Boolean, default: false }, //是否显示搜索
    disabledIds: { type: Array, default: () => [] }, //禁用层级id
    childrenIsDisabled: { type: Boolean, default: false }, //设置子元素禁用状态
  },
  data() {
    return {
      problemCategories: [], // 列表数据源
      problemCategoriesFlat: [],//平状数据源
      iValue: null,
      iDisabledIds: [],
    };
  },
  computed: {},
  watch: {
    value: {
      handler: function (val, oldVal) {
        this.setValue();
      },
      immediate: true,
    },
    disabledIds: {
      handler: function (val, oldVal) {
        this.iDisabledIds = val;
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
  methods: {
    initAxios() {
      apiProblemCategory = new ApiProblemCategory(this.axios);
    },
    async refresh() {
      let response = await apiProblemCategory.getList({ isAll: true });
      if (requestIsSuccess(response)) {
        let _problemCategories = treeArrayItemAddProps(response.data.items, 'children', [
          {
            targetProp: 'title',
            handler: item => {
              let result = item.name.length > 10 ? `${item.name.substring(0, 10)}...` : item.name;
              return <div title={item.name}>{result}</div>;
            },
          },
          { sourceProp: 'id', targetProp: 'value' },
          { sourceProp: 'id', targetProp: 'key' },
          {
            targetProp: 'disabled',
            handler: item => {
              return this.iDisabledIds.indexOf(item.id) > -1;
            },
          },
        ]);
        this.problemCategoriesFlat = treeArrayToFlatArray(_problemCategories);
        if (this.childrenIsDisabled) {
          this.setChildrenDisabled(_problemCategories, false);
        }
        this.problemCategories = _problemCategories;

        if (this.value) {
          this.setValue();
        }
      }
    },

    // 多选模式下，value 值格式为：{value,label}格式
    async setValue() {
      if (this.multiple) {
        this.iValue = this.value && this.value.length > 0
          ? this.problemCategoriesFlat
            .filter(item => {
              if (this.value.indexOf(item.id) > -1) {
                return true;
              }
            })
            .map(item => {
              return {
                value: item.id,
                label: item.name,
              };
            })
          : [];
      } else {
        this.iValue = this.value;
      }
    },

    //设置子元素禁用状态
    setChildrenDisabled(problemCategories, disabled) {
      problemCategories.map(item => {
        item.disabled = disabled ? disabled : item.disabled;
        if (item.disabled && item.children.length > 0) {
          this.setChildrenDisabled(item.children, item.disabled);
        } else {
          this.setChildrenDisabled(item.children, false);
        }
      });
    },
  },
  render() {
    return (
      <a-tree-select
        dropdownStyle={dropdownStyle}
        disabled={this.disabled}
        allowClear={this.allowClear}
        treeData={this.problemCategories}
        value={this.iValue}
        maxTagCount={this.maxTagCount}
        treeCheckStrictly={this.multiple}
        treeNodeFilterProp="title"
        treeCheckable={this.multiple}
        multiple={this.multiple}
        showSearch={this.showSearch}
        onChange={value => {
          this.iValue = value;
          let ids = this.multiple ? value.map(item => item.value) : value;
          this.$emit('input', ids);
          this.$emit('change', ids);
        }}
        placeholder={this.disabled ? '' : this.placeholder}
        style="width: 100%"
      ></a-tree-select>
    );
  },
};
