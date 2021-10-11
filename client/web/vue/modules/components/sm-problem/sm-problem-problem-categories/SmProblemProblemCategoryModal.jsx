import { form as formConfig } from '../../_utils/config';
import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import ApiProblemCategory from '../../sm-api/sm-problem/ProblemCategory';
import SmProblemProblemCategoryTreeSelect from '../sm-problem-problem-category-tree-select';

let apiProblemCategory = new ApiProblemCategory();

// 定义表单字段常量
const formFields = ['parentId', 'name', 'order'];
export default {
  name: 'SmProblemProblemCategoryModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象,
      confirmLoading: false, //确定按钮加载状态
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
      apiProblemCategory = new ApiProblemCategory(this.axios);
    },

    add(parentId) {
      this.status = ModalStatus.Add;
      this.parentId = parentId; //根节点编码;
      this.$nextTick(() => {
        this.form.resetFields();
        this.form.setFieldsValue({ parentId: parentId });
      });
    },

    async edit(record) {
      this.status = ModalStatus.Edit;
      let response = await apiProblemCategory.get(record.id);
      if (utils.requestIsSuccess(response) && response.data) {
        this.record = response.data;
      }
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
      });
    },

    // 详情
    async view(record) {
      this.status = ModalStatus.View;
      let response = await apiProblemCategory.get(record.id);
      if (utils.requestIsSuccess(response) && response.data) {
        this.record = response.data;
      }
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
      });
    },

    // 关闭模态框
    close() {
      this.form.resetFields();
      this.status = ModalStatus.Hide;
      this.confirmLoading = false;
      this.record = null;
      this.parentId = null;
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let _values = {
            ...values,
            order: values.order === null ? 0 : values.order,
          };
          let response = null;
          if (this.status === ModalStatus.View) {
            this.close();
          } else if (this.status === ModalStatus.Add) {
            // 添加
            this.confirmLoading = true;
            response = await apiProblemCategory.create(_values);

          } else if (this.status === ModalStatus.Edit) {
            // 编辑
            this.confirmLoading = true;
            response = await apiProblemCategory.update({ ..._values, id: this.record.id });

          }
          if (response && utils.requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.close();
            this.$emit('success');
          }
        }
      });
      this.confirmLoading = false;
    },
  },
  render() {
    return (
      <a-modal
        title={`${this.title}问题分类`}
        visible={this.visible}
        confirmLoading={this.confirmLoading}
        onCancel={this.close}
        onOk={this.ok}
      >
        <a-form form={this.form}>
          <a-form-item
            label="父级"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmProblemProblemCategoryTreeSelect
              disabled={
                (this.status == ModalStatus.Add && this.parentId != null) ||
                this.status == ModalStatus.View
              }
              childrenIsDisabled={true}
              placeholder="请输入"
              axios={this.axios}
              disabledIds={this.record ? [this.record.id] : []}
              v-decorator={[
                'parentId',
                {
                  initialValue: null,
                },
              ]}
            />
          </a-form-item>

          <a-form-item
            label="名称"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入'}
              v-decorator={[
                'name',
                {
                  initialValue: '',
                  rules: [
                    {
                      max: 100,
                      message: '标题最多可输入100字',
                    },
                    {
                      required: true,
                      message: '请输入标题',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>

          <a-form-item
            label="排序"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number
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
                    { pattern: /^(?:0|[1-9]\d{0,8})?$/, message: '请输入正确排序' },
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
