import { form as formConfig, tips } from '../../_utils/config';
import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import DataDictionary from '../../sm-system/sm-system-data-dictionary-tree-select';
import ApiQualityProblemLibrary from '../../sm-api/sm-quality/QualityProblemLibrary';
import DataEnum from './src/SmSystemDataEnumTreeSelect';

let apiQualityProblemLibrary = new ApiQualityProblemLibrary();

// 定义表单字段常量
const formFields = ['title', 'level', 'professionId', 'type', 'content', 'measures', 'scopIds'];
export default {
  name: 'SmQualityProblemLibraryModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象,
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
      apiQualityProblemLibrary = new ApiQualityProblemLibrary(this.axios);
    },

    //添加
    add() {
      this.status = ModalStatus.Add;
      this.$nextTick(() => {
        this.form.resetFields();
      });
    },

    //编辑
    edit(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      this.record.scopIds = this.record.scops && this.record.scops.length > 0 ? this.record.scops.map(item => item.scopId) : [];
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
      });
    },

    // 详情
    view(record) {
      console.log(record);
      this.status = ModalStatus.View;
      this.record = record;
      this.record.scopIds = this.record.scops && this.record.scops.length > 0 ? this.record.scops.map(item => item.scopId) : [];
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
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let response = null;
          if (this.status === ModalStatus.View) {
            this.close();
          } else if (this.status === ModalStatus.Add) {
            // 添加
            let data = {
              ...values,
              scops: values.scopIds.length > 0 ? values.scopIds.map(id => {
                return { scopId: id };
              }) : [],
            };
            this.confirmLoading = true;
            response = await apiQualityProblemLibrary.create(data);
            if (utils.requestIsSuccess(response)) {
              this.$message.success('操作成功');
              this.close();
              this.$emit('success');
            }
          } else if (this.status === ModalStatus.Edit) {
            // 编辑
            let data = {
              ...values,
              id: this.record.id,
              scops: values.scopIds.length > 0 ? values.scopIds.map(id => {
                return { scopId: id };
              }) : [],
            };

            this.confirmLoading = true;
            response = await apiQualityProblemLibrary.update(data);
            if (utils.requestIsSuccess(response)) {
              this.$message.success('操作成功');
              this.close();
              this.$emit('success');
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
        title={`${this.title}质量问题库`}
        visible={this.visible}
        okText={this.status !== ModalStatus.View ? "保存" : '确定'}
        confirmLoading={this.confirmLoading}
        onCancel={this.close}
        onOk={this.ok}
      >
        <a-form form={this.form}>
          <a-form-item
            label="工作内容"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-textarea
              rows="2"
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入工作内容'}
              v-decorator={[
                'title',
                {
                  initialValue: '',
                  rules: [
                    {
                      required: true,
                      message: '请输入工作内容',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>

          <a-form-item
            label="问题等级"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <DataEnum
              disabled={this.status == ModalStatus.View}
              placeholder="请选择问题等级"
              enum="QualityProblemLevel"
              utils="getQualityProblemLevel"
              v-decorator={[
                'level',
                {
                  initialValue: undefined,
                  rules: [
                    {
                      required: true,
                      message: '请选择问题等级',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>

          <a-form-item
            label="所属专业"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <DataDictionary
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? null : "请选择所属专业"}
              axios={this.axios}
              groupCode="Profession"
              v-decorator={[
                'professionId',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请选择所属专业',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>

          <a-form-item
            label="问题类型"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <DataEnum
              disabled={this.status == ModalStatus.View}
              placeholder="请选择问题类型"
              enum="QualityProblemType"
              utils="getQualityProblemType"
              v-decorator={[
                'type',
                {
                  initialValue: undefined,
                  rules: [
                    {
                      required: true,
                      message: '请选择问题类型',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>

          <a-form-item
            label="问题描述"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-textarea
              rows="3"
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入问题描述'}
              v-decorator={[
                'content',
                {
                  initialValue: '',
                  rules: [
                    {
                      required: true,
                      message: '请输入问题描述',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>

          <a-form-item
            label="整改措施"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-textarea
              rows="3"
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入整改措施'}
              v-decorator={[
                'measures',
                {
                  initialValue: '',
                  rules: [{ max: 1000, message: '最多输入 1000 字符' }],
                },
              ]}
            />
          </a-form-item>

          <a-form-item
            label="适用范围"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <DataDictionary
              disabled={this.status == ModalStatus.View}
              placeholder="请选择适用范围"
              axios={this.axios}
              multiple={true}
              treeCheckStrictly={true}
              groupCode="QualityManager.Scop"
              v-decorator={[
                'scopIds',
                {
                  initialValue: [],
                },
              ]}
            />
          </a-form-item>
        </a-form>
      </a-modal>
    );
  },
};
