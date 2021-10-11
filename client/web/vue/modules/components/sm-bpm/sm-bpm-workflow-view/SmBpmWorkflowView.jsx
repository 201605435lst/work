/**
 * 说明：流程查看组件
 * 作者：easten
 */
import './style';
import { requestIsSuccess } from '../../_utils/utils';
import SmBpmFlowDesign from '../sm-bpm-flow-design';
import { WorkflowStepState, WorkflowState, UserWorkflowGroup } from '../../_utils/enum';
import WorkInfos from '../sm-bpm-workflows/src/WorkInfos';
import ApiWorkflow from '../../sm-api/sm-bpm/Workflow';
let apiWorkflow = new ApiWorkflow();

export default {
  name: 'SmBpmWorkflowView',
  props: {
    axios: { type: Function, default: null },
    width: { type: Number, default: 900 },
    height: { type: Number, default: 700 },
  },
  data() {
    return {
      visible: false,
      flowData: {
        nodes: [],
        edges: [],
      },
      workflowId:null,
    };
  },
  computed: {},
  async created() {
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiWorkflow = new ApiWorkflow(this.axios);
    },
    async refresh() {
      let response = await apiWorkflow.get(this.workflowId);
      if (requestIsSuccess(response)){
        console.log(response);
        // this.workflow = response.data;
        this.flowData={
          nodes:response.data.flowNodes,
          edges:response.data.flowSteps,
        };
      }
    },
    view(workflowId) {
      console.log(workflowId);
      this.workflowId=workflowId;
      this.visible = true;
      this.refresh();
    },
    onOk(){
      this.onClose();
    },
    onClose(){
      this.flowData={
        nodes:[],
        edges:[],
      };
      this.visible=false;
    },
  },
  render() {
    return (<a-modal
      width={this.width}
      title='流程信息'
      okText="确定"
      visible={this.visible}
      onOk={this.onOk}
      onCancel={this.onClose}
    >
      <div class="sm-bpm-workflow-view">
        <div class="flow">
          <SmBpmFlowDesign
            fixPadding={10}
            bordered={false}
            style="width: 100%; min-width: 300px; height: 100%; min-height:200px"
            data={this.flowData}
            axios={this.axios}
            mode="view"
            audit={true}
          />
        </div>
        <div class="info">
          <WorkInfos
            axios={this.axios}
            workflowId={this.workflowId}
            single
          />
        </div>
      </div>
    </a-modal>
    );
  },
};
