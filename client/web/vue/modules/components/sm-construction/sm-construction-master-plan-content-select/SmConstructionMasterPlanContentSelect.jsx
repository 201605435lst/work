
import './style';
import ApiEntity from '../../sm-api/sm-namespace/Entity';
import SelectCommonDiv from '../../sm-construction-base/sm-construction-base-section-select/SelectCommonDiv';
import CommonSelectModal from './CommonSelectModal';
import SmConstructionMasterPlanContentWithGantt
  from '../sm-construction-master-plan-content-with-gantt/SmConstructionMasterPlanContentWithGantt';
import SelectCommonModalDiv from '../../sm-construction-base/sm-construction-base-section-select/SelectCommonModalDiv';
let apiEntity = new ApiEntity();

export default {
  // 总体计划选择框
  name: 'SmConstructionMasterPlanContentSelect',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      visible: false,
      selectedEntities: [], //已选择区段
    };
  },
  computed: {},
  watch: {},
  async created() {
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiEntity = new ApiEntity(this.axios);
    },
    delItem(item) {
      this.selectedEntities = this.selectedEntities.filter(x=>x.id!==item.id);
      this.changed();
    },
    changed() {
      // this.resetGantt();
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
      <div class='sm-construction-master-plan-content-select'>
        <SelectCommonDiv
          tags={this.selectedEntities}
          onClick={()=>this.visible = true}
          onDelClick={this.delItem}
          placeholder='选择施工计划' />
        <CommonSelectModal
          axios={this.axios}
          visible={this.visible}
          value={this.selectedEntities}
          title="施工任务选择"
          multiple={this.multiple}
          onOk={this.changed}
          onChange={visible => (this.visible = visible)}>
          <template slot='modalDiv'>
            <SelectCommonModalDiv tags={this.selectedEntities} onDelClick={this.delItem}/>
          </template>

          <SmConstructionMasterPlanContentWithGantt
            style='margin-top:10px;'
            ref='SmConstructionMasterPlanContentWithGantt'
            axios={this.axios}
            showSelection={true} // 是否显示选择框
            selectedIds={this.selectedEntities.map(x=>x.id)}
            onSelectedChange={tasks=>{
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
