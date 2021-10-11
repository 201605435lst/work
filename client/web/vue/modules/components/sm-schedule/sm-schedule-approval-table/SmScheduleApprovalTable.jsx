import './style';
import { requestIsSuccess } from '../../_utils/utils';
import moment from 'moment';
import ApiApproval from '../../sm-api/sm-schedule/Approval';
import ApprovalTableTwo from './component/SmScheduleApprovalTableTwo';
import ApprovalTableThree from './component/SmScheduleApprovalTableThree';
import ApprovalTableFour from './component/SmScheduleApprovalTableFour';
import ApprovalTableFive from './component/SmScheduleApprovalTableFive';
import print from 'print-js';
let apiApproval = new ApiApproval();

export default {
  name: 'SmScheduleApprovalTable',
  props: {
    axios: { type: Function, default: null },
    id: { type: String, default: null },
  },
  data() {
    return {
      approvals: [], //列表数据源
      builders: '', //施工人员
      auxiliaryInformation: [], // 辅助信息
    };
  },
  computed: {},
  watch: {
    id: {
      handler: function(value, oldValue) {
        if (value) {
          this.refresh();
        }
      },
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiApproval = new ApiApproval(this.axios);
    },
    async refresh() {
      this.builders = '';
      this.auxiliaryInformation = [];
      let response = await apiApproval.get(this.id);
      if (requestIsSuccess(response)) {
        this.approvals = response.data;

        this.builders =
          this.approvals.approvalRltMembers.length > 0
            ? this.approvals.approvalRltMembers.map((item, ind) =>
              item.type == 3
                ? this.builders +
                    item.member.name +
                    (this.approvals.approvalRltMembers.length - 1 == ind ? '' : ',')
                : this.builders + '',
            )
            : '暂无';
        this.builders = this.builders.join('');
        this.builders =
          this.builders != ''
            ? this.builders.indexOf(',') != -1 &&
              this.builders.lastIndexOf(',') == this.builders.length - 1
              ? this.builders.substring(0, this.builders.lastIndexOf(','))
              : this.builders
            : '';
        this.auxiliaryInformation.push({
          approvalRltFiles: this.approvals.approvalRltFiles,
          temporaryEquipment: this.approvals.temporaryEquipment,
          safetyCaution: this.approvals.safetyCaution,
          remark: this.approvals.remark,
        });
      }
    },

    //打印
    print() {
      printJS({
        printable: 'sm-schedule-approval-table',
        type: 'html',
        maxWidth: '100%',
        targetStyles: ['*'],
      });
    },
    //关闭单页
    close() {
      this.$emit('cancel');
    },
  },
  render() {
    return (
      <div class="sm-schedule-approval-table">
        <div id="sm-schedule-approval-table">
          <div class="table-header">
            <a-row>
              <a-col span="24">施工申请单</a-col>
            </a-row>
            <a-row>
              <a-col span="18"></a-col>
              <a-col span="6" style="text-align: left;">
                编号：{this.approvals.code}
              </a-col>
            </a-row>
          </div>
          <div class="table-one">
            <a-row class="row-one">
              <a-col span="3" class="col-left">
                施工日期
              </a-col>
              <a-col span="9">
                {moment(this.approvals.time).format('YYYY-MM-DD') != '0001-01-01'
                  ? moment(this.approvals.time).format('YYYY-MM-DD')
                  : '暂无'}
              </a-col>
              <a-col span="3" class="col-center">
                施工单位
              </a-col>
              <a-col span="9">{this.approvals.organization}</a-col>
            </a-row>

            <a-row class="row-one row-two">
              <a-col span="3" class="col-left">
                施工专业
              </a-col>
              <a-col span="9">
                {this.approvals.profession ? this.approvals.profession.name : null}
              </a-col>
              <a-col span="3" class="col-center">
                施工部位
              </a-col>
              <a-col span="9">{this.approvals.location}</a-col>
            </a-row>

            <a-row class="row-one row-two">
              <a-col span="3" class="col-left">
                施工内容
              </a-col>
              <a-col span="21">{this.approvals.name}</a-col>
            </a-row>

            <a-row class="row-one row-two">
              <a-col span="3" class="col-left">
                现场负责人
              </a-col>
              <a-col span="6">
                {this.approvals.approvalRltMembers
                  ? this.approvals.approvalRltMembers.filter(x => x.type === 2)[0].member.name
                  : null}
              </a-col>
              <a-col span="3" class="col-center">
                施工员
              </a-col>
              <a-col span="6">{this.builders}</a-col>
              <a-col span="3" class="col-center">
                劳务人员
              </a-col>
              <a-col span="3">{this.approvals.memberNum + '人'}</a-col>
            </a-row>

            {/* 中部四个 */}
            <ApprovalTableTwo />
            <ApprovalTableThree materials={this.approvals.approvalRltMaterials} />
            <ApprovalTableFour materials={this.approvals.approvalRltMaterials} />
            <ApprovalTableFive subsidiaries={this.auxiliaryInformation}/>
            {/* subsidiaries={{
                approvalRltFiles: this.approvals.approvalRltFiles ? this.approvals.approvalRltFiles : null,
                temporaryEquipment: this.approvals.temporaryEquipment ? this.approvals.temporaryEquipment : null,
                safetyCaution: this.approvals.safetyCaution ? this.approvals.safetyCaution : null,
                remark: this.approvals.remark ? this.approvals.remark : null,
              }} */}

            {/* 尾部 */}
            <a-row class="row-one row-two">
              <a-col span="4" class="col-left">
                申请人
              </a-col>
              <a-col span="5">
                {this.approvals.approvalRltMembers
                  ? this.approvals.approvalRltMembers.filter(x => x.type === 1)[0].member.name
                  : null}
              </a-col>
              <a-col span="3" class="col-center">
                审批人
              </a-col>
              <a-col span="6">暂无</a-col>
              <a-col span="3" class="col-center">
                审批类型
              </a-col>
              <a-col span="3">
                {this.approvals &&
                this.approvals.schedule &&
                this.approvals.schedule.checkFlowId == null
                  ? '人工审核'
                  : '自动审核'}
              </a-col>
            </a-row>
          </div>
        </div>
        <a-row class="row-button">
          <a-col span="24" style="margin-top: 30px;text-align: right;">
            <a-button
              type="danger"
              onClick={() => {
                this.close();
              }}
            >
              关闭
            </a-button>
            <a-button
              style="marginLeft: 15px;"
              onClick={() => {
                this.print();
              }}
            >
              打印
            </a-button>
          </a-col>
        </a-row>
      </div>
    );
  },
};
