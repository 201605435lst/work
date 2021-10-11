import './style';
import { form as formConfig } from '../../_utils/config';
import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import ApiSubItem from '../../sm-api/sm-construction-base/ApiSubItem';

let apiSubItem = new ApiSubItem();

const formFields = ['name'];


export default {
  name: 'SmConstructionBaseSubItemModal',
  props: {
    axios      : { type: Function, default: null },
    projectList: { type: Array   , default: () => [] },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form  : {},               // 表单
      record: {},               // 表单绑定的对象
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
    // 创建表单
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    initAxios() {
      apiSubItem = new ApiSubItem(this.axios);
    },

    // 添加
    add() {
      this.status = ModalStatus.Add;
      // $nextTick -> Dom异步更新
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
            response = await apiSubItem.create({ ...values });
          } else if (this.status === ModalStatus.Edit) {
            // 添加设备台班信息
            response = await apiSubItem.update(this.record.id, { ...values });
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
        title={`工程量清单${this.title}`}
        visible={this.visible}
        onCancel={this.close}
        destroyOnClose
        onOk={this.ok}
      >
        <a-form form={this.form}>
          <a-form-item label='工程名称' label-col={formConfig.labelCol} wrapper-col={formConfig.wrapperCol}>
            <a-input placeholder={'请输入工程名称'} disabled={this.status === ModalStatus.View} v-decorator={[
              'name',
              {
                initialValue: '',
                rules: [
                  { required: true, message: '请输入工程名称！', whitespace: true },
                ],
              },
            ]}
            />
          </a-form-item>
        </a-form>
      </a-modal>
    );
  },
};
