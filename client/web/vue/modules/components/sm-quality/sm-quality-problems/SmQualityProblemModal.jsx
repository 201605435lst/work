import * as utils from '../../_utils/utils';
import { getQualityProblemType, getQualityProblemLevel, requestIsSuccess, compareDate } from '../../_utils/utils';
import { ModalStatus, MemberType } from '../../_utils/enum';
import ApiProblem from '../../sm-api/sm-quality/QualityProblem';
import SmSystemMemberSelect from '../../sm-system/sm-system-member-select/SmSystemMemberSelect';
import SmSystemOrganizationTreeSelect from '../../sm-system/sm-system-organization-tree-select';
import './style/index';
import moment from 'moment';
import SmD3ModalSelect from '../../sm-d3/sm-d3-modal-select/SmD3ModalSelect';
import SmFileUpload from '../../sm-file/sm-file-upload/SmFileUpload';
import DataDictionary from '../../sm-system/sm-system-data-dictionary-tree-select';
import DataEnum from './src/SmSystemDataEnumTreeSelect';
import ApiQualityProblemLibrary from '../../sm-api/sm-quality/QualityProblemLibrary';
import ApiOrganization from '../../sm-api/sm-system/Organization';
let apiProblem = new ApiProblem();
let apiQualityProblemLibrary = new ApiQualityProblemLibrary();
let apiOrganization = new ApiOrganization();

