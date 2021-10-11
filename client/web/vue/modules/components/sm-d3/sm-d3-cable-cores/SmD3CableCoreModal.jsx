import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import ApiCableLocation from '../../sm-api/sm-resource/CableLocation';

let apiCableLocation = new ApiCableLocation();

export default {
  name: 'SmD3CableCoreModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      record: {}, // 表单绑定的对象,
      loading: false, // 数据保存加载状态
      form: {}, // 表单
    };
  },
  computed: {
    visible() {
      // 计算模态框的显示变量
      return this.status !== ModalStatus.Hide;
    },
  },
  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    initAxios() {
      apiCableLocation = new ApiCableLocation(this.axios);
    },

    // 编辑
    edit(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      this.$nextTick(() => {
        this.form.setFieldsValue({ value: record.value });
      });
    },

    // 关闭模态框
    close() {
      this.form.resetFields();
      this.loading = false;
      this.status = ModalStatus.Hide;
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let response = await apiCableLocation.update({
            ...this.record,
            value: values.value ? values.value : 0,
          });
          if (utils.requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.close();
            this.$emit('success');
          }
        }
      });
      this.loading = false;
    },
  },
  render() {
    return (
      <a-modal
        title="参考值"
        visible={this.visible}
        confirmLoading={this.loading}
        onCancel={this.close}
        onOk={this.ok}
      >
        <a-form form={this.form}>
          <a-form-item label-col={{ span: 6 }} wrapper-col={{ span: 18, offset: 3 }}>
            <a-input
              axios={this.axios}
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入电缆埋深参考值'}
              addonAfter='单位（m）'
              maxLength={15}
              v-decorator={[
                'value',
                {
                  initialValue: null,
                  rules: [
                    { required: true, message: '请输入电缆埋深参考值', whitespace: true },
                    { pattern: /^\d+(?=\.{0,1}\d+$|$)/, message: '请输入正确电缆埋深参考值' },
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
