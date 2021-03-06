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
            this.$message.success('????????????');
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
        title={`${this.title}??????`}
        onCancel={this.close}
        onOk={this.ok}
      >
        <a-form form={this.form}>
          <a-form-item
            label="????????????"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              placeholder={this.status == ModalStatus.View ? '' : '?????????????????????'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'name',
                {
                  initialValue: '',
                  rules: [
                    {
                      required: true,
                      message: '?????????????????????!',
                    },
                    {
                      max: 100,
                      message: '??????????????????100??????',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="????????????"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              placeholder={this.status == ModalStatus.View ? '' : '?????????????????????'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'code',
                {
                  initialValue: '',
                  rules: [
                    {
                      required: true,
                      message: '?????????????????????!',
                    },
                    {
                      max: 50,
                      message: '????????????????????????50??????',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="????????????"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmStdBasicQuotaCategoryTreeSelect
              axios={this.axios}
              showSearch={true}
              parentDisabled={true}
              ref="SmStdBasicQuotaCategoryTreeSelect"
              placeholder={this.status === ModalStatus.View ? '' : '?????????????????????'}
              disabled={this.status === ModalStatus.View}
              v-decorator={[
                'quotaCategoryId',
                {
                  initialValue: null,
                  rules: [{ required: true, message: '????????????????????????' }],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="??????"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              placeholder={this.status == ModalStatus.View ? '' : '???????????????'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'unit',
                {
                  initialValue: '',
                  rules: [
                    {
                      required: true,
                      message: '???????????????!',
                    },
                    {
                      max: 50,
                      message: '??????????????????50??????',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>

          <a-form-item
            label="????????????"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number
              style="width:100%"
              min={0}
              placeholder={this.status == ModalStatus.View ? '' : '?????????????????????'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'weight',
                {
                  initialValue: 0,
                  rules: [
                    {
                      required: true,
                      message: '?????????????????????!',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="?????????"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number
              style="width:100%"
              min={0}
              placeholder={this.status == ModalStatus.View ? '' : '??????????????????'}
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
            label="?????????"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number
              style="width:100%"
              min={0}
              placeholder={this.status == ModalStatus.View ? '' : '??????????????????'}
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
            label="???????????????"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number
              style="width:100%"
              min={0}
              placeholder={this.status == ModalStatus.View ? '' : '????????????????????????'}
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
            label="??????"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              placeholder={this.status == ModalStatus.View ? '' : '???????????????'}
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
