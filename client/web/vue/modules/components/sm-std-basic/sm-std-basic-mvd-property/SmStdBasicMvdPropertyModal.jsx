import { ModalStatus } from '../../_utils/enum';
import { form as formConfig } from '../../_utils/config';
import ApiMVDProperty from '../../sm-api/sm-std-basic/MVDProperty';
import * as utils from '../../_utils/utils';

let apiMVDProperty = new ApiMVDProperty();
const formFields = ['name', 'unit', 'isInstance', 'order', 'remark'];

//信息交换模板属性管理的参数类型
const MVDPropertyTypeEnum = {
  Length: 1, //长度
  Digit: 2, //数值
  String: 3, //字符串
};

export default {
  name: 'SmStdBasicMvdPropertyModal',
  props: {
    axios: { type: Function, default: null },
    MVDCategory: { type: Object, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide,
      form: {},
      defaultInstance:false,
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
      apiMVDProperty = new ApiMVDProperty(this.axios);
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
      this.defaultInstance=false;
      this.form.resetFields();
    },

    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let data = {
            ...values,
            mvdCategoryId: this.MVDCategory.id,
          };
          console.log(data);
          let response = null;
          if (this.status == ModalStatus.View) {
            this.close();
          } else if (this.status == ModalStatus.Add) {
            response = await apiMVDProperty.create(data);
          } else if ((this.status = ModalStatus.Edit)) {
            response = await apiMVDProperty.update({ ...data, id: this.record.id });
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

    getMVDPropertyTypeEnum(status) {
      let title = '';
      switch (status) {
      case MVDPropertyTypeEnum.Length:
        title = '尺寸';
        break;
      case MVDPropertyTypeEnum.Digit:
        title = '数值';
        break;
      case MVDPropertyTypeEnum.String:
        title = '文本';
        break;
      default:
        title = '未定义';
      }
      return title;
    },
  },

  render() {
    //题目类型枚举
    let Options = [];
    for (let item in MVDPropertyTypeEnum) {
      Options.push(
        <a-select-option key={MVDPropertyTypeEnum[item]} value={MVDPropertyTypeEnum[item]}>
          {this.getMVDPropertyTypeEnum(MVDPropertyTypeEnum[item])}
        </a-select-option>,
      );
    }
    return (
      <a-modal
        visible={this.visible}
        title={`${this.title}信息交换模板属性管理`}
        onCancel={this.close}
        onOk={this.ok}
      >
        <a-form form={this.form}>
          <a-form-item
            label="模板分类"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input disabled={true} value={this.MVDCategory.name} />
          </a-form-item>
          <a-form-item
            label="名称"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              placeholder={this.status == ModalStatus.View ? '' : '请输入名称'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'name',
                {
                  initialValue: '',
                  rules: [
                    {
                      required: true,
                      message: '请输入名称!',
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
          {/* <a-form-item
            label="参数类型"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-select
              placeholder={this.status == ModalStatus.View ? '' : '请选择参数类型'}
              disabled={this.status == ModalStatus.View}
              //   onChange={value => {
              //     this.typeSelect = value;
              //  //   this.closeInput();
              //   }}
              v-decorator={[
                'dataType',
                {
                  initialValue: MVDPropertyTypeEnum.String,
                  rules: [
                    {
                      required: true,
                      message: '请选择类型',
                    },
                  ],
                },
              ]}
            >
              {Options}
            </a-select>
          </a-form-item> */}
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
              ]}
            />
          </a-form-item>
          <a-form-item
            label="是否实参"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-checkbox
              style="width:100%"
              disabled={this.status == ModalStatus.View}
              checked={this.defaultInstance}
              onChange={e=>this.defaultInstance=e.target.checked} 
              v-decorator={[
                'isInstance',
                {
                  initialValue: false,
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="排序"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number
              style="width:100%"
              min={1}
              placeholder={this.status == ModalStatus.View ? '' : '请输入排序'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'order',
                {
                  initialValue: 1,
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
