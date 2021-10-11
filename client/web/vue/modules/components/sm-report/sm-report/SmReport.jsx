import './style';
import { PageState } from '../../_utils/enum';
import * as utils from '../../_utils/utils';
import { requestIsSuccess, getReportTypeTitle } from '../../_utils/utils';
import ApiReport from '../../sm-api/sm-report/report';
import RichTextEditor from '../../sm-file/sm-file-text-editor/SmFileTextEditor';
import SmFileManageSelect from '../../sm-file/sm-file-manage-select';
import SmReportSelectProjectModal from './src/SmReportSelectProjectModal';
import { ReportType, MemberType } from '../../_utils/enum';
import SmSystemMemberSelect from '../../sm-system/sm-system-member-select/SmSystemMemberSelect';
import moment from 'moment';
import SmFileUpload from '../../sm-file/sm-file-upload/SmFileUpload';


let apiReport = new ApiReport();
// 定义表单字段常量
const formFields = [
  'type', //类型
  'date_date',
  'month_date',
  'week_date',
  'title', //标题
  'plan', //工作计划
  'workRecord', //工作记录
  // 'reportRltFiles', //附件
  'reportRltUsers', //通知人员
];
export default {
  name: 'SmReport',
  props: {
    signalr: { type: Function, default: null },
    axios: { type: Function, default: null },
    id: { type: String, default: null },
    permissions: { type: Array, default: () => [] },
    pageState: { type: String, default: PageState.Add }, // 页面状态
  },
  data() {
    return {
      typeReport: null, //报告类型
      form: {}, // 表单
      record: null, // 表单绑的对象,
      iId: null, //汇报id
      organizationId: null, //组织机构
      loading: false,
      fileServerEndPoint: '', //文件服务请求头
      summary: null, //工作总结
      recordProject: null, //项目
      reportRltFiles:[],
    };
  },
  computed: {},
  watch: {
    id: {
      handler: function(value, oldValue) {
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
      apiReport = new ApiReport(this.axios);
    },
    cancel() {
      this.$emit('cancel');
      this.form.resetFields();
      this.summary = null;
      this.organizationId = null;
    },
    selectProject() {
      this.$refs.SmReportSelectProjectModal.select();
    },
    async refresh() {
      if (this.iId) {
        let response = await apiReport.get(this.iId);
        if (requestIsSuccess(response)) {
          this.record = response.data;
          let _summary = this.record.summary;
          this.summary =
          _summary == null
            ? null
            : _summary.replace(new RegExp(`src="`, 'g'), `src="${this.fileServerEndPoint}`);
          //构造附件
          let _reportRltFiles = [];
          if (this.record && this.record.reportRltFiles.length > 0) {
            this.record.reportRltFiles.map(item => {
              let file = item.file;
              if (file) {
                _reportRltFiles.push({
                  id: file.id,
                  name: file.name,
                  size: file.size,
                  type: file.type,
                  url: file.url,
                });
              }
            });
          }
          //构造人员
          let _reportRltUsers = [];
          if (this.record && this.record.reportRltUsers.length > 0) {
            this.record.reportRltUsers.map(item => {
              if (item && item.id && item.user) {
                _reportRltUsers.push({
                  type: MemberType.User,
                  id: item.userId,
                });
              }
            });
          }
          this.record.reportRltUsers = _reportRltUsers;
          this.reportRltFiles = _reportRltFiles;
          this.record.date_date =
            this.record && this.record.date && this.record.type == ReportType.DayReport
              ? moment(this.record.date)
              : null;
          this.record.month_date =
            this.record && this.record.date && this.record.type == ReportType.MonthReport
              ? moment(this.record.date)
              : null;
          this.record.week_date =
            this.record && this.record.date && this.record.type == ReportType.WeekReport
              ? moment(this.record.date)
              : null;
          this.$nextTick(() => {
            this.form.setFieldsValue({ ...utils.objFilterProps(this.record, formFields) });
          });
        }
      }
      this.typeReport = this.record && this.record.type ? this.record.type : '';
    },

    save() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let _summary = this.$refs['sc-rich-text-editor'].content();
          let reg = new RegExp(`${this.fileServerEndPoint}`, 'g');
          values.summary = _summary.replace(reg, '');
          let _reportRltFiles = [];
          let _reportRltUsers = [];
          this.reportRltFiles.map(item => {
            _reportRltFiles.push({
              fileId: item.id,
            });
          });
          if (values) {
            values.reportRltUsers.map(item => {
              _reportRltUsers.push({
                userId: item.id,
              });
            });
          }
          let data = {
            ...values,
            reportRltFiles: _reportRltFiles,
            reportRltUsers: _reportRltUsers,
            date: values.date_date
              ? values.date_date
              : values.month_date
                ? values.month_date
                : values.week_date,
          };
          let response = null;
          if (this.pageState === PageState.Add) {
            response = await apiReport.create({ ...data });
          }
          if (this.pageState === PageState.Edit) {
            response = await apiReport.update({ ...data, id: this.iId });
          }
          if (requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.$emit('ok');
            this.summary = null;
            this.reportRltFiles=[];
            this.organizationId = null;
            this.form.resetFields();
          }
        }
      });
      this.loading = false;
    },
  },
  render() {
    let Options = [];
    for (let item in ReportType) {
      Options.push(
        <a-select-option key={ReportType[item]}>
          {getReportTypeTitle(ReportType[item])}
        </a-select-option>,
      );
    }
    return (
      <div class="sm-report">
        <a-form form={this.form}>
          <a-row gutter={24}>
            <a-col sm={12} md={12}>
              <a-form-item label="汇报类型" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <a-select
                  allowClear={false}
                  placeholder={this.pageState == PageState.View ? '' : '请选择汇报类型'}
                  disabled={this.pageState == PageState.View}
                  onChange={val => {
                    this.typeReport = val;
                    console.log(this.typeReport);
                  }}
                  v-decorator={[
                    'type',
                    {
                      initialValue: undefined,
                      rules: [{ required: true, message: '请选择汇报类型' }],
                    },
                  ]}
                >
                  {Options}
                </a-select>
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="日期选择" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                {!this.typeReport || this.typeReport == ReportType.DayReport ? (
                  <a-date-picker
                    placeholder={this.pageState == PageState.View ? '' : '请选择日期'}
                    disabled={this.pageState == PageState.View}
                    style="width:100%"
                    onChange={(date, dateString) => {
                      console.log(date);
                    }}
                    v-decorator={[
                      'date_date',
                      {
                        initialValue: null,
                        rules: [
                          {
                            required: true,
                            message: '请选择汇报时间',
                          },
                        ],
                      },
                    ]}
                  />
                ) : (
                  ''
                )}
                {this.typeReport == ReportType.MonthReport ? (
                  <a-month-picker
                    placeholder={this.pageState == PageState.View ? '' : '请选择日期'}
                    disabled={this.pageState == PageState.View}
                    style="width:100%"
                    onChange={(date, dateString) => {
                      console.log(date);
                    }}
                    v-decorator={[
                      'month_date',
                      {
                        initialValue: null,
                        rules: [
                          {
                            required: true,
                            message: '请选择汇报时间',
                          },
                        ],
                      },
                    ]}
                  />
                ) : (
                  ''
                )}
                {this.typeReport == ReportType.WeekReport ? (
                  <a-week-picker
                    placeholder={this.pageState == PageState.View ? '' : '请选择日期'}
                    disabled={this.pageState == PageState.View}
                    style="width:100%"
                    onChange={(date, dateString) => {
                      console.log(date);
                    }}
                    v-decorator={[
                      'week_date',
                      {
                        initialValue: null,
                        rules: [
                          {
                            required: true,
                            message: '请选择汇报时间',
                          },
                        ],
                      },
                    ]}
                  >
                    <a-icon slot="suffixIcon" type="calendar" />
                  </a-week-picker>
                ) : (
                  ''
                )}
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <a-form-item label="标题" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                <a-input
                  placeholder={this.pageState == PageState.View ? '' : '请输入标题'}
                  disabled={this.pageState == PageState.View}
                  v-decorator={[
                    'title',
                    {
                      initialValue: '',
                      rules: [
                        { required: true, message: '请输入标题', whitespace: true },
                        { max: 50, message: '标题最多输入30字符' },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={24}>
              <a-form-item label="工作计划" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                <a-textarea
                  rows="3"
                  placeholder={this.pageState == PageState.View ? '' : '请输入工作计划'}
                  disabled={this.pageState == PageState.View}
                  v-decorator={[
                    'plan',
                    {
                      initialValue: null,
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <a-form-item label="工作记录" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                <a-textarea
                  rows="3"
                  placeholder={this.pageState == PageState.View ? '' : '请输入工作记录'}
                  disabled={this.pageState == PageState.View}
                  v-decorator={[
                    'workRecord',
                    {
                      initialValue: null,
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <a-form-item label="工作总结" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                <RichTextEditor
                  disabled={this.pageState == PageState.View}
                  ref="sc-rich-text-editor"
                  axios={this.axios}
                  value={this.summary}
                />
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <a-form-item label="附件" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                <SmFileUpload
                  ref="fileUpload"
                  disabled={this.pageState == PageState.View}
                  mode={this.status == PageState.View?"view":"edit"}
                  axios={this.axios}
                  multiple
                  onChange={item => console.log(item)}
                  placeholder={this.pageState == PageState.View ? '' : '请选择附件'}
                  onSelected={(item) => {
                    this.reportRltFiles=item; 
                  }}
                  fileList={this.reportRltFiles}
                /> 
                {/* <SmFileManageSelect
                  disabled={this.pageState == PageState.View}
                  axios={this.axios}
                  height={73}
                  multiple
                  onChange={() => {}}
                  placeholder={this.pageState == PageState.View ? '' : '请选择附件'}
                  enableDownload={true}
                  v-decorator={[
                    'reportRltFiles',
                    {
                      initialValue: [],
                    },
                  ]}
                /> */}
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <a-form-item label="通知人员" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                <SmSystemMemberSelect
                  showUserTab={true}
                  bordered={true}
                  placeholder={this.pageState == PageState.View ? '' : '请选择'}
                  axios={this.axios}
                  disabled={this.pageState == PageState.View}
                  onChange={value => {
                    console.log(value);
                  }}
                  v-decorator={[
                    'reportRltUsers',
                    {
                      initialValue: [],
                      rules: [{ required: true, message: '请选择通知人员' }],
                    },
                  ]}
                ></SmSystemMemberSelect>
              </a-form-item>
            </a-col>
            <a-col sm={24} md={24}>
              <a-col span={20}></a-col>
              <a-col span={4}>
                <a-button style="margin-right: 15px" onClick={() => this.cancel()}>
                  关闭
                </a-button>
                {this.pageState == PageState.View ? (
                  <a-button type="primary" loading={this.loading} onClick={() => this.cancel()}>
                    返回
                  </a-button>
                ) : (
                  <a-button type="primary" loading={this.loading} onClick={this.save}>
                    保存
                  </a-button>
                )}
              </a-col>
            </a-col>
            <a-col sm={12} md={12}></a-col>
          </a-row>
        </a-form>
       
      </div>
    );
  },
};
