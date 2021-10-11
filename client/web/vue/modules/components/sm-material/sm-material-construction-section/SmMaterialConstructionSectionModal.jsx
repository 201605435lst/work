import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import { form as formConfig, tips } from '../../_utils/config';
import ApiConstructionSection from '../../sm-api/sm-material/ConstructionSection';
let apiConstructionSection = new ApiConstructionSection();
import moment from 'moment';

// 定义表单字段常量
const formFields = [
  'name',
  'startSegment',
  'endSegment',
  'remark',
];
export default {
  name: 'SmMaterialConstructionSectionModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象,
      loading: false, //确定按钮加载状态
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
      apiConstructionSection = new ApiConstructionSection(this.axios);
    },
    add() {
      this.status = ModalStatus.Add;
      this.$nextTick(() => {
        this.form.resetFields();
      });
    },
    async view(record) {
      this.status = ModalStatus.View;
      this.record = record;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
      });
    },
    async edit(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
      });
    },
    // 关闭模态框
    close() {
      this.form.resetFields();
      this.record = null;
      this.status = ModalStatus.Hide;
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if(!err){
          let data = {
            ...values,
          };
          let response = null;
          if (this.status === ModalStatus.Add) {
          //添加
            response = await apiConstructionSection.create(data);
          } 
          if (this.status === ModalStatus.Edit) {
          // 编辑
            response = await apiConstructionSection.update({ id: this.record.id, ...data });
          } 
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
        title={`${this.title}施工区段`}
        visible={this.visible}
        onCancel={this.close}
        confirmLoading={this.loading}
        destroyOnClose={true}
        okText={this.status !== ModalStatus.View ? '保存' : '确定'}
        onOk={this.ok}
        width={800}
      >
        <a-form form={this.form}>
          <a-form-item
            label="名称"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入名称'}
              style="width:100%"
              v-decorator={[
                'name',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入名称',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="起始锚段"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入起始锚段'}
              style="width:100%"
              v-decorator={[
                'startSegment',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入起始锚段',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="终止锚段"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入终止锚段'}
              style="width:100%"
              v-decorator={[
                'endSegment',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入终止锚段',
                      whitespace: true,
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
              rows="3"
              placeholder={this.status == ModalStatus.View ? '' : '请输入'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'remark',
                {
                  initialValue: null,
                },
              ]}
            />
          </a-form-item>
        </a-form>
      </a-modal>
    );
  },
};
