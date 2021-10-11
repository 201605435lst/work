import * as utils from '../../_utils/utils';
import { ModalStatus, MarkType } from '../../_utils/enum';
import { form as formConfig, tips } from '../../_utils/config';
import ApiConstructInterfaceInfo from '../../sm-api/sm-technology/ConstructInterfaceInfo';
import { getMarkType } from '../../_utils/utils';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import SmFileUpload from '../../sm-file/sm-file-upload/SmFileUpload';
import SmSystemMenbersSelect from '../../sm-system/sm-system-member-select';
import SmSystemOrganizationUserSelect from '../../sm-system/sm-system-organization-user-select';
let apiConstructInterfaceInfo = new ApiConstructInterfaceInfo();
import moment from 'moment';

// 定义表单字段常量
const formFields = ['name', 'markType', 'markerId', 'markDate', 'builderId', 'reason'];
export default {
  name: 'SmTechnologyInterfaceFlagModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      organizationId: undefined, //组织机构
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象,
      loading: false, //确定按钮加载状态
      markFiles: [],
      imgtypes:['.jpg', '.png', '.tif', 'gif', '.JPG', '.PNG', '.GIF', '.jpeg', '.JPEG'],
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
      apiConstructInterfaceInfo = new ApiConstructInterfaceInfo(this.axios);
    },
    sign(record) {
      this.record = record;
      this.status = ModalStatus.Add;
      let name = record ? record.name : null;
      this.$nextTick(() => {
        this.form.resetFields();
        this.form.setFieldsValue({ name });
      });
    },
    async view(record) {
      this.status = ModalStatus.View;
      this.record = record;
      this.record.markDate =
        this.record && this.record.markDate ? moment(this.record.markDate) : null;
      //构造附件
      let _markFiles = [];
      if (this.record && this.record.markFiles.length > 0) {
        this.record.markFiles.map(item => {
          let file = item.file;
          if (file) {
            _markFiles.push({
              id: file.id,
              name: file.name,
              size: file.size,
              type: file.type,
              url: file.url,
            });
          }
        });
      }
      this.markFiles = _markFiles;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
      });
    },
    async edit(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      this.record.markDate =
        this.record && this.record.markDate ? moment(this.record.markDate) : null;
      //构造附件
      let _markFiles = [];
      if (this.record && this.record.markFiles.length > 0) {
        this.record.markFiles.map(item => {
          let file = item.file;
          if (file) {
            _markFiles.push({
              id: file.id,
              name: file.name,
              size: file.size,
              type: file.type,
              url: file.url,
            });
          }
        });
      }
      this.markFiles = _markFiles;

      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
      });
    },
    // 关闭模态框
    close() {
      this.form.resetFields();
      this.record = null;
      this.markFiles = [];
      this.status = ModalStatus.Hide;
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          await this.$refs.fileUpload.commit();
          let _markFiles = [];

          this.markFiles.map(item => {
            _markFiles.push({
              markFileId: item.id,
            });
          });
          let data = {
            ...values,
            constructInterfaceId: this.record ? this.record.id : null,
            markFiles: _markFiles,
          };
          console.log(data);
          let response = null;
          if (this.status === ModalStatus.Add) {
            //添加
            response = await apiConstructInterfaceInfo.create(data);
          }
          if (this.status === ModalStatus.Edit) {
            // 编辑
            response = await apiConstructInterfaceInfo.update({ id: this.record.id, ...data });
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
    let Options = [];
    for (let item in MarkType) {
      if (MarkType[item] != MarkType.NoCheck) {
        Options.push(
          <a-select-option key={`${MarkType[item]}`}>
            {getMarkType(MarkType[item])}
          </a-select-option>,
        );
      }
    }
    return (
      <a-modal
        class="sm-technology-interface-flag-modal"
        title={`${this.title}接口标记`}
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
            label="接口名称"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              disabled
              placeholder={this.status == ModalStatus.View ? '' : '请输入接口名称'}
              style="width:100%"
              v-decorator={[
                'name',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入接口名称',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
            label="检查情况"
          >
            <a-select
              placeholder="请选择"
              style="width:100%"
              disabled={this.status == ModalStatus.View}
              allowClear
              v-decorator={[
                'markType',
                {
                  initialValue: undefined,
                  rules: [
                    {
                      required: true,
                      message: '请选择检查情况',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            >
              {Options}
            </a-select>
          </a-form-item>
          <a-form-item
            label="检查人员"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmSystemOrganizationUserSelect
              organizationId={this.organizationId}
              placeholder="请选择"
              axios={this.axios}
              onOrgChange={value => {
                this.organizationId = value;
              }}
              v-decorator={[
                'markerId',
                {
                  initialValue: undefined,
                  rules: [
                    {
                      required: true,
                      message: '请选择检查人员',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="检查时间"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-date-picker
              placeholder={this.status == ModalStatus.View ? '' : '请选择检查时间'}
              disabled={this.status == ModalStatus.View}
              style="width:100%"
              onChange={(date, dateString) => {}}
              v-decorator={[
                'markDate',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请选择检查时间',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="土建单位"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <DataDictionaryTreeSelect
              disabled={this.status == ModalStatus.View}
              axios={this.axios}
              groupCode={'ConstructionUnit'}
              placeholder="请选择"
              v-decorator={[
                'builderId',
                {
                  initialValue: undefined,
                  rules: [
                    {
                      required: true,
                      message: '请选择土建单位',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="状况原因"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-textarea
              rows="3"
              placeholder={this.status == ModalStatus.View ? '' : '请输入'}
              disabled={this.status == ModalStatus.View}
              v-decorator={[
                'reason',
                {
                  initialValue: null,
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="上传附件"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmFileUpload
              mode={this.status == ModalStatus.View ? 'view' : 'edit'}
              axios={this.axios}
              height={73}
              multiple
              ref="fileUpload"
              onSelected={item => {
                this.markFiles = item;
              }}
              accept=".jpg, .png, .tif, gif, .JPG, .PNG, .GIF, .jpeg,.JPEG"
              placeholder={this.status == ModalStatus.View ? '' : '请选择文件'}
              download={true}
              fileList={this.markFiles}
            />
          </a-form-item>
        </a-form>
      </a-modal>
    );
  },
};
