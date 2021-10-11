import ApiMaintenanceRecord from '../../sm-api/sm-cr-plan/MaintenanceRecord';
import SmSystemOrganizationTreeSelect from '../../sm-system/sm-system-organization-tree-select';
import { pagination as paginationConfig } from '../../_utils/config';
import { requestIsSuccess, vIf, vP } from '../../_utils/utils';
import SmStdBasicRepairGroupSelect from '../../sm-std-basic/sm-std-basic-repair-group-select';
import SmBasicInstallationSiteSelect from '../../sm-basic/sm-basic-installation-site-select';
import permissionsSmCrPlan from '../../_permissions/sm-cr-plan';
import SmBasicRailwayTreeSelect from '../../sm-basic/sm-basic-railway-tree-select';
import StationCascader from '../../sm-basic/sm-basic-station-cascader';

import moment from 'moment';

let apiMaintenanceRecord = new ApiMaintenanceRecord();

export default {
  name: 'SmCrPlanMaintenanceRecords',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
    permissions: { type: Array, default: () => [] },
    repairTagKey: { type: String, default: null }, //维修项标签
  },
  data() {
    return {
      equipments: [], // 列表数据源
      pageSize: paginationConfig.defaultPageSize,
      totalCount: 0,
      pageIndex: 1,
      queryParams: {
        skipCount: 0,
        maxResultCount: paginationConfig.defaultPageSize,
        organizationId: null, //维护组织单元
        equipTypeId: undefined, //设备类型
        equipNameId: undefined, //设备名称/类别
        installationSiteId: null, //安装地点
        keywords: null,
        time: moment(),//检修时间
        railwayId: undefined,
        stationId: undefined,
      },
      form: this.$form.createForm(this),
      isInit: false,
      currentOrganizationId: null,//当前用户所属组织机构
      allEquipments: [],
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          scopedSlots: { customRender: 'index' },
          width: 90,
        },
        {
          title: '设备类型',
          dataIndex: 'equipType',
          width: 120,
          ellipsis: true,
        },
        {
          title: '设备名称/类别',
          dataIndex: 'equipName',
          width: 120,
          ellipsis: true,
        },
        // {
        //   title: '设备型号',
        //   dataIndex: 'equipModelNumber',
        //   width: 120,
        //   ellipsis: true,
        // },
        // {
        //   title: '设备编码',
        //   dataIndex: 'equipModelCode',
        //   width: 120,
        //   ellipsis: true,
        // },
        {
          title: '作业处所',
          dataIndex: 'installationSite',
          width: 160,
          ellipsis: true,
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 80,
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
  },
  watch: {},

  async created() {
    this.initAxios();
    this.refresh();
    this.isInit = true;
  },
  methods: {
    view(record) {
      let workOrderIds = [...record.maintenanceRecordRltWorkOrders, record.workOrderId];
      this.$emit(
        'view',
        this.queryParams.organizationId,
        record.resourceEquipId,
        record.equipNameId,
        record.equipType,
        record.equipName,
        record.equipModelNumber,
        record.equipModelCode,
        record.installationSite,
        workOrderIds,
      );
    },
    initAxios() {
      apiMaintenanceRecord = new ApiMaintenanceRecord(this.axios);
    },

    async refresh() {
      this.loading = true;
      this.queryParams.startTime = moment(this.queryParams.time)
        .date(1)
        .hours(0)
        .minutes(0)
        .seconds(0)
        .format('YYYY-MM-DD HH:mm:ss');

      this.queryParams.endTime = moment(this.queryParams.time)
        .endOf('month')
        .format('YYYY-MM-DD HH:mm:ss');

      let response = await apiMaintenanceRecord.getList(this.queryParams, this.repairTagKey);
      if (requestIsSuccess(response) && response.data.items) {
        this.allEquipments = response.data.items.map((item, index) => {
          return {
            ...item,
            order: index + 1,
          };
        });

        this.onPageChange(this.pageIndex, this.queryParams.maxResultCount);
        this.totalCount = response.data.totalCount;
      }
      this.loading = false;
    },
    //前端分页
    onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      let indexStart = (page - 1) * pageSize;
      let indexEnd = (page - 1) * pageSize + pageSize;
      this.equipments = this.allEquipments.slice(indexStart, indexEnd);
      console.log(this.equipments);
    },

    filterOption(input, option) {
      console.log(option.componentOptions);
      return (
        option.componentOptions.children[0].text.toLowerCase().indexOf(input.toLowerCase()) >= 0
      );
    },
  },
  render() {
    return (
      <div class="sm-cr-plan-maintenance-records">
        {/* 操作区 */}
        <sc-table-operator
          onSearch={
            ()=> {
              this.refresh();
            }
          }
          
          onReset={() => {
            this.queryParams = {
              organizationId: this.currentOrganizationId,
              state: undefined,
              alterType: undefined,
              keyword: null,
              time: moment(),
              skipCount: 0,
              maxResultCount: paginationConfig.defaultPageSize,
              railwayId: undefined,
              stationId: undefined,
            };
            this.refresh();
          }}
          advancedCount={20}
        >
          <a-form-item label="维护单位">
            <SmSystemOrganizationTreeSelect
              placeholder="请选择维护单位"
              axios={this.axios}
              value={this.queryParams.organizationId}
              autoInitial={true}
              allowClear={false}
              onInput={value => {
                if (this.isInit) {
                  this.currentOrganizationId = value;
                }
                this.isInit = false;
                this.queryParams.organizationId = value;
                this.refresh();
              }}
            />
          </a-form-item>

          <a-form-item label="检修时间">
            <a-month-picker
              style="width:100%"
              value={this.queryParams.time}
              onChange={value => { this.queryParams.time = value; this.refresh(); }}
              allowClear={false}
            />
          </a-form-item>

          <a-form-item label="关键字">
            <a-input
              placeholder="设备型号、设备编码"
              value={this.queryParams.keywords}
              onInput={event => {
                this.queryParams.keywords = event.target.value;
                this.refresh();
              }}
            />
          </a-form-item>

          <a-form-item label="线路">
            <SmBasicRailwayTreeSelect
              axios={this.axios}
              onChange={value => {
                this.queryParams.railwayId = value;
                this.queryParams.installationSiteId = undefined;
                this.refresh();
              }}
              showSearch={true}
            />
          </a-form-item>

          <a-form-item label="站点/区间">
            <StationCascader
              axios={this.axios}
              railwayId={this.queryParams.railwayId}
              organizationId={this.queryParams.organizationId}
              onChange={(value, name) => {
                console.log(name);
                this.queryParams.stationId = value;
                this.queryParams.installationSiteId = undefined;
                this.refresh();
              }}
            />
          </a-form-item>

          <a-form-item label="作业处所">
            <SmBasicInstallationSiteSelect
              placeholder="请选择作业处所"
              axios={this.axios}
              height={32}
              value={this.queryParams.installationSiteId}
              railwayId={this.queryParams.railwayId}
              stationId={this.queryParams.stationId}
              onChange={value => {
                this.queryParams.installationSiteId = value;
                this.refresh();
              }}
            />
          </a-form-item>

          <a-form-item label="设备类型">
            <SmStdBasicRepairGroupSelect
              placeholder="请选择设备类型"
              allowClear
              isTop={true}
              permissions={this.permissions}
              axios={this.axios}
              value={this.queryParams.equipTypeId}
              onChange={value => {
                this.queryParams.equipTypeId = value;
                this.queryParams.equipNameId = undefined;
                this.refresh();
              }}
            />
          </a-form-item>

          <a-form-item label="设备名称/类别">
            <SmStdBasicRepairGroupSelect
              placeholder="请选择设备名称、类别"
              allowClear
              permissions={this.permissions}
              parentId={this.queryParams.equipTypeId}
              axios={this.axios}
              value={this.queryParams.equipNameId}
              onChange={value => {
                this.queryParams.equipNameId = value;
                this.refresh();
              }}
            />
          </a-form-item>

        </sc-table-operator>

        {/* 展示区 */}
        <a-table
          loading={this.loading}
          columns={this.columns}
          rowKey={record => record.order}
          dataSource={this.equipments}
          bordered={this.bordered}
          pagination={false}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return index + 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
              },
              operations: (text, record) => {
                return [
                  <span>
                    {vIf(
                      <a
                        onClick={() => {
                          this.view(record);
                          console.log(record);
                        }}
                      >
                        查看记录
                      </a>,
                      vP(this.permissions, permissionsSmCrPlan.MaintenanceRecord.Default),
                    )}
                  </span>,
                ];
              },
            },
          }}
        ></a-table>
        {/* <a-button></a-button> */}
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
