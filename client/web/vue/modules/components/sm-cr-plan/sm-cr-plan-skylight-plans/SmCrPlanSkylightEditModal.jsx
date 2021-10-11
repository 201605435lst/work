import moment from 'moment';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import ApiSkyLightPlan from '../../sm-api/sm-cr-plan/SkyLightPlan';
import ApiUser from '../../sm-api/sm-system/User';
import ApiWorkflow from '../../sm-api/sm-bpm/Workflow';
import { WorkflowState, UserWorkflowGroup, PageState } from '../../_utils/enum';
let apiSkyLightPlan = new ApiSkyLightPlan();

import {
  requestIsSuccess,
  getWorkflowState,
  getYearMonthPlanStateType,
  getModalTitle,
  objFilterProps,
  vIf,
  vP,
} from '../../_utils/utils';
import { YearMonthPlanState, SelectablePlanType, ModalStatus } from '../../_utils/enum';

const formFields = ['workTime', 'timeLength'];
export default {
  name: 'SmCrPlanSkylightEditModal',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
    permissions: { type: Array, default: () => [] },
    repairTagKey: { type: String, default: null }, //维修项标签
    id: { type: String, default: null },
  },
  data() {
    return {
      creationTime: '',
      peopleName: '',
      loading: false,
      form: this.$form.createForm(this, {}),
      repaireId: null,
      planDetails: [],
      status: ModalStatus.Hide,
    };
  },
  computed: {
    visible() {
      return this.status !== ModalStatus.Hide;
    },
    title() {
      return getModalTitle(this.status);
    },
    columns() {
      return [
        {
          title: '年/月表',
          dataIndex: 'planTypeStr',
          scopedSlots: { customRender: 'planTypeStr' },
          width: 80,
        },
        {
          title: '序号',
          dataIndex: 'number',
          scopedSlots: { customRender: 'number' },
          width: 110,
        },
        {
          title: '设备名称',
          ellipsis: true,
          dataIndex: 'equipName',
        },
        {
          title: '工作内容',
          ellipsis: true,
          dataIndex: 'content',
        },
        {
          title: '日期',
          dataIndex: 'planDate',
          ellipsis: true,
          scopedSlots: { customRender: 'planDate' },
        },
        {
          title: '单位',
          ellipsis: true,
          dataIndex: 'unit',
          width: 60,
        },
        {
          title: '计划数量',
          dataIndex: 'count',
        },
        {
          title: '作业数量',
          dataIndex: 'workCount',
          scopedSlots: { customRender: 'workCount' },
        },
        {
          title: '关联设备',
          dataIndex: 'relateEquipments',
          width: 240,
          scopedSlots: { customRender: 'relateEquipments' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 60,
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  watch: {
    id: {
      handler: function (value, oldValue) {
        this.iId = this.id;
        this.plan = null;
        this.planDetails = [];
        this.form.resetFields();
        if (value) {
          this.initAxios();
          this.refresh();
        }
      },
      immediate: true,
    },
  },

  async created() {
    this.initAxios();
    this.refresh();
  },

  mounted() { },

  methods: {
    initAxios() {
      apiSkyLightPlan = new ApiSkyLightPlan(this.axios);
    },

    edit(skylightPlanId) {
      this.status = ModalStatus.Edit;
      this.refresh(skylightPlanId);
    },

    //初始化表单
    async refresh(id) {
      if (id) {
        this.iId = id;
      }
      let response = await apiSkyLightPlan.get({ id: this.iId, repairTagKey: this.repairTagKey });
      if (requestIsSuccess(response)) {
        this.plan = response.data;
        console.log(this.plan);
        this.$nextTick(() => {
          let values = objFilterProps(this.plan, formFields);
          values = {
            ...values,
            workTime: moment(this.plan.workTime),
          };
          this.planDetails = this.plan.planDetails
            ? this.plan.planDetails.map(item => {
              return {
                ...item,
                planTypeStr: item.dailyPlan ? item.dailyPlan.planTypeStr : '',
                planType: item.dailyPlan ? item.dailyPlan.planType : null,
                number: item.dailyPlan ? item.dailyPlan.number : null,
                equipName: item.dailyPlan ? item.dailyPlan.equipName : null,
                content: item.dailyPlan ? item.dailyPlan.content : null,
                planDate: item.dailyPlan
                  ? moment(item.dailyPlan.planDate).format('YYYY-MM-DD')
                  : '',
                unit: item.dailyPlan ? item.dailyPlan.unit : '',
                count: item.dailyPlan ? item.dailyPlan.count : null,
                workCount: item.planCount,
                relateEquipments: item.relateEquipments.map(item => {
                  return {
                    name: item.equipmentName,
                    id: item.equipmentId,
                    workCount: item.planCount,
                  };
                }),
              };
            })
            : [];
          this.form.setFieldsValue(values);
        });
      }
    },

    removeContentItem(id) {
      this.planDetails = this.planDetails.filter(s => s.id != id);
    },

    async save() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          this.submitLoading = true;
          let data = {
            Id: this.iId,
            PlanDate: values.workTime,
            TimeLength: values.timeLength,
            SavedPlanDetialIds: this.planDetails.map(s => s.id),
          };
          let response = await apiSkyLightPlan.simpleUpdate(data);
          if (requestIsSuccess(response) && response.data) {
            this.$message.info("编辑成功");
            this.$emit('success');
            this.status = ModalStatus.Hide;
          }
          this.submitLoading = false;
        }
      });

    },

    close() {
      this.status = ModalStatus.Hide;
      this.form.resetFields();
    },
  },
  render() {
    return (
      <a-modal
        class="sm-cr-plan-skylight-edit-modal"
        title={`${this.title}计划 `}
        visible={this.visible}
        onCancel={this.close}
        onOk={this.save}
        width={1400}
      >
        {/* 表单区 */}
        <a-form form={this.form}>
          <a-row gutter={24}>
            <a-col sm={12} md={12}>
              <a-form-item label="计划日期" label-col={{ span: 3 }} wrapper-col={{ span: 21 }}>
                <a-date-picker
                  style="width: 100%"
                  showTime={true}
                  disabled={this.iPageState == PageState.View}
                  placeholder={this.iPageState == PageState.View ? '' : '请输入'}
                  disabledDate={this.disabledDate}
                  onChange={value => {
                    this.waitingPlanTime = moment(value)
                      .utc()
                      .format();
                  }}
                  v-decorator={[
                    'workTime',
                    {
                      initialValue:
                        this.iPlanDate && this.iPlanType === PlanType.Vertical
                          ? moment(this.lastSkylightPlanTime ? `${moment(this.iPlanDate).format("YYYY-MM-DD")} ${this.lastSkylightPlanTime}` : this.iPlanDate).date(1)
                          : moment(this.lastSkylightPlanTime ? `${moment(this.iPlanDate).format("YYYY-MM-DD")} ${this.lastSkylightPlanTime}` : this.iPlanDate),
                      rules: [{ required: true, message: '请输入计划日期！' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="计划时长" label-col={{ span: 3 }} wrapper-col={{ span: 21 }}>
                <a-input-number
                  disabled={this.iPageState == PageState.View}
                  style="width: 100%"
                  min={0}
                  precision={0}
                  placeholder={this.iPageState == PageState.View ? '' : '请输入计划时长（分钟）'}
                  v-decorator={[
                    'timeLength',
                    {
                      initialValue: this.lastSkylightPlan ? this.lastSkylightPlan.timeLength : null,
                      rules: [{ required: true, message: '请输入计划时长！' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
          </a-row>
          <a-row gutter={24}>
            <a-col sm={24} md={24}>
              <a-form-item label="计划内容" label-col={{ span: 1 }} wrapper-col={{ span: 23 }}>
                {/* 计划内容展示区 */}
                <a-table
                  columns={this.columns}
                  dataSource={this.planDetails}
                  rowKey={record => record.dailyPlanId}
                  pagination={false}
                  scroll={{ y: 400 }}
                  {...{
                    scopedSlots: {
                      planDate: (text, record) => {
                        return (
                          <a-tooltip
                            placement="topLeft"
                            title={
                              record.planDate ? moment(record.planDate).format('YYYY-MM-DD') : ''
                            }
                          >
                            <span>
                              {record.planDate ? moment(record.planDate).format('YYYY-MM-DD') : ''}
                            </span>
                          </a-tooltip>
                        );
                      },
                      planTypeStr: (text, record, index) => {
                        return record.planType === SelectablePlanType.Month ? '月表' : '年表';
                      },
                      workCount: (text, record) => {
                        return record.relateEquipments.length > 0 ? (
                          this.getWorkCount(record)
                        ) : (
                          <span
                            style="margin: -10px 0"
                            precision={3}
                          >{record.workCount}</span>
                        );
                      },

                      operations: (text, record) => {
                        return (<a onClick={() => { this.removeContentItem(record.id); }} >删除</a>);
                      },
                    },
                  }}
                ></a-table>
              </a-form-item>
            </a-col>
          </a-row>
        </a-form>
      </a-modal>
    );
  },
};
