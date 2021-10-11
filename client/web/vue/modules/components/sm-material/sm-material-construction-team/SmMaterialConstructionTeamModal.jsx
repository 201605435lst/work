import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import { form as formConfig, tips } from '../../_utils/config';
import ApiConstructionTeam from '../../sm-api/sm-material/ConstructionTeam';
let apiConstructionTeam = new ApiConstructionTeam();
import SmMaterialConstructionSectionSelect from '../sm-material-construction-section-select/SmMaterialConstructionSectionSelect';
import moment from 'moment';

// 定义表单字段常量
const formFields = [
  'name',
  'constructionSectionId',
  'peopleName',
  'phone',
];
export default {
  name: 'SmMaterialConstructionTeamModal',
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
      apiConstructionTeam = new ApiConstructionTeam(this.axios);
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
      if (this.status === ModalStatus.View) {
        this.close();
        return ;
      }
      this.form.validateFields(async (err, values) => {
        if(!err){
          let data = {
            ...values,
          };
          let response = null;
          if (this.status === ModalStatus.Add) {
          //添加
            response = await apiConstructionTeam.create(data);
          } 
          if (this.status === ModalStatus.Edit) {
          // 编辑
            response = await apiConstructionTeam.update({ id: this.record.id, ...data });
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
        title={`${this.title}施工队`}
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
            label="施工地点"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmMaterialConstructionSectionSelect
              disabled={this.status == ModalStatus.View}
              axios={this.axios}
              placeholder={this.status == ModalStatus.View ? '' : '请选择施工地点'}
              style="width:100%"
              v-decorator={[
                'constructionSectionId',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请选择施工地点',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="联系人"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入联系人'}
              style="width:100%"
              v-decorator={[
                'peopleName',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入联系人',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="联系电话"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入联系电话'}
              style="width:100%"
              v-decorator={[
                'phone',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入联系电话',
                      whitespace: true,
                    },
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
