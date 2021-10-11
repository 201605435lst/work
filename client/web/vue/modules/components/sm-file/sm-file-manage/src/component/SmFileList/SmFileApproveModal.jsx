import { form as formConfig } from '../../../../../_utils/config';
import { ModalStatus,ApprovalStatus } from '../../../../../_utils/enum';
import ApiFileApprove from '../../../../../sm-api/sm-file-approve/FileApprove';
import { requestIsSuccess } from '../../../../../_utils/utils';
let apiFileApprove = new ApiFileApprove();

// 定义表单字段常量
const formFields = [
  'content',
];
export default {
  name: 'SmFileApproveModal',
  props: {
    value: { type: Boolean, default: null },
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: null, // 表单绑的对象,
      confirmLoading: false,//确定按钮加载状态
    };
  },


  computed: {
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
      apiFileApprove = new ApiFileApprove(this.axios);
    },
    approve(record) {
      this.record = record;
      this.status = ModalStatus.Add;
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
          let data = {
            ...values,
            fileId: this.record ? this.record.id : '',
          };
          this.confirmLoading = true;
          response = await apiFileApprove.create(data);
          if (requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.$emit('success');
            this.close();
          }
          this.confirmLoading = false;
        }
      });
    },
    // 驳回审批
    async rejected() {
      let _this = this;
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let data = {
            ...values,
            fileId: _this.record ? _this.record.id : '',
            status: ApprovalStatus.UnPass,
          };
          this.$confirm({
            title: "温馨提示",
            content: h => <div style="color:red;">确定要驳回此审批吗？</div>,
            okType: 'danger',
            onOk() {
              return new Promise(async (resolve, reject) => {
                let response = await apiFileApprove.process(data);
                if (requestIsSuccess(response)) {
                  _this.$message.success("操作成功");
                  _this.$emit("success");
                  _this.close();
                  setTimeout(resolve, 100);
                } else {
                  setTimeout(reject, 100);
                }
              });
            },
            onCancel() { },
          });
        }
      });
    },
    // 通过审批
    async approved() {
      let _this = this;
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let data = {
            ...values,
            fileId: _this.record ? _this.record.id : '',
            status: ApprovalStatus.Pass,
          };
          let response = await apiFileApprove.process(data);
          if (requestIsSuccess(response)) {
            if (response.data) {
              _this.$message.success("操作成功");
              _this.$emit("success");
              _this.close();
            }
          }
        }
      });
    },
  },
  render() {
    return (
      <a-modal
        class="sm-file-approve-modal"
        title={`文件审批`}
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
            label="审批意见"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-textarea
              placeholder="请输入审批意见"
              auto-size={{ minRows: 2, maxRows: 6 }}
              v-decorator={[
                'content',
                {
                  initialValue: '',
                  rules: [
                    {
                      required: true,
                      message: '请输入审批意见',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
        </a-form>
        <template slot="footer">
          <div>
            <a-button size="small" key="back" onClick={this.close}>取消</a-button>
            <a-button size="small" key="rejected" type="danger" onClick={this.rejected}>驳回</a-button>
            <a-button size="small" key="approved" type="primary" onClick={this.approved}>通过</a-button>
          </div>

        </template>
      </a-modal>
    );
  },
};
