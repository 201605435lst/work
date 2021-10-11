
import './style';
import ApiStandard from '../../sm-api/sm-construction-base/ApiStandard';
import SmConstructionBaseStandard from '../sm-construction-base-standard/SmConstructionBaseStandard';
import SelectCommonDiv from '../sm-construction-base-section-select/SelectCommonDiv';
import SelectCommonModalDiv from '../sm-construction-base-section-select/SelectCommonModalDiv';
import CommonSelectModal from '../../sm-construction/sm-construction-master-plan-content-select/CommonSelectModal';
let apiStandard = new ApiStandard();

export default {
  name: 'SmConstructionBaseStandardSelect', // 工序规范选择框
  model: { // 把value 属性暴露出去,方便表单验证
    prop: 'value',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    height: { type: Number, default: 80 }, // 当前选择框的大小
    disabled: { type: Boolean, default: false }, // 编辑模式和查看模式
    value: { type: [Array, String] }, // 已选择的内容 (单选数组,多选string )
    multiple: { type: Boolean, default: false }, //是否多选，默认多选
    placeholder: { type: String, default: '请选择' },
    modalTitle: { type: String, default: '选择框' },
    bordered: { type: Boolean, default: true }, // 边框模式
  },
  data() {
    return {
      visible: false,
      selectedEntities: [], //已选择区段
      confirmSelected: [],//确认提交数据
    };

  },
  computed: {

  },
  watch: {
    value: {
      async handler(nVal, oVal) {
        this.initAxios();
        let res;
        res = nVal instanceof Array ? await apiStandard.getByIds(nVal) : await apiStandard.getByIds([nVal]);
        this.confirmSelected = res.data;
      },
      immediate: true,
    },
  },

  async created() {
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiStandard = new ApiStandard(this.axios);
    },
    delItem(item) {
      this.confirmSelected = this.confirmSelected.filter(x => x.id !== item.id);
      this.selectedEntities = this.confirmSelected;
      this.changed();
    },
    delItem2(item) {
      this.selectedEntities = this.selectedEntities.filter(x => x.id !== item.id);
    },
    changed() {
      this.confirmSelected = this.selectedEntities;
      if (this.multiple) {
        this.$emit(
          'change',
          this.selectedEntities && this.selectedEntities.length > 0
            ? this.selectedEntities.map(item => item.id)
            : [],
        );
      } else {
        this.$emit(
          'change',
          this.selectedEntities[0] ? this.selectedEntities[0].id : null,
        );
      }
    },
  },
  render() {
    return (
      <div class='sm-construction-master-plan-content-select'>
        <SelectCommonDiv
          tags={this.confirmSelected}
          onClick={() => this.visible = true}
          onDelClick={this.delItem}
          height={this.height}
          placeholder='请选择工序指引' />
        <CommonSelectModal
          axios={this.axios}
          visible={this.visible}
          value={this.selectedEntities}
          title="请选择工序指引"
          multiple={this.multiple}
          modalWidth={800}
          onOk={this.changed}
          onChange={visible => { this.selectedEntities = this.confirmSelected; this.visible = visible; }}>
          <template slot='modalDiv'>
            <SelectCommonModalDiv tags={this.selectedEntities} onDelClick={this.delItem2} />
          </template>

          <SmConstructionBaseStandard
            axios={this.axios}
            isSimple={true}
            multiple={this.multiple}
            bordered={false}
            size="small"
            showOperator={false}
            showSelectRow={true}
            selected={this.selectedEntities}
            onChange={selected => {
              this.selectedEntities = selected;
            }}
          />

        </CommonSelectModal>
      </div>
    );
  },
};
