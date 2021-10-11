import './style';
import ApiEntity from '../../sm-api/sm-namespace/Entity';
import { requestIsSuccess, vP, vIf, getInterfaceCheckSituation } from '../../_utils/utils';
import ApiCostPeople from '../../sm-api/sm-costmanagement/CostPeople';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import { InterfaceCheckSituation } from '../../_utils/enum';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';

let apiEntity = new ApiEntity();

export default {
  name: 'SmTechnologyInterfaceListing',
  props: {
    axios: { type: Function, default: null },
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
      tablePane: null, //表格切换项名---------a-tab-pane =============key
      queryParams: {
        jobLocationId: undefined, //施工地点
        professionalId: undefined, //专业
        unitId: undefined, //土建单位
        checkSituation: undefined, //检查情况
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
          title: '施工地点',
          dataIndex: 'jobLocation',
          ellipsis: true,
          scopedSlots: { customRender: 'jobLocation' },
        },

        {
          title: '专业',
          ellipsis: true,
          dataIndex: 'professional',
          scopedSlots: { customRender: 'professional' },
        },
        {
          title: '接口位置',
          ellipsis: true,
          dataIndex: 'position',
          scopedSlots: { customRender: 'position' },
        },
        {
          title: '材料/规格',
          dataIndex: 'materialSpecification',
          ellipsis: true,
          scopedSlots: { customRender: 'materialSpecification' },
        },
        {
          title: '数量',
          dataIndex: 'count',
          ellipsis: true,
          scopedSlots: { customRender: 'count' },
        },
        {
          title: '接口编号',
          dataIndex: 'interfaceCode',
          ellipsis: true,
          scopedSlots: { customRender: 'interfaceCode' },
        },
        {
          title: '土建单位',
          dataIndex: 'unit',
          ellipsis: true,
          scopedSlots: { customRender: 'unit' },
        },
        {
          title: '检查情况',
          ellipsis: true,
          dataIndex: 'checkSituation',
          scopedSlots: { customRender: 'checkSituation' },
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
      apiEntity = new ApiEntity(this.axios);
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
      let response = await apiEntity.getList({
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
    exprot() {},
    callback(key) {
      console.log(key);
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
    let Options = [];
    for (let item in InterfaceCheckSituation) {
      Options.push(
        <a-select-option key={`${InterfaceCheckSituation[item]}`}>
          {getInterfaceCheckSituation(InterfaceCheckSituation[item])}
        </a-select-option>,
      );
    }
    let tableData = [];
    tableData = (
      <div class="table">
        <div class="table-action">
          <a-form-item label="施工地点">
            <DataDictionaryTreeSelect
              axios={this.axios}
              groupCode={''}
              placeholder="请选择"
              value={this.queryParams.jobLocationId}
              onChange={value => {
                this.queryParams.jobLocationId = value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="专业">
            <DataDictionaryTreeSelect
              axios={this.axios}
              groupCode={''}
              placeholder="请选择"
              value={this.queryParams.professionalId}
              onChange={value => {
                this.queryParams.professionalId = value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="土建单位">
            <DataDictionaryTreeSelect
              axios={this.axios}
              groupCode={''}
              placeholder="请选择"
              value={this.queryParams.unitId}
              onChange={value => {
                this.queryParams.unitId = value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="检查情况">
            <a-select
              placeholder="请选择"
              style="width:100%"
              allowClear
              value={this.queryParams.checkSituation}
              onChange={value => {
                this.queryParams.checkSituation = value;
                this.refresh();
              }}
            >
              {Options}
            </a-select>
          </a-form-item>
          <a-form-item>
            <a-button type="primary" onClick={() => this.refresh()}>
              查询
            </a-button>
            <a-button type="danger" onClick={() => this.exprot()}>
              导出
            </a-button>
          </a-form-item>
        </div>
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
              jobLocation: (text, record) => {
                let result = record.jobLocation ? record.jobLocation.name : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              professional: (text, record) => {
                let result = record.professional ? record.professional.name : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              position: (text, record) => {
                let result = record ? record.position : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              materialSpecification: (text, record) => {
                let result = record.materialSpecification ? record.materialSpecification : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              count: (text, record) => {
                let result = record.count ? record.count : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              interfaceCode: (text, record) => {
                let result = record.interfaceCode ? record.interfaceCode : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              unit: (text, record) => {
                let result = record.unit ? record.unit.name : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              interfaceCode: (text, record) => {
                let result = record.checkSituation ? getInterfaceCheckSituation(record.checkSituation) : '';
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
    return (
      <div class="sm-technology-interface-listing">
        <div class="interface-listing-table">
          <a-tabs
            type="card"
            onChange={key => {
              this.callback(key);
            }}
          >
            <a-tab-pane key="civil" tab="土建接口清单">
              {tableData}
            </a-tab-pane>
            <a-tab-pane key="electron" tab="四建接口清单">
              {tableData}
            </a-tab-pane>
          </a-tabs>
        </div>
      </div>
    );
  },
};
