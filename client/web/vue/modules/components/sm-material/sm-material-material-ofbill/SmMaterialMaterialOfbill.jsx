import './style';
import { requestIsSuccess, vP, vIf } from '../../_utils/utils';
import ApiMaterialOfBill from '../../sm-api/sm-material/MaterialOfBill';
import ApiSection from '../../sm-api/sm-construction-base/ApiSection';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import permissionsMaterialOfbill from '../../_permissions/sm-material';
import SmMaterialMaterialOfbillModal from './SmMaterialMaterialOfbillModal';
import FileSaver from 'file-saver';
import moment from 'moment';
let apiMaterialOfBill = new ApiMaterialOfBill();
let apiSection = new ApiSection();

export default {
  name: 'SmMaterialMaterialOfbill',
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
      pageIndex: 1,
      queryParams: {
        constructionTeam: null,
        startTime: null,
        endTime: null,
        sectionId: null,
        maxResultCount: paginationConfig.defaultPageSize,
      },
      timeValue: null,
      constructionTeam: '', // 施工队
      partialSection: [], // 施工区段
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
          title: '施工队',
          ellipsis: true,
          dataIndex: 'constructionTeam',
        },
        {
          title: '施工区段',
          ellipsis: true,
          dataIndex: 'section.name',
        },
        {
          title: '领料日期',
          ellipsis: true,
          dataIndex: 'time',
          scopedSlots: { customRender: 'time' },
        },
        {
          title: '领料人',
          ellipsis: true,
          dataIndex: 'userName',
        },
        {
          title: '备注',
          dataIndex: 'remark',
          ellipsis: true,
        },
        {
          title: '状态',
          ellipsis: true,
          dataIndex: 'status',
          scopedSlots: { customRender: 'status' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 169,
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  async created() {
    this.initAxios();
    this.refresh();
    this.getPartialSection();
  },
  methods: {
    initAxios() {
      apiMaterialOfBill = new ApiMaterialOfBill(this.axios);
      apiSection = new ApiSection(this.axios);
    },

    // 添加
    add() {
      this.$refs.SmMaterialMaterialOfbillModal.add();
    },
    // 详情
    view(record) {
      this.$refs.SmMaterialMaterialOfbillModal.view(record);
    },
    // 编辑
    edit(record) {
      this.$refs.SmMaterialMaterialOfbillModal.edit(record);
    },
    // 导出
    async export(id) {
      let response = await apiMaterialOfBill.export(id);
      if (requestIsSuccess(response)) {
        if (response.data.byteLength != 0) {
          this.$message.info('导出成功');
          FileSaver.saveAs(
            new Blob([response.data], { type: 'application/vnd.ms-excel' }),
            `物资验收明细.docx`,
          );
        }
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
            let response = await apiMaterialOfBill.delete(record.id);
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
      });
    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let response = await apiMaterialOfBill.getList({
        ...this.queryParams,
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        isChecking: true,
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
    // 获取位置分区信息
    async getPartialSection() {
      let section = await apiSection.getTreeList({ isAll: true });
      if (requestIsSuccess(section)) {
        this.partialSection = section.data.items;
      }
    },
    // 时间选择
    dateOnChange(date) {
      this.timeValue = date;
      if (date.length > 0) {
        this.queryParams.startTime = moment(date[0])
          .startOf('d')
          .format('YYYY-MM-DD HH:mm:ss');
        this.queryParams.endTime = moment(date[1])
          .endOf('d')
          .format('YYYY-MM-DD HH:mm:ss');
        this.refresh();
      }
    },
  },
  render() {
    return (
      <div class="sm-material-acceptance">
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.testingOrganizationId = undefined;
            this.queryParams.startTime = null;
            this.queryParams.endTime = null;
            this.queryParams.constructionTeam = undefined;
            this.timeValue = null;
            this.refresh();
          }}
        >
          <a-form-item label="施工队">
            <a-input
              style="margin-right:10px;"
              placeholder="请输入施工队"
              onDropdown-style="{ maxHeight: '400px', overflow: 'auto' }"
              value={this.queryParams.constructionTeam}
              onInput={event => {
                this.queryParams.constructionTeam = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="施工区段">
            <a-tree-select
              treeData={this.partialSection}
              placeholder="请选择施工区段"
              allow-clear
              replaceFields={{ title: 'name', key: 'id', value: 'id' }}
              tree-default-expand-all
              treeNodeLabelProp="title"
              value={this.queryParams.sectionId}
              onChange={value => {
                this.queryParams.sectionId = value;
                this.refresh();
              }}
            ></a-tree-select>
          </a-form-item>
          <a-form-item label="领料时间">
            <a-range-picker
              class="data-picker"
              allowClear={false}
              placeholder={['开始时间', '结束时间']}
              value={this.timeValue}
              onChange={value => {
                this.dateOnChange(value);
              }}
            />
          </a-form-item>
          <template slot="buttons">
            {vIf(
              <a-button type="primary" size="small" onClick={() => this.add()}>
                添加
              </a-button>,
              vP(this.permissions, permissionsMaterialOfbill.MaterialOfBill.Create),
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
              time: (text, record) => {
                let time = moment(record.time).format('YYYY-MM-DD');
                return (
                  <a-tooltip placement="topLeft" title={time}>
                    {time}
                  </a-tooltip>
                );
              },
              status: (text, record) => {
                return record.state == 1 ? (
                  <a-tag color="red">待提交</a-tag>
                ) : record.state == 2 ? (
                  <a-tag color="blue">待审核</a-tag>
                ) : (
                  <a-tag color="green">已通过</a-tag>
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
                      vP(this.permissions, permissionsMaterialOfbill.MaterialOfBill.Detail),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsMaterialOfbill.MaterialOfBill.Detail) &&
                      vP(this.permissions, [
                        permissionsMaterialOfbill.MaterialOfBill.Update,
                        permissionsMaterialOfbill.MaterialOfBill.Delete,
                      ]),
                    )}

                    {vIf(
                      <a-dropdown trigger={['click']}>
                        <a class="ant-dropdown-link" onClick={e => e.preventDefault()}>
                          更多 <a-icon type="down" />
                        </a>
                        <a-menu slot="overlay">
                          {record.state == 1 && record.state == 2
                            ? vIf(
                              <a-menu-item>
                                <a
                                  onClick={() => {
                                    this.remove(record);
                                  }}
                                >
                                  删除
                                </a>
                              </a-menu-item>,
                              vP(this.permissions, permissionsMaterialOfbill.MaterialOfBill.Delete),
                            )

                            : ''}
                          {record.state == 2
                            ? vIf(
                              <a-menu-item>
                                <a
                                  onClick={() => {
                                    this.edit(record);
                                  }}
                                >
                                  编辑
                                </a>
                              </a-menu-item>,
                              vP(this.permissions, permissionsMaterialOfbill.MaterialOfBill.Update),
                            )

                            : ''}
                          {record.state == 3
                            ? vIf(
                              <a-menu-item>
                                <a
                                  onClick={() => {
                                    this.export(record.id);
                                  }}
                                >
                                  导出
                                </a>
                              </a-menu-item>,
                              vP(this.permissions, permissionsMaterialOfbill.MaterialOfBill.Export),
                            )

                            : ''}
                        </a-menu>
                      </a-dropdown>,
                      vP(this.permissions, [
                        permissionsMaterialOfbill.MaterialOfBill.Create,
                        permissionsMaterialOfbill.MaterialOfBill.Update,
                        permissionsMaterialOfbill.MaterialOfBill.UpdateEnable,
                        permissionsMaterialOfbill.MaterialOfBill.Delete,
                      ]),
                    )}
                  </span>,
                ];
              },
            },
          }}
        ></a-table>
        {/* 添加/编辑模板 */}
        <SmMaterialMaterialOfbillModal
          ref="SmMaterialMaterialOfbillModal"
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
