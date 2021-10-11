
/**
 * 说明：采购清单选择框
 */

import './style';
import { requestIsSuccess, getPurchaseListType, getPurchaseState } from '../../_utils/utils';
import { pagination as paginationConfig } from '../../_utils/config';
import { ModalStatus, PurchaseListType, PurchaseState } from '../../_utils/enum';
import ApiPurchaseList from '../../sm-api/sm-material/PurchaseList';
let apiPurchaseList = new ApiPurchaseList();

export default {
  name: 'SmMaterialPurchaseListSelectModal',
  model: {
    prop: 'visible',
    event: 'change',
  },
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: null },
    width: { type: Number, default: 900 },
    height: { type: Number, default: 1000 },
  },
  data() {
    return {
      status: ModalStatus.Hide,
      queryParams: {
        keyword: null,
        type: undefined,
        state: undefined,
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
      selectedRowKeys: [],// 选中的id
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
          title: '采购单名称',
          dataIndex: 'name',
        },
        {
          title: '关联项目',
          dataIndex: 'project.name',
        },
        {
          title: '采购类型',
          dataIndex: 'type',
          width: 200,
          scopedSlots: { customRender: 'type' },
        },
        {
          title: '采购清单状态',
          dataIndex: 'state',
          width: 100,
          scopedSlots: { customRender: 'state' },
        },
      ];
    },
  },
  watch: {
    visible: {
      handler(nVal, oVal) {
        this.selectRowKeys = [];
        // this.selectedMaterials=[];
        this.materialIds = [];
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
      apiPurchaseList = new ApiPurchaseList(this.axios);
    },
    // 数据刷新
    async refresh(resetPage = true, page) {
      this.materialIds = [];
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let response = await apiPurchaseList.getList({
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
      this.onClose();
    },
    onClose() {
      this.$emit('change', false, this.selectedMaterials);
      this.iVisible = false;
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
    let TypeOptions = [];
    let StateOptions = [];
    for (let item in PurchaseListType) {
      TypeOptions.push(
        <a-select-option key={PurchaseListType[item]}>
          {getPurchaseListType(PurchaseListType[item])}
        </a-select-option>,
      );
    };
    for (let item in PurchaseState) {
      StateOptions.push(
        <a-select-option key={PurchaseState[item]}>
          {getPurchaseState(PurchaseState[item])}
        </a-select-option>,
      );
    };
    let Atag = [];
    console.log(this.selectedRowKeys);
    for (let index in this.selectedMaterials) {
      Atag.push(
        <a-tag style="margin: 2px" closable onClose={() => {
          this.selectedRowKeys.splice(index, 1);
          this.selectedMaterials.splice(index, 1);
        }}>
          {this.selectedMaterials[index].name}
        </a-tag>,
      );
    };
    return (<a-modal
      width={this.width}
      title='采购清单选择'
      okText="确定"
      visible={this.iVisible}
      onOk={this.onOk}
      onCancel={this.onClose}
    >
      <div class='sm-material-purchase-list-select-modal'>
        {/* 工具 */}
        <div class="head">
          <a-row gutter={16}>
            <a-col span={24} class="m-col tags">
              <div class={Atag.length > 0 ? '' : 'tags-div'}>{Atag.length > 0 ? Atag : '清单列表为空'}</div>
            </a-col>
            <a-col span={6} class="m-col">
              <span>关键字：</span>
              <a-input
                placeholder="请输入采购单名称"
                class="m-form"
                size='small'
                value={this.queryParams.keyword}
                onInput={event => {
                  this.queryParams.keyword = event.target.value;
                  this.refresh();
                }}
              />
            </a-col>
            <a-col span={7} class="m-col">
              <span style='width:100px'>采购类型：</span>
              <a-select
                placeholder={'请选择采购类型'}
                style="width:100%"
                size='small'
                value={this.queryParams.type}
                onChange={value => {
                  this.queryParams.type = value;
                  this.refresh();
                }}
              >
                {TypeOptions}
              </a-select>
            </a-col>
            <a-col span={7} class="m-col">
              <span style='width:100px'>采购状态：</span>
              <a-select
                placeholder={'请选择采购状态'}
                style="width:100%"
                size='small'
                value={this.queryParams.state}
                onChange={value => {
                  this.queryParams.state = value;
                  this.refresh();
                }}
              >
                {StateOptions}
              </a-select>
            </a-col>
            <a-col span={2} class="m-col">
              <a-button
                type="primary"
                size="small"
                style="width:90px;margin-left:10px"
                onClick={() => this.refresh()}
              >
                查询
              </a-button>
            </a-col>
            <a-col span={2} class="m-col">
              <a-button
                type="primary"
                size="small"
                style="width:90px;margin-left:10px"
                onClick={
                  () => {
                    this.queryParams = {
                      keyword: null,
                      type: undefined,
                      state: undefined,
                      maxResultCount: paginationConfig.defaultPageSize,
                    };
                    this.refresh();
                  }
                }
              >
                重置
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
                selectedRowKeys: this.selectedRowKeys,
                onChange: (items, keys) => {
                  this.selectedRowKeys = items;
                  this.materialIds = keys;
                },
                onSelect: (record, selected, selectedRow) => {
                  if (selected) {
                    this.selectedMaterials.push(record);
                  } else {
                    this.selectedMaterials = this.selectedMaterials.filter(a => a.id != record.id);
                  }
                },
                onSelectAll: (selected, selectedRows, changeRows) => {
                  if (selected) {
                    this.selectedMaterials.push(...selectedRows);
                  } else {
                    let arraySet = new Set(changeRows.map(a => a.id));
                    this.selectedMaterials = this.selectedMaterials.filter(a => !arraySet.has(a.id));
                  }
                },
              }
            }
            {...{
              scopedSlots: {
                index: (text, record, index) => {
                  return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                },
                type: (t, r, i) => {
                  let type = getPurchaseListType(r.type);
                  return (
                    <a-tooltip placement='topLeft' title={type}>
                      <span>{type}</span>
                    </a-tooltip>
                  );
                },
                state: (t, r, i) => {
                  let state = getPurchaseState(r.state);
                  return (
                    <a-tooltip placement='topLeft' title={state}>
                      <span>{state}</span>
                    </a-tooltip>
                  );
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
      </div>
    </a-modal>
    );
  },
};
