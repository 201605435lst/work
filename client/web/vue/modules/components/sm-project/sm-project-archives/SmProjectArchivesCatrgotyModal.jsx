import ApiArchivesCategory from '../../sm-api/sm-project/ArchivesCategory';
import { form as formConfig, tips } from '../../_utils/config';
import { ModalStatus } from '../../_utils/enum';
import * as utils from '../../_utils/utils';
import { requestIsSuccess } from '../../_utils/utils';
import './style';

let apiArchivesCategory = new ApiArchivesCategory();

// 定义表单字段常量
const formFields = ['order', 'name', 'remark', 'isEncrypt'];
export default {
  name: 'SmProjectArchivesCatrgotyModal',
  props: {
    value: { type: Boolean, default: null },
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: null, // 表单绑的对象,
      confirmLoading: false, //确定按钮加载状态
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
      apiArchivesCategory = new ApiArchivesCategory(this.axios);
    },
    add(record) {
      this.status = ModalStatus.Add;
      this.record = record;
    },
    //编辑
    edit(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(record, formFields) });
      });
    },

    // 详情
    view(record) {
      this.status = ModalStatus.View;
      this.record = record;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(record, formFields) });
      });
    },
    // 关闭模态框
    close() {
      this.record = null;
      this.form.resetFields();
      this.status = ModalStatus.Hide;
      this.confirmLoading = false;
    },
    // 数据提交
    ok() {
      if (this.status == ModalStatus.View) {
        this.close();
      } else {
        this.form.validateFields(async (err, values) => {
          if (!err) {
            let response = null;
            let data = {
              ...values,
              parentId: this.record
                ? this.status === ModalStatus.Add
                  ? this.record.id
                  : this.record.parentId
                : null,
              order: values.order ? values.order : 0,
            };
            console.log(data);
            this.confirmLoading = true;
            if (this.status === ModalStatus.Add) {
              response = await apiArchivesCategory.create(data);
              if (utils.requestIsSuccess(response)) {
                this.$message.success('操作成功');
                this.$emit('success', 'Add', response.data);
                this.close();
              }
            } else if (this.status === ModalStatus.Edit) {
              // 编辑
              response = await apiArchivesCategory.update({ id: this.record.id, ...data });
              if (utils.requestIsSuccess(response)) {
                this.$message.success('操作成功');
                this.$emit('success', 'Edit', response.data);
                this.close();
              }
            }
          }
        });
        this.confirmLoading = false;
      }
    },
  },
  render() {
    // //仓库状态
    // let stateTypeOption = [];
    // for (let item in ArchivesCategoryEnable) {
    //   stateTypeOption.push(
    //     <a-radio key={ArchivesCategoryEnable[item]} value={ArchivesCategoryEnable[item]}>
    //       {getArchivesCategoryEnableOption(ArchivesCategoryEnable[item])}
    //     </a-radio>,
    //   );
    // }
    return (
      <a-modal
        class="sm-project-Archives-model"
        title={`分类管理`}
        visible={this.visible}
        onCancel={this.close}
        confirmLoading={this.confirmLoading}
        destroyOnClose={true}
        okText="保存"
        onOk={this.ok}
        width={700}
      >
        <a-form form={this.form}>
          <a-form-item
            label="分类名称"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              allowClear
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入'}
              v-decorator={[
                'name',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入分类名称',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="排序号"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number
              allowClear
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入'}
              style="width:100%"
              min={0}
              precision={0}
              v-decorator={[
                'order',
                {
                  initialValue: '',
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="备注"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-textarea
              allowClear
              disabled={this.status == ModalStatus.View}
              placeholder={this.status === ModalStatus.View ? '' : '请输入'}
              v-decorator={[
                'remark',
                {
                  initialValue: null,
                },
              ]}
            ></a-textarea>
          </a-form-item>
          <a-form-item
            label="是否加密"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-radio-group
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'isEncrypt',
                {
                  initialValue: true,
                },
              ]}
            >
              <a-radio value={true}>是</a-radio>
              <a-radio value={false}>否</a-radio>
            </a-radio-group>
          </a-form-item>
        </a-form>
      </a-modal>
    );
  },
};
