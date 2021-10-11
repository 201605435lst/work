import { form as formConfig, tips } from '../../_utils/config';
import { ModalStatus, PageState } from '../../_utils/enum';
import * as utils from '../../_utils/utils';
import SmSystemDataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import { requestIsSuccess } from '../../_utils/utils';
import ApiContract from '../../sm-api/sm-oa/Contract';
import OrganizationTreeSelect from '../../sm-system/sm-system-organization-tree-select';
import RichTextEditor from '../../sm-file/sm-file-text-editor/SmFileTextEditor';
import SmFileManageSelect from '../../sm-file/sm-file-manage-select';
import SmSystemUserSelect from '../../sm-system/sm-system-user-select/SmSystemUserSelect';
import SmOaContractSelectAssociatedDocumentModal from './src/SmOaContractSelectAssociatedDocumentModal';
import SmFileUpload from '../../sm-file/sm-file-upload';
import moment from 'moment';

let apiContract = new ApiContract();

import './style';
// 定义表单字段常量
const formFields = [
  'name',
  'code',
  'signTime',
  'partyA',
  'partyB',
  'partyC',
  'hostDepartmentId',
  'underDepartmentId',
  'undertakerId',
  'amount',
  'amountWords',
  'budge',
  'typeId',
  // 'contractRltFiles',
  'otherPartInfo',
];
export default {
  name: 'SmOaContract',
  props: {
    axios: { type: Function, default: null },
    id: { type: String, default: null },
    permissions: { type: Array, default: () => [] },
    pageState: { type: String, default: PageState.Add }, // 页面状态
    organizationId: { type: String, default: null },
  },
  data() {
    return {
      form: {}, // 表单
      record: null, // 表单绑的对象,
      abstract: null,
      iId: null,//合同id
      loading: false,
      COrganizationId: undefined,//承办组织机构
      fileServerEndPoint: '', //文件服务请求头
      content: null,
      fileList:[],//附件绑定信息
      contractRltFiles:[],//附件信息
      RMB: null,//人民币大写
    };
  },
  computed: {
  },
  watch: {
    organizationId: {
      handler: function (value, oldValue) {
        this.ZOrganizationId = value;
      },
      immediate: true,
    },
    id: {
      handler: function (value, oldValue) {
        this.iId = this.id;
        this.initAxios();
        this.refresh();
      },
      immediate: true,
    },
  },
  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
    this.fileServerEndPoint = localStorage.getItem('fileServerEndPoint');
  },
  methods: {
    initAxios() {
      apiContract = new ApiContract(this.axios);
    },
    cancel() {
      this.$emit('cancel');
      this.form.resetFields();
      this.content = '';
    },
    /** 数字金额大写转换(可以处理整数,小数,负数) */
    smalltoBIG(n) {
      n = n * 10000;
      let fraction = ['角', '分'];
      let digit = ['零', '壹', '贰', '叁', '肆', '伍', '陆', '柒', '捌', '玖'];
      let unit = [['元', '万', '亿'], ['', '拾', '佰', '仟']];
      let head = n < 0 ? '欠' : '';
      n = Math.abs(n);

      let s = '';

      for (let i = 0; i < fraction.length; i++) {
        s += (digit[Math.floor(n * 10 * Math.pow(10, i)) % 10] + fraction[i]).replace(/零./, '');
      }
      s = s || '整';
      n = Math.floor(n);

      for (let i = 0; i < unit[0].length && n > 0; i++) {
        let p = '';
        for (let j = 0; j < unit[1].length && n > 0; j++) {
          p = digit[n % 10] + unit[1][j] + p;
          n = Math.floor(n / 10);
        }
        s = p.replace(/(零.)*零$/, '').replace(/^$/, '零') + unit[0][i] + s;
      }
      let RMB = head + s.replace(/(零.)*零元/, '元').replace(/(零.)+/g, '零').replace(/^整$/, '零元整');
      return RMB;
    },
    associatedDocument() {
      this.$refs.SmOaContractSelectAssociatedDocumentModal.select();
    },
    async refresh() {
      if (this.pageState === PageState.Add) {
        await this.generatedCode();
      }
      if (this.iId) {

        let response = await apiContract.get(this.iId);
        if (requestIsSuccess(response)) {
          this.record = response.data;
          let _content = this.record.abstract;
          this.content =
            _content == null ? null
              : _content.replace(new RegExp(`src="`, 'g'), `src="${this.fileServerEndPoint}`);
          if (this.record && this.record.underDepartmentId) {
            this.COrganizationId = this.record.underDepartmentId;
          }
          let _contractRltFiles = [];
          if (this.record && this.record.contractRltFiles.length > 0) {
            console.log(this.record.contractRltFiles);
            this.record.contractRltFiles.map(item => {
              let file = item.file;
              if (file) {
                _contractRltFiles.push({
                  id: file.id,
                  name: file.name,
                  size: file.size,
                  type: file.type,
                  url: file.url,
                });
              }
            });
          }
          this.contractRltFiles=_contractRltFiles;
          console.log( this.contractRltFiles);
          this.record.signTime = this.record && this.record.signTime ? moment(this.record.signTime) : null;
          this.$nextTick(() => {
            this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
          });
        }
      }
    },
    save() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          await this.$refs.fileUpload.commit();
          let _content = this.$refs['sc-rich-text-editor'].content();
          let reg = new RegExp(`${this.fileServerEndPoint}`, 'g');
          values.abstract = _content.replace(reg, '');
          let _contractRltFiles = [];
          this.contractRltFiles.map(item => {
            _contractRltFiles.push({
              fileId: item.id,
            });
          });
          let data = {
            ...values,
            contractRltFiles: _contractRltFiles,
          };
          console.log(data);
          let response = null;
          if (this.pageState === PageState.Add) {
            response = await apiContract.create({ ...data });
          }
          if (this.pageState === PageState.Edit) {
            response = await apiContract.update({ ...data, id: this.iId });
          }
          if (requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.$emit('ok');
            this.content=null;
            this.contractRltFiles=[];
            this.form.resetFields();
          }
        }
      });
      this.loading = false;
    },
    // 生成编码
    async generatedCode() {
      let num = '';
      // let response = await apiContract.getMaxCode();
      // if (utils.requestIsSuccess(response) && response.data) {
      //   let result = response.data;
      //   let code = result ? result.code : '';
      //   if (code.length > 5) {
      //     code = code.substring(code.length - 5);
      //   }
      let date = moment().format('YYYY-MM-DD-HH-mm-ss');
      num = date.replaceAll("-", '');
      let code = "HT-" + num;
      this.$nextTick(() => {
        this.form.setFieldsValue({ code: code });
      });
    },
  },
  render() {
    return (
      <div class="sm-oa-contract">
        <a-form form={this.form}>
          <a-row gutter={24}>
            <a-col sm={24} md={24}>
              <a-form-item
                label-col={{ span: 2 }}
                wrapper-col={{ span: 22 }}
                label="合同名称"
              >
                <a-input
                  placeholder={this.pageState == PageState.View ? '' : '请输入合同名称'}
                  disabled={this.pageState == PageState.View}
                  v-decorator={[
                    'name',
                    {
                      initialValue: '',
                      rules: [
                        { required: true, message: '请输入合同名称' , whitespace: true},
                        { max: 50, message: '名称最多输入50字符' },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item
                label="合同编号"
                label-col={{ span: 4 }}
                wrapper-col={{ span: 20 }}
              >
                <a-input
                  placeholder={this.pageState == PageState.View ? '' : '请输入合同编号'}
                  disabled={true}
                  v-decorator={[
                    'code',
                    {
                      initialValue: '',
                      rules: [
                        { required: true, message: '请输入合同编号',whitespace: true },
                        { max: 50, message: '编号最多输入50字符' },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item
                label="签订时间"
                label-col={{ span: 4 }}
                wrapper-col={{ span: 20 }}
              >
                <a-date-picker
                  placeholder={this.pageState == PageState.View ? '' : '请选择签订时间'}
                  disabled={this.pageState == PageState.View}
                  showTime
                  style="width:100%"
                  format="YYYY-MM-DD HH:mm:ss"
                  onChange={(date, dateString) => {

                  }}
                  v-decorator={[
                    'signTime',
                    {
                      initialValue: null,
                      rules: [
                        {
                          required: true,
                          message: '请选择签订时间',
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item
                label="合同甲方"
                label-col={{ span: 4 }}
                wrapper-col={{ span: 20 }}
              >
                <a-input
                  placeholder={this.pageState == PageState.View ? '' : '请输入甲方名称'}
                  disabled={this.pageState == PageState.View}
                  v-decorator={[
                    'partyA',
                    {
                      initialValue: '',
                      rules: [
                        { required: true, message: '请输入甲方名称',whitespace: true },
                        { max: 50, message: '名称最多输入50字符' },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={12} md={12}>
              <a-form-item
                label="合同乙方"
                label-col={{ span: 4 }}
                wrapper-col={{ span: 20 }}
              >
                <a-input
                  placeholder={this.pageState == PageState.View ? '' : '请输入乙方名称'}
                  disabled={this.pageState == PageState.View}
                  v-decorator={[
                    'partyB',
                    {
                      initialValue: '',
                      rules: [
                        { required: true, message: '请输入乙方名称' ,whitespace: true},
                        { max: 50, message: '名称最多输入50字符' },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item
                label="合同丙方"
                label-col={{ span: 4 }}
                wrapper-col={{ span: 20 }}
              >
                <a-input
                  placeholder={this.pageState == PageState.View ? '' : '请输入丙方名称'}
                  disabled={this.pageState == PageState.View}
                  v-decorator={[
                    'partyC',
                    {
                      initialValue: '',
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item
                label="主办部门"
                label-col={{ span: 4 }}
                wrapper-col={{ span: 20 }}
              >
                <OrganizationTreeSelect
                  placeholder={this.pageState == PageState.View ? '' : '请选择主办部门'}
                  axios={this.axios}
                  disabled={this.pageState == PageState.View}
                  onChange={value => {

                  }}
                  v-decorator={[
                    'hostDepartmentId',
                    {
                      initialValue: null,
                      rules: [
                        { required: true, message: '请选择主办部门'},
                        { max: 50, message: '名称最多输入50字符' },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item
                label="承办部门"
                label-col={{ span: 4 }}
                wrapper-col={{ span: 20 }}
              >
                <OrganizationTreeSelect
                  placeholder={this.pageState == PageState.View ? '' : '请选择承办部门'}
                  axios={this.axios}
                  disabled={this.pageState == PageState.View}
                  onChange={value => {
                    this.COrganizationId = value;
                    this.form.setFieldsValue({ undertakerId: undefined });
                  }}
                  v-decorator={[
                    'underDepartmentId',
                    {
                      initialValue: null,
                      rules: [
                        { required: true, message: '请选择承办部门' },
                        { max: 50, message: '名称最多输入50字符' },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item
                label="承办人"
                label-col={{ span: 4 }}
                wrapper-col={{ span: 20 }}
              >
                <SmSystemUserSelect
                  placeholder={this.pageState == PageState.View ? '' : '请选择承办人'}
                  axios={this.axios}
                  organizationId={this.COrganizationId}
                  disabled={this.pageState == PageState.View}
                  onChange={value => {

                  }}
                  v-decorator={[
                    'undertakerId',
                    {
                      initialValue: undefined,
                      rules: [
                        { required: true, message: '请选择承办人'},
                      ],
                    },
                  ]}
                >
                  <a-icon slot="rightIcon" type="smile" />
                </SmSystemUserSelect>


              </a-form-item>
            </a-col>

            <a-col sm={12} md={12}>
              <a-form-item
                label="合同金额(万)"
                label-col={{ span: 4 }}
                wrapper-col={{ span: 20 }}
              >
                <a-input-number
                  style="width:100%"
                  min={0}
                  placeholder={this.pageState == PageState.View ? '' : '请输入合同金额'}
                  disabled={this.pageState == PageState.View}
                  onChange={async (item) => {
                    let str = /^(([1-9][0-9]*)|(([0]\.\d{1,6}|[1-9][0-9]*\.\d{1,6})))$/;
                    let RMB = item && str.test(item) ? await this.smalltoBIG(item) : null;
                    this.form.setFieldsValue({ amountWords: RMB });
                  }}
                  v-decorator={[
                    'amount',
                    {
                      initialValue: null,
                      rules: [
                        { required: true, message: '请输入合同金额'},
                        {
                          pattern: /^(([1-9][0-9]*)|(([0]\.\d{1,6}|[1-9][0-9]*\.\d{1,6})))$/,
                          message: "金额格式错误",
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item
                label="金额大写"
                label-col={{ span: 4 }}
                wrapper-col={{ span: 20 }}
              >
                <a-input
                  placeholder={this.pageState == PageState.View ? '' : '请输入金额大写'}
                  disabled
                  v-decorator={[
                    'amountWords',
                    {
                      initialValue: null,
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item
                label="成本预算(万)"
                label-col={{ span: 4 }}
                wrapper-col={{ span: 20 }}
              >
                <a-input-number
                  style="width:100%"
                  min={0}
                  placeholder={this.pageState == PageState.View ? '' : '请输入成本预算'}
                  disabled={this.pageState == PageState.View}
                  v-decorator={[
                    'budge',
                    {
                      initialValue: null,
                      rules: [
                        { required: true, message: '请输入成本预算' },
                        {
                          pattern: /^(([1-9][0-9]*)|(([0]\.\d{1,6}|[1-9][0-9]*\.\d{1,6})))$/,
                          message: "金额格式错误",
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item
                label="合同类型"
                label-col={{ span: 4 }}
                wrapper-col={{ span: 20 }}
              >
                <SmSystemDataDictionaryTreeSelect
                  axios={this.axios}
                  disabled={this.pageState == PageState.View}
                  groupCode={'ProjectContractType'}
                  placeholder={this.pageState == PageState.View ? '' : '请选择合同类型'}
                  v-decorator={
                    [
                      'typeId',
                      {
                        initialValue: undefined,
                        rules: [{ required: true, message: '请选择合同类型' }],
                      },
                    ]
                  }
                />
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <a-form-item
                label="合同摘要"
                label-col={{ span: 2 }}
                wrapper-col={{ span: 22 }}
              >
                <RichTextEditor
                  disabled={this.pageState == PageState.View}
                  ref="sc-rich-text-editor"
                  axios={this.axios}
                  value={this.content}
                />
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <a-form-item
                label="合同附件"
                label-col={{ span: 2 }}
                wrapper-col={{ span: 22 }}
              >
                <SmFileUpload
                  ref="fileUpload"
                  disabled={this.pageState == PageState.View}
                  mode={this.status == PageState.View?"view":"edit"}
                  axios={this.axios}
                  multiple
                  onChange={item => console.log(item)}
                  placeholder={this.pageState == PageState.View ? '' : '请选择合同附件'}
                  onSelected={(item) => {
                    this.contractRltFiles=item; 
                  }}
                  fileList={this.contractRltFiles}
                /> 
                {/* <SmFileManageSelect
                  disabled={this.pageState == PageState.View}
                  axios={this.axios}
                  height={73}
                  multiple
                  onChange={item => console.log(item)}
                  placeholder={this.pageState == PageState.View ? '' : '请选择合同附件'}
                  enableDownload={true}
                  v-decorator={[
                    'contractRltFiles',
                    {
                      initialValue: [],
                      // rules: [{ required: true, message: '请选择合同附件' }],
                    },
                  ]}
                /> */}
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <a-form-item
                label="合同对方信息"
                label-col={{ span: 2 }}
                wrapper-col={{ span: 22 }}
              >
                <a-textarea
                  placeholder={this.pageState == PageState.View ? '' : '请输入合同对方信息'}
                  disabled={this.pageState == PageState.View}
                  v-decorator={[
                    'otherPartInfo',
                    {
                      initialValue: '',
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            {/* <a-col sm={24} md={24}>
              <a-form-item
                label="关联文档"
                label-col={{ span: 2 }}
                wrapper-col={{ span: 22 }}
              >
                <span
                  class="relate-document"
                  onClick={() => this.associatedDocument()}
                >
                  <a-icon type="file-text" />
                选择关联文档
                </span>
              </a-form-item>
            </a-col> */}
            <a-col sm={24} md={24}>
              <a-col span={20}></a-col>
              <a-col span={4} >
                <a-button
                  style="margin-right: 15px"
                  onClick={() => this.cancel()}
                >
                  关闭
                </a-button>
                {
                  this.pageState == PageState.View ?
                    <a-button
                      type="primary"
                      loading={this.loading}
                      onClick={() => this.cancel()}
                    >
                      返回
                    </a-button> :
                    <a-button
                      type="primary"
                      loading={this.loading}
                      onClick={this.save}
                    >
                      保存
                    </a-button>
                }

              </a-col>
            </a-col>
            <a-col sm={12} md={12}></a-col>
          </a-row>
        </a-form>
        {/* 关联文档选择模态框 */}
        <SmOaContractSelectAssociatedDocumentModal
          ref="SmOaContractSelectAssociatedDocumentModal"
          axios={this.axios}
        />
      </div >
    );
  },
};