import './style';
import { requestIsSuccess } from '../../_utils/utils';
// import ApiMaterial from '../../sm-api/sm-material/Material';
// let apiMaterial = new ApiMaterial();

export default {
  name: 'SmCommonSelect',
  props: {
    axios: { type: Function, default: null },
    allowClear: { type: Boolean, default: true }, //支持清除
    autoClearSearchValue: { type: Boolean, default: true }, //是否在选中项后清空搜索框，只在 mode 为 multiple 或 tags 时有效
    autoFocus: { type: Boolean, default: false }, //默认获取焦点
    disabled: { type: Boolean, default: false }, //是否禁用
    dropdownClassName: { type: String, default: 'common-select' }, //下拉菜单的 className 属性
    dropdownStyle: { type: Object, default: null }, //下拉菜单的 style 属性
    filterOption: { type: Function, default: () => { } }, //是否根据输入项进行筛选。当其为一个函数时，会接收 inputValue option 两个参数，当 option 符合筛选条件时，应返回 true，反之则返回 false
    firstActiveValue: { type: String, default: null }, //默认高亮的选项
    maxTagCount: { type: Number, default: 5 }, //最多显示多少个 tag
    mode: { type: String, default: 'default' }, //'default' | 'multiple' | 'tags' | 'combobox'
    placeholder: { type: String, default: '请选择' }, //选择框默认文字
    showSearch: { type: Boolean, default: false }, //使单选模式可搜索
    API: { type: Object, default: null }, //接口
    value: { type: String, default: undefined }, //值
  },
  data() {
    return {
      datasource: [],
      keyWords: null,
      iApi: null,
      iValue: undefined,
    };
  },
  computed: {},
  watch: {
    API: {
      handler: function (val, oldVal) {
        if (val) {
          this.iApi = val;
        }
      },
      immediate: true,
      deep: true,
    },
    value: {
      handler: function (n, o) {
        this.iValue = n;
        this.refresh();
      },
      immediate: true,
    },
  },
  async created() {
    this.initAxios();
  },
  methods: {
    initAxios() {
      // apiMaterial = new ApiMaterial(this.axios);
    },
    async refresh() {
      let data = {
        isAll: true,
        keyWords: null,
      };
      let response = await this.iApi.getList(data);
      if (requestIsSuccess(response)) {
        this.datasource =
          response.data && response.data.items ? response.data.items : response.data;
      }
    },
    getRecord(id) {
      return this.datasource.find(item => item.id == id);
    },
  },
  render() {
    let Options = [];
    this.datasource.map(item => {
      Options.push(<a-select-option key={item.id}>{item.name || item.title || item.id}</a-select-option>);
    });
    return (
      <div class="sm-common-select">
        <a-select
          allowClear={this.allowClear}
          autoClearSearchValue={this.autoClearSearchValue}
          autoFocus={this.autoFocus}
          disabled={this.disabled}
          dropdownClassName={this.dropdownClassName}
          dropdownStyle={this.dropdownStyle}
          filterOption={this.filterOption}
          firstActiveValue={this.firstActiveValue}
          maxTagCount={this.maxTagCount}
          mode={this.mode}
          placeholder={this.placeholder}
          showSearch={this.showSearch}
          value={this.iValue}
          onChange={(value, item) => {
            let _record = this.getRecord(value);
            this.$emit('change', value, _record);
            this.$emit('select', value, _record);
            this.iValue = value;
          }}
        >
          {Options}
        </a-select>
      </div>
    );
  },
};
