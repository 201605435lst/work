import { ModalStatus } from '../../_utils/enum';
import { form as formConfig } from '../../_utils/config';
import ApiQuotaItem from '../../sm-api/sm-std-basic/QuotaItem';
import * as utils from '../../_utils/utils';
import SmStdBasicComputerCodeSelect from '../sm-std-basic-computer-code-select';
import SmStdBasicBasePriceSelect from '../sm-std-basic-base-price-select';
let apiQuotaItem = new ApiQuotaItem();
const formFields = ['computerCodeId', 'basePriceIdList', 'standardCode', 'number', 'remark'];
export default {
  name: 'SmStdBasicQuotaItemModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      confirmLoading: false, //确定按钮加载状态
      basePriceIdList: [],
      quotaId: null,
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
      apiQuotaItem = new ApiQuotaItem(this.axios);
    },

    add(record) {
      this.status = ModalStatus.Add;
      this.record = [];
      this.quotaId = record.id;
      this.record.quotaId = record.id;
      this.basePriceIdList=[];
      this.$nextTick(() => {
        this.form.setFieldsValue();
      });
    },

    detail(record) {
      this.status = ModalStatus.View;
      this.record = record;
      this.quotaId = record.quotaId;
      this.computerCodeId = record.computerCodeId;
      this.basePriceIdList = record.basePriceIdList;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(record, formFields) });
      });
    },

    edit(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      this.quotaId = record.quotaId;
      this.computerCodeId = record.computerCodeId;
      this.basePriceIdList = record.basePriceIdList;
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
            areaId: this.areaId,
            quotaId: this.quotaId,
            computerCodeId: this.computerCodeId,
            basePriceIdList:this.basePriceIdList,
          };
          let response = null;
          if (this.status == ModalStatus.View) {
            this.close();
          } else if (this.status == ModalStatus.Add) {
            response = await apiQuotaItem.create(data);
          } else if (this.status == ModalStatus.Edit) {
            response = await apiQuotaItem.update({
              ...data,
              id: this.quotaId,
              computerCodeId: this.computerCodeId,
            });
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
        width={650}
        title={`${this.title}清单`}
        onCancel={this.close}
        onOk={this.ok}
      >
        <a-form form={this.form}>
          <a-form-item
          
            label="电算代号名称"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmStdBasicComputerCodeSelect
              ref="SmStdBasicComputerCodeSelect"
              axios={this.axios}
              showSearch={false}
              treeCheckable={false}
              placeholder={this.status === ModalStatus.View ? '' : '请选择电算代号'}
              disabled={this.status == ModalStatus.View}
              onChange={item => {
                this.computerCodeId = item;
              }}
              v-decorator={[
                'computerCodeId',
                {
                  initialValue: [],
                  rules: [{ required: true, message: '请选择电算代号！' }],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="基期单价"
            label-col={formConfig.labelCol}
            required={ true}
            wrapper-col={formConfig.wrapperCol}
          >
            {/* <div slot="label">
              <a-icon type="plus"/>
              <span>基期单价</span>
            </div> */}
            <SmStdBasicBasePriceSelect
              ref="SmStdBasicBasePriceSelect"
              axios={this.axios}
              treeCheckable={true}
              treeCheckStrictly={true}
              maxTagCount={5}
              showSearch={false}
              computerCodeId={this.computerCodeId}
              placeholder={this.status === ModalStatus.View ? '' : '请选择基价'}
              disabled={this.status == ModalStatus.View}
              value={this.basePriceIdList}
              onChange={value => {
                this.basePriceIdList = value;
                // this.refresh(false);
              }}
              // v-decorator={[
              //   'basePriceIdList',
              //   {
              //     initialValue: [],
              //     rules: [{ required: true, message: '请选择基价！' }],
              //   },
              // ]}
            />
          </a-form-item>
          <a-form-item
            label="数量"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number
              style="width:100%"
              min={0}
              placeholder={this.status == ModalStatus.View ? '' : '请输入数量'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'number',
                {
                  initialValue: 0,
                  rules: [
                    {
                      required: true,
                      message: '请输入数量!',
                    },
                  ],
                },
              ]}
            />
            {/* <a-input
              placeholder={this.status == ModalStatus.View ? '' : '请输入数量'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'number',
                {
                  initialValue: '',
                  rules: [
                    {
                      required: true,
                      message: '请输入数量!',
                    },
                  ],
                },
              ]}
            /> */}
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
