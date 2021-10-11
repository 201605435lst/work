import './style';
import { form as formConfig } from '../../_utils/config';
import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import ApiProcedure from '../../sm-api/sm-construction-base/ApiProcedure';

let apiProcedure = new ApiProcedure();

const formFields = ['name', 'description', 'typeId'];


export default {
  name: 'SmConstructionBaseProcedureModal',
  props: {
    axios: { type: Function, default: null },
    // 工序类型列表
    procedureTypes: { type: Array, default: () => [] },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象
    };
  },
  computed: {
    title() {
      // 计算模态框的标题变量
      return utils.getModalTitle(this.status);
    },
    visible() {
      // 计算模态框的显示变量
      return this.status !== ModalStatus.Hide;
    },
  },
  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});// 创建表单
  },
  methods: {
    initAxios() {
      apiProcedure = new ApiProcedure(this.axios);
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
      this.status = ModalStatus.Hide;
    },
    // 编辑
    edit(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(record, formFields) });
      });
    },
    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          // err  是 表单不通过 的 错误  values 是表单内容{}
          console.log(values);
          let response = null;
          if (this.status === ModalStatus.Add) {
            // 添加设备台班信息
            response = await apiProcedure.create({ ...values });
          } else if (this.status === ModalStatus.Edit) {
            // 添加设备台班信息
            response = await apiProcedure.update(this.record.id, { ...values });
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
        title={`工序${this.title}`}
        visible={this.visible}
        onCancel={this.close}
        destroyOnClose
        onOk={this.ok}
      >
        <a-form form={this.form}>
          <a-form-item
            label='工程类型'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}>
            {
              this.form.getFieldDecorator('typeId', {
                initialValue: undefined,
                rules: [{ required: true, message: '请选择工程类型' }],
              })(
                <a-select placeholder={'请选择工程类型'}>
                  {this.procedureTypes.map(x => (<a-select-option value={x.id}>{x.name}</a-select-option>))}
                </a-select>,
              )
            }

          </a-form-item>
          <a-form-item
            label='工序名称'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            {
              this.form.getFieldDecorator('name', {
                initialValue: '',
                rules: [{ required: true, message: '请输入工序名称！', whitespace: true }],
              })(<a-input disabled={this.status === ModalStatus.View} placeholder={'请输入工序名称'} />)
            }


          </a-form-item>
          <a-form-item
            label='工序说明'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            {
              this.form.getFieldDecorator('description', {
                initialValue: '',
              })(<a-input disabled={this.status === ModalStatus.View} placeholder={'请输入工序说明'} />)
            }

          </a-form-item>


        </a-form>
      </a-modal>
    );
  },
};
