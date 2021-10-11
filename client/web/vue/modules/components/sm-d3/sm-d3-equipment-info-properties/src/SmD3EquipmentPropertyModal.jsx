import { form as formConfig, tips } from '../../../_utils/config';
import * as utils from '../../../_utils/utils';
import { EquipmentPropertyType, ModalStatus, CableLayType } from '../../../_utils/enum';
import ApiEquipmentProperty from '../../../sm-api/sm-resource/EquipmentProperty';
import ApiCableExtend from '../../../sm-api/sm-resource/CableExtend';

let apiEquipmentProperty = new ApiEquipmentProperty();
let apiCableExtend = new ApiCableExtend();

export default {
  name: 'SmD3EquipmentPropertyModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      record: {}, // 表单绑定的对象,
      equipmentId: null, //当前设备Id
      loading: false, // 数据保存加载状态
      form: {}, // 表单
      equipmentPropertyType: EquipmentPropertyType.Extend,

    };
  },
  computed: {
    title() {
      // 计算模态框的标题变量
      return utils.getModalTitle(this.status);
    },
    visible() {
      // 计算模态框的显示变量
      return this.status !== ModalStatus.Hide;
    },
    formFields() {
      // 计算模态框的显示变量
      return this.equipmentPropertyType === EquipmentPropertyType.CableProperty ?
        ['value'] :
        ['name', 'value', 'order'];
    },
  },
  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    initAxios() {
      apiEquipmentProperty = new ApiEquipmentProperty(this.axios);
      apiCableExtend = new ApiCableExtend(this.axios);
    },

    // 添加
    add(equipmentId) {
      this.equipmentId = equipmentId;
      this.status = ModalStatus.Add;
      this.$nextTick(() => {
        this.form.resetFields();
      });
    },

    // 编辑
    edit(equipmentId, record) {
      this.equipmentId = equipmentId;
      this.status = ModalStatus.Edit;
      this.record = JSON.parse(JSON.stringify(record));
      if (this.record.type === EquipmentPropertyType.CableProperty) {
        this.equipmentPropertyType = EquipmentPropertyType.CableProperty;
        this.record.value = this.record.value && this.record.name === '铺设类型' ? parseInt(this.record.value) : this.record.value;
      }
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(this.record, this.formFields) });
      });
    },

    // 关闭模态框
    close() {
      this.form.resetFields();
      this.loading = false;
      this.status = ModalStatus.Hide;
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let response = null;
          this.loading = true;
          let data = null;
          if (this.status === ModalStatus.Add) {
            // 添加
            data = {
              ...values,
              order: values.order ? values.order : 0,
              equipmentId: this.equipmentId,
            };
            response = await apiEquipmentProperty.create(data);
          } else if (this.status === ModalStatus.Edit) {
            // 编辑
            if (this.record.type === EquipmentPropertyType.Extend) {
              data = {
                ...values,
                id: this.record.id,
                order: values.order ? values.order : 0,
                equipmentId: this.equipmentId,
              };
              response = await apiEquipmentProperty.update(data);
            } else if (this.record.type === EquipmentPropertyType.CableProperty) {
              let cableExtendResponse = await apiCableExtend.get(this.record.id);
              let cableExtend = null;
              if (utils.requestIsSuccess(cableExtendResponse)) {
                cableExtend = cableExtendResponse.data;
                if (this.record.name === '芯数') {
                  data = {
                    ...cableExtend,
                    number: values.value,
                  };
                } else if (this.record.name === '备用芯数') {
                  data = {
                    ...cableExtend,
                    spareNumber: values.value,
                  };
                }
                else if (this.record.name === '路产芯数') {
                  data = {
                    ...cableExtend,
                    railwayNumber: values.value,
                  };
                } else if (this.record.name === '皮长公里') {
                  data = {
                    ...cableExtend,
                    length: parseFloat(values.value),
                  };
                } else if (this.record.name === '铺设类型') {
                  data = {
                    ...cableExtend,
                    layType: parseInt(values.value),
                  };
                }
              }
              response = await apiCableExtend.update(data);
            }
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
    let options = [];
    for (let item in CableLayType) {
      options.push(
        <a-select-option key={CableLayType[item]}>
          {utils.getCableLayTypeTile(CableLayType[item])}
        </a-select-option>,
      );
    }

    let rulePattern = {};
    if (this.record.name === '芯数' || this.record.name === '路产芯数' || this.record.name === '备用芯数') {
      rulePattern = { pattern: /^[1-9]+[0-9]*$/, message: '电缆芯数只能是正整数' };
    } else if (this.record.name === '皮长公里') {
      rulePattern = { pattern: /^(|[0-9])+(\.[0-9]+)?$/, message: '请输入正确的长度值' };
    }

    return (
      <a-modal
        title={`${this.title}属性`}
        visible={this.visible}
        confirmLoading={this.loading}
        destroyOnClose={true}
        onCancel={this.close}
        onOk={this.ok}
      >
        <a-form form={this.form}>
          {this.record.type !== EquipmentPropertyType.CableProperty ?
            <a-form-item
              label="属性名称"
              label-col={formConfig.labelCol}
              wrapper-col={formConfig.wrapperCol}
            >
              <a-input
                axios={this.axios}
                disabled={this.status == ModalStatus.View}
                placeholder={this.status == ModalStatus.View ? '' : '请输入属性名称'}
                v-decorator={[
                  'name',
                  {
                    initialValue: null,
                    rules: [{ required: true, message: '请输入属性名称', whitespace: true }],
                  },
                ]}
              />

            </a-form-item> : undefined}


          <a-form-item
            label={this.record.type !== EquipmentPropertyType.CableProperty ? "属性值" : this.record.name}
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            {this.record.name === '铺设类型' ? <a-select
              placeholder='请选择铺设类型'
              v-decorator={[
                'value',
                {
                  initialValue: CableLayType.Conduit,
                  rules: [
                    { required: true, message: '请选择铺设类型' },
                  ],
                },
              ]}
            >
              {options}
            </a-select> : <a-input
              axios={this.axios}
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入属性值'}
              addonAfter={this.record.name === '皮长公里' ? '单位（m）' : ''}
              maxLength={15}
              v-decorator={[
                'value',
                {
                  initialValue: null,
                  rules: [
                    { required: true, message: '请输入属性值', whitespace: true },
                    rulePattern,
                  ],
                },
              ]}
            />}

          </a-form-item>

          {this.record.type === EquipmentPropertyType.Extend || this.status === ModalStatus.Add ? <a-form-item
            label="排序"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入排序'}
              min={0}
              precision={0}
              style="width:100%;"
              v-decorator={[
                'order',
                {
                  initialValue: null,
                  rules: [{ pattern: /^(?:0|[1-9]\d{0,8})?$/, message: '不能超过9位数' }],
                },
              ]}
            />
          </a-form-item> : undefined}

        </a-form>
      </a-modal>
    );
  },
};
