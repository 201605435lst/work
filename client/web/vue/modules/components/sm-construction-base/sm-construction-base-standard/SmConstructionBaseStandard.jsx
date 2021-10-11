
import './style';
import { requestIsSuccess } from '../../_utils/utils';
import ApiStandard from '../../sm-api/sm-construction-base/ApiStandard';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmConstructionBaseStandardModal from './SmConstructionBaseStandardModal';
import ApiDataDictionary from '../../sm-api/sm-system/DataDictionary';


let apiStandard = new ApiStandard();
let apiDataDictionary = new ApiDataDictionary();// 字典api 查 类型用

export default {
  name: 'SmConstructionBaseStandard',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] }, // 权限
    bordered: { type: Boolean, default: false },
    showOperator: { type: Boolean, default: true }, // 是否显示操作栏()
    showSelectRow: { type: Boolean, default: false }, // 是否显示选择栏
    multiple: { type: Boolean, default: false },//是否多选
    selected: { type: Array, default: () => [] }, //待选计划所选工序规范集合
    size: { type: String, default: 'default' },//表格模式
  },
  data() {
    return {
      queryParams: {

        searchKey: '', // 模糊搜索
        professionId: undefined, // 根据所属专业id 搜索
        maxResultCount: paginationConfig.defaultPageSize, // 每页数量
        pageIndex: 1, // 当前页1 这个在 传后端 的时候 过滤掉了,放这里方便复制~
        totalCount: 0, // 总数 这个在 传后端 的时候 过滤掉了,放这里方便复制~
      },
      list: [], // table 数据源
      loading: false, // table 是否处于加载状态
      selectedStandardIds: [], // 选择的 工艺规范 ids
      iSelected: [],//已选 工艺规范 实体
      dicTypes: [], // 类型列表
    };
  },
  computed: {
    columns() {
      let baseColumns = [
        {
          title: '序号', dataIndex: 'id', width: 100, customRender: (text, record, index) => {
            let result = `${index + 1 + this.queryParams.maxResultCount * (this.queryParams.pageIndex - 1)}`;
            return (<span>{result}</span>);
          },
        },

        { title: '规范名称', dataIndex: 'name' },
        { title: '规范编号', dataIndex: 'code' },
        { title: '所属专业', dataIndex: 'profession' },
      ];
      return this.showOperator ? [
        ...baseColumns,
        {
          title: '操作', width: 180, customRender: (record) => {
            return (
              <div>
                <div style='display:inline' onClick={() => this.edit(record)}><a>编辑</a></div>
                <div style='display:inline;margin-left:10px' onClick={() => this.remove(record)}><a>删除</a></div>
              </div>
            );
          },
        },
      ] : baseColumns;
    },
  },
  watch: {
    selected: {
      handler: function (value, oldVal) {
        this.iSelected = value;
        this.selectedStandardIds = value.map(item => item.id);
      },
      immediate: true,
    },
  },
  created() {
    this.initAxios();
    this.refresh();
    this.getDicTypes(); // 获取字典类型列表
  },
  methods: {
    // 初始化axios,将apiStandard实例化
    initAxios() {
      apiStandard = new ApiStandard(this.axios);
      apiDataDictionary = new ApiDataDictionary(this.axios);
    },
    // 获取 工程类型 列表
    async getDicTypes() {
      let res = await apiDataDictionary.getValues({ groupCode: 'Profession.' });
      if (requestIsSuccess(res) && res.data) {
        this.dicTypes = res.data.filter(x => x.name !== "全部");
      }
    },
    // 刷新获取list
    async refresh() {
      this.loading = true;
      let res = await apiStandard.getList({
        skipCount: (this.queryParams.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(res) && res.data) {
        this.list = res.data.items;
        this.queryParams.totalCount = res.data.totalCount;
      }
      this.loading = false;
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
      this.$refs.SmConstructionBaseStandardModal.add();
    },
    // 添加(打开添加模态框) 123
    edit(record) {
      this.$refs.SmConstructionBaseStandardModal.edit(record);
    },
    remove(record) {
      const _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style='color:red;'>{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            const response = await apiStandard.delete(record.id);
            _this.refresh();
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },

    //更新所选数据
    updateSelected(selectedRows) {
      if (this.multiple) {
        // 过滤出其他页面已经选中的
        let _selected = [];
        for (let item of this.iSelected) {
          let target = this.list.find(subItem => subItem.id === item.id);
          if (!target) {
            _selected.push(item);
          }
        }

        // 把当前页面选中的加入
        for (let id of this.selectedStandardIds) {
          let standard = this.list.find(item => item.id === id);
          if (!!standard) { // !! 是 js 里面  (a!=null&&typeof(a)!=undefined&&a!='') 的简略写法
            _selected.push(JSON.parse(JSON.stringify(standard))); // 为了保证 _selected 改变(不这样写拿到的是内存的引用……)
          }
        }

        this.iSelected = _selected;
      } else {
        this.iSelected = selectedRows;
      }

      this.$emit('change', this.iSelected);
    },
  },
  render() {
    return (
      <div>
        {/* 操作区 */}
        <sc-table-operator
          size={this.size}
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.searchKey = '';
            this.queryParams.professionId = undefined;
            this.queryParams.pageIndex = 1;
            this.refresh();
          }}
        >
          <a-form-item label='关键字'>
            <a-input
              placeholder={'请输入关键字'}
              size={this.size}
              value={this.queryParams.searchKey}
              onInput={async event => {
                this.queryParams.searchKey = event.target.value;
                this.queryParams.pageIndex = 1; // 查询的时候 当前页给1,不然查到了数据也不显示
                this.refresh();
              }}
            />
          </a-form-item>

          <a-form-item label='专业'>
            <a-select
              placeholder={'请选择专业'}
              value={this.queryParams.professionId}
              size={this.size}
              onChange={(val) => {
                this.queryParams.professionId = val;
                this.refresh();
              }}>
              {this.dicTypes.map(x => (<a-select-option value={x.id}>{x.name}</a-select-option>))}
            </a-select>
          </a-form-item>

          {this.showOperator ? (
            <template slot='buttons'>
              <a-button
                size={this.size}
                type='primary'
                icon='plus'
                onClick={() => this.add()}
              >
                添加
              </a-button>
            </template>
          ) : undefined}

        </sc-table-operator>
        {/*展示区*/}
        <a-table
          dataSource={this.list}
          rowKey={record => record.id}
          columns={this.columns}
          loading={this.loading}
          bordered={this.bordered}
          size={this.size}
          scroll={this.showSelectRow ? { y: 300 } : undefined}
          pagination={false}
          rowSelection={this.showSelectRow ? {
            type: this.multiple ? 'checkbox' : 'radio',
            columnWidth: 30,
            selectedRowKeys: this.selectedStandardIds, // 绑定 已选择的数据
            onChange: (selectedRowKeys, selectedRows) => {
              this.selectedStandardIds = selectedRowKeys;
              this.updateSelected(selectedRows);
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
          showSizeChanger
          showQuickJumper
          size={this.size}
          showTotal={paginationConfig.showTotal}
        />

        {/*添加/编辑模板*/}
        <SmConstructionBaseStandardModal
          ref='SmConstructionBaseStandardModal'
          axios={this.axios}
          onSuccess={async () => {
            await this.refresh();
          }}
        />
      </div>
    );
  },



};
