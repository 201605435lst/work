import ApiArchives from '../../sm-api/sm-project/Archives';
import ApiBooksClassification from '../../sm-api/sm-project/BooksClassification';
import { form as formConfig, tips } from '../../_utils/config';
import { ModalStatus, SecurityType } from '../../_utils/enum';
import * as utils from '../../_utils/utils';
import { requestIsSuccess, getSecurityTypeTitle } from '../../_utils/utils';
import SmFileManageSelect from '../../sm-file/sm-file-manage-select';
import SmProjectDossierCatrgotyTreeSelect from '../sm-project-dossier-catrgoty-tree-select/SmProjectDossierCatrgotyTreeSelect';
import SmSystemOrganizationTreeSelect from '../../sm-system/sm-system-organization-tree-select';
import SmProjectActionTreeSelect from '../sm-project-action-tree-select/SmProjectActionTreeSelect';
import './style';
import moment from 'moment';

let apiArchives = new ApiArchives();
let apiBooksClassification = new ApiBooksClassification();

// 定义表单字段常量
const formFields = [
  'fileCode',
  'projectCode',
  'archivesFilesCode',
  'booksClassificationId',
  'year',
  'name',
  'security',
  'unit',
  'page',
  'copies',
  'date',
  'remark',
];
export default {
  name: 'SmProjectArchivesModal',
  props: {
    value: { type: Boolean, default: null },
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: null, // 表单绑的对象,
      parentId: null,//父级id
      confirmLoading: false,//确定按钮加载状态
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
      apiArchives = new ApiArchives(this.axios);
      apiBooksClassification = new ApiBooksClassification(this.axios);

    },
    add(parentId) {
      this.status = ModalStatus.Add;
      this.parentId = parentId;
    },
    //编辑
    edit(record,parentId) {
      this.status = ModalStatus.Edit;
      this.parentId = parentId;
      this.record = record;
      this.record.date = this.record && this.record.date ? moment(this.record.date) : null;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(record, formFields) });
      });
    },

    // 详情
    view(record) {
      this.status = ModalStatus.View;
      this.record = record;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(record, formFields) });
      });
    },
    // 关闭模态框
    close() {
      this.record = null;
      this.form.resetFields();
      this.status = ModalStatus.Hide;
      this.confirmLoading = false;
    },
    // 数据提交
    ok() {
      if (this.status == ModalStatus.View) {
        this.close();
      } else {
        this.form.validateFields(async (err, values) => {
          if (!err) {
            let response = null;
            let data = {
              ...values,
              archivesCategoryId: this.parentId,
            };
            this.confirmLoading = true;
            if (this.status === ModalStatus.Add) {
              response = await apiArchives.create(data);
              if (utils.requestIsSuccess(response)) {
                this.$message.success('操作成功');
                this.$emit('success');
                this.close();
              }
            } else if (this.status === ModalStatus.Edit) {
              // 编辑
              response = await apiArchives.update({ id: this.record.id, ...data });
              if (utils.requestIsSuccess(response)) {
                this.$message.success('操作成功');
                this.$emit('success');
                this.close();
              }
            }

          }
        });
        this.confirmLoading = false;
      }
    },
  },
  render() {
    //机密类型
    let securityTypeOption = [];
    for (let item in SecurityType) {
      securityTypeOption.push(
        <a-select-option key={SecurityType[item]} value={SecurityType[item]}>
          {getSecurityTypeTitle(SecurityType[item])}
        </a-select-option>,
      );
    }
    return (
      <a-modal
        class="sm-project-Archives-model"
        title={`卷宗管理`}
        visible={this.visible}
        onCancel={this.close}
        confirmLoading={this.confirmLoading}
        destroyOnClose={true}
        okText="保存"
        onOk={
          this.ok
        }
        width={700}
      >
        <a-form form={this.form}>
          <a-form-item
            label="宗号"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              allowClear
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入'}
              v-decorator={[
                'fileCode',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入宗号',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="档号"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              allowClear
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入'}
              v-decorator={[
                'projectCode',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入档号',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="案卷号"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              allowClear
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入'}
              v-decorator={[
                'archivesFilesCode',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入案卷号',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="案卷分类"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmProjectActionTreeSelect
              axios={this.axios}
              allowClear
              api={apiBooksClassification}
              disabled={this.status == ModalStatus.View}
              placeholder={this.status === ModalStatus.View ? '' : '请选择'}
              v-decorator={[
                'booksClassificationId',
                {
                  initialValue: undefined,
                  rules: [{ required: true, message: '请选择案卷分类！' }],
                },
              ]}
              onChange={value => {
              }}
            />
          </a-form-item>
          <a-form-item
            label="年度"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              allowClear
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入'}
              v-decorator={[
                'year',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入年度',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="案卷题名"
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
                      required: true,
                      message: '请输入案卷题名',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="密级"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-select
              allowClear={false}
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请选择'}
              v-decorator={[
                'security',
                {
                  initialValue: SecurityType.common,
                },
              ]}
            >
              {securityTypeOption}
            </a-select>
          </a-form-item>

          <a-form-item
            label="编制单位"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              allowClear
              disabled={this.status == ModalStatus.View}
              placeholder={this.status === ModalStatus.View ? '' : '请输入'}
              v-decorator={[
                'unit',
                {
                  initialValue: null,
                  rules: [{ required: true, message: '请输入编制单位！' }],
                },
              ]}
              onChange={value => {
              }}
            />
          </a-form-item>
          <a-form-item
            label="编制日期"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-date-picker
              disabled={this.status == ModalStatus.View}
              placeholder={this.status === ModalStatus.View ? '' : '请选择'}
              style="width:100%"
              allowClear
              onChange={(date, dateString) => {
              }}
              v-decorator={[
                'date',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请选择日期',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="页数"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number
              allowClear
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入'}
              style="width:100%"
              min={0}
              precision={0}
              v-decorator={[
                'page',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入页数',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="份数"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input-number
              allowClear
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入'}
              style="width:100%"
              min={0}
              precision={0}
              v-decorator={[
                'copies',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入份数',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="备注"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}

          >
            <a-textarea
              allowClear
              disabled={this.status == ModalStatus.View}
              placeholder={this.status === ModalStatus.View ? '' : '请输入'}
              v-decorator={
                [
                  'remark',
                  {
                    initialValue: null,
                  },
                ]
              }
            ></a-textarea>
          </a-form-item>
        </a-form>
      </a-modal>
    );
  },
};
