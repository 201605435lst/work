import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmStdBasicMvdPropertyModal from './SmStdBasicMvdPropertyModal';
import SmImport from '../../sm-import/sm-import-basic';
import SmExport from '../../sm-common/sm-export-module';
import ApiMVDProperty from '../../sm-api/sm-std-basic/MVDProperty';
import SmStdBasicMvdPropertyTree from './SmStdBasicMvdPropertyTree';
import { requestIsSuccess, vIf, vP } from '../../_utils/utils';
import permissionsSmStdBasic from '../../_permissions/sm-std-basic';
import SmTemplateDownload from '../../sm-common/sm-import-template-module';
let apiMVDProperty = new ApiMVDProperty();
export default {
  name: 'SmStdBasicMvdProperty',
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
        mvdCategoryId: null,
        maxResultCount: paginationConfig.defaultPageSize,
      },
      show: false,
      MVDCategory: null,
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          width: 80,
          scopedSlots: { customRender: 'index' },
          ellipsis: true,
        },
        {
          title: '名称',
          dataIndex: 'name',
          scopedSlots: { customRender: 'name' },
          ellipsis: true,
        },
        // {
        //   title: '参数类型',
        //   dataIndex: 'dataType',
        //   scopedSlots: { customRender: 'dataType' },
        //   ellipsis: true,
        // },
        {
          title: '单位',
          dataIndex: 'unit',
          scopedSlots: { customRender: 'unit' },
        },
        {
          title: '是否实参',
          dataIndex: 'isInstance',
          scopedSlots: { customRender: 'isInstance' },
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
          dataIndex: 'operations',
          width: 150,
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  async mounted() {
    await this.initAxios();
  },
  methods: {
    initAxios() {
      apiMVDProperty = new ApiMVDProperty(this.axios);
      this.$refs.SmStdBasicMvdPropertyTree.refresh();
    },

    add() {
      this.$refs.SmStdBasicMvdPropertyModal.add();
    },

    detail(record) {
      this.$refs.SmStdBasicMvdPropertyModal.detail(record);
    },

    edit(record) {
      this.$refs.SmStdBasicMvdPropertyModal.edit(record);
    },

    async remove(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiMVDProperty.delete(record.id);
            if (requestIsSuccess(response)) {
              _this.refresh();
              _this.$message.success('删除成功');
              setTimeout(resolve, 100);
            } else setTimeout(reject, 100);
          });
        },
        oncancel() { },
      });
    },

    //刷新
    async refresh(resetPage = true, page) {
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let data = {
        ...this.queryParams,
      };
      let response = await apiMVDProperty.getList({
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
        importKey: 'MVDProperty',
        mvdCategoryId: this.MVDCategory.id,
      };
      // 执行文件上传
      await this.$refs.smImport.exect(importParamter);
    },

    //文件导出
    async downloadFile(para) {
      let queryParams = {};
      queryParams = { ...this.queryParams, mvdCategoryId: this.MVDCategory.id };
      //执行文件下载
      await this.$refs.smExport.isCanDownload(para, queryParams);
    },

    getMVDPropertyTypeEnum(status) {
      let title = '';
      switch (status) {
      case 1:
        title = '尺寸';
        break;
      case 2:
        title = '数值';
        break;
      case 3:
        title = '文本';
        break;
      default:
        title = '未定义';
      }
      return title;
    },
  },
  render() {
    return (
      <div class="sm-std-basic-mvd-property ">
        {/* 左侧 */}
        <div class="std-left">
          <SmStdBasicMvdPropertyTree
            ref="SmStdBasicMvdPropertyTree"
            axios={this.axios}
            onRecord={item => {
              this.MVDCategory = item;
              this.queryParams.mvdCategoryId = item.id;
              this.refresh();
            }}
          />
        </div>
        {this.MVDCategory ? (
          <div class="std-right">
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
                  placeholder="请输入属性名称"
                  allowClear
                  value={this.queryParams.keyWords}
                  width={280}
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
                    vP(this.permissions, permissionsSmStdBasic.MVDProperty.Create),
                  )}
                  {vIf(
                    <SmImport
                      ref="smImport"
                      url="api/app/stdBasicMVDProperty/import"
                      axios={this.axios}
                      downloadErrorFile={true}
                      importKey="MVDProperty"
                      onSelected={file => this.fileSelected(file)}
                      onIsSuccess={() => {
                        this.refresh();
                      }}
                    />,
                    vP(this.permissions, permissionsSmStdBasic.MVDProperty.Import),
                  )}
                  {vIf(
                    <SmTemplateDownload
                      axios={this.axios}
                      downloadKey="MVDProperty"
                      downloadFileName="信息交换模板属性"
                    ></SmTemplateDownload>,
                    vP(this.permissions, permissionsSmStdBasic.MVDProperty.Import),
                  )}
                  {vIf(
                    <SmExport
                      ref="smExport"
                      url="api/app/stdBasicMVDProperty/export"
                      axios={this.axios}
                      templateName="MVDProperty"
                      downloadFileName={`${this.MVDCategory.name}分类下的属性表`}
                      rowIndex={5}
                      onDownload={para => this.downloadFile(para)}
                    />,
                    vP(this.permissions, permissionsSmStdBasic.MVDProperty.Export),
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
                  // dataType: (text, record) => {
                  //   return this.getMVDPropertyTypeEnum(record.dataType);
                  // },
                  isInstance: (text, record) => {
                    return text ? "是" : "否";
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
                          vP(this.permissions, permissionsSmStdBasic.MVDProperty.Detail),
                        )}
                        {vIf(
                          <a-divider type="vertical" />,
                          vP(this.permissions, permissionsSmStdBasic.MVDProperty.Detail) &&
                          vP(this.permissions, [
                            permissionsSmStdBasic.MVDProperty.Update,
                            permissionsSmStdBasic.MVDProperty.Delete,
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
                                vP(this.permissions, permissionsSmStdBasic.MVDProperty.Update),
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
                                vP(this.permissions, permissionsSmStdBasic.MVDProperty.Delete),
                              )}
                            </a-menu>
                          </a-dropdown>,
                          vP(this.permissions, [
                            permissionsSmStdBasic.MVDProperty.Update,
                            permissionsSmStdBasic.MVDProperty.Delete,
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
            <SmStdBasicMvdPropertyModal
              ref="SmStdBasicMvdPropertyModal"
              axios={this.axios}
              MVDCategory={this.MVDCategory}
              onSuccess={() => {
                this.refresh(true);
              }}
            />
          </div>
        ) : (
          undefined
        )
        }
      </div>
    );
  },
};
