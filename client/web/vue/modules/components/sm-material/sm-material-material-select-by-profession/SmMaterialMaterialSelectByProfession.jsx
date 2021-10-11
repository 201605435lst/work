import { requestIsSuccess } from '../../_utils/utils';
import { treeArrayItemAddProps } from '../../_utils/tree_array_tools';
import ApiMaterial from '../../sm-api/sm-material/Material';
let apiMaterial = new ApiMaterial();

export default {
  name: 'SmMaterialMaterialSelectByProfession',
  props: {
    axios: { type: Function, default: null },
    value: { type: String || Number, default: undefined },
    disabled: { type: Boolean, default: false },
    professionId: { type: String, default: null },
    placeholder: { type: String, default: '请选择' },
  },
  data() {
    return {
      materials: [], // 列表数据源
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
    professionId: {
      handler: function (val, oldVal) {
        if(val != oldVal){
          this.initAxios();
          this.refresh();
        }
      },
      immediate: true,
    },
  },
  async created() {
    this.initAxios();
    //this.refresh();
  },
  methods: {
    initAxios() {
      apiMaterial = new ApiMaterial(this.axios);
    },
    async refresh() {
      let response = await apiMaterial.getByProfessionId(this.professionId);
      if (requestIsSuccess(response)) {
        let _materials = treeArrayItemAddProps(response.data, 'children', [
          { sourceProp: 'name', targetProp: 'title' },
          { sourceProp: 'id', targetProp: 'value' },
          { sourceProp: 'id', targetProp: 'key' },
        ]);
        this.materials = _materials;
        if (this.materials.length === 0) {
          this.iValue = undefined;
          this.$emit('input', this.iValue);
          this.$emit('change', this.iValue);
        }
      }
    },
  },
  render() {
    return (
      <a-select
        disabled={this.disabled}
        allowClear
        options={this.materials}
        placeholder={this.disabled ? '' : this.placeholder}
        style="width: 100%"
        value={this.iValue}
        onChange={value => {
          this.iValue = value;
          this.$emit('input', value);
          this.$emit('change', value);
        }}
      />
    );
  },
};
