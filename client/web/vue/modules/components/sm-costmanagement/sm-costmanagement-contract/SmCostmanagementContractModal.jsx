import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import { form as formConfig, tips } from '../../_utils/config';
import ApiContract from '../../sm-api/sm-costmanagement/Contract';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import SmFileManageSelect from '../../sm-file/sm-file-manage-select';
import SmFileUpload from '../../sm-file/sm-file-upload/SmFileUpload';

let apiContract = new ApiContract();
import moment from 'moment';

// 定义表单字段常量
const formFields = [
  'name',
  'typeId',
  'date',
  'money',
  'remark',
  //   'contractRltFiles', //文件
];
export default {
  name: 'SmCostmanagementContractModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象,
      loading: false, //确定按钮加载状态
      fileList: [],

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
      apiContract = new ApiContract(this.axios);
    },
    add() {
      this.status = ModalStatus.Add;
      this.$nextTick(() => {
        this.form.resetFields();
        this.form.setFieldsValue();
      });
    },
    async view(record) {
      this.status = ModalStatus.View;
      this.record = record;
      this.record.date = this.record && this.record.date ? moment(this.record.date) : null;
      //构造附件
      let _contractRltFiles = [];
      if (this.record && this.record.contractRltFiles.length > 0) {
        this.record.contractRltFiles.map(item => {
          let file = item.file;
          if (file) {
            _contractRltFiles.push({
              id: file.id,
              name: file.name,
              file: file,
              type: file.type,
              url: file.url,
            });
          }
        });
      }
      this.fileList = _contractRltFiles;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
      });
    },
    async edit(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      this.record.date = this.record && this.record.date ? moment(this.record.date) : null;
      //构造附件
      let _contractRltFiles = [];
      if (this.record && this.record.contractRltFiles.length > 0) {
        this.record.contractRltFiles.map(item => {
          let file = item.file;
          if (file) {
            _contractRltFiles.push({
              id: file.id,
              name: file.name,
              file: file,
              type: file.type,
              url: file.url,
            });
          }
        });
      }
      this.fileList = _contractRltFiles;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
      });
    },
    // 关闭模态框
    close() {
      this.form.resetFields();
      this.record = null;
      this.fileList = [];
      this.status = ModalStatus.Hide;
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          if (this.status === ModalStatus.View) {
            this.close();
            return;
          }
          await this.$refs.fileUpload.commit();
          let _contractRltFiles = [];
          if (values) {
            this.fileList.map(item => {
              _contractRltFiles.push({
                fileId: item.id,
              });
            });
          }
          let _code = this.generatedCode();
          let data = {
            ...values,
            contractRltFiles: _contractRltFiles,
            code: (this.status == ModalStatus.Add ? _code : this.record.code),
          };
          let response = null;
          if (this.status === ModalStatus.Add) {
            console.log("添加");
            //添加
            response = await apiContract.create(data);
          }
          if (this.status === ModalStatus.Edit) {
            // 编辑
            response = await apiContract.update({ id: this.record.id, ...data });
          }
          if (utils.requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.$emit('success');
          }
          this.close();
        }
      });
      this.loading = false;
    },
    disabledDate(current) {
      // Can not select days before today and today
      return current && current > moment().endOf('day');
    },
    // 生成编码
    generatedCode() {
      let num = '';
      let date = moment().format('YYYY-MM-DD-HH-mm-ss');
      num = date.replaceAll('-', '');
      let code = 'HT-' + num;
      return code;
    },
  },
  render() {
    return (
      <a-modal
        title={`${this.title}合同`}
        visible={this.visible}
        onCancel={this.close}
        confirmLoading={this.loading}
        destroyOnClose={true}
        okText={this.status !== ModalStatus.View ? '保存' : '确定'}
        onOk={this.ok}
        width={800}
      >
        <a-form form={this.form}>
          <a-form-item
            label="合同名称"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入合同名称'}
              style="width:100%"
              v-decorator={[
                'name',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入合同名称',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="合同日期"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-date-picker
              disabledDate={this.disabledDate}
              placeholder={this.status == ModalStatus.View ? '' : '请选择合同日期'}
              disabled={this.status == ModalStatus.View}
              style="width:100%"
              v-decorator={[
                'date',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请选择合同日期',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="合同分类"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <DataDictionaryTreeSelect
              disabled={this.status == ModalStatus.View}
              axios={this.axios}
              groupCode={'CostmanagementContract'}
              placeholder="请选择"
              v-decorator={[
                'typeId',
                {
                  initialValue: undefined,
                  rules: [
                    {
                      required: true,
                      message: '请选择合同分类',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="合同金额(万)"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入合同金额'}
              style="width:100%"
              min={0}
              v-decorator={[
                'money',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入合同金额',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="文件"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmFileUpload
              mode={this.status == ModalStatus.View ? "view" : "edit"}
              axios={this.axios}
              height={73}
              multiple
              ref="fileUpload"
              onSelected={(item) => {
                this.fileList = item;
              }}
              placeholder={this.status == ModalStatus.View ? '' : '请选择文件'}
              download={true}
              fileList={this.fileList}
            />
          </a-form-item>
          <a-form-item
            label="备注"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-textarea
              rows="3"
              placeholder={this.status == ModalStatus.View ? '' : '请输入'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'remark',
                {
                  initialValue: null,
                },
              ]}
            />
          </a-form-item>
        </a-form>
      </a-modal>
    );
  },
};
