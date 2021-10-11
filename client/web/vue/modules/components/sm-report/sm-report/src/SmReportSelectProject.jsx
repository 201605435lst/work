import { form as formConfig, tips } from '../../../_utils/config';
import { ModalStatus } from '../../../_utils/enum';
import * as utils from '../../../_utils/utils';
import { requestIsSuccess } from '../../../_utils/utils';
import ApiProject from '../../../sm-api/sm-project/Project';
import { pagination as paginationConfig, tips as tipsConfig } from '../../../_utils/config';

import '../style';

let apiProject = new ApiProject();

export default {
  name: 'SmReportSelectProject',
  props: {
    axios: { type: Function, default: null },
    keyWords: { type: String, default:null }, 
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      dataSource: [],//数据源
      selectedProjectIds: null,//表格的选中项
      pageIndex: 1,
      totalCount: 0,
      tableSelection: null,
      pageSize: paginationConfig.defaultPageSize,
      queryParams: {
        maxResultCount: paginationConfig.defaultPageSize,
      },
    };
  },
  computed: {
    visible() {
      // 计算模态框的显示变量k
      return this.status !== ModalStatus.Hide;
    },
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          ellipsis: true,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '项目编号',
          dataIndex: 'code',
          ellipsis: true,
          scopedSlots: { customRender: 'code' },
        },
        {
          title: '项目名称',
          ellipsis: true,
          dataIndex: 'name',
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '建设单位',
          ellipsis: true,
          dataIndex: 'organization',
          scopedSlots: { customRender: 'organization' },
        },
        {
          title: '项目类型',
          ellipsis: true,
          dataIndex: 'type',
          scopedSlots: { customRender: 'type' },
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
    keyWords: {
      handler: function (value, oldValue) {
        this.initAxios();
        this.refresh();
      },
      immediate: true,
    },

  },
  async created() {
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiProject = new ApiProject(this.axios);
    },
    select() {

    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let data = {
        ...this.queryParams,
        keywords:this.keyWords,
      };
      let response = await apiProject.getList({
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
      <div class="sm-report-new-report">
        {/* 展示区 */}
        <a-table
          columns={this.columns}
          dataSource={this.dataSource}
          rowKey={record => record.id}
          loading={this.loading}
          pagination={false}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                let result = index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                return <a-tooltip title={result}>{result}</a-tooltip>;
              },
              code: (text, record, index) => {
                console.log(record);
                let result = record.code;
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },

              name:(text, record, index) => {
                let result = record.name;
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              organization: (text, record, index) => {
                let result = record.organization ? record.organization.name : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              type:(text, record, index) => {
                let result = record.type ? record.type.name : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              operations: (text, record) => {
                return <span>
                  <a
                    onClick={() => {
                      this.$emit("recordProject",record);
                    }}
                  >
                    选择
                  </a>
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
      </div>
    );
  },
};


