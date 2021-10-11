
import './style';
import { form as formConfig } from '../../_utils/config';
import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import { requestIsSuccess, vIf } from '../../_utils/utils';

import ApiDispatchTemplate from '../../sm-api/sm-construction/ApiDispatchTemplate';


let apiDispatchTemplate = new ApiDispatchTemplate();


// 表单字段 
const formFields = ['name', 'description', 'isDefault', 'remark'];
export default {
  name: 'SmConstructionDispatchTemplateModal',
  props: {
    axios: { type: Function, default: null },
    nameList: { type: Array, default: () => [] },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象
      dicTypes: [],
      isDefault: false,//默认不选中
      editName: '',//编辑时模板名称名称
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
  },
  methods: {
    initAxios() {
      apiDispatchTemplate = new ApiDispatchTemplate(this.axios);
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
      this.editName = record.name;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(record, formFields) });
      });
    },
    // 查看
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
      if (this.status === ModalStatus.View) {
        this.close();
      } else {
        this.form.validateFields(async (err, values) => {
          if (!err) {
            // err  是 表单不通过 的 错误  values 是表单内容{}
            let isRepeat = false;
            if (this.nameList.find(n => n == values.name)) {
              isRepeat = true;
            }
            if (this.status === ModalStatus.Edit && this.editName != '' && this.editName == values.name) {
              isRepeat = false;
            };
            isRepeat ? this.$message.warning('模板名称不能重复') : '';
            if (!isRepeat) {
              let response = null;
              if (this.status === ModalStatus.Add) {
                if (values.isDefault == '') { values.isDefault = false; }
                response = await apiDispatchTemplate.create({ ...values }); // 添加派工单模板
              } else if (this.status === ModalStatus.Edit) {
                response = await apiDispatchTemplate.update(this.record.id, { ...values }); // 编辑派工单模板
              }
              if (utils.requestIsSuccess(response)) {
                this.$message.success('操作成功');
                this.close();
                this.$emit('success');
              }
            }
          }
        });
      }

    },
  },
  render() {
    return (
      <a-modal
        title={`派工单模板${this.title}`}
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
                  initialValue: '',
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
