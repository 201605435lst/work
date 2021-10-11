
import './style';
import { form as formConfig } from '../../_utils/config';
import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import SmBpmWorkflowModal from '../../sm-bpm/sm-bpm-workflow-templates/SmBpmWorkflowSelectModal';
import ApiDispatch from '../../sm-api/sm-construction/ApiDispatch';

let apiDispatch = new ApiDispatch();


// 表单字段 
const formFields = ['name', 'code', 'profession', 'contractorName', 'workFlowName'];
export default {
  name: 'SmConstructionDispatchModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象
      workFlowId: null,//审批流程id
    };
  },
  computed: {
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
      apiDispatch = new ApiDispatch(this.axios);
    },

    // 提交审批
    submit(record) {
      this.record = {
        ...record,
        contractorName: record.contractor ? record.contractor.name : '',
      };
      this.status = ModalStatus.Add;
      this.$nextTick(() => {
        this.form.resetFields();
        this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
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
          let response = await apiDispatch.forSubmit({ id: this.record.id, workFlowId: this.workFlowId });
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
        class="sm-construction-dispatch-modal"
        title='派工单提交审批'
        visible={this.visible}
        onCancel={this.close}
        destroyOnClose={true}
        onOk={this.ok}
        width={600}
      >
        <a-form form={this.form}>
          <a-form-item
            label="派工单名称"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled
              v-decorator={[
                'name',
                {
                  initialValue: null,
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="派工单编号"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled
              v-decorator={[
                'code',
                {
                  initialValue: null,
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="施工专业"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled
              v-decorator={[
                'profession',
                {
                  initialValue: null,
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="承包商"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled
              v-decorator={[
                'contractorName',
                {
                  initialValue: null,
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="审批流程"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.isShow}
              onClick={() => this.$refs.flowSelect.show()}
              placeholder='请选择审批流程'
              v-decorator={[
                'workFlowName',
                {
                  initialValue: undefined,
                  rules: [
                    {
                      required: true,
                      message: '请选择审批流程',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
        </a-form>
        {/* 流程选择框 */}
        <SmBpmWorkflowModal
          ref="flowSelect"
          axios={this.axios}
          onSelected={value => {
            this.workFlowId = value ? value.id : null;
            this.$nextTick(() => {
              this.form.setFieldsValue({ workFlowName: value ? value.name : '' });
            });
          }}
        />
      </a-modal>
    );
  },
};
