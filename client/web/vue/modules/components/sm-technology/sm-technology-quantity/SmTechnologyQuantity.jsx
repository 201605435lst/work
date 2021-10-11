
import './style';
import { requestIsSuccess, vIf, vP } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import ApiQuantity from '../../sm-api/sm-technology/Quantity';
import permissionsTechnology from '../../_permissions/sm-technology';

import FileSaver from 'file-saver';
let apiQuantity = new ApiQuantity();
import SmTechnologyMaterialPlanModal from './SmTechnologyMaterialPlanModal';
export default {
  name: 'SmTechnologyQuantity',
  props: {
    permissions: { type: Array, default: () => [] },
    axios: { type: Function, default: null },
  },
  data() {
    return {
      queryParams: {
        keywords: null,
        specId: null,
        typeId: undefined,
        maxResultCount: paginationConfig.defaultPageSize,
        statistic: false,
      },
      totalCount: 0,
      pageIndex: 1,
      loading: false,
      exportLoading: false,
      specialities: [],// 专业
      quantities: [],// 工程里
      selectedIds: [],// 选中的数据
      selectedQuantities: [],// 选中的工程量数据
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          width: 70,
          align: 'center',
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '专业名称',
          dataIndex: 'speciality',
          ellipsis: true,
          scopedSlots: { customRender: 'speciality' },
        },
        {
          title: '系统1',
          dataIndex: 'system1',
          ellipsis: true,
          scopedSlots: { customRender: 'system1' },
        },
        {
          title: '系统2',
          dataIndex: 'system2',
          ellipsis: true,
          scopedSlots: { customRender: 'system2' },
        },
        {
          title: '名称',
          dataIndex: 'name',
          ellipsis: true,
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '材料名称',
          dataIndex: 'productCategoryName',
          scopedSlots: { customRender: 'productCategoryName' },
          ellipsis: true,
        },
        {
          title: '规格/型号',
          dataIndex: 'spec',
          scopedSlots: { customRender: 'spec' },
          ellipsis: true,
        },
        // {
        //   title: '类别',
        //   dataIndex: 'type',
        //   width: 80,
        //   scopedSlots: { customRender: 'type' },
        //   ellipsis: true,
        // },
        {
          title: '工程数量',
          dataIndex: 'count',
          width: 100,
        },
        {
          title: '计量单位',
          dataIndex: 'unit',
          width: 100,
        },
      ];
    },
  },
  async created() {
    this.initAxios();
    this.queryParams.statistic = true;
    await this.getSpeciality();
    await this.refresh();
  },
  methods: {
    initAxios() {
      apiQuantity = new ApiQuantity(this.axios);
    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let response = await apiQuantity.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(response) && response.data) {
        this.quantities = response.data.items;
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
      if (this.queryParams.statistic) {
        this.$message.success("数据统计成功");
        this.queryParams.statistic = false;
      }
      this.loading = false;
    },
    // 获取专业信息
    async getSpeciality() {
      let response = await apiQuantity.getSpeciality();
      if (requestIsSuccess(response)) {
        this.specialities = response.data;
      }
    },
    handleChange(evt) {
      this.queryParams.specId = evt;
      this.refresh();
    },
    popupScroll(evt) {
      console.log(evt);
    },
    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },
    // 数据导出
    async export() {
      if (this.selectedIds.length == 0) {
        this.$message.warn('请选择需要导出的数据');
        return;
      }
      let _this = this;
      this.exportLoading = true;
      //let data = { ids:this.materialIds}; *
      return new Promise(async (resolve, reject) => {
        let response = await apiQuantity.export(this.selectedIds);
        _this.exportLoading = false;
        if (requestIsSuccess(response)) {
          FileSaver.saveAs(
            new Blob([response.data], { type: 'application/vnd.ms-excel' }),
            `材料详情表.xlsx`,
          );
          setTimeout(resolve, 100);
          this.selectedIds = [];
        } else {
          setTimeout(reject, 100);
        }
      });
    },
    // 生成用料计划
    materialPlan() {
      if (this.selectedIds.length == 0) {
        this.$message.warn('请选择需要生成的数据');
        return;
      }
      this.$refs.smTechnologyMaterialPlanModal.create(this.selectedQuantities);
    },

    //更新所选数据
    updateSelected(selectedRows) {
      // 过滤出其他页面已经选中的
      let _selected = [];
      for (let item of this.selectedQuantities) {
        let target = this.quantities.find(subItem => subItem.id === item.id);
        if (!target) {
          _selected.push(item);
        }
      }

      // 把当前页面选中的加入
      for (let id of this.selectedIds) {
        let quantity = this.quantities.find(item => item.id === id);
        if (!!quantity) { // !! 是 js 里面  (a!=null&&typeof(a)!=undefined&&a!='') 的简略写法
          _selected.push(JSON.parse(JSON.stringify(quantity))); // 为了保证 _selected 改变(不这样写拿到的是内存的引用……)
        }
      }

      this.selectedQuantities = _selected;

    },
  },
  render() {
    return (
      <div class="sm-technology-quantity">
        {/* 工具栏*/}
        <div class="head">
          <a-row gutter={16}>
            <a-col span={7} class="m-col">
              <span>关键字：</span>
              <a-input
                placeholder="请输入专业\系统1\系统2\名称\材料名称"
                class="m-form"
                allowClear
                style="width: 200px"
                value={this.queryParams.keywords}
                onInput={event => {
                  this.queryParams.keywords = event.target.value;
                  this.refresh();
                }}
              />
            </a-col>
            <a-col span={5} class="m-col">
              <span style='width:50px'>专业：</span>
              <a-select
                // mode="multiple"
                // size="size"
                placeholder="请选择专业"
                // default-value="['a1', 'b2']"
                style="width: 200px"
                onChange={this.handleChange}
                onPopupScroll={this.popupScroll}
              >
                {this.specialities.map(a => {
                  return (<a-select-option key={a.id}>
                    {a.name}
                  </a-select-option>);
                })}
              </a-select>
            </a-col>
            {vIf(
              <a-button
                type="primary"
                style="width:90px;margin-left:10px"
                loading={this.loading}
                onClick={() => {
                  this.queryParams.statistic = true;
                  this.refresh();
                }}
              >
                统计
              </a-button>,
              vP(this.permissions, permissionsTechnology.Quanitys.Statistic),
            )}
            {vIf(
              <a-button
                type="default"
                loading={this.exportLoading}
                style="width:90px;margin-left:10px"
                onClick={() => this.export()}
              >
                导出
              </a-button>,
              vP(this.permissions, permissionsTechnology.Quanitys.Export),
            )}
            {vIf(
              <a-button
                type="danger"
                style="width:120px;margin-left:10px"
                onClick={() => this.materialPlan()}
              >
                生成用料计划
              </a-button>,
              vP(this.permissions, permissionsTechnology.Quanitys.GenerateMaterialPlan),
            )}


          </a-row>
        </div>
        {/* 数据表格 */}
        <a-table
          columns={this.columns}
          rowKey={record => record.id}
          dataSource={this.quantities}
          pagination={false}
          loading={this.loading}
          // size="small"
          rowSelection={
            {
              columnWidth: 30,
              selectedRowKeys: this.selectedIds,
              onChange: (selectRowKeys, selectedRows) => {
                this.selectedIds = selectRowKeys;
                this.updateSelected(selectedRows);
              },
            }
          }
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              spec: (text, record, index) => {
                return <a-tooltip title={text}>{text}</a-tooltip>;
              },
              speciality: (text, record, index) => {
                return <a-tooltip title={text}>{text}</a-tooltip>;
              },
              system1: (text, record, index) => {
                return <a-tooltip title={text}>{text}</a-tooltip>;
              },
              system2: (text, record, index) => {
                return <a-tooltip title={text}>{text}</a-tooltip>;
              },
              name: (text, record, index) => {
                return <a-tooltip title={text}>{text}</a-tooltip>;
              },
              productCategoryName: (text, record, index) => {
                return <a-tooltip title={text}>{text}</a-tooltip>;
              },
            },
          }}
        >
        </a-table>
        {/* 表格分页器 */}
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
        <SmTechnologyMaterialPlanModal
          axios={this.axios}
          onSuccess={() => {
            this.selectedQuantities = [];
            this.selectedIds = [];
          }}
          ref="smTechnologyMaterialPlanModal"
        />
      </div>
    );
  },
};
