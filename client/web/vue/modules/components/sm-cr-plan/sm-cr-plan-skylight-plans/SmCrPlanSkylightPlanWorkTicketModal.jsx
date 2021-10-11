import ApiMaintenanceWork from '../../sm-api/sm-cr-plan/MaintenanceWork';
import { requestIsSuccess, getPlanState, getRepairLevelTitle, vIf, vP, CreateGuid } from '../../_utils/utils';
import SmCrPlanSkylightAddWorkTicketModal from './SmCrPlanSkylightAddWorkTicketModal';
import ApiSkylightPlan from '../../sm-api/sm-cr-plan/SkyLightPlan';
import { pagination as paginationConfig, tips as tipsConfig, form as formConfig } from '../../_utils/config';
import SmFileManageSelect from '../../sm-file/sm-file-manage-select';
// import SmCrPlanSkylightEditModal from './SmCrPlanSkylightEditModal';

let apiMaintenanceWork = new ApiMaintenanceWork();
let apiSkylightPlan = new ApiSkylightPlan();
export default {
  name: "SmCrPlanSkylightPlanWorkTicketModal",
  props: {
    axios: { type: Function, default: null },
    repairTagKey: { type: String, default: null }, //维修项标签
    repeatCheck: { type: Boolean, default: false },//是否为复检
    // repaireId:{type:String,default:null},
  },
  data() {
    return {
      visible: false,
      loading: false,
      submitLoading: false,
      maintenanceWorks: [],
      _workflowId: null,
      onButtonProps: {
        props: {
          disabled: true,
        },
      },
      canSubmit: true,//是否可以提交审批
      form: {},
      workTicketId: null,
      skylightPlanId: null,
      relatedFiles: [],   //维修计划关联的方案文件
      repairLevel: 1,
      contentVisible: false,
      opinion:null,
    };

  },
  computed: {
    columns() {
      return [
        {
          title: '计划号',
          dataIndex: 'planIndex',
          // fixed: 'left',
          width: 75,
        },
        {
          title: '线别',
          width: 61,
          ellipsis: true,
          dataIndex: 'railway',
        },
        {
          title: '等级',
          ellipsis: true,
          width: 61,
          dataIndex: 'level',
          scopedSlots: { customRender: 'level' },
        },
        {
          title: '行别',
          ellipsis: true,
          width: 61,
          dataIndex: 'railwayType',
        },
        {
          title: '维修项目',
          ellipsis: true,
          width: 89,
          dataIndex: 'maintenanceProject',
        },
        {
          title: '类型',
          width: 110,
          ellipsis: true,
          dataIndex: 'maintenanceType',
        },
        {
          title: '维修地点',
          ellipsis: true,
          width: 89,
          dataIndex: 'maintenanceLocation',
        },
        {
          title: '维修日期',
          width: 110,
          dataIndex: 'repaireDate',
        },
        {
          title: '维修时间',
          ellipsis: true,
          width: 110,
          dataIndex: 'repaireTime',
        },
        {
          title: '维修内容及影响范围',
          ellipsis: true,
          dataIndex: 'incidence',
          width: 160,
        },
        {
          title: '路用列车信息',
          dataIndex: 'trainInfo',
          width: 117,
          scopedSlots: { customRender: 'planState' },
        },
        {
          title: '作业单位及负责人',
          dataIndex: 'workOrgAndDutyPerson',
          width: 146,
        },
        {
          title: '配合单位',
          width: 89,
          dataIndex: 'cooperationUnit',
        },
        {
          title: '签收单位',
          ellipsis: true,
          dataIndex: 'signOrganization',
        },
        {
          title: '初审部门',
          ellipsis: true,
          dataIndex: 'firstTrial',
        },
        {
          width: 61,
          title: '备注',
          dataIndex: 'remark',
        },
        {
          title: '操作',
          width: 140,
          dataIndex: 'operations',
          // fixed: 'right',
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
    innerColumns() {
      return [
        {
          title: '命令号',
          dataIndex: 'orderNumber',
          // width: 75,
        },
        {
          title: '作业名称',
          dataIndex: 'workTitle',
          ellipsis: true,
          // width: 121,
        },
        {
          title: '作业地点',
          dataIndex: 'workPlace',
          ellipsis: true,
          // width: 150,
        },
        {
          title: '施工维修等级',
          dataIndex: 'repairLevel',
          scopedSlots: { customRender: 'repairLevel' },
          ellipsis: true,
          // width: 150,
        },
        {
          title: '作业内容',
          dataIndex: 'workContent',
          ellipsis: true,
          // width: 187,
        },
        {
          title: '影响范围',
          dataIndex: 'influenceRange',
          ellipsis: true,
        },
        {
          // width: 200,
          title: '制表人',
          dataIndex: 'paperMaker',
          scopedSlots: { customRender: 'paperMaker' },
          ellipsis: true,
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 140,
          // fixed: 'right',
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
    relatedFilescolumns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          width: 75,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '方案文件',
          dataIndex: 'content',
          scopedSlots: { customRender: 'content' },
        },
        {
          title: '封面文件',
          dataIndex: 'cover',
          scopedSlots: { customRender: 'cover' },
        },
        {
          title: '操作',
          dataIndex: 'operations',
          width: 70,
          scopedSlots: { customRender: 'operations' },
        },
      ];
    },
    // canSubmit() {
    //   return this.workTicketsList.length > 0;
    // },
  },
  watch: {
    canSubmit: {
      handler: function (value, oldValue) {
        if (value) {
          this.canSubmit = value;
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
    initAxios() {
      apiMaintenanceWork = new ApiMaintenanceWork(this.axios);
      apiSkylightPlan = new ApiSkylightPlan(this.axios);
    },
    //查看维修计划
    view(workflowId) {
      this.visible = true;
      this.refresh(workflowId);
    },
    add(record) {
      this.repairLevel = record.repairLevel;
      this.visible = true;
      this.refresh(record.id);
    },
    async refresh(workflowId) {
      this._workflowId = workflowId;
      let response = await apiMaintenanceWork.getMaintenanceWork({ workflowId: workflowId });
      if (requestIsSuccess(response) && response.data) {
        this.relatedFiles = response.data.maintenanceWorkRltFileSimples;
        let data = response.data.maintenanceWorkRltWorkTickets;
        for (let i = 0; i < data.length; i++) {
          let workTickets = data[i].workTickets;
          if (workTickets.length == 0 && this.repairLevel == 1) {
            this.canSubmit = false;
            break;
          } else {
            this.canSubmit = true;
          }
        }
        this.maintenanceWorks = data;
      }
    },
    //添加工作票
    addWorkTicket(record) {
      this.$refs.SmCrPlanSkylightAddWorkTicketModal.add(record, this._repaireId);
      this.skylightPlanId = record.id;
    },
    //删除工作票
    removeWorkTicket(record, skylightPlanId) {
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let data = {
              skylightPlanId: skylightPlanId,
              ticketId: record.id,
            };
            let response = await apiSkylightPlan.removeWorkTicket(data);
            if (requestIsSuccess(response)) {
              _this.$message.success('删除成功');
              _this.refresh(_this._repaireId);
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() { },
      });
    },
    //查看工作票
    viewWorkTicket(record) {
      this.$refs.SmCrPlanSkylightAddWorkTicketModal.view(record, this._repaireId);
    },
    //编辑工作票
    editWorkTicket(record) {
      this.skylightPlanId = record.skylightPlanId;
      this.workTicketId = record.id;
      this.$refs.SmCrPlanSkylightAddWorkTicketModal.edit(record, this._repaireId);
    },
    //退回modal框
    removeSkylightPlan(skylightPlanId) {
      this.contentVisible = true;
      this.skylightPlanId = skylightPlanId;
    },
    //退回
    async backPlans() {
      this.submitLoading = true;
      let data = {
        workflowId: this._workflowId,
        skylightPlanId: this.skylightPlanId,
        opinion:this.opinion,
      };
      let response = await apiMaintenanceWork.removeMaintenanceWorkRltSkylightPlan(data);
      if (requestIsSuccess(response)) {
        this.$message.success('退回成功');
        this.contentVisible = false;
        this.refresh(this._workflowId);
      }
      this.submitLoading = false;
      // let _this = this;
      // this.$confirm({
      //   title: tipsConfig.back.title,
      //   content: h => <div style="color:red;">{tipsConfig.back.content}</div>,
      //   okType: 'danger',
      //   onOk() {
      //     return new Promise(async (resolve, reject) => {
      //       let data = {
      //         workflowId: _this._workflowId,
      //         skylightPlanId: this.skylightPlanId,
      //       };
      //       let response = await apiMaintenanceWork.removeMaintenanceWorkRltSkylightPlan(data);
      //       if (requestIsSuccess(response)) {
      //         _this.$message.success('退回成功');
      //         _this.refresh(_this._repaireId);
      //         setTimeout(resolve, 100);
      //       } else {
      //         setTimeout(reject, 100);
      //       }
      //     });
      //   },
      //   onCancel() { },
      // });
    },

    //添加方案
    addRelatedFile() {
      this.relatedFiles.push({
        id: CreateGuid(),
        contentFiles: [],
        coverFile: null,
      });
    },
    //删除方案
    deleteRelatedFile(id) {
      this.relatedFiles = this.relatedFiles.filter(s => s.id != id);
    },
    //编辑垂直天窗
    // updateSkylightPlan(record) {
    //   this.$refs.SmCrPlanSkylightEditModal.edit(record.id);
    // },
    //提交审批
    async submit() {
      console.log(this.relatedFiles);
      this.form.validateFields(async (err, value) => {
        if (!err) {
          this.submitLoading = true;
          let response;
          if (this.repeatCheck) {
            let data = {
              id: this._repaireId,
            };
            response = await apiMaintenanceWork.sumbitSecondFlow(this._repaireId);
          } else {
            console.log(this.relatedFiles);
            if (this.relatedFiles.length > 0) {
              for (let i = 0; i < this.relatedFiles.length; i++) {
                const ele = this.relatedFiles[i];
                if (ele.contentFiles.length == 0) {
                  this.$message.error("未添加计划方案");
                  this.submitLoading = false;
                  return;
                } else if (ele.coverFile == null) {
                  this.$message.error("未添加方案封面");
                  this.submitLoading = false;
                  return;
                }
              }
            }
            else {
              this.$message.error("未添加计划方案");
              this.submitLoading = false;
              return;
            }
            let data = {
              repaireId: this._repaireId,
              maintenanceWorkRltFiles: this.relatedFiles,
            };
            response = await apiMaintenanceWork.sumbitFirsrFlow(data);
          }
          if (requestIsSuccess(response) && response.data) {
            this.$message.info("已提交审批");
            this.$emit("success");
            this.visible = false;
          }
          this.submitLoading = false;
        }
      });
    },
  },
  render() {
    return [
      <div class="SmCrPlanSkylightPlanWorkTicketModal">
        <a-modal
          visible={this.visible}
          title="查看维修作业"
          width={1400}
          // onOk={() => { this.submit(); }}
          onCancel={() => this.visible = false}
          // okText="提交审批"
          // disabled={true}
          // maskClosable={true}
          // okButtonProps={this.canSubmit ? {} : this.onButtonProps}
          // destroyOnClose={true}
          confirmLoading={this.submitLoading}
          footer={null}
        >
          <div class="sm-cr-plan-skylight-work-ticket-label">维修计划内容：</div>
          <a-table
            columns={this.columns}
            rowKey={record => record.id}
            dataSource={this.maintenanceWorks}
            bordered={false}
            pagination={false}
            loading={this.loading}
            scroll={{ x: 1400, y: 500 }}
            {...{
              scopedSlots: {
                level: (index, record, text) => {
                  let level = record.level != null ? record.level.split(",").map(Number) : null;
                  let options = [];
                  if (level != null) {
                    level.forEach(element => {
                      switch (element) {
                      case 1:
                        options.push("天窗点内I级维修、");
                        break;
                      case 2:
                        options.push("天窗点内II级维修、");
                        break;
                      case 3:
                        options.push("天窗点外I级维修、");
                        break;
                      case 4:
                        options.push("天窗点外II级维修");
                        break;
                      default:
                        break;
                      }
                    });
                  }
                  return (
                    <a-tooltip placement="topLeft" title={options}>
                      <span>{options}</span>
                    </a-tooltip>
                  );
                },

                operations: (index, record, text) => {
                  return [
                    <span>
                      {!this.repeatCheck ?
                        [
                          <a
                            onClick={() => {
                              this.updateSkylightPlan(record);
                            }}
                          >编辑</a>,
                          <a-divider type="vertical" />,
                          <a-dropdown trigger={['click']} hidden={this.isApply}>
                            <a class="ant-dropdown-link" onClick={''}>
                              更多 <a-icon type="down" />
                            </a>
                            <a-menu slot="overlay">
                              <a-menu-item>
                                <a
                                  onClick={() => {
                                    this.removeSkylightPlan(record.id);
                                  }}
                                >
                                  删除
                                </a>
                              </a-menu-item>
                              <a-menu-item>
                                <a
                                  onClick={() => {
                                    this.addWorkTicket(record);
                                  }}
                                >
                                  添加工作票
                                </a>
                              </a-menu-item>
                            </a-menu>
                          </a-dropdown>,
                        ]
                        :
                        <a
                          onClick={() => {
                            this.removeSkylightPlan(record.id);
                          }}
                        >
                          退回
                        </a>
                      }
                    </span>,
                  ];
                },
                expandedRowRender: text => {
                  let skyLightPlanId = text.id;
                  return (
                    <a-table
                      rowKey={record => record.id}
                      slot-scope="text"
                      columns={this.innerColumns}
                      dataSource={text.workTickets}
                      bordered={false}
                      pagination={false}
                      {...{
                        scopedSlots: {
                          repairLevel: (index, record, text) => {
                            let level = record.repairLevel != null ? record.repairLevel.split(",").map(Number) : null;
                            let options = [];
                            if (level != null) {
                              level.forEach(element => {
                                switch (element) {
                                case 1:
                                  options.push("天窗点内I级维修、");
                                  break;
                                case 2:
                                  options.push("天窗点内II级维修、");
                                  break;
                                case 3:
                                  options.push("天窗点外I级维修、");
                                  break;
                                case 4:
                                  options.push("天窗点外II级维修");
                                  break;
                                default:
                                  break;
                                }
                              });
                            }
                            return (
                              <a-tooltip placement="topLeft" title={options}>
                                <span>{options}</span>
                              </a-tooltip>
                            );
                          },
                          operations: (index, record, text) => {
                            return [
                              <span>
                                <a
                                  onClick={() => {
                                    this.viewWorkTicket(record);
                                  }}
                                >详情</a>
                                {!this.repeatCheck ?
                                  [<span>
                                    <a-divider type="vertical" />
                                    <a-dropdown trigger={['click']} hidden={this.isApply}>
                                      <a class="ant-dropdown-link" onClick={''}>
                                        更多 <a-icon type="down" />
                                      </a>
                                      <a-menu slot="overlay">
                                        <a-menu-item>
                                          <a
                                            onClick={() => {
                                              this.editWorkTicket(record);
                                            }}
                                          >
                                            编辑
                                          </a>
                                        </a-menu-item>
                                        <a-menu-item>
                                          <a
                                            onClick={() => {
                                              this.removeWorkTicket(record, skyLightPlanId);
                                            }}
                                          >
                                            删除
                                          </a>
                                        </a-menu-item>
                                      </a-menu>
                                    </a-dropdown>
                                  </span>]
                                  : []
                                }
                              </span>,
                            ];
                          },
                        },
                      }}
                    >
                    </a-table>
                  );
                },
              },
            }}
          >
          </a-table>

          {/* <div class="sm-cr-plan-skylight-work-ticket-label" style="line-height: 32px; margin-top: 10px;">
            <span>方案文件：</span>
            {!this.repeatCheck ? <span style="float: right;"><a-button type="primary" onClick={this.addRelatedFile}>添加</a-button></span> : null}
          </div>
          <a-table
            columns={this.relatedFilescolumns}
            dataSource={this.relatedFiles}
            rowKey={record => record.id}
            loading={this.loading}
            pagination={false}
            scroll={{ y: 500 }}
            {...{
              scopedSlots: {
                index: (text, record, index) => {
                  return (index + 1);
                },
                content: (index, record, text) => {
                  return (
                    <SmFileManageSelect
                      height={30}
                      axios={this.axios}
                      disabled={this.repeatCheck}
                      value={record.contentFiles}
                      enableDownload={this.repeatCheck}
                      onChange={value => {
                        record.contentFiles = value;
                      }}
                    />
                  );
                },
                cover: (index, record, text) => {
                  return (
                    <SmFileManageSelect
                      height={30}
                      axios={this.axios}
                      value={record.coverFile}
                      multiple={false}
                      enableDownload={this.repeatCheck}
                      disabled={this.repeatCheck}
                      onChange={value => {
                        record.coverFile = value;
                      }}
                    />
                  );
                },

                operations: (index, record, text) => {
                  return [
                    !this.repeatCheck ?
                      <span>
                        <a
                          onClick={() => {
                            this.deleteRelatedFile(record.id);
                          }}
                        >
                          删除
                        </a>
                      </span> : null,
                  ];
                },
              },
            }}
          ></a-table> */}
        </a-modal>
        <SmCrPlanSkylightAddWorkTicketModal
          ref="SmCrPlanSkylightAddWorkTicketModal"
          axios={this.axios}
          workTicketId={this.workTicketId}
          skylightPlanId={this.skylightPlanId}
          onOk={value => {
            this.refresh(value);
          }}
        />
        <a-modal
          title={"退回原因"}
          visible={this.contentVisible}
          onCancel={() => this.contentVisible = false}
          onOk={() => this.backPlans()}
          loading={this.submitLoading}
        >
          <a-form>
            <a-form-item label="退回原因"
              label-col={formConfig.labelCol}
              wrapper-col={formConfig.wrapperCol}
            >
              <a-input type="textarea"
                value={this.opinion}   
                onChange={value=>this.opinion = value.target.value}
              >
              </a-input>
            </a-form-item>
          </a-form>
          
        </a-modal>
      </div >,
    ];
  },
};