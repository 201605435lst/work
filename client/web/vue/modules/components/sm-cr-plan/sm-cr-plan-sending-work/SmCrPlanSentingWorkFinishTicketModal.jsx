import ApiSkyLightPlan from '../../sm-api/sm-cr-plan/SkyLightPlan';
import * as utils from '../../_utils/utils';
import { requestIsSuccess, getRepairLevelTitle } from '../../_utils/utils';
import moment from 'moment';
import { ModalStatus, SelectablePlanType, RepairLevel } from '../../_utils/enum';
import FileSaver from 'file-saver';
import SmSystemUserSelect from '../../sm-system/sm-system-member-select';

let apiSkyLightPlan = new ApiSkyLightPlan();
const formFields = [
  'orderNumber',
  'workTitle',
  'repairLevel',
  'workPlace',
  'workContent',
  'influenceRange',
  'planStartTime',
  'planFinishTime',
  'realStartTime',
  'realFinsihTime',
  'securityMeasuresAndAttentions',
  'finishContent',
];
export default {
  name: 'SmCrPlanSentingWorkFinishTicketModal',
  props: {
    value: { type: Boolean, default: null },
    axios: { type: Function, default: null },
    repairTagKey: { type: String, default: null }, //维修项标签
  },
  data() {
    return {
      form: this.$form.createForm(this, {}),
      sentedWork: null,
      status: ModalStatus.Hide,
      sendWorkRecord: null,
      tickets: [],
      ticketIndex: 0,
      activeTicket: null,
      workTicketRltCooperationUnits: [],
      cooperationUnitCompletion: null,
      isFine:true,
    };
  },
  computed: {
    visible() {
      return this.status !== ModalStatus.Hide;
    },
    title() {
      return utils.getModalTitle(this.status);
    },
  },

  watch: {},

  created() {
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiSkyLightPlan = new ApiSkyLightPlan(this.axios);
    },

    view(record) {
      this.sendWorkRecord = record;
      this.status = ModalStatus.View;
      this.refresh();
    },

    finish(record) {
      this.sendWorkRecord = record;
      this.status = ModalStatus.Edit;
      this.refresh();
    },

    getActiveState() {
      if (this.activeTicket == null) return '';
      return this.activeTicket.isFinish ? '完成' : '未完成';
    },

    //初始化派工单
    async refresh() {
      let response = await apiSkyLightPlan.getWorkTickets(this.sendWorkRecord.skylightPlanId);
      if (requestIsSuccess(response)) {
        this.tickets = response.data;

        //设置默认数据
        for (let i = 0; i < this.tickets.length; i++) {
          let ele = this.tickets[i];
          if (ele.isFinish) continue;
          ele.orderNumber = this.sendWorkRecord.orderNo;
          ele.finishContent = this.sendWorkRecord.feedback;
          // ele.influenceRange = this.sendWorkRecord.influenceScope;
          ele.realStartTime = this.sendWorkRecord.startRealityTime;
          ele.realFinsihTime = this.sendWorkRecord.endRealityTime;
        }

        this.setPageTicket(0);
      }
    },

    //设置页面中显示的ticket
    setPageTicket(index) {
      if (this.tickets.length > 0) {
        this.activeTicket = this.tickets[index];
        this.activeTicket.repairLevel =
          typeof this.activeTicket.repairLevel == 'string'
            ? this.activeTicket &&
              this.activeTicket.repairLevel &&
              this.activeTicket.repairLevel.split(',').length > 0
              ? this.activeTicket.repairLevel.split(',').map(Number)
              : null
            : this.activeTicket.repairLevel; 
        this.$nextTick(() => {
          let values = utils.objFilterProps(this.activeTicket, formFields);

          values.planStartTime = moment(this.activeTicket.startPlanTime).format(
            'YYYY-MM-DD HH:mm:ss',
          );
          values.planFinishTime = moment(this.activeTicket.endPlanTime).format(
            'YYYY-MM-DD HH:mm:ss',
          );

          values.realStartTime = moment(this.activeTicket.realStartTime);
          values.realFinsihTime = moment(this.activeTicket.realFinsihTime);

          this.workTicketRltCooperationUnits = this.activeTicket.workTicketRltCooperationUnits = null
            ? []
            : this.activeTicket.workTicketRltCooperationUnits;
          this.cooperationUnitCompletion = this.activeTicket.completion;
          this.isFine = this.activeTicket.isFine;
          this.form.setFieldsValue(values);
        });
      }
    },

    previousTicket() {
      if (this.ticketIndex > 0) this.ticketIndex--;
      this.setPageTicket(this.ticketIndex);
      this.activeTicket = this.tickets[this.ticketIndex];
    },

    nextTicket() {
      if (this.ticketIndex < this.tickets.length - 1) this.ticketIndex++;
      this.setPageTicket(this.ticketIndex);
      this.activeTicket = this.tickets[this.ticketIndex];
    },

    save() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let data = JSON.parse(JSON.stringify(values));
          data.id = this.activeTicket.id;
          data.realStartTime = moment(data.realStartTime).format();
          data.realFinsihTime = moment(data.realFinsihTime).format();
          data.isFine = this.isFine;
          let response = await apiSkyLightPlan.finishWorkTicket(data);
          if (utils.requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.$emit('success');
          }
        }
      });
    },

    async export() {
      this.isLoading = true;
      let response = await apiSkyLightPlan.exportWorkTicket(
        this.sendWorkRecord.skylightPlanId,
        false,
      );
      //获取content-disposition
      const disposition = response.headers['content-disposition'];
      let fileName = disposition.substring(disposition.indexOf('UTF-8') + 7, disposition.length);
      // iso8859-1的字符转换成中文
      fileName = decodeURI(fileName);
      // 去掉双引号
      fileName = fileName.replace(/\"/g, '');

      return new Promise(async (resolve, reject) => {
        if (requestIsSuccess(response)) {
          this.isLoading = false;
          FileSaver.saveAs(new Blob([response.data]), fileName);
          setTimeout(resolve, 100);
        } else {
          setTimeout(reject, 100);
        }
      });
    },

    close() {
      this.status = ModalStatus.Hide;
      this.form.resetFields();
    },
  },
  render() {
    let options = [];
    for (let item in RepairLevel) {
      options.push(
        <a-select-option key={RepairLevel[item]} title={getRepairLevelTitle(RepairLevel[item])}>
          {getRepairLevelTitle(RepairLevel[item])}
        </a-select-option>,
      );
    }
    return (
      <a-modal
        class="sm-cr-plan-plan-senting-work-finish-ticket-modal"
        title={`${this.title}工作票 ${this.ticketIndex + 1}/${
          this.tickets.length
        } ${this.getActiveState()}`}
        visible={this.visible}
        onCancel={this.close}
        onOk={this.close}
        width={820}
      >
        <a-row gutter={24} type="flex" align="middle">
          <a-col sm={2} md={2}>
            <a-button type="primary" disabled={this.ticketIndex == 0} onClick={this.previousTicket}>
              ←
            </a-button>
          </a-col>
          <a-col sm={20} md={20}>
            <a-form form={this.form} layout="vertical">
              <a-row gutter={24}>
                <a-col sm={12} md={12}>
                  <a-form-item label="作业名称" style="margin-bottom:0">
                    <a-input
                      style="width: 100%"
                      //value={this.avticeTicket.workTitle}
                      disabled
                      v-decorator={['workTitle']}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={12} md={12}>
                  <a-form-item label="命令票号" style="margin-bottom:0">
                    <a-input
                      style="width: 100%"
                      disabled={this.status === ModalStatus.View}
                      v-decorator={[
                        'orderNumber',
                        {
                          initialValue: '',
                          rules: [{ required: true, message: '请输入命令票号！' }],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
              </a-row>

              <a-row gutter={24}>
                <a-col sm={24} md={12}>
                  <a-form-item label="作业地点" style="margin-bottom:0">
                    <a-input
                      style="width: 100%"
                      // value={this.avticeTicket.workPlace}
                      disabled
                      v-decorator={['workPlace']}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={24} md={12}>
                  <a-form-item label="施工维修等级" style="margin-bottom:0">
                    <a-select
                      mode="multiple"
                      disabled
                      v-decorator={[
                        'repairLevel',
                        {
                          initialValue: null,
                        },
                      ]}
                    >
                      {options}
                    </a-select>
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row gutter={24}>
                <a-col sm={24} md={24}>
                  <a-form-item label="作业内容" style="margin-bottom:0">
                    <a-textarea
                      rows={4}
                      // value={this.avticeTicket.workContent}
                      disabled
                      v-decorator={['workContent']}
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row gutter={24}>
                <a-col sm={24} md={24}>
                  <a-form-item label="影响范围" style="margin-bottom:0">
                    <a-textarea
                      rows={3}
                      disabled={this.status === ModalStatus.View}
                      v-decorator={[
                        'influenceRange',
                        {
                          initialValue: '',
                          rules: [{ required: true, message: '请输入影响范围！' }],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row gutter={24}>
                <a-col sm={24} md={12}>
                  <a-form-item label="作业计划开始时间" style="margin-bottom:0">
                    <a-input
                      style="width: 100%"
                      disabled
                      // value={this.avticeTicket.planStartTime}
                      v-decorator={['planStartTime']}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={24} md={12}>
                  <a-form-item label="作业计划结束时间" style="margin-bottom:0">
                    <a-input
                      style="width: 100%"
                      disabled
                      // value={this.avticeTicket.planFinishTime}
                      v-decorator={['planFinishTime']}
                    />
                  </a-form-item>
                </a-col>
              </a-row>

              <a-row gutter={24}>
                <a-col sm={24} md={12}>
                  <a-form-item label="作业实际开始时间" style="margin-bottom:0">
                    <a-date-picker
                      style="width: 100%"
                      disabled={this.status === ModalStatus.View}
                      show-time
                      // value={this.avticeTicket.realStartTime}
                      v-decorator={[
                        'realStartTime',
                        {
                          // initialValue: '',
                          rules: [{ required: true, message: '请选择时间！' }],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={24} md={12}>
                  <a-form-item label="作业实际结束时间" style="margin-bottom:0">
                    <a-date-picker
                      style="width: 100%"
                      disabled={this.status === ModalStatus.View}
                      show-time
                      v-decorator={[
                        'realFinsihTime',
                        {
                          // initialValue: '',
                          rules: [{ required: true, message: '请选择时间！' }],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row gutter={24}>
                <a-col sm={24} md={24}>
                  <a-form-item label="安全技术措施及注意事项" style="margin-bottom:0">
                    <a-textarea
                      rows={3}
                      // value={this.avticeTicket.securityMeasuresAndAttentions}
                      disabled
                      v-decorator={['securityMeasuresAndAttentions']}
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row gutter={24}>
                <a-col sm={24} md={24}>
                  <a-form-item label="工作完成情况" style="margin-bottom:0">
                    <a-textarea
                      rows={2}
                      disabled={this.status === ModalStatus.View}
                      v-decorator={[
                        'finishContent',
                        {
                          initialValue: '',
                          rules: [{ required: true, message: '请输入完成情况！' }],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row gutter={24}>
                <a-col sm={24} md={24}>
                  <a-form-item
                    label="通信工具检查试验是否良好"
                    style="margin-bottom:0"
                    label-col={{ span: 7}}
                    wrapper-col={{ span: 15 }}
                  >
                    <a-radio-group
                      value={this.isFine}
                      onChange={item => {
                        this.isFine = item.target.value;
                      }}
                      disabled={this.status === ModalStatus.View}
                    >
                      <a-radio value={true}>是</a-radio>
                      <a-radio value={false}>否</a-radio>
                    </a-radio-group>
                  </a-form-item>
                </a-col>
              </a-row>
              {this.workTicketRltCooperationUnits.length > 0
                ? [
                  <a-row gutter={24}>
                    <a-col sm={24} md={24}>
                      <a-form-item label="配合车间" style="margin-bottom:0">
                        <SmSystemUserSelect
                          axios={this.axios}
                          placeholder="请选择配合作业单位"
                          height={65}
                          disabled={true}
                          value={this.workTicketRltCooperationUnits}
                          onChange={value => {
                            this.workTicketRltCooperationUnits = value;
                          }}
                        />
                      </a-form-item>
                    </a-col>
                  </a-row>,
                  <a-row gutter={24}>
                    <a-col sm={24} md={24}>
                      <a-form-item label="配合车间工作完成情况" style="margin-bottom:0">
                        <a-textarea rows={2} disabled value={this.cooperationUnitCompletion} />
                      </a-form-item>
                    </a-col>
                  </a-row>,
                ]
                : null}
            </a-form>
          </a-col>
          <a-col sm={2} md={2}>
            <a-button
              type="primary"
              disabled={this.tickets.length == 0 || this.ticketIndex == this.tickets.length - 1}
              onClick={this.nextTicket}
            >
              →
            </a-button>
          </a-col>
        </a-row>
        <template slot="footer">
          {this.status == ModalStatus.View ? (
            <div>
              <a-button type="primary" onClick={this.export}>
                下载
              </a-button>
              <a-button onClick={this.close}>取消</a-button>
            </div>
          ) : (
            <div>
              <a-button type="primary" onClick={this.export}>
                下载
              </a-button>
              <a-button type="primary" onClick={this.save}>
                保存
              </a-button>
              <a-button onClick={this.close}>取消</a-button>
            </div>
          )}
        </template>
      </a-modal>
    );
  },
};
