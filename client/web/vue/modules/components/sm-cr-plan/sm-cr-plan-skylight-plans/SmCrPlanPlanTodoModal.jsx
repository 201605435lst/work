import ApiPlanTodo from '../../sm-api/sm-cr-plan/PlanTodo';
import ApiAccount from '../../sm-api/sm-system/Account';
import * as utils from '../../_utils/utils';
import { requestIsSuccess } from '../../_utils/utils';
import moment from 'moment';
import { ModalStatus, RepairTagKeys, SelectablePlanType, WorkContentType } from '../../_utils/enum';
import OrganizationTreeSelect from '../../sm-system/sm-system-organization-tree-select';
import OrganizationUserSelect from '../../sm-system/sm-system-organization-user-select';
import SmStdBasicWorkAttentionModal from '../../sm-std-basic/sm-std-basic-work-attention-modal';

let apiPlanTodo = new ApiPlanTodo();
let apiAccount = new ApiAccount();
// const formFields = ['influenceScope', 'announcements', 'toolSituation'];
export default {
  name: 'SmCrPlanPlanTodoModal',
  props: {
    value: { type: Boolean, default: null },
    organizationId: { type: String, default: null },
    axios: { type: Function, default: null },
    repairTagKey: { type: String, default: null }, //维修项标签
    workShop: { type: Boolean, default: false },//新增：是否车间派工
  },
  data() {
    return {
      form: this.$form.createForm(this, {}),
      planTodo: null,
      status: ModalStatus.Hide,
      record: null,
      plans: [],
      startTime: null,
      endTime: null,
      dispatchTime: null,
      iOrganizationId: null,
      workAttentionVisible: false, //作业注意事项弹框是否弹出
      loading: false,
    };
  },
  computed: {
    formFields() {
      return this.repairTagKey === RepairTagKeys.RailwayHighSpeed ? [
        'influenceScope',
        'announcements',
        'sendWorkersId',
        'creatorId',
      ] :
        [
          'influenceScope',
          'announcements',
          'toolSituation',
          'sendWorkersId',
          'creatorId',
        ];
    },
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
          dataIndex: 'planTypeStr',
          scopedSlots: { customRender: 'planTypeStr' },
        },
        {
          title: '序号',
          dataIndex: 'number',
          scopedSlots: { customRender: 'number' },
        },
        {
          title: '作业机房',
          dataIndex: 'workSiteName',
          ellipsis: true,
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
          title: '单位',
          ellipsis: true,
          dataIndex: 'unit',
          scopedSlots: { customRender: 'unit' },
        },
        {
          title: '作业数量',
          dataIndex: 'planCount',
        },
        {
          title: '关联设备',
          dataIndex: 'relateEquipments',
          scopedSlots: { customRender: 'relateEquipments' },
          ellipsis: true,
        },
      ];
    },
  },
  watch: {
    organizationId: {
      handler: function (val, oldVal) {
        this.iOrganizationId = val;
      },
      immediate: true,
    },
  },
  created() {
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiPlanTodo = new ApiPlanTodo(this.axios);
      apiAccount = new ApiAccount(this.axios);
    },

    dispatch(record) {
      this.record = record;
      this.status = ModalStatus.Add;
      this.initAxios();
      this.refresh();
    },

    view(record) {
      this.record = record;
      this.status = ModalStatus.View;
      this.initAxios();
      this.refresh();
    },

    //初始化派工单
    async refresh() {
      let response = null;
      if (this.status === ModalStatus.View) {
        response = await apiPlanTodo.getDetail(this.record.id, this.repairTagKey, false);
        let _planTodo;
        if (requestIsSuccess(response)) {
          this.planTodo = response.data;
          _planTodo = this.planTodo;
          this.plans = _planTodo.planDetailList;

          this.$nextTick(() => {
            let values = utils.objFilterProps(_planTodo, this.formFields);
            values.maintenanceUnitId = _planTodo && _planTodo.maintenanceUnit ? _planTodo.maintenanceUnit.organizationId : null;
            values.influenceScope = _planTodo.influenceScope;
            if (this.repairTagKey !== RepairTagKeys.RailwayHighSpeed) {
              values.toolSituation = _planTodo.toolSituation;
              values.communicationUnitId = _planTodo && _planTodo.communicationUnit ? _planTodo.communicationUnit.organizationId : null;
            }
            values.announcements = _planTodo.announcements;
            values.sendWorkersId = this.status != ModalStatus.View ? _planTodo.sendWorkersId : _planTodo.sendWorkersName;
            values.creatorId = this.status != ModalStatus.View ? _planTodo.creatorId : _planTodo.creatorName;
            values.startPlanTime = moment(_planTodo.startPlanTime);
            values.endPlanTime = moment(_planTodo.endPlanTime);
            values.dispatchingTime = moment(_planTodo.dispatchingTime);
            this.form.setFieldsValue(values);
          });
        }
      } else if (this.status === ModalStatus.Add) {
        let userResponse = await apiAccount.getAppConfig();
        let checkUserId = undefined;

        if (requestIsSuccess(userResponse)) {
          checkUserId = userResponse.data.currentUser.id;
        }
        response = await apiPlanTodo.get({ id: this.record.id, repairTagKey: this.repairTagKey });
        let _planTodo;
        if (requestIsSuccess(response)) {
          this.planTodo = response.data;
          _planTodo = this.planTodo;
          this.plans = _planTodo.planDetailList;
          this.$nextTick(() => {
            let values = utils.objFilterProps(_planTodo, this.formFields);
            values.influenceScope = _planTodo.influenceScope;
            values.sendWorkersId = checkUserId;
            values.creatorId = checkUserId;
            values.startPlanTime = moment(_planTodo.startPlanTime);
            values.endPlanTime = moment(_planTodo.endPlanTime);
            this.form.setFieldsValue(values);
          });
        }
      }

    },

    close() {
      this.status = ModalStatus.Hide;
      this.startTime = null;
      this.endTime = null;
      this.dispatchTime = null;
      this.form.resetFields();
    },

    async ok() {
      // 数据提交
      this.form.validateFields(async (err, values) => {
        if (!err) {
          this.loading = true;
          let data = {
            startPlanTime: values.startPlanTime
              ? moment(values.startPlanTime)
                .utc()
                .format()
              : null,
            endPlanTime: values.endPlanTime
              ? moment(values.endPlanTime)
                .utc()
                .format()
              : null,
            influenceScope: values.influenceScope,
            toolSituation: values.toolSituation,
            announcements: values.announcements,
            dispatchingTime: values.dispatchingTime
              ? moment(values.dispatchingTime)
                .utc()
                .format()
              : null,
            sendWorkersId: values.sendWorkersId,
            creatorId: values.creatorId,
            orderState: 0,
            maintenanceUnitId: values.maintenanceUnitId,
            communicationUnitId: values.communicationUnitId,
            skylightPlanId: this.planTodo.skylightPlanId,
            id: this.planTodo.id,
            repairTagKey: this.repairTagKey,
            workShop: this.workShop,
          };
          let response = await apiPlanTodo.create(data, false);
          if (requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.$emit('success');
            this.close();
            this.loading = false;
          }
        }
      });
    },

    //设置时间禁选项
    startTimeDisabledDate(current) {
      return (
        current <=
        moment(this.planTodo.startPlanTime)
          .subtract(1, 'month')
          .endOf('month') ||
        current >=
        moment(this.planTodo.startPlanTime)
          .add(1, 'month')
          .startOf('month')
      );
    },

    endTimeDisabledDate(current) {
      return (
        current <=
        moment(this.planTodo.endPlanTime)
          .subtract(1, 'month')
          .endOf('month') ||
        current >=
        moment(this.planTodo.endPlanTime)
          .add(1, 'month')
          .startOf('month')
      );
    },
  },
  render() {
    return (
      <a-modal
        class="sm-cr-plan-plan-todo-modal"
        title={`${this.title}派工`}
        visible={this.visible}
        onCancel={this.close}
        onOk={this.ok}
        width={800}
        confirmLoading={this.loading}
      >
        {this.status === ModalStatus.View ? (
          <div slot="footer">
            <a-button type="primary" onClick={() => this.close()}>
              {' '}
              关闭{' '}
            </a-button>
          </div>
        ) : null}
        <a-form form={this.form} layout="vertical">
          <a-row gutter={24}>
            <a-col sm={24} md={this.repairTagKey !== 'RepairTag.RailwayHighSpeed' ? 12 : 24}>
              <a-form-item
                label='作业工区'//  label={this.repairTagKey === RepairTagKeys.RailwayWired ? '作业工区' : '检修工区'
              >
                <OrganizationTreeSelect
                  axios={this.axios}
                  placeholder="车间/工区"
                  disabled={this.status === ModalStatus.View}
                  isAutoDisableOrg={true}
                  v-decorator={[
                    'maintenanceUnitId',
                    {
                      initialValue: null,
                      rules: [{ required: true, message: '请输入车间/工区！' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            {this.repairTagKey !== 'RepairTag.RailwayHighSpeed' ? (
              <a-col sm={24} md={12}>
                <a-form-item
                  label={this.repairTagKey === RepairTagKeys.RailwayWired ? '验收工区' : '通信工区'}
                >
                  <OrganizationTreeSelect
                    axios={this.axios}
                    placeholder="车间/工区"
                    disabled={this.status === ModalStatus.View}
                    isAutoDisableOrg={true}
                    v-decorator={[
                      'communicationUnitId',
                      {
                        initialValue: null,
                        rules: [{ required: true, message: '请输入车间/工区！' }],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
            ) : (
              undefined
            )}

            <a-col sm={24} md={12}>
              <a-form-item label="作业开始时间">
                <a-date-picker
                  style="width: 100%"
                  placeholder="请输入"
                  disabled={this.status === ModalStatus.View}
                  show-time
                  disabledDate={this.startTimeDisabledDate}
                  v-decorator={[
                    'startPlanTime',
                    {
                      initialValue: moment(),
                      rules: [{ required: true, message: '请选择作业时间！' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={24} md={12}>
              <a-form-item label="作业结束时间">
                <a-date-picker
                  style="width: 100%"
                  placeholder="请输入"
                  disabled={this.status === ModalStatus.View}
                  show-time
                  disabledDate={this.endTimeDisabledDate}
                  v-decorator={[
                    'endPlanTime',
                    {
                      initialValue: moment(),
                      rules: [{ required: true, message: '请选择作业时间！' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={24}>
              <a-form-item label="影响范围">
                <a-textarea
                  disabled={this.status != ModalStatus.Hide}
                  v-decorator={[
                    'influenceScope',
                    {
                      initialValue: '',
                      rules: [{ whitespace: true, message: '影响范围不可为空字符' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <a-form-item label="作业注意事项">
                <a-input-group compact>
                  <a-textarea
                    placeholder="请输入"
                    disabled={this.status === ModalStatus.View}
                    v-decorator={[
                      'announcements',
                      {
                        initialValue: '',
                        rules: [
                          {
                            required: true,
                            message: '请输入作业注意事项！',
                            // max: 120,
                            whitespace: true,
                          },
                        ],
                      },
                    ]}
                  />
                  {this.status !== ModalStatus.View ? (
                    <a onClick={() => (this.workAttentionVisible = true)}>插入作业注意事项</a>
                  ) : (
                    undefined
                  )}
                </a-input-group>
              </a-form-item>
            </a-col>
            {this.repairTagKey !== RepairTagKeys.RailwayHighSpeed ? (
              <a-col sm={24} md={24}>
                <a-form-item label="通信工具检查试验情况">
                  <a-textarea
                    placeholder="请输入"
                    disabled={this.status === ModalStatus.View}
                    v-decorator={[
                      'toolSituation',
                      {
                        initialValue: '',
                        rules: [
                          {
                            required: true,
                            message: '请输入通信工具检查试验情况！',
                            max: 120,
                            whitespace: true,
                          },
                        ],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
            ) : (
              undefined
            )}

            <a-col sm={24} md={24}>
              <a-form-item label="计划内容">
                {this.planTodo && this.planTodo.workContentType === WorkContentType.OtherPlan ? (
                  <a-textarea disabled value={this.planTodo.workContent} />
                ) : (
                  <a-table
                    columns={this.columns}
                    dataSource={this.plans}
                    rowKey={record => record.id}
                    pagination={false}
                    scroll={{ y: 300 }}
                    {...{
                      scopedSlots: {
                        planTypeStr: (text, record, index) => {
                          return record.dailyPlan.planType === SelectablePlanType.Month
                            ? '月表'
                            : '年表';
                        },
                        number: (text, record, index) => {
                          return record.dailyPlan.number;
                        },
                        equipName: (text, record, index) => {
                          return (
                            <a-tooltip placement="topLeft" title={record.dailyPlan.equipName}>
                              <span>{record.dailyPlan.equipName}</span>
                            </a-tooltip>
                          );
                        },
                        content: (text, record, index) => {
                          return (
                            <a-tooltip placement="topLeft" title={record.dailyPlan.content}>
                              <span>{record.dailyPlan.content}</span>
                            </a-tooltip>
                          );
                        },
                        unit: (text, record, index) => {
                          return (
                            <a-tooltip placement="topLeft" title={record.dailyPlan.unit}>
                              <span>{record.dailyPlan.unit}</span>
                            </a-tooltip>
                          );
                        },
                        relateEquipments: (text, record, index) => {
                          let equipmentNames = '';
                          record.relateEquipments.map(item => {
                            equipmentNames += item.equipmentName ? `${item.equipmentName} ` : '';
                          });
                          return (
                            <a-tooltip placement="topLeft" title={equipmentNames}>
                              <span>{equipmentNames}</span>
                            </a-tooltip>
                          );
                        },
                      },
                    }}
                  ></a-table>
                )}
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <a-form-item label="派工时间">
                <a-date-picker
                  style="width: 100%"
                  placeholder="请输入"
                  disabled={this.status === ModalStatus.View}
                  show-time
                  v-decorator={[
                    'dispatchingTime',
                    {
                      initialValue: moment(),
                      rules: [{ required: true, message: '请选择派工时间！' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={24} md={12}>
              <a-form-item
                label={this.repairTagKey === RepairTagKeys.RailwayWired ? '派工人员' : '工区工长'}
              >
                {this.status === ModalStatus.View ? (
                  <a-input
                    disabled
                    v-decorator={[
                      'sendWorkersId',
                      {
                        initialValue: null,
                      },
                    ]}
                  />
                ) : (
                  <OrganizationUserSelect
                    axios={this.axios}
                    placeholder="请选择"
                    organizationId={this.iOrganizationId}
                    v-decorator={[
                      'sendWorkersId',
                      {
                        initialValue: null,
                        rules: [{ required: true, message: '请选择派工人员！' }],
                      },
                    ]}
                  />
                )}
              </a-form-item>
            </a-col>

            {this.repairTagKey == RepairTagKeys.RailwayHighSpeed ? null : (
              <a-col sm={24} md={12}>
                <a-form-item label="发起人员">
                  {this.status === ModalStatus.View ? (
                    <a-input
                      disabled
                      v-decorator={[
                        'creatorId',
                        {
                          initialValue: null,
                        },
                      ]}
                    />
                  ) : (
                    <OrganizationUserSelect
                      axios={this.axios}
                      placeholder="请选择"
                      organizationId={this.iOrganizationId}
                      v-decorator={[
                        'creatorId',
                        {
                          initialValue: null,
                          rules: [{ required: true, message: '请选择发起人员！' }],
                        },
                      ]}
                    />
                  )}
                </a-form-item>
              </a-col>
            )}
          </a-row>
        </a-form>
        <SmStdBasicWorkAttentionModal
          axios={this.axios}
          multiple={true}
          repairTagKey={this.repairTagKey}
          visible={this.workAttentionVisible}
          onChange={value => (this.workAttentionVisible = value)}
          onOk={selected => {
            let announcements = this.form.getFieldValue('announcements')
              ? this.form.getFieldValue('announcements')
              : '';
            this.form.setFieldsValue({ announcements: announcements + selected });
          }}
        />
      </a-modal>
    );
  },
};
