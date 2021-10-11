
import './style';
import { form as formConfig } from '../../_utils/config';
import * as utils from '../../_utils/utils';
import dayjs from 'dayjs';
import { MemberType, ModalStatus } from '../../_utils/enum';
import { requestIsSuccess} from '../../_utils/utils';
import ApiPlan from '../../sm-api/sm-construction/ApiPlan';
import ApiDataDictionary from '../../sm-api/sm-system/DataDictionary';
import SmSystemMemberSelect from   '../../sm-system/sm-system-member-select';
import ApiUser from '../../sm-api/sm-system/User';
import moment from 'moment';


let apiPlan = new ApiPlan();
let apiDataDictionary = new ApiDataDictionary();// 字典api 查 类型用
let apiUser = new ApiUser();// 项目api 查类型用


// 表单字段
const formFields = ['name', 'content', 'planStartTime', 'planEndTime', 'period', 'chargerId'];
export default {
  name: 'SmConstructionPlanModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象
      projects: [], // 项目列表
      users: [], // 项目列表
      startDateMark:'', // 开始 时间标记 ,有了这个 好判断 结束时间的范围 (不能小于开始时间)
      endDateMark:'', // 结束 时间标记 ,有了这个 好判断 开始时间的范围 (不能大于开始时间)

    };
  },
  computed: {
    title() {
      return utils.getModalTitle(this.status); // 计算模态框的标题变量
    },
    visible() {
      return this.status !== ModalStatus.Hide; // 计算模态框的显示变量
    },
  },
  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});// 创建表单
    this.getUsers(); // 获取 用户 列表
  },
  methods: {
    initAxios() {
      apiPlan = new ApiPlan(this.axios);
      apiDataDictionary = new ApiDataDictionary(this.axios);
      apiUser = new ApiUser(this.axios);
    },
    // 添加
    add() {
      this.status = ModalStatus.Add;
      this.$nextTick(() => {
        this.form.resetFields();
      });
    },

    // 关闭模态框
    close() {
      this.form.resetFields();
      this.startDateMark = null;
      this.endDateMark = null;
      this.status = ModalStatus.Hide;
      this.record={};
    },
    // 编辑
    edit(record) {
      this.status = ModalStatus.Edit;
      this.record = {...record };
      this.record.chargerId=[{ type: MemberType.User, id:this.record.chargerId }];
      this.record.planStartTime = !!this.record && !!this.record.planStartTime ? moment(this.record.planStartTime ) :null;
      this.startDateMark=this.record.planStartTime;
      this.record.planEndTime = !!this.record && !!this.record.planEndTime ? moment(this.record.planEndTime ) :null;
      this.endDateMark=this.record.planEndTime;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
      });
    },
   
    // 获取 用户 列表
    async getUsers() {
      let res = await apiUser.getList({maxResultCount:999});
      if (requestIsSuccess(res) && res.data) {
        this.users = res.data.items;
      }
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          // err  是 表单不通过 的 错误  values 是表单内容{}
          if (values.chargerId.length > 1) return  this.$message.error('只能选择一个负责人');
          values.chargerId = values.chargerId[0].id;
          values.projectTagId = localStorage.getItem('ProjectId');
          values.organizationRootTagId = localStorage.getItem('OrganizationTagId');
          let response = null;
          if (this.status === ModalStatus.Add) {
            // 添加设备台班信息
            response = await apiPlan.create({ ...values });
          } else if (this.status === ModalStatus.Edit) {
            // 添加设备台班信息
            response = await apiPlan.update(this.record.id, { ...values });
          }
          if (utils.requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.close();
            this.$emit('success');
          }
        }
      });
    },
  },
  render() {
    return (
      <a-modal
        title={`施工计划${this.title}`}
        visible={this.visible}
        onCancel={this.close}
        width={700}
        destroyOnClose
        onOk={this.ok}
      >
        <a-form form={this.form}>
          <a-form-item
            label='计划名称'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            {
              this.form.getFieldDecorator('name', {
                initialValue: '',
                rules: [{ required: true, message: '请输入计划名称！', whitespace: true }],
              })(<a-input disabled={this.status === ModalStatus.View} placeholder={'请输入计划名称'} />)
            }
          </a-form-item>

          <a-form-item
            label='计划描述'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            {
              this.form.getFieldDecorator('content', {
                initialValue: '',
                rules: [],
              })(<a-textarea disabled={this.status === ModalStatus.View} placeholder={'请输入计划描述'} />)
            }
          </a-form-item>


          <a-form-item
            label="计划开始时间"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-date-picker
              placeholder={this.status === ModalStatus.View ? '' : '计划开始时间'}
              disabled={this.status === ModalStatus.View}
              disabledDate={current => current && current > moment(this.endDateMark).subtract(1, 'days')}
              onChange={(date,dateStr )=>{
                // 修改时间后把 开始 时间标记 设置下~
                this.startDateMark = date;
                if (!!this.endDateMark) {
                  let duration = dayjs(this.endDateMark).diff(dayjs(this.startDateMark), 'day');
                  this.form.setFieldsValue({period:duration});
                }
              }}
              style="width: 100%"
              v-decorator={[
                'planStartTime',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请选择计划开始时间',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label='计划结束时间'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-date-picker
              placeholder={this.status === ModalStatus.View ? '' : '计划结束时间'}
              disabled={this.status === ModalStatus.View}
              disabledDate={current => current && current < moment(this.startDateMark).subtract(-1, 'days')}
              onChange={(date,dateStr )=>{
                // 修改时间后把 开始 时间标记 设置下~
                this.endDateMark = date;
                if (!!this.startDateMark) {
                  let duration = dayjs(this.endDateMark).diff(dayjs(this.startDateMark), 'day');
                  this.form.setFieldsValue({period:duration+1});
                }
              }}
              style='width: 100%'
              v-decorator={[
                'planEndTime',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请选择计划结束时间',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label='计划工期'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}>
            {
              this.form.getFieldDecorator('period', {
                initialValue: 0,
                rules: [{ required: true, message: '请输入计划工期!' }],
              })(
                <a-input-number
                  style='width:100%'
                  min={0}
                  disabled={true}
                  precision={2}
                  placeholder={'请输入计划工期'}
                />,
              )
            }
          </a-form-item>


          <a-form-item
            label='负责人'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}>
            {
              this.form.getFieldDecorator('chargerId', {
                initialValue: undefined,
                rules: [{ required: true, message: '请选择负责人 ' }],
              })(
                <SmSystemMemberSelect
                  height={30}
                  shouIconSelect={ true }
                  axios={this.axios }
                  bordered={ true }
                  userMultiple={ false }
                  showUserTab={ true }
                />
                ,
              )
            }

          </a-form-item>

        </a-form>
      </a-modal>
    );
  },
};
