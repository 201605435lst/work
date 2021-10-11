import './style';
import {
  requestIsSuccess,
  vP,
  vIf,
  getMaterialAcceptanceTypeEnable,
  getMaterialAcceptanceTestStatus,
} from '../../_utils/utils';
import { MaterialAcceptanceTypeEnable, MaterialAcceptanceTestStatus } from '../../_utils/enum';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import ApiAcceptance from '../../sm-api/sm-material/Acceptance';
import { pagination as paginationConfig, tips as tipsConfig } from '../../_utils/config';
import permissionsAcceptance from '../../_permissions/sm-material';
import SmMaterialAcceptanceModal from './SmMaterialAcceptanceModal';
import FileSaver from 'file-saver';
import moment from 'moment';
let apiAcceptance = new ApiAcceptance();

export default {
  name: 'SmMaterialAcceptance',
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
        testingOrganizationId: undefined, //检测机构
        startTime: null,
        endTime: null,
        testingType: undefined, //检测类型
        maxResultCount: paginationConfig.defaultPageSize,
      },
      dateRange: [],
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
          title: '报告编号',
          dataIndex: 'code',
          ellipsis: true,
          scopedSlots: { customRender: 'code' },
        },

        {
          title: '检测类型',
          ellipsis: true,
          dataIndex: 'testingType',
          scopedSlots: { customRender: 'testingType' },
        },
        {
          title: '检测机构',
          ellipsis: true,
          dataIndex: 'testingOrganizationName',
          scopedSlots: { customRender: 'testingOrganizationName' },
        },
        {
          title: '验收时间',
          ellipsis: true,
          dataIndex: 'receptionTime',
          scopedSlots: { customRender: 'receptionTime' },
        },
        {
          title: '登记人',
          ellipsis: true,
          dataIndex: 'creator',
          scopedSlots: { customRender: 'creator' },
        },
        {
          title: '状态',
          ellipsis: true,
          dataIndex: 'status',
          scopedSlots: { customRender: 'status' },
        },
        {
          title: '备注',
          dataIndex: 'remark',
          ellipsis: true,
          scopedSlots: { customRender: 'remark' },
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
  },
  methods: {
    initAxios() {
      apiAcceptance = new ApiAcceptance(this.axios);
    },

    // 添加
    add() {
      this.$refs.SmMaterialAcceptanceModal.add();
    },
    // 详情
    view(record) {
      this.$refs.SmMaterialAcceptanceModal.view(record);
    },
    // 编辑
    edit(record) {
      this.$refs.SmMaterialAcceptanceModal.edit(record);
    },
    // 导出
    async export(id) {
      let response = await apiAcceptance.export(id);
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
            let response = await apiAcceptance.delete(record.id);
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
        onCancel() { },
      });
    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let data = {
        ...this.queryParams,
        startTime: this.dateRange[0] ? this.dateRange[0].format('YYYY-MM-DD HH:mm:ss') : null,
        endTime: this.dateRange[1] ? this.dateRange[1].format('YYYY-MM-DD HH:mm:ss') : null,
      };
      let response = await apiAcceptance.getList({
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
    getDate(record) {
      let time = null;
      switch (record.TestingStatus) {
      case MaterialAcceptanceTestStatus.ToSubmit:
        time = record.creationTime;
        break;
      case MaterialAcceptanceTestStatus.ForAcceptance:
        time = record.submissionTime;
        break;
      case MaterialAcceptanceTestStatus.Approved:
        time = record.receptionTime;
        break;
      default:
        time = record.creationTime;
        break;
      }
      return time;
    },
    getStatus(record) {
      let tar = null;
      switch (record.testingStatus) {
      case MaterialAcceptanceTestStatus.ForAcceptance:
        tar = (
          <a-tag color="blue">
              待验收
          </a-tag>
        );
        break;
      case MaterialAcceptanceTestStatus.Approved:
        tar = (
          <a-tag color="green">
              已验收
          </a-tag>
        );
        break;
      }
      return tar;
    },
  },
  render() {
    let TypeOptions = [];
    for (let item in MaterialAcceptanceTypeEnable) {
      TypeOptions.push(
        <a-select-option key={`${MaterialAcceptanceTypeEnable[item]}`}>
          {getMaterialAcceptanceTypeEnable(MaterialAcceptanceTypeEnable[item])}
        </a-select-option>,
      );
    }
    return (
      <div class="sm-material-acceptance">
        <sc-table-operator
          onSearch={() => {
            this.refresh();
          }}
          onReset={() => {
            this.queryParams.testingOrganizationId = undefined; // 检测机构
            this.queryParams.startTime = null;
            this.queryParams.endTime = null;
            this.queryParams.testingType = undefined; // 检测类型
            this.dateRange = [];
            this.refresh();
          }}
        >
          <a-form-item label="检测机构">
            <DataDictionaryTreeSelect
              axios={this.axios}
              groupCode={'TestingOrganization'}
              placeholder="请选择检测机构"
              value={this.queryParams.testingOrganizationId}
              onChange={value => {
                this.queryParams.testingOrganizationId = value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="检测日期">
            <a-range-picker
              class="data-picker"
              value={this.dateRange}
              onChange={value => {
                this.dateRange = value;
                this.refresh();
              }}
            />
          </a-form-item>
          <a-form-item label="检测类型">
            <a-select
              allowClear
              placeholder="请选择"
              value={this.queryParams.testingType}
              onChange={value => {
                this.queryParams.testingType = value;
                this.refresh();
              }}
            >
              {TypeOptions}
            </a-select>
          </a-form-item>
          <template slot="buttons">
            {vIf(
              <a-button type="primary" onClick={() => this.add()}>
                添加
              </a-button>,
              vP(this.permissions, permissionsAcceptance.Acceptances.Create),
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
              code: (text, record) => {
                let result = record ? record.code : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span> {result}</span>
                  </a-tooltip>
                );
              },
              testingType: (text, record) => {
                let result =
                  record && record.testingType
                    ? getMaterialAcceptanceTypeEnable(record.testingType)
                    : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span> {result}</span>
                  </a-tooltip>
                );
              },
              testingOrganizationName: (text, record) => {
                let result = record.testingOrganization ? record.testingOrganization.name : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              receptionTime: (text, record) => {
                let time = moment(record.receptionTime).format("YYYY-MM-DD HH:mm:ss");
                return (
                  <a-tooltip placement="topLeft" title={time}>
                    {time}
                  </a-tooltip>
                );
              },
              creator: (text, record) => {
                let result = record.creator && record.creator.name ? record.creator.name : '';
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span> {result}</span>
                  </a-tooltip>
                );
              },
              status: (text, record) => {
                let result = this.getStatus(record);
                return (
                  <span> {result}</span>
                );
              },
              remark: (text, record) => {
                return (
                  <a-tooltip placement="topLeft" title={record.remark}>
                    <span>{record.remark}</span>
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
                      >
                        详情
                      </a>,
                      vP(this.permissions, permissionsAcceptance.Acceptances.Detail),
                    )}
                    {vIf(
                      <a-divider type="vertical" />,
                      vP(this.permissions, permissionsAcceptance.Acceptances.Detail) &&
                      vP(this.permissions, [
                        permissionsAcceptance.Acceptances.Update,
                        permissionsAcceptance.Acceptances.Delete,
                      ]),
                    )}

                    {vIf(
                      <a-dropdown trigger={['click']}>
                        <a class="ant-dropdown-link" onClick={e => e.preventDefault()}>
                          更多 <a-icon type="down" />
                        </a>
                        <a-menu slot="overlay">
                          {!(record.testingStatus == MaterialAcceptanceTestStatus.Approved) ?
                            vIf(
                              <a-menu-item>
                                <a
                                  onClick={() => {
                                    this.edit(record);
                                  }}
                                >
                                  编辑
                                </a>
                              </a-menu-item>,
                              vP(this.permissions, permissionsAcceptance.Acceptances.Update),
                            ) : ''}
                          {record.testingStatus == MaterialAcceptanceTestStatus.ForAcceptance ?
                            vIf(
                              <a-menu-item>
                                <a
                                  onClick={() => {
                                    this.remove(record);
                                  }}
                                >
                                  删除
                                </a>
                              </a-menu-item>,
                              vP(this.permissions, permissionsAcceptance.Acceptances.Delete),
                            ) : ''}
                          {record.testingStatus == MaterialAcceptanceTestStatus.Approved ?
                            vIf(
                              <a-menu-item>
                                <a
                                  onClick={() => {
                                    this.export(record.id);
                                  }}
                                >
                                  导出
                                </a>
                              </a-menu-item>,
                              vP(this.permissions, permissionsAcceptance.Acceptances.Export),
                            ) : ''}
                        </a-menu>
                      </a-dropdown>,
                      vP(this.permissions, [
                        permissionsAcceptance.Acceptances.Create,
                        permissionsAcceptance.Acceptances.Update,
                        permissionsAcceptance.Acceptances.UpdateEnable,
                        permissionsAcceptance.Acceptances.Delete,
                      ]),
                    )}
                  </span>,
                ];
              },
            },
          }}
        ></a-table>
        {/* 添加/编辑模板 */}
        <SmMaterialAcceptanceModal
          ref="SmMaterialAcceptanceModal"
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
