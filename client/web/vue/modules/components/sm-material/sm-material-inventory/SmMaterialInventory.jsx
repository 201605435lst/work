/**
 * 说明：库存管理模块
 * 作者：easten
 */
import { requestIsSuccess, getSupplierType, vIf, vP } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import moment from 'moment';
import ApiInventory from '../../sm-api/sm-material/Inventory';
import SmMaterialPartitalTreeSelect from '../sm-material-partital-tree-select';
import InventoryDetailModal from './InventoryDetailModal';
import FileSaver from 'file-saver';
import './style';
let apiInventory = new ApiInventory();
export default {
  name: 'SmMaterialInventory',
  components: {},
  props: {
    axios: { type: Function, default: null },
    isSimple: { type: Boolean, default: false },//是否精简模式
    multiple: { type: Boolean, default: true },//是否多选
    bordered: { type: Boolean, default: false },//是否显示表格边框
    selected: { type: Array, default: () => [] },//所选数据
    partitionId: { type: String, default: null },//所选数据
    size: { type: String, default: 'default' },//表格大小
  },
  data() {
    return {
      queryParams: {
        keyword: '',
        partitionId: undefined,//库存地点
        maxResultCount: 0,
      },
      dateRange: [],
      totalCount: 0,
      pageIndex: 1,
      dataSource: [],
      selectedRowKeys: [],
      iSelected: [],//已选库存
    };
  },
  computed: {
    columns() {
      return this.isSimple ? [
        {
          title: '编号',
          dataIndex: 'code',
          width: 60,
          ellipsis: true,
          scopedSlots: { customRender: 'code' },
        },
        {
          title: '材料名称',
          dataIndex: 'material.name',
          ellipsis: true,
        },
        {
          title: '规格',
          dataIndex: 'material.spec',
          ellipsis: true,
        },
        {
          title: '库存地点',
          dataIndex: 'partition.name',
          ellipsis: true,
        },
        {
          title: '库存量',
          dataIndex: 'amount',
          ellipsis: true,
        },
        {
          title: '价格',
          dataIndex: 'price',
          ellipsis: true,
        },
        {
          title: '供应商',
          dataIndex: 'supplier.name',
          ellipsis: true,
        },
      ] : [
        {
          title: '编号',
          dataIndex: 'code',
          width: 60,
          ellipsis: true,
          scopedSlots: { customRender: 'code' },
        },
        {
          title: '材料名称',
          dataIndex: 'material.name',
          ellipsis: true,
        },
        {
          title: '规格',
          dataIndex: 'material.spec',
          ellipsis: true,
        },
        {
          title: '库存地点',
          dataIndex: 'partition.name',
          ellipsis: true,
        },
        {
          title: '库存量',
          dataIndex: 'amount',
          ellipsis: true,
        },
        {
          title: '价格',
          dataIndex: 'price',
          ellipsis: true,
        },
        {
          title: '供应商',
          dataIndex: 'supplier.name',
          ellipsis: true,
        },
        {
          title: '入库时间',
          dataIndex: 'entryTime',
          ellipsis: true,
          scopedSlots: { customRender: 'entryTime' },
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
  watch: {
    selected: {
      handler: function (value, oldVal) {
        if (value && value.length > 0) {
          this.iSelected = value;
          this.selectedRowKeys = value.map(item => item.id);
        } else {
          this.iSelected = value;
          this.selectedRowKeys = [];
        }

      },
      immediate: true,
    },
    partitionId: {
      handler: function (value, oldVal) {
        if (value) {
          this.queryParams.partitionId = value;
          apiInventory = new ApiInventory(this.axios);
          this.refresh();
        }
      },
      immediate: true,
    },
  },
  async created() {
    apiInventory = new ApiInventory(this.axios);
    await this.refresh();
  },
  methods: {
    // 刷新
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      // 获取数据
      let response = await apiInventory.getList({
        sTime: this.dateRange.length > 0 ? moment(this.dateRange[0]).hours(0).minutes(0).seconds(0).format('YYYY-MM-DD HH:mm:ss') : '',
        eTime: this.dateRange.length > 0 ? moment(this.dateRange[1]).hours(23).minutes(59).seconds(59).format('YYYY-MM-DD HH:mm:ss') : '',
        ...this.queryParams,
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
      });
      if (requestIsSuccess(response)) {
        this.loading = false;
        this.dataSource = response.data.items;
        this.totalCount = response.data.totalCount;
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

    // 删除
    delete(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content} </div>,
        okType: 'danger',
        onOk() {
          if (record) {
            return new Promise(async (resolve, reject) => {
              let response = await apiInventory.delete(record.id);
              if (requestIsSuccess(response)) {
                _this.refresh(false, _this.pageIndex);
                setTimeout(resolve, 100);
              } else {
                setTimeout(reject, 100);
              }
            });
          } else {
            return new Promise(async (resolve, reject) => {
              let response = await apiInventory.deleteRange(_this.selectedRowKeys);
              if (requestIsSuccess(response)) {
                _this.refresh(false, _this.pageIndex);
                setTimeout(resolve, 100);
              } else {
                setTimeout(reject, 100);
              }
            });
          }
        },
        onCancel() { },
      });
    },
    // 导出
    async export() {
      if (this.selectedRowKeys.length === 0) {
        this.$message.error('请选择需要导出的数据');
        return false;
      }
      let response = await apiInventory.export(this.selectedRowKeys);
      if (requestIsSuccess(response)) {
        if (response.data.byteLength != 0) {
          this.$message.info('库存信息导出成功');
          this.selectedRowKeys = [];
          FileSaver.saveAs(
            new Blob([response.data], { type: 'application/vnd.ms-excel' }),
            `库存信息表.xlsx`,
          );
        }
      }
    },

    // 分页事件
    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      this.queryParams.SkipCount = (this.pageIndex - 1) * this.queryParams.maxResultCount;
      if (page !== 0) {
        this.refresh(false);
      }
    },

    // 记录编辑
    edit(record) {
      this.$refs.InventoryDetailModal.edit(record);
    },

    // 查看
    view(record) {
      this.$refs.InventoryDetailModal.view(record);
    },

    //更新所选数据
    updateSelected(selectedRows) {
      if (this.multiple) {
        // 过滤出其他页面已经选中的
        let _selected = [];
        for (let item of this.iSelected) {
          let target = this.dataSource.find(subItem => subItem.id === item.id);
          if (!target) {
            _selected.push(item);
          }
        }

        // 把当前页面选中的加入
        for (let id of this.selectedRowKeys) {
          let inventory = this.dataSource.find(item => item.id === id);
          if (!!inventory) {
            _selected.push(JSON.parse(JSON.stringify(inventory)));
          }
        }

        this.iSelected = _selected;
      } else {
        this.iSelected = selectedRows;
      }
      this.$emit('change', this.iSelected);
    },
  },
  render() {
    return (
      <div class="sm-material-inventory">
        {/* 操作区 */}
        <sc-table-operator
          size={this.size}
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.keyword = '';
            this.queryParams.partitionId = undefined;//库存地点
            this.dateRange = [];
            this.refresh();
          }}
        >
          <a-form-item label="材料名称">
            <a-input
              placeholder="请输入材料名称"
              class="m-form"
              size={this.isSimple ? 'small' : 'default'}
              value={this.queryParams.keyword}
              onInput={event => {
                this.queryParams.keyword = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          {this.isSimple ? undefined :
            <a-form-item label="登记时间">
              <a-range-picker class="m-form"
                placeholder={['开始时间', '结束时间']}
                size={this.isSimple ? 'small' : 'default'}
                value={this.dateRange}
                onChange={value => {
                  this.dateRange = value;
                  this.refresh();
                }} />
            </a-form-item>}


          <a-form-item label="库存位置">
            <SmMaterialPartitalTreeSelect
              size={this.isSimple ? 'small' : 'default'}
              axios={this.axios}
              placeholder='请选择仓库地点'
              value={this.queryParams.partitionId}
              onChange={value => {
                this.queryParams.partitionId = value;
                this.refresh();
              }}
            />
          </a-form-item>
          {this.isSimple ? undefined :
            <template slot="buttons">
              <a-button
                disabled={this.selectedRowKeys <= 0}
                color="red"
                type="primary"
                onClick={() => this.export()}
              >
                导出
              </a-button>
            </template>}
        </sc-table-operator>

        <a-table
          columns={this.columns}
          dataSource={this.dataSource}
          rowKey={record => record.id}
          bordered={this.bordered}
          loading={this.loading}
          size={this.size}
          rowSelection={{
            type: this.multiple ? 'checkbox' : 'radio',
            columnWidth: 30,
            selectedRowKeys: this.selectedRowKeys,
            onChange: (selectedRowKeys, selectedRows) => {
              this.selectedRowKeys = selectedRowKeys;
              this.updateSelected(selectedRows);
            },
          }}
          scroll={this.isSimple ? { y: 300 } : undefined}

          pagination={false}
          {...{
            scopedSlots: {
              code: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              operations: (text, record) => {
                return [
                  <span>
                    <a-button style="padding:2px" onClick={() => this.view(record)} type="link">
                      查看
                    </a-button>
                  </span>,
                  record.amount <= 0 ? [
                    <a-divider type="vertical" />,
                    <span>
                      <a
                        href="javascript:;"
                        onClick={() => this.delete(record)}
                        style="color:red"
                      >
                        删除
                      </a>
                    </span>] : undefined,
                ];
              },
              entryTime: (text, record) => {
                return moment(record.entryTime).format('YYYY-MM-DD');
              },
            },
          }}
        ></a-table>

        {/* 分页器 */}

        <a-pagination
          style="margin-top:10px; text-align: right;"
          total={this.totalCount}
          pageSize={this.queryParams.maxResultCount}
          current={this.pageIndex}
          onChange={this.onPageChange}
          onShowSizeChange={this.onPageChange}
          showSizeChanger
          showQuickJumper
          size={this.isSimple ? 'small' : ''}
          showTotal={paginationConfig.showTotal}
        />

        <InventoryDetailModal
          ref="InventoryDetailModal"
          axios={this.axios}
          onSuccess={() => this.refresh()}
        />
      </div>
    );
  },
};
