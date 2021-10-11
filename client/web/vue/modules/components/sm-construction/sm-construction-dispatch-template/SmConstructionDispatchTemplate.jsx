
import './style';
import * as utils from '../../_utils/utils';
import { requestIsSuccess } from '../../_utils/utils';
import ApiDispatchTemplate from '../../sm-api/sm-construction/ApiDispatchTemplate';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmConstructionDispatchTemplateModal from './SmConstructionDispatchTemplateModal';



let apiDispatchTemplate = new ApiDispatchTemplate();

export default {
  name: 'SmConstructionDispatchTemplate',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: true },
    showOperator: { type: Boolean, default: true }, // 是否显示操作栏()
    showSelectRow: { type: Boolean, default: false }, // 是否显示选择栏
  },
  data() {
    return {
      queryParams: {

        searchKey: '', // 模糊搜索 
        maxResultCount: paginationConfig.defaultPageSize, // 每页数量
        pageIndex: 1, // 当前页1 这个在 传后端 的时候 过滤掉了,放这里方便复制~
        totalCount: 0, // 总数 这个在 传后端 的时候 过滤掉了,放这里方便复制~
      },
      list: [], // table 数据源
      loading: false, // table 是否处于加载状态
      selectDispatchTemplateIds: [], // 选择的 派工单模板 ids (选择框模式的时候用)
      dicTypes: [],
      nameList: [],
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          ellipsis: true,
          width: 100,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '模板名称',
          dataIndex: 'name',
          ellipsis: true,
          scopedSlots: { customRender: 'name' },
        },

        {
          title: '模板说明',
          ellipsis: true,
          dataIndex: 'description',
          scopedSlots: { customRender: 'description' },
        },
        {
          title: '状态',
          ellipsis: true,
          dataIndex: 'isDefault',
          scopedSlots: { customRender: 'isDefault' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 169,
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },

  created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    // 初始化axios,将apiStandard实例化
    initAxios() {
      apiDispatchTemplate = new ApiDispatchTemplate(this.axios);
    },
    // 刷新获取list 
    async refresh() {
      this.loading = true;
      let res = await apiDispatchTemplate.getAllList({
        skipCount: (this.queryParams.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(res) && res.data) {
        this.list = res.data.items;
        // console.log(res.data.items);
        this.queryParams.totalCount = res.data.totalCount;
      }
      this.loading = false;
      // 刷新获取全部模板供提交时判断name
      let resModal = await apiDispatchTemplate.getAllList({ isAll: true });
      if (requestIsSuccess(resModal) && resModal.data && resModal.data.items) {
        this.nameList = [];
        resModal.data.items.map(item => {
          this.nameList.push(item.name);
        });
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
    // 添加(打开添加模态框)
    add() {
      this.$refs.SmConstructionDispatchTemplateModal.add();
    },
    // 添加(打开添加模态框)
    edit(record) {
      this.$refs.SmConstructionDispatchTemplateModal.edit(record);
    },
    // 编辑(打开编辑模态框)
    view(record) {
      this.$refs.SmConstructionDispatchTemplateModal.view(record);
    },
    // 设为模板
    async setDefault(record) {
      let response = await apiDispatchTemplate.setDefault(record.id);
      if (utils.requestIsSuccess(response)) {
        this.$message.success('操作成功');
      }
      this.refresh();
    },
    remove(record) {
      const _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style='color:red;'>{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            const response = await apiDispatchTemplate.delete(record.id);
            _this.refresh();
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },
  },
  render() {
    return (
      <div class="sm-construction-dispatch-template">
        {/* 操作区 */}
        <sc-table-operator
          size={this.isSimple ? 'small' : 'default'}
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.searchKey = '';
            this.queryParams.pageIndex = 1;
            this.refresh();
          }}
        >

          <a-form-item label="模糊搜索">
            <a-input
              size={this.isSimple ? 'small' : 'default'}
              placeholder="请输入模板名称、说明"
              value={this.queryParams.searchKey}
              onInput={event => {
                this.queryParams.searchKey = event.target.value;
                this.queryParams.pageIndex = 1; // 查询的时候 当前页给1,不然查到了数据也不显示
                this.refresh();
              }}
            />
          </a-form-item>
          <template slot='buttons'>
            <a-button
              size='small'
              style="margin-left: 10px;"
              type="primary"
              onClick={() => this.add()}
            >
              新增
            </a-button>
          </template>
        </sc-table-operator>
        {/*展示区*/}
        <a-table
          dataSource={this.list}
          rowKey={record => record.id}
          columns={this.columns}
          loading={this.loading}
          pagination={false}
          class="dispatch-template-table"
          rowSelection={this.showSelectRow ? {
            selectedRowKeys: this.selectDispatchTemplateIds,
            columnWidth: 30,
            onChange: (selectedRowKeys, selectedRows) => {
              this.selectDispatchTemplateIds = selectedRowKeys;
            },
          } : undefined}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                let result = `${index + 1 + this.queryParams.maxResultCount * (this.queryParams.pageIndex - 1)}`;
                return (<span>{result}</span>);
              },
              name: (text, record) => {
                let result = record ? record.name : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <a onClick={() => this.view(record)}> {result}</a>
                  </a-tooltip>
                );
              },
              description: (text, record) => {
                let result = record ? record.description : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span> {result}</span>
                  </a-tooltip>
                );
              },
              isDefault: (text, record) => {
                let result = record && record.isDefault ?
                  <a-tag color="blue">
                    默认模板
                  </a-tag> : '';
                return result;
              },
              operations: (text, record) => {
                return (
                  <div>
                    <div style='display:inline' onClick={() => this.edit(record)}><a>编辑</a></div>
                    <div style='display:inline;margin-left:10px' onClick={() => this.remove(record)}><a>删除</a></div>
                    {record.isDefault ? '' : <div style='display:inline;margin-left:10px' onClick={() => this.setDefault(record)}><a>设为默认</a></div>}
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
          showSizeChanger={true}
          showQuickJumper={true}
          size={this.isSimple || this.isFault ? 'small' : ''}
          showTotal={paginationConfig.showTotal}
        />

        {/*添加/编辑模板*/}
        <SmConstructionDispatchTemplateModal
          ref='SmConstructionDispatchTemplateModal'
          axios={this.axios}
          nameList={this.nameList}
          onSuccess={async () => {
            this.list = []; // 有树状table的话,给数组清空,不然table 默认不展开 ……
            await this.refresh();
          }}
        />
      </div>
    );
  },



};
