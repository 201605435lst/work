import './style';
import { form as formConfig } from '../../_utils/config';
import { ModalStatus } from '../../_utils/enum';
import * as utils from '../../_utils/utils';
import ApiEquipmentTeam from '../../sm-api/sm-construction-base/ApiEquipmentTeam';

let apiEquipmentTeam = new ApiEquipmentTeam();
const formFields = ['name', 'typeId', 'spec', 'cost'];
export default {
  name: 'SmConstructionBaseEquipmentTeamModal',
  props: {
    axios: { type: Function, default: null },
    // 设备类型
    equipmentTypes: { type: Array, default: () => [] },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象
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
      apiEquipmentTeam = new ApiEquipmentTeam(this.axios);
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
            response = await apiEquipmentTeam.create({ ...values });
          } else if (this.status === ModalStatus.Edit) {
            // 添加设备台班信息
            response = await apiEquipmentTeam.update(this.record.id, { ...values });
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
        title={`${this.title}设备台班信息`}
        visible={this.visible}
        onCancel={this.close}
        destroyOnClose
        onOk={this.ok}
      >
        <a-form form={this.form}>
          <a-form-item
            label='设备类型'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}>
            <a-select
              placeholder={'请选择设备类型'}
              v-decorator={[
                'typeId',
                {
                  initialValue: undefined,
                  rules: [{ required: true, message: '请选择设备类型' }],
                },
              ]}>
              {this.equipmentTypes.map(x => {
                return (<a-select-option value={x.id}>{x.name}</a-select-option>);
              })}
            </a-select>
          </a-form-item>
          <a-form-item
            label='设备名称'
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
                    { required: true, message: '请输入设备台班名！', whitespace: true },
                  ],
                },
              ]}
              placeholder={this.status === ModalStatus.View ? '' : '请输入设备台班名'}
            />
          </a-form-item>
          <a-form-item
            label='设备规格'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status === ModalStatus.View}
              v-decorator={[
                'spec',
                {

                  initialValue: '',
                  rules: [
                    { required: true, message: '请输入设备规格！', whitespace: true },
                  ],
                },
              ]}
              placeholder={this.status === ModalStatus.View ? '' : '请输入设备规格'}
            />
          </a-form-item>
          <a-form-item
            label='综合成本'
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number
              style='width:100%'
              min={0}
              precision={2}
              placeholder={this.status === ModalStatus.View ? '' : '0元'}
              v-decorator={[
                'cost',
                {
                  initialValue: null,
                  rules: [
                    { pattern: /^(|[0-9])+(\.[0-9]+)?$/, message: '请输入正确数字' },
                    { required: true, message: '请输入正确数字' },
                  ],
                },
              ]}
            />
          </a-form-item>
        </a-form>
      </a-modal>
    );
  }
  ,
}
;
