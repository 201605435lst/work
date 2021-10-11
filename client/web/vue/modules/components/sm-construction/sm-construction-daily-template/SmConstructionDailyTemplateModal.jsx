
import './style';
import { form as formConfig } from '../../_utils/config';
import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';

import ApiDailyTemplate from '../../sm-api/sm-construction/DailyTemplate';


let apiDailyTemplate = new ApiDailyTemplate();


// 表单字段 
const formFields = ['name', 'description', 'isDefault', 'remark'];
export default {
  name: 'SmConstructionDailyTemplateModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide,
      form: {},
      record: {},
      dicTypes: [],
      isDefault: false,
    };
  },
  computed: {
    title() {
      return utils.getModalTitle(this.status);
    },
    visible() {
      return this.status !== ModalStatus.Hide;
    },
  },
  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    initAxios() {
      apiDailyTemplate = new ApiDailyTemplate(this.axios);
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
      this.isDefault = false;
      this.form.resetFields();
      this.status = ModalStatus.Hide;
    },
    // 编辑
    edit(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      this.isDefault = record.isDefault;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(record, formFields) });
      });
    },
    // 编辑
    view(record) {
      this.status = ModalStatus.View;
      this.record = record;
      this.isDefault = record.isDefault;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(record, formFields) });
      });
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let response = null;
          if (this.status === ModalStatus.Add) {
            response = await apiDailyTemplate.create({ ...values }); // 添加日志模板
          } else if (this.status === ModalStatus.Edit) {
            response = await apiDailyTemplate.update({ ...values, id: this.record.id }); // 编辑日志模板
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
        title={`日志模板${this.title}`}
        visible={this.visible}
        onCancel={this.close}
        destroyOnClose={true}
        onOk={this.ok}
      >
        <a-form form={this.form}>

          <a-form-item
            label='模板名称'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status === ModalStatus.View}
              placeholder={'请输入模板名称'}
              v-decorator={[
                'name',
                {
                  initialValue: '',
                  rules: [
                    {
                      required: true,
                      message: '请输入模板名称！',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>

          <a-form-item
            label='模板描述'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status === ModalStatus.View}
              placeholder={'请输入模板描述'}
              v-decorator={[
                'description',
                {
                  initialValue: '',
                  rules: [
                    {
                      message: '请输入模板描述！',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>

          <a-form-item
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-checkbox
              style='margin-left: 30px;'
              checked={this.isDefault}
              disabled={this.status === ModalStatus.View}
              onChange={e => this.isDefault = e.target.checked}
              v-decorator={[
                'isDefault',
                {
                  initialValue: false,
                },
              ]}
            >
                            设为默认模板
            </a-checkbox>
          </a-form-item>

        </a-form>
      </a-modal>
    );
  },
};
