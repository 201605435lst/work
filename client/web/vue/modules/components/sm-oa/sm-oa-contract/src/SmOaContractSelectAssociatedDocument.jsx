import { form as formConfig, tips } from '../../../_utils/config';
import { ModalStatus } from '../../../_utils/enum';
import * as utils from '../../../_utils/utils';
import { requestIsSuccess } from '../../../_utils/utils';
import ApiContract from '../../../sm-api/sm-oa/Contract';
import { pagination as paginationConfig, tips as tipsConfig } from '../../../_utils/config';

import '../style';

let apiContract = new ApiContract();

export default {
  name: 'SmOaContractSelectAssociatedDocument',
  props: {
    axios: { type: Function, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      dataSource: [],//数据源
      selectedDocumentIds:null,//表格的选中项
      pageIndex: 1,
      totalCount: 0,
      tableSelection:null,
      pageSize: paginationConfig.defaultPageSize,
      queryParams: {
        keyWords: null,//关键字
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
          title: '主题',
          dataIndex: 'zt',
          ellipsis: true,
          scopedSlots: { customRender: 'zt' },
        },
        {
          title: '创建人',
          ellipsis: true,
          dataIndex: 'cr',
          scopedSlots: { customRender: 'cr' },
        },
        {
          title: '创建时间',
          ellipsis: true,
          dataIndex: 'crtm',
          scopedSlots: { customRender: 'crtm' },
        },
      ];
    },
    
  },
  watch: {},
  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
    this.refresh();
  },
  methods: {
    initAxios() {
      apiContract = new ApiContract(this.axios);
    },
    select(){

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
      // let response = await api.getList({
      //   skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
      //   ...data,
      // });
      // if (requestIsSuccess(response)) {
      //   this.dataSource = response.data.items;
      //   this.totalCount = response.data.totalCount;
      //   if (page && this.totalCount && this.queryParams.maxResultCount) {
      //     let currentPage = parseInt(this.totalCount / this.queryParams.maxResultCount);
      //     if (this.totalCount % this.queryParams.maxResultCount !== 0) {
      //       currentPage = page + 1;
      //     }
      //     if (page > currentPage) {
      //       this.pageIndex = currentPage;
      //       this.refresh(false, this.pageIndex);
      //     }
      //   }
      // }
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
      <div class="sm-oa-contract-new-contract">
        {/* 展示区 */}
        <a-table
          columns={this.columns}
          dataSource={this.dataSource}
          rowKey={record => record.id}
          loading={this.loading}
          pagination={false}
          rowSelection={{
            onChange: (selectedRowKeys) => {
              this.selectedDocumentIds = selectedRowKeys;
            },
          }}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                let result = index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                return <a-tooltip title={result}>{result}</a-tooltip>;
              },
              zt: (text, record, index) => {
                console.log(record);
                let result = index;
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
             
              cr: async (text, record, index) => {
                let result = index;
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              crtm: (text, record, index) => {
                console.log(record);
                let result = index;
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
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


