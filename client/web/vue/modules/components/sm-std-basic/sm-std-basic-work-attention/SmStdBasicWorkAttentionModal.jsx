import ApiWorkAttention from '../../sm-api/sm-std-basic/WorkAttention';
import { form as formConfig, tips } from '../../_utils/config';
import { ModalStatus } from '../../_utils/enum';
import * as utils from '../../_utils/utils';
import { requestIsSuccess } from '../../_utils/utils';
import SmStdBasicWorkAttentionTreeSelect from '../sm-std-basic-work-attention-tree-select/SmStdBasicWorkAttentionTreeSelect';
import './style';

let apiWorkAttention = new ApiWorkAttention();

// 定义表单字段常量
const formFields = [
  'typeId',
  'content',
];
export default {
  name: 'SmStdBasicWorkAttentionModal',
  props: {
    value: { type: Boolean, default: null },
    axios: { type: Function, default: null },
    repairTagKey: { type: String, default: null }, //维修项标签
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: null, // 表单绑的对象,
      confirmLoading: false,//确定按钮加载状态
      parentId: null,
    };
  },
  computed: {
    title() {
      // 计算模态框的标题变量
      return utils.getModalTitle(this.status);
    },
    visible() {
      // 计算模态框的显示变量k
      return this.status !== ModalStatus.Hide;
    },
  },
  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});

  },

  methods: {
    initAxios() {
      apiWorkAttention = new ApiWorkAttention(this.axios);
    },


    add(record) {
      this.status = ModalStatus.Add;

    },
    //编辑
    edit(record) {
      this.record = record;
      this.status = ModalStatus.Edit;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(record, formFields) });
      });
    },

    // 关闭模态框
    close() {
      this.record = null;
      this.form.resetFields();
      this.status = ModalStatus.Hide;
    },
    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let response = null;
          let data = {
            ...values,
            repairTagKey: this.repairTagKey,
          };
          this.confirmLoading = true;
          if (this.status === ModalStatus.Add) {
            response = await apiWorkAttention.create(data);
            if (utils.requestIsSuccess(response)) {
              this.$message.success('操作成功');
              this.$emit('success');
              this.close();
            }
          } else if (this.status === ModalStatus.Edit) {
            // 编辑
            response = await apiWorkAttention.update({ id: this.record.id, ...data });
            if (utils.requestIsSuccess(response)) {
              this.$message.success('操作成功');
              this.$emit('success');
              this.close();
            }
          }

        }
      });
      this.confirmLoading = false;
    },
  },

  render() {
    return (
      <a-modal
        class="sm-std-basic-work-attention-model"
        title={`${this.title}`}
        visible={this.visible}
        onCancel={this.close}
        confirmLoading={this.confirmLoading}
        destroyOnClose={true}
        okText="保存"
        onOk={
          this.ok
        }
        width={700}
      >
        <a-form form={this.form}>
          <a-form-item
            label="类别"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmStdBasicWorkAttentionTreeSelect
              repairTagKey={this.repairTagKey}
              axios={this.axios}
              placeholder={this.status == ModalStatus.View ? '' : '请选择类别'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'typeId',
                {
                  initialValue: undefined,
                  rules: [
                    { required: true, message: '请选择类别',whitespace: true },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="注意事项"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-textarea
              rows="3"
              placeholder={this.status == ModalStatus.View ? '' : '请输入注意事项'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'content',
                {
                  initialValue: null,
                  rules: [
                    { required: true, message: '请输入注意事项',whitespace: true },
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
