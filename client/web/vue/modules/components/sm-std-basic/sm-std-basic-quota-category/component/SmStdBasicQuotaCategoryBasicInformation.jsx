import SmStdBasicQuotaCategoryModel from './SmStdBasicQuotaCategoryModel';
import ApiQuotaCategory from '../../../sm-api/sm-std-basic/QuotaCategory';
import { requestIsSuccess, vP, vIf } from '../../../_utils/utils';
import permissionsSmStdBasic from '../../../_permissions/sm-std-basic';

let apiQuotaCategory = new ApiQuotaCategory();

export default {
  name: 'SmStdBasicQuotaCategoryBasicInformation',
  props: {
    datas: { type: Object, default: null },//数据源
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
  },

  data() {
    return {
      dataSource: {},
    };
  },
  computed: {
  },
  watch: {
    datas: {
      handler: function (val, oldVal) {
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
      apiQuotaCategory = new ApiQuotaCategory(this.axios);
    },
    //编辑
    edit() {
      this.$refs.SmStdBasicQuotaCategoryModel.edit(this.dataSource);
    },
    //刷新
    async refresh(data) {
      let response = await apiQuotaCategory.get(this.datas ? this.datas.id : '');
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
      <div class="sm-std-basic-quota-category-basic-information">

        <a-descriptions bordered column={1}>
          <a-descriptions-item label="父级">
            {
              this.dataSource.parent ? this.dataSource.parent.name : ''
            }
          </a-descriptions-item>
          <a-descriptions-item label="名称">
            {this.dataSource.name}
            
          </a-descriptions-item>
          <a-descriptions-item label="编码">
            {this.dataSource.code}
          </a-descriptions-item>
          <a-descriptions-item label="专业">
            {this.dataSource.specialtyName}
          </a-descriptions-item>
          <a-descriptions-item label="标准编号">
            {this.dataSource.standardCodeName}
          </a-descriptions-item>
          <a-descriptions-item label="内容" >
            {this.dataSource.content}
          </a-descriptions-item>
          <a-descriptions-item label="行政区域" >
            {this.dataSource.areaName}
          </a-descriptions-item>
         
        </a-descriptions>
        {vIf(
          <div class="info-button">
            <a-button
              style="margin-right:20px"
              type="primary"
              onClick={() => {
                this.edit();
              }}>
              编辑
            </a-button>
          </div>,
          vP(this.permissions, permissionsSmStdBasic.QuotaCategorys.Update),
        )}
        <SmStdBasicQuotaCategoryModel
          ref="SmStdBasicQuotaCategoryModel"
          axios={this.axios}
          onSuccess={(item) => {
            this.refresh(item);
          }}
        />

      </div>
    );
  },
};