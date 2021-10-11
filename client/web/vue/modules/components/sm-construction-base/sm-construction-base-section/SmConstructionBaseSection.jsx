
import './style';
import { requestIsSuccess } from '../../_utils/utils';
import ApiSection from '../../sm-api/sm-construction-base/ApiSection';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmConstructionBaseSectionModal from './SmConstructionBaseSectionModal';
import { treeArrayToFlatArray } from '../../_utils/tree_array_tools';

let apiSection = new ApiSection();

export default {
  name: 'SmConstructionBaseSection',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] }, // 权限
    bordered: { type: Boolean, default: true },
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
        maxResultCount: paginationConfig.defaultPageSize, // 每页数量
        pageIndex: 1, // 当前页1 这个在 传后端 的时候 过滤掉了,放这里方便复制~
        totalCount: 0, // 总数 这个在 传后端 的时候 过滤掉了,放这里方便复制~
      },
      list: [], // table 数据源
      flatList: [], // 平状数据
      loading: false, // table 是否处于加载状态
      selectedSectionIds: [], // 选择的 施工区段 ids
      iSelected: [],//已选 施工区段 实体
    };
  },
  computed: {
    columns() {
      let baseColumns = [
        {
          title: '区段名称', dataIndex: 'name', customRender: (text, record, index) => {
            return `${index + 1 + this.queryParams.maxResultCount * (this.queryParams.pageIndex - 1)}. ${text}`;
          },
        },
        { title: '区段描述', dataIndex: 'desc' },
      ];
      return this.showOperator ? [
        ...baseColumns,
        {
          title: '操作', width: 180, customRender: (record) => {
            return (
              <div>
                <div style='display:inline' onClick={() => this.addChildren(record)}><a>新增</a></div>
                <div style='display:inline;margin-left:10px' onClick={() => this.edit(record)}><a>编辑</a></div>
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
        this.selectedSectionIds = value.map(item => item.id);
      },
      immediate: true,
    },
  },
  created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    // 初始化axios,将apiSection实例化
    initAxios() {
      apiSection = new ApiSection(this.axios);
    },
    // 刷新获取list
    async refresh() {
      this.list = [];
      this.loading = true;
      let res = await apiSection.getTreeList({
        skipCount: (this.queryParams.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(res) && res.data) {
        this.list = res.data.items;
        this.queryParams.totalCount = res.data.totalCount;
        this.flatList = treeArrayToFlatArray(res.data.items);

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
      this.$refs.SmConstructionBaseSectionModal.add();
    },
    // 添加子级(打开添加模态框)
    addChildren(record) {
      this.$refs.SmConstructionBaseSectionModal.addChildren(record);
    },
    // 添加(打开添加模态框) 123
    edit(record) {
      this.$refs.SmConstructionBaseSectionModal.edit(record);
    },
    remove(record) {
      const _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style='color:red;'>{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            const response = await apiSection.delete(record.id);
            _this.refresh();
            setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
          });
        },
      });
    },
    // //更新所选数据
    // updateSelected() {
    //   // this.iSelected = selectedRows;
    //   // 当选择模式时，数据不能删除
    //   this.$emit('change', this.iSelected);
    // },

    //更新所选数据
    updateSelected(selectedRows) {
      if (this.multiple) {
        // 过滤出其他页面已经选中的
        let _selected = [];
        for (let item of this.iSelected) {
          let target = this.flatList.find(subItem => subItem.id === item.id);
          if (!target) {
            _selected.push(item);
          }
        }

        // 把当前页面选中的加入
        for (let id of this.selectedSectionIds) {
          let section = this.flatList.find(item => item.id === id);
          if (!!section) {
            _selected.push(JSON.parse(JSON.stringify(section)));
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
        {this.list && this.list.length ?
          <a-table
            dataSource={this.list}
            rowKey={record => record.id}
            columns={this.columns}
            loading={this.loading}
            size={this.size}
            pagination={false}
            bordered={this.bordered}
            defaultExpandAllRows={true}
            scroll={this.showSelectRow ? { y: 300 } : undefined}
            rowSelection={this.showSelectRow ? {
              type: this.multiple ? 'checkbox' : 'radio',
              columnWidth: 30,
              selectedRowKeys: this.selectedSectionIds, // 绑定 已选择的数据
              onChange: (selectedRowKeys, selectedRows) => {
                this.selectedSectionIds = selectedRowKeys;
                this.updateSelected(selectedRows);
              },
            } : undefined}
          />
          :
          '暂无数据 '
        }

        {/*分页*/}
        <a-pagination
          style='margin-top:10px; text-align: right;'
          total={this.queryParams.totalCount}
          pageSize={this.queryParams.maxResultCount}
          current={this.queryParams.pageIndex}
          onChange={this.onPageChange}
          onShowSizeChange={this.onPageChange}
          size={this.size}
          showSizeChanger={true}
          showQuickJumper={true}
          showTotal={paginationConfig.showTotal}
        />

        {/*添加/编辑模板*/}
        <SmConstructionBaseSectionModal
          ref='SmConstructionBaseSectionModal'
          axios={this.axios}
          onSuccess={async () => {
            await this.refresh();
          }}
        />
      </div>
    );
  },



};
