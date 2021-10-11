import { pagination as paginationConfig, tips as tipsConfig } from '../../../_utils/config';
import ApiQuotaCategory from '../../../sm-api/sm-std-basic/QuotaCategory';
import ApiProductCategoryRltQuota from '../../../sm-api/sm-std-basic/ProductCategoryRltQuota';
import SmStdBasicQuotaTreeSelect from '../../sm-std-basic-quota-tree-select/SmStdBasicQuotaTreeSelect';
import { requestIsSuccess } from '../../../_utils/utils';

let apiQuotaCategory = new ApiQuotaCategory();
let apiProductCategoryRltQuota = new ApiProductCategoryRltQuota();
export default {
  name: 'SmStdBasicProductCategoryRltQuota',
  props: {
    axios: { type: Function, default: null },
    datas: { type: Object, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      dataSource: [],
      quotas: null,
      totalCount: 0,
      pageIndex: 1,
      queryParams: {
        maxResultCount: paginationConfig.defaultPageSize,
      },

      categoryId: null,
      form: {}, // 表单
      action: null,
    };
  },

  computed: {
    //关联工程工项
    columns() {
      return [
        {
          title: '序号',
          // width: 80,
          dataIndex: 'index',
          scopedSlots: { customRender: 'index' },
          ellipsis: true,
        },
        {
          title: '定额名称',
          width: 100,
          dataIndex: 'name',
          ellipsis: true,
        },
        {
          title: '定额编号',
          // width: 100,
          dataIndex: 'code',
          ellipsis: true,
        },
        {
          title: '单位',
          width: 80,
          dataIndex: 'unit',
          ellipsis: true,
        },
        {
          title: '重量',
          // width: 80,
          dataIndex: 'weight',
          ellipsis: true,
        },
        {
          title: '定额分类',
          width: 100,
          dataIndex: 'quotaCategoryName',
          ellipsis: true,
        },
        {
          title: '工作内容',
          width: 100,
          dataIndex: 'content',
          ellipsis: true,
        },

        {
          title: '操作',
          dataIndex: 'operations',
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },

  watch: {
    datas: {
      handler: function(val, oldVal) {
        if (this.datas != null) {
          this.categoryId = this.datas.id;
          this.form = this.$form.createForm(this, {});
          this.initAxios();
          // this.refresh();
        }
      },
      immediate: true,
      deep: true,
    },
  },
  async created() {
    this.form = this.$form.createForm(this, {});
    this.initAxios();
  },
  methods: {
    async initAxios() {
      apiQuotaCategory = new ApiQuotaCategory(this.axios);
      apiProductCategoryRltQuota = new ApiProductCategoryRltQuota(this.axios);
      this.quotas = null;
      this.dataSource = null;
      let response = await apiProductCategoryRltQuota.getListByProductCategoryId({
        id: this.datas ? this.datas.id : undefined,
        isAll: true,
      });

      if (requestIsSuccess(response) && response.data.items) {
        this.quotas = response.data.items.map(x => x.quotaId); //传入组件的Ids
      }
      this.refresh();
    },

    async refresh(resetPage = true, page) {
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let data = {
        quotaIds: this.quotas,
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      };
      let response = await apiQuotaCategory.getListTree(data);
      if (requestIsSuccess(response) && response.data.items.length > 0) {
        let quotaItems = [];
        response.data.items.map(x => {
          if (x.type === 2) quotaItems.push(x);
        });
        this.dataSource = quotaItems;

        this.totalCount = quotaItems.length;
        if (page && this.totalCount && this.queryParams.maxResultCount) {
          let currentPage = parseInt(this.totalCount / this.queryParams.maxResultCount);
          if (this.totalCount % this.queryParams.maxResultCount !== 0) {
            currentPage = page + 1;
          }
          if (page > currentPage) {
            this.pageIndex = currentPage;
            this.refresh(false, this.pageIndex);
          }
        }
      }
    },

    onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },

    async save() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let response = await apiProductCategoryRltQuota.editList({
            productionCategoryId: this.categoryId,
            quotaIdList: this.quotas,
          });
          if (requestIsSuccess(response)) this.$message.success('保存成功');
          this.form.resetFields();
        }
      });
    },
    //取消
    cancel() {
      this.initAxios();
    },
    delete(record) {
      this.quotas = this.quotas.filter(x => x != record.id);
      this.refresh();
    },
  },
  render() {
    return (
      <div>
        <SmStdBasicQuotaTreeSelect
          ref="SmStdBasicQuotaTreeSelect"
          style="margin-bottom:10px"
          axios={this.axios}
          allowClear
          treeCheckable={true}
          disabled={this.isShow}
          value={this.quotas}
          placeholder={'请选择关联的定额'}
          onChange={value => {
            this.quotas = value;
            this.refresh(false);
          }}
        />
        <a-table
          columns={this.columns}
          visible={true}
          dataSource={this.dataSource}
          rowKey={record => record.id}
          pagination={false}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                let result = index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                return result;
              },
              operations: (text, record, index) => {
                return [
                  <span>
                    <a
                      onClick={() => {
                        this.delete(record);
                      }}
                    >
                      {' '}
                      删除
                    </a>
                  </span>,
                ];
              },
            },
          }}
        ></a-table>
        <a-pagination
          style="float:right; margin-top:10px"
          total={this.totalCount}
          pageSize={this.queryParams.maxResultCount}
          current={this.pageIndex}
          onChange={this.onPageChange}
          onShowSizeChange={this.onPageChange}
          showSizeChanger
          showQuickJumper
          showTotal={paginationConfig.showTotal}
        />
        <div style="margin-top: 10px;">
          <a-button type="primary" onClick={() => this.save()}>
            保存
          </a-button>
          <a-button
            style="margin-left:20px"
            onClick={() => {
              this.cancel();
            }}
          >
            取消
          </a-button>
        </div>
      </div>
    );
  },
};
