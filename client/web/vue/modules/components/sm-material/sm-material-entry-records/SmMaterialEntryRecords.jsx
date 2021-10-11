import './style';
import { requestIsSuccess, getSupplierType, vIf, vP } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmMaterialPartitalTreeSelect from '../sm-material-partital-tree-select';
import SmMaterialEntryRecordModal from './SmMaterialEntryRecordModal';
import ApiEntryRecord from '../../sm-api/sm-material/EntryRecord';
import permissionsSmMaterial from '../../_permissions/sm-material';
import FileSaver from 'file-saver';
import moment from 'moment';
import ApiDispatch from '../../sm-api/sm-construction/Dispatch';
let apiDispatch = new ApiDispatch();

let apiEntryRecord = new ApiEntryRecord();

export default {
  name: 'SmMaterialEntryRecords',
  props: {
    permissions: { type: Array, default: () => [] },
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
  },
  data() {
    return {
      entryRecords: [], // 列表数据源
      totalCount: 0,
      pageIndex: 1,
      pageSize: paginationConfig.defaultPageSize,
      queryParams: {
        keyword: '', // 模糊查询
        partitionId: null, // 仓库地点
        maxResultCount: paginationConfig.defaultPageSize,
      },
      dateRange: [],
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          ellipsis: true,
          width: 100,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '编号',
          dataIndex: 'code',
          ellipsis: true,
        },
        {
          title: '仓库',
          dataIndex: 'partition.name',
          ellipsis: true,
        },
        {
          title: '项目',
          dataIndex: 'project.name',
          ellipsis: true,
        },
        {
          title: '登记人',
          dataIndex: 'creator.name',
        },
        {
          title: '入库时间',
          dataIndex: 'time',
          scopedSlots: { customRender: 'time' },
          ellipsis: true,
        },
        {
          title: '备注',
          dataIndex: 'remark',
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
      apiEntryRecord = new ApiEntryRecord(this.axios);
      apiDispatch = new ApiDispatch(this.axios);
    },

    //添加
    add() {
      this.$refs.SmMaterialEntryRecordModal.add();
    },

    //详情
    view(record) {
      this.$refs.SmMaterialEntryRecordModal.view(record);
    },

    // 导出
    async export(id) {
      let response = await apiEntryRecord.export(id);
      if (requestIsSuccess(response)) {
        if (response.data.byteLength != 0) {
          this.$message.info('导出成功');
          FileSaver.saveAs(
            new Blob([response.data], { type: 'application/vnd.ms-excel' }),
            `物资入库明细表.docx`,
          );
        }
      }
    },

    // 刷新列表
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }

      let response = await apiEntryRecord.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
        sTime: this.dateRange.length > 0 ? moment(this.dateRange[0]).hours(0).minutes(0).seconds(0).format('YYYY-MM-DD HH:mm:ss') : '',
        eTime: this.dateRange.length > 0 ? moment(this.dateRange[1]).hours(23).minutes(59).seconds(59).format('YYYY-MM-DD HH:mm:ss') : '',
      });
      if (requestIsSuccess(response) && response.data) {
        this.entryRecords = response.data.items;
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
      <div class="sm-material-entry-records">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.keyword = ''; // 模糊查询
            this.queryParams.partitionId = null; // 仓库地点
            this.dateRange = [];
            this.refresh();
          }}
        >
          <a-form-item label="仓库分区">
            <SmMaterialPartitalTreeSelect
              axios={this.axios}
              placeholder='请选择仓库地点'
              value={this.queryParams.partitionId}
              onChange={value => { this.queryParams.partitionId = value; this.refresh(); }}
            />
          </a-form-item>

          <a-form-item label="入库时间">
            <a-range-picker
              style="width: 100%"
              allowClear={false}
              placeholder={['开始时间', '结束时间']}
              value={this.dateRange}
              onChange={value => {
                this.dateRange = value;
                this.refresh();
              }}
            />
          </a-form-item>

          <a-form-item label="关键字">
            <a-input
              style="margin-right:10px;"
              placeholder="请输入备注、编号"
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
                入库登记
              </a-button>,
              vP(this.permissions, permissionsSmMaterial.EntryRecords.Create),
            )}
            {/* {vIf(
              <a-button
                style="background-color: #52c41a; color: #fff;"
                onClick={this.export}
                loading={this.loading}
              >
                导出
              </a-button>,
              vP(this.permissions, permissionsSmMaterial.Suppliers.Export),
            )} */}
          </template>



        </sc-table-operator>
        {/* 展示区 */}
        <a-table
          columns={this.columns}
          dataSource={this.entryRecords}
          rowKey={record => record.id}
          bordered={this.bordered}
          loading={this.loading}
          pagination={false}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              partition: (text, record, index) => {
                return record.partition ? record.partition.name : '';
              },

              project: (text, record, index) => {
                return record.project ? record.project.name : '';
              },

              creator: (text, record, index) => {
                return record.creator ? record.creator.name : '';
              },

              time: (text, record, index) => {
                return text ? moment(text).format('YYYY-MM-DD HH:mm:ss') : '';
              },

              operations: (text, record) => {
                return [
                  <span>
                    {vIf(
                      <a
                        onClick={() => {
                          this.view(record);
                        }}
                      >
                        详情
                      </a>,
                      vP(this.permissions, permissionsSmMaterial.EntryRecords.Detail),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsSmMaterial.EntryRecords.Detail) &&
                      vP(this.permissions, permissionsSmMaterial.EntryRecords.Export),
                    )}

                    {vIf(
                      <a
                        onClick={() => {
                          this.export(record.id);
                        }}
                      >
                        导出
                      </a>,
                      vP(this.permissions, permissionsSmMaterial.EntryRecords.Export),
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

        {/* 添加/查看模板 */}
        <SmMaterialEntryRecordModal
          ref="SmMaterialEntryRecordModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
      </div>
    );
  },
};
