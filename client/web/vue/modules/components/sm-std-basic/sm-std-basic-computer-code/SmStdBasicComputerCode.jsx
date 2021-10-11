import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import { ComputerCodeType } from '../../_utils/enum';
import SmStdBasicComputerCodeModal from './SmStdBasicComputerCodeModal';
import SmStdBasicBasePriceModal from './SmStdBasicBasePriceModal';
import SmImport from '../../sm-import/sm-import-basic';
import SmExport from '../../sm-common/sm-export-module';
import SmImportBasePrice from '../../sm-import/sm-import-basic';
import SmExportBasePrice from '../../sm-common/sm-export-module';
import ApiComputerCode from '../../sm-api/sm-std-basic/ComputerCode';
import ApiBasePrice from '../../sm-api/sm-std-basic/BasePrice';
import { requestIsSuccess, vIf, vP, getComputerCodeTypeTitle } from '../../_utils/utils';
import SmTemplateDownload from '../../sm-common/sm-import-template-module';
import permissionsSmStdBasic from '../../_permissions/sm-std-basic';
let apiComputerCode = new ApiComputerCode();
let apiBasePrice = new ApiBasePrice();

export default {
  name: 'SmStdBasicComputerCode',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      computerCodes: [], //数据源
      pageIndex: 1,
      totalCount: 0,
      pageSize: paginationConfig.defaultPageSize,
      queryParams: {
        keyWord: null, //关键字
        maxResultCount: paginationConfig.defaultPageSize,
      },

      form: this.$form.createForm(this),
      selectedRowKeys: [],
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          // width: 80,
          dataIndex: 'index',
          scopedSlots: { customRender: 'index' },
          ellipsis: true,
        },
        {
          title: '名称及规格',
          width: 120,
          dataIndex: 'name',
          ellipsis: true,
        },
        {
          title: '电算代号',
          width: 100,
          dataIndex: 'code',
          ellipsis: true,
        },
        {
          title: '电算类型',
          width: 100,
          dataIndex: 'type',
          scopedSlots: { customRender: 'type' },
          ellipsis: true,
        },
        {
          title: '单位',
          // width: 80,
          dataIndex: 'unit',
          ellipsis: true,
        },
        {
          title: '单位重量',
          // width: 80,
          dataIndex: 'weight',
          ellipsis: true,
        },
        {
          title: '备注',
          // width: 100,
          dataIndex: 'remark',
          ellipsis: true,
        },
        {
          title: '操作',
          width: 120,
          dataIndex: 'operations',
          scopedSlots: { customRender: 'operations' },
          ellipsis: true,
        },
      ];
    },
    innerColumns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          width: 80,
          scopedSlots: { customRender: 'index' },
          ellipsis: true,
        },
        {
          title: '标准编号',
          width: 100,
          dataIndex: 'standardCodeName',
          scopedSlots: { customRender: 'standardCodeName' },
          ellipsis: true,
        },
        {
          title: '行政区域',
          width: 100,
          dataIndex: 'areaName',
          scopedSlots: { customRender: 'areaName' },
          ellipsis: true,
        },
        {
          title: '基期单价',
          width: 100,
          dataIndex: 'price',
          scopedSlots: { customRender: 'price' },
          ellipsis: true,
        },
        {
          title: '操作',
          width: 120,
          dataIndex: 'bpOperations',
          scopedSlots: { customRender: 'bpOperations' },
        },
      ];
    },
  },
  watch: {},
  created() {
    this.initAxios();
    this.refresh();
  },
  methods: {
    initAxios() {
      apiComputerCode = new ApiComputerCode(this.axios);
      apiBasePrice = new ApiBasePrice(this.axios);
    },

    add() {
      this.$refs.SmStdBasicComputerCodeModal.add();
    },

    detail(record) {
      this.$refs.SmStdBasicComputerCodeModal.detail(record);
    },

    edit(record) {
      this.$refs.SmStdBasicComputerCodeModal.edit(record);
    },
    getComputerCodeTypeTitle(record) {
      if (record.computerCodeType === 1) {
        return utils.getComputerCodeTypeTitle(ComputerCodeType.Artificial);
      } else if (record.computerCodeType === 2) {
        return utils.getComputerCodeTypeTitle(ComputerCodeType.Mechanics);
      } else if (record.computerCodeType === 3) {
        return utils.getComputerCodeTypeTitle(ComputerCodeType.Material);
      } else {
        return utils.getComputerCodeTypeTitle();
      }
    },

    remove(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            if (record.basePrices == null) {
              let response = await apiComputerCode.delete(record.id);
              if (requestIsSuccess(response)) {
                _this.refresh();
                _this.$message.success('删除成功');
                setTimeout(resolve, 100);
              } else setTimeout(reject, 100);
            } else {
              _this.$message.error('该电算代号下有基价数据，无法删除');
            }
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
      let response = await apiComputerCode.getList({
        isRltMaterial: false,
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(response)) {
        let _railways = response.data.items;
        _railways.map(item => {
          item.basePrices = item.basePrices;
        });
        this.computerCodes = _railways;
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

    // 展开事件
    async onExpandComputerCode(record) {
      let response = await apiBasePrice.getList({
        computerCodeId: record.id,
        isAll: true,
      });
      record.basePrices = response.data.items;
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
        importKey: 'computerCode',
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

    async importBasePrice(file, id) {
      // 构造导入参数（根据自己后台方法的实际参数进行构造）
      let importParamter = {
        'file.file': file,
        importKey: 'basePrice',
        id: id,
      };
      // 执行文件上传
      await this.$refs.smImportBasePrice.exect(importParamter);
    },

    //文件导出
    async exportBasePrice(para, id) {
      let queryParams = {};
      queryParams = { ...this.queryParams, computerCodeId: id };
      //执行文件下载
      await this.$refs.smExport.isCanDownload(para, queryParams);
    },

    addBasePrice(record) {
      this.$refs.SmStdBasicBasePriceModal.add(record);
    },

    detailBasePrice(record) {
      this.$refs.SmStdBasicBasePriceModal.detail(record);
    },

    editBasePrice(record) {
      this.$refs.SmStdBasicBasePriceModal.edit(record);
    },

    removeBasePrice(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiBasePrice.delete(record.id);
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
              placeholder="请输入名称或者代号"
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
                vP(this.permissions, permissionsSmStdBasic.ComputerCodes.Create),
              )}
              {vIf(
                <SmImport
                  ref="smImport"
                  url="api/app/stdBasicComputerCode/upload"
                  axios={this.axios}
                  downloadErrorFile={true}
                  importKey="computerCode"
                  onSelected={file => this.fileSelected(file)}
                  onIsSuccess={() => {
                    this.refresh();
                  }}
                />,
                vP(this.permissions, permissionsSmStdBasic.ComputerCodes.Import),
              )}
              {vIf(
                <SmTemplateDownload
                  axios={this.axios}
                  downloadKey="computerCode"
                  downloadFileName="电算代号"
                >
                </SmTemplateDownload>,
                vP(this.permissions, permissionsSmStdBasic.ComputerCodes.Import),
              )}
              {vIf(
                <SmExport
                  ref="smExport"
                  url="api/app/stdBasicComputerCode/export"
                  axios={this.axios}
                  templateName="computerCode"
                  downloadFileName="电算代号信息表"
                  rowIndex={5}
                  onDownload={para => this.downloadFile(para)}
                />,
                vP(this.permissions, permissionsSmStdBasic.ComputerCodes.Export),
              )}
            </div>
          </template>
        </sc-table-operator>
        <a-table
          columns={this.columns}
          dataSource={this.computerCodes}
          rowKey={record => record.id}
          pagination={false}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                let result = index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                return result;
              },
              type: (text, record, index) => {
                let result = getComputerCodeTypeTitle(record.type);
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
                      vP(this.permissions, permissionsSmStdBasic.ComputerCodes.Detail),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsSmStdBasic.ComputerCodes.Detail) &&
                        vP(this.permissions, [
                          permissionsSmStdBasic.ComputerCodes.Update,
                          permissionsSmStdBasic.ComputerCodes.Delete,
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
                            vP(this.permissions, permissionsSmStdBasic.ComputerCodes.Update),
                          )}
                          {vIf(
                            <a-menu-item>
                              <a onClick={() => this.remove(record)}>删除</a>
                            </a-menu-item>,
                            vP(this.permissions, permissionsSmStdBasic.ComputerCodes.Delete),
                          )}
                          {vIf(
                            <a-menu-item>
                              <a onClick={() => this.addBasePrice(record)}>添加基价</a>
                            </a-menu-item>,
                            vP(this.permissions, permissionsSmStdBasic.BasePrices.Create),
                          )}
                          {vIf(
                            <SmImportBasePrice
                              ref="smImportBasePrice"
                              url="api/app/stdBasicBasePrice/upload"
                              axios={this.axios}
                              downloadErrorFile={true}
                              importKey="basePrice"
                              defaultTitle={'导入基价'}
                              btnDefaultType={'link'}
                              // icon={'display:none'}

                              onSelected={file => this.importBasePrice(file, record.id)}
                              onIsSuccess={() => {
                                this.refresh();
                              }}
                            />,

                            vP(this.permissions, permissionsSmStdBasic.BasePrices.Import),
                          )}
                          {vIf(
                            // <a-menu-item>
                            //   <a onClick>导出基价</a>
                            // </a-menu-item>,
                            <SmExportBasePrice
                              ref="smExport"
                              url="api/app/stdBasicBasePrice/export"
                              axios={this.axios}
                              templateName="basePrice"
                              downloadFileName="基价信息表"
                              defaultTitle={'导出基价'}
                              // btnDefaultType={'link'}
                              // icon={'display:none'}
                              rowIndex={5}
                              onDownload={para => this.exportBasePrice(para, record.id)}
                            />,
                            vP(this.permissions, permissionsSmStdBasic.BasePrices.Export),
                          )}
                        </a-menu>
                      </a-dropdown>,
                      vP(this.permissions, [
                        permissionsSmStdBasic.ComputerCodes.Update,
                        permissionsSmStdBasic.ComputerCodes.Delete,
                      ]),
                    )}
                  </span>,
                ];
              },
              expandedRowRender: text => {
                return (
                  <div>
                    <a-table
                      rowKey={record => record.id}
                      slot-scope="text"
                      columns={this.innerColumns}
                      dataSource={text.basePrices}
                      pagination={false}
                      {...{
                        scopedSlots: {
                          index: (text, record, index) => {
                            return index + 1;
                          },
                          bpOperations: (text, record, index) => {
                            return [
                              <span>
                                {vIf(
                                  <a
                                    onClick={() => {
                                      this.detailBasePrice(record);
                                    }}
                                  >
                                    详情
                                  </a>,
                                  vP(this.permissions, permissionsSmStdBasic.BasePrices.Detail),
                                )}
                                {vIf(
                                  <a-divider type="vertical" />,
                                  vP(this.permissions, permissionsSmStdBasic.BasePrices.Detail) &&
                                    vP(this.permissions, [
                                      permissionsSmStdBasic.BasePrices.Update,
                                      permissionsSmStdBasic.BasePrices.Delete,
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
                                          <a onClick={() => this.editBasePrice(record)}>编辑</a>
                                        </a-menu-item>,
                                        vP(
                                          this.permissions,
                                          permissionsSmStdBasic.BasePrices.Update,
                                        ),
                                      )}
                                      {vIf(
                                        <a-menu-item>
                                          <a onClick={() => this.removeBasePrice(record)}>删除</a>
                                        </a-menu-item>,
                                        vP(
                                          this.permissions,
                                          permissionsSmStdBasic.BasePrices.Delete,
                                        ),
                                      )}
                                    </a-menu>
                                  </a-dropdown>,
                                  vP(this.permissions, [
                                    permissionsSmStdBasic.ComputerCodes.Update,
                                    permissionsSmStdBasic.ComputerCodes.Delete,
                                  ]),
                                )}
                              </span>,
                            ];
                          },
                        },
                      }}
                    ></a-table>
                  </div>
                );
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
        <SmStdBasicComputerCodeModal
          ref="SmStdBasicComputerCodeModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />

        <SmStdBasicBasePriceModal
          ref="SmStdBasicBasePriceModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
      </div>
    );
  },
};
