import { requestIsSuccess } from '../../_utils/utils';
import { treeArrayItemAddProps, treeArrayToFlatArray } from '../../_utils/tree_array_tools';
import ApiQuotaCategory from '../../sm-api/sm-std-basic/QuotaCategory';
import { dropdownStyle } from '../../_utils/config';

let apiQuotaCategory = new ApiQuotaCategory();

export default {
  name: 'SmStdBasicQuotaTreeSelect',
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
    // showSearch: { type: Boolean, default: false }, //是否显示搜索
  },

  data() {
    return {
      quotas: [], // 列表数据源
      quotasFlat: [], //平状数据源
      iValue: null,
      isSearch: false, //树选择框是否处于搜索状态
    };
  },

  computed: {},

  watch: {
    value: {
      handler: async function(val, oldVal) {
        this.iValue = this.value;
        this.initAxios();
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
      apiQuotaCategory = new ApiQuotaCategory(this.axios);
    },
    // 当选择框已经有值的时候，判断需不需要重新加载数据
    isValueLoading() {
      let refresh = false;
      // 当是多选的时候
      if (this.value instanceof Array) {
        if (this.value.length > 0) {
          // 保证数组里面的所有数据已经加载
          if (this.value.some(id => this.quotasFlat.find(x => x.id == id) == null)) {
            refresh = true;
          }
        } else {
          if (this.value.length == 0 && this.quotasFlat.length == 0) {
            refresh = true;
          }
        }
      }
      // 当是单选的时候
      else {
        if (this.value) {
          if (this.quotasFlat.find(x => x.id === this.value) == null) {
            refresh = true;
          }
        } else {
          if (!this.value && this.quotasFlat.length == 0) {
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
        let response = await apiQuotaCategory.getListTree({
          parentId: null,
          quotaIds: !keyWords
            ? this.value instanceof Array
              ? this.value
              : this.value
                ? [this.value]
                : []
            : [],
          isAll: true,
          keyWords: keyWords ? keyWords : '',
        });

        if (requestIsSuccess(response) && response.data.items) {
          let _quotas = treeArrayItemAddProps(response.data.items, 'children', [
            {
              targetProp: 'title',
              handler: item => {
                let result =
                  item.type === 1 ? `${'[' + item.code + ']' + item.name}...` : item.name;
                return result;
              },
            },
            // { sourceProp: 'name', targetProp: 'title' },
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
                return item.type === 1 ? true : false;
              },
            },
          ]);

          this.quotasFlat = treeArrayToFlatArray(_quotas);
          this.quotas = _quotas;
        }
      }
    },

    //异步加载数据
    async onLoadData(treeNode) {
      treeArrayItemAddProps(treeNode.dataRef.children, 'children', [
        {
          targetProp: 'title',
          handler: item => {
            let result = item.type === 1 ? `${'[' + item.code + ']' + item.name}...` : item.name;
            return result;
          },
        },
        { sourceProp: 'id', targetProp: 'value' },
        { sourceProp: 'id', targetProp: 'key' },
        {
          targetProp: 'isLeaf',
          handler: item => {
            return item.children === null ? true : false;
          },
        },
      ]);
    },

    // 判断传过来的id是否在数据中
    processData(array, value) {
      let data = false;
      try {
        array.forEach((item, index, arr) => {
          //当是多选的情况，value是数组，单选是字符串
          if (this.treeCheckable && value.some(values => item.id == values)) {
            data = true;
            throw new Error('error');
          } else {
            if (item.id == value) {
              data = true;
              throw new Error('error');
            }
          }
          if (item.children != null && item.children.length > 0) {
            this.processData(item.children, value);
          }
        });
      } catch (e) {
        if (e.message != 'error') throw e;
      }
      return data;
    },
  },
  render() {
    return (
      <a-tree-select
        dropdownStyle={dropdownStyle}
        disabled={this.disabled}
        allowClear={this.allowClear}
        treeData={this.quotas}
        value={this.iValue}
        //showCheckedStrategy="SHOW_ALL"
        showCheckedStrategy="SHOW_CHILD"
        maxTagCount={this.maxTagCount}
        treeCheckStrictly={this.treeCheckStrictly}
        treeNodeFilterProp="title"
        treeCheckable={this.treeCheckable}
        showSearch={this.showSearch}
        loadData={this.onLoadData}
        onChange={value => {
          this.iValue = value;
          // let ids = this.treeCheckable ? value.map(item => item.value) : value;
          let ids = value;
          this.$emit('input', ids);
          this.$emit('change', ids);
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
