import './style';
import * as utils from '../../_utils/utils';
import ApiReport from '../../sm-api/sm-report/report';
import ApiUser from '../../sm-api/sm-system/User';
import { ReportType } from '../../_utils/enum';
import moment from 'moment';
import permissionsSmReport from '../../_permissions/sm-report';
import { requestIsSuccess, getReportTypeTitle, vIf, vP } from '../../_utils/utils';
import FileSaver from 'file-saver';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
let apiReport = new ApiReport();
let apiUser = new ApiUser();

export default {
  name: 'SmReports',
  props: {
    permissions: { type: Array, default: () => [] },
    axios: { type: Function, default: null },
    reportsType: { type: String, default: null },
  },
  data() {
    return {
      dataSource: [],//数据源
      pageIndex: 1,
      totalCount: 0,
      isCanExport: true,//导出项是否禁用
      selectedRowKeys: [],
      pageSize: paginationConfig.defaultPageSize,
      queryParams: {
        keyWords: null,//关键字
        type: 0,//类型
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
          title: '汇报类型',
          dataIndex: 'type',
          ellipsis: true,
          scopedSlots: { customRender: 'type' },
        },
        {
          title: '标题',
          dataIndex: 'title',
          ellipsis: true,
          scopedSlots: { customRender: 'title' },
        },
        {
          title: '汇报日期',
          ellipsis: true,
          dataIndex: 'date',
          scopedSlots: { customRender: 'date' },
        },
        {
          title: '组织机构',
          ellipsis: true,
          dataIndex: 'organization',
          scopedSlots: { customRender: 'organization' },
        },
        this.reportsType == "receive" ?
          {
            title: '汇报人',
            ellipsis: true,
            dataIndex: 'creatorName',
            scopedSlots: { customRender: 'creatorName' },
          }:{},
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
      apiReport = new ApiReport(this.axios);
      apiUser = new ApiUser(this.axios);
    },
    // 添加
    add() {
      this.$emit('add');
    },
    view(record) {
      this.$emit('view', record.id);
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
      };
      let response = await apiReport.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...data,
        reportsType: this.reportsType,
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
            {_this.selectedRowKeys.length == 0 ? '确定要导出全部数据吗？' : `确定要导出这 ${_this.selectedRowKeys.length} 条数据吗？`}
          </div>
        ),
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let data={
              keyWords:_this.queryParams.keyWords,
              reportsType: _this.reportsType,
              ids:_this.selectedRowKeys,
              type:_this.queryParams.type,
            };
            let response = await apiReport.export({...data});
            _this.isLoading = false;
            if (requestIsSuccess(response)) {
              FileSaver.saveAs(
                new Blob([response.data], { type: 'application/vnd.ms-excel' }),
                `项目工作汇报表.xlsx`,
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
              let response = await apiReport.delete(selectedRowKeys);
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
        this.$message.error('请选择要删除的汇报内容！');
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
      <div class="sm-reports">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.keyWords = null;
            this.queryParams.type = 0;
            this.refresh();
          }}
        >
          <a-form-item label="关键字">
            <a-input
              allowClear
              placeholder="请输入标题"
              value={this.queryParams.keyWords}
              onInput={event => {
                this.queryParams.keyWords = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="汇报类型">
            <div class="type-button">
              <a-radio-group value={this.queryParams.type} button-style="solid">
                <a-radio-button
                  onClick={(event) => {
                    this.queryParams.type = parseInt(event.target.value);
                    this.refresh();
                  }}
                  class="button"
                  size="small"
                  value={0}
                >
                  全部
                </a-radio-button>
                <a-radio-button
                  onClick={(event) => {
                    this.queryParams.type = parseInt(event.target.value);
                    this.refresh();
                  }}
                  class="button"
                  size="small"
                  value={1}
                >
                  日报
                </a-radio-button>
                <a-radio-button
                  onClick={(event) => {
                    this.queryParams.type = parseInt(event.target.value);
                    this.refresh();
                  }}
                  class="button"
                  size="small"
                  value={2}
                >
                  周报
                </a-radio-button>
                <a-radio-button
                  onClick={(event) => {
                    this.queryParams.type = parseInt(event.target.value);
                    this.refresh();
                  }}
                  class="button"
                  size="small"
                  value={3}
                >
                  月报
                </a-radio-button>
              </a-radio-group>
            </div>
          </a-form-item>

          <template slot="buttons">
            <div class="buttons">
              {/* {this.reportsType == "send" ? */}
              <span>
                {this.reportsType == "send" || !this.reportsType ?
                  <span>
                    {vIf(
                      <a-button
                        type="primary"
                        icon="plus"
                        onClick={() => {
                          this.add();
                        }}
                      > 新建
                      </a-button>,
                      vP(this.permissions, permissionsSmReport.Reports.Create),
                    )}</span> : ''}
                {this.reportsType == "send" || !this.reportsType ?
                  <span>
                    {vIf(
                      <a-button
                        type="danger"
                        onClick={() => {
                          this.remove(true, this.selectedRowKeys);
                        }} >
                        <si-ashbin />
                        批量删除
                      </a-button>,
                      vP(this.permissions, permissionsSmReport.Reports.Delete),
                    )}</span> : ''}
                <span>
                  {vIf(
                    <a-button
                      type="primary"
                      onClick={this.export}
                      disabled={this.isCanExport}>
                      <a-icon type="download" /> 导出
                    </a-button>,
                    vP(this.permissions, permissionsSmReport.Reports.Export),
                  )}</span>
              </span>

            </div>
          </template>

        </sc-table-operator>

        {/* 展示区 */}
        <a-table
          columns={this.columns}
          dataSource={this.dataSource}
          rowKey={record => record.id}
          loading={this.loading}
          pagination={false}
          rowSelection={{
            onChange: (selectedRowKeys, recordList) => {
              this.selectedRowKeys = selectedRowKeys;
            },
          }}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                let result = index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                return <a-tooltip title={result}>{result}</a-tooltip>;
              },
              type: (text, record, index) => {
                let result = record && record.type ? getReportTypeTitle(parseInt(record.type)) : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              title: (text, record, index) => {

                let result = record ? record.title : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              date: (text, record, index) => {
                let result = moment(record.date).format("YYYY-MM-DD");
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              organization: (text, record, index) => {
                let result = record && record.organization ? record.organization.name : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              creatorName: (text, record, index) => {
                let result = record ? record.creatorName : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              operations: (text, record) => {
                return <span>
                  <span class="detail-icon">
                    {vIf(
                      <a-tooltip placement="top" title="详情">
                        <a
                          onClick={() => {
                            this.view(record);
                          }}
                        ><a-icon type="eye" class="icon" />
                        </a>
                      </a-tooltip>,
                      vP(this.permissions, permissionsSmReport.Reports.Detail),
                    )}</span>
                  {this.reportsType == "receive" ?
                    null
                    :
                    <span>
                      <span class="edit-icon">
                        {vIf(
                          <a-tooltip placement="top" title="编辑">
                            <a
                              onClick={() => {
                                this.edit(record);
                              }}
                            ><a-icon type="edit" color="green" />
                            </a>
                          </a-tooltip>,
                          vP(this.permissions, permissionsSmReport.Reports.Update),
                        )}</span>
                      <span class="edit-icon">
                        {vIf(
                          <a-tooltip placement="top" title="删除">
                            <a
                              onClick={() => {
                                this.remove(false, record.id);
                              }}
                            ><si-ashbin color="red" size={22} />
                            </a>
                          </a-tooltip>,
                          vP(this.permissions, permissionsSmReport.Reports.Delete),
                        )}</span>
                    </span>
                  }
                  
                  
               
                </span>;
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
