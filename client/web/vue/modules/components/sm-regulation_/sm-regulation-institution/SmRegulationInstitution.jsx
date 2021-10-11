import './style';
import ApiInstitution from '../../sm-api/sm-regulation/Institution';
import { requestIsSuccess, vIf, vP } from '../../_utils/utils';
import SmRegulationInstitutionModal from './SmRegulationInstitutionModal';
import SmRegulationPermissionModal from './SmRegulationPermissionModal';
import SmRegulationAuditModal from './SmRegulationAuditModal';
import SmRegulationViewModal from './SmRegulationViewModal';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import moment from 'moment';
import FileSaver from 'file-saver';
import permissionsSmRegulation from '../../_permissions/sm-regulation';

let apiInstitution = new ApiInstitution();

export default {
  name: 'SmRegulationInstitution',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
  },

  data() {
    return {
      dataSource: [], //数据源
      pageIndex: 1,
      totalCount: 0,
      columnKey: 'creationTime', //默认排序索引创建时间
      order: 'descend', //默认排序方式降序
      pageSize: paginationConfig.defaultPageSize,
      queryParams: {
        keyWords: null, //关键字
        maxResultCount: paginationConfig.defaultPageSize,
      },
      selectedRowKeys: [], //选择的制度
    };
  },

  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '文件标题',
          dataIndex: 'header',
          scopedSlots: { customRender: 'header' },
        },
        {
          title: '文件编号',
          dataIndex: 'code',
        },
        {
          title: '状态',
          dataIndex: 'state',
        },
        {
          title: '所属部门',
          dataIndex: 'organizationId',
          scopedSlots: { customRender: 'organizationId' },
        },
        {
          title: '制度分类',
          dataIndex: 'classify',
          scopedSlots: { customRender: 'classify' },
        },
        {
          title: '录入人',
          dataIndex: 'inputPeople',
        },
        {
          title: '生效时间',
          dataIndex: 'effectiveTime',
          width: 160,
          sorter: () => {},
          scopedSlots: { customRender: 'effectiveTime' },
        },
        {
          title: '创建时间',
          dataIndex: 'creationTime',
          width: 160,
          sorter: () => {},
          scopedSlots: { customRender: 'creationTime' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },

  watch: {},

  async created() {
    this.initAxios();
    this.refresh();
  },

  methods: {
    initAxios() {
      apiInstitution = new ApiInstitution(this.axios);
    },

    // 添加
    add() {
      this.$refs.SmRegulationInstitutionModal.add();
    },

    // 编辑
    edit(record) {
      this.$refs.SmRegulationInstitutionModal.edit(record);
    },

    //修改权限
    editPermission(selectedRowKeys) {
      if (this.selectedRowKeys.length == 0) {
        this.$message.error('请选择制度信息！');
        return;
      }
      this.$refs.SmRegulationPermissionModal.editPermission(this.selectedRowKeys);
    },

    authority(record) {
      this.$refs.SmRegulationPermissionModal.authority(record);
    },

    //审核
    audit(record) {
      //this.$refs.SmRegulationAuditModal.audit(record);
      this.$emit('audit', record.id);
    },

    //查看
    view(record) {
      this.$emit('view', record.id);
    },

    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let data = {
        ...this.queryParams,
        columnKey: this.columnKey,
        order: this.order ? this.order : 'descend',
      };
      let response = await apiInstitution.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...data,
      });
      if (requestIsSuccess(response)) {
        this.dataSource = response.data.items;
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
      if (this.dataSource.length > 0) {
        this.isCanExport = false;
      }
    },

    //删除
    async remove(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiInstitution.delete(record.id);
            if (requestIsSuccess(response)) {
              _this.refresh();
              _this.$message.success('删除成功');
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() {},
      });
    },

    //导出
    async download(multiple, selectedRowKeys) {
      if (this.selectedRowKeys.length == 0) {
        this.$message.error('请选择制度信息！');
        return;
      }
      let _this = this;
      _this.$confirm({
        title: '确认导出',
        content: h => (
          <div style="color:red;">{`确定要导出这 ${this.selectedRowKeys.length} 条数据吗？`}</div>
        ),
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiInstitution.downLoad(_this.selectedRowKeys);
            if (requestIsSuccess(response)) {
              FileSaver.saveAs(
                new Blob([response.data], { type: 'application/vnd.ms-excel' }),
                `制度管理表.xlsx`,
              );
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() {},
      });
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
      <div class="sm-regulation-institution">
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.keyWords = null;
            this.refresh();
          }}
        >
          <a-form-item label="关键字">
            <a-input
              allowClear
              placeholder="请输入文件名称或者文件编号"
              value={this.queryParams.keyWords}
              onInput={event => {
                this.queryParams.keyWords = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>

          <template slot="buttons">
            <div class="buttons">
              {vIf(
                <a-button
                  type="primary"
                  icon="plus"
                  onClick={() => {
                    this.add();
                  }}
                >
                  新建
                </a-button>,
                vP(this.permissions, permissionsSmRegulation.Institutions.Create),
              )}
              {vIf(
                <a-button
                  type="primary"
                  icon="upload"
                  onClick={() => {
                    this.download(true, this.selectedRowKeys);
                  }}
                >
                  导出
                </a-button>,
                vP(this.permissions, permissionsSmRegulation.Institutions.Export),
              )}
              {vIf(
                <a-button
                  type="primary"
                  icon="edit"
                  onClick={() => {
                    this.editPermission(this.selectedRowKeys);
                  }}
                >
                  批量修改权限
                </a-button>,
                vP(this.permissions, permissionsSmRegulation.Institutions.Update),
              )}
            </div>
          </template>
        </sc-table-operator>

        {/* 展示区 */}
        <a-table
          columns={this.columns}
          dataSource={this.dataSource}
          rowKey={record => record.id}
          loading={this.loading}
          pagination={false}
          onChange={(a, b, c) => {
            this.columnKey = c.columnKey;
            this.order = c.order;
            this.refresh();
          }}
          rowSelection={{
            onChange: selectedRowKeys => {
              this.selectedRowKeys = selectedRowKeys;
            },
          }}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                let result = index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                return result;
              },
              header: (text, record, index) => {
                return [
                  <span>
                    {record.viewInstitution == record.id ? (
                      <span>
                        {vIf(
                          <a
                            href="#"
                            onClick={() => {
                              this.view(record);
                            }}
                          >
                            <a-tooltip title={'查看制度'}>{record.header}</a-tooltip>
                          </a>,
                          vP(this.permissions, permissionsSmRegulation.Institutions.Detail),
                        )}
                      </span>
                    ) : (
                      <a-tooltip title={'查看制度'}>{record.header}</a-tooltip>
                    )}
                  </span>,
                ];
              },
              organizationId: (text, record, index) => {
                let result = record.organization.name;
                return result;
              },
              classify: (text, record, index) => {
                let result = record.classify;
                switch (result) {
                  case 1:
                    return '行政法规';
                  case 2:
                    return '章程';
                  case 3:
                    return '制度';
                  case 4:
                    return '公约';
                }
              },
              creationTime: (text, record, index) => {
                let result = moment(record.creationTime).format('YYYY-MM-DD hh:mm:ss');
                return result;
              },
              effectiveTime: (text, record, index) => {
                let result = moment(record.creationTime).format('YYYY-MM-DD hh:mm:ss');
                return result;
              },
              operations: (text, record) => {
                return [
                  <span>
                    {record.editInstitution == record.id ? (
                      <span>
                        {vIf(
                          <a
                            onClick={() => {
                              this.edit(record);
                            }}
                          >
                            编辑
                          </a>,
                          vP(this.permissions, permissionsSmRegulation.Institutions.Update),
                        )}
                        {vIf(
                          <a-divider type="vertical" />,
                          vP(this.permissions, permissionsSmRegulation.Institutions.Update) &&
                            vP(this.permissions, [
                              permissionsSmRegulation.Institutions.Update,
                              permissionsSmRegulation.Institutions.Update,
                              permissionsSmRegulation.Institutions.Delete,
                            ]),
                        )}
                      </span>
                    ) : (
                      undefined
                    )}
                    {vIf(
                      <a-dropdown trigger={['click']}>
                        <a class="ant-dropdown-link">
                          更多 <a-icon type="down" />
                        </a>

                        <a-menu slot="overlay">
                          {record.editInstitution == record.id ? (
                            <a-menu-item>
                              {vIf(
                                <a onClick={() => this.authority(record)}>权限</a>,
                                vP(this.permissions, permissionsSmRegulation.Institutions.Update),
                              )}
                            </a-menu-item>
                          ) : (
                            undefined
                          )}

                          {/* <a-menu-item>
                          <a>
                            发布
                          </a>
                        </a-menu-item> */}
                          {vIf(
                            <a-menu-item>
                              <a
                                onClick={() => {
                                  this.audit(record);
                                }}
                              >
                                审核
                              </a>
                            </a-menu-item>,
                            vP(this.permissions, permissionsSmRegulation.Institutions.Update),
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
                            vP(this.permissions, permissionsSmRegulation.Institutions.Delete),
                          )}
                        </a-menu>
                      </a-dropdown>,
                      vP(this.permissions, [
                        permissionsSmRegulation.Institutions.Update,
                        permissionsSmRegulation.Institutions.Update,
                        permissionsSmRegulation.Institutions.Delete,
                      ]),
                    )}
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

        <SmRegulationInstitutionModal
          ref="SmRegulationInstitutionModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
        <SmRegulationPermissionModal
          ref="SmRegulationPermissionModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />

        <SmRegulationViewModal
          ref="SmRegulationViewModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
        <SmRegulationAuditModal
          ref="SmRegulationAuditModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
      </div>
    );
  },
};
