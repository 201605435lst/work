import ApiWorkOrder from '../../sm-api/sm-cr-plan/WorkOrder';
import * as utils from '../../_utils/utils';
import { requestIsSuccess, getDateReportTypeTitle } from '../../_utils/utils';
import { PlanState, RepairTagKeys, OrderState } from '../../_utils/enum';
import moment from 'moment';
import {
  ModalStatus,
  YearMonthPlanType,
  StandTestType,
  MemberType,
  WorkContentType,
} from '../../_utils/enum';
import SmFileManageSelect from '../../sm-file/sm-file-manage-select';
import SmSystemUserSelect from '../../sm-system/sm-system-member-select';
import SmTestWorkModal from '../sm-cr-plan-sending-work/SmTestWorkModal';
import SmFileUpload from '../../sm-file/sm-file-upload/SmFileUpload';

let apiWorkOrder = new ApiWorkOrder();
const formFields = ['startRealityTime', 'endRealityTime'];
export default {
  name: 'SmCrPlanOtherWorkModal',
  props: {
    axios: { type: Function, default: null },
    planDate: { type: String, default: '' },
    repairTagKey: { type: String, default: null }, //维修项标签
    orderState: { type: Number, default: OrderState.Complete }, //计划状态
  },
  data() {
    return {
      form: this.$form.createForm(this, {}),
      otherWork: null,
      status: ModalStatus.Hide,
      startTime: null,
      endTime: null,
      flatPlanDetails: [],
      planDetailList: [],
      record: null, //当前选中的维修项
      workUsers: [],
      cashFeedBack: PlanState.Complete,
      cancelReson: null,
      sendingWorkId: null,
    };
  },

  computed: {
    visible() {
      return this.status !== ModalStatus.Hide;
    },

    title() {
      return this.status == ModalStatus.Edit ? '完成录入' : '详情';
    },

    columns() {
      return [
        {
          title: '设备名称/类别',
          dataIndex: 'deviceName',
          ellipsis: true,
          width: 140,
          customRender: (text, record, index) => {
            return {
              children: text,
              attrs: {
                rowSpan: record.typeRowSpan,
              },
            };
          },
        },
        {
          title: '年/月表',
          dataIndex: 'yearMonthPlanType',
          ellipsis: true,
          width: 90,
          customRender: (text, record, index) => {
            return {
              children: `${getDateReportTypeTitle(record.yearMonthPlanType)}`,
              attrs: {
                rowSpan: record.monthYearRowSpan,
              },
            };
          },
        },
        {
          title: '序号',
          dataIndex: 'number',
          scopedSlots: { customRender: 'number' },
          ellipsis: true,
          width: 90,
          customRender: (text, record, index) => {
            return {
              children: text,
            };
          },
        },
        {
          title: '工作内容',
          width: 500,
          ellipsis: true,
          dataIndex: 'workContent',
          customRender: (text, record, index) => {
            //添加手动添加测试项提示
            let isFillContent = false;
            let isShowAdd = false;
            let isExcelInfo = false;
            let isNumberInfo = false;
            if (record.equipmentTestResultList && record.equipmentTestResultList.length > 0) {
              record.equipmentTestResultList.map(item => {
                if (item.predictedValue == null && item.testType != StandTestType.Excel) {
                  isShowAdd = true;
                  // return;
                }
                if (isShowAdd && (item.testResult == null || item.testResult == '')) {
                  isFillContent = true;
                  return;
                }
                if (item.testType === StandTestType.Excel) {
                  isExcelInfo = true;
                }
                if (item.testType === StandTestType.Number) {
                  isNumberInfo = true;
                }
              });
            }
            let span = isFillContent && this.repairTagKey === RepairTagKeys.RailwayHighSpeed ? (
              <span style="color:red">(需要手动添加测试结果)</span>
            ) : (
              undefined
            );
            span = isExcelInfo ? <span style="color:red">(需要上传检修记录表)</span> : undefined;
            let numberSpan = isNumberInfo && this.repairTagKey !== RepairTagKeys.RailwayHighSpeed ? <span style="color:red">(需手动输入数字测试值)</span> : undefined;
            return {
              children: (
                <div>
                  <span style="display: flex; align-items: center;">
                    <a-icon
                      style="margin-right: 10px;"
                      type={record.isShow ? 'up' : 'down'}
                      onClick={() => {
                        record.isShow = !record.isShow;
                      }}
                    />
                    <a-tooltip placement="topLeft" title={record.workContent}>
                      <div style="width:265px; overflow: hidden; text-overflow: ellipsis;">
                        {record.workContent}
                        {span}
                        {numberSpan}
                      </div>
                    </a-tooltip>
                  </span>
                  <a-table
                    v-show={record.isShow}
                    pagination={false}
                    data-source={record.equipmentTestResultList}
                    rowKey={record => record.id}
                    columns={[
                      {
                        title: '测试项目',
                        dataIndex: 'testName',
                        ellipsis: true,
                        width: 140,
                      },
                      {
                        title: '检修记录',
                        dataIndex: 'testResult',
                        customRender: (text, record, index) => {
                          let item;
                          if (record.testType === StandTestType.Excel) {
                            item = [
                              <SmFileManageSelect
                                ref="SmFileManageSelect"
                                axios={this.axios}
                                multiple={false}
                                height={40}
                                disabled={true}
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
                                onSelected={value => {
                                  this.uploadFile(value);
                                  record.file = value[0];
                                  record.fileId = value[0].id;
                                }}
                              />,
                            ];
                          } else if (record.predictedValue && record.predictedValue.length > 0) {
                            item = (
                              <a-select
                                disabled={this.status == ModalStatus.View}
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
                              <span style="color:rgb(242 84 8)">
                                输入值与阈值不符
                                {`(${min}~${max}${record.unit == undefined ? '' : record.unit})`}
                              </span>
                            ) : (
                              ''
                            );

                            item = [
                              <a-input
                                value={record.testResult}
                                disabled={this.status == ModalStatus.View}
                                onChange={event => {
                                  record.testResult = event.target.value;
                                }}
                              ></a-input>,
                              <div>{text}</div>,
                            ];
                          }
                          return {
                            children: item,
                          };
                        },
                      },
                    ]}
                  ></a-table>
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
                </div>
              ),
            };
          },
        },
        {
          title: '作业数量',
          dataIndex: 'planCount',
          scopedSlots: { customRender: 'planCount' },
          width: 90,
        },
        {
          title: '完成数量',
          dataIndex: 'workCount',
          scopedSlots: { customRender: 'workCount' },
          width: 120,
        },
        {
          title: '作业人员',
          dataIndex: 'maintenanceUserList',
          scopedSlots: { customRender: 'maintenanceUserList' },
        },
      ];
    },
    otherPlanColumns() {
      return [
        {
          title: '工作内容',
          width: 500,
          ellipsis: true,
          dataIndex: 'workContent',
          customRender: (text, record, index) => {
            return {
              children:
                this.otherWork && this.otherWork.workContentType === WorkContentType.OtherPlan ? (
                  <a-tooltip placement="topLeft" title={record.workContent}>
                    <div style="width:265px; overflow: hidden; text-overflow: ellipsis;">
                      {record.workContent}
                    </div>
                  </a-tooltip>
                ) : (
                  <div>
                    <span style="display: flex; align-items: center;">
                      <a-icon
                        style="margin-right: 10px;"
                        type={record.isShow ? 'up' : 'down'}
                        onClick={() => {
                          record.isShow = !record.isShow;
                        }}
                      />
                      <a-tooltip placement="topLeft" title={record.workContent}>
                        <div style="width:265px; overflow: hidden; text-overflow: ellipsis;">
                          {record.workContent}
                        </div>
                      </a-tooltip>
                    </span>
                    <a-table
                      v-show={record.isShow}
                      pagination={false}
                      data-source={record.equipmentTestResultList}
                      rowKey={record => record.id}
                      columns={[
                        {
                          title: '测试项目',
                          dataIndex: 'testName',
                          ellipsis: true,
                          width: 140,
                        },
                        {
                          title: '检修记录',
                          dataIndex: 'testResult',
                          customRender: (text, record, index) => {
                            let item;
                            if (record.testType === StandTestType.Excel) {
                              item = (
                                <SmFileManageSelect
                                  ref="SmFileManageSelect"
                                  axios={this.axios}
                                  multiple={false}
                                  height={40}
                                  disabled
                                  enableDownload={true}
                                  value={record.file ? record.file : {}}
                                  onChange={value => {
                                    record.file = value;
                                  }}
                                />
                              );

                            } else if (record.predictedValue && record.predictedValue.length > 0) {
                              item = (
                                <a-select
                                  disabled={this.status == ModalStatus.View}
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
                              item = (
                                <a-input
                                  value={record.testResult}
                                  disabled={this.status == ModalStatus.View}
                                  onChange={event => {
                                    record.testResult = event.target.value;
                                  }}
                                ></a-input>
                              );
                            }
                            return {
                              children: item,
                            };
                          },
                        },
                      ]}
                    ></a-table>
                  </div>
                ),
            };
          },
        },
        {
          title: '作业人员',
          dataIndex: 'maintenanceUserList',
          scopedSlots: { customRender: 'maintenanceUserList' },
        },
      ];
    },
    feedBack() {
      return this.cashFeedBack != PlanState.Complete;
    },
  },

  watch: {
    orderState: {
      handler: function (value, oldValue) {
        // console.log(value);
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
  },

  methods: {
    initAxios() {
      apiWorkOrder = new ApiWorkOrder(this.axios);
    },

    edit(record) {
      this.otherWork = record;
      this.sendingWorkId = record.id;
      // console.log(record);
      this.status = ModalStatus.Edit;
      this.refresh();
    },
    view(record) {
      this.otherWork = record;
      this.sendingWorkId = record.id;
      this.status = ModalStatus.View;
      this.refresh();
    },

    //初始化作业表单
    async refresh() {
      let response = await apiWorkOrder.getDetail({
        id: this.otherWork.id,
        repairTagKey: this.repairTagKey,
      });
      let _otherWork;
      if (requestIsSuccess(response)) {
        _otherWork = response.data;
        // console.log(_otherWork);
        this.planDetailList = _otherWork.planDetailList;
        this.otherWork = _otherWork;
        // 计划详情数据数组扁平化处理
        let planDetailList = [];
        this.planDetailList.map((planDetail, index1) => {
          planDetail.jobContentEquipmentList.map((jobContentEquipment, index2) => {
            jobContentEquipment.yearMonthDetailedList.sort(
              (a, b) => a.yearMonthPlanType - b.yearMonthPlanType,
            );

            // 找到所有年表
            let allYearDetailedList = jobContentEquipment.yearMonthDetailedList.filter(
              item => item.yearMonthPlanType === YearMonthPlanType.Year,
            );
            // 找到所有月表
            let allMonthDetailedList = jobContentEquipment.yearMonthDetailedList.filter(
              item => item.yearMonthPlanType === YearMonthPlanType.Month,
            );

            jobContentEquipment.yearMonthDetailedList.map((yearMonthDetailed, index3) => {
              yearMonthDetailed.planDetailedList.map((planDetailed, index4) => {
                let item = {
                  id: index1 + '_' + index2 + '_' + index3 + '_' + index4,
                  ...planDetail,
                  ...jobContentEquipment,
                  ...yearMonthDetailed,
                  ...planDetailed,
                  typeRowSpan: 0,
                  nameRowSpan: 0,
                  monthYearRowSpan: 0,
                  maintenanceUserListIds: planDetailed.maintenanceUserList
                    ? planDetailed.maintenanceUserList.map(item => {
                      return {
                        id: item.userId,
                        type: MemberType.User,
                      };
                    })
                    : [],
                  isShow: false,
                };
                if (index2 === 0 && index3 === 0 && index4 === 0) {
                  planDetail.jobContentEquipmentList.map(jobContentEquipment => {
                    jobContentEquipment.yearMonthDetailedList.map(yearMonthDetailed => {
                      item.typeRowSpan += yearMonthDetailed.planDetailedList.length;
                    });
                  });
                }

                if (index3 === 0 && index4 === 0) {
                  jobContentEquipment.yearMonthDetailedList.map(yearMonthDetailed => {
                    return (item.nameRowSpan += yearMonthDetailed.planDetailedList.length);
                  });

                  allYearDetailedList.map(_item => {
                    item.monthYearRowSpan += _item.planDetailedList.length;
                  });
                }

                if (index3 === allYearDetailedList.length && index4 === 0) {
                  allMonthDetailedList.map(_item => {
                    item.monthYearRowSpan += _item.planDetailedList.length;
                  });
                }

                planDetailList.push(item);
              });
            });
          });
        });
        this.flatPlanDetails = planDetailList;
        // console.log(this.flatPlanDetails);
        this.$nextTick(() => {
          let values = utils.objFilterProps(_otherWork, formFields);
          values.startRealityTime = moment(values.startRealityTime);
          values.endRealityTime = moment(values.endRealityTime);
          this.form.setFieldsValue(values);
        });
      }
    },

    close() {
      this.status = ModalStatus.Hide;
      this.form.resetFields();
      this.record = null;
      this.workUsers = [];
      this.flatPlanDetails = null;
    },

    //一键完成操作
    quicklyFish() {
      this.flatPlanDetails.map(item => {
        item.workCount = item.planCount;
        if (item.equipmentTestResultList && item.equipmentTestResultList.length > 0) {
          item.equipmentTestResultList.map(_item => {
            if (_item.testType === StandTestType.String) {
              _item.testResult = '合格';
            } else if (
              _item.testType === StandTestType.Number &&
              _item.predictedValue &&
              _item.predictedValue.length > 0
            ) {
              _item.testResult = _item.predictedValue[0];
            }
          });
        }
      });
    },

    //设置时间禁选项
    startTimeDisabledDate(current) {
      return (
        current <=
        moment(this.otherWork.startRealityTime)
          .subtract(1, 'month')
          .endOf('month') ||
        current >=
        moment(this.otherWork.startRealityTime)
          .add(1, 'month')
          .startOf('month')
      );
    },

    endTimeDisabledDate(current) {
      return (
        current <=
        moment(this.otherWork.endRealityTime)
          .subtract(1, 'month')
          .endOf('month') ||
        current >=
        moment(this.otherWork.endRealityTime)
          .add(1, 'month')
          .startOf('month')
      );
    },

    //提交数据
    ok() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let planDetailList = JSON.parse(JSON.stringify(this.flatPlanDetails));
          let data;
          data = {
            id: this.otherWork.id,
            startRealityTime: moment(values.startRealityTime)
              .utc()
              .format(),
            endRealityTime: moment(values.endRealityTime)
              .utc()
              .format(),
            orderNo: this.otherWork.orderNo,
            feedback: this.otherWork.feedback,
            workContentType: this.otherWork.workContentType,
            cashFeedBack: this.cashFeedBack,
            equipmentList: planDetailList.map(item => {
              return (item = {
                id: item.id,
                planCount: item.planCount,
                workCount: item.workCount,
                maintenanceUserList:
                  item.maintenanceUserListIds && item.maintenanceUserListIds.length > 0
                    ? item.maintenanceUserListIds.map(item => item.id)
                    : [],
                equipmentTestResultList:
                  item.equipmentTestResultList && item.equipmentTestResultList.length > 0
                    ? item.equipmentTestResultList.map(_item => {
                      return (_item = {
                        id: _item.id,
                        testResult: _item.testResult,
                        fileId: _item.file ? _item.file.id : null,
                      });
                    })
                    : [],
              });
            }),
          };
          if (
            this.otherWork &&
            this.otherWork.workContentType === WorkContentType.OtherPlan &&
            data.equipmentList[0].maintenanceUserList.length <= 0 &&
            this.cashFeedBack === PlanState.Complete
          ) {
            this.$message.warning('请添加作业人员');
            return;
          }
          //有线科需补录之前计表本，故编辑模式情况也可保存
          if (this.status === ModalStatus.Edit || this.status == ModalStatus.View ) {
            let response = await apiWorkOrder.finish(true, true, data, this.repairTagKey);
            if (requestIsSuccess(response)) {
              this.$message.success('操作成功');
              this.$emit('success');
              this.close();
            }
          } else {
            this.close();
          }
        }
      });
    },

    //添加测试项
    addTsetWork(record) {
      this.$refs.SmTestWorkModal.add(record);
    },

    //上传文件执行实时更新
    async uploadFile(files) {
      await this.$refs.uploadFile.commit(files);
    },
  },
  render() {
    //高铁办：兑现反馈
    let feedBack = [];
    for (let item in PlanState) {
      if (
        item === 'Complete' ||
        item === 'NaturalDisasterCancel' ||
        item === 'OrderCancel' ||
        item === 'OtherReasonCancel'
      ) {
        feedBack.push(
          <a-select-option key={PlanState[item]}>
            {utils.getPlanState(PlanState[item])}
          </a-select-option>,
        );
      }
    }
    return (
      <div>
        <a-modal
          class="sm-cr-plan-other-work-modal"
          title={this.title}
          destroyOnClose={true}
          visible={this.visible}
          onCancel={this.close}
          onOk={this.ok}
          ok-text="保存"
          width={'70%'}
        >
          <a-form form={this.form}>
            <a-row gutter={24}>
              <a-col sm={24} md={12}>
                <a-form-item
                  label="实际开始时间"
                  label-col={{ span: 5 }}
                  wrapper-col={{ span: 19 }}
                >
                  <a-date-picker
                    style="width: 100%;"
                    disabled={this.status == ModalStatus.View}
                    show-time
                    disabledDate={this.startTimeDisabledDate}
                    v-decorator={[
                      'startRealityTime',
                      {
                        initialValue: moment(),
                        rules: [{ required: true, message: '请选择开始时间' }],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>

              <a-col sm={24} md={12}>
                <a-form-item
                  label="实际结束时间"
                  label-col={{ span: 5 }}
                  wrapper-col={{ span: 19 }}
                >
                  <a-date-picker
                    style="width: 100%;"
                    disabled={this.status == ModalStatus.View}
                    disabledDate={this.endTimeDisabledDate}
                    show-time
                    v-decorator={[
                      'endRealityTime',
                      {
                        initialValue: moment(),
                        rules: [{ required: true, message: '请选择结束时间' }],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              {this.otherWork && this.otherWork.workContentType !== WorkContentType.OtherPlan ? (
                <a-col sm={24} md={12}>
                  <a-form-item label="作业人员" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                    <SmSystemUserSelect
                      axios={this.axios}
                      showUserTab={true}
                      height={50}
                      disabled={this.status == ModalStatus.View || this.feedBack}
                      value={this.workUsers}
                      onChange={values => {
                        this.workUsers = values;
                        this.flatPlanDetails.map(item => {
                          item.maintenanceUserListIds = values;
                        });
                      }}
                    />
                  </a-form-item>
                </a-col>
              ) : (
                undefined
              )}

              {this.repairTagKey == RepairTagKeys.RailwayHighSpeed ? (
                <a-col sm={24} md={12}>
                  <a-form-item label="兑现反馈" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                    <a-select
                      value={this.cashFeedBack}
                      onChange={vallue => {
                        this.cashFeedBack = vallue;
                      }}
                      disabled={this.status == ModalStatus.View}
                    >
                      {feedBack}
                    </a-select>
                  </a-form-item>
                </a-col>
              ) : (
                undefined
              )}
              {this.feedBack ? (
                <a-col sm={24} md={12}>
                  <a-form-item label="取消原因" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                    <a-input
                      type="textarea"
                      value={this.otherWork.feedback}
                      disabled={this.status == ModalStatus.View}
                      onChange={value => {
                        this.otherWork.feedback = value.target.value;
                      }}
                    ></a-input>
                  </a-form-item>
                </a-col>
              ) : (
                undefined
              )}

              {this.status === ModalStatus.View ||
                (this.otherWork && this.otherWork.workContentType === WorkContentType.OtherPlan) ||
                this.feedBack ? (
                  undefined
                ) : (
                  <a-col sm={24} md={12}>
                    <a-form-item label-col={{ span: 6 }} wrapper-col={{ span: 18, offset: 19 }}>
                      <a-button
                        style="margin-right:20px;"
                        type="primary"
                        onClick={this.quicklyFish}
                        disabled={this.feedBack}
                      >
                      一键完成
                      </a-button>
                    </a-form-item>
                  </a-col>
                )}

              <a-col sm={24} md={24}>
                <a-table
                  columns={
                    this.otherWork && this.otherWork.workContentType !== WorkContentType.OtherPlan
                      ? this.columns
                      : this.otherPlanColumns
                  }
                  dataSource={this.flatPlanDetails}
                  rowKey={record => record.id}
                  scroll={{ y: 500 }}
                  pagination={false}
                  bordered
                  {...{
                    scopedSlots: {
                      planCount: (text, record, index) => {
                        return text;
                      },
                      workCount: (text, record, index) => {
                        return (
                          <a-input-number
                            disabled={this.status == ModalStatus.View || this.feedBack}
                            style="width: 100%;"
                            precision={3}
                            min={0}
                            max={record.planCount}
                            value={this.cashFeedBack == PlanState.Complete ? record.workCount : 0}
                            onChange={value => {
                              record.workCount = value ? value : 0;
                            }}
                          />
                        );
                      },
                      maintenanceUserList: (text, record, index) => {
                        return (
                          <SmSystemUserSelect
                            axios={this.axios}
                            bordered={false}
                            showUserTab={true}
                            value={
                              this.cashFeedBack == PlanState.Complete
                                ? record.maintenanceUserListIds
                                : []
                            }
                            disabled={this.status == ModalStatus.View || this.feedBack}
                            onChange={values => {
                              record.maintenanceUserListIds = values;
                            }}
                          />
                        );
                      },
                    },
                  }}
                ></a-table>
              </a-col>
            </a-row>
          </a-form>
        </a-modal>
        <SmTestWorkModal
          axios={this.axios}
          ref="SmTestWorkModal"
          sendingWorkId={this.sendingWorkId}
          disabled={this.status == ModalStatus.View || this.feedBack}
          isShow={false}
        />
      </div>
    );
  },
};
