
import './style';
import SelectCommonDiv from '../../sm-construction-base/sm-construction-base-section-select/SelectCommonDiv';
import CommonSelectModal from '../sm-construction-master-plan-content-select/CommonSelectModal';
import SelectCommonModalDiv from '../../sm-construction-base/sm-construction-base-section-select/SelectCommonModalDiv';
import SmConstructionMasterPlanContentWithGantt
  from '../sm-construction-master-plan-content-with-gantt/SmConstructionMasterPlanContentWithGantt';
import SmConstructionPlanWithGantt from '../sm-construction-plan-with-gantt/SmConstructionPlanWithGantt';

export default {
  name: 'SmConstructionPlanContentSelect',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      visible: false,
      selectedEntities: [], //已选择区段
      confirmSelected: [], //确认提交数据
    };
  },
  computed: {},
  async created() { },
  methods: {
    delItem(item) {
      this.confirmSelected = this.confirmSelected.filter(x => x.id !== item.id);
      this.selectedEntities = this.confirmSelected;
      this.changed();
    },
    delItem2(item) {
      this.selectedEntities = this.selectedEntities.filter(x => x.id !== item.id);
      this.changed();
    },
    changed() {
      // this.resetGantt();
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
    //重置gantt 子 组件的数据
    resetGantt() {
      this.$refs.SmConstructionMasterPlanContentWithGantt.resetByParent();
    },
  },
  render() {
    return (
      <div class="sm-construction-plan-content-select">

        <SelectCommonDiv
          tags={this.confirmSelected}
          onClick={() => this.visible = true}
          onDelClick={this.delItem}
          placeholder='请选择总体计划' />
        <CommonSelectModal
          axios={this.axios}
          visible={this.visible}
          value={this.selectedEntities}
          title="选择总体计划"
          multiple={this.multiple}
          onOk={this.changed}
          modalWidth={1200}
          onChange={visible => (this.visible = visible)}>
          <template slot='modalDiv'>
            <SelectCommonModalDiv tags={this.selectedEntities} onDelClick={this.delItem2} />
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

        </CommonSelectModal>

      </div>
    );
  },
};
