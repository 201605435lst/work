
import './style';
import ApiSection from '../../sm-api/sm-construction-base/ApiSection';
import SmConstructionBaseSection from '../sm-construction-base-section/SmConstructionBaseSection';
import SelectCommonModalDiv from './SelectCommonModalDiv';
import SelectCommonDiv from './SelectCommonDiv';
import CommonSelectModal from '../../sm-construction/sm-construction-master-plan-content-select/CommonSelectModal';
let apiSection = new ApiSection();

export default {
  name: 'SmConstructionBaseSectionSelect',
  model: { // 把visible 属性暴露出去,方便表单验证
    prop: 'value',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null }, // axios
    bordered: { type: Boolean, default: true }, // 边框模式
    disabled: { type: Boolean, default: false }, // 编辑模式和查看模式
    value: { type: [Array, String] }, // 已选择的内容 (单选数组,多选string )
    multiple: { type: Boolean, default: true }, //是否多选，默认多选
    placeholder: { type: String, default: '请点击选择施工区段' },
    modalTitle: { type: String, default: '选择框' },
    height: { type: Number, default: 80 }, // 当前选择框的大小
  },
  data() {
    return {
      visible: false,
      selectedEntities: [], //已选择区段
      confirmList: [],//确认提交数据
    };
  },
  computed: {

  },
  watch: {
    value: {
      async handler(nVal, oVal) {
        this.initAxios();
        let res;
        res = nVal instanceof Array ? await apiSection.getByIds(nVal) : await apiSection.getByIds([nVal]);
        this.confirmList = res.data;
      },
      immediate: true,
    },
  },

  async created() {
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiSection = new ApiSection(this.axios);
    },
    delItem(item) {
      this.confirmList = this.confirmList.filter(x => x.id !== item.id);
      this.selectedEntities = this.confirmList;
      this.changed();
    },
    delItem2(item) {
      this.selectedEntities = this.selectedEntities.filter(x => x.id !== item.id);
    },
    changed() {
      this.confirmList = this.selectedEntities;
      if (this.multiple) {
        this.$emit(
          'change',
          this.confirmList && this.confirmList.length > 0
            ? this.confirmList.map(item => item.id)
            : [],
        );
      } else {
        this.$emit(
          'change',
          this.confirmList[0] ? this.confirmList[0].id : null,
        );
      }
    },
  },
  render() {
    return (
      <div class='sm-construction-master-plan-content-select'>
        <SelectCommonDiv
          tags={this.confirmList}
          onClick={() => this.visible = true}
          onDelClick={this.delItem}
          height={this.height}
          placeholder={this.placeholder} />
        <CommonSelectModal
          axios={this.axios}
          visible={this.visible}
          value={this.selectedEntities}
          title={this.modalTitle}
          modalWidth={800}
          multiple={this.multiple}
          onOk={() => this.changed()}
          onChange={visible => { this.selectedEntities = this.confirmList; this.visible = visible; }}>
          <template slot='modalDiv'>
            <SelectCommonModalDiv tags={this.selectedEntities} onDelClick={this.delItem2} />
          </template>

          <SmConstructionBaseSection
            axios={this.axios}
            isSimple={true}
            size='small'
            bordered={false}
            multiple={this.multiple}
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
