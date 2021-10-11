import ApiWorker from '../../sm-api/sm-construction-base/Worker';
import { form as formConfig } from '../../_utils/config';
import { ModalStatus } from '../../_utils/enum';
import * as utils from '../../_utils/utils';

let apiWorker = new ApiWorker();

// 定义表单字段常量
const formFields = ['name'];
export default {
  name: 'SmConstructionBaseWorkerMod',
  props: {
    axios: { type: Function, default: null },
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
    // 创建表单
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    initAxios() {
      apiWorker = new ApiWorker(this.axios);
    },

    // 添加
    add() {
      this.status = ModalStatus.Add;
      // $nextTick -> Dom异步更新
      this.$nextTick(() => {
        this.form.resetFields();
      });
    },

    // 编辑
    edit(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(record, formFields) });
      });
    },

    // 关闭模态框
    close() {
      this.form.resetFields();
      this.status = ModalStatus.Hide;
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let name = this.form.getFieldValue('name');
          // 用正则把逗号去掉
          values.name = name.replace(/[, ]/g, '');
          let response = null;
          if (this.status === ModalStatus.Add) {
            // 添加工种信息
            response = await apiWorker.create({ ...values });
          } else if (this.status === ModalStatus.Edit) {
            // 添加工种信息
            response = await apiWorker.update(this.record.id, { ...values });
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
        title={`${this.title}工种信息`}
        visible={this.visible}
        onCancel={this.close}
        destroyOnClose
        onOk={this.ok}
      >
        <a-form form={this.form}>
          <a-form-item
            label='名称'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'name',
                {
                  initialValue: '',
                  rules: [
                    { required: true, message: '请输入工种名！', whitespace: true },
                  ],
                },
              ]}
              placeholder={this.status == ModalStatus.View ? '' : '请输入工种名'}
            />
          </a-form-item>
        </a-form>
      </a-modal>
    );
  },
};
