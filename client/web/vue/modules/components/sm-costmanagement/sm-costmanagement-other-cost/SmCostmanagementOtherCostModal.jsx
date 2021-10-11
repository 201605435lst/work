import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import { form as formConfig, tips } from '../../_utils/config';
import ApiCostOther from '../../sm-api/sm-costmanagement/CostOther';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';

let apiCostOther = new ApiCostOther();
import moment from 'moment';

// 定义表单字段常量
const formFields = [
  'typeId',
  'date',
  'money',
  'remark',
];
export default {
  name: 'SmCostmanagementOtherCostModal',
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
      apiCostOther = new ApiCostOther(this.axios);
    },
    add() {
      this.status = ModalStatus.Add;
      this.$nextTick(() => {
        this.form.resetFields();
        this.form.setFieldsValue();
      });
    },

    async edit(record) {
      this.status = ModalStatus.Edit;
      this.record=record;
      this.record.date = this.record && this.record.date ? moment(this.record.date) : null;
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
    disabledDate(current) {
      // Can not select days before today and today
      return current && current > moment().endOf('day');
    },
    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let data = {
            ...values,
          };
          console.log(data);
          let response = null;
          if (this.status === ModalStatus.Add) {
            //添加
            response = await apiCostOther.create(data);
          } else if (this.status === ModalStatus.Edit) {
            // 编辑
            response = await apiCostOther.update({ id: this.record.id, ...data });
          } else {
            this.close();
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
        title={`${this.title}成本`}
        visible={this.visible}
        onCancel={this.close}
        confirmLoading={this.loading}
        destroyOnClose={true}
        okText={this.status !== ModalStatus.View ? "保存" : '确定'}
        onOk={this.ok}
        width={800}
      >
        <a-form form={this.form}>
          <a-form-item
            label="费用类型"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <DataDictionaryTreeSelect
              axios={this.axios}
              groupCode={'CostmanagementCostType'}
              placeholder="请选择"
              v-decorator={[
                'typeId',
                {
                  initialValue: undefined,
                  rules: [
                    {
                      required: true,
                      message: '请选择费用类型',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="费用金额(万)"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入费用金额'}
              style="width:100%"
              min={0}
              // precision={3}
              v-decorator={[
                'money',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入费用金额',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="日期选择"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-date-picker
              disabledDate={this.disabledDate}
              placeholder={this.status == ModalStatus.View? '' : '请选择日期'}
              disabled={this.status == ModalStatus.View}
              style="width:100%"
              v-decorator={[
                'date',
                {
                  initialValue: null,
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

