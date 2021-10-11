import './style';
import { requestIsSuccess, vP, vIf } from '../../_utils/utils';
import ApiContract from '../../sm-api/sm-costmanagement/Contract';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import permissionsCostmanagement from '../../_permissions/sm-costmanagement';
import SmCostmanagementContractModal from './SmCostmanagementContractModal';

import moment from 'moment';
let apiContract = new ApiContract();

export default {
  name: 'SmCostmanagementContract',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      dataSource: [],
      loading: false,
      totalCount: 0,
      record: null,
      recordList: [],
      selectedRowKeys: [],
      pageIndex: 1,
      queryParams: {
        typeId: undefined, //id
        name: null, //付款单位id
        startTime: null,
        endTime: null,
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
          width:70,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '合同编码',
          dataIndex: 'code',
          ellipsis: true,
          scopedSlots: { customRender: 'code' },
        },
       
        {
          title: '合同名称',
          ellipsis: true,
          dataIndex: 'name',
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '合同类型',
          ellipsis: true,
          dataIndex: 'type',
          scopedSlots: { customRender: 'type' },
        },
        {
          title: '合同日期',
          dataIndex: 'date',
          ellipsis: true,
          scopedSlots: { customRender: 'date' },
        },
        {
          title: '合同金额(万元)',
          dataIndex: 'money',
          ellipsis: true,
          scopedSlots: { customRender: 'money' },
        },
        {
          title: '上传人',
          dataIndex: 'peopleName',
          ellipsis: true,
          scopedSlots: { customRender: 'peopleName' },
        },
        {
          title: '备注',
          dataIndex: 'remark',
          ellipsis: true,
          scopedSlots: { customRender: 'remark' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 169,
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
      apiContract = new ApiContract(this.axios);
    },
    
    // 添加
    add() {
      this.$refs.SmCostmanagementContractModal.add();
    },
    // 详情
    view(record) {
      this.$refs.SmCostmanagementContractModal.view(record);
    },
    // 编辑
    edit(record) {
      this.$refs.SmCostmanagementContractModal.edit(record);
    },
    // 删除
    remove(multiple, selectedIds) {
      if (selectedIds && selectedIds.length > 0) {
        let _this = this;
        this.$confirm({
          title: tipsConfig.remove.title,
          content: h => (
            <div style="color:red;">
              {multiple ? '确定要删除这几条数据？' : tipsConfig.remove.content}
            </div>
          ),
          okType: 'danger',
          onOk() {
            return new Promise(async (resolve, reject) => {
              let response = await apiContract.delete(selectedIds);
              if (requestIsSuccess(response)) {
                _this.refresh(false, _this.pageIndex);
                setTimeout(resolve, 100);
              } else {
                setTimeout(reject, 100);
              }
            });
          },
          onCancel() { },
        });
      } else {
        this.$message.error('请选择要删除的数据！');
      }
    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let data = {
        ...this.queryParams,
        startTime: this.queryParams.startTime
          ? moment(this.queryParams.startTime)
            .format('YYYY-MM')
          : '',
        endTime: this.queryParams.endTime
          ? moment(this.queryParams.endTime)
            .add(1, 'months')
            .format('YYYY-MM')
          : '',
      };
      let response = await apiContract.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...data,
      });
     
      if (requestIsSuccess(response)) {
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
      this.loading = false;
    },
    //切换页码
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
      <div class="sm-costmanagement-contract">
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.endTime = null;
            this.queryParams.startTime = null;
            this.queryParams.name = undefined;
            this.queryParams.typeId = undefined;
            this.refresh();
          }}
        >
          <a-form-item label="合同名称">
            <a-input
              placeholder="请选择"
              value={this.queryParams.name}
              onChange={event => {
                this.queryParams.name = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="合同类型">
            <DataDictionaryTreeSelect
              axios={this.axios}
              groupCode={'CostmanagementContract'}
              placeholder="请选择"
              value={this.queryParams.typeId}
              onChange={value => {
                this.queryParams.typeId = value;
                this.refresh();
              }}
            />
          </a-form-item>
          <div class="costmanagement-contract-date">
            <a-form-item label="日期选择">
              <a-month-picker
                style="width: 100%;"
                placeholder="开始时间"
                allowClear={false}
                value={this.queryParams.startTime}
                onChange={value => {
                  this.queryParams.startTime = value;
                  this.refresh();
                }}
              />
              <span class="month-picker_">-</span>
              <a-month-picker
                style="width: 100%;"
                allowClear={false}
                value={this.queryParams.endTime}
                onChange={value => {
                  this.queryParams.endTime = value;
                  this.refresh();
                }}
                placeholder="结束时间"
              />
            </a-form-item>
            
          </div>
       
          <template slot="buttons">
            {vIf(
              <a-button type="primary"  onClick={() => this.add()}>
                新建
              </a-button>,
              vP(this.permissions, permissionsCostmanagement.Contracts.Create),
            )}
            {vIf( 
              <a-button type="danger"  onClick={() => this.remove(true, this.selectedRowKeys)}>
                删除
              </a-button>,
              vP(this.permissions, permissionsCostmanagement.Contracts.Delete),
            )}
          </template>
        </sc-table-operator>
        <a-table
          columns={this.columns}
          rowKey={record => record.id}
          dataSource={this.dataSource}
          bordered={this.bordered}
          pagination={false}
          loading={this.loading}
          rowSelection={{
            onChange: (selectedRowKeys, recordList) => {
              this.recordList = recordList;
              this.selectedRowKeys = selectedRowKeys;
            },
          }}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              code: (text, record) => {
                let result=record?record.code:'';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span 
                      class="view-action"
                      onClick={() => {
                        this.view(record);
                      }}
                    >
                      {result}</span>
                  </a-tooltip>
                );
              },
              name: (text, record) => {
                return (
                  <a-tooltip placement="topLeft" title={record.name}>
                    <span>{record.name}</span>
                  </a-tooltip>
                );
              },
              type: (text, record) => {
                let result=record.type?record.type.name:'';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              date: (text, record) => {
                let result = record.date ? moment(record.date).format('YYYY-MM-DD') : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              money: (text, record) => {
                return (
                  <a-tooltip placement="topLeft" title={record.money}>
                    <span>{record.money}</span>
                  </a-tooltip>
                );
              },
              peopleName: (text, record) => {
                let result=record && record.creator?record.creator.name:'';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              remark: (text, record) => {
                return (
                  <a-tooltip placement="topLeft" title={record.remark}>
                    <span>{record.remark}</span>
                  </a-tooltip>
                );
              },
              operations: (text, record) => {
                return [
                  <span>
                    {vIf(
                      <a
                        onClick={() => {
                          this.edit(record);
                        }}
                      >编辑
                      </a> ,
                      vP(this.permissions, permissionsCostmanagement.Contracts.Update),
                    )}
                    {vIf(
                      <a-divider type="vertical" /> ,
                      ( vP(this.permissions, permissionsCostmanagement.Contracts.Update) &&
                        vP(this.permissions, permissionsCostmanagement.Contracts.Delete)), 
                    )}
                    {vIf( 
                      <a
                        disabled={record.diaryCode ? true : false}
                        onClick={() => {
                          this.remove(false, [record.id]);
                        }}
                      >
                        删除
                      </a> ,
                      vP(this.permissions, permissionsCostmanagement.Contracts.Delete),
                    )} 
                  </span>,
                ];
              },
            },
          }}
        ></a-table>
        {/* 添加/编辑模板 */}
        <SmCostmanagementContractModal
          ref="SmCostmanagementContractModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
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
