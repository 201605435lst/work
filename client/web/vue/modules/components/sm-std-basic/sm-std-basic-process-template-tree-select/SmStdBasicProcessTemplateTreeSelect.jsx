import { requestIsSuccess } from '../../_utils/utils';
import { treeArrayItemAddProps, treeArrayToFlatArray } from '../../_utils/tree_array_tools';
import ApiProcessTemplate from '../../sm-api/sm-std-basic/ProcessTemplate';
import { dropdownStyle } from '../../_utils/config';

let apiProcessTemplate = new ApiProcessTemplate();

export default {
  name: 'SmStdBasicProcessTemplateTreeSelect',
  model: {
    prop: 'value',
    event: 'change',
  },

  props: {
    axios: { type: Function, default: null },
    value: { type: [Array, String] }, // 已选择的内容 (单选数组,多选string )
    disabled: { type: Boolean, default: false }, //是否禁用
    parentDisabled: { type: Boolean, default: false }, //父级是否禁用
    placeholder: { type: String, default: '请选择' },
    treeCheckable: { type: Boolean, default: false }, //是否多选
    treeCheckStrictly: { type: Boolean, default: false }, //父子级是否严格
    maxTagCount: { type: Number, default: 2 }, //多选状态下最多显示tag数
    allowClear: { type: Boolean, default: true }, //是否清除
    showSearch: { type: Boolean, default: false }, //是否显示搜索
  },

  data() {
    return {
      processTemplates: [], // 列表数据源
      processTemplatesFlat: [], //平状数据源
      iValue: null,
      isSearch: false, //树选择框是否处于搜索状态
    };
  },

  computed: {},

  watch: {
    value: {
      handler: async function(val, oldVal) {
        this.initAxios();
        if (this.isSearch) {
          await this.refresh(null, true);
          this.isSearch = false;
        } else {
          await this.refresh();
        }
        this.setValue();
      },
      immediate: true,
    },
  },

  async created() {
    this.initAxios();
  },

  methods: {
    initAxios() {
      apiProcessTemplate = new ApiProcessTemplate(this.axios);
    },
    // 当选择框已经有值的时候，判断需不需要重新加载数据
    isValueLoading() {
      let refresh = false;
      // 当是多选的时候
      if (this.value instanceof Array) {
        if (this.value.length > 0) {
          // 保证数组里面的所有数据已经加载
          if (this.value.some(id => this.processTemplatesFlat.find(x => x.id == id) == null)) {
            refresh = true;
          }
        } else {
          if (this.value.length == 0 && this.processTemplatesFlat.length == 0) {
            refresh = true;
          }
        }
      }
      // 当是单选的时候
      else {
        if (this.value) {
          if (this.processTemplatesFlat.find(x => x.id === this.value) == null) {
            refresh = true;
          }
        } else {
          if (!this.value && this.processTemplatesFlat.length == 0) {
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
        let response = await apiProcessTemplate.getList({
          parentId: null,
          ids: !keyWords
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
          this.processTemplates = [];
          this.processTemplatesFlat = [];
          let _processTemplates = treeArrayItemAddProps(response.data.items, 'children', [
            {
              targetProp: 'title',
              handler: item => {
                let result = item.name.length > 10 ? `${item.name.substring(0, 10)}...` : item.name;
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
          ]);
          this.processTemplatesFlat = treeArrayToFlatArray(_processTemplates);

          this.processTemplates = _processTemplates;
        }
      }
    },


    //异步加载数据
    async onLoadData(treeNode) {
      if (treeNode.dataRef.children && treeNode.dataRef.children.length == 0) {
        let response = await apiProcessTemplate.getList({
          parentId: treeNode.dataRef.value,
          ids: [],
          isAll: true,
        });
        if (requestIsSuccess(response) && response.data.items) {
          this.processTemplatesFlat = this.processTemplatesFlat.concat(response.data.items);

          treeNode.dataRef.children = treeArrayItemAddProps(response.data.items, 'children', [
            {
              targetProp: 'title',
              handler: item => {
                let result = item.name.length > 10 ? `${item.name.substring(0, 10)}...` : item.name;
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
    // 多选模式下，value 值格式为：{value,label}格式
    async setValue() {
      let result = await this.processData(this.processTemplatesFlat, this.value);
      if (result) {
        if (this.treeCheckable) {
          this.iValue = this.value
            ? this.processTemplatesFlat
              .filter(item => {
                if (this.value.indexOf(item.id) > -1) {
                  return true;
                }
              })
              .map(item => {
                return {
                  value: item.id,
                  label: item.name.length > 10 ? `${item.name.substring(0, 10)}...` : item.name,
                  //name: item.name,
                  // parentId: item.parentId,
                  // duration: item.duration,
                  // durationUnit: item.durationUnit,
                  // prepositionId: item.prepositionId,   //前置任务，标准库只选一个前置任务
                };
              })
            : [];
        } else {
          this.iValue = this.value;
        }
      } else {
        this.iValue = null;
      }
    },
    // 判断传过来的id是否在数据中
    processData(array, value) {
      let data = false;
      try {
        array.forEach((item, index, arr) => {
          //当是多选的情况，value是数组，单选是字符串
          if (this.treeCheckable && value && value.some(values => item.id == values)) {
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
        treeData={this.processTemplates}
        value={this.iValue}
        showCheckedStrategy="SHOW_ALL"
        maxTagCount={this.maxTagCount}
        treeCheckStrictly={this.treeCheckStrictly}
        treeNodeFilterProp="title"
        treeCheckable={this.treeCheckable}
        showSearch={this.showSearch}
        loadData={this.onLoadData}
        onChange={value => {
          this.iValue = value;
          let ids = this.treeCheckable ? value.map(item => item.value) : value;
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