// 定义表单字段常量
const formFields = [
  'title',  /// 问题标题
  'type',  /// 问题类型
  'level',  /// 问题等级
  'professionId',  /// 所属专业
  'checkTime',/// 检查时间
  'limitTime',/// 限制整改时间
  'checkUnitId',/// 检查单位
  'checkUnitName',//检查单位名称
  'checkerId', /// 检查人
  'responsibleUnit', /// 责任单位
  'responsibleOrganizationId',/// 责任部门
  'responsibleUserId', /// 责任人
  'ccUsers',//抄送人
  'verifierId',/// 问题验证人
  // 'qualityProblemRltEquipment',/// 关联模型
  'content', /// 问题内容
  'suggestion',  /// 整改措施，整改意见
  'equipmentIds',
];
export default {
  name: 'SmQualityProblemModal',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {}, // 表单绑定的对象,
      loading: false, //确定按钮加载状态
      files: [],//附件
      contentData: [],
      checkUnitDisabled: false,
      checkUnitName: null,
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
    let responses = await apiQualityProblemLibrary.getList({ isAll: true });
    if (requestIsSuccess(responses) && responses.data && responses.data.items) {
      this.contentData = responses.data.items;
    }
  },
  methods: {
    initAxios() {
      apiProblem = new ApiProblem(this.axios);
      apiQualityProblemLibrary = new ApiQualityProblemLibrary(this.axios);
      apiOrganization = new ApiOrganization(this.axios);
    },
    add() {
      this.status = ModalStatus.Add;
      this.$nextTick(() => {
        this.form.resetFields();
        this.form.setFieldsValue();
      });
    },

    //根据id获取检测机构的名称
    async getOrganization(organizationId) {
      if (organizationId) {
        let response = await apiOrganization.get(organizationId);
        this.form.setFieldsValue({
          checkUnitName: response.data.name,
        });
        this.checkUnitName = this.form.getFieldValue('checkUnitName').replace(/[, ]/g, '');
      } else {
        this.form.setFieldsValue({
          checkUnitName: null,
        });
        this.checkUnitName = null;
      }
    },

    async edit(record) {
      this.status = ModalStatus.Edit;
      this.record = { ...record };
      let _checker = [];
      let _responsibleUser = [];
      let _verifier = [];
      let _ccUsers = [];
      let _equipmentIds = [];
      if (this.record && this.record.equipments && this.record.equipments.length > 0) {
        this.record.equipments.map(item => {
          _equipmentIds.push(
            item.equipmentId,
          );
        });
      }
      _checker.push({
        id: record ? record.checkerId : undefined,
        type: MemberType.User,
      });
      _verifier.push({
        id: record ? record.verifierId : undefined,
        type: MemberType.User,
      });
      _responsibleUser.push({
        id: record ? record.responsibleUserId : undefined,
        type: MemberType.User,
      });
      if (this.record && this.record.ccUsers && this.record.ccUsers.length > 0) {
        this.record.ccUsers.map(item => {
          if (item && item.ccUserId) {
            _ccUsers.push({
              type: MemberType.User,
              id: item.ccUserId,
            });
          }
        });
      }
      //构造附件
      let _files = [];
      if (this.record && this.record.files.length > 0) {
        this.record.files.map(item => {
          let file = item.file;
          if (file) {
            _files.push({
              id: file.id,
              name: file.name,
              size: file.size,
              type: file.type,
              url: file.url,
            });
          }
        });
      }
      this.files = _files;
      this.record.checkerId = _checker;
      this.record.verifierId = _verifier;
      this.record.responsibleUserId = _responsibleUser;
      this.record.checkTime = this.record && this.record.checkTime ? moment(this.record.checkTime) : undefined;
      this.record.limitTime = this.record && this.record.limitTime ? moment(this.record.limitTime) : undefined;
      this.record.ccUsers = _ccUsers;
      this.record.equipmentIds = _equipmentIds;
      this.record.qualityProblemRltEquipment = null;
      this.$nextTick(() => {
        this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
      });
    },
    // 关闭模态框
    close() {
      this.form.resetFields();
      this.status = ModalStatus.Hide;
      this.files = [];
      this.record = null;
      this.checkUnitDisabled = false;
      this.checkUnitName = null;
    },

    // 数据提交
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err && compareDate(values.checkTime, values.limitTime) == '>') {
          this.$message.warning('“限期时间”不应该早于“检查时间”');
        } else {
          if (this.files.length > 0) {
            _ = await this.$refs.fileUpload.commit(); // 提交文件
          }
          let _files = [];
          this.files.map(item => {
            _files.push({
              fileId: item.id,
            });
          });

          let _ccUsers = [];

          values.ccUsers.map(item => {
            _ccUsers.push({
              ccUserId: item.id,
            });
          });
          let _code = this.status === ModalStatus.Add ? this.generatedCode() : this.record.code;
          let _equipmentIds = [];
          values.equipmentIds.map(item => {
            _equipmentIds.push({
              equipmentId: item,
            });
          });
          let data = {
            ...values,
            checkerId: Object.assign(...values.checkerId) ? Object.assign(...values.checkerId).id : '',
            responsibleUserId: Object.assign(...values.responsibleUserId) ? Object.assign(...values.responsibleUserId).id : '',
            verifierId: Object.assign(...values.verifierId) ? Object.assign(...values.verifierId).id : '',
            files: _files,
            ccUsers: _ccUsers,
            code: _code,
            equipments: _equipmentIds,
          };
          let response = null;
          if (this.status === ModalStatus.Add) {
            //添加
            response = await apiProblem.create(data);
          } else if (this.status === ModalStatus.Edit) {
            // 编辑
            response = await apiProblem.update({ id: this.record.id, ...data });
          } else {
            this.close();
          }
          if (requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.close();
            this.$emit('success');
          }
        }
      });
      this.loading = false;
    },
    // 生成编码
    generatedCode() {
      let num = '';
      let date = moment().format('YYYY-MM-DD-HH-mm-ss');
      num = date.replaceAll("-", '');
      let code = "SF-" + num;
      return code;
    },
    onContent(e) {
      this.form.setFieldsValue({ ['content']: this.contentData[e.key].content, ['suggestion']: this.contentData[e.key].measures });
    },
  },
  render() {
    let contentData = [];
    if (this.contentData && this.contentData.length > 0) {
      for (let index = 0; index < this.contentData.length; index++) {
        contentData.push(
          <a-menu-item class="quality-problem-menu-item" key={index}>{this.contentData[index].content}</a-menu-item>, <a-menu-divider />,
        );
      }
    } else {
      contentData = false;
    }
    return (
      <a-modal
        class="sm-quality-problem-modal"
        title={`${this.title}质量问题`}
        visible={this.visible}
        okText="确定"
        onCancel={this.close}
        confirmLoading={this.loading}
        destroyOnClose={true}
        okText={this.status !== ModalStatus.View ? "保存" : '确定'}
        onOk={this.ok}
        width={900}
      >
        <a-form form={this.form}>
          <a-row gutter={24}>
            <a-col sm={12} md={12}>
              <a-form-item label-col={{ span: 6 }} wrapper-col={{ span: 18 }} label="问题标题">
                <a-input
                  placeholder={this.status == ModalStatus.View ? '' : '请输入问题标题'}
                  disabled={this.status == ModalStatus.View}
                  v-decorator={[
                    'title',
                    {
                      initialValue: '',
                      rules: [
                        {
                          required: true,
                          message: '问题标题不能为空',
                          whitespace: true,
                        },
                      ],
                    },
                  ]}
                >
                </a-input>
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="问题类型" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <DataEnum
                  disabled={this.status == ModalStatus.View}
                  placeholder="请选择问题类型"
                  enum="QualityProblemType"
                  utils="getQualityProblemType"
                  v-decorator={[
                    'type',
                    {
                      initialValue: undefined,
                      rules: [
                        {
                          required: true,
                          message: '请选择问题类型',
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="问题等级" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <DataEnum
                  disabled={this.status == ModalStatus.View}
                  placeholder="请选择问题等级"
                  enum="QualityProblemLevel"
                  utils="getQualityProblemLevel"
                  v-decorator={[
                    'level',
                    {
                      initialValue: undefined,
                      rules: [
                        {
                          required: true,
                          message: '请选择问题等级',
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="所属专业" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <DataDictionary
                  disabled={this.status == ModalStatus.View}
                  placeholder={this.status == ModalStatus.View ? null : "请选择所属专业"}
                  axios={this.axios}
                  groupCode="Profession"
                  v-decorator={[
                    'professionId',
                    {
                      initialValue: null,
                      rules: [
                        {
                          required: true,
                          message: '请选择所属专业',
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="检查时间" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-date-picker
                  placeholder={this.status == ModalStatus.View ? '' : '请选择检查时间'}
                  disabled={this.status == ModalStatus.View}
                  style="width:100%"
                  v-decorator={[
                    'checkTime',
                    {
                      initialValue: null,
                      rules: [
                        {
                          required: true,
                          message: '检查时间不能为空',
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="限期时间" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-date-picker
                  placeholder={this.status == ModalStatus.View ? '' : '请选择限期时间'}
                  disabled={this.status == ModalStatus.View}
                  style="width:100%"
                  v-decorator={[
                    'limitTime',
                    {
                      initialValue: null,
                      rules: [
                        {
                          required: true,
                          message: '限期时间不能为空',
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="检查单位" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <SmSystemOrganizationTreeSelect
                  axios={this.axios}
                  placeholder={this.status == ModalStatus.View ? '' : '请选择检查单位'}
                  disabled={this.checkUnitDisabled || this.status == ModalStatus.View}
                  onChange={value => {
                    this.getOrganization(value);
                  }}
                  v-decorator={[
                    'checkUnitId',
                    {
                      initialValue: null,
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item
                label="检查单位名称"
                label-col={{ span: 6 }}
                wrapper-col={{ span: 18 }}
              >
                <a-input
                  placeholder={this.status == ModalStatus.View ? '' : '请输入检查单位名称'}
                  disabled={this.checkUnitName != null || this.status == ModalStatus.View}
                  onChange={e => {
                    if (e.target.value == '') {
                      this.checkUnitDisabled = false;
                    } else {
                      this.checkUnitDisabled = true;
                    }
                  }}
                  v-decorator={[
                    'checkUnitName',
                    {
                      initialValue: null,
                      rules: [
                        { required: true, message: '请输入检测机构名称' },
                        { max: 50, message: '名称最多输入50字符' },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="责任部门" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <SmSystemOrganizationTreeSelect
                  axios={this.axios}
                  disabled={this.status == ModalStatus.View}
                  placeholder={this.status === ModalStatus.View ? '' : '请选择责任部门'}
                  v-decorator={[
                    'responsibleOrganizationId',
                    {
                      initialValue: null,
                      // rules: [{ required: true, message: '责任部门不能为空！' }],
                    },
                  ]}

                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="责任单位" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-input
                  placeholder={this.status == ModalStatus.View ? '' : '请输入责任单位'}
                  disabled={this.status == ModalStatus.View}
                  v-decorator={[
                    'responsibleUnit',
                    {
                      initialValue: '',
                      rules: [
                        {
                          required: true,
                          message: '责任单位不能为空',
                          whitespace: true,
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12} style='height: 64px;'>
              <a-form-item label="检查人" label-col={{ span: 6 }} wrapper-col={{ span: 18 }} >
                <SmSystemMemberSelect
                  height={32}
                  shouIconSelect={true}
                  showUserTab={true}
                  userMultiple={false}
                  bordered={true}
                  simple={true}
                  placeholder={this.status == ModalStatus.View ? '' : '请选择检查人'}
                  axios={this.axios}
                  disabled={this.status == ModalStatus.View}
                  onChange={value => {
                    console.log(Object.assign(...value));
                  }}
                  v-decorator={[
                    'checkerId',
                    {
                      initialValue: undefined,
                      rules: [{ required: true, message: '检查人不能为空' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12} style='height: 64px;'>
              <a-form-item label="责任人" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <SmSystemMemberSelect
                  height={32}
                  userMultiple={false}
                  showUserTab={true}
                  shouIconSelect={true}
                  bordered={true}
                  simple={true}
                  placeholder={this.status == ModalStatus.View ? '' : '请选择责任人'}
                  axios={this.axios}
                  disabled={this.status == ModalStatus.View}
                  onChange={value => {
                    console.log(value);
                  }}
                  v-decorator={[
                    'responsibleUserId',
                    {
                      initialValue: undefined,
                      rules: [{ required: true, message: '责任人不能为空' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12} style='height: 64px;'>
              <a-form-item label="验证人" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <SmSystemMemberSelect
                  height={32}
                  userMultiple={false}
                  shouIconSelect={true}
                  showUserTab={true}
                  bordered={true}
                  simple={true}
                  placeholder={this.status == ModalStatus.View ? '' : '请选择验证人'}
                  axios={this.axios}
                  disabled={this.status == ModalStatus.View}
                  v-decorator={[
                    'verifierId',
                    {
                      initialValue: undefined,
                      rules: [{ required: true, message: '验证人不能为空' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12} >
              <a-form-item label="抄送人" label-col={{ span: 6 }} wrapper-col={{ span: 18 }} class="member-multiple-select" >
                <SmSystemMemberSelect
                  height={32}
                  userMultiple={true}
                  showUserTab={true}
                  shouIconSelect={true}
                  bordered={true}
                  simple={true}
                  placeholder={this.status == ModalStatus.View ? '' : '请选择抄送人'}
                  axios={this.axios}
                  disabled={this.status == ModalStatus.View}
                  onChange={value => {
                    console.log(value);
                  }}
                  v-decorator={[
                    'ccUsers',
                    {
                      initialValue: undefined,
                      rules: [{ required: true, message: '抄送人不能为空' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={24}>
              <a-form-item label="关联模型" label-col={{ span: 3 }} wrapper-col={{ span: 21 }}>
                <SmD3ModalSelect
                  placeholder={this.isShow ? '' : '请选择关联模型'}
                  disabled={this.status == ModalStatus.View}
                  axios={this.axios}
                  multiple
                  size="small"
                  v-decorator={[
                    'equipmentIds',
                    {
                      initialValue: [],
                      rules: [
                        {
                          required: true,
                          message: '请选择关联模型',
                        },
                      ],
                    },
                  ]}
                  onChange={this.dSelect}
                />
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <a-form-item label="问题描述" label-col={{ span: 3 }} wrapper-col={{ span: 21 }}>
                <a-dropdown trigger={['click']} getPopupContainer={Node => { return Node.parentNode; }}>
                  <a class="ant-dropdown-link">
                    <a-textarea
                      rows="3"
                      placeholder={this.status == ModalStatus.View ? '' : '请输入问题描述'}
                      disabled={this.status == ModalStatus.View}
                      v-decorator={[
                        'content',
                        {
                          initialValue: null,
                        },
                      ]}
                    />
                  </a>
                  {
                    contentData ?
                      <a-menu slot="overlay" onClick={e => this.onContent(e)}>
                        {contentData}
                      </a-menu> : ''

                  }
                </a-dropdown>
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <a-form-item label="整改意见" label-col={{ span: 3 }} wrapper-col={{ span: 21 }}>
                <a-textarea
                  rows="3"
                  placeholder={this.status == ModalStatus.View ? '' : '请输入整改意见'}
                  disabled={this.status == ModalStatus.View}
                  v-decorator={[
                    'suggestion',
                    {
                      initialValue: null,
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <a-form-item label="资料上传" label-col={{ span: 3 }} wrapper-col={{ span: 21 }}>
                <SmFileUpload
                  ref="fileUpload"
                  disabled={this.status == ModalStatus.View}
                  mode={this.status == ModalStatus.View ? "view" : "edit"}
                  axios={this.axios}
                  multiple
                  placeholder={this.status == ModalStatus.View ? '' : '影像图文资料上传'}
                  onSelected={(item) => {
                    this.files = item;
                  }}
                  fileList={this.files}
                />
              </a-form-item>
            </a-col>
          </a-row>
        </a-form>
      </a-modal>
    );
  },
};

