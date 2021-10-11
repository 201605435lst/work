
import './style';
import { requestIsSuccess, vP, vIf } from '../../_utils/utils';
import { pagination as paginationConfig } from '../../_utils/config';
import MaterialTypeSelect from '../sm-material-material-type-select';
import SmMaterialInquireModal from './SmMaterialInquireModal';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import permissionsSmMaterial from '../../_permissions/sm-material';
import ApiUsePlan from '../../sm-api/sm-material/UsePlan';
import ApiPartition from '../../sm-api/sm-material/Partition';
import FileSaver from 'file-saver';
let apiUsePlan = new ApiUsePlan();
let apiPartition = new ApiPartition();

export default {
  name: 'SmMaterialInquire',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      materials: [], //列表数据源
      form: this.$form.createForm(this),
      totalCount: 0,
      pageIndex: 1,
      queryParams: {
        keywords: null,
        professionId: null,
        typeId: undefined,
        partitionId: null,
        maxResultCount: paginationConfig.defaultPageSize,
      },
      loading: false,
      materialIds:[], //所选材料Ids
      materialsList:[], //所选材料
      partial: [], //库存位置
    };
  },
  computed: {
    columns() {
      return [
        {
          title:'#',
          dataIndex:'index',
          scopedSlots:{customRender: 'index'},
        },
        {
          title:'材料名称',
          dataIndex:'name',
          scopedSlots:{customRender: 'name'},
        },
        {
          title:'类别',
          dataIndex:'type',
          scopedSlots:{customRender: 'type'},
        },
        {
          title:'所属专业',
          dataIndex:'profession',
          scopedSlots:{customRender: 'profession'},
        },
        {
          title:'料库地点',
          dataIndex:'partitionName',
          scopedSlots:{customRender: 'partitionName'},
        },
        {
          title:'规格',
          dataIndex:'spec',
          scopedSlots:{customRender: 'spec'},
        },
        {
          title:'库存',
          dataIndex:'amount',
          scopedSlots:{customRender: 'amount'},
        },
        {
          title:'计划用料',
          dataIndex:'planNumber',
          scopedSlots:{customRender: 'planNumber'},
        },
        {
          title:'量差',
          dataIndex:'gap',
          scopedSlots:{customRender: 'gap'},
        },
        {
          title:'操作',
          dataIndex:'operations',
          width: 169,
          scopedSlots: { customRender: 'operations' },
          fixed: 'right',
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
      apiUsePlan = new ApiUsePlan(this.axios);
      apiPartition = new ApiPartition(this.axios);
    },
    async refresh(resetPage = true, page) { 
      // 获取位置分区信息
      let partialresponse = await apiPartition.getTreeList();
      if (requestIsSuccess(partialresponse)) {
        this.partial = partialresponse.data;
      }
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let response = await apiUsePlan.getMaterialList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(response) && response.data) {
        this.materials = response.data.items;
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
    view(record){
      this.$refs.smMaterialInquireModal.view(record);
    },
    export(){
      let _this = this;
      this.loading = true;
      let data = { usePlanSearchForInquireDtos:this.materialsList};
      return new Promise(async (resolve, reject) => {
        let response = await apiUsePlan.exportMaterial(data);
        _this.loading = false;
        if (requestIsSuccess(response)) {
          FileSaver.saveAs(
            new Blob([response.data], { type: 'application/vnd.ms-excel' }),
            `物资详情表.xlsx`,
          );
          setTimeout(resolve, 100);
        } else {
          setTimeout(reject, 100);
        }
      });
    },
  },
  render() {
    return (
      <div class="sm-material-inquire">
        <sc-table-operator
          onSearch={() =>{
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.keywords = null;
            this.queryParams.professionId= null,
            this.queryParams.typeId= null,
            this.queryParams.partitionId= null,
            this.refresh();
          }}
        >
          <a-form-item label="类别">
            <MaterialTypeSelect
              axios={this.axios}
              placeholder='请选择材料类别'
              search={true}
              value={this.queryParams.typeId}
              onChange={value => {
                this.queryParams.typeId = value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="专业">
            <DataDictionaryTreeSelect
              axios={this.axios}
              groupCode={'Profession'}
              placeholder='请选择所属专业'
              value={this.queryParams.professionId}
              onChange={value => {
                this.queryParams.professionId = value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="库存位置">
            <a-tree-select
              treeData={this.partial}
              show-search
              value={this.queryParams.partitionId}
              class="m-form"
              style="width:80%"
              onDropdown-style="{ maxHeight: '400px', overflow: 'auto' }"
              placeholder="请选择库存位置"
              allow-clear
              replaceFields={{ title: 'name', key: 'id', value: 'id' }}
              tree-default-expand-all
              treeNodeLabelProp="title"
              onChange={value => {
                this.queryParams.partitionId = value;
                this.refresh();
              }}
            ></a-tree-select>
          </a-form-item>
          <a-form-item label="关键字">
            <a-input
              axios={this.axios}
              placeholder={'请输入名称/规格/型号/价格'}
              value={this.queryParams.keywords}
              onInput={event => {
                this.queryParams.keywords = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          <template slot="buttons">
            <div style={'display:flex'}>
              {vIf(
                <a-button type="primary" onClick={() => this.export()} disabled={this.materialIds.length === 0} loading={this.loading}> <a-icon type="export" /> 导出</a-button>,
                vP(this.permissions, permissionsSmMaterial.Inquires.Export),
              )}
            </div>
          </template>
        </sc-table-operator>
        {/* 展示区 */}
        <a-table
          columns={this.columns}
          rowKey={record => record.material.id}
          dataSource={this.materials}
          pagination={false}
          loading={this.loading}
          rowSelection={
            {
              columnWidth: 30,
              onChange: (selectedRowKeys, selectedRows) => {
                this.materialIds = selectedRowKeys;
                this.materialsList = selectedRows;
              },
            }}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              name: (text, record) => {
                return record.material ? record.material.name : null;
              },
              profession: (text, record) => {
                let professionName = record.material.profession ? record.material.profession.name : null;
                return <a-tooltip placement='topLeft' title={professionName}><span>{professionName}</span></a-tooltip>;
              },
              partitionName: (text, record) => {
                return record.partitionName ? record.partitionName : <p style='color:red'>暂未入库</p>;
              },
              type: (text, record) => {
                let typeName = record.material.type ? record.material.type.name : null;
                return <a-tooltip placement='topLeft' title={typeName}><span>{typeName}</span></a-tooltip>;
              },
              spec: (text, record) => {
                return record.material ? record.material.spec : null;
              },
              amount: (text, record) => {
                return record.amount ? record.amount : 0;
              },
              planNumber: (text, record) => {
                return record.planNumber ? record.planNumber : 0;
              },
              gap: (text, record) => {
                let gap = record.amount ? record.amount - record.planNumber : 0-record.planNumber;
                return (<p style={gap && gap > 0 ? undefined : 'color:red'}>{gap}</p>);
              },
              operations: (text, record) => {
                return [
                  <span>
                    {vIf(
                      <a onClick={() => { this.view(record); }}>查看</a>,
                      vP(this.permissions, permissionsSmMaterial.Inquires.Detail),
                    )}
                  </span>,
                ];
              },
            },
          }}
        >
        </a-table>

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
          showTotal={paginationConfig.showTotal}
        />

        {/* 物资查询详情 */}
        <SmMaterialInquireModal
          axios={this.axios}
          ref="smMaterialInquireModal"
        >
        </SmMaterialInquireModal>
      </div>
    );
  },
};
    