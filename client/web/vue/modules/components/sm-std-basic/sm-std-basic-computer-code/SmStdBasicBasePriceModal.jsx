import { ModalStatus } from '../../_utils/enum';
import { form as formConfig } from '../../_utils/config';
import ApiBasePrice from '../../sm-api/sm-std-basic/BasePrice';
import * as utils from '../../_utils/utils';
import SmCommonAreamodele from '../../sm-common/sm-area-module';
import SmDataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
let apiBasePrice = new ApiBasePrice();
const formFields = [
  // 'computerCodeId',
  'price',
  'standardCodeId',
  'areaId',
];
export default {
  name: 'SmStdBasicBasePriceModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      confirmLoading: false, //确定按钮加载状态
      computerCodeId: null,
      areaId: null,
      record: null, //当前数据记录
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
      apiBasePrice = new ApiBasePrice(this.axios);
    },

    add(record) {
      this.status = ModalStatus.Add;
      this.computerCodeId = record.id;
      this.$nextTick(() => {
        this.form.setFieldsValue();
      });
    },

    detail(record) {
      this.status = ModalStatus.View;
      this.record = record;
      record.areaId = [String(record.areaId)];
      this.record.areaId = record.areaId;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(record, formFields) });
      });
    },

    edit(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      this.computerCodeId = record.computerCodeId;
      record.areaId = [String(record.areaId)];
      this.record.areaId = record.areaId;
      this.areaId = record.areaId;

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
            //areaId: this.areaId.length>0 ? this.areaId.pop() : "",
            areaId:
            Array.isArray(this.areaId)? this.areaId[this.areaId.length - 1] : this.areaId,
            computerCodeId: this.computerCodeId,
          };
          let response = null;
          if (this.status == ModalStatus.View) {
            this.close();
          } else if (this.status == ModalStatus.Add) {
            response = await apiBasePrice.create(data);
          } else if ((this.status = ModalStatus.Edit)) {
            response = await apiBasePrice.update({ ...data, id: this.record.id });
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
        title={`${this.title}基价`}
        onCancel={this.close}
        onOk={this.ok}
      >
        <a-form form={this.form}>
          <a-form-item
            label="标准编号"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmDataDictionaryTreeSelect
              axios={this.axios}
              showSearch={true}
              parentDisabled={true}
              ref="SmDataDictionaryTreeSelect"
              placeholder={this.status === ModalStatus.View ? '' : '请选择标准编号'}
              disabled={this.status === ModalStatus.View}
              groupCode={'StandardCode'}
              v-decorator={[
                'standardCodeId',
                {
                  initialValue: null,
                  rules: [{ required: true, message: '请选择标准编号！' }],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="行政区域"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmCommonAreamodele
              ref="SmCommonAreamodele"
              axios={this.axios}
              showSearch={true}
              deep={2}
              placeholder={this.status === ModalStatus.View ? '' : '请选择行政区域'}
              disabled={this.status === ModalStatus.View}
              onChange={item => (this.areaId = item[item.length - 1])}
              // value={this.record?this.record.areaId:undefined}
              v-decorator={[
                'areaId',
                {
                  initialValue: null,
                  rules: [{ required: true, message: '请选择行政区域！' }],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="基期单价"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number
              style={'width:100%'}
              min={0}
              placeholder={this.status == ModalStatus.View ? '' : '请输入基期单价'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'price',
                {
                  initialValue: 0,
                },
              ]}
            />
            {/* <a-input
              placeholder={this.status == ModalStatus.View ? '' : '请输入基期单价'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'price',
                {
                  initialValue: '',
                  rules: [{ required: true, message: '请输入基期单价！' }],
                },
              ]}
            /> */}
          </a-form-item>
        </a-form>
      </a-modal>
    );
  },
};
