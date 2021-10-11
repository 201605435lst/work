import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import { requestIsSuccess, getSafetyRiskLevel } from '../../_utils/utils';
import SmSafeProblemReportModal from '../sm-safe-problem/SmSafeProblemReportModal';
import SmQualityProblemReportModal from '../../sm-quality/sm-quality-problems/SmQualityProblemReportModal';
import ApiSafeProblem from '../../sm-api/sm-safe/Problem';
import ApiQualityProblem from '../../sm-api/sm-quality/QualityProblem';
let apiSafeProblem = new ApiSafeProblem();
let apiQualityProblem = new ApiQualityProblem();
import moment from 'moment';
import './style/index';
export default {
  name: 'SmSafeRltQualityModalSelectModal',
  props: {
    axios: { type: Function, default: null },
    value: { type: String, default: undefined }, //值
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      dataSource: [],
      loading: false,
      totalCount: 0,
      record: null,
      selectedRowKeys: [],
      selectedRows: [],
      pageIndex: 1,
      filterType: 1,
      api: null,
      type: null,
      iValue: undefined,
      queryParams: {
        title: null,
        maxResultCount: paginationConfig.defaultPageSize,
      },
      dateRange: [],
    };
  },
  computed: {
    visible() {
      // 计算模态框的显示变量k
      return this.status !== ModalStatus.Hide;
    },
    columns() {
      let col = [
        {
          title: '序号',
          dataIndex: 'index',
          ellipsis: true,
          width: 80,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '标题',
          dataIndex: 'title',
          ellipsis: true,
          scopedSlots: { customRender: 'title' },
        },
        {
          title: '所属专业',
          dataIndex: 'profession',
          ellipsis: true,
          scopedSlots: { customRender: 'profession' },
        },
        {
          title: '检查时间',
          ellipsis: true,
          dataIndex: 'checkTime',
          scopedSlots: { customRender: 'checkTime' },
        },
        {
          title: '检查人',
          dataIndex: 'checker',
          ellipsis: true,
          scopedSlots: { customRender: 'checker' },
        },

      ];
      if (this.type) {
        col = [
          ...col,
          {
            title: '操作',
            dataIndex: 'operations',
            scopedSlots: { customRender: 'operations' },
          },
        ];
      }
      return col;
    },
  },
  watch: {
    value: {
      handler: function (n, o) {
        this.iValue = n;
      },
      immediate: true,
    },
  },
  async created() {
  },

  methods: {
    add(api, type, data) {
      this.selectedRowKeys = data.map(item => item.id);
     
      this.api = api;
      this.type = type;
      this.status = ModalStatus.Add;
      this.refresh();
    },
    // 关闭模态框
    close() {
      this.record = null;
      this.selectedRowKeys = [];
      this.selectedRows = [];
      this.status = ModalStatus.Hide;
    },
    //查看
    view(record) {
      if (this.type == 'safe') {
        this.$refs.SmSafeProblemReportModal.view(record);
      }
      if (this.type == 'quality') {
        this.$refs.SmQualityProblemReportModal.view(record);
      }
    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let data = {
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      };
      let response = await this.api.getWaitingImproveList(data);
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
    // 数据提交
    ok() {
      if(this.selectedRows.length==0 && this.selectedRowKeys.length>0 ){
        let _selectedRows=[];
        this.selectedRowKeys.map(a => {
          this.dataSource.map(item=>{
            if(item.id==a){
              _selectedRows=[item,..._selectedRows]
            }
          })
        });
        this.selectedRows=_selectedRows;
        console.log(this.selectedRows);
      }
      console.log(this.selectedRows);
      if (this.type == 'safe') {
        
        this.$emit('safe', this.selectedRows);
      }
      if (this.type == 'quality') {
        this.$emit('quality', this.selectedRows);
      }
      this.close();
    },
    /* 页面呈现内容分类 */
    changeItem(e) {
      this.filterType = e.target.value;
      this.refresh();
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
      <a-modal
        class="sm-safe-rlt-quality-modal-select-modal"
        title={`问题选择`}
        visible={this.visible}
        onCancel={this.close}
        confirmLoading={this.loading}
        destroyOnClose={true}
        okText={'确定'}
        onOk={this.ok}
        width={900}
      >
        {/* 筛选 */}
        <sc-table-operator
          size="small"
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.title = ''; // 关键字
            this.refresh();
          }}
        >
          <a-form-item label="关键字">
            <a-input
              size="small"
              placeholder="请输入标题"
              value={this.queryParams.title}
              onInput={event => {
                this.queryParams.title = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
        </sc-table-operator>

        <a-table
          columns={this.columns}
          size="small"
          rowKey={record => record.id}
          dataSource={this.dataSource}
          bordered={false}
          pagination={false}
          loading={this.loading}
          rowSelection={
            {
              columnWidth: 30,
              selectedRowKeys: this.selectedRowKeys,
              onChange: (selectedRowKeys, selectedRows) => {
                this.selectedRowKeys = selectedRowKeys;
                this.selectedRows = selectedRows;
              },
            }
          }
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              profession: (text, record, index) => {
                let profession = record.profession ? record.profession.name : '';
                return (
                  <a-tooltip placement="topLeft" title={profession}>
                    <span>{profession}</span>
                  </a-tooltip>
                );
              },
              riskLevel: (text, record, index) => {
                let riskLevel = record.riskLevel ? getSafetyRiskLevel(record.riskLevel) : '';
                return (
                  <a-tooltip placement="topLeft" title={riskLevel}>
                    <span>{riskLevel}</span>
                  </a-tooltip>
                );
              },
              type: (text, record) => {
                let title = record && record.type ? record.type.name : '';
                return (
                  <a-tooltip placement="topLeft" title={title}>
                    <span>{title}</span>
                  </a-tooltip>
                );
              },
              checkTime: (text, record) => {
                let result = record && record.checkTime ? moment(record.checkTime).format('YYYY-MM-DD') : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>
                      {result}
                    </span>
                  </a-tooltip>
                );
              },
              checker: (text, record) => {
                let result = record && record.checker ? record.checker.name : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              operations: (text, record) => {
                return [
                  <span>

                    <a
                      onClick={() => {
                        this.view(record);
                      }}
                    >查看
                    </a>
                  </span>,
                ];
              },
            },
          }}
        ></a-table>
        {/* 分页器 */}
        <a-pagination
          size="small"
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
        {/* 问题报告模板 */}
        <SmSafeProblemReportModal
          ref="SmSafeProblemReportModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
        {/* 问题报告模板 */}
        <SmQualityProblemReportModal
          ref="SmQualityProblemReportModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
      </a-modal>
    );
  },
};
