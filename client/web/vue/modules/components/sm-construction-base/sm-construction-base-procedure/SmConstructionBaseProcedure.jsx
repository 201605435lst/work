import './style';
import { requestIsSuccess } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import ApiDataDictionary from '../../sm-api/sm-system/DataDictionary';
import ApiProcedure from '../../sm-api/sm-construction-base/ApiProcedure';
import SmConstructionBaseProcedureModal from './SmConstructionBaseProcedureModal';
import SmConstructionBaseProcedureConfigModal from './SmConstructionBaseProcedureConfigModal';
import ApiMaterial from '../../sm-api/sm-construction-base/ApiMaterial';
import ApiWorker from '../../sm-api/sm-construction-base/Worker';
import ApiEquipmentTeam from '../../sm-api/sm-construction-base/ApiEquipmentTeam';

let apiProcedure = new ApiProcedure();


let apiDataDictionary = new ApiDataDictionary();// 字典api 查工程类型用
let apiMaterial = new ApiMaterial();
let apiWorker = new ApiWorker();
let apiEquipmentTeam = new ApiEquipmentTeam();
export default {
  name: 'SmConstructionBaseProcedure',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: true },
  },
  data() {
    return {
      queryParams: {
        // 模糊搜索
        typeId: '00000000-0000-0000-0000-000000000000',
        // 每页数量
        maxResultCount: paginationConfig.defaultPageSize,
        // 当前页1 这个在params 里面 也过滤掉了,放这里方便复制~
        pageIndex: 1,
        // 总数 这个在params 里面 也过滤掉了,放这里方便复制~
        totalCount: 0,
      },
      // 工程量清单列表
      list: [],
      // table 是否处于加载状态
      loading: false,
      procedureTypes: [],
      workers: [],
      materials: [],
      equipmentTeams: [],
    };
  },
  computed: {
    columns() {
      return [
        { title: '序号', dataIndex: 'id', scopedSlots: { customRender: 'id' }, width: 100 },
        { title: '工序名称', dataIndex: 'name', width: 200 },
        { title: '工序说明', dataIndex: 'description' },
        { title: '操作', scopedSlots: { customRender: 'operations' }, width: 180 },
      ];
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
    await this.getProcedureTypes();
  },
  methods: {
    initAxios() {
      apiProcedure = new ApiProcedure(this.axios);
      apiDataDictionary = new ApiDataDictionary(this.axios);
      apiMaterial = new ApiMaterial(this.axios);
      apiWorker = new ApiWorker(this.axios);
      apiEquipmentTeam = new ApiEquipmentTeam(this.axios);
    },
    async refresh() {
      this.loading = true;
      let res = await apiProcedure.getList({
        skipCount: (this.queryParams.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(res) && res.data) {
        this.list = res.data.items;
        console.log(res.data.items);
        this.totalCount = res.data.totalCount;
      }
      this.loading = false;
    },
    async onPageChange(page, pageSize) {
      this.queryParams.queryParams.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh();
      }
    },
    // 获取 工程类型 列表
    async getProcedureTypes() {
      let res = await apiDataDictionary.getValues({ groupCode: 'Progress.ProjectType' });
      if (requestIsSuccess(res) && res.data) {
        this.procedureTypes = res.data;
      }
    },
    // 添加(打开添加模态框)
    add() {
      this.$refs.SmConstructionBaseProcedureModal.add();
    },
    // 添加(打开添加模态框)
    edit(record) {
      this.$refs.SmConstructionBaseProcedureModal.edit(record);
    },
    // 资源配置(打开资源配置模态框)
    config(record) {
      this.$refs.SmConstructionBaseProcedureConfigModal.config(record);
    },
    remove(record) {
      const _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style='color:red;'>{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          // 删除工程量清单业务逻辑
          return new Promise(async (resolve, reject) => {
            const response = await apiProcedure.delete(record.id);
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
            this.queryParams.typeId = '00000000-0000-0000-0000-000000000000';
            this.queryParams.pageIndex = 1;
            this.refresh();
          }}
        >
          <a-form-item label='工程类型'>
            <a-select
              placeholder={'请选择工程类型'}
              value={this.queryParams.typeId}
              onChange={(val) => {
                this.queryParams.typeId = val;
                this.refresh();
              }}>
              <a-select-option value='00000000-0000-0000-0000-000000000000'>全部</a-select-option>
              {this.procedureTypes.map(x => (<a-select-option value={x.id}>{x.name}</a-select-option>))}
            </a-select>
          </a-form-item>


          <template slot='buttons'>
            <a-button type='primary' icon='plus' onClick={() => this.add()}>
              添加
            </a-button>
          </template>
        </sc-table-operator>

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
                let result = `${index + 1 + this.queryParams.totalCount * (this.queryParams.pageIndex - 1)}`;
                return (<span>{result}</span>);
              },
              operations: (record) => {
                return (
                  <div>
                    <div style='display:inline' onClick={() => this.edit(record)}><a>编辑</a></div>
                    <div style='display:inline;margin-left:10px' onClick={() => this.config(record)}><a>资源配置</a></div>
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
        <SmConstructionBaseProcedureModal
          ref='SmConstructionBaseProcedureModal'
          axios={this.axios}
          procedureTypes={this.procedureTypes}
          onSuccess={async () => {
            await this.refresh();
          }}
        />
        {/*资源配置模板*/}
        <SmConstructionBaseProcedureConfigModal
          ref='SmConstructionBaseProcedureConfigModal'
          axios={this.axios}
          procedureTypes={this.procedureTypes}
          onSuccess={async () => {
            await this.refresh();
          }}
        />
      </div>
    );
  },
};
