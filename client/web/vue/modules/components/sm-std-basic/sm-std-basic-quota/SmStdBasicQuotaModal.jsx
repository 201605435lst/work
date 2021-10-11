import { ModalStatus, QuotaType } from '../../_utils/enum';
import { form as formConfig } from '../../_utils/config';
import ApiQuota from '../../sm-api/sm-std-basic/Quota';
import SmStdBasicQuotaCategoryTreeSelect from '../sm-std-basic-quota-category-tree-select';
import * as utils from '../../_utils/utils';
let apiQuota = new ApiQuota();
const formFields = [
  'quotaCategoryId',
  'name',
  'code',
  'unit',
  'laborCost',
  'materialCost',
  'machineCost',
  'weight',
  'remark',
];
export default {
  name: 'SmStdBasicQuotaModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide,
      form: {},
      record: null,
      computerCodeId: null,
    };
  },

  computed: {
    title() {
      return utils.getModalTitle(this.status);
    },
    visible() {
      return this.status !== ModalStatus.Hide;
    },
  },

  created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },

  methods: {
    initAxios() {
      apiQuota = new ApiQuota(this.axios);
    },

    add() {
      this.status = ModalStatus.Add;
      this.$nextTick(() => {
        this.form.setFieldsValue();
      });
    },

    detail(record) {
      this.status = ModalStatus.View;
      this.record = record;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(record, formFields) });
      });
    },

    edit(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(record, formFields) });
      });
    },

    close() {
      this.status = ModalStatus.Hide;
      this.form.resetFields();
    },

    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let data = {
            ...values,
          };
          let response = null;
          if (this.status == ModalStatus.View) {
            this.close();
          } else if (this.status == ModalStatus.Add) {
            response = await apiQuota.create(data);
          } else if (this.status === ModalStatus.Edit) {
            data = {
              ...data,
              id: this.record ? this.record.id : '',
            };
            response = await apiQuota.update(data);
          }
          if (utils.requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.close();
            this.form.resetFields();
            this.$emit('success');
          }
        }
      });
    },
  },

  render() {
    return (
      <a-modal
        visible={this.visible}
        title={`${this.title}定额`}
        onCancel={this.close}
        onOk={this.ok}
      >
        <a-form form={this.form}>
          <a-form-item
            label="定额名称"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              placeholder={this.status == ModalStatus.View ? '' : '请输入定额名称'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'name',
                {
                  initialValue: '',
                  rules: [
                    {
                      required: true,
                      message: '请输入定额名称!',
                    },
                    {
                      max: 100,
                      message: '名称最多输入100字符',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="定额编号"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              placeholder={this.status == ModalStatus.View ? '' : '请输入定额编号'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'code',
                {
                  initialValue: '',
                  rules: [
                    {
                      required: true,
                      message: '请输入定额编号!',
                    },
                    {
                      max: 50,
                      message: '定额编号最多输入50字符',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="定额分类"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmStdBasicQuotaCategoryTreeSelect
              axios={this.axios}
              showSearch={true}
              parentDisabled={true}
              ref="SmStdBasicQuotaCategoryTreeSelect"
              placeholder={this.status === ModalStatus.View ? '' : '请选择定额分类'}
              disabled={this.status === ModalStatus.View}
              v-decorator={[
                'quotaCategoryId',
                {
                  initialValue: null,
                  rules: [{ required: true, message: '请选择定额分类！' }],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="单位"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              placeholder={this.status == ModalStatus.View ? '' : '请输入单位'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'unit',
                {
                  initialValue: '',
                  rules: [
                    {
                      required: true,
                      message: '请输入单位!',
                    },
                    {
                      max: 50,
                      message: '单位最多输入50字符',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>

          <a-form-item
            label="单位重量"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number
              style="width:100%"
              min={0}
              placeholder={this.status == ModalStatus.View ? '' : '请输入单位重量'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'weight',
                {
                  initialValue: 0,
                  rules: [
                    {
                      required: true,
                      message: '请输入单位重量!',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="人工费"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number
              style="width:100%"
              min={0}
              placeholder={this.status == ModalStatus.View ? '' : '请输入人工费'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'laborCost',
                {
                  initialValue: 0,
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="材料费"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number
              style="width:100%"
              min={0}
              placeholder={this.status == ModalStatus.View ? '' : '请输入材料费'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'materialCost',
                {
                  initialValue: 0,
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="机械使用费"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number
              style="width:100%"
              min={0}
              placeholder={this.status == ModalStatus.View ? '' : '请输入机械使用费'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'machineCost',
                {
                  initialValue: 0,
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="备注"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              placeholder={this.status == ModalStatus.View ? '' : '请输入备注'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'remark',
                {
                  initialValue: '',
                },
              ]}
            />
          </a-form-item>
        </a-form>
      </a-modal>
    );
  },
};
