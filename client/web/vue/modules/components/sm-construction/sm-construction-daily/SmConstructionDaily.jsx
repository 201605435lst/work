import './style';
import { requestIsSuccess, objFilterProps, CreateGuid, getUnplannedTaskType, getSafeProblemState, getQualityProblemType } from '../../_utils/utils';
import ApiDaily from '../../sm-api/sm-construction/Daily';
import ApiDispatch from '../../sm-api/sm-construction/ApiDispatch';
import { PageState, MaterialType, MemberType, ApprovalStatus } from '../../_utils/enum';
import * as utils from '../../_utils/utils';
import SmFileUpload from '../../sm-file/sm-file-upload/SmFileUpload';
import SmSystemMemberSelect from '../../sm-system/sm-system-member-select';
import SmCommonSelect from '../../sm-common/sm-common-select';
import SmDailyUnplannedTaskModal from './SmDailyUnplannedTaskModal';
import ApiSafeProblem from '../../sm-api/sm-safe/Problem';
import ApiQualityProblem from '../../sm-api/sm-quality/QualityProblem';
import SmSafeRltQualityModalSelectModal from '../../sm-safe/sm-safe-rlt-quality-modal-select/SmSafeRltQualityModalSelectModal';
import SmConstructionDispatchSelect from '../sm-construction-dispatch-select/SmConstructionDispatchSelect';
import SmConstructionDailyView from '../sm-construction-daily-view';
import moment from 'moment';
let apiSafeProblem = new ApiSafeProblem();
let apiQualityProblem = new ApiQualityProblem();
let apiDaily = new ApiDaily();
let apiDispatch = new ApiDispatch();

