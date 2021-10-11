import ApiSkyLightPlan from '../../sm-api/sm-cr-plan/SkyLightPlan';
import * as utils from '../../_utils/utils';
import { requestIsSuccess, getRepairLevelTitle, objFilterProps } from '../../_utils/utils';
import moment from 'moment';
import {
  ModalStatus,
  SelectablePlanType,
  RepairLevel,
  WorkTicketRltCooperationUnitState,
} from '../../_utils/enum';

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
  'securityMeasuresAndAttentions',
  'finishContent',
  'cooperateContent',
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
      realStartTime: moment(),
      realFinsihTime: moment(),
      workTicketRltCooperationUnitId: null,
      completion:null,
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
      // console.log(record);
      this.sendWorkRecord = record;
      //   this.realStartTime = moment(record.cooperateRealStartTime).format('yyyy-MM-DD HH:mm:ss');
      //   this.realFinsihTime = moment(record.cooperateRealFinishTime).format(
      //     'yyyy-MM-DD HH:mm:ss',
      //   );
      this.completion = record.completion,
      this.status = ModalStatus.View;
      this.refresh(record);
    },

    finish(record) {
      this.sendWorkRecord = record;
      this.workTicketRltCooperationUnitId = record.workTicketRltCooperationUnitId;
      console.log(this.workTicketRltCooperationUnitId);
      this.completion=record.completion,
      this.status = ModalStatus.Edit;
      this.refresh(record);
    },

    getActiveState() {
      if (activeTicket == null) return '';
      return activeTicket.isFinish ? '完成' : '未完成';
    },

    //初始化派工单
    async refresh(record = null) {
      let response = await apiSkyLightPlan.getWorkTickets(this.sendWorkRecord.skylightPlanId);
      if (requestIsSuccess(response)) {
        this.tickets = response.data;
        let activeTicket = this.tickets[0];
        activeTicket.repairLevel =
              activeTicket &&
              activeTicket.repairLevel &&
              activeTicket.repairLevel.split(',').length > 0
                ? activeTicket.repairLevel.split(',').map(Number)
                : null;
        this.$nextTick(() => {
          let values = utils.objFilterProps(activeTicket, formFields);
          values.planStartTime = moment(activeTicket.planStartTime).format('YYYY-MM-DD HH:mm:ss');
          values.planFinishTime = moment(activeTicket.planFinishTime).format('YYYY-MM-DD HH:mm:ss');
          values.finishContent = this.completion;
         
          this.realFinsihTime =
            record.state == WorkTicketRltCooperationUnitState.Finish
              ? moment(record.cooperateRealFinishTime).format('yyyy-MM-DD HH:mm:ss')
              : moment(activeTicket.planFinishTime).format('yyyy-MM-DD HH:mm:ss');
          this.realStartTime =
               record.state == WorkTicketRltCooperationUnitState.Finish
                 ? moment(record.cooperateRealStartTime).format('yyyy-MM-DD HH:mm:ss')
                 : moment(activeTicket.planStartTime).format('yyyy-MM-DD HH:mm:ss');
          this.form.setFieldsValue(values);
        });
      }
    },
    //设置页面中显示的ticket
    // setPageTicket(index) {
    //   if (this.tickets.length > 0) {
    //     activeTicket = this.tickets[index];
    //     activeTicket.repairLevel =
    //       activeTicket &&
    //       activeTicket.repairLevel &&
    //       activeTicket.repairLevel.split(',').length > 0
    //         ? activeTicket.repairLevel.split(',').map(Number)
    //         : null;
    //     this.$nextTick(() => {
    //       let values = utils.objFilterProps(activeTicket, formFields);

    //       console.log(activeTicket.startPlanTime);
    //       values.planStartTime = moment(activeTicket.startPlanTime).format(
    //         'YYYY-MM-DD HH:mm:ss',
    //       );
    //       values.planFinishTime = moment(activeTicket.endPlanTime).format(
    //         'YYYY-MM-DD HH:mm:ss',
    //       );

    //       values.finishContent = this.completion;
    //       //   values.realFinsihTime = moment(activeTicket.realFinsihTime);

    //       this.form.setFieldsValue(values);
    //     });
    //   }
    // },
    previousTicket() {
      if (this.ticketIndex > 0) this.ticketIndex--;
      this.setPageTicket(this.ticketIndex);
      activeTicket = this.tickets[this.ticketIndex];
    },
    nextTicket() {
      if (this.ticketIndex < this.tickets.length - 1) this.ticketIndex++;
      this.setPageTicket(this.ticketIndex);
      activeTicket = this.tickets[this.ticketIndex];
    },
    save() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let data = {
            cooperationWorkId: this.workTicketRltCooperationUnitId,
            cooperateRealFinishTime: this.realFinsihTime,
            cooperateRealStartTime: this.realStartTime,
            completion: this.form.getFieldValue("finishContent"),
          };
          let response = await apiSkyLightPlan.finishCooperationWork(data);
          if (utils.requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.$emit('success');
            this.close();
          }
        }
      });
    },
    // async export() {
    //   this.isLoading = true;
    //   let response = await apiSkyLightPlan.exportWorkTicket(
    //     this.sendWorkRecord.skylightPlanId,
    //     false,
    //   );
    //   //获取content-disposition
    //   const disposition = response.headers['content-disposition'];
    //   let fileName = disposition.substring(disposition.indexOf('UTF-8') + 7, disposition.length);
    //   // iso8859-1的字符转换成中文
    //   fileName = decodeURI(fileName);
    //   // 去掉双引号
    //   fileName = fileName.replace(/\"/g, '');

    //   return new Promise(async (resolve, reject) => {
    //     if (requestIsSuccess(response)) {
    //       this.isLoading = false;
    //       FileSaver.saveAs(new Blob([response.data]), fileName);
    //       setTimeout(resolve, 100);
    //     } else {
    //       setTimeout(reject, 100);
    //     }
    //   });
    // },
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
        title={'配合作业详情'}
        visible={this.visible}
        onCancel={this.close}
        onOk={this.close}
        width={820}
      >
        <a-row gutter={24}>
          <a-col sm={24} md={24}>
            <a-form form={this.form}>
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
                      disabled={true}
                      v-decorator={[
                        'orderNumber',
                        {
                          initialValue: '',
                          //   rules: [{ required: true, message: '请输入命令票号！' }],
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
                  <a-form-item label="主体车间作业内容" style="margin-bottom:0">
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
                  <a-form-item label="配合作业内容" style="margin-bottom:0">
                    <a-textarea
                      rows={4}
                      // value={this.avticeTicket.workContent}
                      disabled
                      v-decorator={['cooperateContent']}
                    />
                  </a-form-item>
                </a-col>
              </a-row>
              <a-row gutter={24}>
                <a-col sm={24} md={24}>
                  <a-form-item label="影响范围" style="margin-bottom:0">
                    <a-textarea
                      rows={3}
                      disabled={true}
                      v-decorator={[
                        'influenceRange',
                        {
                          initialValue: '',
                          //   rules: [{ required: true, message: '请输入影响范围！' }],
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
                      value={this.realStartTime}
                      onChange={value => {
                        this.realStartTime = value;
                      }}
                    />
                  </a-form-item>
                </a-col>
                <a-col sm={24} md={12}>
                  <a-form-item label="作业实际结束时间" style="margin-bottom:0">
                    <a-date-picker
                      style="width: 100%"
                      disabled={this.status === ModalStatus.View}
                      show-time
                      value={this.realFinsihTime}
                      onChange={value => {
                        this.realFinsihTime = value;
                      }}
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
                          rules: [
                            { required: true, whitespace: true, message: '请输入完成情况！' },
                          ],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
              </a-row>
            </a-form>
          </a-col>
        </a-row>
        <template slot="footer">
          {this.status == ModalStatus.View ? (
            <a-button onClick={this.close}>取消</a-button>
          ) : (
            <a-button onClick={this.save} type="primary">
              保存
            </a-button>
          )}
        </template>
      </a-modal>
    );
  },
};
