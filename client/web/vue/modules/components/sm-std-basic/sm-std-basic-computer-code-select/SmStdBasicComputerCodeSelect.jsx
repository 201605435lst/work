import { requestIsSuccess } from '../../_utils/utils';
import { treeArrayItemAddProps } from '../../_utils/tree_array_tools';
import ApiComputerCode from '../../sm-api/sm-std-basic/ComputerCode';
import { dropdownStyle } from '../../_utils/config';
let apiComputerCode = new ApiComputerCode();

export default {
  name: 'SmStdBasicComputerCodeSelect',
  model: {
    prop: 'value',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    value: { type: [Array, String] }, //返回值
    disabled: { type: Boolean, default: false },//是否禁用
    treeCheckable: { type: Boolean, default: false },//设置多选还是单选
    maxTagCount: { type: Number, default: 3 }, //多选状态下最多显示tag数
    placeholder: { type: String, default: '请选择' },
    allowClear: { type: Boolean, default: true }, //是否清除
    showSearch: { type: Boolean, default: false }, //是否显示搜索
    computerType: { type: Number, default: null }, 
  },
  data() {
    return {
      computerCodes: [], // 列表数据源
      iValue:[],
      computerCodeIdList: [], //选中的电算代号
      isSearch: false, //树选择框是否处于搜索状态
    };
  },
  computed: {},
  watch: {
    value: {
      handler: function(val, oldVal) {
        if (val instanceof Array) {
          this.iValue = val;
        }else if(val== null){
          this.iValue =null;
        }
        else
        {
          this.iValue = [val];
        }
        this.initAxios();
      },
      immediate: true,
    },
    computerType: {
      handler: function (val, oldVal) {
       
        this.refresh(null, true);
     
      },
      // immediate: true,
    },

  },
  async created() {
    this.initAxios();
    this.refresh(null, true);
  },
  methods: {
    initAxios() {
      apiComputerCode = new ApiComputerCode(this.axios);
    },
    async refresh(keyWords, isReset) {
      let isValueLoading = isReset ? true : await this.isValueLoading();
      //是否刷新
      if (isValueLoading) {
        let response = await apiComputerCode.getList({
          type:this.computerType?this.computerType:null,
          isAll: true,
          keyWord: keyWords ? keyWords : '',
        });
        if (requestIsSuccess(response) && response.data.items) {
          this.computerCodes =null;
          let _computerCodes = treeArrayItemAddProps(response.data.items, 'children', [
            
            { sourceProp: 'name', targetProp: 'title' },
            { sourceProp: 'id', targetProp: 'value' },
            { sourceProp: 'id', targetProp: 'key' },
          ]);
  
          this.computerCodes = _computerCodes;
        }
      }
    },
  },
  //搜索功能
  async onSearch(value) {
    await this.refresh(value, true);
    this.isSearch = true;
    
  },
  render() {
    return (
      <a-tree-select
        treeCheckable={this.treeCheckable}
        maxTagCount={this.maxTagCount}
        dropdownStyle={dropdownStyle}
        disabled={this.disabled}
        allowClear={this.allowClear}
        value={this.iValue}
        treeData={this.computerCodes}
        placeholder={this.disabled ? '' : this.placeholder}
        showSearch={this.showSearch}
        allowClear={this.allowClear}
        style="width: 100%"
        onChange={value => {
          this.iValue = value;
          this.$emit('input', value);
          this.$emit('change', value);
        }}
        onSearch={value => {
          this.onSearch(value);
        }}
      ></a-tree-select>
    );
  },
};
