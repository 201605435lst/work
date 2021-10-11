import './style';
import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import OrganizationTreeSelect from '../../sm-system/sm-system-organization-tree-select';
import SmSystemMemberSelect from '../../sm-system/sm-system-member-select';
import SmBpmWorkflowSelectModal from '../../sm-bpm/sm-bpm-workflow-templates/SmBpmWorkflowSelectModal';
import SealTreeSelect from '../../sm-oa/sm-oa-seals-select';
import LabelTreeSelect from '../sm-regulation-label-tree-select';
import ApiInstitution from '../../sm-api/sm-regulation/Institution';
import { requestIsSuccess } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import RichTextEditor from '../../sm-file/sm-file-text-editor/SmFileTextEditor';
import moment from 'moment';
import SmFileManageSelect from '../../sm-file/sm-file-manage-select';

let apiInstitution = new ApiInstitution();
// 定义表单字段常量
const formFields = [
  'header',
  'code',
  'classify',
  'organizationId',
  'expireTime',
  'effectiveTime',
  'institutionRltLabels',
  'institutionRltFiles',
  'version',
  'listView',
  'listEdit',
  'listDownLoad',

  'workflowId',
  'workflowState',
  'isPublish',
  'publishClassify',
  'isApprove',
];

export default {
  name: 'SmRegulationInstitutionModal',
  props: {
    axios: { type: Function, default: null },
  },

  data() {
    return {
      form: {}, // 表单
      record: null, // 表单绑的对象,
      loading: false,
      status: ModalStatus.Hide,
      content: null,
      show: false, //是否显示新闻发布类别
      showEdition: true, //是否显示文件版本
      showAuthority: true, //是否显示权限配置
      showFlow: true, //是否显示流程设置
      showApprove: true, //是否显示发布设置
    };
  },

  computed: {
    title() {
      // 计算模态框的标题变量
      return utils.getModalTitle(this.status);
    },

    visible() {
      return this.status !== ModalStatus.Hide;
    },
  },

  watch: {},

  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },

  methods: {
    initAxios() {
      apiInstitution = new ApiInstitution(this.axios);
    },

    cancel() {
      this.status = ModalStatus.Hide;
      this.$emit('cancel');
      this.form.resetFields();
      this.content = '';
    },

    // 关闭模态框
    close() {
      this.status = ModalStatus.Hide;
      this.record = null;
      this.loading = false;
      this.content = '';
    },

    async add() {
      this.status = ModalStatus.Add;
      this.version = 1;
      this.$nextTick(() => {
        this.form.setFieldsValue();
      });
    },

    // 生成编码
    async generatedCode() {
      let date = moment().format('YYYY-MM-DD-HH-mm-ss');
      let num = date.replaceAll('-', '');
      let code = 'WJ-' + num;
      this.$nextTick(() => {
        this.form.setFieldsValue({ code: code });
      });
    },

    async search(id) {
      let response = await apiInstitution.get(id);
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

      this.record.effectiveTime = this.record.effectiveTime
        ? moment(this.record.effectiveTime)
        : null;
      this.record.expireTime = this.record.expireTime ? moment(this.record.expireTime) : null;

      this.$nextTick(() => {
        let fields = { ...utils.objFilterProps(this.record, formFields) };
        this.form.setFieldsValue(fields);
      });
    },

    async edit(record) {
      this.search(record.id);
      this.status = ModalStatus.Edit;
    },

    // 详情
    async detail(record) {
      this.search(record.id);
      this.status = ModalStatus.View;
    },

    // 数据提交
    async ok() {
      if (this.status == ModalStatus.Add) {
        await this.generatedCode();
      } else {
      }
      if (this.status == ModalStatus.View) {
        this.close();
      } else {
        this.form.validateFields(async (err, values) => {
          console.log(values);
          let _content = this.$refs['sc-rich-text-editor'].content();
          let reg = new RegExp(`${this.fileServerEndPoint}`, 'g');
          values.abstract = _content.replace(reg, '');
          let _values = values;
          if (!err) {
            let data = {
              ..._values,
              institutionRltFiles: _values.institutionRltFiles
                ? _values.institutionRltFiles.map(item => {
                  return item.id;
                })
                : [],
            };
            let response = null;
            this.loading = true;
            if (this.status === ModalStatus.Add) {
              response = await apiInstitution.create(data);
            } else if (this.status === ModalStatus.Edit) {
              data.version = data.version + 1;
              this.confirmLoading = true;
              response = await apiInstitution.update({ ...data, id: this.record.id });
            }

            if (utils.requestIsSuccess(response)) {
              this.$message.success('操作成功');
              this.close();
              this.$emit('success');
              this.content = null;
              this.form.resetFields();
            }
          }
        });
      }
    },
  },

  render() {
    return (
      <a-modal
        title={`${this.title}制度`}
        visible={this.visible}
        onCancel={this.close}
        confirmLoading={this.loading}
        destroyOnClose={true}
        forceRender={this.forceRender}
        okText="保存"
        onOk={this.ok}
        width={800}
      >
        <a-form form={this.form}>
          <a-row gutter={24}>
            <a-col sm={12} md={12}>
              <a-form-item label="文件标题" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-input
                  placeholder={this.status == ModalStatus.View ? '' : '请输入文件标题'}
                  disabled={this.status == ModalStatus.View}
                  v-decorator={[
                    'header',
                    {
                      initialValue: '',
                      rules: [
                        {
                          required: true,
                          message: '请输入文件标题',
                        },
                        {
                          max: 100,
                          message: '文件标题最多输入100字符',
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="文件编号" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-input
                  placeholder={'提交后自动生成'}
                  disabled={true}
                  v-decorator={[
                    'code',
                    {
                      initialValue: '',
                    },
                  ]}
                ></a-input>
              </a-form-item>
            </a-col>

            <a-col sm={12} md={12}>
              <a-form-item label="所属类别" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-select
                  placeholder={this.status == ModalStatus.View ? '' : '请选择所属类别'}
                  disabled={this.status == ModalStatus.View}
                  v-decorator={[
                    'classify',
                    {
                      initialValue: 1,
                    },
                  ]}
                >
                  <a-select-option value={1}>{'行政法规'}</a-select-option>
                  <a-select-option value={2}>{'章程'}</a-select-option>
                  <a-select-option value={3}>{'制度'}</a-select-option>
                  <a-select-option value={4}>{'公约'}</a-select-option>
                </a-select>
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="所属部门" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <OrganizationTreeSelect
                  placeholder={this.status == ModalStatus.View ? '' : '请选择所属部门'}
                  disabled={this.status == ModalStatus.View}
                  axios={this.axios}
                  onChange={value => {}}
                  v-decorator={[
                    'organizationId',
                    {
                      initialValue: null,
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={12} md={12}>
              <a-form-item label="生效时间" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-date-picker
                  placeholder={this.status == ModalStatus.View ? '' : '请选择生效时间'}
                  disabled={this.status == ModalStatus.View}
                  showTime
                  style="width:100%"
                  onChange={(date, dateString) => {}}
                  v-decorator={[
                    'effectiveTime',
                    {
                      initialValue: null,
                      rules: [
                        {
                          required: true,
                          message: '请选择生效时间',
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
            <a-col sm={12} md={12}>
              <a-form-item label="过期时间" label-col={{ span: 6 }} wrapper-col={{ span: 18 }}>
                <a-date-picker
                  placeholder={this.status == ModalStatus.View ? '' : '请选择过期时间'}
                  disabled={this.status == ModalStatus.View}
                  showTime
                  style="width:100%"
                  format="YYYY-MM-DD HH:mm:ss"
                  onChange={(date, dateString) => {}}
                  v-decorator={[
                    'expireTime',
                    {
                      initialValue: null,
                      rules: [
                        {
                          required: true,
                          message: '请选择过期时间',
                        },
                      ],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={24}>
              <a-form-item label="标签" label-col={{ span: 3 }} wrapper-col={{ span: 21 }}>
                <LabelTreeSelect
                  axios={this.axios}
                  height={73}
                  multiple
                  onChange={item => console.log(item)}
                  placeholder={this.status == ModalStatus.View ? '' : '请选择标签'}
                  disabled={this.status == ModalStatus.View}
                  v-decorator={[
                    'institutionRltLabels',
                    {
                      initialValue: [],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={24}>
              <a-form-item label="摘要" label-col={{ span: 3 }} wrapper-col={{ span: 21 }}>
                <RichTextEditor
                  placeholder={this.status == ModalStatus.View ? '' : '请输入'}
                  disabled={this.status == ModalStatus.View}
                  ref="sc-rich-text-editor"
                  axios={this.axios}
                  value={this.content}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={24}>
              <a-form-item label="文件附件" label-col={{ span: 3 }} wrapper-col={{ span: 21 }}>
                <SmFileManageSelect
                  placeholder={this.status == ModalStatus.View ? '' : '请选择文件'}
                  disabled={this.status == ModalStatus.View}
                  axios={this.axios}
                  height={73}
                  multiple
                  onChange={item => console.log(item)}
                  enableDownload={true}
                  v-decorator={[
                    'institutionRltFiles',
                    {
                      initialValue: [],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
          </a-row>

          <a
            href="#"
            onClick={() => {
              this.showEdition = !this.showEdition;
            }}
          >
            文件版本
          </a>
          {this.showEdition ? (
            <a-icon type="down-circle" style="padding-left:10px" />
          ) : (
            <a-icon type="up-circle" style="padding-left:10px" />
          )}
          <a-divider style="margin:12px 0px;" />
          {this.showEdition ? (
            <a-row gutter={24}>
              <a-col sm={24} md={24}>
                <a-form-item label="当前版本" label-col={{ span: 3 }} wrapper-col={{ span: 21 }}>
                  <a-input
                    disabled={true}
                    v-decorator={[
                      'version',
                      {
                        initialValue: 1,
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              ,
            </a-row>
          ) : (
            []
          )}

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
                  label-col={{ span: 3 }}
                  wrapper-col={{ span: 21 }}
                >
                  <SmSystemMemberSelect
                    axios={this.axios}
                    height={73}
                    multiple
                    placeholder={this.status == ModalStatus.View ? '' : '请选择'}
                    disabled={this.status == ModalStatus.View}
                    // value={this.record ? this.record.listView : undefined}
                    // onChange={item => {
                    //   console.log(item.target.value);
                    //   this.record.listView = item.target.value;
                    // }}
                    v-decorator={[
                      'listView',
                      {
                        initialValue: [],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>

              <a-col sm={24} md={24}>
                <a-form-item
                  label="可编辑者"
                  placeholder={'请选择'}
                  label-col={{ span: 3 }}
                  wrapper-col={{ span: 21 }}
                >
                  <SmSystemMemberSelect
                    axios={this.axios}
                    height={73}
                    multiple
                    onChange={item => console.log(item)}
                    placeholder={this.status == ModalStatus.View ? '' : '请选择'}
                    disabled={this.status == ModalStatus.View}
                    v-decorator={[
                      'listEdit',
                      {
                        initialValue: [],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>

              <a-col sm={24} md={24}>
                <a-form-item
                  label="附件可下载"
                  placeholder={'请选择'}
                  label-col={{ span: 3 }}
                  wrapper-col={{ span: 21 }}
                >
                  <SmSystemMemberSelect
                    axios={this.axios}
                    height={73}
                    multiple
                    onChange={item => console.log(item)}
                    placeholder={this.status == ModalStatus.View ? '' : '请选择'}
                    disabled={this.status == ModalStatus.View}
                    enableDownload={true}
                    v-decorator={[
                      'listDownLoad',
                      {
                        initialValue: [],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
            </a-row>
          ) : (
            []
          )}

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
                <a-form-item label="选择流程" label-col={{ span: 3 }} wrapper-col={{ span: 21 }}>
                  {/* <SmBpmWorkflowSelectModal
                    //SmBpmWorkflowSelectModal
                    axios={this.axios}
                   // select={true}

                    // height={73}
                    //  onChange={item => console.log(item)}
                    // onSelected={}
                    // onOpen={}

                    placeholder={this.status == ModalStatus.View ? '' : '请选择'}
                    bordered={true}
                    select={true}
                    disabled={this.status == ModalStatus.View}
                    v-decorator={[
                      'workflowId',
                      {
                        initialValue: [],
                      },
                    ]}
                  /> */}
                  <a-button
                    onClick={() => {
                      this.$refs.SmBpmWorkflowSelectModal.show();
                    }}
                    // v-decorator={[
                    //   'workflowId',
                    //   {
                    //     initialValue: ' ',
                    //   },
                    // ]}
                  >
                    点击选择流程
                    {/* <a-icon slot="suffix" type="dash" /> */}
                  </a-button>
                </a-form-item>
              </a-col>

              <a-col sm={24} md={24}>
                <a-form-item label="流程状态" label-col={{ span: 3 }} wrapper-col={{ span: 21 }}>
                  <a-input
                    placeholder={this.status == ModalStatus.View ? '' : '请选择'}
                    disabled={this.status == ModalStatus.View}
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

          {/* <a
            href="#"
            onClick={() => {
              this.showApprove = !this.showApprove;
            }}
          >
            发布设置
          </a>
          {this.showApprove ? (
            <a-icon type="down-circle" style="padding-left:10px" />
          ) : (
            <a-icon type="up-circle" style="padding-left:10px" />
          )}
          <a-divider style="margin:12px 0px;" />
          {this.showApprove ? (
            <a-row gutter={24}>
              <a-col sm={24} md={24}>
                <a-form-item
                  label="是否发布到新闻"
                  label-col={{ span: 4 }}
                  wrapper-col={{ span: 20 }}
                >
                  <a-radio-group
                    disabled={this.status === ModalStatus.View}
                    v-decorator={[
                      'isPublish',
                      {
                        initialValue: 2,
                      },
                    ]}
                  >
                    <a-radio
                      value={1}
                      onClick={() => {
                        this.show = true;
                      }}
                    >
                      是
                    </a-radio>
                    <a-radio
                      value={2}
                      onClick={() => {
                        this.show = false;
                      }}
                    >
                      否
                    </a-radio>
                  </a-radio-group>
                </a-form-item>
              </a-col>

              {!this.show ? (
                []
              ) : (
                <a-col
                  sm={24}
                  md={24}
                  style={`${this.show ? 'display: visible' : 'display: none'}`}
                >
                  <a-form-item
                    label="新闻发布类别"
                    label-col={{ span: 4 }}
                    wrapper-col={{ span: 20 }}
                  >
                    <a-select
                      placeholder={this.status == ModalStatus.View ? '' : '请选择所属类别'}
                      disabled={this.status == ModalStatus.View}
                      v-decorator={[
                        'publishClassify',
                        {
                          initialValue: 1,
                        },
                      ]}
                    >
                      <a-select-option value={1}>{'突发性新闻'}</a-select-option>
                      <a-select-option value={2}>{'持续性新闻'}</a-select-option>
                      <a-select-option value={3}>{'周期性新闻'}</a-select-option>
                    </a-select>
                  </a-form-item>
                </a-col>
              )}

              <a-col sm={24} md={24}>
                <a-form-item
                  label="是否需要新闻审批"
                  label-col={{ span: 4 }}
                  wrapper-col={{ span: 20 }}
                >
                  <a-radio-group
                    placeholder={this.status == ModalStatus.View ? '' : '请选择'}
                    disabled={this.status == ModalStatus.View}
                    v-decorator={[
                      'isApprove',
                      {
                        initialValue: 1,
                      },
                    ]}
                  >
                    <a-radio value={1}>是</a-radio>
                    <a-radio value={2}>否</a-radio>
                  </a-radio-group>
                </a-form-item>
              </a-col>
            </a-row>
          ) : (
            []
          )} */}
        </a-form>
        <template slot="footer">
          <a-button
            type="default"
            onClick={() => {
              this.close();
            }}
          >
            关闭
          </a-button>
          <a-button type="primary" onClick={() => this.ok()}>
            暂存
          </a-button>
          <a-button type="primary" onClick={() => this.ok()}>
            保存
          </a-button>
        </template>
        <SmBpmWorkflowSelectModal
          ref="SmBpmWorkflowSelectModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
      </a-modal>
    );
  },
};
