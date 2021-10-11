import { form as formConfig } from '../../_utils/config';

import { ModalStatus, ModelDetailLevel } from '../../_utils/enum';
import * as utils from '../../_utils/utils';
import ApiModelFile from '../../sm-api/sm-std-basic/ModelFile';
import SmFileManageSelect from '../../sm-file/sm-file-manage-select';

const formFields = ['familyFile', 'detailLevel', 'thumb'];
let apiModelFile = new ApiModelFile();
export default {
  name: 'SmStdBasicModelFileModal',

  props: {
    axios: { type: Function, default: null },
  },

  data() {
    return {
      form: {},
      status: ModalStatus.Hide,
      record: null,

      modelId: null,
    };
  },

  computed: {
    visible() {
      return this.status !== ModalStatus.Hide;
    },

    title() {
      return utils.getModalTitle(this.status);
    },
  },

  created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },

  methods: {
    initAxios() {
      apiModelFile = new ApiModelFile(this.axios);
    },

    add(key) {
      this.status = ModalStatus.Add;
      this.modelId = key;
      this.form.resetFields();
    },

    edit(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
      });
    },

    close() {
      this.form.resetFields();
      this.status = ModalStatus.Hide;
    },

    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let response = null;
          let data = {
            ...values,
            modelId: this.modelId,
            familyFileId: values.familyFile.id,
            thumbId: values.thumb.id,
          };
          if (this.status === ModalStatus.Add) response = await apiModelFile.create(data);
          else if (this.status === ModalStatus.Edit)
            response = await apiModelFile.update({ ...data, id: this.record.id });
          if (utils.requestIsSuccess(response)) {
            this.$emit('success');
            this.$message.success('操作成功');
            this.close();
          }
        }
      });
    },
  },

  render() {
    let detailLevelOption = [];
    for (let item in ModelDetailLevel) {
      detailLevelOption.push(
        <a-select-option key={ModelDetailLevel[item]} value={ModelDetailLevel[item]}>
          {utils.getModelDetailLevel(ModelDetailLevel[item])}
        </a-select-option>,
      );
    }
    return (
      <a-modal
        visible={this.visible}
        title={`${this.title}模型文件`}
        onCancel={this.close}
        onOk={this.ok}
      >
        <a-form form={this.form}>
          <a-form-item
            label="族文件"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmFileManageSelect
              disabled={this.status == ModalStatus.View}
              axios={this.axios}
              height={73}
              multiple={false}
              placeholder={this.status == ModalStatus.View ? '' : '请选择'}
              enableDownload={true}
              v-decorator={[
                'familyFile',
                {
                  initialValue: null,
                  rules: [
                    { required: true, message: '请添加族文件' },
                    {
                      validator: (rule, value, callback) => {
                        if (value && value.type != '.rfa') {
                          callback('您所选的族文件错误，请重新选择');
                        }
                        callback();
                      },
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="精细等级"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-select
              placeholder={this.status == ModalStatus.View ? '' : '请选择'}
              v-decorator={[
                'detailLevel',
                {
                  initialValue: '',
                  rules: [{ required: true, message: '请选择精细等级' }],
                },
              ]}
            >
              {detailLevelOption}
            </a-select>
          </a-form-item>
          <a-form-item
            label="缩略图"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmFileManageSelect
              disabled={this.status == ModalStatus.View}
              axios={this.axios}
              height={73}
              multiple={false}
              placeholder={this.status == ModalStatus.View ? '' : '请选择'}
              enableDownload={true}
              v-decorator={[
                'thumb',
                {
                  initialValue: null,
                  rules: [
                    { required: true, message: '请添加缩略图' },
                    {
                      validator: (rule, value, callback) => {
                        if (value && value.type.indexOf('g') == -1) {
                          callback('您所选的缩略图文件错误，请重新选择');
                        }
                        callback();
                      },
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
