import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmStdBasicProjecItemModal from './SmStdBasicProjecItemModal';
import SmStdBasicRltComponentModal from './SmStdBasicRltComponentModal';
import SmImport from '../../sm-import/sm-import-basic';
import SmExport from '../../sm-common/sm-export-module';
import ApiProjectItem from '../../sm-api/sm-std-basic/ProjectItem';
import { requestIsSuccess, vIf, vP } from '../../_utils/utils';
import permissionsSmStdBasic from '../../_permissions/sm-std-basic';
import SmTemplateDownload from '../../sm-common/sm-import-template-module';
let apiProjectItem = new ApiProjectItem();
export default {
  name: 'SmStdBasicProjectItem',
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
        keyWord: null, //关键字
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
          width: 260,
          dataIndex: 'name',
        },
        {
          title: '编码',
          width: 260,
          dataIndex: 'code',
        },
        {
          title: '备注',
          width: 260,
          dataIndex: 'remark',
        },
        {
          title: '操作',
          width: 140,
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
      apiProjectItem = new ApiProjectItem(this.axios);
    },

    add() {
      this.$refs.SmStdBasicProjecItemModal.add();
    },

    detail(record) {
      this.$refs.SmStdBasicProjecItemModal.detail(record);
    },

    edit(record) {
      this.$refs.SmStdBasicProjecItemModal.edit(record);
    },

    remove(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiProjectItem.delete(record.id);
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
      let response = await apiProjectItem.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
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

    onPageChange(page, pageSize) {
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
        importKey: 'ProjectItem',
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

    relevenceComponent(record) {
      this.$refs.SmStdBasicRltComponentModal.relevenceComponent(record);
    },

    relevenceProduct(record) {
      this.$refs.SmStdBasicRltComponentModal.relevenceProduct(record);
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
            this.queryParams.keyWord = null;
            this.refresh();
          }}
        >
          <a-form-item label="关键字">
            <a-input
              placeholder="请输入名称或者编码"
              allowClear
              value={this.queryParams.keyWord}
              onChange={event => {
                this.queryParams.keyWord = event.target.value;
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
                vP(this.permissions, permissionsSmStdBasic.ProjectItems.Create),
              )}
              {vIf(
                <SmImport
                  ref="smImport"
                  url="api/app/stdBasicProjectItem/upload"
                  axios={this.axios}
                  downloadErrorFile={true}
                  importKey="ProjectItem"
                  onSelected={file => this.fileSelected(file)}
                  onIsSuccess={() => {
                    this.refresh();
                  }}
                />,
                vP(this.permissions, permissionsSmStdBasic.ProjectItems.Import),
              )}
              {vIf(
                <SmTemplateDownload
                  axios={this.axios}
                  downloadKey="ProjectItem"
                  downloadFileName="工程工项"
                >
                </SmTemplateDownload>,
                vP(this.permissions, permissionsSmStdBasic.ProjectItems.Import),
              )}
              {vIf(
                <SmExport
                  ref="smExport"
                  url="api/app/stdBasicProjectItem/export"
                  axios={this.axios}
                  templateName="ProjectItem"
                  downloadFileName="工程工项信息表"
                  rowIndex={5}
                  onDownload={para => this.downloadFile(para)}
                />,
                vP(this.permissions, permissionsSmStdBasic.ProjectItems.Export),
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
                      vP(this.permissions, permissionsSmStdBasic.ProjectItems.Detail),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsSmStdBasic.ProjectItems.Detail) &&
                        vP(this.permissions, [
                          permissionsSmStdBasic.ProjectItems.Update,
                          permissionsSmStdBasic.ProjectItems.Delete,
                          //关联产品和构件
                          permissionsSmStdBasic.ProjectItems.Delete,
                          permissionsSmStdBasic.ProjectItems.Delete,
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
                              <a onClick={() => this.edit(record)}>编辑</a>
                            </a-menu-item>,
                            vP(this.permissions, permissionsSmStdBasic.ProjectItems.Update),
                          )}
                          {vIf(
                            <a-menu-item>
                              <a onClick={() => this.remove(record)}>删除</a>
                            </a-menu-item>,
                            vP(this.permissions, permissionsSmStdBasic.ProjectItems.Delete),
                          )}
                          {vIf(
                            <a-menu-item>
                              <a onClick={() => this.relevenceProduct(record)}>关联产品</a>
                            </a-menu-item>,
                            vP(this.permissions, permissionsSmStdBasic.ProjectItems.Delete),
                          )}
                          {vIf(
                            <a-menu-item>
                              <a onClick={() => this.relevenceComponent(record)}>关联构件</a>
                            </a-menu-item>,
                            vP(this.permissions, permissionsSmStdBasic.ProjectItems.Delete),
                          )}
                        </a-menu>
                      </a-dropdown>,
                      vP(this.permissions, [
                        permissionsSmStdBasic.ProjectItems.Update,
                        permissionsSmStdBasic.ProjectItems.Delete,
                        //关联产品和构件
                        permissionsSmStdBasic.ProjectItems.Delete,
                        permissionsSmStdBasic.ProjectItems.Delete,
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
        <SmStdBasicProjecItemModal
          ref="SmStdBasicProjecItemModal"
          axios={this.axios}
          onSuccess={() => this.refresh(false)}
        />

        <SmStdBasicRltComponentModal ref="SmStdBasicRltComponentModal" axios={this.axios} />
      </div>
    );
  },
};