const formResults = [
  'code', //日志编号
  // 'dispatchId', //派工单
  'date', //填报日期
  'informantId', //填报人 --
  'weathers', //天气 --
  'temperature', //温度 --
  'windDirection', //风力风向
  'airQuality', //空气质量
  'team', //施工班组
  'builderCount', //施工人员
  'location', //施工部位
  'summary', //施工总结
  'remark',//其他内容
];
export default {
  name: 'SmConstructionDaily',
  props: {
    axios: { type: Function, default: null },
    id: { type: String, default: null },
    approval: { type: Boolean, default: false },// 是否为审批页面,配合查询条件，获取审批的内容
    permissions: { type: Array, default: () => [] },
    pageState: { type: String, default: PageState.Add },
  },
  data() {
    return {
      dailyRltFiles: [], //施工现场照片
      dailyRltSafe: [], //安全问题
      dailyRltQuality: [], //质量问题
      dispatchs: null,//派工单选择
      unplannedTask: [], //临时任务
      dailyRltEquipments: [],//设备信息
      dailyRltPlan: [], //施工任务
      record: null, // 表单绑的对象,
      comments: "",// 审批意见
      form: {}, // 表单
      iId: null,
      // sumCount: 0,//已完成的工作量
    };
  },
  computed: {
    dailyRltPlanColumns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          ellipsis: true,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '任务名称',
          ellipsis: true,
          dataIndex: 'name',
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '单位',
          dataIndex: 'unit',
          ellipsis: true,
          scopedSlots: { customRender: 'unit' },
        },
        {
          title: '工程量',
          ellipsis: true,
          dataIndex: 'quantity',
          scopedSlots: { customRender: 'quantity' },
        },
        {
          title: '当天完成',
          dataIndex: 'count',
          ellipsis: true,
          scopedSlots: { customRender: 'count' },
        },
        {
          title: '当天完成量',
          dataIndex: 'currentCount',
          scopedSlots: { customRender: 'currentCount' },
        },
        {
          title: '累计完成量',
          dataIndex: 'sumCount',
          scopedSlots: { customRender: 'sumCount' },
        },
      ];
    },
    equipmentInfoColumns() {
      return [
        {
          title: '序号',
          dataIndex: 'index',
          ellipsis: true,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '设备名称',
          ellipsis: true,
          dataIndex: 'name',
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '规格型号',
          dataIndex: 'spec',
          ellipsis: true,
          scopedSlots: { customRender: 'spec' },
        },
        {
          title: '计量单位',
          ellipsis: true,
          dataIndex: 'unit',
          scopedSlots: { customRender: 'unit' },
        },
        {
          title: '工程量',
          dataIndex: 'quantity',
          ellipsis: true,
          scopedSlots: { customRender: 'quantity' },
        },
      ];
    },
    unplannedTaskColumns() {
      let col = [
        {
          title: '序号',
          dataIndex: 'index',
          ellipsis: true,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '任务类型',
          ellipsis: true,
          dataIndex: 'taskType',
          scopedSlots: { customRender: 'taskType' },
        },
        {
          title: '任务说明',
          dataIndex: 'content',
          ellipsis: true,
          scopedSlots: { customRender: 'content' },
        },

      ];
      if (this.pageState === PageState.Add || this.pageState === PageState.Edit) {
        col = [
          ...col,
          {
            title: '操作',
            dataIndex: 'operations',
            scopedSlots: { customRender: 'operations' },
          },
        ];
      }
      return col;
    },
    problemColumns() {
      let col = [
        {
          title: '序号',
          dataIndex: 'index',
          ellipsis: true,
          scopedSlots: { customRender: 'index' },
        },
        {
          title: '问题类型',
          ellipsis: true,
          dataIndex: 'type',
          scopedSlots: { customRender: 'type' },
        },
        {
          title: '问题名称',
          dataIndex: 'name',
          ellipsis: true,
          scopedSlots: { customRender: 'name' },
        },
        {
          title: '问题发起人',
          ellipsis: true,
          dataIndex: 'creator',
          scopedSlots: { customRender: 'creator' },
        },
        {
          title: '当前状态',
          dataIndex: 'state',
          ellipsis: true,
          scopedSlots: { customRender: 'state' },
        },
      ];
      if (this.pageState === PageState.Add || this.pageState === PageState.Edit) {
        col = [
          ...col,
          {
            title: '操作',
            dataIndex: 'operations',
            scopedSlots: { customRender: 'operations' },
          },
        ];
      }
      return col;
    },
  },
  watch: {
    id: {
      handler: async function (value, oldValue) {
        this.iId = value;
        this.initAxios();
        this.refresh();
      },
      immediate: true,
    },
  },
  async created() {
    this.form = this.$form.createForm(this, {});
    this.initAxios();
  },
  methods: {
    initAxios() {
      apiDaily = new ApiDaily(this.axios);
      apiDispatch = new ApiDispatch(this.axios);
      apiSafeProblem = new ApiSafeProblem(this.axios);
      apiQualityProblem = new ApiQualityProblem(this.axios);
    },
    remove() { },
    save() {
      this.form.validateFields(async (error, values) => {
        if (!error) {
          let _dailyRltFile = [];
          let _dailyRltSafe = [];
          let _dailyRltQuality = [];
          if (this.dailyRltFiles.length == 0) {
            this.$message.error('施工现场照片不能为空');
          } else {
            await this.$refs.fileUploadPicture.commit();
            this.dailyRltFiles.map(item => {
              _dailyRltFile.push({
                fileId: item.id,
              });
            });
            this.dailyRltQuality.map(item => {
              _dailyRltQuality.push({
                qualityProblemId: item.id,
              });
            });
            this.dailyRltSafe.map(item => {
              _dailyRltSafe.push({
                safeProblemId: item.id,
              });
            });
            let data = {
              ...values,
              informantId: Object.assign(...values.informantId) ? Object.assign(...values.informantId).id : '',
              dailyRltFiles: _dailyRltFile,
              unplannedTask: this.unplannedTask,
              dailyRltSafe: _dailyRltSafe,
              dispatchId: this.dispatchId,
              dailyRltQuality: _dailyRltQuality,
              dailyRltPlan: this.dailyRltPlan,
            };
            let response = null;
            if (this.pageState === PageState.Add) {
              response = await apiDaily.create({ ...data });
            }
            if (this.pageState === PageState.Edit) {
              response = await apiDaily.update({ ...data, id: this.record.id });
            }
            if (requestIsSuccess(response)) {
              this.$message.success('操作成功');
              this.$emit('ok');
              this.form.resetFields();
            }
          }
        }
      });
    },
    async refresh() {
      if (this.iId) {
        let response = await apiDaily.get(this.iId);
        if (requestIsSuccess(response)) {
          this.record = response.data;
          let _informantId = [];
          _informantId.push({
            id: this.record ? this.record.informantId : undefined,
            type: MemberType.User,
          });
          //构造附件
          let _files = [];
          if (this.record && this.record.dailyRltFiles.length > 0) {
            this.record.dailyRltFiles.map(item => {
              let file = item.file;
              if (file) {
                _files.push({
                  id: file.id,
                  name: file.name,
                  file: file,
                  type: file.type,
                  url: file.url,
                });
              }
            });
          }
          this.dailyRltFiles = _files;
          let data = {
            ...this.record,
            date: this.record && this.record.date ? moment(this.record.date) : undefined,
            informantId: _informantId,
          };
          this.unplannedTask = this.record.unplannedTask;
          this.dailyRltQuality = this.record.dailyRltQuality.map(item => item.qualityProblem);
          this.dailyRltSafe = this.record.dailyRltSafe.map(item => item.safeProblem);
          this.dispatchId = this.record ? this.record.dispatchId : null;
          this.planHandle(this.record);
          (this.pageState === PageState.Add || this.pageState === PageState.Edit) ?
            this.$nextTick(() => {
              this.form.setFieldsValue({ ...utils.objFilterProps(data, formResults) });
            }) : '';
        }
      } else {
        this.generatedCode();
      }
    },
    // 生成编码
    generatedCode() {
      let num = '';
      let date = moment().format('YYYY-MM-DD-HH-mm-ss');
      num = date.replaceAll("-", '');
      let code = "RZ-" + num;
      this.$nextTick(() => {
        this.form.setFieldsValue({
          code: code,
        });
      });
    },
    addUnplannedTask(data) {
      this.unplannedTask = [...this.unplannedTask, data];
    },
    addDailyRltSafe(data) {
      this.dailyRltSafe = [...data];
    },
    addDailyRltQuality(data) {
      this.dailyRltQuality = [...data];
    },
    addTask() {
      this.$refs.SmDailyUnplannedTaskModal.add();
    },
    addSafe() {
      this.$refs.SmSafeRltQualityModalSelectModal.add(apiSafeProblem, 'safe', this.dailyRltSafe);
    },
    addQuailty() {
      this.$refs.SmSafeRltQualityModalSelectModal.add(apiQualityProblem, 'quality', this.dailyRltQuality);
    },
    deleteUnplannedTask(record) {
      this.unplannedTask = this.unplannedTask.filter(item => item.id != record.id);
    },
    deleteDailyRltQuality(record) {
      this.dailyRltQuality = this.dailyRltQuality.filter(item => item.id != record.id);
    },
    deleteDailyRltSafe(record) {
      this.dailyRltSafe = this.dailyRltSafe.filter(item => item.id != record.id);
    },
    // 判断当前材料
    changeTabPane(item) {
      this.tabPane = item; //material,appliance,mechanical,securityProtection
    },
    //获取施工任务和设备信息
    planHandle(data) {
      let content = data.dailyRltPlan;
      let _dailyRltPlan = [];
      content.map(item => {
        _dailyRltPlan = [
          ..._dailyRltPlan,
          {
            ...item.planMaterial,
            name: item.planMaterial && item.planMaterial.planContent ? item.planMaterial.planContent.name : '',
            count: item.count,
            planMaterialId: item.planMaterialId,
          },
        ];
      });
      _dailyRltPlan.map(async (item) => {
        let res = await apiDaily.getDailyRltPlanMaterial(item.planMaterialId);
        let _count = 0;
        if (requestIsSuccess(res)) {
          _count = res.data;
        }
        this.dailyRltPlan = [...this.dailyRltPlan, { ...item, sumCount: _count }];
      });
      let _dailyRltEquipments = [];
      content.map(item => {
        item.planMaterial && item.planMaterial.planMaterialRltEquipments && item.planMaterial.planMaterialRltEquipments.map(e => {
          _dailyRltEquipments = [
            ..._dailyRltEquipments,
            {
              ...e.equipment,
              children: null,
            },
          ];
        });
      });
      this.dailyRltEquipments = _dailyRltEquipments;
    },
    async dispatchSelected(data) {

      if (data) {
        this.dispatchId = null;
        this.dailyRltEquipments = [];
        this.dailyRltPlan=[];
        let response = await apiDispatch.get(data.id);
        if (requestIsSuccess(response) && response.data) {
          this.dispatchs = response.data;
          console.log(data);
          console.log(this.dispatchs);
          this.dispatchId = this.dispatchs ? this.dispatchs.id : null;
          let planContent = this.dispatchs.dispatchRltPlanContents.map(item => item.planContent) || [];
          let _dailyRltPlan = [];
          planContent.map(item => {
            item.planMaterials.map(m => {
              _dailyRltPlan = [
                ..._dailyRltPlan,
                {
                  ...m,
                  name: item.name,
                  count: 0,
                  planMaterialId: m.id,
                },
              ];
            });
          });
          _dailyRltPlan.map(async (item) => {
            let res = await apiDaily.getDailyRltPlanMaterial(item.planMaterialId);
            let _count = 0;
            if (requestIsSuccess(res)) {
              _count = res.data;
            }
            this.dailyRltPlan = [...this.dailyRltPlan, { ...item, sumCount: _count }];
          });
          let _dailyRltEquipments = [];
          _dailyRltPlan.map(item => {
            item.planMaterialRltEquipments.map(e => {
              _dailyRltEquipments = [
                ..._dailyRltEquipments,
                {
                  ...e.equipment,
                  children: null,
                },
              ];
            });
          });
          this.dailyRltEquipments = _dailyRltEquipments;
          this.$nextTick(() => {
            this.form.setFieldsValue({
              team: this.dispatchs ? this.dispatchs.team : null,
            });
          });
        }
      } else {
        this.dispatchId = null;
        this.dailyRltEquipments = [];
        this.dailyRltPlan=[];
        this.$nextTick(() => {
          this.form.setFieldsValue({
            team: null,
          });
        });
      }
    },

    async view() {
      let _record = {};
      this.form.validateFields(async (error, values) => {
        if (!error) {
          _record = values;
          (_record.materialList = this.material.materialList), //辅材
          (_record.applianceList = this.material.applianceList), //器具
          (_record.mechanicalList = this.material.mechanicalList), //机械
          (_record.securityProtectionList = this.material.securityProtectionList), //安防用品
          (_record.talkMedias = this.talkMedia);
          _record.processMedias = this.processMedia;
          _record.pictures = this.picture;

          //施工员
          let _buildersIds = [];
          let _directorsIds = [];
          if (_record && _record.builders.length > 0) {
            _record.builders.map(item => {
              _buildersIds.push(item.id);
            });
          }
          if (_record && _record.directors.length > 0) {
            _record.directors.map(item => {
              _directorsIds.push(item.id);
            });
          }
          let _builders = null;
          let responseBuilder = await apiUser.getListByIds(_buildersIds);
          if (requestIsSuccess(responseBuilder)) {
            let _buildersData = responseBuilder.data;
            _buildersData.map(item => {
              if (!_builders) {
                _builders = item.name;
              } else {
                _builders += '、' + item.name;
              }
            });
          }
          let _directors = null;
          let responseDirectors = await apiUser.getListByIds(_directorsIds);
          if (requestIsSuccess(responseDirectors)) {
            let _directorsData = responseDirectors.data;
            _directorsData.map(item => {
              if (!_directors) {
                _directors = item.name;
              } else {
                _directors += '、' + item.name;
              }
            });
          }
          _record.buildersName = _builders;
          _record.directorsName = _directors;

          this.approvalRecord.diaryCode = _record.code;
          let data = {
            dRecord: _record,
            aRecord: this.approvalRecord,
          };
          this.$refs.SmScheduleDiaryModal.preview(data);
        }
      });
    },
    cancel() {
      this.comments = '';
      this.sumCount = 0;
      this.dailyRltFiles = [];
      this.$emit('cancel');
      this.form.resetFields();
    },
    // 驳回审批
    async rejected() {
      let _this = this;
      if (this.comments == "") {
        this.$message.warn("请输入审批意见");
        return;
      }
      let data = {
        planId: this.record ? this.record.id : null,
        content: this.comments,
        status: ApprovalStatus.UnPass,
      };
      this.$confirm({
        title: "温馨提示",
        content: h => <div style="color:red;">确定要驳回此审批吗？</div>,
        okType: 'danger',
        onOk() {
          return new Promise(async (resolve, reject) => {
            let response = await apiDaily.process(data);
            if (requestIsSuccess(response)) {
              _this.$message.success("操作成功");
              _this.$emit("ok");
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() { },
      });
    },
    // 通过审批
    async approved() {
      if (this.comments == "") {
        this.$message.warn("请输入审批意见");
        return;
      }
      let data = {
        planId: this.record ? this.record.id : null,
        content: this.comments,
        status: ApprovalStatus.Pass,
      };
      let response = await apiDaily.process(data);
      if (requestIsSuccess(response)) {
        if (response.data) {
          this.$message.success("操作成功");
          this.$emit('ok');
        }
      }
    },
  },
  render() {
    /* 基本信息 */
    let _basic = <div> <a-col sm={12} md={12}>
      <a-form-item label="日志编号" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
        <a-input
          placeholder={this.pageState == PageState.View ? '' : '请输入日志编号'}
          disabled={true}
          v-decorator={[
            'code',
            {
              initialValue: '',
              rules: [
                {
                  required: true,
                  message: '日志编号不能为空',
                  whitespace: true,
                },
              ],
            },
          ]}
        />
      </a-form-item>
    </a-col>
    <a-col sm={12} md={12} style="margin-bottom: 10px">
      <a-form-item label="派工单" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
        <SmConstructionDispatchSelect
          placeholder={this.pageState == PageState.View ? '' : '请选择派工单'}
          disabled={this.pageState == PageState.View}
          value={this.dispatchId}
          axios={this.axios}
          isSelect
          onChange={this.dispatchSelected}
        />
      </a-form-item>
    </a-col>
    <a-col sm={12} md={12}>
      <a-form-item label="填报日期" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
        <a-date-picker
          style="width:100%"
          disabled={this.pageState == PageState.View}
          format="YYYY-MM-DD"
          v-decorator={[
            'date',
            {
              initialValue: '',
              rules: [
                {
                  required: true,
                  message: '请选择填报时间',
                },
              ],
            },
          ]}
        />
      </a-form-item>
    </a-col>
    <a-col sm={12} md={12} style={{ "margin-bottom": "10px" }}>
      <a-form-item label="填报人" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
        <SmSystemMemberSelect
          height={32}
          shouIconSelect={true}
          showUserTab={true}
          userMultiple={false}
          bordered={true}
          simple={true}
          placeholder={this.pageState == PageState.View ? '' : '请选择'}
          axios={this.axios}
          disabled={this.pageState == PageState.View}
          v-decorator={[
            'informantId',
            {
              initialValue: undefined,
              rules: [{ required: true, message: '填报人不能为空' }],
            },
          ]}
        />
      </a-form-item>
    </a-col>
    <a-col sm={12} md={12}>
      <a-form-item label="天气" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
        <a-input
          placeholder={this.pageState == PageState.View ? '' : '请输入天气'}
          disabled={this.pageState == PageState.View}
          v-decorator={[
            'weathers',
            {
              initialValue: '',
              whitespace: true,
              rules: [{ required: true, message: '天气不能为空' }],
            },
          ]}
        />
      </a-form-item>
    </a-col>
    <a-col sm={12} md={12}>
      <a-form-item label="温度" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
        <a-input
          placeholder={this.pageState == PageState.View ? '' : '请输入温度'}
          disabled={this.pageState == PageState.View}
          v-decorator={[
            'temperature',
            {
              initialValue: '',
              whitespace: true,
              rules: [{ required: true, message: '温度不能为空' }],
            },
          ]}
        />
      </a-form-item>
    </a-col>
    <a-col sm={12} md={12}>
      <a-form-item label="风力风向" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
        <a-input
          placeholder={this.pageState == PageState.View ? '' : '请输入风力风向'}
          disabled={this.pageState == PageState.View}
          v-decorator={[
            'windDirection',
            {
              initialValue: '',
              whitespace: true,
              rules: [{ required: true, message: '风力风向不能为空' }],
            },
          ]}
        />
      </a-form-item>
    </a-col>
    <a-col sm={12} md={12}>
      <a-form-item label="空气质量" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
        <a-input
          placeholder={this.pageState == PageState.View ? '' : '请输入空气质量'}
          disabled={this.pageState == PageState.View}
          v-decorator={[
            'airQuality',
            {
              initialValue: '',
              whitespace: true,
              rules: [{ required: true, message: '空气质量不能为空' }],
            },
          ]}
        />
      </a-form-item>
    </a-col>
    <a-col sm={12} md={12}>
      <a-form-item label="施工班组" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
        <a-input
          placeholder={this.pageState == PageState.View ? '' : '请输入施工班组'}
          disabled={true}
          v-decorator={[
            'team',
            {
              initialValue: '',
              // rules: [{ required: true, message: '施工班组不能为空' }],
            },
          ]}
        />
      </a-form-item>
    </a-col>
    <a-col sm={12} md={12}>
      <a-form-item label="施工人员" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
        <a-input-number
          placeholder={this.pageState == PageState.View ? '' : '请输入施工人员个数'}
          disabled={this.pageState == PageState.View}
          min={1}
          style="width:100%"
          v-decorator={[
            'builderCount',
            {
              initialValue: '',
              rules: [
                {
                  required: true,
                  message: '施工人员个数不能为空',
                },
              ],
            },
          ]}
        />
      </a-form-item>
    </a-col>
    <a-col sm={24} md={24}>
      <a-form-item label="施工部位" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
        <a-input
          placeholder={this.pageState == PageState.View ? '' : '请输入施工部位'}
          disabled={this.pageState == PageState.View}
          v-decorator={[
            'location',
            {
              initialValue: '',
            },
          ]}
        />
      </a-form-item>
    </a-col>
    <a-col sm={24} md={24}>
      <a-form-item label="施工总结" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
        <a-textarea
          rows="3"
          placeholder={this.pageState == PageState.View ? '' : '请输入施工总结'}
          disabled={this.pageState == PageState.View}
          v-decorator={[
            'summary',
            {
              initialValue: '',
            },
          ]}
        />
      </a-form-item>
    </a-col>
    </div>;

    /* 临时任务 */
    let _unplannedTask = <a-table
      columns={this.unplannedTaskColumns}
      rowKey={record => record.id}
      size="small"
      dataSource={this.unplannedTask}
      bordered={true}
      pagination={false}
      loading={this.loading}
      {...{
        scopedSlots: {
          index: (text, record, index) => {
            return (
              index + 1
            );
          },
          taskType: (text, record) => {
            return getUnplannedTaskType(record.taskType);
          },
          content: (text, record) => {
            return text;
          },
          operations: (text, record) => {
            return [
              <a onClick={() => this.deleteUnplannedTask(record)}><a-icon type="delete" style="color: red;fontSize: 16px;" /></a>,
            ];
          },
        },
      }}
    ></a-table>;
    /* 施工现场照片 */
    let _dailyRltFiles =
      <SmFileUpload
        mode={this.pageState == PageState.View ? 'view' : 'edit'}
        axios={this.axios}
        height={73}
        multiple
        accept=".jpg, .png, .tif, gif, .JPG, .PNG, .GIF, .jpeg,.JPEG"
        ref="fileUploadPicture"
        onSelected={item => {
          this.dailyRltFiles = item;
        }}
        placeholder={this.pageState == PageState.View ? '' : '请选择讲话时图片'}
        download={false}
        fileList={this.dailyRltFiles}
      />;
    /* 存在的安全问题 */
    let _dailyRltSafe =
      <a-table
        columns={this.problemColumns}
        rowKey={record => record.id}
        size="small"
        dataSource={this.dailyRltSafe}
        bordered={true}
        pagination={false}
        loading={this.loading}
        {...{
          scopedSlots: {
            index: (text, record, index) => {
              return (
                index + 1
              );
            },
            type: (text, record) => {
              let type = record.type ? record.type.name : '';
              return (
                <a-tooltip placement="topLeft" title={type}>
                  <span>{type}</span>
                </a-tooltip>
              );
            },
            name: (text, record) => {
              return record ? record.title : '';
            },
            creator: (text, record) => {
              let result = record && record.checker ? record.checker.name : '';
              return (
                <a-tooltip placement="topLeft" title={result}>
                  <span>{result}</span>
                </a-tooltip>
              );
            },

            state: (text, record) => {
              let result = record && record.state ? getSafeProblemState(record.state) : '';
              return (
                <a-tooltip placement="topLeft" title={result}>
                  <span >{result}</span>
                </a-tooltip>
              );
            },
            operations: (text, record) => {
              return [
                <a onClick={() => this.deleteDailyRltSafe(record)}><a-icon type="delete" style="color: red;fontSize: 16px;" /></a>,
              ];
            },
          },
        }}
      ></a-table>;
    /* 存在的质量问题 */
    let _dailyRltQuality =
      <a-table
        columns={this.problemColumns}
        rowKey={record => record.id}
        size="small"
        dataSource={this.dailyRltQuality}
        bordered={true}
        pagination={false}
        loading={this.loading}
        {...{
          scopedSlots: {
            index: (text, record, index) => {
              return (
                index + 1
              );
            },
            type: (text, record, index) => {
              let type = record.type ? getQualityProblemType(record.type) : '';
              return (
                <a-tooltip placement="topLeft" title={type}>
                  <span>{type}</span>
                </a-tooltip>
              );
            },
            name: (text, record) => {
              return record ? record.title : '';
            },
            creator: (text, record) => {
              let result = record && record.checker ? record.checker.name : '';
              return (
                <a-tooltip placement="topLeft" title={result}>
                  <span>{result}</span>
                </a-tooltip>
              );
            },

            state: (text, record) => {
              let result = record && record.state ? getSafeProblemState(record.state) : '';
              return (
                <a-tooltip placement="topLeft" title={result}>
                  <span >{result}</span>
                </a-tooltip>
              );
            },
            operations: (text, record) => {
              return [
                <a onClick={() => this.deleteDailyRltQuality(record)}><a-icon type="delete" style="color: red;fontSize: 16px;" /></a>,
              ];
            },
          },
        }}
      ></a-table>;
    /* 施工任务 */
    let _dailyRltPlan =
      <a-table
        columns={this.dailyRltPlanColumns}
        rowKey={record => record.id}
        size="small"
        dataSource={this.dailyRltPlan}
        bordered={true}
        pagination={false}
        loading={this.loading}
        {...{
          scopedSlots: {
            index: (text, record, index) => {
              return (
                index + 1
              );
            },
            name: (text, record) => {
              return text;
            },
            unit: (text, record) => {
              return text;
            },
            quantity: (text, record) => {
              return record.quantity;
            },

            count: (text, record) => {
              return (this.pageState === PageState.Add || this.pageState === PageState.Edit) ? <a-input-number
                value={record.count}
                min={0}
                style="width:100%"
                max={record.quantity && record.sumCount ? record.quantity - record.sumCount : 100000}
                size="small"
                onChange={(v) => {
                  record.count = v;
                }}
              /> : text;
            },
            currentCount: (text, record) => {
              let str = Number((record.count / record.quantity) * 100).toFixed(2);
              str += "%";
              return str;
            },
            sumCount: (text, record) => {
              let str = Number(((record.count + record.sumCount) / record.quantity) * 100).toFixed(2);
              str += "%";
              return str;
            },
          },
        }}
      ></a-table>;
    /* 设备信息 */
    let _equipmentInfo =
      <a-table
        columns={this.equipmentInfoColumns}
        rowKey={record => record.id}
        size="small"
        dataSource={this.dailyRltEquipments}
        bordered={true}
        pagination={false}
        loading={this.loading}
        {...{
          scopedSlots: {
            index: (text, record, index) => {
              return (
                index + 1
              );
            },
            name: (text, record) => {
              return text;
            },
            spec: (text, record) => {
              let str = record.productCategory ? record.productCategory.name : '';
              return str;
            },
            unit: (text, record) => {
              let str = record.componentCategory && record.componentCategory.unit ? record.componentCategory.unit : "未定义";
              return str;
            },

            quantity: (text, record) => {
              return text;
            },
          },
        }}
      ></a-table>;

    return (
      <div class="sm-construction-daily">
        {/* ----基本信息---- */}
        <a-form form={this.form}>

          <a-row gutter={24}>
            {(this.pageState === PageState.Add || this.pageState === PageState.Edit) ?
              <span>
                <a-tabs  >
                  <a-tab-pane key="basic" tab="基本信息">
                    {_basic}
                  </a-tab-pane>
                </a-tabs>
                {/* ----施工任务---- */}
                <a-tabs>
                  <a-tab-pane key="process" tab="施工任务">
                    <a-col sm={24} md={24} >
                      {_dailyRltPlan}
                    </a-col>
                  </a-tab-pane>
                </a-tabs>
                {/* ----设备信息---- */}
                <a-tabs>
                  <a-tab-pane key="infor" tab="设备信息">
                    <a-col sm={24} md={24} >
                      {_equipmentInfo}
                    </a-col>
                  </a-tab-pane>
                </a-tabs>
                {/* ----临时任务---- */}
                <a-tabs>
                  <a-tab-pane key="infor" tab="临时任务">
                    <a-col sm={24} md={24} >
                      {_unplannedTask}
                    </a-col>
                  </a-tab-pane>
                  {this.pageState === PageState.Add || this.pageState === PageState.Edit ?
                    <a-button type="primary" icon="plus" slot="tabBarExtraContent" size="small" class="icon-plus" onClick={this.addTask} /> : ""}
                </a-tabs>
                {/* ----存在的安全问题---- */}
                <a-tabs>
                  <a-tab-pane key="infor" tab="存在的安全问题">
                    <a-col sm={24} md={24} >
                      {_dailyRltSafe}
                    </a-col>
                  </a-tab-pane>
                  {this.pageState === PageState.Add || this.pageState === PageState.Edit ?
                    <a-button type="primary" icon="plus" slot="tabBarExtraContent" size="small" class="icon-plus" onClick={this.addSafe} /> : ''}
                </a-tabs>
                {/* ----存在的质量问题---- */}
                <a-tabs>
                  <a-tab-pane key="infor" tab="存在的质量问题">
                    <a-col sm={24} md={24}>
                      {_dailyRltQuality}
                    </a-col>
                  </a-tab-pane>
                  {this.pageState === PageState.Add || this.pageState === PageState.Edit ?
                    <a-button type="primary" icon="plus" slot="tabBarExtraContent" size="small" class="icon-plus" onClick={this.addQuailty} /> : ''}
                </a-tabs>
                {/* ----施工现场照片---- */}
                <a-tabs>
                  <a-tab-pane key="infor" tab="施工现场照片">
                    <a-col sm={24} md={24}>
                      {_dailyRltFiles}
                    </a-col>
                  </a-tab-pane>
                </a-tabs>
                {/* 其他内容 */}
                <a-tabs>
                  <a-tab-pane key="infor" tab="其他内容">
                    <a-col sm={24} md={24} >
                      <a-form-item>
                        <a-textarea
                          rows="3"
                          placeholder={this.pageState == PageState.View ? '' : '请输入'}
                          disabled={this.pageState == PageState.View}
                          v-decorator={[
                            'remark',
                            {
                              initialValue: '',
                            },
                          ]}
                        />
                      </a-form-item>

                    </a-col>
                  </a-tab-pane>
                </a-tabs>
              </span> :
              <SmConstructionDailyView
                record={this.record}
                approval={this.approval}
                dailyRltFiles={this.dailyRltFiles}
                dailyRltSafe={this.dailyRltSafe}
                dailyRltQuality={this.dailyRltQuality}
                dispatchs={this.dispatchs}
                sumCount={this.sumCount}
                unplannedTask={this.unplannedTask}
                dailyRltEquipments={this.dailyRltEquipments}
                dailyRltPlan={this.dailyRltPlan} />
            }
            {/* ----审批意见 ---- */}
            {this.pageState == PageState.Approval ? <a-tabs>
              <a-tab-pane key="infor" tab="审批意见">
                <a-col sm={24} md={24} >
                  <a-textarea
                    placeholder="请输入审批意见"
                    value={this.comments}
                    onChange={(e) => {
                      this.comments = e.target.value;
                    }}
                  />
                </a-col>
              </a-tab-pane>
            </a-tabs> : ''}

          </a-row>
          <div class="button-actions">
            <span class="back">

              <a-button size="small" type="primary" onClick={() => this.cancel()}>
                返回
              </a-button>
            </span>
            {this.pageState != PageState.Approval ?
              <a-button class="button-submit" type="primary" size="small" onClick={this.pageState == PageState.View ? this.cancel : this.save}>
                {this.pageState == PageState.View ? '确定' : '保存'}
              </a-button>
              :
              <span >
                <a-button class="back" size="small" key="rejected" type="danger" onClick={this.rejected}>驳回</a-button>

                <a-button size="small" key="approved" type="primary" onClick={this.approved}>通过</a-button>
              </span>
            }
          </div>
        </a-form>
        {/* 添加计划模态框 */}
        <SmDailyUnplannedTaskModal
          ref="SmDailyUnplannedTaskModal"
          axios={this.axios}
          onSuccess={(data) => { this.addUnplannedTask(data); }}
        />
        {/* 安全和质量选择模态框 */}
        <SmSafeRltQualityModalSelectModal
          ref="SmSafeRltQualityModalSelectModal"
          axios={this.axios}
          onSafe={(data) => { this.addDailyRltSafe(data); }}
          onQuality={(data) => { this.addDailyRltQuality(data); }}
        />
      </div>
    );
  },
};