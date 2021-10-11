import { form as formConfig } from '../../_utils/config';
import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import ApiProblem from '../../sm-api/sm-problem/Problem';
import SmProblemProblemCategoryTreeSelect from '../sm-problem-problem-category-tree-select';
import SmFileTextEditor from '../../sm-file/sm-file-text-editor';

let apiProblem = new ApiProblem();

// 定义表单字段常量
const formFields = ['problemRltProblemCategories', 'name', 'order'];
export default {
  name: 'SmProblemProblemModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象,
      confirmLoading: false, //确定按钮加载状态
      fileServerEndPoint: '', //文件服务请求头
      content: null,//问题详情
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
    this.fileServerEndPoint = localStorage.getItem('fileServerEndPoint');
  },
  methods: {
    initAxios() {
      apiProblem = new ApiProblem(this.axios);
    },

    add() {
      this.status = ModalStatus.Add;
      this.$nextTick(() => {
        this.form.resetFields();
      });
    },

    async edit(record) {
      this.status = ModalStatus.Edit;
      let response = await apiProblem.get(record.id);
      if (utils.requestIsSuccess(response) && response.data) {
        this.record = response.data;
        this.record.problemRltProblemCategories = this.record.problemRltProblemCategories.map(item => item.problemCategoryId);
        this.content = this.record.content.replace(
          new RegExp(`src="`, 'g'),
          `src="${this.fileServerEndPoint}`,
        );
      }
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
      });
    },

    // 详情
    async view(record) {
      this.status = ModalStatus.View;
      let response = await apiProblem.get(record.id);
      if (utils.requestIsSuccess(response) && response.data) {
        this.record = response.data;
        this.record.problemRltProblemCategories = this.record.problemRltProblemCategories.map(item => item.problemCategoryId);
        this.content = this.record.content.replace(
          new RegExp(`src="`, 'g'),
          `src="${this.fileServerEndPoint}`,
        );
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
      this.content = null;
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        let _content = this.$refs.SmFileTextEditor.content();
        let reg = new RegExp(`${this.fileServerEndPoint}`, 'g');
        if (!err) {
          let _values = {
            ...values,
            order: values.order === null ? 0 : values.order,
            problemRltProblemCategories: values.problemRltProblemCategories.map(item => { return { problemCategoryId: item }; }),
            content: _content.replace(reg, ''),
          };
          let response = null;
          if (this.status === ModalStatus.View) {
            this.close();
          } else if (this.status === ModalStatus.Add) {
            // 添加
            this.confirmLoading = true;
            response = await apiProblem.create(_values);

          } else if (this.status === ModalStatus.Edit) {
            // 编辑
            this.confirmLoading = true;
            response = await apiProblem.update({ ..._values, id: this.record.id });

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
        title={`${this.title}问题`}
        width={800}
        visible={this.visible}
        confirmLoading={this.confirmLoading}
        destroyOnClose={true}
        onCancel={this.close}
        onOk={this.ok}
      >
        <a-form form={this.form}>
          <a-form-item
            label="类型"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmProblemProblemCategoryTreeSelect
              disabled={this.status == ModalStatus.View}
              placeholder="请输入"
              axios={this.axios}
              multiple={true}
              v-decorator={[
                'problemRltProblemCategories',
                {
                  initialValue: [],
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
              disabled={this.status === ModalStatus.View}
              placeholder={this.status === ModalStatus.View ? '' : '请输入'}
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
              disabled={this.status === ModalStatus.View}
              placeholder={this.status === ModalStatus.View ? '' : '请输入'}
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
          <a-form-item
            label="详情"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmFileTextEditor
              ref="SmFileTextEditor"
              axios={this.axios}
              disabled={this.status === ModalStatus.View}
              multiple={true}
              placeholder={this.status === ModalStatus.View ? '' : '请输入'}
              value={this.content}
              height={300}
            />
          </a-form-item>
        </a-form>
      </a-modal>
    );
  },
};
