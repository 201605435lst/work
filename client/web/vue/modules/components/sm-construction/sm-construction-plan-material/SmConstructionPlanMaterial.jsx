import signalr from '@/utils/signalr';
import './style';
import { requestIsSuccess } from '../../_utils/utils';
import ApiPlanMaterial from '../../sm-api/sm-construction/ApiPlanMaterial';
import ApiDataDictionary from '../../sm-api/sm-system/DataDictionary';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmD3 from '../../sm-d3/sm-d3/SmD3';


let apiPlanMaterial = new ApiPlanMaterial();
let apiDataDictionary = new ApiDataDictionary();// 字典api 查类型用

// 施工计划工程量
export default {
  name: 'SmConstructionPlanMaterial',
  props: {
    axios: { type: Function, default: null },
    isApproval: { type: Boolean, default: false },  // 审批或查看模式下不能设置工程量
    bordered: { type: Boolean, default: true },
    showOperator: { type: Boolean, default: true }, // 是否显示操作栏()
    showSelectRow: { type: Boolean, default: false }, // 是否显示选择栏
    showTableOperator: { type: Boolean, default: true }, // 是否显示搜索操作栏
    planContentId: { type: String, default: undefined },
  }, // 施工计划详情id

  data() {
    return {
      queryParams: {
        maxResultCount: paginationConfig.defaultPageSize, // 每页数量
        pageIndex: 1, // 当前页1 这个在 传后端 的时候 过滤掉了,放这里方便复制~
        totalCount: 0, // 总数 这个在 传后端 的时候 过滤掉了,放这里方便复制~
        planContentId: undefined, // 施工计划详情id
      },
      list: [], // table 数据源
      loading: false, // table 是否处于加载状态
      selectPlanMaterialIds: [], // 选择的 施工计划工程量 ids (选择框模式的时候用)
      dicTypes: [],
      snEarthProjectUrl: window.snEarthProjectUrl,
      d3ModalVisible: false,
      selectedEquipments: [], // 选择的设备
    };
  },
  computed: {
    columns() {
      let baseColumns = [
        {
          title: '序号', dataIndex: 'id', width: 60, customRender: (text, record, index) => {
            let result = `${index + 1 + this.queryParams.maxResultCount * (this.queryParams.pageIndex - 1)}`;
            return (<span>{result}</span>);
          },
        },
        { title: '构件分类', dataIndex: 'componentCategoryName', ellipsis: true },
        {
          title: '设备列表', dataIndex: 'planMaterialRltEquipments', width: 200, customRender: (text, record, index) => {
            return <div>
              {text.map(x =>
                <a-tooltip placement="topLeft" title={x.equipment.name}>
                  <div style="white-space: nowrap;overflow: hidden;text-overflow: ellipsis;">
                    {x.equipment.name}
                  </div>
                </a-tooltip>)
              }
            </div>;
          },
        },
        { title: '规格型号', dataIndex: 'spec', ellipsis: true },
        { title: '单位', dataIndex: 'unit', width: 60 },
        { title: '数量', dataIndex: 'quantity', width: 60 },
        { title: '工日', dataIndex: 'workDay', width: 60 },
      ];
      return this.showOperator ? [
        ...baseColumns,
        {
          title: '操作', width: 60, customRender: (record) => {
            return (
              <div>
                <div style='display:inline;margin-left:10px' onClick={() => this.remove(record)}><a>删除</a></div>
              </div>
            );
          },
        },
      ] : baseColumns;
    },
  },
  watch: {
    planContentId: {
      handler(nVal, oVal) {
        // console.log("watch planContentId",nVal);
        this.queryParams.planContentId = nVal;
        if (nVal !== undefined) { // axios 存在的时候 才刷新,不然报 axios is not a function
          this.refresh();
        }
      },
      immediate: true, // 是否立马执行
    },

  },

  async created() {
    this.initAxios();
    await this.refresh();
    await this.getDicTypes();
  },
  methods: {
    //设置 工程量
    async setPlanMaterial() {
      this.$emit('change', true);
      this.loading = true;
      let res = await apiPlanMaterial.setPlanMaterial(this.planContentId, this.selectedEquipments.map(x => x.id));
      if (requestIsSuccess(res)) {
        this.$message.success('设置成功!');
      }
      this.loading = false;
      this.$emit('change', false);
    },
    // 选择设备添加进工程量
    // 初始化axios,将apiStandard实例化
    initAxios() {
      apiPlanMaterial = new ApiPlanMaterial(this.axios);
      apiDataDictionary = new ApiDataDictionary(this.axios);
    },
    // 刷新获取list
    async refresh() {
      this.loading = true;
      let res = await apiPlanMaterial.getList({
        skipCount: (this.queryParams.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(res) && res.data) {
        this.list = res.data.items;
        // console.log(res.data.items);
        this.queryParams.totalCount = res.data.totalCount;
        // this.selectedEquipments = this.list.map(x=>x.planMaterialRltEquipments)
        let dudu = this.list.map(x => x.planMaterialRltEquipments).flat().map(x => ({
          id: x.equipment.id,
          name: x.equipment.name,
          groupName: x.equipment.group.name,
        }));
        this.selectedEquipments = dudu;
      }
      this.loading = false;
    },
    // 获取 类型 列表
    async getDicTypes() {
      let res = await apiDataDictionary.getValues({ groupCode: 'Progress.ProjectType' });
      if (requestIsSuccess(res) && res.data) {
        this.dicTypes = res.data;
      }
    },

    // 分页事件
    async onPageChange(page, pageSize) {
      this.queryParams.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh();
      }
    },
    remove(record) {
      const _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style='color:red;'>{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            const response = await apiPlanMaterial.delete(record.id);
            _this.refresh();
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },

  },
  render() {
    return (
      <div>
        {this.showOperator && <div style='width:100%;margin-bottom:10px;display:flex;justify-content:flex-end;'>
          <a-button type='primary' size='small' onClick={() => {
            this.d3ModalVisible = true;
          }}>
            设置工程量
          </a-button>
        </div>}
        {/*展示区*/}
        <a-table
          dataSource={this.list}
          rowKey={record => record.id}
          size='small'
          columns={this.columns}
          loading={this.loading}
          bordered={this.bordered}
          pagination={false}
          rowSelection={this.showSelectRow ? {
            selectedRowKeys: this.selectPlanMaterialIds,
            columnWidth: 30,
            onChange: (selectedRowKeys, selectedRows) => {
              this.selectPlanMaterialIds = selectedRowKeys;
            },
          } : undefined}

        />

        {/*分页*/}
        <a-pagination
          style='margin-top:10px; text-align: right;'
          total={this.queryParams.totalCount}
          pageSize={this.queryParams.maxResultCount}
          current={this.queryParams.pageIndex}
          onChange={this.onPageChange}
          onShowSizeChange={this.onPageChange}
          showSizeChanger={true}
          showQuickJumper={true}
          size={this.isSimple || this.isFault ? 'small' : ''}
          showTotal={paginationConfig.showTotal}
        />


        <a-modal
          title='工程量信息'
          visible={this.d3ModalVisible}
          width='70%'
          onCancel={() => {
            this.d3ModalVisible = false;
          }}
          onOk={async () => {
            console.log("ok", this.selectedEquipments.map(x => x.id));
            if (this.selectedEquipments.length === 0) return this.$message.warn('未选择设备');
            this.d3ModalVisible = false;
            await this.setPlanMaterial();
            await this.refresh();
          }}
        >
          <div
            style='
        height: 600px;
        display: flex;
        /* position: fixed;
        z-index: 100000000;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0; */
    '
          >
            <SmD3
              axios={this.axios}
              signalr={signalr}
              snEarthProjectUrl={this.snEarthProjectUrl}
              globalTerrainUrl='//172.16.1.12:8165/terrain/World'
              globalImageryUrl='//172.16.1.12:8165/imagery/World'
              select={true}
              // selectedEquipments={[
              //   { id: '39fcdd0b-1cc9-0f68-0726-f02dc7077030', name: '桥架配件_3', groupName: '7号线/窑上村站/DongZhao' },
              //   { id: '39fcdd0b-1ded-f533-b975-ea98ee4604e8', name: '桥架配件_10', groupName: '7号线/窑上村站/DongZhao' },
              // ]}
              selectedEquipments={this.selectedEquipments}
              onSelectedEquipmentsChange={e => {
                this.selectedEquipments = e;
              }}
            />
          </div>
        </a-modal>

      </div>
    );
  },



};
