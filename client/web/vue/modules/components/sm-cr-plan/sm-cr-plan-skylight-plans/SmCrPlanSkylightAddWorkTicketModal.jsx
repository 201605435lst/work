import { ModalStatus } from '../../_utils/enum';
import { form as formConfig } from '../../_utils/config';
import * as utils from '../../_utils/utils';
import SmSystemUserSelect from '../../sm-system/sm-system-member-select';
import ApiSkylightPlan from '../../sm-api/sm-cr-plan/SkyLightPlan';
import moment from 'moment';

let apiSkylightPlan = new ApiSkylightPlan();

const formFields = [
  'workTitle', /// 作业名称
  'workPlace', /// 作业地点
  'workContent',/// 作业内容
  'influenceRange',/// 影响范围
  'securityMeasuresAndAttentions',  /// 安全技术措施及注意事项
  'paperMaker', /// 制表人
  'personInCharge', /// 作业负责人
];

export default {
  name: "SmCrPlanSkylightAddWorkTicketModal",
  props: {
    axios: { type: Function, default: null },
    workTicketId: { type: String, default: null },
    skylightPlanId: { type: String, default: null },
  },
  data() {
    return {
      form: {},
      status: ModalStatus.Hide,
      loading: false,
      repaireId: null,
      dateRange: [moment(moment()).startOf('second'), moment(moment()).endOf('second')],
      workTicketRltCooperationUnits: [],
      cooperateContent: null,
      safeGuard:true,
    };
  },
  computed: {
    title() {
      return utils.getModalTitle(this.status) + "工作票";
    },
    visible() {
      return this.status !== ModalStatus.Hide;
    },
    disabled() {
      return this.status == ModalStatus.View;
    },
  },
  watch: {
  },
  created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    initAxios() {
      apiSkylightPlan = new ApiSkylightPlan(this.axios);
    },

    add(record) {
      this.status = ModalStatus.Add;
      this.repaireId = record.id;
      this.dateRange = [moment(record.workTime).startOf('second'), moment(record.workTime).add(record.timeLength,"minutes").startOf('second')];
      // this.$nextTick(() => {
      //   this.form.setFieldsValue({ influenceRange: record.incidence });
      // });
    },
    view(record) {
      console.log(record);
      this.status = ModalStatus.View;
      this.$nextTick(() => {
        this.dateRange = [moment(record.planStartTime).startOf('second'), moment(record.planFinishTime).startOf('second')];
        this.workTicketRltCooperationUnits =
          record.workTicketRltCooperationUnits == null ? [] : record.workTicketRltCooperationUnits;
        this.cooperateContent = record.cooperateContent;
        this.safeGuard = record.safeGuard;
        let value = { ...utils.objFilterProps(record, formFields) };
        this.form.setFieldsValue(value);
      });
    },
    edit(record) {
      this.status = ModalStatus.Edit;
      this.repaireId = record.id;
      this.$nextTick(() => {
        this.dateRange = [moment(record.planStartTime).startOf('second'), moment(record.planFinishTime).startOf('second')];
        this.workTicketRltCooperationUnits =
          record.workTicketRltCooperationUnits == null ? [] : record.workTicketRltCooperationUnits;
        this.cooperateContent = record.cooperateContent;
        let value = { ...utils.objFilterProps(record, formFields) };
        this.form.setFieldsValue(value);
      });
    },
    async save() {
      this.form.validateFields(async (err, value) => {
        if (!err) {
          this.loading = true;
          let data = {
            ...value,
            planStartTime: moment(this.dateRange[0]).format('YYYY-MM-DD HH:MM:SS'),
            planFinishTime: moment(this.dateRange[1]).format('YYYY-MM-DD HH:MM:SS'),
            skylightPlanId: this.skylightPlanId,
            workTicketRltCooperationUnits: this.workTicketRltCooperationUnits,
            cooperateContent: this.cooperateContent,
            safeGuard:this.safeGuard,
          };
          let response;
          if (this.status == ModalStatus.Add) {
            response = await apiSkylightPlan.createWorkTicket(data);
          } else if (this.status == ModalStatus.Edit) {
            data = {
              ...data,
              id: this.workTicketId,
            };
            response = await apiSkylightPlan.updateWorkTicket(data);
          }
          if (utils.requestIsSuccess(response) && response.data) {
            this.$emit('ok', this.repaireId);
            this.status = ModalStatus.Hide;
            this.$message.info("操作成功");
          }
          this.loading = false;
        }
      });
    },
    close() {
      this.status = ModalStatus.Hide;
    },
  },
  render() {
    return (
      <div class="SmCrPlanSkylightAddWorkTicketModal">
        <a-modal
          visible={this.visible}
          title={this.title}
          onCancel={this.close}
          confirmLoading={this.loading}
          // maskClosable={false}
          onOk={() => {
            !this.disabled ? this.save() : this.close();
          }}
          destroyOnClose={true}
        >
          <a-form form={this.form}>
            <a-form-item
              label="制表人"
              label-col={formConfig.labelCol}
              wrapper-col={formConfig.wrapperCol}
            >
              <a-input
                placeholder="请输入制表人"
                disabled={this.disabled}
                v-decorator={[
                  'paperMaker',
                  {
                    initialValue: null,
                    rules: [
                      {
                        required: true,
                        message: '请输入制表人',
                        whitespace: true,
                      },
                    ],
                  },
                ]}
              ></a-input>
            </a-form-item>

            <a-form-item
              label="作业名称"
              label-col={formConfig.labelCol}
              wrapper-col={formConfig.wrapperCol}
            >
              <a-input
                disabled={this.disabled}
                placeholder="请输入作业名称"
                v-decorator={[
                  'workTitle',
                  {
                    initialValue: null,
                    rules: [
                      {
                        required: true,
                        message: '请输入作业名称',
                        whitespace: true,
                      },
                    ],
                  },
                ]}
              ></a-input>
            </a-form-item>
            <a-form-item
              label="作业地点"
              label-col={formConfig.labelCol}
              wrapper-col={formConfig.wrapperCol}
            >
              <a-input
                disabled={this.disabled}
                placeholder="请输入作业地点"
                v-decorator={[
                  'workPlace',
                  {
                    initialValue: null,
                    rules: [
                      {
                        required: true,
                        message: '请输入作业地点',
                        whitespace: true,
                      },
                    ],
                  },
                ]}
              ></a-input>
            </a-form-item>
            <a-form-item
              label="作业内容"
              label-col={formConfig.labelCol}
              wrapper-col={formConfig.wrapperCol}
            >
              <a-input
                disabled={this.disabled}
                type="textarea"
                placeholder="请输入作业内容"
                v-decorator={[
                  'workContent',
                  {
                    initialValue: null,
                    rules: [
                      {
                        required: true,
                        message: '请输入作业内容',
                        whitespace: true,
                      },
                    ],
                  },
                ]}
              ></a-input>
            </a-form-item>
            <a-form-item
              disabled={this.disabled}
              label="影响范围"
              label-col={formConfig.labelCol}
              wrapper-col={formConfig.wrapperCol}
            >
              <a-input
                disabled={this.disabled}
                type="textarea"
                placeholder="请输入影响范围"
                v-decorator={[
                  'influenceRange',
                  {
                    initialValue: null,
                    rules: [
                      {
                        whitespace: true,
                        message: '影响范围不能为空字符',
                      },
                    ],
                  },
                ]}
              ></a-input>
            </a-form-item>
            <a-form-item
              disabled={this.disabled}
              label="作业负责人"
              label-col={formConfig.labelCol}
              wrapper-col={formConfig.wrapperCol}
            >
              <a-input
                disabled={this.disabled}
                placeholder="请输入作业负责人"
                v-decorator={[
                  'personInCharge',
                  {
                    initialValue: null,
                    rules: [
                      {
                        required: true,
                        message: '请输入作业负责人',
                        whitespace: true,
                      },
                    ],
                  },
                ]}
              ></a-input>
            </a-form-item>
            <a-form-item
              disabled={this.disabled}
              label="计划时间"
              label-col={formConfig.labelCol}
              wrapper-col={formConfig.wrapperCol}
            >
              <a-range-picker
                disabled={this.disabled}
                style="width: 100%;"
                placeholder={['请选择计划开始时间', '请选择计划结束时间']}
                showTime
                allowClear={false}
                value={this.dateRange}
                onChange={value => {
                  this.dateRange = value;
                }}
              ></a-range-picker>
            </a-form-item>

            <a-form-item label="是否设置防护员" label-col={{ span: 6 }} wrapper-col={{ span: 16 }}>
              <a-radio-group
                value={this.safeGuard}
                onChange={item => {
                  this.safeGuard = item.target.value;
                }}
                disabled={this.disabled}
              >
                <a-radio value={true}>是</a-radio>
                <a-radio value={false}>否</a-radio>
              </a-radio-group>
            </a-form-item>

            <a-form-item
              label-col={formConfig.labelCol}
              wrapper-col={formConfig.wrapperCol}
              label="配合作业车间"
            >
              <SmSystemUserSelect
                axios={this.axios}
                placeholder="请选择配合作业单位"
                height={65}
                showOrganizationTab={true}
                showAllOrganization={true}
                disabled={this.disabled}
                value={this.workTicketRltCooperationUnits}
                onChange={value => {
                  this.workTicketRltCooperationUnits = value;
                }}
              />
            </a-form-item>

            <a-form-item
              label-col={formConfig.labelCol}
              wrapper-col={formConfig.wrapperCol}
              label="配合作业内容"
            >
              <a-input
                type="textarea"
                value={this.cooperateContent}
                onChange={value => (this.cooperateContent = value.target.value)}
                disabled={this.disabled}
              ></a-input>
            </a-form-item>
            <a-form-item
              label="安全技术措施及注意事项"
              // label-col={formConfig.labelCol}
              // wrapper-col={formConfig.wrapperCol}
            >
              <a-input
                disabled={this.disabled}
                type="textarea"
                placeholder="请输入安全技术措施及注意事项"
                v-decorator={[
                  'securityMeasuresAndAttentions',
                  {
                    initialValue: null,
                    rules: [
                      {
                        required: true,
                        message: '请输入安全技术措施及注意事项',
                        whitespace: true,
                      },
                    ],
                  },
                ]}
              ></a-input>
            </a-form-item>
          </a-form>
        </a-modal>
      </div>
    );
  },
};