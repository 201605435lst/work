import './style';
import {
  requestIsSuccess,
  getRepairLevelTitle,
  vIf,
  vP,
  getWorkTicketRltCooperationUnitState,
} from '../../_utils/utils';
import OrganizationTreeSelect from '../../sm-system/sm-system-organization-tree-select';
import ApiSkyLightPlan from '../../sm-api/sm-cr-plan/SkyLightPlan';
import moment from 'moment';
import SmCrPlanCooperateWorkMoadl from './SmCrPlanCooperateWorkModal';
import {
  pagination as paginationConfig,
} from '../../_utils/config';
import {
  WorkTicketRltCooperationUnitState,
} from '../../_utils/enum';
import permissionsSmCrPlan from '../../_permissions/sm-cr-plan';


let apiSkyLightPlan = new ApiSkyLightPlan();

export default {
  name: 'SmCrPlanCooperateWork',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      form: {},
      totalCount: 0,
      pageIndex: 1,
      queryParams: {
        workUnit: null,
        dateRange: [moment(moment()).startOf('day'), moment(moment()).endOf('day')], //时间查询
        maxResultCount: paginationConfig.defaultPageSize,
      },
      loading: false,
      workTicketList: [],
    };
  },
  computed: {
    columns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          // width: 80,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '作业名称',
          dataIndex: 'workTitle',
          // width: 60,
          scopedSlots: { customRender: 'workTitle' },
        },
        {
          title: '计划时间',
          dataIndex: 'workTime',
          // width: 60,
          ellipsis: true,
          scopedSlots: { customRender: 'workTime' },
        },
        {
          title: '作业地点',
          dataIndex: 'workPlace',
          // width: 60,
          scopedSlots: { customRender: 'workPlace' },
        },
        {
          title: '维修等级',
          dataIndex: 'repairLevel',
          // width: 60,
          scopedSlots: { customRender: 'repairLevel' },
        },
        {
          title: '主体车间',
          dataIndex: 'mainWorkShopName',
          // width: 60,
          // scopedSlots: { customRender: 'mainWorkShopName' },
        },
        {
          title: '状态',
          dataIndex: 'state',
          // width: 60,
          scopedSlots: { customRender: 'state' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          // width: 140,
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
      apiSkyLightPlan = new ApiSkyLightPlan(this.axios);
    },
    async refresh(resetPage = true, page) {
      this.loading = true;
      if (resetPage) {
        this.pageIndex = 1;
        this.queryParams.maxResultCount = paginationConfig.defaultPageSize;
      }
      let queryParams = {
        workUnit: this.queryParams.workUnit,
        startTime: moment(this.queryParams.dateRange[0])
          .hours(0)
          .minutes(0)
          .seconds(0)
          .format('YYYY-MM-DD HH:mm:ss'),
        endTime: moment(this.queryParams.dateRange[1])
          .hours(23)
          .minutes(59)
          .seconds(59)
          .format('YYYY-MM-DD HH:mm:ss'),
        maxResultCount: this.queryParams.maxResultCount,
      };
      let response = await apiSkyLightPlan.getWorkTicketList({
        skipCount: (this.pageIndex - 1) * this.queryParams.maxResultCount,
        ...queryParams,
      });
      if (requestIsSuccess(response)) {
        this.workTicketList = response.data.items;
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

    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },

    view(record) {
      this.$refs.SmCrPlanCooperateWorkMoadl.view(record);
    },

    //兑现反馈
    finish(record) {
      this.$refs.SmCrPlanCooperateWorkMoadl.finish(record);
    },

    //状态颜色选择
    getTagColor(state) {
      let tagColor = '';
      switch (state) {
      case WorkTicketRltCooperationUnitState.Finish: {
        //已完成
        tagColor = 'green';
        break;
      }
      case WorkTicketRltCooperationUnitState.UnFinish: {
        //未完成
        tagColor = 'red';
        break;
      }
      }
      return tagColor;
    },
  },
  render() {
    return (
      <div class="sm-cr-plan-cooperate-work">
        <a-form form={this.form}>
          <sc-table-operator
            onSearch={() => {
              this.refresh();
            }}
            onReset={() => {
              this.queryParams.workUnit = null;
            }}
          >
            <a-form-item label="作业车间">
              <OrganizationTreeSelect
                axios={this.axios}
                autoInitial={true}
                value={this.queryParams.workUnit}
                onChange={value => {
                  this.queryParams.workUnit = value;
                  this.refresh();
                }}
              ></OrganizationTreeSelect>
            </a-form-item>
            <a-form-item label="作业时间">
              <a-range-picker
                value={this.queryParams.dateRange}
                onChange={value => {
                  this.queryParams.dateRange = value;
                  this.refresh();
                }}
              ></a-range-picker>
            </a-form-item>
          </sc-table-operator>
        </a-form>
        <a-table
          columns={this.columns}
          loading={this.loading}
          dataSource={this.workTicketList}
          rowKey={record => record.id}
          pagination={false}
          {...{
            scopedSlots: {
              index: (text, record, index) => {
                return (this.pageIndex - 1) * this.queryParams.maxResultCount + (index + 1);
              },
              workTime: (text, record, index) => {
                let time =
                  moment(record.planStartTime).format('yyyy-MM-DD HH:mm:ss') +
                  '-' +
                  moment(record.planFinishTime).format('yyyy-MM-DD HH:mm:ss');
                return (
                  <a-tooltip placement="topLeft" title={time}>
                    <span>{time}</span>
                  </a-tooltip>
                );
              },
              repairLevel: (text, record, index) => {
                let leavelList = record.repairLevel.split(',');
                let result = null;
                leavelList.map(item => {
                  if (!result) {
                    result = getRepairLevelTitle(parseInt(item));
                  } else {
                    result = result + '、' + getRepairLevelTitle(parseInt(item));
                  }
                });
                return (
                  <a-tooltip placement="topLeft" title={result}>
                    <span>{result}</span>
                  </a-tooltip>
                );
              },
              operations: (text, record, index) => {
                return (
                  <span>
                    {vIf(
                      <a onClick={() => this.view(record)}>详情</a>,
                      vP(this.permissions, permissionsSmCrPlan.CooperateWork.Detail),
                    )}

                    {record.state == WorkTicketRltCooperationUnitState.UnFinish
                      ? [
                        vIf(
                          <a-divider type="vertical" />,
                          vP(this.permissions, permissionsSmCrPlan.CooperateWork.Feedback) &&
                              vP(this.permissions, permissionsSmCrPlan.CooperateWork.Detail),
                        ),
                        vIf(
                          <a onClick={() => this.finish(record)}>兑现反馈</a>,
                          vP(this.permissions, permissionsSmCrPlan.CooperateWork.Feedback),
                        ),
                      ]
                      : undefined}
                  </span>
                );
              },
              state: (text, record, index) => {
                return (
                  <a-tag color={this.getTagColor(record.state)}>
                    {getWorkTicketRltCooperationUnitState(record.state)}
                  </a-tag>
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

        <SmCrPlanCooperateWorkMoadl
          axios={this.axios}
          ref="SmCrPlanCooperateWorkMoadl"
          onSuccess={() => {
            this.refresh();
          }}
        />
      </div>
    );
  },
};
    