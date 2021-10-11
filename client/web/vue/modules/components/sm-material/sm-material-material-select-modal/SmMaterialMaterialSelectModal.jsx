
/**
 * 说明：材料选择框
 * 作者：easten
 */
import './style';
import { requestIsSuccess } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import { ModalStatus } from '../../_utils/enum';
import ApiEntity from '../../sm-api/sm-namespace/Entity';
import ApiMaterial from '../../sm-api/sm-technology/Material';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
let apiMaterial = new ApiMaterial();

export default {
  name: 'SmMaterialMaterialSelectModal',
  model: {
    prop: 'visible',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: null },
    width: { type: Number, default: 900 },
    height: { type: Number, default: 700 },
  },
  data() {
    return {
      status: ModalStatus.Hide,
      queryParams: {
        keywords: null,
        professionId: null,
        typeId: undefined,
        maxResultCount: paginationConfig.defaultPageSize,
      },
      totalCount: 0,
      pageIndex: 1,
      loading: false,
      materialIds: [],
      materials: [],// 查询的材料
      id: null,
      iVisible: false,
      selectedMaterials: [],// 选中的数据
      selectedRowKeys:[],
    };
  },
  computed: {
    dataSourceColumns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          width: 60,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '材料名称',
          dataIndex: 'name',
          scopedSlots: { customRender: 'name' },
        },

        {
          title: '规格/型号',
          dataIndex: 'spec',
        },
        {
          title: '类别',
          dataIndex: 'type',
          width: 200,
          scopedSlots: { customRender: 'type' },
        },
        {
          title: '价格(元)',
          dataIndex: 'price',
          width: 100,
          scopedSlots: { customRender: 'price' },
        },
      ];
    },
  },
  watch: {
    visible: {
      handler(nVal, oVal) {
        this.selectRowKeys=[];  // 选中的表
        this.selectedRowKeys=[]; // 源表
        this.selectedMaterials=[];
        this.materialIds=[];
        this.iVisible = nVal;
      },
      immediate: true,
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiMaterial = new ApiMaterial(this.axios);
    },
    // 数据刷新
    async refresh(resetPage = true, page) {
      this.materialIds = [];
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let response = await apiMaterial.getList({
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
    onOk() {
      this.$emit('change', false,this.selectedMaterials);
      this.onClose();
    },
    add(){
      this.selectRowKeys=[];  // 选中的表
      this.selectedRowKeys=[]; // 源表
      this.selectedMaterials=[];
      this.materialIds=[];
      this.iVisible = true;
    },
    onClose() {
      this.iVisible = false;
      this.$emit('change', false,[]);
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
    return (<a-modal
      width={this.width}
      title='物资材料选择'
      okText="确定"
      visible={this.iVisible}
      onOk={this.onOk}
      onCancel={this.onClose}
    >
      <div class='sm-material-material-select-modal'>
        {/* 工具 */}
        <div class="head">
          <a-row gutter={16}>
            <a-col span={11} class="m-col">
              <span>关键字：</span>
              <a-input
                placeholder="请输入材料名称"
                class="m-form"
                size='small'
                value={this.queryParams.keywords}
                onInput={event => {
                  this.queryParams.keywords = event.target.value;
                  this.refresh();
                }}
              />
            </a-col>
            <a-col span={11} class="m-col">
              <span style='width:100px'>材料类别：</span>
              <DataDictionaryTreeSelect
                axios={this.axios}
                placeholder="请选择材料类别"
                groupCode={'MaterialType'}
                size="small"
                onChange={(value) => {
                  this.queryParams.typeId = value;
                  this.refresh();
                }}
              />
            </a-col>
            <a-col span={2} class="m-col">
              <a-button
                type="primary"
                size="small"
                style="width:100%"
                onClick={() => this.refresh()}
              >
                查询
              </a-button>
            </a-col>
          </a-row>
        </div>

        {/* 数据表格  有分页  可复选*/}
        <div class='table-source'>
          <a-table
            columns={this.dataSourceColumns}
            rowKey={record => record.id}
            dataSource={this.materials}
            pagination={false}
            loading={this.loading}
            size="small"
            rowSelection={
              {
                columnWidth: 30,
                selectedRowKeys:this.selectedRowKeys,
                onChange: (selectedRowKeys, keys) => {
                  this.selectedRowKeys=selectedRowKeys;
                  this.materialIds = keys;
                },
                onSelect: (record, selected, selectedRow) => {
                  if (selected) {
                    this.selectedMaterials.push(record);
                  } else {
                    this.selectedMaterials = this.selectedMaterials.filter(a => a.id != record.id);
                  }
                },
                onSelectAll:(selected,selectedRows,changeRows)=>{
                  if(selected){
                    this.selectedMaterials.push(...selectedRows);
                  }else{
                    let arraySet=new Set(changeRows.map(a=>a.id));
                    this.selectedMaterials=this.selectedMaterials.filter(a=>!arraySet.has(a.id));
                  }
                },
              }
            }
            scroll={
              {
                y: 200,
              }
            }
            {...{
              scopedSlots: {
                index: (text, record, index) => {
                  return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                },
                name: (t, r, i) => {
                  return <a href="#">{r.name}</a>;
                },
                type: (t, r, i) => {
                  return r.type==null?<a-tooltip placement='topLeft' title=''><span></span></a-tooltip>:<a-tooltip placement='topLeft' title={r.type.name|'自定义'}><span>{r.type.name|'自定义'}</span></a-tooltip>
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
            size="small"
            showTotal={paginationConfig.showTotal}
          />
        </div>
        {/* 已选择的内容 */}
        <div class='table-selected'>
          <a-table
            columns={this.dataSourceColumns}
            rowKey={record => record.id}
            dataSource={this.selectedMaterials}
            pagination={false}
            loading={this.loading}
            size="small"
            scroll={{ y: 200 }
            }
            {...{
              scopedSlots: {
                index: (text, record, index) => {
                  return index + 1;
                },
                name: (t, r, i) => {
                  return <a href="#">{r.name}</a>;
                },
                type: (t, r, i) => {
                  return <a-tooltip placement='topLeft' title={r.type.name}><span>{r.type.name}</span></a-tooltip>;
                },
              },
            }}
          >
          </a-table>
        </div>
      </div>
    </a-modal>
    );
  },
};
