import SmExport from '../../sm-common/sm-export-module';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import ApiContract from '../../sm-api/sm-oa/Contract';
import { requestIsSuccess, vP, vIf } from '../../_utils/utils';
import { reject } from 'lodash';
import moment from 'moment';
import permissionsSmOa from '../../_permissions/sm-oa';
import './style';
let apiContract = new ApiContract();
import FileSaver from 'file-saver';

export default {
  name: 'SmOaContracts',
  props: {
    permissions: { type: Array, default: () => [] },
    axios: { type: Function, default: null },
    isContract: { type: Boolean, default: true },
  },
  data() {
    return {
      dataSource: [],//数据源
      recordList: [],//引用的数据
      pageIndex: 1,
      totalCount: 0,
      isCanExport: true,//导出项是否禁用
      columnKey: "creationTime", //默认排序索引创建时间
      order: "descend",//默认排序方式降序
      selectedRowKeys: [],
      pageSize: paginationConfig.defaultPageSize,
      queryParams: {
        keyWords: null,//关键字
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
          title: '合同名称',
          dataIndex: 'name',
          ellipsis: true,
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '合同编号',
          dataIndex: 'code',
          ellipsis: true,
          scopedSlots: { customRender: 'code' },
        },
        {
          title: '合同类型',
          dataIndex: 'type',
          ellipsis: true,
          scopedSlots: { customRender: 'type' },
        },
        {
          title: '合同额(万)',
          ellipsis: true,
          width: 150,
          dataIndex: 'amount',
          scopedSlots: { customRender: 'amount' },
        },
        {
          title: '创建人',
          ellipsis: true,
          dataIndex: 'undertaker',
          scopedSlots: { customRender: 'undertaker' },
        },
        {
          title: '创建时间',
          ellipsis: true,
          width: 160,
          dataIndex: 'creationTime',
          sorter: () => { },
          scopedSlots: { customRender: 'creationTime' },
        },
        {
          title: '签订时间',
          ellipsis: true,
          dataIndex: 'signTime',
          width: 160,
          sorter: () => { },
          scopedSlots: { customRender: 'signTime' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  watch: {},
  async created() {
    this.initAxios();
    await this.refresh();
  },
  methods: {
    initAxios() {
      apiContract = new ApiContract(this.axios);
    },
    // 添加
    add() {
      this.$emit('add');
    },
    view(record) {
      this.$emit('view', record.id);
    },
    //引用
    apply(record) {
      if (!(record instanceof Array)) {
        let result = [];
        result.push({ ...record });
        record = result;
      }
      this.$emit('apply', record);
    },
    edit(record) {
      this.$emit('edit', record.id);
    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let data = {
        ...this.queryParams,
        columnKey: this.columnKey,
        order: this.order ? this.order : 'descend',
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
      if (this.dataSource.length > 0) {
        this.isCanExport = false;
      }
    },
    async export() {
      let _this = this;
      //导出按钮
      _this.isLoading = true;
      _this.$confirm({
        title: "确认导出",
        content: h => (
          <div style="color:red;">
            {this.selectedRowKeys.length == 0 ? '确定要导出全部数据吗？' : `确定要导出这 ${this.selectedRowKeys.length} 条数据吗？`}
          </div>
        ),
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiContract.downLoad(_this.selectedRowKeys);
            _this.isLoading = false;
            if (requestIsSuccess(response)) {
              FileSaver.saveAs(
                new Blob([response.data], { type: 'application/vnd.ms-excel' }),
                `合同管理表.xlsx`,
              );
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() { },
      });
    },
    remove(multiple, selectedRowKeys) {
      if (selectedRowKeys && selectedRowKeys.length > 0) {
        let _this = this;
        _this.$confirm({
          title: tipsConfig.remove.title,
          content: h => (
            <div style="color:red;">
              {multiple ? '确定要删除这几条数据？' : tipsConfig.remove.content}
            </div>
          ),
          okType: 'danger',
          onOk() {
            return new Promise(async (resolve, reject) => {
              let response = await apiContract.delete(selectedRowKeys);
              if (requestIsSuccess(response)) {
                if(multiple){
                  _this.refresh(true);
                }else{
                  _this.refresh(false, _this.pageIndex);
                }
                setTimeout(resolve, 100);
              } else {
                setTimeout(reject, 100);
              }
            });
          },
          onCancel() { },
        });
      }
      else {
        this.$message.error('请选择要删除的合同！');
      }
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
      <div class="sm-oa-contracts">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.keyWords = null;
            this.refresh();
          }}
        >
          <a-form-item label="关键字">
            <a-input
              allowClear
              placeholder="请输入合同名称或者合同编号"
              value={this.queryParams.keyWords}
              onInput={event => {
                this.queryParams.keyWords = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          <template slot="buttons">
            {this.isContract ?
              <div class="buttons">
                {vIf(
                  <a-button
                    type="primary"
                    icon="plus"
                    onClick={() => {
                      this.add();
                    }}
                  >
                    新建
                  </a-button>,
                  vP(this.permissions, permissionsSmOa.Contracts.Create),
                )}
                {vIf(
                  <a-button
                    type="danger"
                    onClick={() => {
                      this.remove(true, this.selectedRowKeys);
                    }} >
                    <si-ashbin />
                   批量删除
                  </a-button>,
                  vP(this.permissions, permissionsSmOa.Contracts.Delete),
                )}
                {vIf(
                  <a-button
                    type="primary"
                    onClick={this.export}
                    disabled={this.isCanExport}>
                    <a-icon type="download" /> 导出
                  </a-button>,
                  vP(this.permissions, permissionsSmOa.Contracts.Export),
                )}
              </div>
              : <span>
                <a-button
                  type="primary"
                  icon="plus"
                  onClick={() => {
                    this.apply(this.recordList);
                  }}
                  disabled={this.selectedRowKeys.length == 0}
                  size="small"
                >
                      批量引用
                </a-button>
              </span>}
          </template>

        </sc-table-operator>

        {/* 展示区 */}
        <a-table
          columns={this.columns}
          dataSource={this.dataSource}
          rowKey={record => record.id}
          loading={this.loading}
          pagination={false}
          onChange={(a, b, c) => {
            this.columnKey = c.columnKey;
            this.order = c.order;
            this.refresh();
          }}
          rowSelection={{
            onChange: (selectedRowKeys, recordList) => {
              this.recordList = recordList;
              this.selectedRowKeys = selectedRowKeys;
            },
          }}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                let result = index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                return <a-tooltip title={result}>{result}</a-tooltip>;
              },
              name: (text, record, index) => {
                let result = record.name;
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              code: (text, record, index) => {
                let result = record.code;
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              type: (text, record, index) => {
                let result = record.type ? record.type.name : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              amount: (text, record, index) => {
                let result = record.amount;
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              undertaker: (text, record, index) => {
                let result = record.undertaker ? record.undertaker.name : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              creationTime: (text, record, index) => {
                let result = moment(record.creationTime).format("YYYY-MM-DD HH:mm:ss");
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              signTime: (text, record, index) => {

                let result = moment(record.signTime).format("YYYY-MM-DD HH:mm:ss");
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              operations: (text, record) => {
                return <span>
                  {vIf(
                    <a
                      onClick={() => {
                        this.view(record);
                      }}
                    >
                      详情
                    </a>,
                    vP(this.permissions, permissionsSmOa.Contracts.Detail),
                  )}
                  {vIf(
                    <a-divider type="vertical" />,
                    this.isContract ?
                      vP(this.permissions, permissionsSmOa.Contracts.Detail) &&
                      vP(this.permissions, [permissionsSmOa.Contracts.Update, permissionsSmOa.Contracts.Delete]) :
                      vP(this.permissions, permissionsSmOa.Contracts.Detail) &&
                      vP(this.permissions, permissionsSmOa.Contracts.Apply),
                  )}
                  {this.isContract ?
                    <span>
                      {vIf(
                        <a-dropdown trigger={['click']}>
                          <a
                            class="ant-dropdown-link"
                            onClick={e => e.preventDefault()}>
                            更多 <a-icon type="down" />
                          </a>
                          <a-menu slot="overlay">
                            {vIf(
                              <a-menu-item>
                                <a
                                  onClick={() => {
                                    this.edit(record);
                                  }}
                                >编辑
                                </a>
                              </a-menu-item>,
                              vP(this.permissions, permissionsSmOa.Contracts.Update),
                            )}
                            {vIf(
                              <a-menu-item>
                                <a
                                  onClick={() => {
                                    this.remove(false, record.id);
                                  }}
                                >
                                  删除
                                </a>
                              </a-menu-item>,
                              vP(this.permissions, permissionsSmOa.Contracts.Delete),
                            )}
                          </a-menu>
                        </a-dropdown>,
                        vP(this.permissions, [permissionsSmOa.Contracts.Update, permissionsSmOa.Contracts.Delete]),
                      )}</span> :
                    <span>
                      <a onClick={() => {
                        this.apply(record);
                      }}>引用</a>
                    </span>
                  } </span>;
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
      </div >
    );
  },
};
