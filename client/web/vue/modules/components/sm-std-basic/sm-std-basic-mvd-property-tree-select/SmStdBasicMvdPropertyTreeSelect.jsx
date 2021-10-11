import { requestIsSuccess } from '../../_utils/utils';
import { treeArrayItemAddProps, treeArrayToFlatArray } from '../../_utils/tree_array_tools';
import ApiMVDCategory from '../../sm-api/sm-std-basic/MVDCategory';
import { dropdownStyle } from '../../_utils/config';

let apiMVDCategory = new ApiMVDCategory();

export default {
  name: 'SmStdBasicMvdPropertyTreeSelect',
  model: {
    prop: 'value',
    event: 'change',
  },

  props: {
    axios: { type: Function, default: null },
    value: { type: [Array, String] }, //返回值
    disabled: { type: Boolean, default: false }, //是否禁用
    placeholder: { type: String, default: '请选择' },
    treeCheckable: { type: Boolean, default: false }, //是否多选
    treeCheckStrictly: { type: Boolean, default: false }, //父子级是否严格
    maxTagCount: { type: Number, default: 2 }, //多选状态下最多显示tag数
    allowClear: { type: Boolean, default: true }, //是否清除
    showSearch: { type: Boolean, default: false }, //是否显示搜索
  },

  data() {
    return {
      mvdProperties: [], // 列表数据源
      mvdPropertiesFlat: [], //平状数据源
      iValue: null,
      isSearch: false, //树选择框是否处于搜索状态
    };
  },

  computed: {},

  watch: {
    value: {
      handler: async function (val, oldVal) {
        this.initAxios();
        this.iValue = this.value;
        if (this.isSearch) {
          await this.refresh(null, true);
          this.isSearch = false;
        } else {
          await this.refresh();
        }
      },
      immediate: true,
    },
  },

  async created() {
    this.initAxios();
  },

  methods: {
    initAxios() {
      apiMVDCategory = new ApiMVDCategory(this.axios);
    },
    // 当选择框已经有值的时候，判断需不需要重新加载数据
    isValueLoading() {
      let refresh = false;
      // 当是多选的时候
      if (this.value instanceof Array) {
        if (this.value.length > 0) {
          // 保证数组里面的所有数据已经加载
          if (this.value.some(id => this.mvdPropertiesFlat.find(x => x.id == id) == null)) {
            refresh = true;
          }
        } else {
          if (this.value.length == 0 && this.mvdPropertiesFlat.length == 0) {
            refresh = true;
          }
        }
      }
      // 当是单选的时候
      else {
        if (this.value) {
          if (this.mvdPropertiesFlat.find(x => x.id === this.value) == null) {
            refresh = true;
          }
        } else {
          if (!this.value && this.mvdPropertiesFlat.length == 0) {
            refresh = true;
          }
        }
      }
      return refresh;
    },

    //初始化页面加载数据
    async refresh(keyWords, isReset) {
      let isValueLoading = isReset ? true : await this.isValueLoading();
      //是否刷新
      if (isValueLoading) {
        console.log(this.value)
        let response = await apiMVDCategory.getListTree({
          parentId: null,
          ids: this.value instanceof Array
            ? this.value.map(item => item instanceof Object ? item.value : item)
            : [],
          isAll: true,
          keyWords: keyWords ? keyWords : '',
        });
        if (requestIsSuccess(response) && response.data.items) {
          let _componentCategories = treeArrayItemAddProps(response.data.items, 'children', [
            { sourceProp: 'name', targetProp: 'title' },
            { sourceProp: 'id', targetProp: 'value' },
            { sourceProp: 'id', targetProp: 'key' },
            {
              targetProp: 'isLeaf',
              handler: item => {
                return item.children === null ? true : false;
              },
            },
            {
              targetProp: 'disabled',
              handler: item => {
                return item.isDisabled;
              },
            },
          ]);
          this.mvdPropertiesFlat = treeArrayToFlatArray(_componentCategories);
          this.mvdProperties = _componentCategories;
        }
      }
    },

    //搜索功能
    async onSearch(value) {
      await this.refresh(value, true);
      this.isSearch = true;
      if (!this.treeCheckable) {
        this.setValue();
      }
    },
  },
  render() {
    return (
      <a-tree-select
        dropdownStyle={dropdownStyle}
        disabled={this.disabled}
        allowClear={this.allowClear}
        treeData={this.mvdProperties}
        value={this.iValue}
        showCheckedStrategy="SHOW_CHILD"
        maxTagCount={this.maxTagCount}
        treeCheckStrictly={this.treeCheckStrictly}
        treeNodeFilterProp="title"
        treeCheckable={this.treeCheckable}
        showSearch={this.showSearch}
        // loadData={this.onLoadData}
        onChange={value => {
          console.log('---------');
          this.iValue = value;
          this.$emit('input', value);
          this.$emit('change', value);
        }}
        placeholder={this.disabled ? '' : this.placeholder}
        style="width: 100%"
        onSearch={value => {
          this.onSearch(value);
        }}
      ></a-tree-select>
    );
  },
};
