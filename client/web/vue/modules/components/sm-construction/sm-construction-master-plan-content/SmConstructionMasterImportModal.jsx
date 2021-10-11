
import './style';
import { form as formConfig } from '../../_utils/config';
import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';

import ApiMasterPlanContent from '../../sm-api/sm-construction/ApiMasterPlanContent';
import ApiDataDictionary from '../../sm-api/sm-system/DataDictionary';


let apiMasterPlanContent = new ApiMasterPlanContent();
let apiDataDictionary = new ApiDataDictionary();// 字典api 查 类型用


// 表单字段
const formFields = ['masterPlanId', 'name', 'content', 'planStartTime', 'planEndTime', 'period', 'parentId'];
export default {
  name: 'SmConstructionMasterImportModal', // 引用模态框
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象
      dicTypes: [],
      parentId: undefined, // 父级id
      masterPlanId: undefined, // 计划 Id
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
  },
  methods: {
    initAxios() {
      apiMasterPlanContent = new ApiMasterPlanContent(this.axios);
      apiDataDictionary = new ApiDataDictionary(this.axios);
    },

    // 关闭模态框
    close() {
      this.form.resetFields();
      this.status = ModalStatus.Hide;
    },
    // 打开模态框
    open() {
      this.status = ModalStatus.Add;
    },

    // 数据提交
    async  ok() {
      // err  是 表单不通过 的 错误  values 是表单内容{}
      let response = null;
      if (this.status === ModalStatus.Add) {
        response = await apiMasterPlanContent.create({ ...values }); // 添加任务计划详情
      } else if (this.status === ModalStatus.Edit) {
        response = await apiMasterPlanContent.update(this.record.id, { ...values }); // 编辑任务计划详情
      }
      if (utils.requestIsSuccess(response)) {
        this.$message.success('操作成功');
        this.close();
        this.$emit('success');
      }
    },
  },
  render() {
    return (
      <a-modal
        title={`施工计划详情${this.title}`}
        visible={this.visible}
        onCancel={this.close}
        destroyOnClose={true}
        onOk={this.ok}
      >
        <a-form form={this.form}>

          <a-form-item
            label='施工总体计划Id ' // 这个隐藏了
            label-col={formConfig.labelCol}
            style="display:none"
            wrapper-col={formConfig.wrapperCol}
          >
            {
              this.form.getFieldDecorator('masterPlanId', {
                initialValue: this.masterPlanId,
                rules: [],
              })(<a-input disabled={this.status === ModalStatus.View} placeholder={'请输入施工总体计划Id '} />)
            }
          </a-form-item>

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
            label='计划开始时间'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-date-picker
              placeholder={this.status === ModalStatus.View ? '' : '计划计划开始时间'}
              disabled={this.status === ModalStatus.View}
              style='width: 100%'
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
              placeholder={this.status === ModalStatus.View ? '' : '计划计划结束时间'}
              disabled={this.status === ModalStatus.View}
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
            label='工期'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}>
            {
              this.form.getFieldDecorator('period', {
                initialValue: 0,
                rules: [{ required: true, message: '请输入工期!' }],
              })(
            	<a-input-number
            	  style='width:100%'
            	  min={0}
            	  precision={2}
            	  placeholder={'工期'}
            	/>,
              )
            }
          </a-form-item>

          <a-form-item
            label='父级id' // 这个隐藏起来
            style="display:none;"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            {
              this.form.getFieldDecorator('parentId', {
                initialValue: this.parentId,
                rules: [],
              })(<a-input disabled={this.status === ModalStatus.View} placeholder={'请输入父级id'} />)
            }
          </a-form-item>

        </a-form>
      </a-modal>
    );
  },
};
