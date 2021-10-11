import { ModalStatus } from '../../_utils/enum';
import { form as formConfig } from '../../_utils/config';
import ApiSkyLightPlan from '../../sm-api/sm-cr-plan/SkyLightPlan';
import ApiMaintenanceWork from '../../sm-api/sm-cr-plan/MaintenanceWork';
import moment from 'moment';
import { PlanType, RepairLevel, RepairTagKeys, PlanState} from '../../_utils/enum';
import './style';
import { requestIsSuccess, getRepairLevelTitle, CreateGuid} from '../../_utils/utils';
import SmFileManageSelect from '../../sm-file/sm-file-manage-select';

let apiSkyLightPlan = new ApiSkyLightPlan();
let apiMaintenanceWork = new ApiMaintenanceWork();
export default {
  name: 'SmCrPlanGenerateRepairPlanModal',
  props: {
    axios: { type: Function, default: null },
    repairTagKey: { type: String, default: null }, //维修项标签
    organizationId: { type: String, default: null },
  },
  data() {
    return {
      status: ModalStatus.Hide,
      form: {},
      dateRange: [moment(moment()).startOf('month'), moment(moment()).endOf('month')],
      loading: false,
      params: {},
      repaireLevel: null,
      planState: null,
      relatedFiles: [],   //维修计划关联的方案文件
      skylightPlanIds: [],
      skylightPlanCount:0,//已经勾选的垂直天窗计划数量
    };
  },
  computed: {
    visible() {
      return this.status !== ModalStatus.Hide;
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
  },
  created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    initAxios() {
      apiSkyLightPlan = new ApiSkyLightPlan(this.axios);
      apiMaintenanceWork = new ApiMaintenanceWork(this.axios);
    },
    generatePlan(params, skylightPlanIds) {
      console.log(params);
      // this.repaireLevel = params.repaireLevel;
      this.dateRange = params.dateRange;
      this.planState = params.planState;
      this.skylightPlanIds = skylightPlanIds;
      this.skylightPlanCount = skylightPlanIds.length;
      this.status = ModalStatus.Add;
    },
    close() {
      this.form.resetFields();
      this.relatedFiles = [];
      this.status = ModalStatus.Hide;
      this.loading = false;
    },
    async ok() {
      // let canSubmit = true;
      // //验证是否添加方案及封面
      // if (this.relatedFiles.length > 0 && this.repaireLevel == RepairLevel.LevelI) {
      //   for (let i = 0; i < this.relatedFiles.length; i++) {
      //     const ele = this.relatedFiles[i];
      //     if (ele.contentFiles.length == 0) {
      //       this.$message.error("未添加计划方案");
      //       canSubmit = false;
      //       this.loading = false;
      //       return;
      //     } else if (ele.coverFile == null) {
      //       this.$message.error("未添加方案封面");
      //       canSubmit = false;
      //       this.loading = false;
      //       return;
      //     }
      //   }
      // }
      // else {
      //   canSubmit = false;
      //   this.$message.error("未添加计划方案");
      //   this.loading = false;
      // }
      // if (canSubmit) {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          this.loading = true;
          let response = null;
          let data = {
            ...values,
            startTime: moment(this.dateRange[0]).format("YYYY-MM-DD HH:mm:ss"),
            endTime: moment(this.dateRange[1]).endOf('day').format("YYYY-MM-DD HH:mm:ss"),
            repairTagKey: this.repairTagKey,
            organizationId: this.organizationId,
            MaintenanceType: PlanType.Vertical,
            // RepaireLevel: this.repaireLevel,
            state: PlanState.Submited,
            maintenanceWorkRltPlanFiles: this.relatedFiles,
            skylightPlanIds: this.skylightPlanIds,
          };
          response = await apiMaintenanceWork.create(data);
          if (requestIsSuccess(response)) {
            this.$message.success('已提交');
            this.$emit('success');
            this.close();
          }
          //   response = await apiSkyLightPlan(data);
          //   if (utils.requestIsSuccess(response)) {
          //     this.$message.success('操作成功');
          //     this.$emit('success');
          //     this.close();
          //   }
        }
        this.loading = false;
      });
      // }
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
  
  },
  render() {
    let repairLevelOption = [];
    for (let item in RepairLevel) {
      if (this.repairTagKey == RepairTagKeys.RailwayWired ||
        this.repairTagKey == "" ||
        (this.repairTagKey == RepairTagKeys.RailwayHighSpeed && (RepairLevel[item] == RepairLevel.LevelI || RepairLevel[item] == RepairLevel.LevelII)))
        repairLevelOption.push(
          <a-select-option key={RepairLevel[item]}>
            {getRepairLevelTitle(RepairLevel[item])}
          </a-select-option>,
        );
    }
    return (
      <a-modal
        class="sm-crPlan-generate-repair-plan-modal-a"
        title={`计划提交`}
        visible={this.visible}
        onCancel={this.close}
        destroyOnClose={true}
        onOk={this.ok}
        width={750}
        confirmLoading={this.loading}
      >
        <a-form form={this.form}>
          {/* <a-form-item
            label="维修等级"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-select
              value={this.repaireLevel}
              onChange={value => {
                this.repaireLevel = value;
              }}
              disabled
            >
              {repairLevelOption}
            </a-select>
          </a-form-item> */}
          <a-form-item
            label="已勾选数"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-input value={this.skylightPlanCount} disabled></a-input>
          </a-form-item>
          {/* <a-form-item
            label="计划时间"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-range-picker
              allowClear={false}
              class="data-range-picker"
              value={this.dateRange}
              onChange={value => {
                this.dateRange = value;
              }}
            />
          </a-form-item> */}
          <a-form-item
            label="作业单位及负责人"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-textarea
              rows="3"
              placeholder="请输入作业单位及负责人"
              v-decorator={[
                'workOrgAndDutyPerson',
                {
                  initialValue: null,
                  rules: [
                    {
                      required: true,
                      message: '作业单位及负责人不能为空',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="签收单位"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-textarea
              rows="3"
              placeholder="请输入签收单位"
              v-decorator={[
                'signOrganization',
                {
                  initialValue: '',
                  rules: [
                    {
                      required: true,
                      message: '签收单位不能为空',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="初审部门"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-textarea
              rows="1"
              placeholder="请输入初审部门"
              v-decorator={[
                'firstTrial',
                {
                  initialValue: '',
                  rules: [
                    {
                      required: true,
                      message: '初审部门不能为空',
                      whitespace: true,
                    },
                  ],
                },
              ]}
            />
          </a-form-item>
          <a-form-item
            label="备注"
            label-col={formConfig.labelCol}
            wrapper-col={formConfig.wrapperCol}
          >
            <a-textarea
              rows="3"
              placeholder="请输入备注"
              v-decorator={[
                'remark',
                {
                  initialValue: '',
                },
              ]}
            />
          </a-form-item>
        </a-form>
        {/* 高铁需要变更：添加计划方案 暂不要封面*/}
        <a-form-item
          label="计划方案"
          label-col={formConfig.labelCol}
          wrapper-col={formConfig.wrapperCol}
        >
          <SmFileManageSelect
            height={30}
            axios={this.axios}
            disabled={this.repeatCheck}
            // value={this.relatedFiles}
            enableDownload={this.repeatCheck}
            onChange={value => {
              this.relatedFiles = value;
            }}
          />
        </a-form-item>

        {/* 高铁需要变更：添加计划方案及封面
        <a-button style="marginBottom:10px" type="primary" onClick={this.addRelatedFile}>添加</a-button>
        {
          // this.repaireLevel == RepairLevel.LevelI ?
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
          ></a-table>
          // :null
        } */}
      </a-modal>
    );
  },
};