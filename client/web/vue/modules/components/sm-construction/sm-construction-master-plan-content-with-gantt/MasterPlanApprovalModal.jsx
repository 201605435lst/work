import './style';
import { requestIsSuccess } from '../../_utils/utils';
import ApiMasterPlanContent from '../../sm-api/sm-construction/ApiMasterPlanContent';
import { ApprovalStatus, ModalStatus } from '../../_utils/enum';
import ApiMasterPlan from '../../sm-api/sm-construction/ApiMasterPlan';
import SmConstructionMasterPlanContentWithGantt from '../../sm-construction/sm-construction-master-plan-content-with-gantt/SmConstructionMasterPlanContentWithGantt';
let apiMasterPlanContent = new ApiMasterPlanContent();
let apiMasterPlan = new ApiMasterPlan();

// 按钮 拒绝 还是 审批 状态
const BtnStatus = {
  None: 0, // 无
  Reject: 1, // 拒绝
  Approve: 2, // 审批
};
export default {
  name: 'MasterPlanApprovalModal', //施工 计划审批 模态框
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      comments:"",// 审批意见
      masterPlanId: undefined, // 施工计划id
      isApproval:false, // 是否审批为审批弹窗
      title: '', // modal  标题
      childVisible: false, // 子modal 显示/隐藏
      BtnStatus : BtnStatus.None, // 当前 是 拒绝 还是审批
    };
  },
  computed: {
    visible() {
      return this.status !== ModalStatus.Hide; // 计算模态框的显示变量
    },
  },
  async created() {
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiMasterPlanContent = new ApiMasterPlanContent(this.axios);
      apiMasterPlan = new ApiMasterPlan(this.axios);
    },
    // 查看
    view(id) {
      this.title = "总体计划查看";
      this.masterPlanId = id;
      this.status = ModalStatus.View;
    },
    // 通过审批
    async approved() {
      if(this.comments===""){
        this.$message.warn("请输入审批意见");
        return;
      }
      let data={
        planId:this.masterPlanId,
        content:this.comments,
        status:ApprovalStatus.Pass,
      };
      let response=await apiMasterPlan.process(data);
      if (requestIsSuccess(response)){
        if(response.data){
          this.$message.success("操作成功");
          this.$emit("success");
          this.close();
        }
      }

    },
    // 驳回审批
    async rejected() {
      let _this = this;
      if(this.comments===""){
        this.$message.warn("请输入审批意见");
        return;
      }
      let data={
        planId:this.masterPlanId,
        content:this.comments,
        status:ApprovalStatus.UnPass,
      };
      this.$confirm({
        title: "温馨提示",
        content: h => <div style="color:red;">确定要驳回此审批吗？</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response=await apiMasterPlan.process(data);
            if (requestIsSuccess(response)) {
              _this.$message.success("操作成功");
              _this.$emit("success");
              _this.close();
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() {},
      });
    },
    // 审批
    approval(id){
      this.view(id);
      this.title = "总体计划审批";
      this.isApproval=true;
    },
    // 关闭模态框
    close() {
      this.status = ModalStatus.Hide;
      this.masterPlanId = undefined;
      this.comments = '';
      this.isApproval=false;
    },
    // 数据提交
    ok() {
      this.$message.success('操作成功');
      this.close();
    },
    // 打开 审批意见 模态框
    openCommentModal(btnStatus) {
      this.BtnStatus = btnStatus;
      this.childVisible = true;
    },
  },
  render() {
    return (
      <a-modal
        title={this.title}
        width='80%'
        visible={this.visible}
        bodyStyle={{height: '500px', overflowY: 'auto'}}
        onCancel={this.close}
        destroyOnClose={true}
        onOk={this.ok}
      >
        <SmConstructionMasterPlanContentWithGantt
          axios={this.axios}
          masterPlanId={this.masterPlanId}
          isApproval={true}
          inModal={true}
        />

        <template slot="footer">
          {this.isApproval ? <div>
            <a-button size="small" key="back" onClick={this.close}>取消</a-button>
            <a-button size="small" key="rejected" type="danger"  onClick={()=>this.openCommentModal(BtnStatus.Reject)}>驳回</a-button>
            <a-button size="small" key="approved" type="primary"  onClick={()=>this.openCommentModal(BtnStatus.Approve)}>通过</a-button>
          </div> :
            <div>
              <a-button size="small" key="back" onClick={this.close}>取消</a-button>
              <a-button size="small" key="submit" type="primary" onClick={this.ok}>确定</a-button>
            </div>
          }
        </template>

        <a-modal
          title="审批意见"
          visible={this.childVisible}
          placeholder='请输入审批意见'
          onCancel={()=>{
            this.comments = '';
            this.childVisible = false;
          }}
          destroyOnClose={true}
          onOk={async () => {
            if (this.comments === '') {
              this.$message.warn("请输入审批意见");
              return;
            }
            switch (this.BtnStatus) {
            case BtnStatus.Approve:
              await this.approved();
              break;
            case BtnStatus.Reject:
              await this.rejected();
              break;
            }
            this.childVisible = false;
            this.comments = '';
          }}
        >
          {/* 审批意见 */}
          <a-textarea
            placeholder='请输入审批意见'
            auto-size={{ minRows: 2, maxRows: 6 }}
            value={this.comments}
            onChange={(e) => {
              this.comments = e.target.value;
            }}
          />
        </a-modal>
      </a-modal>
    );
  },
};
