import * as utils from '../../_utils/utils';
import { ModalStatus ,UnplannedTaskType} from '../../_utils/enum';
import { getUnplannedTaskType,CreateGuid} from '../../_utils/utils';
import { form as formConfig, tips } from '../../_utils/config';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
// 定义表单字段常量
const formFields = [
  'taskType',
  'content',
];
export default {
  name: 'SmDailyUnplannedTaskModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象,
      loading: false, //确定按钮加载状态
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
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    add() {
      this.status = ModalStatus.Add;
      this.$nextTick(() => {
        this.form.resetFields();
        this.form.setFieldsValue();
      });
    },
    // 关闭模态框
    close() {
      this.form.resetFields();
      this.record = null;
      this.status = ModalStatus.Hide;
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let data = {
            ...values,
            id:CreateGuid(),
          };
          this.$emit('success',data);
          this.close();
        }
      });
      this.loading = false;
    },
  },
  render() {
    let Options = [];
    for (let item in UnplannedTaskType) {
      Options.push(
        <a-select-option key={UnplannedTaskType[item]}>
          {getUnplannedTaskType(UnplannedTaskType[item])}
        </a-select-option>,
      );
    }
    return (
      <a-modal
        title={`${this.title}临时任务`}
        visible={this.visible}
        onCancel={this.close}
        confirmLoading={this.loading}
        destroyOnClose={true}
        okText={ '确定'}
        onOk={this.ok}
        width={800}
      >

        <a-form form={this.form}>
          <a-form-item
            label="任务类型"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-select
              placeholder="请选择任务类型"
              style="width:100%"
              disabled={this.status == ModalStatus.View}
              allowClear
              v-decorator={[
                'taskType',
                {
                  initialValue: undefined,
                  rules: [
                    {
                      required: true,
                      message: '任务类型不能为空',
                    },
                  ],
                },
              ]}
            >
              {Options}
            </a-select>
          </a-form-item>
          <a-form-item
            label="任务说明"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-textarea
              rows="3"
              placeholder={this.status == ModalStatus.View ? '' : '请输入'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'content',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '任务说明不能为空',
                      whitespace: true,
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
