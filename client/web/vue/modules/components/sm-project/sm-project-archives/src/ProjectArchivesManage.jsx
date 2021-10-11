import '../style/index';
import { pagination as paginationConfig, tips as tipsConfig } from '../../../_utils/config';
import { requestIsSuccess, getSecurityTypeTitle, vIf, vP } from '../../../_utils/utils';
import SmExport from '../../../sm-common/sm-export-module';
import { SecurityType } from '../../../_utils/enum';
import SmProjectUploadModal from '../../sm-project-upload-modal/SmProjectUploadModal';
import FileSaver from 'file-saver';
import SmImport from '../../../sm-import/sm-import-basic';
import permissionsSmProject from '../../../_permissions/sm-project';
import SmProjectArchivesModal from '../SmProjectArchivesModal';
import ApiArchives from '../../../sm-api/sm-project/Archives';
import moment from 'moment';
let apiArchives = new ApiArchives();
export default {
  name: 'ProjectArchivesManage',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
    parentId: { type: String, default: null },
  },
  data() {
    return {
      totalCount: 0,
      pageIndex: 1,
      isCanExport: true, //导出项是否禁用
      queryParams: {
        name: null,
        maxResultCount: paginationConfig.defaultPageSize,
      },
      rowIndex: 2,
      iParentId: null, //父级记录id
      loading: false,
      dataSource: [], //数据源
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          width: 50,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '宗号',
          dataIndex: 'fileCode',
          width: 60,
          scopedSlots: { customRender: 'fileCode' },
          ellipsis: true,
        },
        {
          title: '档号',
          dataIndex: 'projectCode',
          ellipsis: true,
          width: 60,
          scopedSlots: { customRender: 'projectCode' },
        },
        {
          title: '案卷号',
          ellipsis: true,
          width: 80,
          dataIndex: 'archivesFilesCode  ',
          scopedSlots: { customRender: 'archivesFilesCode' },
        },
        {
          ellipsis: true,
          title: '案卷题名',
          dataIndex: 'name  ',
          width: 90,
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '编制日期',
          ellipsis: true,
          width: 90,
          dataIndex: 'date',
          scopedSlots: { customRender: 'date' },
        },
        {
          title: '年度',
          dataIndex: 'year',
          width: 60,
          ellipsis: true,
          scopedSlots: { customRender: 'year' },
        },
        {
          title: '页数',
          ellipsis: true,
          width: 60,
          dataIndex: 'page',
          scopedSlots: { customRender: 'page' },
        },
        {
          title: '份数',
          ellipsis: true,
          dataIndex: 'copies',
          width: 60,
          scopedSlots: { customRender: 'copies' },
        },
        {
          title: '案卷分类',
          width: 90,
          ellipsis: true,
          dataIndex: 'booksClassificationId',
          scopedSlots: { customRender: 'booksClassificationId' },
        },
        {
          title: '密级',
          width: 60,
          ellipsis: true,
          dataIndex: 'security',
          scopedSlots: { customRender: 'security' },
        },
        {
          title: '操作',
          width: 120,
          dataIndex: 'operations',
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  watch: {
    parentId: {
      handler: function(val, oldVal) {
        this.iParentId = val;
        this.queryParams.name = null;
        this.initAxios();
        this.refresh();
      },
      immediate: true,
    },
  },
  created() {},
  methods: {
    initAxios() {
      apiArchives = new ApiArchives(this.axios);
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
      let response = await apiArchives.getList({
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
    add() {
      this.$refs.SmProjectArchivesModal.add(this.iParentId);
    },
    edit(record) {
      this.$refs.SmProjectArchivesModal.edit(record, this.iParentId);
    },
    getSecurityColor(type) {
      let color = null;
      switch (type) {
      case SecurityType.secret:
        color = '#ff0000';
        break;
      case SecurityType.confidential:
        color = '#faad14';
        break;
      default:
        color = null;
      }
      return color;
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
            let response = await apiArchives.delete(record.id);
            if (requestIsSuccess(response)) {
              _this.record = record;
              _this.$message.success('操作成功');
              _this.refresh(false,  _this.pageIndex);
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() {},
      });
    },
    //文件导入
    async fileSelected(file) {
      // 构造导入参数（根据自己后台方法的实际参数进行构造）
      let importParamter = {
        'file.file': file,
        importKey: 'projectArchives',
        parentId: this.iParentId,
      };
      // 执行文件上传
      await this.$refs.smImport.exect(importParamter);
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
            let response = await apiArchives.export({ ...queryParams });
            _this.isLoading = false;
            if (requestIsSuccess(response)) {
              FileSaver.saveAs(
                new Blob([response.data], { type: 'text/plain;charset=utf-8' }),
                `案卷信息表.docx`,
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
    upload() {
      let importParamter = {
        importKey: 'projectArchives',
        parentId: this.iParentId,
        name: '案卷导入模板',
        url: '/api/app/projectArchives/upload',
      };
      this.$refs.SmProjectUploadModal.upload(importParamter);
    },
    addLock() {},
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
      <div class="sm-project-archives-manage">
        <a-card>
          <div class="archives-manage-button-input">
            <div class="archives-manage-button">
              {vIf(
                <a-button type="primary" onClick={() => this.add()} size="small" icon="plus">
                添加
                </a-button>,
                vP(this.permissions, permissionsSmProject.Archivess.Create),
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
                vP(this.permissions, permissionsSmProject.Archivess.Import),
              )}
              {/* <SmImport
                ref="smImport"
                url="url"
                axios={this.axios}
                url="/api/app/projectArchives/upload"
                size="small"
                defaultTitle="导入"
                importKey="projectArchives"
                downloadErrorFile={true}
                onSelected={(file) => {
                  this.fileSelected(file);
                }}
                onSuccess={() => this.refresh()}
              /> */}
              {vIf(
                <a-button
                  type="primary"
                  onClick={this.export}
                  size="small"
                  disabled={this.isCanExport}
                >
                  <a-icon type="download" /> 导出
                </a-button>,
                vP(this.permissions, permissionsSmProject.Archivess.Export),
              )}
            </div>
            <div class="archives-manage-input">
              <a-input-search
                placeholder={'请输入卷名查找'}
                value={this.queryParams.name}
                onChange={event => {
                  this.queryParams.name = event.target.value;
                  this.refresh();
                }}
                onSearch={event => {
                  this.refresh();
                }}
              />
            </div>
          </div>
          <a-table
            columns={this.columns}
            dataSource={this.dataSource}
            rowKey={record => record.id}
            bordered={this.bordered}
            size="middle"
            loading={this.loading}
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
                fileCode: (text, record, index) => {
                  let result = record ? record.fileCode : '';
                  return result ? (
                    <a-tooltip title={result} placement="topLeft">
                      {result}
                    </a-tooltip>
                  ) : (
                    ''
                  );
                },
                projectCode: (text, record, index) => {
                  let result = record ? record.projectCode : '';
                  return result ? (
                    <a-tooltip title={result} placement="topLeft">
                      {result}
                    </a-tooltip>
                  ) : (
                    ''
                  );
                },

                archivesFilesCode: (text, record, index) => {
                  let result = record ? record.archivesFilesCode : '';
                  return result ? (
                    <a-tooltip title={result} placement="topLeft">
                      {result}
                    </a-tooltip>
                  ) : (
                    ''
                  );
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
                year: (text, record, index) => {
                  let result = record ? record.year : '';
                  return result ? (
                    <a-tooltip title={result} placement="topLeft">
                      {result}
                    </a-tooltip>
                  ) : (
                    ''
                  );
                },
                copies: (text, record, index) => {
                  let result = record ? record.copies : '';
                  return result ? (
                    <a-tooltip title={result} placement="topLeft">
                      {result}
                    </a-tooltip>
                  ) : (
                    ''
                  );
                },
                booksClassificationId: (text, record, index) => {
                  let result =
                    record && record.booksClassification ? record.booksClassification.name : '';
                  return result ? (
                    <a-tooltip title={result} placement="topLeft">
                      {result}
                    </a-tooltip>
                  ) : (
                    ''
                  );
                },
                security: (text, record, index) => {
                  let result =
                    record && record.security ? getSecurityTypeTitle(record.security) : '';
                  return result ? (
                    <a-tooltip title={result} placement="topLeft">
                      <span style={{color:this.getSecurityColor(record && record.security?record.security:null)}}>{result}</span>
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
                        vP(this.permissions, permissionsSmProject.Archivess.Update),
                      )}
                      {vIf(
                        <a-divider type="vertical" />,
                        vP(this.permissions, permissionsSmProject.Archivess.Update) &&
                        vP(this.permissions, [permissionsSmProject.Archivess.Delete, permissionsSmProject.Archivess.Apply]),
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
                              vP(this.permissions, permissionsSmProject.Archivess.Delete),
                            )}
                            {vIf(
                              <a-menu-item>
                                <a
                                  onClick={() => {
                                    this.addLock(record);
                                  }}
                                >
                              权限
                                </a>
                              </a-menu-item>,
                              vP(this.permissions, permissionsSmProject.Archivess.Apply),
                            )}
                          </a-menu>
                        </a-dropdown>,
                        vP(this.permissions, [permissionsSmProject.Archivess.Delete, permissionsSmProject.Archivess.Apply]),
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
        <SmProjectArchivesModal
          ref="SmProjectArchivesModal"
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
      </div>
    );
  },
};
