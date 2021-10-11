import './style';
import { requestIsSuccess } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmConstructionBaseEquipmentTeamModal from './SmConstructionBaseEquipmentTeamModal';
import ApiEquipmentTeam from '../../sm-api/sm-construction-base/ApiEquipmentTeam';
import ApiDataDictionary from '../../sm-api/sm-system/DataDictionary';

let apiEquipmentTeam = new ApiEquipmentTeam();
// 字典api 查设备类型用
let apiDataDictionary = new ApiDataDictionary();

export default {
  name: 'SmConstructionBaseEquipmentTeam',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: true },
  },
  data() {
    return {
      queryParams: {
        // 设备类型搜索(guid类型 )
        typeId: '00000000-0000-0000-0000-000000000000',
        // 模糊搜索
        name: '',
        // 每页数量
        maxResultCount: paginationConfig.defaultPageSize,
      },
      // 当前页1
      pageIndex: 1,
      // 设备台班列表
      list: [],
      // 总数
      totalCount: 0,
      // table 是否处于加载状态
      loading: false,
      // 设备类型
      equipmentTypes: [],
    };
  },
  computed: {
    columns() {
      return [
        { title: '序号', dataIndex: 'id', scopedSlots: { customRender: 'id' }, width: 100 },
        { title: '设备类型', dataIndex: 'type' },
        { title: '设备名称', dataIndex: 'name' },
        { title: '设备规格', dataIndex: 'spec' },
        { title: '综合成本', dataIndex: 'cost' },
        { title: '操作', scopedSlots: { customRender: 'operations' }, width: 180 },
      ];
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
    await this.getEquipmentTypes();
  },
  methods: {
    initAxios() {
      apiEquipmentTeam = new ApiEquipmentTeam(this.axios);
      apiDataDictionary = new ApiDataDictionary(this.axios);
    },

    // 获取 设备类型  列表
    async getEquipmentTypes() {
      let res = await apiDataDictionary.getValues({ groupCode: 'Progress.EquipmentType' });
      if (requestIsSuccess(res) && res.data) {
        this.equipmentTypes = res.data;
      }
    },
    async refresh() {
      this.loading = true;
      let res = await apiEquipmentTeam.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(res) && res.data) {
        this.list = res.data.items;
        // console.log(this.workers);
        this.totalCount = res.data.totalCount;
      }
      this.loading = false;
    },

    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh();
      }
    },

    // 添加(打开添加模态框)
    add() {
      this.$refs.SmConstructionBaseEquipmentTeamModal.add();
    },
    // 添加(打开添加模态框) 123
    edit(record) {
      this.$refs.SmConstructionBaseEquipmentTeamModal.edit(record);
    },

    remove(record) {
      const _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style='color:red;'>{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          // 删除角色业务逻辑
          return new Promise(async (resolve, reject) => {
            const response = await apiEquipmentTeam.delete(record.id);
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
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.name = '';
            this.queryParams.typeId = '00000000-0000-0000-0000-000000000000';
            this.pageIndex = 1;
            this.refresh();
          }}
        >
          <a-form-item label='关键字'>
            <a-input
              placeholder={'请输入设备台班名'}
              value={this.queryParams.name}
              onInput={async event => {
                this.queryParams.name = event.target.value;
                this.pageIndex = 1; // 查询的时候 当前页给1,不然查到了数据也不显示
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label='设备类型'>
            <a-select
              placeholder={'请选择设备类型'}
              value={this.queryParams.typeId}
              onChange={(val) => {
                this.queryParams.typeId = val;
                this.refresh();
              }}>
              <a-select-option value='00000000-0000-0000-0000-000000000000'>全部</a-select-option>
              {this.equipmentTypes.map(x => (<a-select-option value={x.id}>{x.name}</a-select-option>))}
            </a-select>
          </a-form-item>

          <template slot='buttons'>
            <a-button type='primary' icon='plus' onClick={() => this.add()}>
              添加
            </a-button>
          </template>
        </sc-table-operator>
        {/*展示区*/}
        <a-table
          dataSource={this.list}
          rowKey={record => record.id}
          columns={this.columns}
          loading={this.loading}
          bordered={this.bordered}
          pagination={false}
          {...{
            scopedSlots: {
              /* 这个id 要和 columns[] 的  scopedSlots: { customRender: 'id' } 对应 */
              id: (text, record, index) => {
                let result = `${index + 1 + this.totalCount * (this.pageIndex - 1)}`;
                return (<span>{result}</span>);
              },
              operations: (record) => {
                return (
                  <div>
                    <div style='display:inline' onClick={() => this.edit(record)}><a>编辑</a></div>
                    <div style='display:inline;margin-left:10px' onClick={() => this.remove(record)}><a>删除</a>
                    </div>
                  </div>
                );
              },
            },
          }}
        />

        {/*分页*/}
        <a-pagination
          style='margin-top:10px; text-align: right;'
          total={this.totalCount}
          pageSize={this.queryParams.maxResultCount}
          current={this.pageIndex}
          onChange={this.onPageChange}
          onShowSizeChange={this.onPageChange}
          showSizeChanger
          showQuickJumper
          size={this.isSimple || this.isFault ? 'small' : ''}
          showTotal={paginationConfig.showTotal}
        />

        {/*添加/编辑模板*/}
        <SmConstructionBaseEquipmentTeamModal
          ref='SmConstructionBaseEquipmentTeamModal'
          axios={this.axios}
          equipmentTypes={this.equipmentTypes}
          onSuccess={async () => {
            await this.refresh();
          }}
        />

      </div>
    );
  },
};
