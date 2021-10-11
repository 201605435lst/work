import ApiDossier from '../../sm-api/sm-project/Dossier';
import ApiFileCategory from '../../sm-api/sm-project/FileCategory';
import { form as formConfig, tips } from '../../_utils/config';
import { ModalStatus } from '../../_utils/enum';
import * as utils from '../../_utils/utils';
import { requestIsSuccess } from '../../_utils/utils';
import SmFileUpload from '../../sm-file/sm-file-upload/SmFileUpload';
import SmSystemOrganizationTreeSelect from '../../sm-system/sm-system-organization-tree-select';
import SmProjectActionTreeSelect from '../sm-project-action-tree-select/SmProjectActionTreeSelect';
import moment from 'moment';

import './style';

let apiDossier = new ApiDossier();
let apiFileCategory = new ApiFileCategory();


// 定义表单字段常量
const formFields = [
  'code',
  'personName',
  'name',
  'page',
  'fileCategoryId',
  'date',
  'remark',
];
export default {
  name: 'SmProjectDossierModal',
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
      // fileList:[],
      dossierRltFiles:[],
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
      apiDossier = new ApiDossier(this.axios);
      apiFileCategory = new ApiFileCategory(this.axios);

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
      //构造附件
      let _dossierRltFiles = [];
      if (this.record && this.record.dossierRltFiles.length > 0) {
        this.record.dossierRltFiles.map(item => {
          let file = item.file;
          if (file) {
            _dossierRltFiles.push({
              id: file.id,
              name: file.name,
              type: file.type,
              url: file.url,
            });
          }
        });
      }
      this.dossierRltFiles = _dossierRltFiles;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
      });
    },

    // 详情
    view(record,parentId) {
      this.status = ModalStatus.View;
      this.parentId = parentId;
      this.record = record;
      this.record.date = this.record && this.record.date ? moment(this.record.date) : null;
      //构造附件
      let _dossierRltFiles = [];
      if (this.record && this.record.dossierRltFiles.length > 0) {
        this.record.dossierRltFiles.map(item => {
          let file = item.file;
          if (file) {
            _dossierRltFiles.push({
              id: file.id,
              name: file.name,
              type: file.type,
              url: file.url,
            });
          }
        });
      }
      this.dossierRltFiles = _dossierRltFiles;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
      });
    },
    // 关闭模态框
    close() {
      this.record = null;
      this.form.resetFields();
      // this.fileList=[];
      this.dossierRltFiles=[];
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
            await this.$refs.fileUpload.commit();
            let response = null;
            let _dossierRltFiles = [];
            if (values) {
              this.dossierRltFiles.map(item => {
                _dossierRltFiles.push({
                  fileId: item.id,
                });
              });
            }
            let data = {
              ...values,
              dossierRltFiles:_dossierRltFiles,
              archivesId:this.parentId,
            };
            this.confirmLoading = true;
            if (this.status === ModalStatus.Add) {
              response = await apiDossier.create(data);
              if (utils.requestIsSuccess(response)) {
                this.$message.success('操作成功');
                this.$emit('success');
                this.close();
              }
            } else if (this.status === ModalStatus.Edit) {
              // 编辑
              response = await apiDossier.update({ id: this.record.id, ...data });
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
    // //仓库状态
    // let stateTypeOption = [];
    // for (let item in DossierEnable) {
    //   stateTypeOption.push(
    //     <a-radio key={DossierEnable[item]} value={DossierEnable[item]}>
    //       {getDossierEnableOption(DossierEnable[item])}
    //     </a-radio>,
    //   );
    // }
    return (
      <a-modal
        class="sm-project-dossier-model"
        title={`文件管理`}
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
            label="文件编号"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              allowClear
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入'}
              v-decorator={[
                'code',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入文件编号',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="责任人"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input
              allowClear
              disabled={this.status == ModalStatus.View}
              placeholder={this.status == ModalStatus.View ? '' : '请输入'}
              v-decorator={[
                'personName',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '请输入责任人',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="文件题名"
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
                      message: '请输入文件题名',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="页号"
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
                      message: '请输入页号',
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="文件类型"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmProjectActionTreeSelect
              allowClear
              axios={this.axios}
              api={apiFileCategory}
              disabled={this.status == ModalStatus.View}
              placeholder={this.status === ModalStatus.View ? '' : '请选择'}
              v-decorator={[
                'fileCategoryId',
                {
                  initialValue: [],
                  rules: [{ required: true, message: '请选择文件类型！' }],
                },
              ]}
              onChange={value => {
              }}
            />
          </a-form-item>
          <a-form-item
            label="日期"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-date-picker
              allowClear
              disabled={this.status == ModalStatus.View}
              placeholder={this.status === ModalStatus.View ? '' : '请选择'}
              style="width:100%"
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
          <a-form-item
            label="附件"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <SmFileUpload
              mode={this.status == ModalStatus.View?"view":"edit"}
              axios={this.axios}
              height={73}
              multiple
              ref="fileUpload"
              onSelected={(item) => {
                this.dossierRltFiles=item; 
              }}
              placeholder={this.status == ModalStatus.View ? '' : '请选择附件'}
              download={false}
              fileList={this.dossierRltFiles}
            />
          </a-form-item>
        </a-form>
      </a-modal>
    );
  },
};
