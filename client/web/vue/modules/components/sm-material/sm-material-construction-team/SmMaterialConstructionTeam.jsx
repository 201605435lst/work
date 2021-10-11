import './style';
import { requestIsSuccess, vP, vIf } from '../../_utils/utils';
import ApiConstructionTeam from '../../sm-api/sm-material/ConstructionTeam';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import permissionsConstructionTeam from '../../_permissions/sm-material';
import SmMaterialConstructionTeamModal from './SmMaterialConstructionTeamModal';
import SmProjectUploadModal from '../../sm-project/sm-project-upload-modal';
import SmMaterialConstructionSectionSelect from '../sm-material-construction-section-select/SmMaterialConstructionSectionSelect';
import SmExport from '../../sm-common/sm-export-module';

import moment from 'moment';
let apiConstructionTeam = new ApiConstructionTeam();

export default {
  name: 'SmMaterialConstructionTeam',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      dataSource: [],
      loading: false,
      totalCount: 0,
      record: null,
      isCanExport: false,
      pageIndex: 1,
      queryParams: {
        keyWords: null, //施工区段名称
        constructionSectionId: undefined,
        maxResultCount: paginationConfig.defaultPageSize,
      },
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          ellipsis: true,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '名称',
          dataIndex: 'name',
          ellipsis: true,
          scopedSlots: { customRender: 'name' },
        },

        {
          title: '施工地点',
          ellipsis: true,
          dataIndex: 'constructionSectionName',
          scopedSlots: { customRender: 'constructionSectionName' },
        },
        {
          title: '联系人',
          ellipsis: true,
          dataIndex: 'peopleName',
          scopedSlots: { customRender: 'peopleName' },
        },
        {
          title: '电话',
          dataIndex: 'phone',
          ellipsis: true,
          scopedSlots: { customRender: 'phone' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
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
      apiConstructionTeam = new ApiConstructionTeam(this.axios);
    },

    // 添加
    add() {
      this.$refs.SmMaterialConstructionTeamModal.add();
    },
    // 详情
    view(record) {
      this.$refs.SmMaterialConstructionTeamModal.view(record);
    },
    // 编辑
    edit(record) {
      this.$refs.SmMaterialConstructionTeamModal.edit(record);
    },
    //导入
    upload() {
      let importParamter = {
        importKey: 'constructionTeam',
        name: '工队管理导入模板',
        url: '/api/app/materialConstructionTeam/upload',
      };
      this.$refs.SmProjectUploadModal.upload(importParamter);
    },

    //文件导出
    async downloadFile(para) {
      let data = {
        ...this.queryParams,
      };
      //执行文件下载
      await this.$refs.smExport.isCanDownload(para, data);
    },
    // async export() {
    //   let _this = this;
    //   //导出按钮
    //   _this.isLoading = true;
    //   _this.$confirm({
    //     title: '确认导出',
    //     content: h => (
    //       <div style="color:red;">
    //         {!_this.queryParams.keyWords
    //           ? '确定要导出全部数据吗？'
    //           : `确定要导出这 ${_this.totalCount} 条数据吗？`}
    //       </div>
    //     ),
    //     okType: 'danger',
    //     onOk() {
    //       return new Promise(async (resolve, reject) => {
    //         let queryParams = {};
    //         queryParams = {
    //           parentId: _this.iParentId,
    //           name: _this.queryParams.name,
    //         };
    //         let response = await apiConstructionTeam.export({ ...queryParams });
    //         _this.isLoading = false;
    //         if (requestIsSuccess(response)) {
    //           FileSaver.saveAs(
    //             new Blob([response.data], { type: 'application/vnd.ms-excel' }),
    //             `施工队管理表.xlsx`,
    //           );
    //           setTimeout(resolve, 100);
    //         } else {
    //           setTimeout(reject, 100);
    //         }
    //       });
    //     },
    //     onCancel() {},
    //   });
    // },
    // 删除
    remove(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiConstructionTeam.delete(record.id);
            if (requestIsSuccess(response)) {
              _this.record = record;
              _this.$message.success('操作成功');
              _this.refresh(false, _this.pageIndex);
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() {},
      });
    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let data = {
        ...this.queryParams,
      };
      let response = await apiConstructionTeam.getList({
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
        if (this.totalCount == 0) {
          this.isCanExport = true;
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
  },
  render() {
    return (
      <div class="sm-material-construction-team">
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.keyWords = undefined;
            this.queryParams.constructionSectionId = undefined;
            this.refresh();
          }}
        >
          <a-form-item label="名称">
            <a-input
              placeholder="请输入名称\联系人"
              value={this.queryParams.keyWords}
              allowClear
              onChange={event => {
                this.queryParams.keyWords = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="施工地点">
            <SmMaterialConstructionSectionSelect
              axios={this.axios}
              placeholder="请选择施工地点"
              allowClear
              style="width:100%"
              value={this.queryParams.constructionSectionId}
              onChange={value => {
                this.queryParams.constructionSectionId = value;
                this.refresh();
              }}
            />
          </a-form-item>
          <template slot="buttons">
            {vIf(
              <a-button type="primary" onClick={() => this.add()}>
                添加
              </a-button>,
              vP(this.permissions, permissionsConstructionTeam.ConstructionTeams.Create),
            )}
            {vIf(
              <a-button
                icon="import"
                type="primary"
                onClick={() => {
                  this.upload();
                }}
              >
                导入
              </a-button>,
              vP(this.permissions, permissionsConstructionTeam.ConstructionTeams.Import),
            )}
            {vIf(
              <SmExport
                ref="smExport"
                url="api/app/materialConstructionTeam/export"
                defaultTitle="导出"
                axios={this.axios}
                templateName="constructionTeam"
                downloadFileName="施工队管理表"
                rowIndex={2}
                disabled={this.isCanExport}
                onDownload={para => this.downloadFile(para)}
              ></SmExport>,
              vP(this.permissions, permissionsConstructionTeam.ConstructionTeams.Export),
            )}
          </template>
        </sc-table-operator>
        <a-table
          columns={this.columns}
          rowKey={record => record.id}
          dataSource={this.dataSource}
          bordered={this.bordered}
          pagination={false}
          loading={this.loading}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              name: (text, record) => {
                let result = record ? record.name : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span> {result}</span>
                  </a-tooltip>
                );
              },
              constructionSectionName: (text, record) => {
                let result = record.constructionSection ? record.constructionSection.name : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              peopleName: (text, record) => {
                let result = record.peopleName ? record.peopleName : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              phone: (text, record) => {
                return (
                  <a-tooltip placement="topLeft" title={record.phone}>
                    <span>{record.phone}</span>
                  </a-tooltip>
                );
              },
              operations: (text, record) => {
                return [
                  <span>
                    {vIf(
                      <a
                        onClick={() => {
                          this.view(record);
                        }}
                      >
                        详情
                      </a>,
                      vP(this.permissions, permissionsConstructionTeam.ConstructionTeams.Detail),
                    )}

                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsConstructionTeam.ConstructionTeams.Detail) &&
                        vP(this.permissions, [
                          permissionsConstructionTeam.ConstructionTeams.Update,
                          permissionsConstructionTeam.ConstructionTeams.Delete,
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
                                  this.edit(record);
                                }}
                              >
                                编辑
                              </a>
                            </a-menu-item>,
                            vP(
                              this.permissions,
                              permissionsConstructionTeam.ConstructionTeams.Update,
                            ),
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
                            vP(
                              this.permissions,
                              permissionsConstructionTeam.ConstructionTeams.Delete,
                            ),
                          )}
                        </a-menu>
                      </a-dropdown>,
                      vP(this.permissions, [
                        permissionsConstructionTeam.ConstructionTeams.Create,
                        permissionsConstructionTeam.ConstructionTeams.Update,
                        permissionsConstructionTeam.ConstructionTeams.UpdateEnable,
                        permissionsConstructionTeam.ConstructionTeams.Delete,
                      ]),
                    )}
                  </span>,
                ];
              },
            },
          }}
        ></a-table>
        {/* 添加/编辑模板 */}
        <SmMaterialConstructionTeamModal
          ref="SmMaterialConstructionTeamModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
        {/* 文件上传模板 */}
        <SmProjectUploadModal
          ref="SmProjectUploadModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
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
      </div>
    );
  },
};
