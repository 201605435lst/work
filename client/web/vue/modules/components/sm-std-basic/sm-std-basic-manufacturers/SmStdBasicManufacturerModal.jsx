import { ModalStatus } from '../../_utils/enum';
import { form as formConfig } from '../../_utils/config';
import * as utils from '../../_utils/utils';
import ApiManufacturer from '../../sm-api/sm-std-basic/Manufacturer';
import ManufacturerSelect from '../sm-std-basic-manufacturer-select';
import { requestIsSuccess } from '../../_utils/utils';

let apiManufacturer = new ApiManufacturer();

const formFields = [
  'parentId',
  'name',
  'shortName',
  'introduction',
  'csrgCode',
  'code',
  'principal',
  'telephone',
  'address',
];

export default {
  name: 'SmStdBasicManufacturerModal',
  props: {
    value: { type: Boolean, default: null },
    axios: { type: Function, default: null },
    type: { type: String, default: 'checkbox' },
  },
  data() {
    return {
      status: ModalStatus.Hide,
      form: {},
      record: null,
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
      apiManufacturer = new ApiManufacturer(this.axios);
    },

    add(record) {
      this.form.resetFields();
      this.status = ModalStatus.Add;
      this.record = record;
      this.$nextTick(() => {
        this.form.setFieldsValue({ parentId: record ? record.id : null });
      });
    },

    async edit(record) {
      this.status = ModalStatus.Edit;
      let response = await apiManufacturer.get(record.id);
      if (requestIsSuccess(response)) {
        this.record = response.data;
        this.$nextTick(() => {
          this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
        });
      }
    },

    async view(record) {
      this.status = ModalStatus.View;
      let response = await apiManufacturer.get(record.id);
      if (requestIsSuccess(response)) {
        this.record = response.data;
        this.$nextTick(() => {
          this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
        });
      }
    },

    // ???????????????
    close() {
      this.form.resetFields();
      this.status = ModalStatus.Hide;
      this.record = null;
    },

    async ok() {
      // ????????????
      if (this.status == ModalStatus.View) {
        this.close();
      } else {
        this.form.validateFields(async (err, values) => {
          if (!err) {
            let shortName = this.form.getFieldValue('shortName');
            let name = this.form.getFieldValue('name');
            values.shortName = shortName.replace(/[, ]/g, '');
            values.name = name.replace(/[, ]/g, '');
            let response = null;
            if (this.status === ModalStatus.Add) {
              // ??????
              response = await apiManufacturer.create({
                ...values,
              });
              if (requestIsSuccess(response)) {
                this.$message.success('????????????');
                this.close();
                this.$emit('success', 'Add', response.data);
              }
            } else if (this.status === ModalStatus.Edit) {
              // ??????
              response = await apiManufacturer.update({ id: this.record.id, ...values });
              if (requestIsSuccess(response)) {
                this.$message.success('????????????');
                this.close();
                this.$emit('success', 'Edit', response.data);
              }
            }
          }
        });
      }
    },
  },
  render() {
    return (
      <a-modal
        class="sm-std-basic-manufacturer-model"
        title={`${this.title}??????`}
        visible={this.visible}
        onCancel={this.close}
        onOk={this.ok}
      >
        <a-form form={this.form}>
          <a-form-item
            label="??????"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <ManufacturerSelect
              axios={this.axios}
              placeholder={
                this.status === ModalStatus.View || (this.record && this.record.parentId == null)
                  ? ''
                  : '?????????????????????'
              }
              disabled={this.record !== null}
              height={32}
              v-decorator={[
                'parentId',
                {
                  initialValue: null,
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
                'name',
                {
                  initialValue: '',
                  rules: [
                    { required: true, message: '????????????????????????' },
                    { max: 100, message: '??????????????????100??????' },
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
                'shortName',
                {
                  initialValue: '',
                  rules: [{ max: 50, message: '??????????????????50??????', whitespace: true }],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="????????????"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-textarea
              placeholder={this.status == ModalStatus.View ? '' : '?????????????????????'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'introduction',
                {
                  initialValue: '',
                  rules: [{ max: 500, message: '??????????????????500??????', whitespace: true }],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="CSRG??????"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              placeholder={this.status == ModalStatus.View ? '' : '?????????CSRG??????'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'csrgCode',
                {
                  initialValue: '',
                  rules: [
                    { max: 50, message: 'CSRG??????????????????50??????', whitespace: true },
                    // { pattern: /^\d+$/, message: '????????????????????????' },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="Code??????"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              placeholder={this.status == ModalStatus.View ? '' : '???????????????'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'code',
                {
                  initialValue: '',
                  rules: [
                    { max: 50, message: '??????????????????50??????', whitespace: true },
                    // { pattern: /^\d+$/, message: '????????????????????????' },
                  ],
                },
              ]}
            />
          </a-form-item>
          {/* <a-form-item
            label="??????"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              placeholder={this.status == ModalStatus.View ? '' : '???????????????'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'type',
                {
                  initialValue: '',
                  rules: [{ max: 120, whitespace: true }],
                },
              ]}
            />
          </a-form-item> */}

          {/* <a-form-item
            label="????????????"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <EquipmentControlTypeSelect
              axios={this.axios}
              placeholder={this.status == ModalStatus.View ? '' : '???????????????????????????'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'EquipmentControlTypes',
                {
                  initialValue: [],
                },
              ]}
            />
          </a-form-item> */}
          <a-form-item
            label="?????????"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              placeholder={this.status == ModalStatus.View ? '' : '??????????????????'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'principal',
                {
                  initialValue: '',
                  rules: [{ max: 50, message: '?????????????????????50??????', whitespace: true }],
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
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '?????????????????????'}
              v-decorator={[
                'telephone',
                {
                  initialValue: '',
                  rules: [{ max: 50, message: '????????????????????????50??????', whitespace: true }],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="??????"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-textarea
              placeholder={this.status == ModalStatus.View ? '' : '?????????????????????'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'address',
                {
                  initialValue: '',
                  rules: [{ max: 500, message: '??????????????????500??????', whitespace: true }],
                },
              ]}
            />
          </a-form-item>
        </a-form>
      </a-modal>
    );
  },
};
