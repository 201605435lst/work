
import './style';
import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import { requestIsSuccess} from '../../_utils/utils';

import ApiPlanMaterial from '../../sm-api/sm-construction/ApiPlanMaterial';
import ApiDataDictionary from '../../sm-api/sm-system/DataDictionary';


let apiPlanMaterial = new ApiPlanMaterial();
let apiDataDictionary = new ApiDataDictionary();// 字典api 查 类型用


// 表单字段
const formFields = [];
export default {
  name: 'SmConstructionPlanMaterialModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象
      dicTypes: [],

    };
  },
  computed: {
    title() {
      return utils.getModalTitle(this.status); // 计算模态框的标题变量
    },
    visible() {
      return this.status !== ModalStatus.Hide; // 计算模态框的显示变量
    },
  },
  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});// 创建表单
    this.getDicTypes(); // 获取字典类型列表
  },
  methods: {
    initAxios() {
      apiPlanMaterial = new ApiPlanMaterial(this.axios);
      apiDataDictionary = new ApiDataDictionary(this.axios);
    },
    // 添加
    add() {
      this.status = ModalStatus.Add;
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
    // 获取 工程类型 列表
    async getDicTypes() {
      let res = await apiDataDictionary.getValues({ groupCode: 'Profession.' });
      if (requestIsSuccess(res) && res.data) {
        this.dicTypes = res.data;
      }
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          // err  是 表单不通过 的 错误  values 是表单内容{}
          // console.log(values);
          let response = null;
          if (this.status === ModalStatus.Add) {
            response = await apiPlanMaterial.create({ ...values }); // 添加施工计划工程量
          } else if (this.status === ModalStatus.Edit) {
            response = await apiPlanMaterial.update(this.record.id, { ...values }); // 编辑施工计划工程量
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
        title={`施工计划工程量${this.title}`}
        visible={this.visible}
        onCancel={this.close}
        destroyOnClose={true}
        onOk={this.ok}
      >
        <a-form form={this.form}>

        </a-form>
      </a-modal>
    );
  },
};
