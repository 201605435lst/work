import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import SmStdBasicQuotaModal from './SmStdBasicQuotaModal';
import SmStdBasicQuotaItemModal from './SmStdBasicQuotaItemModal';
import SmImport from '../../sm-import/sm-import-basic';
import SmExport from '../../sm-common/sm-export-module';
import SmImportQuotaItem from '../../sm-import/sm-import-basic';
import SmExportQuotaItem from '../../sm-common/sm-export-module';
import ApiQuota from '../../sm-api/sm-std-basic/Quota';
import ApiQuotaItem from '../../sm-api/sm-std-basic/QuotaItem';
import { requestIsSuccess, vIf, vP } from '../../_utils/utils';
import permissionsSmStdBasic from '../../_permissions/sm-std-basic';
import SmTemplateDownload from '../../sm-common/sm-import-template-module';
let apiQuota = new ApiQuota();
let apiQuotaItem = new ApiQuotaItem();

export default {
  name: 'SmStdBasicQuota',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      iSendingWorkId: null,
      quotas: [], //数据源
      selectQuota: null,
      quotaItemEditList: [], //清单值
      pageIndex: 1,
      totalCount: 0,
      pageSize: paginationConfig.defaultPageSize,
      queryParams: {
        keyWord: null, //关键字
        maxResultCount: paginationConfig.defaultPageSize,
      },

      form: this.$form.createForm(this),
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
          title: '定额名称',
          width: 120,
          dataIndex: 'name',
          ellipsis: true,
        },
        {
          title: '定额编号',
          width: 100,
          dataIndex: 'code',
          ellipsis: true,
        },
        {
          title: '定额分类',
          // width: 100,
          dataIndex: 'quotaCategoryName',
          ellipsis: true,
        },
        {
          title: '专业',
          // width: 80,
          dataIndex: 'specialtyName',
          ellipsis: true,
        },
        {
          title: '标准编号',
          // width: 100,
          dataIndex: 'standardCodeName',
          ellipsis: true,
        },
        {
          title: '行政区域',
          // width: 100,
          dataIndex: 'areaName',
          ellipsis: true,
        },
        {
          title: '人工费',
          // width: 80,
          dataIndex: 'laborCost',
          ellipsis: true,
        },
        {
          title: '材料费',
          // width: 80,
          dataIndex: 'materialCost',
          ellipsis: true,
        },
        {
          title: '机械使用费',
          // width: 120,
          dataIndex: 'machineCost',
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
          dataIndex: 'comIndex',
          width: '10%',
          ellipsis: true,
          customRender: (text, row, index) => {
            return {
              children: row.device.comIndex,
              attrs: { rowSpan: row.device.rowSpan },
            };
          },
        },
        {
          title: '电算代号',
          width: '20%',
          dataIndex: 'computerCode',
          ellipsis: true,
          customRender: (text, row, index) => {
            return {
              children: row.device.computerCode,
              attrs: { rowSpan: row.device.rowSpan },
            };
          },
        },
        {
          title: '电算代号名称',
          width: '20%',
          dataIndex: 'computerCodeName',
          ellipsis: true,
          customRender: (text, row, index) => {
            return {
              children: row.device.computerCodeName,
              attrs: { rowSpan: row.device.rowSpan },
            };
          },
        },
        {
          title: '数量',
          width: '10%',
          dataIndex: 'number',
          ellipsis: true,
          customRender: (text, row, index) => {
            return {
              children: row.device.number,
              attrs: { rowSpan: row.device.rowSpan },
            };
          },
        },
        {
          title: '标准编号',
          width: '10%',
          dataIndex: 'standardCodeName',
        },
        {
          title: '行政区域',
          width: '10%',
          dataIndex: 'areaName',
        },
        {
          title: '基期单价',
          width: '10%',
          dataIndex: 'price',
        },
        {
          title: '操作',
          width: '20%',
          dataIndex: 'bpOperations',
          ellipsis: true,
          customRender: (text, row, index) => {
            let operation = (
              <span>
                {vIf(
                  <a
                    onClick={() => {
                      this.detailQuotaItem(row);
                    }}
                  >
                    详情
                  </a>,
                  vP(this.permissions, permissionsSmStdBasic.QuotaItems.Detail),
                )}
                {vIf(
                  <a-divider type="vertical" />,
                  vP(this.permissions, permissionsSmStdBasic.QuotaItems.Detail) &&
                    vP(this.permissions, [
                      permissionsSmStdBasic.QuotaItems.Update,
                      permissionsSmStdBasic.QuotaItems.Delete,
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
                          <a onClick={() => this.editQuotaItem(row)}>编辑</a>
                        </a-menu-item>,
                        vP(this.permissions, permissionsSmStdBasic.QuotaItems.Update),
                      )}
                      {vIf(
                        <a-menu-item>
                          <a onClick={() => this.removeQuotaItem(row)}>删除</a>
                        </a-menu-item>,
                        vP(this.permissions, permissionsSmStdBasic.QuotaItems.Delete),
                      )}
                    </a-menu>
                  </a-dropdown>,
                  vP(this.permissions, [
                    permissionsSmStdBasic.QuotaItems.Update,
                    permissionsSmStdBasic.QuotaItems.Delete,
                  ]),
                )}
              </span>
            );
            return {
              children: operation,

              attrs: { rowSpan: row.device.rowSpan },
            };
          },
        },
      ];
      return arr;
    },
  },

  watch: {
    immediate: true,
  },
  created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    initAxios() {
      apiQuota = new ApiQuota(this.axios);
      apiQuotaItem = new ApiQuotaItem(this.axios);
      this.refresh();
    },
    // 初始化对象
    initPlanDetailTemplate(device) {
      return {
        device: device || {},
      };
    },

    //合并单元格
    mergeBasePrice(quota) {
      this.quotaItemEditList = [];
      quota.quotaItems.map((item, index) => {
        item.quotaItemEditList.some((item2, index2) => {
          let device = {
            computerCodeName: item.computerCodeName,
            computerCode: item.computerCode,
            number: item.number,
            comIndex: index + 1,
            quotaId: quota.id,
            computerCodeId: item.computerCodeId,
            remark: item.remark,
            rowSpan: 0,
          };
          device.rowSpan = index2 == 0 ? item.quotaItemEditList.length : 0;
          let planDetail = this.initPlanDetailTemplate(device);
          planDetail.price = item2.price;
          planDetail.areaName = item2.areaName;
          planDetail.basePriceId = item2.basePriceId;
          planDetail.quotaId=device.quotaId;
          planDetail.standardCodeName = item2.standardCodeName;
          this.quotaItemEditList.push(planDetail);
        });
      });
    },
    add() {
      this.$refs.SmStdBasicQuotaModal.add();
    },

    detail(record) {
      this.$refs.SmStdBasicQuotaModal.detail(record);
    },

    edit(record) {
      this.$refs.SmStdBasicQuotaModal.edit(record);
    },

    onExpandQuota(record) {
      this.selectQuota = null;

      this.selectQuota = record;
    },

    remove(record) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            if (record.quotaItems.length <= 0) {
              let response = await apiQuota.delete(record.id);
              if (requestIsSuccess(response)) {
                _this.refresh();
                _this.$message.success('删除成功');
                setTimeout(resolve, 100);
              } else setTimeout(reject, 100);
            } else {
              _this.$message.error('该定额下有定额清单数据，无法删除');
            }
          });
        },
      });
    },

    //刷新
    async refresh(resetPage = true, page) {
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let response = await apiQuota.getList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...this.queryParams,
      });
      if (requestIsSuccess(response)) {
        let _railways = response.data.items;
        _railways.map(item => {
          item.quotaItems = item.quotaItems;
          this.quotaItemEditList = [];
          if (item.quotaItems && item.quotaItems.length > 0) {
            this.mergeBasePrice(item);
          }
          item.basePrices = this.quotaItemEditList;
        });
        this.quotas = _railways;
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

    //页面改变事件
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
        importKey: 'quota',
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

    async importQuotaItem(file, id) {
      // 构造导入参数（根据自己后台方法的实际参数进行构造）
      let importParamter = {
        'file.file': file,
        id: id,
        importKey: 'quotaItem',
      };
      // 执行文件上传
      await this.$refs.smImportQuotaItem.exect(importParamter);
    },

    //文件导出
    async exportQuotaItem(para, id) {
      let queryParams = {};
      queryParams = { ...this.queryParams, quotaId: id };
      //执行文件下载
      await this.$refs.smExport.isCanDownload(para, queryParams);
    },
    //添加清单
    addQuotaItem(record) {
      this.$refs.SmStdBasicQuotaItemModal.add(record);
    },
    //清单详情
    async  detailQuotaItem(record) {
      let basePriceIdList = [];
      let response = await apiQuotaItem.getList({
        quotaId:record.device.quotaId,
        isAll:true,
        ...this.queryParams,
      });
      if (requestIsSuccess(response))
      {
        response.data.items.map(item=>{
          if (item.computerCodeId == record.device.computerCodeId) {
            item.quotaItemEditList.map(it=>{ basePriceIdList.push(it.basePriceId);});
           
          }});
      }
      // this.selectQuota.basePrices.map(item => {
      //   if (item.device.computerCodeId === record.device.computerCodeId) {
      //     basePriceIdList.push(item.basePriceId);
      //   }
      // });
      let editValue = {
        computerCodeId: record.device.computerCodeId,
        basePriceIdList: basePriceIdList,
        standardCodeName: record.device.standardCodeName,
        quotaId: record.device.quotaId,
        number: record.device.number,
        remark: record.device.remark,
      };
      this.$refs.SmStdBasicQuotaItemModal.detail(editValue);
    },
    //编辑清单
    async  editQuotaItem(record) {
      let basePriceIdList = [];
      let response = await apiQuotaItem.getList({
        quotaId:record.device.quotaId,
        isAll:true,
        ...this.queryParams,
      });
      if (requestIsSuccess(response))
      {
        response.data.items.map(item=>{
          if (item.computerCodeId == record.device.computerCodeId) {
            item.quotaItemEditList.map(it=>{ basePriceIdList.push(it.basePriceId);});
           
          }});
      }
      // this.selectQuota.basePrices.map(item => {
      //   if (item.device.computerCodeId == record.device.computerCodeId) {
      //     basePriceIdList.push(item.basePriceId);
      //   }
      // });
      let editValue = {
        computerCodeId: record.device.computerCodeId,
        basePriceIdList: basePriceIdList,
        standardCodeName: record.device.standardCodeName,
        quotaId: record.device.quotaId,
        number: record.device.number,
        remark: record.device.remark,
      };
      this.$refs.SmStdBasicQuotaItemModal.edit(editValue);
    },
    //删除清单
    removeQuotaItem(record) {
      console.log(record.device.quotaId, record.device.computerCodeId);
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiQuotaItem.delete(
              record.device.quotaId,
              record.device.computerCodeId,
            );
            if (requestIsSuccess(response)) {
              _this.refresh();
              _this.$message.success('删除成功');
              setTimeout(resolve, 100);
            } else setTimeout(reject, 100);
          });
        },
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
              placeholder="请输入定额名称或者编码"
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
                vP(this.permissions, permissionsSmStdBasic.Quotas.Create),
              )}
              {vIf(
                <SmImport
                  ref="smImport"
                  url="api/app/stdBasicQuota/upload"
                  axios={this.axios}
                  downloadErrorFile={true}
                  importKey="quota"
                  onSelected={file => this.fileSelected(file)}
                  onIsSuccess={() => {
                    this.refresh();
                  }}
                />,
                vP(this.permissions, permissionsSmStdBasic.Quotas.Import),
              )}
              {vIf(
                <SmTemplateDownload
                  axios={this.axios}
                  downloadKey="quota"
                  downloadFileName="定额"
                >
                </SmTemplateDownload>,
                vP(this.permissions, permissionsSmStdBasic.Quotas.Import),
              )}
              {vIf(
                <SmExport
                  ref="smExport"
                  url="api/app/stdBasicQuota/export"
                  axios={this.axios}
                  templateName="quota"
                  downloadFileName="定额信息表"
                  rowIndex={5}
                  onDownload={para => this.downloadFile(para)}
                />,
                vP(this.permissions, permissionsSmStdBasic.Quotas.Export),
              )}
            </div>
          </template>
        </sc-table-operator>
        <a-table
          width={600}
          columns={this.columns}
          dataSource={this.quotas}
          rowKey={record => record.id}
          pagination={false}
          onExpand={(expanded, record) => {
            this.onExpandQuota(record);
          }}
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
                      vP(this.permissions, permissionsSmStdBasic.Quotas.Detail),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsSmStdBasic.Quotas.Detail) &&
                        vP(this.permissions, [
                          permissionsSmStdBasic.Quotas.Update,
                          permissionsSmStdBasic.Quotas.Delete,
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
                            vP(this.permissions, permissionsSmStdBasic.Quotas.Update),
                          )}
                          {vIf(
                            <a-menu-item>
                              <a onClick={() => this.remove(record)}>删除</a>
                            </a-menu-item>,
                            vP(this.permissions, permissionsSmStdBasic.Quotas.Delete),
                          )}
                          {vIf(
                            <a-menu-item>
                              <a onClick={() => this.addQuotaItem(record)}>添加清单</a>
                            </a-menu-item>,
                            vP(this.permissions, permissionsSmStdBasic.QuotaItems.Create),
                          )}
                          {vIf(
                            // <a-menu-item>
                            //   <a onClick>导出定额清单</a>
                            // </a-menu-item>,

                            <SmImportQuotaItem
                              ref="smImportQuotaItem"
                              url="api/app/stdBasicQuotaItem/upload"
                              axios={this.axios}
                              downloadErrorFile={true}
                              importKey="quotaItem"
                              defaultTitle={'导入清单'}
                              btnDefaultType={'link'}
                              // icon={'display:none'}
                              onSelected={file => this.importQuotaItem(file, record.id)}
                              onIsSuccess={() => {
                                this.refresh();
                              }}
                            />,

                            vP(this.permissions, permissionsSmStdBasic.QuotaItems.Import),
                          )}
                          {vIf(
                            <SmExportQuotaItem
                              ref="smExport"
                              url="api/app/stdBasicQuotaItem/export"
                              axios={this.axios}
                              templateName="quotaItem"
                              downloadFileName="定额清单信息表"
                              defaultTitle={'导出清单'}
                              rowIndex={5}
                              onDownload={para => this.exportQuotaItem(para, record.id)}
                            />,
                            vP(this.permissions, permissionsSmStdBasic.QuotaItems.Export),
                          )}
                        </a-menu>
                      </a-dropdown>,
                      vP(this.permissions, [
                        permissionsSmStdBasic.Quotas.Update,
                        permissionsSmStdBasic.Quotas.Delete,
                      ]),
                    )}
                  </span>,
                ];
              },
              expandedRowRender: text => {
                return (
                  <div>
                    <a-table
                      rowKey={record => record.basePriceId}
                      slot-scope="text"
                      columns={this.innerColumns}
                      dataSource={text.basePrices}
                      pagination={false}
                      bordered
                      // {...{
                      //   scopedSlots: {
                      //     index: (text, record, index) => {
                      //       return index + 1;
                      //     },
                      //     bpOperations: (text, record, index) => {
                      //       return [
                      //         <span>
                      //           {vIf(
                      //             <a
                      //               onClick={() => {
                      //                 this.detailQuotaItem(record);
                      //               }}
                      //             >
                      //               详情
                      //             </a>,
                      //             vP(this.permissions, permissionsSmStdBasic.QuotaItems.Detail),
                      //           )}
                      //           {vIf(
                      //             <a-divider type="vertical" />,
                      //             vP(this.permissions, permissionsSmStdBasic.QuotaItems.Detail) &&
                      //               vP(this.permissions, [
                      //                 permissionsSmStdBasic.QuotaItems.Update,
                      //                 permissionsSmStdBasic.QuotaItems.Delete,
                      //               ]),
                      //           )}
                      //           {vIf(
                      //             <a-dropdown trigger={['click']}>
                      //               <a>
                      //                 更多
                      //                 <a-icon type="down" />
                      //               </a>
                      //               <a-menu slot="overlay">
                      //                 {vIf(
                      //                   <a-menu-item>
                      //                     <a onClick={() => this.editQuotaItem(record)}>编辑</a>
                      //                   </a-menu-item>,
                      //                   vP(
                      //                     this.permissions,
                      //                     permissionsSmStdBasic.QuotaItems.Update,
                      //                   ),
                      //                 )}
                      //                 {vIf(
                      //                   <a-menu-item>
                      //                     <a onClick={() => this.removeQuotaItem(record)}>删除</a>
                      //                   </a-menu-item>,
                      //                   vP(
                      //                     this.permissions,
                      //                     permissionsSmStdBasic.QuotaItems.Delete,
                      //                   ),
                      //                 )}
                      //               </a-menu>
                      //             </a-dropdown>,
                      //             vP(this.permissions, [
                      //               permissionsSmStdBasic.QuotaItems.Update,
                      //               permissionsSmStdBasic.QuotaItems.Delete,
                      //             ]),
                      //           )}
                      //         </span>,
                      //       ];
                      //     },
                      //   },
                      // }}
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
        <SmStdBasicQuotaModal
          ref="SmStdBasicQuotaModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />

        <SmStdBasicQuotaItemModal
          ref="SmStdBasicQuotaItemModal"
          axios={this.axios}
          onSuccess={() => {
            this.refresh(false);
          }}
        />
      </div>
    );
  },
};
