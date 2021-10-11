
import './style';
import { form as formConfig } from '../../_utils/config';
import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import { requestIsSuccess} from '../../_utils/utils';

import ApiStandard from '../../sm-api/sm-construction-base/ApiStandard';
import ApiDataDictionary from '../../sm-api/sm-system/DataDictionary';


let apiStandard = new ApiStandard();
let apiDataDictionary = new ApiDataDictionary();// 字典api 查 类型用


// 表单字段
const formFields = ['name', 'code', 'professionId'];
export default {
  name: 'SmConstructionBaseStandardModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象
      dicTypes: [],

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
    this.getDicTypes(); // 获取字典类型列表
  },
  methods: {
    initAxios() {
      apiStandard = new ApiStandard(this.axios);
      apiDataDictionary = new ApiDataDictionary(this.axios);
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
    // 获取 工程类型 列表
    async getDicTypes() {
      let res = await apiDataDictionary.getValues({ groupCode: 'Profession.' });
      if (requestIsSuccess(res) && res.data) {
        this.dicTypes = res.data;
      }
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          // err  是 表单不通过 的 错误  values 是表单内容{}
          // console.log(values);
          let response = null;
          if (this.status === ModalStatus.Add) {
            // 添加设备台班信息
            response = await apiStandard.create({ ...values });
          } else if (this.status === ModalStatus.Edit) {
            // 添加设备台班信息
            response = await apiStandard.update(this.record.id, { ...values });
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
        title={`工序规范维护${this.title}`}
        visible={this.visible}
        onCancel={this.close}
        destroyOnClose
        onOk={this.ok}
      >
        <a-form form={this.form}>

          <a-form-item
            label='规范名称'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            {
              this.form.getFieldDecorator('name', {
                initialValue: '',
                rules: [{ required: true, message: '请输入规范名称！', whitespace: true }],
              })(<a-input disabled={this.status === ModalStatus.View} placeholder={'请输入规范名称'} />)
            }
          </a-form-item>

          <a-form-item
            label='规范编号'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            {
              this.form.getFieldDecorator('code', {
                initialValue: '',
                rules: [{ required: true, message: '请输入规范编号！', whitespace: true }],
              })(<a-input disabled={this.status === ModalStatus.View} placeholder={'请输入规范编号'} />)
            }
          </a-form-item>

          <a-form-item
            label='所属专业'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}>
            {
              this.form.getFieldDecorator('professionId', {
            	initialValue: undefined,
            	rules: [{ required: true, message: '请选择所属专业' }],
              })(
            	<a-select placeholder={'请选择所属专业'}>
            	  {this.dicTypes.map(x => (<a-select-option value={x.id}>{x.name}</a-select-option>))}
            	</a-select>,
              )
            }

          </a-form-item>


        </a-form>
      </a-modal>
    );
  },
};
