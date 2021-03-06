import { ModalStatus, ComputerCodeType } from '../../_utils/enum';
import { form as formConfig } from '../../_utils/config';
import ApiComputerCode from '../../sm-api/sm-std-basic/ComputerCode';
import * as utils from '../../_utils/utils';
import { requestIsSuccess, getComputerCodeTypeTitle } from '../../_utils/utils';
let apiComputerCode = new ApiComputerCode();
const formFields = ['name', 'code', 'unit', 'type', 'weight', 'remark'];
export default {
  name: 'SmStdBasicComputerCodeModal',
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
      apiComputerCode = new ApiComputerCode(this.axios);
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
            response = await apiComputerCode.create(data);
          } else if ((this.status = ModalStatus.Edit)) {
            response = await apiComputerCode.update({ ...data, id: this.record.id });
          }
          if (requestIsSuccess(response)) {
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
    //??????????????????
    let computerCodeTypeOption = [];
    for (let item in ComputerCodeType) {
      computerCodeTypeOption.push(
        <a-select-option key={ComputerCodeType[item]}>
          {getComputerCodeTypeTitle(ComputerCodeType[item])}
        </a-select-option>,
      );
    }
    return (
      <a-modal
        visible={this.visible}
        title={`${this.title}????????????`}
        onCancel={this.close}
        onOk={this.ok}
      >
        <a-form form={this.form}>
          <a-form-item
            label="???????????????"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              placeholder={this.status == ModalStatus.View ? '' : '????????????????????????'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'name',
                {
                  initialValue: '',
                  rules: [
                    {
                      required: true,
                      message: '????????????????????????!',
                    },
                    {
                      max: 200,
                      message: '??????????????????200??????',
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
            label="??????????????????"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-select
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '???????????????????????????'}
              v-decorator={[
                'type',
                {
                  initialValue: ComputerCodeType.Uniline,
                  rules: [{ required: true, message: '??????????????????????????????' }],
                },
              ]}
            >
              {computerCodeTypeOption}
            </a-select>
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
              style={'width:100%'}
              min={0}
              placeholder={this.status == ModalStatus.View ? '' : '?????????????????????'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'weight',
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
