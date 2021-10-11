
import './style';
import { requestIsSuccess, vIf, vP, getSafetyRiskLevel } from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import ApiSafeProblemLibrary from '../../sm-api/sm-safe/ProblemLibrary';
import permissionsSmSafe from '../../_permissions/sm-safe';
import DataDictionary from '../../sm-system/sm-system-data-dictionary-tree-select';
import SmImport from '../../sm-import/sm-import-basic';
import SmTemplateDownload from '../../sm-common/sm-import-template-module';
import SmExport from '../../sm-common/sm-export-module';
import SmSafeProblemLibraryModal from './SmSafeProblemLibraryModal';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import DataEnum from './src/SmSystemDataEnumTreeSelect';
let apiSafeProblemLibrary = new ApiSafeProblemLibrary();

export default {
  name: 'SmSafeProblemLibrary',
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
        riskLevel: undefined,//风险等级
        eventTypeId: null,//事件类型
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
          title: '风险等级',
          dataIndex: 'riskLevel',
          ellipsis: true,
          scopedSlots: { customRender: 'riskLevel' },
        },
        {
          title: '所属专业',
          dataIndex: 'profession',
          ellipsis: true,
          scopedSlots: { customRender: 'profession' },
        },
        {
          title: '风险因素',
          dataIndex: 'content',
          ellipsis: true,
          scopedSlots: { customRender: 'content' },
        },
        {
          title: '事件类型',
          ellipsis: true,
          dataIndex: 'eventType',
          scopedSlots: { customRender: 'eventType' },
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
      apiSafeProblemLibrary = new ApiSafeProblemLibrary(this.axios);
    },

    //添加
    add() {
      this.$refs.SmSafeProblemLibraryModal.add();
    },

    //编辑
    edit(record) {
      this.$refs.SmSafeProblemLibraryModal.edit(record);
    },

    //详情
    view(record) {
      this.$refs.SmSafeProblemLibraryModal.view(record);
    },

    /* 文件导出 */
    async downloadFile(para) {
      let data = {
        ...this.queryParams,
      };
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
            let response = await apiSafeProblemLibrary.delete(record.id);
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
        importKey: 'safeproblemlibrary',
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

      let response = await apiSafeProblemLibrary.getList({
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
      <div class="sm-Safe-problem-libraries">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams = {
              keyword: '',
              riskLevel: '',
              eventTypeId: null,
            };
            this.refresh();
          }}
        >
          <a-form-item label="关键字">
            <a-input
              placeholder="请输入工作内容、风险因素、控制措施"
              value={this.queryParams.keyword}
              onInput={event => {
                this.queryParams.keyword = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>

          <a-form-item label="事件类型">
            <DataDictionary
              placeholder="请选择事件类型"
              axios={this.axios}
              groupCode="SafeManager.EventType"
              value={this.queryParams.eventTypeId}
              onInput={value => {
                this.queryParams.eventTypeId = value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="风险等级" class="sm-quality-problem-type">
            <DataEnum
              disabled={this.status == ModalStatus.View}
              placeholder="请选择风险等级"
              enum="SafetyRiskLevel"
              utils="getSafetyRiskLevel"
              value={this.queryParams.riskLevel}
              onChange={value => {
                this.queryParams.riskLevel = value;
                this.refresh();
              }}
            />
          </a-form-item>
          <template slot="buttons">
            <div style="display:flex;">
              {vIf(
                <a-button type="primary" onClick={this.add} icon="plus">
                  新建
                </a-button>,
                vP(this.permissions, permissionsSmSafe.SafeProblemLibrarys.Create),
              )}
              {
                vIf(
                  < SmImport
                    ref="smImport"
                    url='api/app/safeProblemLibrary/upload'
                    axios={this.axios}
                    downloadErrorFile={true}
                    importKey="safeproblemlibrary"
                    onSelected={file => this.fileSelected(file)}
                    onIsSuccess={() => this.refresh()}
                  />,
                  vP(this.permissions, permissionsSmSafe.SafeProblemLibrarys.Import),
                )
              }
              {vIf(
                <SmTemplateDownload
                  axios={this.axios}
                  downloadKey="safeproblemlibrary"
                  downloadFileName="安全问题库"
                >
                </SmTemplateDownload>,
                vP(this.permissions, permissionsSmSafe.SafeProblemLibrarys.Import),
              )}
              {vIf(
                <SmExport
                  ref="smExport"
                  url="api/app/SafeProblemLibrary/export"
                  defaultTitle="导出"
                  axios={this.axios}
                  templateName="safeproblemlibrary"
                  downloadFileName="安全问题库"
                  rowIndex={2}
                  disabled={this.isCanExport}
                  onDownload={para => this.downloadFile(para)}
                ></SmExport>,
                vP(this.permissions, permissionsSmSafe.SafeProblemLibrarys.Export),
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
              riskLevel: (text, record, index) => {
                let riskLevel = record.riskLevel ? getSafetyRiskLevel(record.riskLevel) : '';
                return (
                  <a-tooltip placement="topLeft" title={riskLevel}>
                    <span>{riskLevel}</span>
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
                      vP(this.permissions, permissionsSmSafe.SafeProblemLibrarys.Detail),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsSmSafe.SafeProblemLibrarys.Detail) &&
                      vP(this.permissions, [permissionsSmSafe.SafeProblemLibrarys.Update, permissionsSmSafe.SafeProblemLibrarys.Delete]),
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
                            vP(this.permissions, permissionsSmSafe.SafeProblemLibrarys.Update),
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
                            vP(this.permissions, permissionsSmSafe.SafeProblemLibrarys.Delete),
                          )}
                        </a-menu>
                      </a-dropdown>,
                      vP(this.permissions, [permissionsSmSafe.SafeProblemLibrarys.Update, permissionsSmSafe.SafeProblemLibrarys.Delete]),
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
        <SmSafeProblemLibraryModal
          ref="SmSafeProblemLibraryModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
      </div>
    );
  },
};