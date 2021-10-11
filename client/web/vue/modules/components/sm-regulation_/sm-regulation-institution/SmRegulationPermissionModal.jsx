import './style';
import * as utils from '../../_utils/utils';
import SmSystemMemberSelect from '../../sm-system/sm-system-member-select';
import { ModalStatus } from '../../_utils/enum';
import ApiInstitution from '../../sm-api/sm-regulation/Institution';
import { requestIsSuccess } from '../../_utils/utils';

let apiInstitution = new ApiInstitution();
// 定义表单字段常量
const formFields = ['listView', 'listEdit', 'listDownLoad'];

export default {
  name: 'SmRegulationPermissionModal',
  props: {
    axios: { type: Function, default: null },
  },

  data() {
    return {
      form: {}, // 表单
      record: null, // 表单绑的对象,
      loading: false,
      status: ModalStatus.Hide,
      flag: 0,
      selectedIds: [],
    };
  },

  computed: {
    visible() {
      return this.status !== ModalStatus.Hide;
    },
  },

  watch: {},

  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },

  methods: {
    initAxios() {
      apiInstitution = new ApiInstitution(this.axios);
    },

    editPermission(selectedRowKeys) {
      this.selectedIds = selectedRowKeys;
      this.status = ModalStatus.Add;
      this.$nextTick(() => {
        this.form.setFieldsValue();
      });
    },

    async authority(record) {
      console.log(record);
      this.status = ModalStatus.Edit;
      let response = await apiInstitution.get(record.id);
      if (requestIsSuccess(response)) this.record = response.data;
      this.record = JSON.parse(JSON.stringify(this.record));
      this.$nextTick(() => {
        let fields = { ...utils.objFilterProps(this.record, formFields) };
        this.form.setFieldsValue(fields);
      });
    },

    cancel() {
      this.status = ModalStatus.Hide;
      this.$emit('cancel');
      this.form.resetFields();
    },

    // 关闭模态框
    close() {
      this.status = ModalStatus.Hide;
      this.loading = false;
    },

    // 数据提交
    async ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let data = { ...values };
          data.flag = 1;
          let response = null;
          this.loading = true;
          if (this.status === ModalStatus.Edit) {
            let fields = { ...data, id: this.record.id };
            response = await apiInstitution.update(fields);
          } else if (this.status === ModalStatus.Add) {
            data.selectedIds = this.selectedIds;
            response = await apiInstitution.create(data);
          }
          if (utils.requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.close();
            this.$emit('success');
            this.form.resetFields();
          }
        }
      });
    },
  },

  render() {
    return (
      <a-modal
        title={'权限配置'}
        visible={this.visible}
        onCancel={this.close}
        confirmLoading={this.loading}
        destroyOnClose={true}
        forceRender={this.forceRender}
        okText="保存"
        onOk={this.ok}
        width={700}
      >
        <a-form form={this.form}>
          <a-row gutter={24}>
            <a-col sm={24} md={24}>
              <a-form-item
                label="可查看者"
                placeholder={'请选择'}
                label-col={{ span: 4 }}
                wrapper-col={{ span: 20 }}
              >
                <SmSystemMemberSelect
                  axios={this.axios}
                  height={60}
                  multiple
                  onChange={item => console.log(item)}
                  placeholder={this.status == ModalStatus.View ? '' : '请选择'}
                  disabled={this.status == ModalStatus.View}
                  enableDownload={true}
                  v-decorator={[
                    'listView',
                    {
                      initialValue: [],
                      rules: [
                        {
                          required: true,
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={24}>
              <a-form-item
                label="可编辑者"
                placeholder={'请选择'}
                label-col={{ span: 4 }}
                wrapper-col={{ span: 20 }}
              >
                <SmSystemMemberSelect
                  axios={this.axios}
                  height={60}
                  multiple
                  onChange={item => console.log(item)}
                  placeholder={this.status == ModalStatus.View ? '' : '请选择'}
                  disabled={this.status == ModalStatus.View}
                  enableDownload={true}
                  v-decorator={[
                    'listEdit',
                    {
                      initialValue: [],
                      rules: [
                        {
                          required: true,
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={24}>
              <a-form-item
                label="附件可下载"
                placeholder={'请选择'}
                label-col={{ span: 4 }}
                wrapper-col={{ span: 20 }}
              >
                <SmSystemMemberSelect
                  axios={this.axios}
                  height={60}
                  multiple
                  onChange={item => console.log(item)}
                  placeholder={this.status == ModalStatus.View ? '' : '请选择'}
                  disabled={this.status == ModalStatus.View}
                  enableDownload={true}
                  v-decorator={[
                    'listDownLoad',
                    {
                      initialValue: [],
                      rules: [
                        {
                          required: true,
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
          </a-row>
        </a-form>
      </a-modal>
    );
  },
};
