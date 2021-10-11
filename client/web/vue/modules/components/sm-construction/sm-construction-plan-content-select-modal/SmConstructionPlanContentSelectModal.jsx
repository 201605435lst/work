
import './style';
import CommonSelectModal from '../sm-construction-master-plan-content-select/CommonSelectModal';
import SelectCommonModalDiv from '../../sm-construction-base/sm-construction-base-section-select/SelectCommonModalDiv';
import SmConstructionPlanWithGantt from '../sm-construction-plan-with-gantt/SmConstructionPlanWithGantt';

export default {
  name: 'SmConstructionPlanContentSelectModal',
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false },
    multiple: { type: Boolean, default: true },
  },
  data() {
    return {
      iVisible: false,
      selectedEntities: [], //已选择区段
    };
  },
  computed: {},
  watch: {
    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
      },
      immediate: true,
    },
    // value: {
    //   handler: async function (value, oldValue) {
    //     this.selectedEntities = value instanceof Array ? value : [value];
    //   },
    //   immediate: true,
    // },
  },
  async created() {
  },
  methods: {
   
    delItem(item) {
      this.selectedEntities = this.selectedEntities.filter(x => x.id !== item.id);
      // this.changed();
    },
    changed() {
      // this.resetGantt();
      if (this.multiple) {
        this.$emit(
          'change',
          this.selectedEntities && this.selectedEntities.length > 0
            ? this.selectedEntities
            : [],
        );
      } else {
        this.$emit(
          'change',
          this.selectedEntities[0] ? this.selectedEntities[0] : null,
        );
      }
    },
  },
  render() {
    return (
      <div class="sm-construction-plan-content-select-modal">
        <CommonSelectModal
          axios={this.axios}
          visible={this.iVisible}
          value={this.selectedEntities}
          title="选择施工计划"
          multiple={this.multiple}
          onOk={this.changed}
          onChange={visible => {
            this.iVisible = visible;
            this.selectedEntities=[];
            this.$emit('close');
          }}>
          <template slot='modalDiv'>
            <SelectCommonModalDiv tags={this.selectedEntities} onDelClick={this.delItem} />
          </template>

          <SmConstructionPlanWithGantt
            style='margin-top:10px;'
            ref='SmConstructionMasterPlanContentWithGantt'
            axios={this.axios}
            showSelection={true} // 是否显示选择框
            selectedIds={this.selectedEntities.map(x => x.id)}
            onSelectedChange={tasks => {
              this.selectedEntities = tasks;
            }}
            isSelectName={true} //是否选择名称
            isApproval={true}
            inModal={true}
          />

        </CommonSelectModal></div>
    );
  },
};
