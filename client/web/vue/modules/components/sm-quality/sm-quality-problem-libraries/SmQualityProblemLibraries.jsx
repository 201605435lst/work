
import './style';
import { requestIsSuccess, vIf, vP, getQualityProblemType, getQualityProblemLevel } from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import ApiQualityProblemLibrary from '../../sm-api/sm-quality/QualityProblemLibrary';
import permissionsSmQuality from '../../_permissions/sm-quality';
import DataDictionary from '../../sm-system/sm-system-data-dictionary-tree-select';
import SmImport from '../../sm-import/sm-import-basic';
import SmTemplateDownload from '../../sm-common/sm-import-template-module';
import SmExport from '../../sm-common/sm-export-module';
import SmQualityProblemLibraryModal from './SmQualityProblemLibraryModal';
import DataEnum from './src/SmSystemDataEnumTreeSelect';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';

let apiQualityProblemLibrary = new ApiQualityProblemLibrary();

export default {
  name: 'SmQualityProblemLibraries',
  props: {
    permissions: { type: Array, default: () => [] },
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
  },
  data() {
    return {
      problemLibraries: [], // 列表数据源
      totalCount: 0,
      pageIndex: 1,
      pageSize: paginationConfig.defaultPageSize,
      queryParams: {
        keyword: '', // 模糊查询
        eventTypeId: null,//事件类型
        level: undefined,
        type: undefined,
        maxResultCount: paginationConfig.defaultPageSize,
      },
      isCanExport: false,
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          width: 120,
          ellipsis: true,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '工作内容',
          dataIndex: 'title',
          ellipsis: true,
          scopedSlots: { customRender: 'title' },
        },
        {
          title: '所属专业',
          ellipsis: true,
          dataIndex: 'profession',
          scopedSlots: { customRender: 'profession' },
        },
        {
          title: '问题类型',
          ellipsis: true,
          dataIndex: 'type',
          scopedSlots: { customRender: 'type' },
        },
        {
          title: '问题等级',
          ellipsis: true,
          dataIndex: 'level',
          scopedSlots: { customRender: 'level' },
        },
        {
          title: '问题描述',
          dataIndex: 'content',
          ellipsis: true,
          scopedSlots: { customRender: 'content' },
        },
        {
          title: '控制措施',
          dataIndex: 'measures',
          ellipsis: true,
          scopedSlots: { customRender: 'measures' },
        },
        {
          title: '适用范围',
          dataIndex: 'scops',
          scopedSlots: { customRender: 'scops' },
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
  async created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiQualityProblemLibrary = new ApiQualityProblemLibrary(this.axios);
    },

    //添加
    add() {
      this.$refs.SmQualityProblemLibraryModal.add();
    },

    //编辑
    edit(record) {
      this.$refs.SmQualityProblemLibraryModal.edit(record);
    },

    //详情
    view(record) {
      this.$refs.SmQualityProblemLibraryModal.view(record);
    },

    /* 文件导出 */
    async downloadFile(para) {
      let data = {
        ...this.queryParams,
      };
      console.log(para,data);
      //执行文件下载
      await this.$refs.smExport.isCanDownload(para, data);
    },

    // 删除
    remove(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content} </div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiQualityProblemLibrary.delete(record.id);
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
    async fileSelected(file) {
      // 构造导入参数（根据自己后台方法的实际参数进行构造）
      let importParamter = {
        'file.file': file,
        importKey: 'qualityproblemlibrary',
      };
      // 执行文件上传
      await this.$refs.smImport.exect(importParamter);
    },
    // 刷新列表
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }

      let response = await apiQualityProblemLibrary.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(response) && response.data) {
        this.problemLibraries = response.data.items;
        this.totalCount = response.data.totalCount;
        this.isCanExport = this.totalCount > 0 ? false : true;

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
      <div class="sm-quality-problem-libraries">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams = {
              keyword: '',
              level: '',
              type: '',
              eventTypeId: null,
            };
            this.refresh();
          }}
        >
          <a-form-item label="关键字">
            <a-input
              placeholder="请输入工作内容、问题描述、控制措施"
              value={this.queryParams.keyword}
              onInput={event => {
                this.queryParams.keyword = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>

          <a-form-item label="问题等级">
            <DataEnum
              disabled={this.status == ModalStatus.View}
              placeholder="请选择问题等级"
              enum="QualityProblemLevel"
              utils="getQualityProblemLevel"
              value={this.queryParams.level}
              onChange={value => {
                this.queryParams.level = value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="问题类型">
            <DataEnum
              disabled={this.status == ModalStatus.View}
              placeholder="请选择问题类型"
              enum="QualityProblemType"
              utils="getQualityProblemType"
              value={this.queryParams.type}
              onChange={value => {
                this.queryParams.type = value;
                this.refresh();
              }}
            />
          </a-form-item>
          <template slot="buttons">
            <div style="display:flex;">
              {vIf(
                <a-button type="primary" onClick={this.add} icon="plus">
                  新增
                </a-button>,
                vP(this.permissions, permissionsSmQuality.QualityProblemLibraries.Create),
              )}
              {
                vIf(
                  < SmImport
                    ref="smImport"
                    url='api/app/qualityProblemLibrary/upload'
                    axios={this.axios}
                    downloadErrorFile={true}
                    importKey="qualityproblemlibrary"
                    onSelected={file => this.fileSelected(file)}
                    onIsSuccess={() => this.refresh()}
                  />,
                  vP(this.permissions, permissionsSmQuality.QualityProblemLibraries.Import),
                )
              }
              {vIf(
                <SmTemplateDownload
                  axios={this.axios}
                  downloadKey="qualityproblemlibrary"
                  downloadFileName="质量问题库"
                >
                </SmTemplateDownload>,
                vP(this.permissions, permissionsSmQuality.QualityProblemLibraries.Import),
              )}
              {vIf(
                <SmExport
                  ref="smExport"
                  url="api/app/QualityProblemLibrary/export"
                  defaultTitle="导出"
                  axios={this.axios}
                  templateName="qualityproblemlibrary"
                  downloadFileName="质量问题库"
                  rowIndex={2}
                  disabled={this.isCanExport}
                  onDownload={para => this.downloadFile(para)}
                ></SmExport>,
                vP(this.permissions, permissionsSmQuality.QualityProblemLibraries.Export),
              )}
            </div>

          </template>
        </sc-table-operator>

        {/* 展示区 */}
        <a-table
          columns={this.columns}
          dataSource={this.problemLibraries}
          rowKey={record => record.id}
          bordered={this.bordered}
          loading={this.loading}

          pagination={false}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              title: (text, record, index) => {
                let result = record ? record.title : null;
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              profession: (text, record, index) => {
                let profession = record.profession ? record.profession.name : '';
                return (
                  <a-tooltip placement="topLeft" title={profession}>
                    <span>{profession}</span>
                  </a-tooltip>
                );
              },
              level: (text, record, index) => {
                let level = record.level ? getQualityProblemLevel(record.level) : '';
                return (
                  <a-tooltip placement="topLeft" title={level}>
                    <span>{level}</span>
                  </a-tooltip>
                );
              },
              type: (text, record, index) => {
                let type = record.type ? getQualityProblemType(record.type): '';
                return (
                  <a-tooltip placement="topLeft" title={type}>
                    <span>{type}</span>
                  </a-tooltip>
                );
              },
              eventType: (text, record, index) => {
                return (
                  <a-tooltip placement="topLeft" title={record.eventType ? record.eventType.name : ''}>
                    <span>{record.eventType ? record.eventType.name : ''}</span>
                  </a-tooltip>
                );
              },
              content: (text, record, index) => {
                let result = record ? record.content : null;
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              measures: (text, record, index) => {
                let result = record ? record.measures : null;
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              scops: (text, record, index) => {
                let result = '';
                record.scops.map(
                  (item, index) => (result += `${index == 0 ? '' : '，'}${item && item.scop ? item.scop.name : ''}`),
                );
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
                          this.view(record);
                        }}
                      >详情
                      </a>,
                      vP(this.permissions, permissionsSmQuality.QualityProblemLibraries.Detail),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsSmQuality.QualityProblemLibraries.Detail) &&
                      vP(this.permissions, [permissionsSmQuality.QualityProblemLibraries.Update, permissionsSmQuality.QualityProblemLibraries.Delete]),
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
                            vP(this.permissions, permissionsSmQuality.QualityProblemLibraries.Update),
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
                            vP(this.permissions, permissionsSmQuality.QualityProblemLibraries.Delete),
                          )}
                        </a-menu>
                      </a-dropdown>,
                      vP(this.permissions, [permissionsSmQuality.QualityProblemLibraries.Update, permissionsSmQuality.QualityProblemLibraries.Delete]),
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
        <SmQualityProblemLibraryModal
          ref="SmQualityProblemLibraryModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
      </div>
    );
  },
};