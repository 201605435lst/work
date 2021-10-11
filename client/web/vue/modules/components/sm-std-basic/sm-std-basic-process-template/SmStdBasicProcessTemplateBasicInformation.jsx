import SmStdBasicProcessTemplateModal from './SmStdBasicProcessTemplateModal';
import ApiProcessTemplate from '../../sm-api/sm-std-basic/ProcessTemplate';
import {
  requestIsSuccess,
  getServiceLifeUnit,
  getProcessTypeOption,
  vP,
  vIf,
} from '../../_utils/utils';
import permissionsSmStdBasic from '../../_permissions/sm-std-basic';
let apiProcessTemplate = new ApiProcessTemplate();

export default {
  name: 'SmStdBasicProcessTemplateBasicInformation',
  props: {
    datas: { type: Object, default: null }, //数据源
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
  },

  data() {
    return {
      dataSource: {},
    };
  },
  computed: {},
  watch: {
    datas: {
      handler: function(val, oldVal) {
        if (this.datas != null) {
          this.initAxios();
          this.refresh();
        }
      },
      immediate: true,
      deep: true,
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiProcessTemplate = new ApiProcessTemplate(this.axios);
    },
    //编辑
    edit() {
      this.$refs.SmStdBasicProcessTemplateModal.edit(this.dataSource);
    },
    //刷新
    async refresh(data) {
      let response = await apiProcessTemplate.get(this.datas ? this.datas.id : '');
      if (requestIsSuccess(response) && response.data) {
        this.dataSource = response.data;
        if (data) {
          this.$emit('editData', data);
        }
      }
    },
  },
  render() {
    return (
      <div class="sm-std-basic-process-template-basic-information">
        <a-descriptions bordered column={1}>
          <a-descriptions-item label="父级">
            {this.dataSource && this.dataSource.parent ? this.dataSource.parent.name : undefined}
          </a-descriptions-item>
          <a-descriptions-item label="编码">
            {this.dataSource ? this.dataSource.code : undefined}
          </a-descriptions-item>
          <a-descriptions-item label="名称">
            {this.dataSource ? this.dataSource.name : undefined}
          </a-descriptions-item>
          <a-descriptions-item label="工作项单位">
            {this.dataSource ? this.dataSource.unit : undefined}
          </a-descriptions-item>
          <a-descriptions-item label="工期">
            {this.dataSource ? this.dataSource.duration : undefined}
            {this.dataSource ? getServiceLifeUnit(this.dataSource.durationUnit) : undefined}
          </a-descriptions-item>
          <a-descriptions-item label="工作内容">
            {this.dataSource ? this.dataSource.content : undefined}
          </a-descriptions-item>
          <a-descriptions-item label="工序类型">
            {this.dataSource ? getProcessTypeOption(this.dataSource.type) : undefined}
          </a-descriptions-item>
          <a-descriptions-item label="前置任务">
            {this.dataSource.prepositionCode}
          </a-descriptions-item>
          <a-descriptions-item label="备注">
            {this.dataSource && this.dataSource.remark ? this.dataSource.remark : undefined}
          </a-descriptions-item>
        </a-descriptions>
        {vIf(
          <div class="info-button">
            <a-button
              style="margin-right:20px"
              type="primary"
              onClick={() => {
                this.edit();
              }}
            >
              编辑
            </a-button>
          </div>,
          vP(this.permissions, permissionsSmStdBasic.ProcessTemplates.Update),
        )}
        <SmStdBasicProcessTemplateModal
          ref="SmStdBasicProcessTemplateModal"
          axios={this.axios}
          onSuccess={item => {
            this.refresh(item);
          }}
        />
      </div>
    );
  },
};
