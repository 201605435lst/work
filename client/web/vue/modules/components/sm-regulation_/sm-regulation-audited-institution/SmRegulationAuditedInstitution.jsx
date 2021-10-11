import './style';
import * as utils from '../../_utils/utils';
import { PageState } from '../../_utils/enum';
import SmSystemMemberSelect from '../../sm-system/sm-system-member-select';
import SealTreeSelect from '../../sm-oa/sm-oa-seals-select';
import LabelTreeSelect from '../sm-regulation-label-tree-select';
import ApiInstitution from '../../sm-api/sm-regulation/Institution';
import { requestIsSuccess } from '../../_utils/utils';
import moment from 'moment';
import SmFileManageSelect from '../../sm-file/sm-file-manage-select';

let apiInstitution = new ApiInstitution();
// 定义表单字段常量
const formFields = ['workflowId', 'workflowState', 'suggestion', 'suggestionInput', 'sealId'];

export default {
  name: 'SmRegulationAuditedInstitution',
  props: {
    axios: { type: Function, default: null },
    institutionId: { type: String, default: null },
    pageState: { type: String, default: PageState.Add }, // 页面状态
  },

  data() {
    return {
      form: {}, // 表单
      record: null, // 表单绑的对象,
      loading: false,
      content: null,
      show: false, //是否显示新闻发布类别
      showAuthority: true, //是否显示权限配置
      showFlow: true, //是否显示流程设置
      showAudit: true, //是否显示审批意见
    };
  },

  computed: {},

  watch: {},

  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },

  methods: {
    initAxios() {
      apiInstitution = new ApiInstitution(this.axios);
      this.refresh();
    },

    // 关闭模态框
    close() {
      this.$emit('cancel');
      this.form.resetFields();
      this.record = null;
      this.loading = false;
      this.content = '';
    },

    async refresh() {
      if (this.institutionId == null) return;
      let response = await apiInstitution.get(this.institutionId);
      if (requestIsSuccess(response)) this.record = response.data;
      this.record = JSON.parse(JSON.stringify(this.record));

      let _content = this.record.abstract;
      this.content =
        _content == null
          ? null
          : _content.replace(new RegExp(`src="`, 'g'), `src="${this.fileServerEndPoint}`);

      if (
        this.record &&
        this.record.institutionRltFiles &&
        this.record.institutionRltFiles.length > 0
      ) {
        this.record.institutionRltFiles = this.record.institutionRltFiles.map(item => item.file);
      }

      if (
        this.record &&
        this.record.institutionRltLabels &&
        this.record.institutionRltLabels.length > 0
      ) {
        this.record.institutionRltLabels = this.record.institutionRltLabels.map(
          item => item.labelId,
        );
      }

      this.$nextTick(() => {
        let fields = { ...utils.objFilterProps(this.record, formFields) };
        this.form.setFieldsValue(fields);
      });
    },

    // 数据提交
    async ok() {
      this.$emit('success');
      this.form.resetFields();
      this.content = null;
      // this.form.validateFields(async (err, values) => {
      //   let _values = values;
      //   if (!err) {
      //     let data = {
      //       ..._values,
      //     };
      //     this.loading = true;
      //     // let response = await apiInstitution.create(data);
      //     if (utils.requestIsSuccess(response)) {
      //       this.$message.success('操作成功');
      //       this.close();
      //       this.$emit('success');
      //       this.content = null;
      //       this.form.resetFields();
      //     }
      //   }
      // });
    },
  },

  render() {
    return (
      <a-form form={this.form} class="SmRegulationAuditedInstitution">
        <div class="divider">
          <div style="display:flex;">
            <div style="display:flex;align-items:flex-end;margin:5px">
              <a-icon type="tags" />
            </div>
            <LabelTreeSelect
              bordered={false}
              simple={true}
              disabled={true}
              axios={this.axios}
              multiple
              value={this.record ? this.record.institutionRltLabels : null}
            />
          </div>
          <div style="justify-content:flex-end;">
            <div style="display:flex;">
              <div class="overflow">
                文件编号:
                <a-tooltip title={this.record ? this.record.code : null}>
                  {this.record ? this.record.code : null}
                </a-tooltip>
              </div>
              <div class="overflow">录入人: {null}</div>
            </div>
            <div style="display:flex;">
              <div class="overflow">
                生效时间:{' '}
                {this.record ? moment(this.record.effectiveTime).format('YYYY-MM-DD') : null}
              </div>
              <div class="overflow">
                所属部门: {this.record ? this.record.organization.name : null}
              </div>
            </div>
            <div style="display:flex;">
              <div class="overflow">
                过期时间: {this.record ? moment(this.record.expireTime).format('YYYY-MM-DD') : null}
              </div>
              <div class="overflow">当前版本: {this.record ? this.record.version : null}</div>
            </div>
          </div>
        </div>
        <div style="text-align:center;font-Size:30px;">
          {this.record ? this.record.header : '标题'}
        </div>
        <div
          {...{
            domProps: {
              innerHTML: this.record ? this.record.abstract : '摘要',
            },
          }}
        ></div>
        <div style="padding:5px 0">
          <SmFileManageSelect
            disabled={true}
            bordered={false}
            axios={this.axios}
            simpledisabled={true}
            height={30}
            multiple
            value={this.record ? this.record.institutionRltFiles : null}
          />
        </div>
        <div>
          <a
            href="#"
            onClick={() => {
              this.showAuthority = !this.showAuthority;
            }}
          >
            权限配置
          </a>
          {this.showAuthority ? (
            <a-icon type="down-circle" style="padding-left:10px" />
          ) : (
            <a-icon type="up-circle" style="padding-left:10px" />
          )}
          <a-divider style="margin:12px 0px;" />
          {this.showAuthority ? (
            <a-row gutter={24}>
              <a-col sm={24} md={24}>
                <a-form-item
                  label="可查看者"
                  placeholder={'请选择'}
                  label-col={{ span: 2 }}
                  wrapper-col={{ span: 22 }}
                >
                  <SmSystemMemberSelect
                    axios={this.axios}
                    bordered={false}
                    simple={true}
                    disabled={true}
                    simpledisabled={true}
                    height={40}
                    value={this.record ? this.record.listView : undefined}
                  />
                </a-form-item>
              </a-col>

              <a-col sm={24} md={24}>
                <a-form-item
                  label="可编辑者"
                  placeholder={'请选择'}
                  label-col={{ span: 2 }}
                  wrapper-col={{ span: 22 }}
                >
                  <SmSystemMemberSelect
                    axios={this.axios}
                    bordered={false}
                    simple={true}
                    disabled={true}
                    simpledisabled={true}
                    height={40}
                    value={this.record ? this.record.listEdit : undefined}
                  />
                </a-form-item>
              </a-col>

              <a-col sm={24} md={24}>
                <a-form-item
                  label="附件可下载"
                  placeholder={'请选择'}
                  label-col={{ span: 2 }}
                  wrapper-col={{ span: 22 }}
                >
                  <SmSystemMemberSelect
                    axios={this.axios}
                    bordered={false}
                    simple={true}
                    disabled={true}
                    simpledisabled={true}
                    height={40}
                    value={this.record ? this.record.listDownLoad : undefined}
                  />
                </a-form-item>
              </a-col>
            </a-row>
          ) : (
            []
          )}
        </div>

        <div>
          <a
            href="#"
            onClick={() => {
              this.showFlow = !this.showFlow;
            }}
          >
            流程设置
          </a>
          {this.showFlow ? (
            <a-icon type="down-circle" style="padding-left:10px" />
          ) : (
            <a-icon type="up-circle" style="padding-left:10px" />
          )}
          <a-divider style="margin:12px 0px;" />
          {this.showFlow ? (
            <a-row gutter={24}>
              <a-col sm={24} md={24}>
                <a-form-item label="选择流程" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                  <a-input
                    // disabled={this.status == ModalStatus.View}
                    v-decorator={[
                      'workflowId',
                      {
                        initialValue: ' ',
                      },
                    ]}
                  >
                    <a-icon slot="suffix" type="dash" />
                  </a-input>
                </a-form-item>
              </a-col>

              <a-col sm={24} md={24}>
                <a-form-item label="流程状态" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                  <a-input
                    // placeholder={this.status == ModalStatus.View ? '' : '请选择'}
                    // disabled={this.status == ModalStatus.View}
                    v-decorator={[
                      'workflowState',
                      {
                        initialValue: '',
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
            </a-row>
          ) : (
            []
          )}
        </div>

        {/* <div>
          <a
            href="#"
            onClick={() => {
              this.showAudit = !this.showAudit;
            }}
          >
            审批意见
          </a>
          {this.showAudit ? (
            <a-icon type="down-circle" style="padding-left:10px" />
          ) : (
            <a-icon type="up-circle" style="padding-left:10px" />
          )}
          <a-divider style="margin:12px 0px;" />
          {this.showAudit ? (
            <a-row gutter={24}>
              <a-col sm={12} md={12}>
                <a-form-item label="常用意见" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <a-select
                    // placeholder={this.status == ModalStatus.View ? '' : '下拉选择'}
                    disabled={false}
                    v-decorator={[
                      'suggestion',
                      {
                        initialValue: 1,
                      },
                    ]}
                  >
                    <a-select-option value={1}>{'已阅，审批通过'}</a-select-option>
                    <a-select-option value={2}>{'信息不完善，审批不通过'}</a-select-option>
                    <a-select-option value={3}>{'已转审'}</a-select-option>
                  </a-select>
                </a-form-item>
              </a-col>
              <a-col sm={24} md={24}>
                <a-form-item label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                  <a-textarea
                    style="margin-left:15px"
                    rows="2"
                    v-decorator={[
                      'suggestionInput',
                      {
                        initialValue: '',
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              <a-col sm={24} md={24}>
                <a-form-item
                  label="签章选择"
                  placeholder={'请选择'}
                  label-col={{ span: 2 }}
                  wrapper-col={{ span: 22 }}
                >
                  <SealTreeSelect
                    axios={this.axios}
                    height={40}
                    multiple
                    onChange={item => console.log(item)}
                    //placeholder={this.status == ModalStatus.View ? '' : '请选择'}
                    disabled={false}
                    v-decorator={[
                      'sealId',
                      {
                        initialValue: '',
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
            </a-row>
          ) : (
            []
          )}
        </div> */}
        <div style="display:flex;justify-content:flex-end">
          <a-button style="margin-right: 15px" onClick={() => this.close()}>
            关闭
          </a-button>

          <a-button type="primary" loading={this.loading} onClick={() => this.ok()}>
            保存{' '}
          </a-button>
        </div>
      </a-form>
    );
  },
};
