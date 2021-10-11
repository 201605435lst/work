import './style';
import * as utils from '../../_utils/utils';
import LabelTreeSelect from '../sm-regulation-label-tree-select';
import ApiInstitution from '../../sm-api/sm-regulation/Institution';
import { requestIsSuccess } from '../../_utils/utils';
import moment from 'moment';
import SmFileManageSelect from '../../sm-file/sm-file-manage-select';

let apiInstitution = new ApiInstitution();

const formFields = [];
export default {
  name: 'SmRegulationViewInstitution',
  props: {
    axios: { type: Function, default: null },
    institutionId: { type: String, default: null },
  },

  data() {
    return {
      form: {}, // 表单
      record: null, // 表单绑的对象,
      loading: false,
      content: null,
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
    cancel() {
      this.$emit('cancel');
      this.record = null;
      this.loading = false;
      this.content = '';
    },

    // 数据提交
    ok() {
      this.$emit('success');
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
  },

  render() {
    return (
      <a-form form={this.form} class="SmRegulationViewInstitution">
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
          <div>
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
          style="padding-bottom:5px;"
        ></div>
        <div>
          <SmFileManageSelect
            disabled={true}
            bordered={false}
            axios={this.axios}
            simpledisabled={true}
            height={30}
            multiple
            //判断当前用户能否下载此制度
            enableDownload={
              this.record && this.record.downLoadInstitution == this.institutionId ? true : false
            }
            value={this.record ? this.record.institutionRltFiles : undefined}
          />
        </div>
        <div style="display:flex;justify-content:flex-end;margin-top:10px">
          <a-button style="margin-right: 15px" onClick={() => this.cancel()}>
            取消
          </a-button>

          <a-button type="primary" loading={this.loading} onClick={() => this.ok()}>
            确定
          </a-button>
        </div>
      </a-form>
    );
  },
};
