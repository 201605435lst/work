import ApiDossierCategory from '../../sm-api/sm-project/DossierCategory';
import { form as formConfig, tips } from '../../_utils/config';
import { ModalStatus } from '../../_utils/enum';
import * as utils from '../../_utils/utils';
import { requestIsSuccess } from '../../_utils/utils';
import './style';

let apiDossierCategory = new ApiDossierCategory();

// 定义表单字段常量
const formFields = [
  'order',
  'name',
  'remark',
];
export default {
  name: 'SmProjectDossierCatrgotyModal',
  props: {
    value: { type: Boolean, default: null },
    axios: { type: Function, default: null },
    api: { type: Object, default: null },
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
      apiDossierCategory = new ApiDossierCategory(this.axios);
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
              parentId: this.record ? this.status === ModalStatus.Add ? this.record.id : this.record.parentId : null,
            };
            this.confirmLoading = true;
            if (this.status === ModalStatus.Add) {
              response = await apiDossierCategory.create(data);
              if (utils.requestIsSuccess(response)) {
                this.$message.success('操作成功');
                this.$emit('success', "Add", response.data);
                this.close();
              }
            } else if (this.status === ModalStatus.Edit) {
              // 编辑
              response = await apiDossierCategory.update({ id: this.record.id, ...data });
              if (utils.requestIsSuccess(response)) {
                this.$message.success('操作成功');
                this.$emit('success', "Edit", response.data);
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
    // for (let item in DossierCategoryEnable) {
    //   stateTypeOption.push(
    //     <a-radio key={DossierCategoryEnable[item]} value={DossierCategoryEnable[item]}>
    //       {getDossierCategoryEnableOption(DossierCategoryEnable[item])}
    //     </a-radio>,
    //   );
    // }
    return (
      <a-modal
        class="sm-project-dossier-model"
        title={`文件管理`}
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
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入序号',
                    },
                  ],
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
              v-decorator={
                [
                  'remark',
                  {
                    initialValue: null,
                  },
                ]
              }
            ></a-textarea>
          </a-form-item>
        </a-form>
      </a-modal>
    );
  },
};
