
import './style';
import { requestIsSuccess } from '../../_utils/utils';
import ApiMaterial from '../../sm-api/sm-material/Material';
let apiMaterial = new ApiMaterial();

export default {
  name: 'SmMaterialMaterialSelect',
  props: {
    axios: { type: Function, default: null },
    keyword: {type: Array[String], default: []}, //根据*回显对应材料
    disabled: { type: Boolean, default: false }, //是否禁用
    allowClear: { type: Boolean, default: true }, //是否清除
    placeholder: { type: String, default: '请选择材料' }, //是否显示搜索
  },
  data() {
    return {
      selectItems:[],
      value:undefined,
      iKeyword:[],
    };
  },
  computed: {},
  watch: {
    keyword: {
      handler: function(n, o) {
        this.iKeyword = n;
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
      apiMaterial = new ApiMaterial(this.axios);
    },
    async refresh() {
      let response = await apiMaterial.getListByTypeName(this.iKeyword);
      if (requestIsSuccess(response) && response.data) {
        this.selectItems = response.data.items;
      }
    },
  },
  render() {
    let materialOptions = [];
    this.selectItems.map(item => {
      materialOptions.push(
        <a-select-option key={item.id}>
          {item.name}
        </a-select-option>,
      );
    });
  
    return (
      <div class="sm-material-material-select">
        <a-select
          disabled={this.disabled}
          showSearch={true}
          style="width: 100%"
          allowClear={this.allowClear}
          placeholder={this.placeholder}
          optionFilterProp="children"
          filterOption={(input, option) => {return (
            option.componentOptions.children[0].text.toLowerCase().indexOf(input.toLowerCase()) >= 0
          );}}
          value={this.value}
          onChange={value => this.value = value}
        >
          {materialOptions}
        </a-select>
      </div>
    );
  },
};
    