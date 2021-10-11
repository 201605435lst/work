
import './style';
import { requestIsSuccess, vIf, vP } from '../../_utils/utils';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import ApiProblem from '../../sm-api/sm-problem/Problem';
import SmProblemProblemCategoryTreeSelect from '../sm-problem-problem-category-tree-select';
import SmProblemProblemModal from './SmProblemProblemModal';
import moment from 'moment';
import permissionsSmProblem from '../../_permissions/sm-problem';

let apiProblem = new ApiProblem();

export default {
  name: 'SmProblemProblems',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      problems: [],
      totalCount: 0, //总条数
      pageIndex: 1,
      queryParams: {
        problemCategoryIds: [],
        keywords: '', // 名称搜索
        maxResultCount: paginationConfig.defaultPageSize,
      },
      loading: false,
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          width: 100,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '类型',
          dataIndex: 'problemRltProblemCategories',
          scopedSlots: { customRender: 'problemRltProblemCategories' },
          ellipsis: true,
        },
        {
          title: '名称',
          dataIndex: 'name',
          ellipsis: true,
        },
        // {
        //   title: '详情',
        //   dataIndex: 'content',
        //   ellipsis: true,
        // },
        {
          title: '创建时间',
          dataIndex: 'creationTime',
          scopedSlots: { customRender: 'creationTime' },
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
    await this.refresh();
  },
  methods: {
    initAxios() {
      apiProblem = new ApiProblem(this.axios);
    },

    // 添加
    add() {
      this.$refs.SmProblemProblemModal.add();
    },

    // 编辑
    edit(record) {
      this.$refs.SmProblemProblemModal.edit(record);
    },

    // 详情
    view(record) {
      this.$refs.SmProblemProblemModal.view(record);
    },

    // 删除
    remove(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => (
          <div style="color:red;">
            { tipsConfig.remove.content}
          </div>
        ),
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiProblem.delete(record.id);
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

      let response = await apiProblem.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(response) && response.data) {
        this.problems = response.data.items;
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
      <div class="sm-problem-problems">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.problemCategoryIds = [];
            this.queryParams.keywords = '';
            this.refresh();
          }}
        >
          <a-form-item label="类型">
            <SmProblemProblemCategoryTreeSelect
              placeholder="请选择类型"
              axios={this.axios}
              value={this.queryParams.problemCategoryIds}
              onChange={value => {
                this.queryParams.problemCategoryIds = value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="关键词">
            <a-input
              placeholder="请输入标题、详情"
              value={this.queryParams.keywords}
              onInput={event => {
                this.queryParams.keywords = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>

          <template slot="buttons">
            <div style={'display:flex'}>
              {vIf(
                <a-button type="primary" icon="plus" onClick={this.add}>
                  添加
                </a-button>,
                vP(this.permissions, permissionsSmProblem.Problems.Create),
              )}
            </div>
          </template>
        </sc-table-operator>


        {/* 展示区 */}
        <a-table
          columns={this.columns}
          rowKey={record => record.id}
          dataSource={this.problems}
          bordered={this.bordered}
          pagination={false}
          // size={this.isSimple ? 'small' : 'default'}
          // rowSelection={this.isSimple ? {
          //   type: this.multiple ? 'checkbox' : 'radio',
          //   columnWidth: 30,
          //   selectedRowKeys: this.iSelected,
          //   onChange: selectedRowKeys => {
          //     this.targetIds = selectedRowKeys;
          //     this.updateSelected();
          //   },
          // } : undefined}
          loading={this.loading}
          // scroll={this.isSimple ? { y: 140 } : undefined}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              name: (text, record) => {
                return record.name;
              },
              problemRltProblemCategories: (text, record, index) => {
                let str = '';
                if (
                  record.problemRltProblemCategories &&
                  record.problemRltProblemCategories.length > 0
                ) {
                  record.problemRltProblemCategories.map((item, index) => {
                    str += `${item.problemCategory.name}${index !== record.problemRltProblemCategories.length - 1 ? '、' : ''
                    }`;
                  });
                }
                return <a-tooltip title={str}>{str}</a-tooltip>;
              },
              creationTime: (text, record) => {
                return moment(text).format('YYYY-MM-DD HH:mm:ss');
              },
              operations: (text, record) => {
                return [
                  <span>
                    {vIf(
                      <a
                        onClick={() => {
                          this.view(record);
                        }}
                      >详情
                      </a>,
                      vP(this.permissions, permissionsSmProblem.Problems.Detail),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsSmProblem.Problems.Detail) &&
                      vP(this.permissions, [permissionsSmProblem.Problems.Update, permissionsSmProblem.Problems.Delete]),
                    )}

                    {vIf(
                      <a-dropdown trigger={['click']}>
                        <a
                          class="ant-dropdown-link"
                          onClick={e => e.preventDefault()}>
                          更多 <a-icon type="down" />
                        </a>
                        <a-menu slot="overlay">
                          {vIf(
                            <a-menu-item>
                              <a
                                onClick={() => {
                                  this.edit(record);
                                }}
                              >编辑
                              </a>
                            </a-menu-item>,
                            vP(this.permissions, permissionsSmProblem.Problems.Update),
                          )}
                          {vIf(
                            <a-menu-item>
                              <a
                                onClick={() => {
                                  this.remove(record);
                                }}
                              >删除
                              </a>
                            </a-menu-item>,
                            vP(this.permissions, permissionsSmProblem.Problems.Delete),
                          )}
                        </a-menu>
                      </a-dropdown>,
                      vP(this.permissions, [permissionsSmProblem.Problems.Update, permissionsSmProblem.Problems.Delete]),
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
          // size={this.isSimple ? 'small' : ''}
          showTotal={paginationConfig.showTotal}
        />

        <SmProblemProblemModal
          ref="SmProblemProblemModal"
          axios={this.axios}
          bordered={this.bordered}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
      </div>
    );
  },
};
