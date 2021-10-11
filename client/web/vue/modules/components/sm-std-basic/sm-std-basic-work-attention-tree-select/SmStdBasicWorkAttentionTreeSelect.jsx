import { requestIsSuccess } from '../../_utils/utils';
import { treeArrayItemAddProps, treeArrayToFlatArray } from '../../_utils/tree_array_tools';
import ApiWorkAttention from '../../sm-api/sm-std-basic/WorkAttention';
import { dropdownStyle } from '../../_utils/config';

let apiWorkAttention = new ApiWorkAttention();

export default {
  name: 'SmStdBasicWorkAttentionTreeSelect',
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
    isRefresh: { type: String, default: null }, //根据时间戳决定需不需要刷新
    disabledIds: { type: Array, default: () => [] }, //禁用层级id
    childrenIsDisabled: { type: Boolean, default: false }, //设置子元素禁用状态
    repairTagKey: { type: String, default: null }, //维修项标签
  },
  data() {
    return {
      workAttentions: [], // 列表数据源
      workAttentionsFlat: [], //平状数据源
      iValue: null,
      iDisabledIds: [],
    };
  },
  computed: {},
  watch: {
    value: {
      handler: function(val, oldVal) {
        this.setValue();
      },
      immediate: true,
    },
    isRefresh: {
      handler: function(val, oldVal) {
        this.initAxios();
        this.refresh();
      },
      immediate: true,
    },
    disabledIds: {
      handler: function(val, oldVal) {
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
      apiWorkAttention = new ApiWorkAttention(this.axios);
    },
    async refresh() {
      let response = await apiWorkAttention.getList({
        isAll: true,
        isType: true,
        repairTagKey: this.repairTagKey,
      });
      if (requestIsSuccess(response)) {
        let _workAttentions = treeArrayItemAddProps(response.data.items, 'children', [
          {
            targetProp: 'title',
            handler: item => {
              let result = item.content;
              return <div title={result}>{result}</div>;
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
        this.workAttentionsFlat = treeArrayToFlatArray(_workAttentions);
        if (this.childrenIsDisabled) {
          this.setChildrenDisabled(_workAttentions, false);
        }
        this.workAttentions = _workAttentions;

        if (this.value) {
          this.setValue();
        }
      }
    },

    // 多选模式下，value 值格式为：{value,label}格式
    async setValue() {
      let result = await this.processData(this.workAttentionsFlat, this.value);
      if (result) {
        if (this.multiple) {
          this.iValue =
            this.value && this.value.length > 0
              ? this.workAttentionsFlat
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
    //设置子元素禁用状态
    setChildrenDisabled(workAttentions, disabled) {
      workAttentions.map(item => {
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
        treeData={this.workAttentions}
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
