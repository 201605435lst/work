import '../style/index';
import { pagination as paginationConfig, tips as tipsConfig } from '../../../_utils/config';
import { requestIsSuccess, getStoreHouseEnableOption, vIf, vP } from '../../../_utils/utils';
import permissionsSmResource from '../../../_permissions/sm-resource';
import SmProjectUploadModal from '../../sm-project-upload-modal/SmProjectUploadModal';
import SmExport from '../../../sm-common/sm-export-module';
import SmImport from '../../../sm-import/sm-import-basic';
import permissionsSmProject from '../../../_permissions/sm-project';
import FileSaver from 'file-saver';
import SmProjectDossierModal from '../SmProjectDossierModal';
import { fileDownload } from '../../../sm-file/sm-file-manage/src/common';
import ApiDossier from '../../../sm-api/sm-project/Dossier';
import SmProjectDossierCatrgotyTreeSelect from '../../sm-project-dossier-catrgoty-tree-select/SmProjectDossierCatrgotyTreeSelect';
import moment from 'moment';
let apiDossier = new ApiDossier();

export default {
  name: 'ProjectDossierManage',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
    parentId: { type: String, default: null },
  },
  data() {
    return {
      totalCount: 0,
      pageIndex: 1,
      iParentId: null, //父级记录id
      queryParams: {
        name: null,
        parentId: null,
        maxResultCount: paginationConfig.defaultPageSize,
      },
      loading: false,
      record: null,
      dataSource: [], //数据源
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          width: 60,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '文件编号',
          dataIndex: 'code',
          scopedSlots: { customRender: 'code' },
          ellipsis: true,
        },
        {
          title: '文件题名',
          dataIndex: 'name',
          ellipsis: true,
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '页号',
          ellipsis: true,
          width: 60,
          dataIndex: 'page  ',
          scopedSlots: { customRender: 'page' },
        },
        {
          ellipsis: true,
          title: '责任人',
          dataIndex: 'personName  ',
          scopedSlots: { customRender: 'personName' },
        },
        {
          title: '文件日期',
          ellipsis: true,
          dataIndex: 'date',
          scopedSlots: { customRender: 'date' },
        },
        {
          title: '文件分类',
          dataIndex: 'fileCategoryId',
          ellipsis: true,
          scopedSlots: { customRender: 'fileCategoryId' },
        },
        {
          title: '备注',
          ellipsis: true,
          dataIndex: 'remark',
          scopedSlots: { customRender: 'remark' },
        },
        {
          title: '操作',
          width: 130,
          dataIndex: 'operations',
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  watch: {
    parentId: {
      handler: function(val, oldVal) {
        this.queryParams.name = null;
        this.iParentId = val;
        this.initAxios();
        this.refresh();
      },
      immediate: true,
    },
  },
  created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiDossier = new ApiDossier(this.axios);
    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let data = {
        ...this.queryParams,
        parentId: this.iParentId,
      };
      let response = await apiDossier.getList({
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
      if (this.dataSource.length > 0) {
        this.isCanExport = false;
      } else {
        this.isCanExport = true;
      }
      this.loading = false;
    },
    loadFile(record) {
      //构造附件
      let _dossierRltFiles = [];
      if (record && record.dossierRltFiles.length > 0) {
        record.dossierRltFiles.map(item => {
          let file = item.file;
          if (file) {
            _dossierRltFiles.push({
              id: file.id,
              name: file.name,
              type: file.type,
              url: file.url,
            });
          }
        });
        let filse =
          _dossierRltFiles.length == 1 ? Object.assign(..._dossierRltFiles) : _dossierRltFiles;

        fileDownload(filse);
      } else {
        this.$message.warning('当前文件没有附件');
      }
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
            let response = await apiDossier.delete(record.id);
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
    //文件导入import_templates_projectDossier
    async fileSelected(file) {
      // 构造导入参数（根据自己后台方法的实际参数进行构造）
      let importParamter = {
        'file.file': file,
        importKey: 'projectDossier',
        parentId: this.iParentId,
      };
      // 执行文件上传
      await this.$refs.smImport.exect(importParamter);
    },
    add() {
      this.$refs.SmProjectDossierModal.add(this.iParentId);
    },
    view(record) {
      this.$refs.SmProjectDossierModal.view(record, this.iParentId);
    },
    edit(record) {
      this.$refs.SmProjectDossierModal.edit(record, this.iParentId);
    },
    upload() {
      let importParamter = {
        importKey: 'projectDossier',
        parentId: this.iParentId,
        name: '文件导入模板',
        url: '/api/app/projectDossier/upload',
      };
      this.$refs.SmProjectUploadModal.upload(importParamter);
    },
    //文件导出
    async export() {
      let _this = this;
      //导出按钮
      _this.isLoading = true;
      _this.$confirm({
        title: '确认导出',
        content: h => (
          <div style="color:red;">
            {!_this.queryParams.name
              ? '确定要导出全部数据吗？'
              : `确定要导出这 ${_this.totalCount} 条数据吗？`}
          </div>
        ),
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let queryParams = {};
            queryParams = {
              parentId: _this.iParentId,
              name: _this.queryParams.name,
            };
            let response = await apiDossier.export({ ...queryParams });
            _this.isLoading = false;
            if (requestIsSuccess(response)) {
              FileSaver.saveAs(
                new Blob([response.data], { type: 'text/plain;charset=utf-8' }),
                `文件信息表.docx`,
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
      <div class="sm-project-dossier-manage">
        <div class="dossier-manage-button-input">
          <div class="dossier-manage-button">
            {vIf(
              <a-button type="primary" onClick={() => this.add()} value="small" icon="plus">
              添加
              </a-button>,
              vP(this.permissions, permissionsSmProject.Dossiers.Create),
            )}
            {vIf(
              <a-button
                icon="import"
                type="primary"
                size="small"
                onClick={() => {
                  this.upload();
                }}
              >
              导入
              </a-button>,
              vP(this.permissions, permissionsSmProject.Dossiers.Import),
            )}
            {vIf(
              <a-button type="primary" onClick={this.export} size="small" disabled={this.isCanExport}>
                <a-icon type="download" /> 导出
              </a-button>,
              vP(this.permissions, permissionsSmProject.Dossiers.Export),
            )}
            {/* <SmExport
              ref="smExport"
              axios={this.axios}
              templateName="projectDossier"
              url="/api/app/projectDossier/export"
              size="small"
              defaultTitle="导出"
              rowIndex={this.rowIndex}
              downloadErrorFile={true}
              downloadFileName="案卷表"
              onSuccess={() => {}}
            /> */}
          </div>
          <div class="dossier-manage-input">
            <a-input-search
              placeholder={'请输入文件名'}
              value={this.queryParams.name}
              onChange={event => {
                this.queryParams.name = event.target.value;
                this.refresh();
              }}
              onSearch={event => {
                this.refresh();
              }}
            />
            {/* <a-input-group compact>
              <SmProjectDossierCatrgotyTreeSelect
                axios={this.axios}
                onChange={(value)=>{
                  this.iParentId=value;
                  this.refresh();
                }}
              />
              <a-input-search 
                placeholder={'请输入'}
                onChange={(event)=>{
                  this.queryParams.name=event.target.value;
                  this.refresh();
                }}
                onSearch={()=>{
                  this.refresh();
                }}
              />
            </a-input-group> */}
          </div>
        </div>

        <a-card>
          <a-table
            columns={this.columns}
            dataSource={this.dataSource}
            rowKey={record => record.id}
            bordered={this.bordered}
            loading={this.loading}
            size="middle"
            pagination={false}
            {...{
              scopedSlots: {
                index: (text, record, index) => {
                  let result = index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                  return (
                    <a-tooltip title={result} placement="topLeft">
                      {result}
                    </a-tooltip>
                  );
                },
                code: (text, record, index) => {
                  let result = record ? record.code : '';
                  let res = result ? (
                    <a-tooltip title={result} placement="topLeft">
                      <span class="code-style"
                        onClick={() => {
                          this.view(record);
                        }}
                      >
                        {result}
                      </span>
                    </a-tooltip>
                  ) : (
                    ''
                  );
                  return res;
                },
                name: (text, record, index) => {
                  let result = record ? record.name : '';
                  return result ? (
                    <a-tooltip title={result} placement="topLeft">
                      {result}
                    </a-tooltip>
                  ) : (
                    ''
                  );
                },

                page: (text, record, index) => {
                  let result = record ? record.page : '';
                  return result ? (
                    <a-tooltip title={result} placement="topLeft">
                      {result}
                    </a-tooltip>
                  ) : (
                    ''
                  );
                },

                personName: (text, record, index) => {
                  let result = record ? record.personName : '';
                  return result ? (
                    <a-tooltip title={result} placement="topLeft">
                      {result}
                    </a-tooltip>
                  ) : (
                    ''
                  );
                },
                date: (text, record, index) => {
                  let result = record ? moment(record.date).format('YYYY-MM-DD') : '';
                  return result ? (
                    <a-tooltip title={result} placement="topLeft">
                      {result}
                    </a-tooltip>
                  ) : (
                    ''
                  );
                },
                fileCategoryId: (text, record, index) => {
                  let result = record.fileCategoryId ? record.fileCategory.name : '';
                  return result ? (
                    <a-tooltip title={result} placement="topLeft">
                      {result}
                    </a-tooltip>
                  ) : (
                    ''
                  );
                },
                remark: (text, record, index) => {
                  let result = record ? record.remark : '';
                  return result ? (
                    <a-tooltip title={result} placement="topLeft">
                      {result}
                    </a-tooltip>
                  ) : (
                    ''
                  );
                },
                operations: (text, record) => {
                  return [
                    <span>
                      {vIf(
                        <a
                          onClick={() => {
                            this.edit(record);
                          }}
                        >
                        编辑
                        </a>,
                        vP(this.permissions, permissionsSmProject.Dossiers.Update),
                      )}
                      {vIf(
                        <a-divider type="vertical" />,
                        vP(this.permissions, permissionsSmProject.Dossiers.Update) &&
                        vP(this.permissions, [permissionsSmProject.Dossiers.Delete, permissionsSmProject.Dossiers.Download]),
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
                                    this.remove(record);
                                  }}
                                >
                              删除
                                </a>
                              </a-menu-item>,
                              vP(this.permissions, permissionsSmProject.Dossiers.Delete),
                            )}
                            {record && record.dossierRltFiles.length > 0 ? (
                              vIf(
                                <a-menu-item>
                                  <a
                                    onClick={() => {
                                      this.loadFile(record);
                                    }}
                                  >
                                下载附件
                                  </a>
                                </a-menu-item>,
                                vP(this.permissions, permissionsSmProject.Dossiers.Download),
                              )
                            ) : (
                              ''
                            )}
                          </a-menu>
                        </a-dropdown>,
                        vP(this.permissions, [permissionsSmProject.Dossiers.Delete, permissionsSmProject.Dossiers.Download]),
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
        </a-card>

        {/* 添加/编辑模板 */}
        <SmProjectDossierModal
          ref="SmProjectDossierModal"
          axios={this.axios}
          onSuccess={(action, data) => {
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
      </div>
    );
  },
};
