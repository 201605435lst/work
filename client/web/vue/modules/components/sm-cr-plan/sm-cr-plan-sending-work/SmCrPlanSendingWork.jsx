import './style';
import moment from 'moment';
import {
  DateReportType,
  SendWorkOperatorType,
  StandTestType,
  MemberType,
  PlanState,
  RepairTagKeys,
  WorkContentType,
  OrderState,
} from '../../_utils/enum';
import * as utils from '../../_utils/utils';
import FileSaver from 'file-saver';
import SmFileManageSelect from '../../sm-file/sm-file-manage-select';
import SmSystemAllUserSelect from '../../sm-system/sm-system-member-modal';
import SmSystemUserSelect from '../../sm-system/sm-system-member-select';
import ApiWorkOrder from '../../sm-api/sm-cr-plan/WorkOrder';
import ApiSkyLightPlan from '../../sm-api/sm-cr-plan/SkyLightPlan';
import ApiAccount from '../../sm-api/sm-system/Account';
import SmCrPlanSentingWorkFinishTicketModal from './SmCrPlanSentingWorkFinishTicketModal';
import { pagination as paginationConfig } from '../../_utils/config';
import SmTestWorkModal from './SmTestWorkModal';
import SmFileUpload from '../../sm-file/sm-file-upload/SmFileUpload';

let apiSkyLightPlan = new ApiSkyLightPlan();
let apiWorkOrder = new ApiWorkOrder();
let apiAccount = new ApiAccount();

const formFields = ['orderNo', 'feedback'];

