import './style';
import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import LabelTreeSelect from '../sm-regulation-label-tree-select';
import ApiInstitution from '../../sm-api/sm-regulation/Institution';
import { requestIsSuccess } from '../../_utils/utils';
import moment from 'moment';
import SmFileManageSelect from '../../sm-file/sm-file-manage-select';

let apiInstitution = new ApiInstitution();

const formFields = [];
export default {
  name: 'SmRegulationViewModal',
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
    };
  },

  computed: {
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

    // 关闭模态框
    close() {
      this.status = ModalStatus.Hide;
      this.record = null;
      this.loading = false;
      this.content = '';
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

      this.$nextTick(() => {
        let fields = { ...utils.objFilterProps(this.record, formFields) };
        this.form.setFieldsValue(fields);
      });
    },

    // 详情
    async detail(record) {
      this.status = ModalStatus.View;
      this.search(record.id);
    },

    // 数据提交
    async ok() {
      if (this.status == ModalStatus.View) {
        this.close();
      }
    },
  },

  render() {
    return (
      <a-modal
        title={`查看制度`}
        visible={this.visible}
        onCancel={this.close}
        confirmLoading={this.loading}
        destroyOnClose={true}
        forceRender={this.forceRender}
        onOk={this.ok}
        width={800}
      >
        <a-form form={this.form} class="SmRegulationInstitution">
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
                  <a-tooltip title={this.record ? this.record.code : 'null'}>
                    {this.record ? this.record.code : 'null'}
                  </a-tooltip>
                </div>
                <div>录入人: {'null'}</div>
              </div>
              <div style="display:flex;">
                <div style="margin-right:10px">
                  生效时间:{' '}
                  {this.record ? moment(this.record.effectiveTime).format('YYYY-MM-DD') : null}
                </div>
                <div>所属部门: {this.record ? this.record.organization.name : null}</div>
              </div>
              <div style="display:flex;">
                <div style="margin-right:10px">
                  过期时间:{' '}
                  {this.record ? moment(this.record.expireTime).format('YYYY-MM-DD') : null}
                </div>
                <div>当前版本: {this.record ? this.record.version : null}</div>
              </div>
            </div>
          </div>
          <div style="text-align:center;font-Size:20px;">
            {this.record ? this.record.header : null}
          </div>
          <div
            {...{
              domProps: {
                innerHTML: this.record ? this.record.abstract : null,
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
              enableDownload={true}
              value={this.record ? this.record.institutionRltFiles : null}
            />
          </div>
        </a-form>
      </a-modal>
    );
  },
};
