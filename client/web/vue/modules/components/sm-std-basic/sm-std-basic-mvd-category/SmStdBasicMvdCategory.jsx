import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmStdBasicMvdCategoryModal from './SmStdBasicMvdCategoryModal';
import SmImport from '../../sm-import/sm-import-basic';
import SmExport from '../../sm-common/sm-export-module';
import ApiMVDCategory from '../../sm-api/sm-std-basic/MVDCategory';
import { requestIsSuccess, vIf, vP } from '../../_utils/utils';
import permissionsSmStdBasic from '../../_permissions/sm-std-basic';
import SmTemplateDownload from '../../sm-common/sm-import-template-module';
let apiMVDCategory = new ApiMVDCategory();
export default {
  name: 'SmStdBasicMvdCategory',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      dataSource: [], //数据源
      pageIndex: 1,
      totalCount: 0,
      pageSize: paginationConfig.defaultPageSize,
      queryParams: {
        keyWords: null, //关键字
        maxResultCount: paginationConfig.defaultPageSize,
      },
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          width: 120,
          dataIndex: 'index',
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '名称',
          width: 230,
          dataIndex: 'name',
          scopedSlots: { customRender: 'name' },
          ellipsis: true,
        },
        {
          title: '代号',
          // width: 230,
          dataIndex: 'code',
          scopedSlots: { customRender: 'code' },
          ellipsis: true,
        },
        {
          title: '排序',
          dataIndex: 'order',
          scopedSlots: { customRender: 'order' },
          ellipsis: true,
        },
        {
          title: '备注',
          dataIndex: 'remark',
          scopedSlots: { customRender: 'remark' },
          ellipsis: true,
        },
        {
          title: '操作',
          width: 150,
          dataIndex: 'operations',
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiMVDCategory = new ApiMVDCategory(this.axios);
    },

    add() {
      this.$refs.SmStdBasicMvdCategoryModal.add();
    },

    detail(record) {
      this.$refs.SmStdBasicMvdCategoryModal.detail(record);
    },

    edit(record) {
      this.$refs.SmStdBasicMvdCategoryModal.edit(record);
    },

    async remove(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiMVDCategory.delete(record.id);
            if (requestIsSuccess(response)) {
              _this.refresh();
              _this.$message.success('删除成功');
              setTimeout(resolve, 100);
            } else setTimeout(reject, 100);
          });
        },
        oncancel() {},
      });
    },

    //刷新
    async refresh(resetPage = true, page) {
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let response = await apiMVDCategory.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        isAll: false,
        ...this.queryParams,
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
    },

    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },
    async fileSelected(file) {
      // 构造导入参数（根据自己后台方法的实际参数进行构造）
      let importParamter = {
        'file.file': file,
        importKey: 'MVDCategory',
      };
      // 执行文件上传
      await this.$refs.smImport.exect(importParamter);
    },

    //文件导出
    async downloadFile(para) {
      let queryParams = {};
      queryParams = { ...this.queryParams };
      //执行文件下载
      await this.$refs.smExport.isCanDownload(para, queryParams);
    },
  },
  render() {
    return (
      <div>
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
              placeholder="请输入分类名称或者代号"
              allowClear
              value={this.queryParams.keyWords}
              onChange={event => {
                this.queryParams.keyWords = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          <template slot="buttons">
            <div style={'display:flex'}>
              {vIf(
                <a-button type="primary" icon="plus" onClick={() => this.add()}>
                  添加
                </a-button>,
                vP(this.permissions, permissionsSmStdBasic.MVDCategory.Create),
              )}
              {vIf(
                <SmImport
                  ref="smImport"
                  url="api/app/stdBasicMVDCategory/import"
                  axios={this.axios}
                  downloadErrorFile={true}
                  importKey="MVDCategory"
                  onSelected={file => this.fileSelected(file)}
                  onIsSuccess={() => {
                    this.refresh();
                  }}
                />,
                vP(this.permissions, permissionsSmStdBasic.MVDCategory.Import),
              )}
              {vIf(
                <SmTemplateDownload
                  axios={this.axios}
                  downloadKey="MVDCategory"
                  downloadFileName="信息交换模板分类"
                >
                </SmTemplateDownload>,
                vP(this.permissions, permissionsSmStdBasic.MVDCategory.Import),
              )}
              {vIf(
                <SmExport
                  ref="smExport"
                  url="api/app/stdBasicMVDCategory/export"
                  axios={this.axios}
                  templateName="MVDCategory"
                  downloadFileName="信息交换模板分类表"
                  rowIndex={5}
                  onDownload={para => this.downloadFile(para)}
                />,
                vP(this.permissions, permissionsSmStdBasic.MVDCategory.Export),
              )}
            </div>
          </template>
        </sc-table-operator>
        <a-table
          columns={this.columns}
          dataSource={this.dataSource}
          rowKey={record => record.id}
          pagination={false}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                let result = index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                return result;
              },
              operations: (text, record, index) => {
                return [
                  <span>
                    {vIf(
                      <a
                        onClick={() => {
                          this.detail(record);
                        }}
                      >
                        详情
                      </a>,
                      vP(this.permissions, permissionsSmStdBasic.MVDCategory.Detail),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsSmStdBasic.MVDCategory.Detail) &&
                        vP(this.permissions, [
                          permissionsSmStdBasic.MVDCategory.Update,
                          permissionsSmStdBasic.MVDCategory.Delete,
                        ]),
                    )}
                    {vIf(
                      <a-dropdown trigger={['click']}>
                        <a>
                          更多
                          <a-icon type="down" />
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
                            vP(this.permissions, permissionsSmStdBasic.MVDCategory.Update),
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
                            vP(this.permissions, permissionsSmStdBasic.MVDCategory.Delete),
                          )}
                        </a-menu>
                      </a-dropdown>,
                      vP(this.permissions, [
                        permissionsSmStdBasic.MVDCategory.Update,
                        permissionsSmStdBasic.MVDCategory.Delete,
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
        <SmStdBasicMvdCategoryModal
          ref="SmStdBasicMvdCategoryModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
      </div>
    );
  },
};
