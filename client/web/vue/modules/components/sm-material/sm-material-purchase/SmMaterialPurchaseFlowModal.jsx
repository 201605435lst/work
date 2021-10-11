import { requestIsSuccess, objFilterProps, CreateGuid, vP, vIf } from '../../_utils/utils';
import ApiPurchaseFlowTemplate from '../../sm-api/sm-material/PurchaseFlowTemplate';
import ApiPurchase from '../../sm-api/sm-material/Purchase';
import SmBpmWorkflowModal from '../../sm-bpm/sm-bpm-workflow-templates/SmBpmWorkflowSelectModal';
import SmBmpFlowBase from '../../sm-common/sm-flow-base';
import { ModalStatus } from '../../_utils/enum';
import moment from 'moment';

let apiPurchaseFlowTemplate = new ApiPurchaseFlowTemplate();
let apiPurchase = new ApiPurchase();

export default {
  name: 'SmMaterialPurchaseModal',
  props: {
    axios: { type: Function, default: null },
    width: { type: Number, default: 1000 },
    canSelect: {type: Boolean, default: true}, //是否展示选择流程按钮。默认展示
  },
  data() {
    return {
      form: {}, // 表单
      record: {}, // 表单绑定的对象,
      status: ModalStatus.Hide, // 模态框状态
      nodes: [], //审批流程详情数据
      isView: null,// 是否为查看模式，只显示节点名称
    };
  },
  computed: {
    isShow() {
      return this.status == ModalStatus.View;
    },
    visible() {
      // 计算模态框的显示变量k
      return this.status !== ModalStatus.Hide;
    },
  },
  async created() {
    this.form = this.$form.createForm(this, {});
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiPurchaseFlowTemplate = new ApiPurchaseFlowTemplate(this.axios);
      apiPurchase = new ApiPurchase(this.axios);
    },

    save(){
      let _this = this;
      if (_this.record) {
        this.$confirm({
          title: '配置流程',
          content: h => <div style="color:red;">{'确认配置该流程？'}</div>,
          okType: 'warning',
          onOk() {
            return new Promise(async (resolve, reject) => {
              let response = await apiPurchaseFlowTemplate.create({ workflowTemplateId: _this.record.id });
              if (requestIsSuccess(response)) {
                _this.$message.success('操作成功');
                _this.close();
              }
              setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
            });
          },
        });
      }
    },

    async add(){
      this.isView = true;
      this.status = ModalStatus.Add;
      let response = await apiPurchaseFlowTemplate.get();
      if (requestIsSuccess(response) && response.data) {
        let record = {
          id:response.data.workflowTemplateId,
        };
        this.setFlow(record);
      }
    },

    view(nodes){
      this.status = ModalStatus.View;
      this.nodes = nodes;
      this.isView = false;
    },

    async setFlow(record){
      this.nodes = [];
      let response = await apiPurchase.getFlowInfo(record.id);
      if (requestIsSuccess(response)) {
        this.getNodes(response.data[0]);
        this.nodes = response.data;
      }
    },
    //递归拼装nodes
    getNodes(data){
      let name = '';
      data.approvers.length > 0 ? data.approvers.map((item,ind) => name += item.name + (data.approvers.length - 1 == ind ? "" : ",") + '') : undefined;
      //data.comments && data.comments.length > 0 ? data.comments.map(item => item.content = item.content.toString()) : undefined;
      data.comments && data.comments.length > 0 ? data.comments.map(item => item.approveTime = moment(item.approveTime).format('YYYY-MM-DD')) : undefined;

      data.date = data.comments && data.comments.length > 0 ? moment(data.comments[0].approveTime).format('YYYY-MM-DD') : '0001-01-01';
      data.creator = name;


      if(data.children){
        data.children = data.children.filter(x => x.type != "bpmCc");
        data.children.map(item => {
          this.getNodes(item);
        });
      };
    },

    // 关闭模态框
    close() {
      this.form.resetFields();
      this.nodes = [];
      this.status = ModalStatus.Hide;
    },
  },
  render() {
    let flowMessage = (
      <div>
        <SmBmpFlowBase
          nodes={this.nodes}
          bpmNode={true}
          showDetail={false}
          isView={this.isView}
          //scroller={false}
        />
      </div>
    );
    return (
      <div>
        <a-modal
          width={this.width}
          title={this.isShow ? "流程详细" : "流程配置"}
          okText="保存"
          visible={this.visible}
          onOk={this.isShow ? this.close : this.save}
          onCancel={this.close}
        >
          {this.canSelect ? 
            <div class="flowTemplate-modal">
              <a-button type="primary" disabled={this.isShow} onClick={() => this.$refs.flowSelect.show()}><a-icon type="plus" /> 选择流程</a-button>,
            </div>
            : undefined}
          
          {this.nodes.length > 0 ? flowMessage : <a-divider>当前未配置审批流程</a-divider>}
          
        </a-modal>
        {/* 流程选择框 */}
        <SmBpmWorkflowModal
          ref="flowSelect"
          axios={this.axios}
          onSelected={value => {
            this.record = value;
            this.setFlow(value);
          }}
        ></SmBpmWorkflowModal>
      </div>
    );
  },
};