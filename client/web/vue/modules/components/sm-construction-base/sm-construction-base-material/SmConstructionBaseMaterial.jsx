import './style';
import { requestIsSuccess } from '../../_utils/utils';
import ApiMaterial from '../../sm-api/sm-construction-base/ApiMaterial';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmConstructionBaseMaterialModal from './SmConstructionBaseMaterialModal';

let apiMaterial = new ApiMaterial();

export default {
  name: 'SmConstructionBaseMaterial',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: true },
  },
  data() {
    return {
      queryParams: {
        // 模糊搜索
        name: '',
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
    };
  },
  computed: {
    columns() {
      return [
        { title: '序号', dataIndex: 'id', scopedSlots: { customRender: 'id' }, width: 100 },
        { title: '工程量编码', dataIndex: 'code' },
        { title: '名称及规格', dataIndex: 'name' },
        { title: '计量单位', dataIndex: 'unitStr' },
        { title: '工程材料', dataIndex: 'isSelf', scopedSlots: { customRender: 'isSelf' } },
        { title: '甲供', dataIndex: 'isPartyAProvide', scopedSlots: { customRender: 'isPartyAProvide' } },
        { title: '提前到场天数', dataIndex: 'presentDays', scopedSlots: { customRender: 'presentDays' } },
        { title: '采购前置天数', dataIndex: 'prePurchaseDays', scopedSlots: { customRender: 'prePurchaseDays' } },
        { title: '操作', scopedSlots: { customRender: 'operations' }, width: 180 },
      ];
    },
  },
  created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiMaterial = new ApiMaterial(this.axios);
    },
    async refresh() {
      this.loading = true;
      let res = await apiMaterial.getList({
        skipCount: (this.queryParams.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(res) && res.data) {
        this.list = res.data.items;
        // console.log(res.data.items);
        this.queryParams.totalCount = res.data.totalCount;
      }
      this.loading = false;
    },
    async onPageChange(page, pageSize) {
      this.queryParams.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh();
      }
    },
    // 添加(打开添加模态框)
    add() {
      this.$refs.SmConstructionBaseMaterialModal.add();
    },
    // 添加(打开添加模态框) 123
    edit(record) {
      this.$refs.SmConstructionBaseMaterialModal.edit(record);
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
            const response = await apiMaterial.delete(record.id);
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
            this.queryParams.pageIndex = 1;
            this.refresh();
          }}
        >
          <a-form-item label='关键字'>
            <a-input
              placeholder={'请输入关键字'}
              value={this.queryParams.name}
              onInput={async event => {
                this.queryParams.name = event.target.value;
                this.queryParams.pageIndex = 1; // 查询的时候 当前页给1,不然查到了数据也不显示
                this.refresh();
              }}
            />
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
                let result = `${index + 1 + this.queryParams.maxResultCount * (this.queryParams.pageIndex - 1)}`;
                return (<span>{result}</span>);
              },
              isSelf: (value, record, index) => (value ? <span>是</span> : <span>否</span>),
              isPartyAProvide: (value, record, index) => {
                // 如果 自己提供 ，甲供这里就填空
                if (record.isSelf) {
                  return (<span />);
                } else {
                  // 甲也不提供 的话 也填空
                  if (value) {
                    return (<span>是</span>);
                  }
                  return (<span />);
                }
              },
              presentDays: (value, record, index) => (record.isSelf ? <span>{value}</span> : <span />),
              prePurchaseDays: (value, record, index) => (record.isSelf ? <span>{value}</span> : <span />),
              operations: (record) => {
                return (
                  <div>
                    <div style='display:inline' onClick={() => this.edit(record)}><a>编辑</a></div>
                    <div style='display:inline;margin-left:10px' onClick={() => this.remove(record)}><a>删除</a></div>
                  </div>
                );
              },
            },
          }}
        />

        {/*分页*/}
        <a-pagination
          style='margin-top:10px; text-align: right;'
          total={this.queryParams.totalCount}
          pageSize={this.queryParams.maxResultCount}
          current={this.queryParams.pageIndex}
          onChange={this.onPageChange}
          onShowSizeChange={this.onPageChange}
          showSizeChanger
          showQuickJumper
          size={this.isSimple || this.isFault ? 'small' : ''}
          showTotal={paginationConfig.showTotal}
        />

        {/*添加/编辑模板*/}
        <SmConstructionBaseMaterialModal
          ref='SmConstructionBaseMaterialModal'
          axios={this.axios}
          onSuccess={async () => {
            await this.refresh();
          }}
        />

      </div>
    );
  },
};
