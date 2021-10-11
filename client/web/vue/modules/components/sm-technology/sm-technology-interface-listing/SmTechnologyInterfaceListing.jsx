import './style';
import ApiConstructInterface from '../../sm-api/sm-technology/ConstructInterface';
import { requestIsSuccess, vP, vIf, getMarkType } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import { MarkType, ConstructType } from '../../_utils/enum';
import SmExport from '../../sm-common/sm-export-module';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import SmTechnologyTable from '../sm-technology-components/src/SmTechnologyTable';
import ApiDataDictionary from '../../sm-api/sm-system/DataDictionary';
let apiDataDictionary = new ApiDataDictionary();
let apiConstructInterface = new ApiConstructInterface();
import permissionsSmTechnology from '../../_permissions/sm-technology';
export default {
  name: 'SmTechnologyInterfaceListing',
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
      isCanExport: false,
      selectedRowKeys: [],
      dataDictonaries: [],
      pageIndex: 1,
      tablePane: null, //表格切换项名---------a-tab-pane =============key
      queryParams: {
        jobLocationId: undefined, //施工地点
        professionId: undefined, //专业
        builderId: undefined, //土建单位
        interfaceManagementTypeId: null,//工程类型
        markType: undefined, //检查情况
        maxResultCount: paginationConfig.defaultPageSize,
      },
    };
  },
  computed: {},
  async created() {
    this.initAxios();
    await this.tabSelect();
    setTimeout(() => {
      this.refresh(); 
    });
  },
  methods: {
    initAxios() {
      apiConstructInterface = new ApiConstructInterface(this.axios);
      apiDataDictionary = new ApiDataDictionary(this.axios);
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
      let response = await apiConstructInterface.getList({
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
      this.isCanExport = this.totalCount > 0 ? false : true;
      this.loading = false;
    },
    async tabSelect() {
      let response = await apiDataDictionary.getValues({ groupCode: 'InterfaceManagementType' });
      if (requestIsSuccess(response)) {
        this.dataDictonaries = response.data;
        this.queryParams.interfaceManagementTypeId=this.dataDictonaries[0]?this.dataDictonaries[0].id:'';
      }
    },
    exprot(record) { },
    view(record) { },
    callback(key) {
      this.queryParams.interfaceManagementTypeId = key;
      this.refresh();
    },
    /* 文件导出 */
    async downloadFile(para) {
      let data = {
        ...this.queryParams,
        ids: this.selectedRowKeys,
      };
      //执行文件下载
      await this.$refs.smExport.isCanDownload(para, data);
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
    for (let item in MarkType) {
      Options.push(
        <a-select-option key={`${MarkType[item]}`}>
          {getMarkType(MarkType[item])}
        </a-select-option>,
      );
    }
    let tableData = [];
    tableData = (
      <div class="table">
        <div class="table-action">
          {/* <a-form-item label="施工地点">
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
            </a-form-item> */}
          <a-form-item label="专业">
            <DataDictionaryTreeSelect
              axios={this.axios}
              groupCode={'Profession'}
              placeholder="请选择"
              value={this.queryParams.professionId}
              onChange={value => {
                this.queryParams.professionId = value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="土建单位">
            <DataDictionaryTreeSelect
              axios={this.axios}
              groupCode={'ConstructionUnit'}
              placeholder="请选择"
              value={this.queryParams.builderId}
              onChange={value => {
                this.queryParams.builderId = value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="检查情况">
            <a-select
              placeholder="请选择"
              style="width:100%"
              allowClear
              value={this.queryParams.markType}
              onChange={value => {
                this.queryParams.markType = value;
                this.refresh();
              }}
            >
              {Options}
            </a-select>
          </a-form-item>
          <a-form-item>
            <div class="button-action">
              <a-button type="primary" onClick={() => this.refresh()}>
                查询
              </a-button>

              {vIf(
                <SmExport
                  ref="smExport"
                  url="api/app/constructInterface/export"
                  defaultTitle="导出"
                  axios={this.axios}
                  templateName="technologyInterface"
                  downloadFileName="接口管理表"
                  rowIndex={2}
                  disabled={this.isCanExport}
                  onDownload={para => this.downloadFile(para)}
                ></SmExport>,
                vP(this.permissions, permissionsSmTechnology.ConstructInterfaces.Export),
              )}
            </div>

          </a-form-item>
        </div>
        <div>
          <SmTechnologyTable
            permissions={this.permissions}
            datas={this.dataSource}
            type="list"
            onRowSelection={(selectedRowKeys, recordList) => {
              this.recordList = recordList;
              this.selectedRowKeys = selectedRowKeys;
            }}
            onView={value => {
              this.view(value);
            }}
            onExport={value => {
              this.exprot(value);
            }}
            axios={this.axios}
          />
        </div>
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
    let tableAction =[] ;
    {this.dataDictonaries &&this.dataDictonaries.length>0?
      this.dataDictonaries.map(item=>{
        tableAction.push(<a-tab-pane key={item.id} tab={item.name}>
          {tableData}
        </a-tab-pane>);
      }) 
      :'';};
    return (
      <div class="sm-technology-interface-listing">
        <div class="interface-listing-table">
          <a-tabs
            type="card"
            onChange={key => {
              this.callback(key);
            }}
          >
            {tableAction}
          </a-tabs>
        </div>
      </div>
    );
  },
};
