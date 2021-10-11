
import './style';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import { requestIsSuccess, vIf, vP } from '../../_utils/utils';
import { deleteEmptyProps } from '../../_utils/tree_array_tools';
import ApiProblemCategory from '../../sm-api/sm-problem/ProblemCategory';
import SmProblemProblemCategoryModal from './SmProblemProblemCategoryModal';
import permissionsSmProblem from '../../_permissions/sm-problem';

let apiProblemCategory = new ApiProblemCategory();

export default {
  name: 'SmProblemProblemCategories',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      problemCategories: [],//数据源
      totalCount: 0, //总条数
      pageIndex: 1,
      queryParams: {
        name: '', // 名称搜索
        maxResultCount: paginationConfig.defaultPageSize,
      },
      loading: false,
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '#  名称',
          dataIndex: 'name',
          scopedSlots: { customRender: 'name' },
          ellipsis: true,
        },
        {
          title: '排序',
          dataIndex: 'order',
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 180,
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiProblemCategory = new ApiProblemCategory(this.axios);
    },
    // 添加
    add(parentId) {
      this.$refs.SmProblemProblemCategoryModal.add(parentId);
    },

    //编辑
    edit(record) {
      this.$refs.SmProblemProblemCategoryModal.edit(record);
    },

    // 查看
    view(record) {
      this.$refs.SmProblemProblemCategoryModal.view(record);
    },

    // 删除
    remove(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiProblemCategory.delete(record.id);
            if (requestIsSuccess(response)) {
              _this.refresh(false, _this.pageIndex);
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() { },
      });
    },

    // 刷新列表
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }

      let response = await apiProblemCategory.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });

      if (requestIsSuccess(response) && response.data) {
        let _problemCategories = deleteEmptyProps(response.data.items, 'children', ['children']);
        _problemCategories = this.sortByOrder(_problemCategories);
        this.problemCategories = _problemCategories;
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

    // 根据order进行排序
    sortByOrder(array) {
      array
        .sort((a, b) => a.order - b.order)
        .map(item => {
          if (item.children && item.children.length > 0) {
            this.sortByOrder(item.children);
          }
        });
      return array;
    },

    //切换页码
    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },
  },
  render() {
    return (
      <div class="sm-problem-categories">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.name = '';
            this.refresh();
          }}
        >
          <a-form-item label="名称">
            <a-input
              placeholder="请输入"
              value={this.queryParams.name}
              onInput={event => {
                this.queryParams.name = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>

          <template slot="buttons">
            {vIf(
              <a-button type="primary" onClick={() => this.add()} icon="plus">
                新建
              </a-button>,
              vP(this.permissions, permissionsSmProblem.ProblemCategories.Create),
            )}
          </template>
        </sc-table-operator>

        {/* 展示区 */}
        <a-table
          columns={this.columns}
          dataSource={this.problemCategories}
          rowKey={record => record.id}
          bordered={this.bordered}
          loading={this.loading}
          pagination={false}
          {...{
            scopedSlots: {
              name: (text, record, index) => {
                let result = `${index +
                  1 +
                  this.queryParams.maxResultCount * (this.pageIndex - 1)}. ${text}`;
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },

              operations: (text, record) => {
                return [
                  <span>
                    {vIf(
                      <a
                        onClick={() => {
                          this.add(record.id);
                        }}
                      >
                        增加子项
                      </a>,
                      vP(this.permissions, permissionsSmProblem.ProblemCategories.Create),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsSmProblem.ProblemCategories.Create) &&
                      vP(this.permissions, [
                        permissionsSmProblem.ProblemCategories.Update,
                        permissionsSmProblem.ProblemCategories.Detail,
                        permissionsSmProblem.ProblemCategories.Delete,
                      ]),
                    )}
                    {vIf(
                      <a-dropdown trigger={['click']}>
                        <a class="ant-dropdown-link" onClick={e => e.preventDefault()}>
                          更多 <a-icon type="down" />
                        </a>
                        <a-menu slot="overlay">
                          {vIf(
                            <a-menu-item>
                              <a
                                onClick={() => {
                                  this.view(record);
                                }}
                              >
                                详情
                              </a>
                            </a-menu-item>,
                            vP(this.permissions, permissionsSmProblem.ProblemCategories.Detail),
                          )}
                          {vIf(
                            <a-menu-item>
                              <a
                                onClick={() => {
                                  this.edit(record);
                                }}
                              >
                                编辑
                              </a>
                            </a-menu-item>,
                            vP(this.permissions, permissionsSmProblem.ProblemCategories.Update),
                          )}
                          {vIf(
                            <a-menu-item>
                              <a
                                onClick={() => {
                                  this.remove(record);
                                }}
                              >
                                删除
                              </a>
                            </a-menu-item>,
                            vP(this.permissions, permissionsSmProblem.ProblemCategories.Delete),
                          )}
                        </a-menu>
                      </a-dropdown>,

                      vP(this.permissions, [
                        permissionsSmProblem.ProblemCategories.Update,
                        permissionsSmProblem.ProblemCategories.Detail,
                        permissionsSmProblem.ProblemCategories.Delete,
                      ]),

                    )}

                  </span>,
                ];
              },
            },
          }}
        ></a-table>

        {/* 分页器 */}

        <a-pagination
          style="float:right; margin-top:10px"
          total={this.totalCount}
          pageSize={this.queryParams.maxResultCount}
          current={this.pageIndex}
          onChange={this.onPageChange}
          onShowSizeChange={this.onPageChange}
          showSizeChanger
          showQuickJumper
          showTotal={paginationConfig.showTotal}
        />


        {/* 添加/编辑模板 */}
        <SmProblemProblemCategoryModal
          ref="SmProblemProblemCategoryModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />

      </div>
    );
  },
};
