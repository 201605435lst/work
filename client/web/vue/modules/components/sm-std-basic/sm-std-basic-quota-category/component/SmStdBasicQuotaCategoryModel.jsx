import * as utils from '../../../_utils/utils';
import { ModalStatus } from '../../../_utils/enum';
import ApiQuotaCategory from '../../../sm-api/sm-std-basic/QuotaCategory';
import { form as formConfig } from '../../../_utils/config';
import SmCommonAreamodele from '../../../sm-common/sm-area-module';
import SmDataDictionaryTreeSelect from '../../../sm-system/sm-system-data-dictionary-tree-select';
import SmStdBasicQuotaCategoryTreeSelect from '../../sm-std-basic-quota-category-tree-select/SmStdBasicQuotaCategoryTreeSelect';
let apiQuotaCategory = new ApiQuotaCategory();

//定义表单字段常量
const formFields = [
  'parentId',
  'name',
  'code',
  'standardCodeId',
  'standardCodeName',
  'specialtyId',
  'specialtyName',
  'content',
  // 'areaId',
  'areaName',
];
export default {
  name: 'SmStdBasicQuotaCategoryModel',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      confirmLoading: false,//确定按钮加载状态
      parentId: null,//父节点的id
      specialtyId:null,
      standardCodeId:null,
      areaId:null,
      record: null,//当前数据记录
      code: null,//自动生成的编码

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
      apiQuotaCategory = new ApiQuotaCategory(this.axios);
    },
    //添加
    add(record) {

      this.record = record;
      this.status = ModalStatus.Add;
      record.areaId = [String(record.areaId)];
      if (record != null || record != undefined)
      {

        this.parentId = record.id;
        this.areaId=record.areaId;
        this.specialtyId=record.specialtyId;
        this.standardCodeId=record.standardCodeId;

      }
      else
      {
        this.parentId =null;
        this.areaId=null;
        this.specialtyId=null;
        this.standardCodeId=null;
      }
      this.$nextTick(() => {  setTimeout(() => {
        let _values ={ parentId: this.parentId,areaId:this.areaId,specialtyId:this.specialtyId,standardCodeId:this.standardCodeId};
        this.form.setFieldsValue(_values);

      });  });
    },
    // 生成编码
    async generatedCode() {
      let num = '001';
      let response = await apiQuotaCategory.getListCode(
        this.record ? this.record.id : null,
      );
      if (utils.requestIsSuccess(response) && response.data) {

        let result = response.data;
        if (result) {
          let arr = result.code.split('.');
          let code = arr[arr.length - 1];
          num = (parseInt(code) + 1).toString();
          if (num.length == 1) {
            num = "00" + num;
          }
          if (num.length == 2) {
            num = "0" + num;
          }
        }
      }
      this.$nextTick(() => {
        this.form.setFieldsValue({ code: num });
      });
    },

    //存数据库的时候拼接
    getGeneratedCode() {

      let code = null;
      if (this.record != null) {
        code = this.record.code;
      }
      if (this.record != null && this.status == ModalStatus.Edit) {
        let arr = this.record.code.split('.');
        code = arr.slice(0, arr.length - 1).toString().replace(/,/g, '.');

      }

      this.togGetGeneratedCode(code);
    },
    togGetGeneratedCode(code) {

      let codeLength = null;

      if (code == null) {
        codeLength = 1;
      } else {
        let codes = code.split('.');
        if (codes.length > 1 || codes.length == 1) {
          codeLength = 2;
        }
      }
      switch (codeLength) {
      case 1:
        this.code = "DE" + '.';
        break;
      case 2:
        this.code = code + '.';

        break;
      }
    },

    // 编辑
    edit(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      record.areaId = [String(record.areaId)];
      this.parentId = record.parentId;

      if (record != null) {
        this.areaId=record.areaId;
        let code = record.code.split('.').pop().toString();
        this.$nextTick(() => {setTimeout(() => {
          this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields), code: code, areaId:this.areaId});

        });});
      }
      if (record == null) {
        this.status = ModalStatus.Hide;
      }

    },
    close() {
      this.form.resetFields();
      this.status = ModalStatus.Hide;
      this.confirmLoading = false;
      this.parentId = null;
    },

    ok() {
      if (this.status == ModalStatus.View) {
        this.close();
      } else {
        this.form.validateFields(async (err, values) => {
          let _values = values;

          if (!err) {

            let response = null;
            this.getGeneratedCode();
            let data = {
              ..._values,
              code: values ? this.code + values.code : '',
              areaId:this.areaId.constructor === Array?this.areaId[this.areaId.length-1]:this.areaId,

            };

            if (this.status === ModalStatus.Add) {
              response = await apiQuotaCategory.create(data);
            } else if (this.status === ModalStatus.Edit) {
              data = {
                ...data,
                id: this.record ? this.record.id : '',
              };
              response = await apiQuotaCategory.update(data);
            }
            if (utils.requestIsSuccess(response)) {
              this.$message.success('操作成功');
              if (this.status === ModalStatus.Add) {
                this.$emit('success', 'Add');

              }
              if (this.status === ModalStatus.Edit) {
                this.$emit('success', data);
                this.$emit('getParent');
              }
              this.close();

            }
          }
        });
      }
    },

  },
  render() {
    return (
      <a-modal
        title={`${this.title}定额分类`}
        visible={this.visible}
        onCancel={this.close}
        confirmLoading={this.confirmLoading}
        destroyOnClose={true}
        okText={this.status !== ModalStatus.View ? "保存" : '确定'}
        onOk={this.ok}
        width={600}
      >
        <a-form form={this.form}>
          {((this.status == ModalStatus.Add && this.parentId != null) ||
            this.status == ModalStatus.Edit ||
            this.status == ModalStatus.View) ?
            <a-form-item
              label="父级"
              label-col={formConfig.labelCol}
              wrapper-col={formConfig.wrapperCol}
            >
              <SmStdBasicQuotaCategoryTreeSelect
                axios={this.axios}
                disabled
                v-decorator={[
                  'parentId',
                  {
                    initialValue: null,
                    rules: [
                      {
                        message: '请选择父级',
                        whitespace: true,
                      },
                    ],
                  },
                ]}
              />
            </a-form-item>
            : ''
          }
          <a-form-item
            label="名称"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              allowClear
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入'}
              v-decorator={[
                'name',
                {
                  initialValue: null,
                  rules: [
                    {
                      message: '请输入定额分类名称',
                      required: true,
                      whitespace: true,
                    },
                    { max: 100, message: '名称最多输入100字符' },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="编码"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-search
              disabled={this.status == ModalStatus.View || this.status == ModalStatus.Edit}
              placeholder={this.status == ModalStatus.View || this.status == ModalStatus.Edit ? '' : '请生成编码'}
              enter-button="自动编码"
              onSearch={this.generatedCode}
              v-decorator={[
                'code',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入编码',
                      whitespace: true,
                    },
                    { max: 50, message: '编码最多输入50字符' },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="专业"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmDataDictionaryTreeSelect
              axios={this.axios}
              showSearch={true}
              parentDisabled={true}
              ref="SmDataDictionaryTreeSelect"
              placeholder={this.parentId != null ? '' : '请选择专业'}
              disabled={this.parentId != null}
              groupCode={'Profession'}
              v-decorator={[
                'specialtyId',
                {
                  initialValue: null,
                  rules: [{ required: true, message: '请选择专业！' }],
                },
              ]}
            />
          </a-form-item>
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
              placeholder={this.parentId != null? '' : '请选择标准编号'}
              disabled={ this.parentId != null}
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
            label="工作内容"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              allowClear
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入'}
              v-decorator={[
                'content',
                {
                  initialValue: null,
                  rules: [
                    { whitespace: true },
                    { max: 50, message: '工作内容最多输入50字符' },
                  ],
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
              placeholder={this.parentId != null? '' : '请选择行政区域'}
              // disabled={this.isShow}
              disabled={this.parentId != null}
              onChange={ item=>this.areaId=item[item.length-1]}
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
        </a-form>
      </a-modal>
    );
  },
};


