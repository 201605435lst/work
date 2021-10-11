import './style';
import ApiMaterial from '../../sm-api/sm-construction-base/ApiMaterial';
import { form as formConfig } from '../../_utils/config';
import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import { vIf } from '../../_utils/utils';

let apiMaterial = new ApiMaterial();

const formFields = ['code', 'name', 'unit', 'isSelf', 'isPartyAProvide', 'presentDays', 'prePurchaseDays'];


export default {
  name: 'SmConstructionBaseMaterialModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象
      // 计量单位
      unit: [
        { id: 1, name: '个' },
        { id: 2, name: '方' },
        { id: 3, name: '吨' },
        { id: 4, name: '米' },
        { id: 5, name: 'm³' },
        { id: 6, name: '袋' },
      ],
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
  },
  async created() {
    this.initAxios();
    // 创建表单
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    initAxios() {
      apiMaterial = new ApiMaterial(this.axios);
    },

    // 添加
    add() {
      this.status = ModalStatus.Add;
      // $nextTick -> Dom异步更新
      this.$nextTick(() => {
        this.form.resetFields();
      });
    },
    // 关闭模态框
    close() {
      this.form.resetFields();
      this.status = ModalStatus.Hide;
    },
    // 编辑
    edit(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(record, formFields) });
      });
    },
    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          // err  是 表单不通过 的 错误  values 是表单内容{}
          console.log(values);
          let response = null;
          if (this.status === ModalStatus.Add) {
            // 添加设备台班信息
            response = await apiMaterial.create({ ...values });
          } else if (this.status === ModalStatus.Edit) {
            // 添加设备台班信息
            response = await apiMaterial.update(this.record.id, { ...values });
          }
          if (utils.requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.close();
            this.$emit('success');
          }
        }
      });
    },
  },
  render() {
    return (
      <a-modal
        title={`工程量清单${this.title}`}
        visible={this.visible}
        onCancel={this.close}
        destroyOnClose
        onOk={this.ok}
      >
        <a-form form={this.form}>
          <div style='display:flex;margin-left:20.83333333%'>
            <a-form-item style='display:inline'>
              <a-checkbox
                onChange={$event => {
                  this.form.isSelf = $event.target.checked;
                  if ($event.target.checked) {
                    // 给form 里面 的 某个属性赋值要这么写,直接赋值没用
                    this.form.setFieldsValue({ isPartyAProvide: false });
                  }

                }}
                v-decorator={[
                  'isSelf',
                  {
                    valuePropName: 'checked',
                    initialValue: false,
                  },
                ]}
              >
                工程材料
              </a-checkbox>
            </a-form-item>
            <a-form-item style='display:inline;margin-left:2jpx'>
              <a-checkbox
                onChange={$event => {
                  this.form.isPartyAProvide = $event.target.checked;
                  if ($event.target.checked) {
                    // 给form 里面 的 某个属性赋值要这么写,直接赋值没用
                    this.form.setFieldsValue({ isSelf: false });
                  }
                }}
                v-decorator={[
                  'isPartyAProvide',
                  {
                    valuePropName: 'checked',
                    initialValue: false,
                  },
                ]}
              >
                甲供
              </a-checkbox>
            </a-form-item>
          </div>


          <a-form-item label='工程量编码' label-col={formConfig.labelCol} wrapper-col={formConfig.wrapperCol}>
            <a-input
              disabled={this.status === ModalStatus.View}
              v-decorator={[
                'code',
                {
                  initialValue: '',
                  rules: [
                    { required: true, message: '请输入工程量编码！', whitespace: true },
                  ],
                },
              ]}
              placeholder={'请输入工程量编码'}
            />
          </a-form-item>
          <a-form-item
            label='工程量名称'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status === ModalStatus.View}
              v-decorator={[
                'name',
                {
                  initialValue: '',
                  rules: [
                    { required: true, message: '请输入工程量名称！', whitespace: true },
                  ],
                },
              ]}
              placeholder={'请输入工程量名称'}
            />
          </a-form-item>
          <a-form-item
            label='计量单位'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}>
            <a-select
              placeholder={'请选择计量单位'}
              v-decorator={[
                'unit',
                {
                  initialValue: undefined,
                  rules: [{ required: true, message: '请选择计量单位' }],
                },
              ]}>
              {this.unit.map(x => (<a-select-option value={x.id}>{x.name}</a-select-option>))}
            </a-select>
          </a-form-item>
          {vIf(
            <div>
              <a-form-item
                label='提前到场天数'
                label-col={formConfig.labelCol}
                wrapper-col={formConfig.wrapperCol}>
                {
                  this.form.getFieldDecorator('presentDays', {
                    initialValue: 0,
                    rules: [
                      { pattern: /^[0-9]\d*$/, message: '请输入正确数字' },
                    ],
                  })(
                    <a-input-number
                      style='width:100%'
                      min={0}
                      precision={0}
                      placeholder={'请输入提前到场天数'}
                    />,
                  )
                }

              </a-form-item>
              <a-form-item
                label='采购前置天数'
                label-col={formConfig.labelCol}
                wrapper-col={formConfig.wrapperCol}
              >
                {
                  this.form.getFieldDecorator('prePurchaseDays', {
                    initialValue: 0,
                    rules: [
                      { pattern: /^[0-9]\d*$/, message: '请输入正确数字' },
                    ],
                  })(
                    <a-input-number
                      style='width:100%'
                      min={0}
                      placeholder={'请输入采购前置天数'}
                    />,
                  )
                }

              </a-form-item>
            </div>,
            // 获取 form 的 isSelf值,true 的时候 显示 天数输入框
            this.form.getFieldValue('isSelf'),
          )}
        </a-form>
      </a-modal>
    );
  },
};
