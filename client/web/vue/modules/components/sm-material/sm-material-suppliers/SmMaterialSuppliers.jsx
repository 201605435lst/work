import './style';
import { requestIsSuccess, getSupplierType, vIf, vP } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import ApiSupplier from '../../sm-api/sm-material/Supplier';
import permissionsSmMaterial from '../../_permissions/sm-material';
import FileSaver from 'file-saver';

let apiSupplier = new ApiSupplier();

export default {
  name: 'SmMaterialSuppliers',
  props: {
    permissions: { type: Array, default: () => [] },
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
  },
  data() {
    return {
      suppliers: [], // 列表数据源
      totalCount: 0,
      pageIndex: 1,
      isCanExport: false,
      pageSize: paginationConfig.defaultPageSize,
      queryParams: {
        keyword: '', // 模糊查询
        maxResultCount: paginationConfig.defaultPageSize,
      },
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          ellipsis: true,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '编号',
          dataIndex: 'code',
          width: 120,
          ellipsis: true,
          scopedSlots: { customRender: 'code' },
        },
        {
          title: '名称',
          dataIndex: 'name',
          ellipsis: true,
        },
        {
          title: '负责人',
          dataIndex: 'principal',
          ellipsis: true,
        },
        {
          title: '类型',
          dataIndex: 'type',
          scopedSlots: { customRender: 'type' },
        },
        {
          title: '法人',
          dataIndex: 'legalPerson',
          ellipsis: true,
        },
        {
          title: '主营范围',
          dataIndex: 'businessScope',
          ellipsis: true,
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: '140px',
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiSupplier = new ApiSupplier(this.axios);
    },

    //添加
    add() {
      this.$emit('add');
    },

    //编辑
    edit(record) {
      this.$emit('edit', record.id);
    },

    //详情
    view(record) {
      this.$emit('view', record.id);
    },

    //导出
    export() {
      console.log("来了");
      let _this = this;
      this.loading = true;
      return new Promise(async (resolve, reject) => {
        let response = await apiSupplier.export(_this.queryParams);
        _this.loading = false;
        if (requestIsSuccess(response)) {
          FileSaver.saveAs(
            new Blob([response.data], { type: 'application/vnd.ms-excel' }),
            `供应商数据表.xlsx`,
          );
          setTimeout(resolve, 100);
        } else {
          setTimeout(reject, 100);
        }
      });
    },

    // 删除
    remove(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content} </div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiSupplier.delete(record.id);
            if (requestIsSuccess(response)) {
              _this.refresh(false, _this.pageIndex);
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() {},
      });
    },

    // 刷新列表
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }

      let response = await apiSupplier.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(response) && response.data) {
        this.suppliers = response.data.items;
        this.totalCount = response.data.totalCount;
        this.isCanExport = this.totalCount > 0 ? false : true;
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
      this.loading = false;
    },

    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },
  },
  render() {
    return (
      <div class="sm-material-suppliers">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.keyword = '';
            this.refresh();
          }}
        >
          <a-form-item label="关键字">
            <a-input
              style="margin-right:10px;"
              placeholder="请输入供应商名称、编号"
              value={this.queryParams.keyword}
              onInput={event => {
                this.queryParams.keyword = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          <template slot="buttons">
            {vIf(
              <a-button type="primary" style="margin-right:10px;" onClick={this.add}>
                新增
              </a-button>,
              vP(this.permissions, permissionsSmMaterial.Suppliers.Create),
            )}
            {vIf(
              <a-button
                style="background-color: #52c41a; color: #fff;"
                disabled={this.isCanExport}
                onClick={this.export}
                loading={this.loading}
              >
                导出
              </a-button>,
              vP(this.permissions, permissionsSmMaterial.Suppliers.Export),
            )}
          </template>
        
          

        </sc-table-operator>
        {/* 展示区 */}
        <a-table
          columns={this.columns}
          dataSource={this.suppliers}
          rowKey={record => record.id}
          bordered={this.bordered}
          loading={this.loading}
          pagination={false}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              code: (text, record, index) => {
                return (
                  <a-tooltip placement="topLeft" title={record.code}>
                    {this.permissions.indexOf(permissionsSmMaterial.Suppliers.Detail) !== -1 ? (
                      <a
                        onClick={() => {
                          this.view(record);
                        }}
                      >
                        {record.code}
                      </a>
                    ) : (
                      record.code
                    )}
                  </a-tooltip>
                );
              },

              type: (text, record, index) => {
                return record.type ? getSupplierType(record.type) : '';
              },

              operations: (text, record) => {
                return [
                  <span>
                    {vIf(
                      <a
                        onClick={() => {
                          this.edit(record);
                        }}
                      >
                        编辑
                      </a>,
                      vP(this.permissions, permissionsSmMaterial.Suppliers.Update),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsSmMaterial.Suppliers.Update) &&
                        vP(this.permissions, permissionsSmMaterial.Suppliers.Delete),
                    )}

                    {vIf(
                      <a
                        style="color:red;"
                        onClick={() => {
                          this.remove(record);
                        }}
                      >
                        删除
                      </a>,
                      vP(this.permissions, permissionsSmMaterial.Suppliers.Delete),
                    )}
                  </span>,
                ];
              },
            },
          }}
        ></a-table>

        {/* 分页器 */}

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
      </div>
    );
  },
};
