import * as utils from '../../../_utils/utils';
import moment from 'moment';
import {
  ModalStatus, SelectablePlanType, MemberType, RepairTagKeys, WorkContentType, OrderState,
} from '../../../_utils/enum';
import FileSaver from 'file-saver';
import SmSystemUserSelect from '../../../sm-system/sm-system-member-select';
import ApiWorkOrder from '../../../sm-api/sm-cr-plan/WorkOrder';

let apiWorkOrder = new ApiWorkOrder();

export default {
  name: 'SmCrPlanSendingWorkModal',
  props: {
    value: { type: Boolean, default: null },
    axios: { type: Function, default: null },
    repairTagKey: { type: String, default: null }, //维修项标签
  },
  data() {
    return {
      stations: [],
      status: ModalStatus.Hide,
      form: {}, // 表单
      record: {},
      formData: {},
      toolSituation: null,//通信工具检查情况
      platformLiaisonOfficer: false,//是否选择为驻台联络员
      skylightPlanId: null,//垂直天窗id
    };
  },
  computed: {
    visible() {
      return this.status !== ModalStatus.Hide;
    },
    title() {
      return utils.getModalTitle(this.status);
    },
    columns() {
      return [
        {
          title: '年/月表',
          dataIndex: 'planType',
          width: 80,
          scopedSlots: { customRender: 'planType' },
        },
        {
          title: '序号',
          dataIndex: 'number',
          width: 90,
          ellipsis: true,
          scopedSlots: { customRender: 'number' },
        },
        {
          title: '作业机房',
          dataIndex: 'workSiteName',
          ellipsis: true,
          scopedSlots: { customRender: 'workSiteName' },
        },
        {
          title: '设备名称',
          dataIndex: 'equipName',
          ellipsis: true,
          scopedSlots: { customRender: 'equipName' },
        },
        {
          title: '工作内容',
          ellipsis: true,
          dataIndex: 'content',
          scopedSlots: { customRender: 'content' },
        },
        {
          title: '作业数量',
          dataIndex: 'planCount',
          width: 100,
        },
        {
          title: '关联设备',
          dataIndex: 'equipments',
          ellipsis: true,
          scopedSlots: { customRender: 'equipments' },
        },
      ];
    },
  },
  created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    initAxios() {
      apiWorkOrder = new ApiWorkOrder(this.axios);
    },

    // 派工单
    async dispatch(id, state = null) {
      if (state !== OrderState.UnFinished) {
        this.status = ModalStatus.View;
      } else {
        this.status = ModalStatus.Edit;
      }
      await this.refresh(id);
      this.$nextTick(() => {
        this.form.resetFields();
      });
    },

    async refresh(id) {
      let response = await apiWorkOrder.get({ id: id, repairTagKey: this.repairTagKey, isUpdate: true });
      if (utils.requestIsSuccess(response)) {
        this.skylightPlanId = response.data.skylightPlanId;
        if (response.data.planDetailList && response.data.planDetailList.length > 0) {
          response.data.planDetailList.some(item => {
            item.workSiteName = response.data.workSiteName;
          });
        }
        this.record = response.data;
        this.record.dispatchingTime = this.record.dispatchingTime ? moment(this.record.dispatchingTime) : null,
        // 初始化可编辑的表单信息
        this.formData = {
          id: this.record.id,
          startPlanTime: this.record.startPlanTime ? moment(this.record.startPlanTime) : null,
          endPlanTime: this.record.endPlanTime ? moment(this.record.endPlanTime) : null,
          workLeader:
              this.record.workLeader ? [{ id: this.record.workLeader.userId, type: MemberType.User }] : [], // 作业组长
          workMemberList:
              this.record.workMemberList && this.record.workMemberList.length > 0
                ? this.record.workMemberList.map(user => { return { id: user.userId, type: MemberType.User }; })
                : [], // 作业人员
          fieldGuardList:
              this.record.fieldGuardList && this.record.fieldGuardList.length > 0
                ? this.record.fieldGuardList.map(user => { return { id: user.userId, type: MemberType.User }; })
                : [], //现场联系人
          stationLiaisonOfficerList:
              this.record.stationLiaisonOfficerList && this.record.stationLiaisonOfficerList.length > 0
                ? this.record.stationLiaisonOfficerList.map(user => { return { id: user.userId, type: MemberType.User }; })
                : [], // 驻场联络人
        };
      }
    },

    //设置时间禁选项
    startTimeDisabledDate(current) {
      return (
        current <=
        moment(this.record.startPlanTime)
          .subtract(1, 'month')
          .endOf('month') ||
        current >=
        moment(this.record.startPlanTime)
          .add(1, 'month')
          .startOf('month')
      );
    },

    endTimeDisabledDate(current) {
      return (
        current <=
        moment(this.record.endPlanTime)
          .subtract(1, 'month')
          .endOf('month') ||
        current >=
        moment(this.record.endPlanTime)
          .add(1, 'month')
          .startOf('month')
      );
    },

    close() {
      this.form.resetFields();
      this.status = ModalStatus.Hide;
      this.record = null;
    },

    // 保存
    ok() {
      if (this.formData.workLeader.length < 1) {
        this.$message.error('请选择作业组长');
        return;
      }
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let data = JSON.parse(JSON.stringify(this.formData));

          data = {
            id: this.record.id,
            startPlanTime: moment(this.formData.startPlanTime).format(),
            endPlanTime: moment(this.formData.endPlanTime).format(),
            workLeader: data.workLeader[0].id,
            workMemberList: data.workMemberList.map(item => item.id),
            stationLiaisonOfficerList: data.stationLiaisonOfficerList.map(item => item.id),
            fieldGuardList: data.fieldGuardList.map(item => item.id),
            toolSituation: this.toolSituation,
          };
          let response = await apiWorkOrder.update(data, this.repairTagKey);

          if (utils.requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.close();
            this.$emit('success');
          }
        }
      });
    },

    // 导出
    async export() {
      let response = await apiWorkOrder.export(this.record.id, false, this.repairTagKey);
      if (utils.requestIsSuccess(response)) {
        FileSaver.saveAs(
          new Blob([response.data], { type: 'application/vnd.ms-excel' }),
          `${this.record.name}.xlsx`,
        );
      }
    },

    //选择驻台联络员
    async checkPlatformLiaisonOfficer() {
      this.platformLiaisonOfficer = true;
      let response = await apiWorkOrder.checkPlatformLiaisonOfficer({ skylightPlanId: this.skylightPlanId });

      if (utils.requestIsSuccess(response) && response.data) {
        this.formData.stationLiaisonOfficerList.push(response.data);
      }
    },
  },
  render() {
    return (
      <a-modal
        class="sm-cr-plan-plan-todo-modal"
        title={`${this.title}派工单`}
        visible={this.visible}
        onCancel={this.close}
        width={800}
        scroll={{ y: 300 }}
      >
        <a-form form={this.form} layout="vertical">
          <a-row gutter={24}>
            <div style="display:flex">
              <a-col sm={24} md={12}>
                <a-form-item label="作业组长">
                  <SmSystemUserSelect
                    axios={this.axios}
                    showUserTab={true}
                    disabled={this.status == ModalStatus.View}
                    height={40}
                    showUserTab={true}
                    value={this.formData.workLeader}
                    onChange={values => {
                      this.formData.workLeader = values;
                    }}
                  />
                </a-form-item>
              </a-col>

              <a-col sm={24} md={12}>
                <a-form-item label="作业人员">
                  <SmSystemUserSelect
                    axios={this.axios}
                    showUserTab={true}
                    disabled={this.status == ModalStatus.View}
                    height={40}
                    showUserTab={true}
                    value={this.formData.workMemberList}
                    onChange={values => {
                      this.formData.workMemberList = values;
                    }}
                  />
                </a-form-item>
              </a-col>
            </div>
            <div style="display:flex">
              <a-col sm={24} md={12}>
                {
                  //高铁科需求更改
                  // this.repairTagKey == RepairTagKeys.RailwayHighSpeed ?
                  //   [
                  //     <a-radio-group name="checkType" style="margin-bottom:8px" >
                  //       <a-radio
                  //         value="2"
                  //         onChange={value => { this.platformLiaisonOfficer = false; this.formData.stationLiaisonOfficerList = []; }
                  //         }>
                  //         驻站联络员
                  //       </a-radio>
                  //       <a-radio
                  //         value="1"
                  //         onChange={value => { this.checkPlatformLiaisonOfficer(); }
                  //         }
                  //       >
                  //         驻台联络员
                  //       </a-radio>
                  //     </a-radio-group>,
                  //     <SmSystemUserSelect
                  //       axios={this.axios}
                  //       height={40}
                  //       showUserTab={true}
                  //       value={this.formData.stationLiaisonOfficerList}
                  //       // disabled={this.platformLiaisonOfficer}
                  //       onChange={values => {
                  //         this.formData.stationLiaisonOfficerList = values;
                  //       }}
                  //     />,
                  //   ]
                  //   :
                  <a-form-item
                    label={`${this.repairTagKey == RepairTagKeys.RailwayHighSpeed
                      ? '驻站/驻台联络员'
                      : '驻站联络员'
                    }`}
                  >
                    <SmSystemUserSelect
                      axios={this.axios}
                      height={40}
                      showUserTab={true}
                      value={this.formData.stationLiaisonOfficerList}
                      disabled={this.status == ModalStatus.View}
                      onChange={values => {
                        this.formData.stationLiaisonOfficerList = values;
                      }}
                    />
                  </a-form-item>
                }
              </a-col>

              <a-col sm={24} md={12}>
                <a-form-item label="现场联系人员">
                  <SmSystemUserSelect
                    axios={this.axios}
                    height={40}
                    showUserTab={true}
                    value={this.formData.fieldGuardList}
                    disabled={this.status == ModalStatus.View}
                    onChange={values => {
                      this.formData.fieldGuardList = values;
                    }}
                  />
                </a-form-item>
              </a-col>
            </div>

            <a-col sm={24} md={12}>
              <a-form-item label="作业开始时间">
                <a-date-picker
                  style="width: 100%"
                  placeholder="请输入"
                  show-time
                  allowClear={false}
                  disabledDate={this.startTimeDisabledDate }
                  disabled={this.status == ModalStatus.View}
                  value={this.formData.startPlanTime}
                  onChange={value => {
                    this.formData.startPlanTime = value;
                  }}
                />
              </a-form-item>
            </a-col>
            <a-col sm={24} md={12}>
              <a-form-item label="作业结束时间">
                <a-date-picker
                  style="width: 100%"
                  placeholder="请输入"
                  show-time
                  allowClear={false}
                  disabledDate={this.endTimeDisabledDate }
                  value={this.formData.endPlanTime}
                  disabled={this.status == ModalStatus.View}
                  onChange={value => {
                    this.formData.endPlanTime = value;
                  }}
                />
              </a-form-item>
            </a-col>

            {/* <a-col sm={24} md={24}>
              <a-form-item label="作业时间">
                <a-range-picker
                  allowClear={false}
                  disabled-date={this.disabledDate}
                  style="width: 100%"
                  value={[
                    moment(this.formData.startPlanTime, 'YYYY-MM-DD HH:mm:ss'),
                    moment(this.formData.endPlanTime, 'YYYY-MM-DD HH:mm:ss'),
                  ]}
                  format="YYYY-MM-DD HH:mm:ss"
                  onChange={(date, dateStrings) => {
                    this.formData.startPlanTime = dateStrings[0];
                    this.formData.endPlanTime = dateStrings[1];
                  }}
                />
              </a-form-item>
            </a-col> */}

            <a-col sm={24} md={24}>
              <a-form-item label="影响范围">
                <a-textarea disabled value={this.record ? this.record.influenceScope : ''} />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={24}>
              <a-form-item label="通信工具检查试验情况">
                <a-textarea
                  value={this.record ? this.record.toolSituation : ''}
                  disabled={this.repairTagKey === RepairTagKeys.RailwayHighSpeed || this.status == ModalStatus.View}
                  onChange={value => {
                    this.toolSituation = value.target.value;
                  }}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={24}>
              <a-form-item label="作业注意事项">
                <a-textarea disabled value={this.record ? this.record.announcements : ''} />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={24}>
              <a-form-item label="计划内容">
                {this.record && this.record.workContentType === WorkContentType.OtherPlan ? (
                  <a-textarea disabled value={this.record.workContent} />
                ) : (
                  <a-table
                    columns={this.columns}
                    dataSource={this.record ? this.record.planDetailList : []}
                    rowKey={record => record.id}
                    pagination={false}
                    scroll={{ y: 300 }}
                    {...{
                      scopedSlots: {
                        planType: (text, record) => {
                          if (SelectablePlanType.Month === record.dailyPlan.planType) return '月表';
                          else return '年表';
                        },
                        number: (text, record) => {
                          return (
                            <a-tooltip placement="topLeft" title={record.dailyPlan.number}>
                              <span>{record.dailyPlan.number}</span>
                            </a-tooltip>
                          );
                        },
                        workSiteName: (text, record) => {
                          return (
                            <a-tooltip placement="topLeft" title={record.workSiteName}>
                              <span>{record.workSiteName}</span>
                            </a-tooltip>
                          );
                        },
                        equipName: (text, record) => {
                          return (
                            <a-tooltip placement="topLeft" title={record.dailyPlan.equipName}>
                              <span>{record.dailyPlan.equipName}</span>
                            </a-tooltip>
                          );
                        },
                        content: (text, record) => {
                          return (
                            <a-tooltip placement="topLeft" title={record.dailyPlan.content}>
                              <span>{record.dailyPlan.content}</span>
                            </a-tooltip>
                          );
                        },
                        equipments: (text, record) => {
                          let relateEquipments = record.relateEquipments.map(
                            item => item.equipmentName,
                          );
                          return (
                            <a-tooltip placement="topLeft" title={relateEquipments.toString()}>
                              <span>{relateEquipments.toString()}</span>
                            </a-tooltip>
                          );
                        },
                      },
                    }}
                  />
                )}
              </a-form-item>
            </a-col>

            <a-col sm={24} md={24}>
              <a-form-item label="派工时间">
                <a-date-picker
                  show-time
                  style="width: 100%"
                  disabled
                  value={this.record ? this.record.dispatchingTime : null}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={12}>
              <a-form-item label={this.repairTagKey == RepairTagKeys.RailwayHighSpeed ? "工区工长" : "派工人员"}>
                <a-input value={this.record ? this.record.sendWorkersName : ''} disabled />
              </a-form-item>
            </a-col>

            {this.repairTagKey !== RepairTagKeys.RailwayHighSpeed ?
              <a-col sm={24} md={12}>
                <a-form-item label="发起人员">
                  <a-input value={this.record ? this.record.creatorName : ''} disabled />
                </a-form-item>
              </a-col> : undefined
            }

          </a-row>
        </a-form>
        <template slot="footer">
          <a-button onClick={this.close}>取消</a-button>
          <a-button type="primary" onClick={this.ok}>
            保存
          </a-button>
          <a-button type="primary" onClick={this.export}>
            导出
          </a-button>
        </template>
      </a-modal>
    );
  },
};
