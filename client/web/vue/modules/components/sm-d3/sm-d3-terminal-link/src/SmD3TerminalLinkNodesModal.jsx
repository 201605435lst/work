import { form as formConfig } from '../../../_utils/config';
import * as utils from '../../../_utils/utils';
import { ModalStatus } from '../../../_utils/enum';

import ApiTerminalBusiness from '../../../sm-api/sm-resource/TerminalBusiness';
import Terminal from '../../../sm-api/sm-resource/Terminal';
let apiTerminalBusiness = new ApiTerminalBusiness();

export default {
  name: 'SmD3TerminalLinkPathModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide,     // 模态框状态
      record: {},                   // 表单绑定的对象,
      form: {},                     // 表单
    };
  },
  computed: {
    visible() {
      return this.status !== ModalStatus.Hide;
    },
  },
  watch: {

  },
  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    initAxios() {
      apiTerminalBusiness = new ApiTerminalBusiness(this.axios);
    },
    add(record) {
      this.status = ModalStatus.Add;
      this.record = record;
      this.$nextTick(() => {
        this.form.resetFields();
      });
    },
    edit(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      this.$nextTick(() => {
        this.form.setFieldsValue({ name: record.name, remark: record.remark });
      });
    },
    // 关闭模态框
    close() {
      this.form.resetFields();
      this.status = ModalStatus.Hide;
      this.record = null;
    },
    // 数据提交
    ok() {
      if (this.status === ModalStatus.View) {
        this.close();
        return;
      }
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let response = null;
          let data = {
            ...values,
            nodes: this.record.nodes.map((item, index) => {
              return {
                order: index,
                terminalId: item.terminal.id,
                cableCoreId: item.cableCore ? item.cableCore.id : null,
              };
            }),
          };


          // 添加
          if (this.status === ModalStatus.Add) {
            response = await apiTerminalBusiness.create(data);
          } else if (this.status === ModalStatus.Edit) {
            // 编辑
            response = await apiTerminalBusiness.update(data);
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
    return <a-modal
      title="径路"
      visible={this.visible}
      onCancel={this.close}
      onOk={this.ok}
    >
      <a-form form={this.form}>
        <a-form-item
          label="业务名称"
          label-col={formConfig.labelCol}
          wrapper-col={formConfig.wrapperCol}
        >
          <a-input
            disabled={this.status == ModalStatus.View}
            placeholder={this.status == ModalStatus.View ? '' : '请输入业务名称'}
            v-decorator={[
              'name',
              {
                initialValue: '',
                rules: [{ required: true, message: '请输入业务名称', whitespace: true }],
              },
            ]}
          />
        </a-form-item>

        <a-form-item
          label="备注"
          label-col={formConfig.labelCol}
          wrapper-col={formConfig.wrapperCol}
        >
          <a-input
            disabled={this.status == ModalStatus.View}
            placeholder={this.status == ModalStatus.View ? '' : '请输入备注'}
            v-decorator={[
              'remark',
            ]}
          />
        </a-form-item>
      </a-form>
    </a-modal>;
  },
};