export default {
  name: 'SmCrPlanSendingWork',
  props: {
    axios: { type: Function, default: null },
    bordered: { type: Boolean, default: false },
    sendingWorkId: { type: String, default: null }, //派工作业Id
    operatorType: { type: Number, default: SendWorkOperatorType.View }, // 页面状态
    repairTagKey: { type: String, default: null }, //维修项标签
    orderState: { type: Number, default: OrderState.Complete }, //计划状态
  },
  data() {
    return {
      iSendingWorkId: null,
      iOperatorType: SendWorkOperatorType.View,
      record: null, // 获取的数据库原始数据
      planDetailList: [], // 计划详细
      editFileRecord: null, // 要更新文件的测试项纪录
      dateRange: [moment(), moment()],
      fileModalIsVisible: false, //文件弹框是否弹出
      userSelectVisible: false, //用户选择框是否弹出
      maintenanceUserList: [],
      isRailwayHighSpeed: this.repairTagKey == RepairTagKeys.RailwayHighSpeed, //当前页面是否是高铁版
      userSelectVisibleForHightWay: false, //用户选择框是否弹出 高铁
      repairUserListForHightWay: [],
      workTicketTotalCount: 0, //高铁 工作票总数
      workTicketFinishCount: 0, //高铁 工作票完成数
      form: {},
      cashFeedBack: PlanState.Complete,
      exportLoading: false,
      isAcceptance:false,
    };
  },
  computed: {
    columns() {
      let arr = [
        {
          title: '设备名称/类别',
          dataIndex: 'deviceName',
          width: 130,
          ellipsis: true,
          customRender: (text, row, index) => {
            return {
              children: row.device.deviceName,
              attrs: { rowSpan: row.device.rowSpan },
            };
          },
        },
        {
          title: '设备型号/编号',
          dataIndex: 'equipmentName',
          width: 130,
          ellipsis: true,
          customRender: (text, row, index) => {
            return {
              children: row.equipment.equipmentName,
              attrs: { rowSpan: row.equipment.rowSpan },
            };
          },
        },
        {
          title: '年/月表',
          dataIndex: 'yearMonthPlanType',
          width: 90,
          customRender: (text, row, index) => {
            let strText = '';
            switch (row.yearMonth.yearMonthPlanType) {
            case DateReportType.Year:
              strText = '年表';
              break;
            case DateReportType.Month:
              strText = '月表';
              break;
            default:
              break;
            }
            return {
              children: strText,
              attrs: { rowSpan: row.yearMonth.rowSpan },
            };
          },
        },
        {
          title: '序号',
          dataIndex: 'number',
          width: 110,
          ellipsis: true,
        },
        {
          title: '工作内容',
          dataIndex: 'workContent',
          width: 650,
          scopedSlots: { customRender: 'workContent' },
        },
        {
          title: '作业数量',
          dataIndex: 'planCount',
          width: 90,
          ellipsis: true,
        },
        {
          title: '完成数量',
          dataIndex: 'workCount',
          width: 100,
          ellipsis: true,
          customRender: (text, row, index) => {
            return (
              <a-input-number
                disabled={
                  this.iOperatorType == SendWorkOperatorType.View ||
                  this.iOperatorType == SendWorkOperatorType.Acceptance
                }
                style="width:100%;"
                precision={3}
                min={0}
                max={row.planCount}
                // defaultValue={row.workCount = row.planCount}
                value={this.isComplete ? 0 : row.workCount}
                onChange={value => {
                  row.workCount = value ? value : 0;
                }}
              />
            );
          },
        },
        {
          title: '检修人',
          dataIndex: 'maintenanceUserList',
          width: 300,
          customRender: (text, row, index) => {
            return (
              <SmSystemUserSelect
                axios={this.axios}
                bordered={false}
                showUserTab={true}
                height={50}
                disabled={
                  this.iOperatorType == SendWorkOperatorType.View ||
                  this.iOperatorType == SendWorkOperatorType.Acceptance ||
                  this.isComplete
                }
                value={row.maintenanceUserList}
                onChange={values => {
                  row.maintenanceUserList = values;
                }}
              />
            );
          },
        },
      ];

      if (
        this.iOperatorType != SendWorkOperatorType.Finish ||
        (this.isRailwayHighSpeed && this.iOperatorType == SendWorkOperatorType.Finish)
      ) {
        arr.push({
          title: '验收人',
          dataIndex: 'acceptanceUserList',
          width: 300,
          customRender: (text, row, index) => {
            let isDisable = false;
            if (this.isRailwayHighSpeed == false) {
              isDisable =
                this.iOperatorType == SendWorkOperatorType.View ||
                this.iOperatorType == SendWorkOperatorType.Finish;
            } else {
              isDisable = this.iOperatorType == SendWorkOperatorType.View;
            }
            return (
              <SmSystemUserSelect
                axios={this.axios}
                bordered={false}
                showUserTab={true}
                height={50}
                disabled={isDisable || this.isComplete}
                value={row.acceptanceUserList}
                onChange={values => {
                  if (row.equipmentTestResultList && row.equipmentTestResultList.length > 0) {
                    row.equipmentTestResultList.some(result => {
                      result.checkResult = '合格';
                    });
                  }
                  row.acceptanceUserList = values;
                }}
              />
            );
          },
        });
      }
      return arr;
    },
    otherPlanColumns() {
      let arr = [
        {
          title: '工作内容',
          dataIndex: 'workContent',
          width: 300,
          scopedSlots: { customRender: 'workContent' },
        },
        {
          title: '检修人',
          dataIndex: 'maintenanceUserList',
          width: 300,
          customRender: (text, row, index) => {
            return (
              <SmSystemUserSelect
                axios={this.axios}
                bordered={false}
                showUserTab={true}
                height={50}
                disabled={
                  this.iOperatorType == SendWorkOperatorType.View ||
                  this.iOperatorType == SendWorkOperatorType.Acceptance ||
                  this.isComplete
                }
                value={row.maintenanceUserList}
                onChange={values => {
                  row.maintenanceUserList = values;
                }}
              />
            );
          },
        },
      ];

      if (
        this.iOperatorType != SendWorkOperatorType.Finish ||
        (this.isRailwayHighSpeed && this.iOperatorType == SendWorkOperatorType.Finish)
      ) {
        arr.push({
          title: '验收人',
          dataIndex: 'acceptanceUserList',
          width: 300,
          customRender: (text, row, index) => {
            let isDisable = false;
            if (this.isRailwayHighSpeed == false) {
              isDisable =
                this.iOperatorType == SendWorkOperatorType.View ||
                this.iOperatorType == SendWorkOperatorType.Finish;
            } else {
              isDisable = this.iOperatorType == SendWorkOperatorType.View;
            }
            return (
              <SmSystemUserSelect
                axios={this.axios}
                bordered={false}
                showUserTab={true}
                height={50}
                disabled={isDisable || this.isComplete}
                value={row.acceptanceUserList}
                onChange={values => {
                  if (row.equipmentTestResultList && row.equipmentTestResultList.length > 0) {
                    row.equipmentTestResultList.some(result => {
                      result.checkResult = '合格';
                    });
                  }
                  row.acceptanceUserList = values;
                }}
              />
            );
          },
        });
      }
      return arr;
    },
    isComplete() {
      return (
        this.repairTagKey === RepairTagKeys.RailwayHighSpeed &&
        this.cashFeedBack != PlanState.Complete
      );
    },
  },
  watch: {
    sendingWorkId: {
      handler: function (value, oldValue) {
        if (value) {
          this.iSendingWorkId = value;
          this.initAxios();
        } else {
          this.form.resetFields();
        }
      },
      immediate: true,
    },

    operatorType: {
      handler: function (value, oldValue) {
        if (value) {
          this.iSendingWorkId = this.sendingWorkId;
          this.iOperatorType = value;
          this.initAxios();
        } else {
          this.form.resetFields();
        }
      },
      immediate: true,
    },

    orderState: {
      handler: function (value, oldValue) {
        if (value == OrderState.NaturalDisasterCancel) {
          this.cashFeedBack = PlanState.NaturalDisasterCancel;
        } else if (value == OrderState.OrderCancel) {
          this.cashFeedBack = PlanState.OrderCancel;
        } else if (value == OrderState.OtherReasonCancel) {
          this.cashFeedBack = PlanState.OtherReasonCancel;
        } else {
          this.cashFeedBack = PlanState.Complete;
        }
      },
      immediate: true,
    },
  },
  created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    async initAxios() {
      apiSkyLightPlan = new ApiSkyLightPlan(this.axios);
      apiWorkOrder = new ApiWorkOrder(this.axios);
      apiAccount = new ApiAccount(this.axios);
      await this.refresh();
    },
    async refresh(id) {
      if (id) {
        this.iSendingWorkId = id;
      }
      this.record = null;
      let response = await apiWorkOrder.getDetail({
        id: this.iSendingWorkId,
        repairTagKey: this.repairTagKey,
      });
      if (utils.requestIsSuccess(response)) {
        this.record = response.data;
        //派工单的状态是否为验收状态
        this.isAcceptance = this.record.orderState == SendWorkOperatorType.Acceptance;
        console.log(this.isAcceptance);
        await this.getTicketFinishInfo();
        this.dateRange = this.record
          ? [moment(this.record.startRealityTime), moment(this.record.endRealityTime)]
          : [moment(), moment()];
        this.$nextTick(() => {
          let values = utils.objFilterProps(this.record, formFields);
          this.form.setFieldsValue(values);
        });
        // 解析计划详细列表数据
        this.resolverPlanDetailList2();
      }
    },

    // 获取工作票完成情况
    async getTicketFinishInfo() {
      let ticketFinishInfo = await apiSkyLightPlan.getWorkTicketFinishInfo(
        this.record.skylightPlanId,
      );
      if (utils.requestIsSuccess(ticketFinishInfo)) {
        this.workTicketTotalCount = ticketFinishInfo.data.totalCount;
        this.workTicketFinishCount = ticketFinishInfo.data.finishCount;
      }
    },

    // 解析计划详细列表数据
    resolverPlanDetailList() {
      if (this.record.planDetailList !== null && this.record.planDetailList.length > 0) {
        this.record.planDetailList.some((item, index) => {
          // 遍历设备名称/类别层
          if (item.jobContentEquipmentList === null || item.jobContentEquipmentList.length < 1) {
            let device = { deviceName: item.deviceName, rowSpan: 0 };
            let planDetail = this.initPlanDetailTemplate(`${index}`, device);
            this.planDetailList.push(planDetail);
          } else {
            item.jobContentEquipmentList.some((item2, index2) => {
              // 遍历设备型号/编号层
              if (item2.yearMonthDetailedList === null || item2.yearMonthDetailedList.length < 1) {
                let equipment = { equipmentName: item2.equipmentName, rowSpan: 0 };
                let device2 = { deviceName: item.deviceName, rowSpan: 0 };
                if (index2 === 0) {
                  device2.rowSpan = item.jobContentEquipmentList.length;
                }

                let planDetail = this.initPlanDetailTemplate(
                  `${index}-${index2}`,
                  device2,
                  equipment,
                );
                planDetail.equipmentId = item2.equipmentId;
                this.planDetailList.push(planDetail);
              } else {
                item2.yearMonthDetailedList.some((item3, index3) => {
                  // 遍历年/月表层
                  if (item3.planDetailedList == null || item3.planDetailedList.length < 1) {
                    let yearMonth = { yearMonthPlanType: item3.yearMonthPlanType, rowSpan: 0 };
                    let equipment2 = { equipmentName: item2.equipmentName, rowSpan: 0 };
                    let device3 = { deviceName: item.deviceName, rowSpan: 0 };
                    if (index3 === 0) {
                      equipment2.rowSpan = item2.yearMonthDetailedList.length;

                      if (index2 === 0) {
                        device3.rowSpan =
                          item2.yearMonthDetailedList.length * item.jobContentEquipmentList.length;
                      }
                    }

                    let planDetail = this.initPlanDetailTemplate(
                      `${index}-${index2}-${index3}`,
                      device3,
                      equipment2,
                      yearMonth,
                    );
                    planDetail.equipmentId = item2.equipmentId;
                    this.planDetailList.push(planDetail);
                  } else {
                    item3.planDetailedList.some((item4, index4) => {
                      // 遍历设备测试项结果
                      let yearMonth2 = { yearMonthPlanType: item3.yearMonthPlanType, rowSpan: 0 };
                      let equipment3 = { equipmentName: item2.equipmentName, rowSpan: 0 };
                      let device4 = { deviceName: item.deviceName, rowSpan: 0 };
                      if (index4 === 0) {
                        yearMonth2.rowSpan = item3.planDetailedList.length;

                        if (index3 === 0) {
                          equipment3.rowSpan =
                            item3.planDetailedList.length * item2.yearMonthDetailedList.length;
                          if (index2 === 0) {
                            device4.rowSpan =
                              item3.planDetailedList.length *
                              item2.yearMonthDetailedList.length *
                              item.jobContentEquipmentList.length;
                          }
                        }
                      }
                      let planDetail = this.initPlanDetailTemplate(
                        `${index}-${index2}-${index3}-${index4}`,
                        device4,
                        equipment3,
                        yearMonth2,
                      );

                      planDetail.equipmentId = item2.equipmentId;
                      planDetail.relatedEquipmentId = item4.id; // 管理设备Id
                      planDetail.planDetailedId = item4.planDetailedId;
                      planDetail.number = item4.number;
                      planDetail.workContent = item4.workContent;

                      planDetail.planCount = item4.planCount;
                      planDetail.workCount = item4.workCount;
                      planDetail.maintenanceUserList = item4.maintenanceUserList.map(user => {
                        return {
                          id: user.userId,
                          type: MemberType.User,
                        };
                      }); // 保存检修人Id数组
                      planDetail.acceptanceUserList = item4.acceptanceUserList.map(user => {
                        return {
                          id: user.userId,
                          type: MemberType.User,
                        };
                      }); // 保存验收人Id数组

                      // 初始化验收结果为合格
                      if (item4.equipmentTestResultList.length > 0) {
                        planDetail.equipmentTestResultList = item4.equipmentTestResultList;
                      }

                      this.planDetailList.push(planDetail);
                    });
                  }
                });
              }
            });
          }
        });
      }
    },
    resolverPlanDetailList2() {
      this.planDetailList = [];
      // 遍历设备名称/类别层
      // console.log(this.record.planDetailList);
      if (
        this.record.planDetailList &&
        this.record.planDetailList.length > 0 &&
        this.record.planDetailList != null
      ) {
        this.record.planDetailList.some((item, index) => {
          // 遍历设备型号/编号层
          let deviceRowSpan = 0;
          item.jobContentEquipmentList.map(jobContentEquipment => {
            jobContentEquipment.yearMonthDetailedList.map(yearMonthDetailed => {
              return (deviceRowSpan += yearMonthDetailed.planDetailedList.length);
            });
          });
          item.jobContentEquipmentList.some((item2, index2) => {
            // 遍历年/月表层
            let equipmentRowSpan = 0; // 设备型号/编号合并行数
            item2.yearMonthDetailedList.map(yearMonthDetailed => {
              return (equipmentRowSpan += yearMonthDetailed.planDetailedList.length);
            });
            item2.yearMonthDetailedList.some((item3, index3) => {
              // 遍历详细计划
              item3.planDetailedList.some((item4, index4) => {
                let yearMonth = { yearMonthPlanType: item3.yearMonthPlanType, rowSpan: 0 };
                let equipment = { equipmentName: item2.equipmentName, rowSpan: 0 };
                let device = { deviceName: item.deviceName, rowSpan: 0 };

                if (index4 === 0) yearMonth.rowSpan = item3.planDetailedList.length;
                if (index4 === 0 && index3 === 0) equipment.rowSpan = equipmentRowSpan;
                if (index4 === 0 && index3 === 0 && index2 === 0) device.rowSpan = deviceRowSpan;

                let planDetail = this.initPlanDetailTemplate(
                  `${index}-${index2}-${index3}-${index4}`,
                  device,
                  equipment,
                  yearMonth,
                );

                planDetail.equipmentId = item2.equipmentId;
                planDetail.relatedEquipmentId = item4.id; // 管理设备Id
                planDetail.planDetailedId = item4.planDetailedId;
                planDetail.number = item4.number;
                planDetail.workContent = item4.workContent;
                planDetail.planCount = item4.planCount;
                planDetail.workCount =
                  this.operatorType == SendWorkOperatorType.Finish
                    ? item4.workCount != 0
                      ? item4.workCount
                      : item4.planCount
                    : item4.workCount; //初始化完成数量等于作业数量
                //高铁新增需求
                planDetail.maintenanceUserList =
                  item4.maintenanceUserList.length > 0
                    ? item4.maintenanceUserList.map(user => {
                      return {
                        id: user.userId,
                        type: MemberType.User,
                      };
                    })
                    : null; // 保存检修人Id数组
                planDetail.acceptanceUserList =
                  item4.acceptanceUserList.length > 0
                    ? item4.acceptanceUserList.map(user => {
                      return {
                        id: user.userId,
                        type: MemberType.User,
                      };
                    })
                    : null; // 保存验收人Id数组

                // 初始化验收结果为合格
                planDetail.equipmentTestResultList = item4.equipmentTestResultList;
                // if (this.repairTagKey === RepairTagKeys.RailwayWired) {
                if (planDetail.equipmentTestResultList != null) {
                  planDetail.equipmentTestResultList.map(item => {
                    if (item.predictedValue !== null) {
                      if (this.repairTagKey === RepairTagKeys.RailwayWired) {
                        item.testResult =
                          item.testResult != null ? item.testResult : item.predictedValue[0];
                      } else {
                        item.testResult = item.testResult != null ? item.testResult : '合格';
                      }
                    }
                    if (
                      this.repairTagKey === RepairTagKeys.RailwayWired &&
                      item.testResult === null
                    ) {
                      item.testResult = '完成';
                    }
                    // if (item.testResult !== null) {

                    // }
                  });
                }
                // }
                this.planDetailList.push(planDetail);
              });
            });
          });
        });
      }
    },

    // 初始化计划详细对象
    initPlanDetailTemplate(id, device, equipment, yearMonth) {
      return {
        id: id,
        device: device || {},
        equipmentId: null,
        equipment: equipment || {},
        yearMonth: yearMonth || {},
        relatedEquipmentId: null,
        planDetailedId: null,
        number: 0,
        workContent: 'workContent',
        isShorten: true,
        planCount: 0,
        workCount: 0,
        maintenanceUserList: [],
        acceptanceUserList: [],
        equipmentTestResultList: [],
      };
    },

    //设置时间禁选项
    disabledDate(current) {
      return (
        current <=
        moment(this.record.startRealityTime)
          .subtract(1, 'month')
          .endOf('month') ||
        current >=
        moment(this.record.startRealityTime)
          .add(1, 'month')
          .startOf('month')
      );
    },

    // 组建保存时的设备列表实例集合
    getEquipmentList() {
      let equipmentList = [];
      this.planDetailList.some((item, index) => {
        let equipment = {
          id: item.relatedEquipmentId, //关联设备ID
          planCount: item.planCount,
          workCount: item.workCount,
          repairTagKey: this.repairTagKey,
        };

        //检修人员
        equipment.maintenanceUserList = item.maintenanceUserList
          ? item.maintenanceUserList.map(item => item.id)
          : [];
        if (
          this.record &&
          this.record.workContentType == WorkContentType.OtherPlan &&
          this.cashFeedBack == PlanState.Complete &&
          equipment.maintenanceUserList.length <= 0
        ) {
          this.$message.warning('请添加检修人员');
          return;
        }

        //验收人员
        equipment.acceptanceUserList = item.acceptanceUserList
          ? item.acceptanceUserList.map(item => item.id)
          : [];
        if (
          this.record &&
          this.record.workContentType == WorkContentType.OtherPlan &&
          this.cashFeedBack == PlanState.Complete &&
          equipment.acceptanceUserList.length <= 0
        ) {
          this.$message.warning('请添加验收人员');
          return;
        }
        equipment.equipmentTestResultList =
          item.equipmentTestResultList && item.equipmentTestResultList.length > 0
            ? item.equipmentTestResultList.map(result => {
              return {
                id: result.id,
                testResult: result.testResult,
                checkResult: result.checkResult,
                fileId: result.file ? result.file.id : null,
              };
            })
            : [];

        equipmentList.push(equipment);
      });

      return equipmentList;
    },

    // 一键验收
    async allAcceptanced() {
      let response = await apiAccount.getAppConfig();
      let checkUserId = undefined;

      if (utils.requestIsSuccess(response)) {
        checkUserId = response.data.currentUser.id;
      }
      this.planDetailList.some(item => {
        item.acceptanceUserList = []; //赋空
        item.acceptanceUserList.push({ id: checkUserId, type: MemberType.User });

        item.equipmentTestResultList.some(result => {
          result.checkResult = '合格';
        });
      });
    },

    //一键检修
    allRepair(value) {
      this.maintenanceUserList = value;
      //对表格中的检修人进行赋值
      this.planDetailList.map(item => {
        item.maintenanceUserList = value;
      });
    },

    //一键验收 高铁版使用
    allRepairForHightWay(value) {
      this.repairUserListForHightWay = value;
      //对表格中的检修人进行赋值
      this.planDetailList.map(item => {
        item.acceptanceUserList = value;
        if (item.equipmentTestResultList && item.equipmentTestResultList.length > 0) {
          item.equipmentTestResultList.map(result => {
            result.checkResult = '合格';
          });
        }
      });
    },
    // 保存
    async save(isSave) {
      let response = null;
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let _values = JSON.parse(JSON.stringify(values));
          let equipmentList = this.getEquipmentList();
          if (equipmentList.length <= 0) {
            return;
          }
          if (this.iOperatorType == SendWorkOperatorType.Finish) {
            // 完成逻辑
            let data = {
              id: this.record.id,
              workContentType: this.record.workContentType,
              startRealityTime: this.dateRange[0].format(),
              endRealityTime: this.dateRange[1].format(),
              orderNo: _values.orderNo,
              feedback: _values.feedback,
              equipmentList: equipmentList,
              cashFeedBack: this.cashFeedBack,
              isAcceptance: this.isAcceptance,
            };
            console.log(data);
            response = await apiWorkOrder.finish(isSave, false, data, this.repairTagKey);
          } else if (this.iOperatorType == SendWorkOperatorType.Acceptance) {
            // 验收逻辑
            let data = {
              id: this.record.id,
              workContentType: this.record.workContentType,
              equipmentList: equipmentList,
            };
            response = await apiWorkOrder.acceptance(isSave, data, this.repairTagKey);
          } else if (this.iOperatorType == SendWorkOperatorType.Edit) {
            // 编辑逻辑
            let data = {
              id: this.record.id,
              startRealityTime: this.record.startRealityTime,
              workContentType: this.record.workContentType,
              endRealityTime: this.record.endRealityTime,
              orderNo: _values.orderNo,
              feedback: _values.feedback,
              equipmentList: equipmentList,
            };
            response = await apiWorkOrder.updateDetail(data, this.repairTagKey);
          }

          if (utils.requestIsSuccess(response)) {
            if (isSave) {
              if (
                this.iOperatorType == SendWorkOperatorType.Edit &&
                response.data.finishInfos.length > 0
              ) {
                let content = [];
                const h = this.$createElement;
                let _this = this;
                response.data.finishInfos.map(i => {
                  content.push(<p>{i.content}</p>);
                });
                this.$info({
                  title: '部分完成数量已被自动修改',
                  width: '600px',
                  content: <div>{content}</div>,
                  onOk() {
                    _this.$message.success('保存成功');
                  },
                });
              } else this.$message.success('保存成功');
            } else this.close();
          }
        }
      });
    },

    // 导出
    async export() {
      this.exportLoading = true;
      let response = await apiWorkOrder.export(this.record.id, true, this.repairTagKey);
      if (utils.requestIsSuccess(response)) {
        let fileName = '';
        if (this.repairTagKey == RepairTagKeys.RailwayWired) {
          fileName = '一单两表.xlsx';
        } else if (this.repairTagKey == RepairTagKeys.RailwayHighSpeed) {
          fileName = '检修表.xlsx';
        }
        FileSaver.saveAs(new Blob([response.data], { type: 'application/vnd.ms-excel' }), fileName);
        this.exportLoading = false;
      }
      this.exportLoading = false;
    },

    // 取消
    close() {
      this.$emit('cancel');
    },
    //高铁科一键验收及一键完成设置特定人
    setMember(isWorkLeader) {
      if (isWorkLeader) {
        let workLeader = [];
        workLeader.push({ type: MemberType.User, id: this.record.workLeader.userId });
        this.allRepairForHightWay(workLeader);
      } else {
        let workMemberList = [];
        if (this.record.workMemberList.length > 0) {
          this.record.workMemberList.map(user => {
            workMemberList.push({
              type: MemberType.User,
              id: user.userId,
            });
          });
          this.allRepair(workMemberList);
        }
      }
    },

    //添加测试项
    addTsetWork(record) {
      this.$refs.SmTestWorkModal.add(record);
    },
  },
  render() {
    // scroll={{ x: 'calc(800px + 60%)', y: 600 }}
    let planDetailTable = (
      <a-table
        columns={
          this.record && this.record.workContentType == WorkContentType.OtherPlan
            ? this.otherPlanColumns
            : this.columns
        }
        rowKey={record => record.id}
        data-source={this.planDetailList}
        bordered
        pagination={false}
        scroll={
          this.record && this.record.workContentType !== WorkContentType.OtherPlan
            ? { x: 'calc(~"100% - 40px")', y: 600 }
            : {}
        }
        {...{
          scopedSlots: {
            workContent: (text, record, index) => {
              let isFillContent = false;
              let isExcelInfo = false;
              let isShowAdd = false;
              if (record.equipmentTestResultList && record.equipmentTestResultList.length > 0) {
                record.equipmentTestResultList.map(item => {
                  if (
                    item.predictedValue == null &&
                    item.testType != StandTestType.Excel
                  ) {
                    isShowAdd = true;
                    // return;
                  }
                  if (item.testType === StandTestType.Excel) {
                    isExcelInfo = true;
                  }
                  if (isShowAdd && (item.testResult == null || item.testResult == '')) {
                    isFillContent = true;
                    return;
                  }
                });
              }
              let span = isFillContent && this.repairTagKey == RepairTagKeys.RailwayHighSpeed ? (
                <span style="color:red">(需要手动添加测试结果)</span>
              ) : (
                undefined
              );
              span = isExcelInfo ? <span style="color:red">(需要上传检修记录表)</span> : undefined;
              return this.record && this.record.workContentType === WorkContentType.OtherPlan ? (
                <a-tooltip placement="topLeft">{text}</a-tooltip>
              ) : (
                [
                  <div class="test-result">
                    <div class="test-content">
                      <a-icon
                        type={record.isShorten ? 'down' : 'up'}
                        onClick={() => {
                          record.isShorten = !record.isShorten;
                        }}
                      />
                      <a-tooltip placement="topLeft">
                        <template slot="title">
                          <span>{text}</span>
                        </template>
                        <span class="test-content-span">
                          {text}
                          {span}
                        </span>
                      </a-tooltip>
                      {/* <span class="test-content-span">{text}</span> */}
                    </div>
                    <a-table
                      v-show={!record.isShorten}
                      pagination={false}
                      rowKey={record => record.id}
                      data-source={record.equipmentTestResultList}
                      columns={[
                        {
                          title: '测试项目',
                          dataIndex: 'testName',
                          width: 100,
                          ellipsis: true,
                        },
                        {
                          title: '检修记录',
                          dataIndex: 'testResult',
                          width: 250,
                          customRender: (text, record, index) => {
                            let item;
                            if (record.testType === StandTestType.Excel) {
                              if (record.uploadFile === null) item = <div></div>;
                              else {
                                item = [
                                  <SmFileManageSelect
                                    ref="SmFileManageSelect"
                                    axios={this.axios}
                                    bordered={
                                      this.iOperatorType == SendWorkOperatorType.Finish
                                        ? true
                                        : false
                                    }
                                    multiple={false}
                                    height={40}
                                    disabled
                                    enableDownload={true}
                                    value={record.file ? record.file : {}}
                                    onChange={value => {
                                      record.file = value;
                                    }}
                                  />,
                                  <SmFileUpload
                                    style="margin-top:3px"
                                    ref="uploadFile"  
                                    axios={this.axios}
                                    autoSave
                                    onSelected={async value =>  {
                                      if (record.file == null) {
                                        record.file = value[0];
                                      } else{
                                        record.file.id = value[0].id;
                                      }
                                      await this.$refs.uploadFile.commit(value);
                                    }}
                                  />,
                                ];
                              }
                            } else if (record.predictedValue && record.predictedValue.length > 0) {
                              item = (
                                <a-select
                                  disabled={
                                    this.iOperatorType == SendWorkOperatorType.View ||
                                    this.iOperatorType == SendWorkOperatorType.Acceptance
                                  }
                                  // defaultValue={record.predictedValue[0]}
                                  value={record.testResult}
                                  onChange={value => {
                                    record.testResult = value;
                                  }}
                                >
                                  {record.predictedValue.map(item => {
                                    return (
                                      <a-select-option key={item} value={item}>
                                        {item}
                                      </a-select-option>
                                    );
                                  })}
                                </a-select>
                              );
                            } else {
                              let isShow = false;
                              let max = record.maxRated;
                              let min = record.minRated;

                              let value = parseFloat(record.testResult);
                              if (
                                record.testType == StandTestType.Number &&
                                max != 0 &&
                                min != 0 &&
                                this.repairTagKey == RepairTagKeys.RailwayHighSpeed
                              ) {
                                let num = parseFloat(value);
                                if (num > max || num < min) {
                                  isShow = true;
                                }
                              }

                              let text = isShow ? (
                                <span style="color:rgb(242 84 8);display:block">
                                  输入值与阈值不符
                                  {`(${min}~${max}${record.unit == undefined ? '' : record.unit})`}
                                </span>
                              ) : (
                                ''
                              );

                              item = [
                                <a-input
                                  disabled={
                                    this.iOperatorType == SendWorkOperatorType.View ||
                                    this.iOperatorType == SendWorkOperatorType.Acceptance
                                  }
                                  value={record.testResult}
                                  onInput={event => {
                                    record.testResult = event.target.value;
                                  }}
                                ></a-input>,
                                text,
                              ];
                            }
                            return {
                              children: item,
                            };
                          },
                        },
                        {
                          title: '验收记录',
                          dataIndex: 'checkResult',
                          width: 250,
                          customRender: (text, record, index) => {
                            let isDisable = false;
                            if (this.isRailwayHighSpeed == false) {
                              isDisable =
                                this.iOperatorType == SendWorkOperatorType.View ||
                                this.iOperatorType == SendWorkOperatorType.Finish;
                            } else {
                              isDisable = this.iOperatorType == SendWorkOperatorType.View;
                            }
                            return (
                              <a-select
                                disabled={isDisable}
                                value={record.checkResult}
                                onChange={value => {
                                  record.checkResult = value;
                                }}
                              >
                                <a-select-option value="合格">合格</a-select-option>
                                <a-select-option value="不合格">不合格</a-select-option>
                              </a-select>
                            );
                          },
                        },
                      ]}
                    />
                    {isShowAdd ? (
                      <div class="addStyle">
                        <a-icon
                          type="plus"
                          onClick={() => {
                            this.addTsetWork(record);
                          }}
                          class="plusStyle"
                        />
                      </div>
                    ) : null}
                  </div>,
                ]
              );
            },
          },
        }}
      />
    );
    //高铁办：兑现反馈
    let feedBack = [];
    for (let item in PlanState) {
      if (
        (item === 'Complete' ||
          item === 'NaturalDisasterCancel' ||
          item === 'OrderCancel' ||
          item === 'OtherReasonCancel')
      )
        [
          feedBack.push(
            <a-select-option key={PlanState[item]}>
              {utils.getPlanState(PlanState[item])}
            </a-select-option>,
          ),
        ];
    }

    return (
      <div class="sm-cr-plan-sending-work">
        {/* 表单区 */}
        <a-form form={this.form}>
          <a-row gutter={24}>
            <a-col sm={24} md={12}>
              <a-form-item label="作业组长" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <a-input disabled value={this.record ? this.record.workLeader.userName : ''} />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={12}>
              <a-form-item label="作业成员" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <a-input
                  disabled
                  value={
                    this.record
                      ? this.record.workMemberList.map(user => user.userName).toString()
                      : ''
                  }
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={12}>
              <a-form-item label="驻站联络人员" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <a-input
                  disabled
                  value={
                    this.record
                      ? this.record.stationLiaisonOfficerList.map(user => user.userName).toString()
                      : ''
                  }
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={12}>
              <a-form-item
                label={
                  this.repairTagKey == RepairTagKeys.RailwayHighSpeed
                    ? '现场防护人员'
                    : '现场联系人员'
                }
                label-col={{ span: 4 }}
                wrapper-col={{ span: 20 }}
              >
                <a-input
                  disabled
                  value={
                    this.record
                      ? this.record.fieldGuardList.map(user => user.userName).toString()
                      : ''
                  }
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={12}>
              <a-form-item label="计划作业时间" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <a-range-picker
                  style="width: 100%"
                  allowClear={false}
                  disabled
                  value={[
                    this.record && this.record.startPlanTime
                      ? moment(this.record.startPlanTime, 'YYYY-MM-DD HH:mm:ss')
                      : null,
                    this.record && this.record.endPlanTime
                      ? moment(this.record.endPlanTime, 'YYYY-MM-DD HH:mm:ss')
                      : null,
                  ]}
                  format="YYYY-MM-DD HH:mm:ss"
                  onChange={(date, dateStrings) => {
                    this.record.startPlanTime = dateStrings[0];
                    this.record.endPlanTime = dateStrings[1];
                  }}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={12}>
              <a-form-item label="实际作业时间" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <a-range-picker
                  style="width: 100%"
                  disabled={
                    this.iOperatorType == SendWorkOperatorType.View ||
                    this.iOperatorType == SendWorkOperatorType.Acceptance ||
                    this.isAcceptance
                  }
                  allowClear={false}
                  showTime
                  value={this.dateRange}
                  onChange={value => {
                    this.dateRange = value;
                  }}
                  disabled-date={this.disabledDate}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={12}>
              <a-form-item label="影响范围" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <a-input disabled value={this.record ? this.record.influenceScope : ''} />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={12}>
              <a-form-item label="命令票号" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <a-input
                  disabled={
                    this.iOperatorType == SendWorkOperatorType.View ||
                    this.iOperatorType == SendWorkOperatorType.Acceptance ||
                    this.isAcceptance
                  }
                  v-decorator={[
                    'orderNo',
                    {
                      initialValue: null,
                      rules: [{ required: true, message: '请输入命令票号！' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            {this.repairTagKey == RepairTagKeys.RailwayHighSpeed ? (
              <a-col sm={24} md={12}>
                <a-form-item label="兑现反馈" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                  <a-select
                    value={this.cashFeedBack}
                    onChange={value => {
                      this.cashFeedBack = value;
                    }}
                    disabled={
                      this.iOperatorType == SendWorkOperatorType.View ||
                      this.iOperatorType == SendWorkOperatorType.Acceptance
                    }
                  >
                    {feedBack}
                  </a-select>
                </a-form-item>
              </a-col>
            ) : (
              undefined
            )}

            <a-col sm={24} md={24}>
              <a-form-item label={!this.isComplete ? "完成情况" : "取消原因"} label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                <a-textarea
                  disabled={
                    this.iOperatorType == SendWorkOperatorType.View ||
                    this.iOperatorType == SendWorkOperatorType.Acceptance ||
                    this.isAcceptance
                  }
                  v-decorator={[
                    'feedback',
                    {
                      initialValue: null,
                      rules: [{ required: true, message: !this.isComplete ? '请输入完成情况!' : '请输入取消原因' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={24}>
              <a-form-item label="计划内容" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                {/* 高铁科使用的验收方式 */}
                {this.isRailwayHighSpeed && this.iOperatorType == SendWorkOperatorType.Finish ? (
                  <a-button
                    type="primary"
                    style="float: right;margin-bottom: 10px;z-index: 1;"
                    disabled={this.isComplete}
                    onClick={() => {
                      this.setMember(true);
                      // this.userSelectVisibleForHightWay = true;
                    }}
                  >
                    一键验收
                  </a-button>
                ) : null}

                <a-button
                  type="primary"
                  style="float: right;margin-bottom: 10px;z-index: 1;"
                  v-show={this.iOperatorType == SendWorkOperatorType.Acceptance}
                  onClick={() => {
                    this.allAcceptanced();
                  }}
                >
                  一键验收
                </a-button>

                <a-button
                  style=""
                  type="primary"
                  style="float: right;margin-bottom: 10px;z-index: 1;margin-right:20px;"
                  v-show={this.iOperatorType == SendWorkOperatorType.Finish}
                  disabled={this.isComplete || this.isAcceptance}
                  onClick={() => {
                    this.isRailwayHighSpeed && this.iOperatorType !== SendWorkOperatorType.Finish
                      ? (this.userSelectVisible = true)
                      : this.setMember(false);
                  }}
                >
                  一键完成
                </a-button>
                {this.isRailwayHighSpeed ? (
                  <a-tooltip placement="bottom">
                    <template slot="title">
                      <span>
                        {'完成:' + this.workTicketFinishCount + ' 总:' + this.workTicketTotalCount}
                      </span>
                    </template>
                    <a-button
                      type="primary"
                      style="float: right;margin-bottom: 10px; margin-right:20px; z-index: 1;"
                      disabled={this.isComplete}
                      onClick={() => {
                        if (this.workTicketTotalCount == 0) {
                          this.$message.info('无工作票');
                          return;
                        }
                        this.record.startRealityTime = this.dateRange[0].format();
                        this.record.endRealityTime = this.dateRange[1].format();
                        if (this.iOperatorType == SendWorkOperatorType.View)
                          this.$refs.SmCrPlanSentingWorkFinishTicketModal.view(this.record);
                        else this.$refs.SmCrPlanSentingWorkFinishTicketModal.finish(this.record);
                      }}
                    >
                      工作票 {this.workTicketFinishCount + '(' + this.workTicketTotalCount + ')'}
                    </a-button>
                  </a-tooltip>
                ) : null}
                <SmSystemAllUserSelect
                  axios={this.axios}
                  visible={this.userSelectVisible}
                  showUserTab={true}
                  selected={this.maintenanceUserList}
                  onChange={iValue => {
                    this.userSelectVisible = iValue;
                  }}
                  onOk={value => {
                    this.allRepair(value);
                  }}
                />
                {/* 高铁科使用的一键验收用户选择组件 */}
                {this.isRailwayHighSpeed ? (
                  <SmSystemAllUserSelect
                    axios={this.axios}
                    visible={this.userSelectVisibleForHightWay}
                    showUserTab={true}
                    selected={this.repairUserListForHightWay}
                    onChange={iValue => {
                      this.userSelectVisibleForHightWay = iValue;
                    }}
                    onOk={value => {
                      this.allRepairForHightWay(value);
                    }}
                  />
                ) : null}

                {/* 计划内容展示区 */}
                {planDetailTable}
              </a-form-item>
            </a-col>
            {/* <a-col sm={24} md={12}>
              <a-form-item label="检修人">
                <SmSystemUserSelect
                  axios={this.axios}
                  disabled={
                    this.iOperatorType == SendWorkOperatorType.View ||
                    this.iOperatorType == SendWorkOperatorType.Acceptance
                  }
                  height={40}
                  showUserTab={true}
                  value={this.formData.workLeader}
                  onChange={values => {
                    this.formData.workLeader = values;
                  }}
                />
              </a-form-item>
            </a-col>

            <a-col sm={24} md={12}>
              <a-form-item label="验收人">
                <SmSystemUserSelect
                  axios={this.axios}
                  disabled={
                    this.iOperatorType == SendWorkOperatorType.View ||
                    this.iOperatorType == SendWorkOperatorType.Acceptance
                  }
                  height={40}
                  showUserTab={true}
                  value={this.formData.workMemberList}
                  onChange={values => {
                    this.formData.workMemberList = values;
                  }}
                />
              </a-form-item>
            </a-col> */}
          </a-row>
        </a-form>
        <div style="float: right;padding-top: 10px;">
          <a-button
            type="primary"
            style="margin-right: 20px"
            v-show={this.iOperatorType != SendWorkOperatorType.View}
            disabled={this.isComplete}
            onClick={() => {
              this.save(true);
            }}
          >
            保存
          </a-button>
          <a-button
            type="primary"
            style="margin-right: 20px"
            v-show={
              this.iOperatorType == SendWorkOperatorType.Finish ||
              this.iOperatorType == SendWorkOperatorType.Acceptance
            }
            onClick={() => {
              if (
                this.isRailwayHighSpeed &&
                this.workTicketFinishCount < this.workTicketTotalCount &&
                !this.isComplete
              ) {
                this.$message.error('有未完成的工作票，无法提交');
                return;
              }
              this.save(false);
            }}
          >
            提交
          </a-button>
          <a-button
            type="primary"
            style="margin-right: 20px"
            // v-show={this.iOperatorType != SendWorkOperatorType.View}
            onClick={this.export}
            loading={this.exportLoading}
          >
            导出检修表
          </a-button>
          <a-button onClick={this.close}>取消</a-button>
        </div>
        <SmCrPlanSentingWorkFinishTicketModal
          ref="SmCrPlanSentingWorkFinishTicketModal"
          axios={this.axios}
          repairTagKey={this.repairTagKey}
          onSuccess={async () => {
            await this.getTicketFinishInfo();
          }}
        />
        <SmTestWorkModal
          axios={this.axios}
          ref="SmTestWorkModal"
          sendingWorkId={this.sendingWorkId}
          disabled={
            this.iOperatorType == SendWorkOperatorType.View ||
            this.iOperatorType == SendWorkOperatorType.Acceptance
          }
        />
      </div>
    );
  },
};
