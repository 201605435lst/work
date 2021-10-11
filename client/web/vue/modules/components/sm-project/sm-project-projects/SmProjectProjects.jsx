import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import { requestIsSuccess, vP, vIf } from '../../_utils/utils';
import SmFileManageSelect from '../../sm-file/sm-file-manage-select';
import './style/index.less';
import * as utils from '../../_utils/utils';
import ApiProject from "../../sm-api/sm-project/Project";
import { PageState, ProjectState, BuildUnitType } from '../../_utils/enum';
import permissionsSmProject from '../../_permissions/sm-project';
import FileSaver from 'file-saver';

let apiProject = new ApiProject();
export default {
  name: 'SmProjectProjects',
  props: {
    permissions: { type: Array, default: () => [] },
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
  },
  data() {
    return {
      loading: false,
      totalCount: 0,
      pageIndex: 1,
      pageSize: paginationConfig.defaultPageSize,
      projectIds: [],//导出
      exportLoading:false,
      queryParams: {
        keywords: null, // 名称搜索
      },
    };
  },

  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          width: 90,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '项目名称',
          dataIndex: 'name',
          ellipsis: true,
        },
        {
          title: '编号',
          dataIndex: 'code',
          ellipsis: true,
        },
        {
          title: '组织',
          dataIndex: 'organization',
          scopedSlots: { customRender: 'organization' },
          ellipsis: true,
        },
        {
          title: '负责人',
          dataIndex: 'manager',
          scopedSlots: { customRender: 'manager'},
          ellipsis: true,
        },
        {
          title: '成员',
          dataIndex: 'members',
          ellipsis: true,
          scopedSlots: { customRender: 'members' },
        },
        {
          title: '位置',
          dataIndex: 'address',
          scopedSlots: { customRender: 'address' },
          ellipsis: true,
        },
        {
          title: '概况',
          dataIndex: 'description',
          ellipsis: true,
        },
        {
          title: '项目状态',
          dataIndex: 'state',
          scopedSlots: { customRender: 'state' },
          ellipsis: true,
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: '140px',
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  watch: {
  },
  mounted() {
    this.refresh();
  },
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiProject = new ApiProject(this.axios);
    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let queryParams = {
        keywords: this.queryParams.keywords, // 名称搜索
        maxResultCount: paginationConfig.defaultPageSize,
        maxResultCount: this.queryParams.maxResultCount,
      };

      let response = await apiProject.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...queryParams,
      });
      if (requestIsSuccess(response) && response.data.items) {
        this.projectData = response.data.items;
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
    //切换页码
    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },
    //添加
    add() {
      this.$emit('add');
    },
    //详情
    view(record) {
      this.$emit('view',record.id);
    },
    //编辑
    edit(record) {
      this.$emit('edit', record.id);
    },
    //删除
    remove(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiProject.delete(record.id);
            if (requestIsSuccess(response)) {
              _this.refresh(false, _this.pageIndex);
              _this.$message.success('删除成功');
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() { },
      });
    },
    //更新状态
    async updateState(state, id) {
      let data = {
        id: id,
        state:state,
      };
      let response = await apiProject.updateState(data);
      if (utils.requestIsSuccess(response)) {
        this.$message.info("操作成功");
        this.refresh();
      }
    },
    //导出
    async export() {
      let _this = this;
      this.exportLoading = true;
      let data = { projectIds:this.projectIds};
      return new Promise(async (resolve, reject) => {
        let response = await apiProject.export(data);
        _this.exportLoading = false;
        if (requestIsSuccess(response)) {
          FileSaver.saveAs(
            new Blob([response.data], { type: 'application/vnd.ms-excel' }),
            `项目台账.xlsx`,
          );
          setTimeout(resolve, 100);
        } else {
          setTimeout(reject, 100);
        }
      });
    },
  },
  render() {
    return (
      <div class="SmEmergPlans">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams = {
              keywords: null,
              compoentCategoryIds: [],
              levelId: undefined,
            };
            this.refresh();
          }}
        >
          <a-form-item label="关键字">
            <a-input
              placeholder="请输入项目名称或合同编号"
              value={this.queryParams.keywords}
              onInput={value => {
                this.queryParams.keywords = value.target.value;
                this.refresh(false);
              }}
            />
          </a-form-item>
          <template slot="buttons">
            {
              vIf(
                <a-button type="primary" onClick={this.add}>
                  添加
                </a-button>,
                vP(this.permissions, permissionsSmProject.Projects.Create),
              )
              
            }
          </template>
          <template slot="buttons">
            {
              vIf(
                <a-button type="primary" onClick={() => this.export()} disabled={this.projectIds.length === 0} loading={this.exportLoading}>
                  导出
                </a-button>,
                vP(this.permissions, permissionsSmProject.Projects.Export),
              )

            }
          </template>
        </sc-table-operator>
        {/* 表格展示 */}
        <a-table
          columns={this.columns}
          dataSource={this.projectData}
          rowKey={record => record.id}
          bordered={this.bordered}
          loading={this.loading}
          pagination={false}
          pagination={false}
          rowSelection={{
            columnWidth: 30,
            onChange: selectedRowKeys => {
              this.projectIds = selectedRowKeys;
              console.log(this.exportList);
            },
          }}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              organization: (text, record, index) => {
                return (
                  <a-tooltip placement="topLeft" title={record.organization.name}>
                    <span>{record.organization.name}</span>
                  </a-tooltip>);
              },
              state: (text, record, index) => {
                return utils.getProjectStateTitle(record.state);
              },
              manager: (text, record, index) => {
                return (
                  <a-tooltip placement="topLeft" title={record.manager.name}>
                    <span>{record.manager.name}</span>
                  </a-tooltip>);
              },
              members: (text, record, index) => {
                let result = record.projectRltMembers.length > 0 ? `${record.projectRltMembers.map(item => item.manager.name)}` : "";
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>);
              },
              operations: (text, record, index) => {
                return [
                  <span>
                    {vIf(
                      <a
                        onClick={() => {
                          this.view(record);
                        }}>详情</a>,
                      vP(this.permissions, permissionsSmProject.Projects.Detail),
                    )}
                    {
                      vIf(
                        <a-divider type="vertical" />,
                        vP(this.permissions, permissionsSmProject.Projects.Detail) &&
                        vP(this.permissions, [permissionsSmProject.Projects.Delete, permissionsSmProject.Projects.Update]),
                      )
                    }
                    {
                      vIf(
                        <span>
                          <a-dropdown trigger={['click']}>
                            <a class="ant-dropdown-link" onClick={''}>
                              更多 <a-icon type="down" />
                            </a>
                            <a-menu slot="overlay">
                              {
                                vIf(
                                  [<a-menu-item>
                                    <a onClick={() => {
                                      this.edit(record);
                                    }}>
                                      编辑
                                    </a>
                                  </a-menu-item>,
                                  record.state !== ProjectState.Start ?
                                    <a-menu-item>
                                    
                                      <a
                                        onClick={() => {
                                          this.updateState(ProjectState.Start, record.id);
                                        }}
                                      >
                                        启动
                                      </a>
                                    </a-menu-item> : "",
                                  record.state !== ProjectState.Finshed?
                                    <a-menu-item>
                                      <a
                                        onClick={() => {
                                          this.updateState(ProjectState.Finshed, record.id);
                                        }}
                                      >
                                        竣工
                                      </a>
                                    </a-menu-item> : "",
                                  record.state !== ProjectState.Stop ?
                                    <a-menu-item>
                                      <a
                                        onClick={() => {
                                          this.updateState(ProjectState.Stop, record.id);
                                        }}
                                      >
                                        终止
                                      </a>
                                    </a-menu-item> : ""],
                                  vP(this.permissions, permissionsSmProject.Projects.Update),
                                )
                              }
                              {
                                vIf(
                                  record.state !== ProjectState.Start ?
                                    <a-menu-item>
                                      <a
                                        onClick={() => {
                                          this.remove(record);
                                        }}
                                      >
                                    删除
                                      </a>
                                    </a-menu-item> : "",
                                  vP(this.permissions, permissionsSmProject.Projects.Delete),
                                )
                              }
                            </a-menu>
                          </a-dropdown>
                        </span>,
                        vP(this.permissions, [permissionsSmProject.Projects.Update, permissionsSmProject.Projects.Delete]),
                      )
                    }
                    
                  </span>,
                ];
              },
            },
          }}
        ></a-table>

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

      </div>
    );
  },
};
