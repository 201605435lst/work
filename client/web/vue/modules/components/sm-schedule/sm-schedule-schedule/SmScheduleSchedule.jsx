
import './style';
import { requestIsSuccess, objFilterProps, getWorkType, getScheduleState } from '../../_utils/utils';
import ApiSchedule from '../../sm-api/sm-schedule/Schedule';
import ApiEquipment from '../../sm-api/sm-resource/Equipments';
import { WorkType, PageState, ScheduleState } from '../../_utils/enum';
import { pagination as paginationConfig } from '../../_utils/config';
import DataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import SmBpmWorkflowModal from '../../sm-bpm/sm-bpm-workflow-templates/SmBpmWorkflowSelectModal';
import SmStdBasicProcessTemplateSelece from '../../sm-std-basic/sm-std-basic-process-template-tree-select';
import SmStdBasicProjectItemRltProcessTemplate from '../../sm-std-basic/sm-std-basic-process-template/SmStdBasicProjectItemRltProcessTemplate';
import SchedulesSelect from '../sm-schedule-schedules-select';

import moment from 'moment';
let apiSchedule = new ApiSchedule();
let apiEquipment = new ApiEquipment();

const formValues = [
  'professionId',//所属专业
  'name',//任务名称
  'type',//工作类型
  'location',//施工部位
  'startTime',//计划开始时间
  'endTime',//结束时间
  'timeLimit',//计划工期
  'scheduleRltSchedules',//前置任务
  'scheduleFlowInfos',//审核流程
  //'scheduleRltProjectItems',//工程工项
  'state',//任务状态
];

export default {
  name: 'SmScheduleSchedule',
  props: {
    axios: { type: Function, default: null },
    id: { type: String, default: null },
    pageState: { type: String, default: PageState.Edit},
    single: { type: Boolean, default: true }, //默认为单个添加
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      form: {},
      loading: false,
      checkWay:'',
      isArtificial: false, //默认为自动审核。自动审核不要选流程，但需提前设置流程
      formValues:[],
      flowTemplate:null,

      startTime:null,
      endTime:null,
      sheduleIdLists:[], //批量添加计划时所选的Ids

      isEBS: true, //单个添加时 右侧默认为EBS关联界面

      projectItemIds: [], //选中的EBS的Ids
      modelsList:[], //关联模型数据
      gapDosagesList:[], //用量对比数据
      selectedEquipments:[], //选中的关联的设备

      totalCount: 0,
      pageIndex: 1,
      maxResultCount: paginationConfig.defaultPageSize,
    };
  },
  computed: {
    isShow() {
      return this.pageState == PageState.View;
    },
    sm(){
      return this.single ? 24 : 12;
    },
    md(){
      return this.single ? 24 : 12;
    },
    tab1Columns() {
      return [
        {
          title:'序号',
          dataIndex:'index',
          width: 60,
          scopedSlots:{customRender: 'index'},
        },
        {
          title:'名称',
          dataIndex:'name',
          ellipsis: true,
          scopedSlots:{customRender: 'name'},
        },
        {
          title:'编码',
          dataIndex:'code',
          ellipsis: true,
          scopedSlots:{customRender: 'code'},
        },
        {
          title:'开始时间',
          dataIndex:'startTime',
          scopedSlots:{customRender: 'startTime'},
        },
        {
          title:'结束时间',
          dataIndex:'endTime',
          scopedSlots:{customRender:'endTime'},
        },
        {
          title:'进度',
          dataIndex:'progress',
          scopedSlots:{customRender: 'progress'},
          // customCell: (record)=> {  //table组件单元格样式事件
          //   return {
          //     on: {
          //       mouseenter: (event)=>{

          //         record.editable = true;
          //       },
          //       mouseleave: (event)=>{
          //         record.editable = false;
          //       },
          //     }};},
        },
        {
          title:'状态',
          dataIndex:'state',
          scopedSlots:{customRender:'state'},
        },
      ];
    },
    tab2Columns(){
      return [
        {
          title:'序号',
          dataIndex:'index',
          scopedSlots:{customRender: 'index'},
        },
        {
          title:'名称',
          dataIndex:'name',
          scopedSlots:{customRender: 'name'},
        },
        {
          title:'材料名称',
          dataIndex:'materialName',
          scopedSlots:{customRender:'materialName'},
        },
        {
          title:'规格型号',
          dataIndex:'specModel',
          scopedSlots:{customRender: 'specModel'},
        },
        {
          title:'计划总量',
          dataIndex:'planTotal',
          scopedSlots:{customRender:'planTotal'},
        },
        {
          title:'实际用量',
          dataIndex:'AQ',
          scopedSlots:{customRender: 'AQ'},
        },
        {
          title:'单位',
          dataIndex:'unit',
          scopedSlots:{customRender:'unit'},
        },
      ];
    },
  },
  watch: {
    id: {
      handler: function (value, oldValue) {
        if (value) {
          this.refresh();
        }
      },
    },
    pageState: {
      handler: function (value, oldValue) {
        if (value) {
          this.refresh();
        }
      },
    },
  },
  async created() {
    this.initAxios();
    this.form = this.$form.createForm(this, {});
    this.refresh();
  },
  methods: {
    initAxios() {
      apiSchedule = new ApiSchedule(this.axios);
      apiEquipment = new ApiEquipment(this.axios);
    },
    async refresh() {
      if (this.pageState !== PageState.Add) {
        let response = await apiSchedule.get(this.id);
        if (requestIsSuccess(response)) {
          let _schedules = response.data;
          this.formValues = formValues;
          let schedules = {
            ..._schedules,
            startTime: moment(_schedules.startTime).format('YYYY-MM-DD') != '0001-01-01' ? moment(_schedules.startTime) : null,
            endTime: moment(_schedules.endTime).format('YYYY-MM-DD') != '0001-01-01' ? moment(_schedules.endTime) : null,
            scheduleRltSchedules: _schedules.scheduleRltSchedules.map(item =>  {return item.frontScheduleId; }),
            scheduleRltProjectItems: _schedules.scheduleRltProjectItems.map(item =>  { this.projectItemIds.push(item.projectItemId); }),
            scheduleRltEquipments: _schedules.scheduleRltEquipments.map(item =>  { this.modelsList.push(item); }),
            state: getScheduleState(_schedules.state),
          };
          this.checkWay = schedules.isAuto ? '自动审核' : '人工审核';
          this.formValues = this.pageState == PageState.Add ? this.formValues : this.removeElement('scheduleFlowInfos');
          this.$nextTick(() => {
            let values = objFilterProps(schedules, this.formValues);
            this.form.setFieldsValue(values);
          });
        }
      }
    },

    removeElement(val){
      let index = this.formValues.indexOf(val);
      if(index > -1){
        this.formValues.splice(index,1);
      }
      return this.formValues;
    },
    
    save(){
      if(this.selectedEquipments.length == 1){
        this.$message.error('请选择关联模型');
      }
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let _values = JSON.parse(JSON.stringify(values));
          let data;
          if(this.single){
            data = {
              ..._values,
              scheduleRltSchedules: _values.scheduleRltSchedules
                ? _values.scheduleRltSchedules.map(item => {
                  return { id: item };
                })
                : [],
              scheduleRltProjectItems: this.projectItemIds
                ? this.projectItemIds.map(item => {
                  return { id: item };
                })
                : [],
              scheduleRltEquipments: this.selectedEquipments,
              parentId: this.id != null ? this.id : '',
              timeLimit: parseInt(_values.timeLimit) ? parseInt(_values.timeLimit) : 0,
              scheduleFlowInfos: this.flowTemplate ? this.flowTemplate : null,
              workflowTemplateId: this.flowTemplate ? this.flowTemplate.formTemplates[0].workflowTemplateId : null, //选择了流程，为人工审核
              isAuto: this.flowTemplate ? false : true, //当选择了流程时，为人工审核
            };
          }else{
            if(this.sheduleIdLists.length == 0){
              this.$message.error('请选择工序模板');
            }else{
              data = {
                ..._values,
                workflowTemplateId: this.flowTemplate ? this.flowTemplate.formTemplates[0].workflowTemplateId : null, //选择了流程，为人工审核
                scheduleIdLists:this.sheduleIdLists,
                isAuto: this.flowTemplate ? false : true, //当选择了流程时，为人工审核
              };
            }
          }
          
          this.loading = true;

          if (this.pageState === PageState.Add && data) {
            let response = await apiSchedule.create(data);
            if (requestIsSuccess(response)) {
              this.$message.success('添加成功');
              this.$emit('success');
              this.form.resetFields();
              this.close();
            }
          } else if (this.pageState === PageState.Edit) {
            let _data = { id: this.id, ...data };
            let response = await apiSchedule.update(_data);
            if (requestIsSuccess(response)) {
              this.$message.success('编辑成功');
              this.$emit('success');
              this.form.resetFields();
              this.close();
            }
          }
          this.loading = false;
        }
      });
    },
    //关闭单页
    close() {
      this.$emit('cancel');
    },

    //获取标准时间
    p(s) {
      return s < 10 ? '0' + s : s;
    },
    getDateGap(){
      if(this.endTime != null && this.startTime != null){
        const s = new Date(this.startTime._d);
        const e = new Date(this.endTime._d);
        const start = s.getFullYear() + '-' + this.p((s.getMonth() + 1)) + '-' + this.p(s.getDate());
        const end = e.getFullYear() + '-' + this.p((e.getMonth() + 1)) + '-' + this.p(e.getDate());
        let time1 = new Date(start.replace(/-/,"/"));
        let time2 = new Date(end.replace(/-/,"/"));
        this.form.setFieldsValue({ timeLimit : (time2 - time1)/(1000*60*60*24) + 1 + ' days' });
      }else{
        this.form.setFieldsValue({ timeLimit : '' });
      }
    },

    async onPageChange(page, pageSize) {
      this.pageIndex = page;
      this.queryParams.maxResultCount = pageSize;
      if (page !== 0) {
        this.refresh(false);
      }
    },

    async changeTab(resetPage = true, page){
      if(!this.isEBS){ //如果跳转到模型构建
        this.loading = true;
        this.modelsList = [];
        this.gapDosagesList = [];
        if (resetPage) {
          this.pageIndex = 1;
          this.maxResultCount = paginationConfig.defaultPageSize;
        }
        let response = await apiEquipment.getEquipment(this.projectItemIds);
        if (requestIsSuccess(response) && response.data) {
          //this.modelsList = response.data;
          response.data.map(item => {
            this.modelsList.push({
              id: item.id,
              code: item.code,
              name: item.name,
              progress: 0,
              startTime: null, //开始时间最低为整个计划的开始时间，
              endTime: null,
              state: 4, //默认未启动
            });
          });
          this.totalCount = response.data.length;
          if (page && this.totalCount && this.maxResultCount) {
            let currentPage = parseInt(this.totalCount / this.maxResultCount);
            if (this.totalCount % this.maxResultCount !== 0) {
              currentPage = page + 1;
            }
            if (page > currentPage) {
              this.pageIndex = currentPage;
              this.changeTab(false, this.pageIndex);
            }
          }
        }
        this.loading = false;
      }
    },

  },
  render() {
    let _workType = [];
    for (let item in WorkType) {
      _workType.push(
        <a-select-option key={WorkType[item]}>
          {getWorkType(WorkType[item])}
        </a-select-option>,
      );
    }
    let _scheduleType = [];
    for (let item in ScheduleState) {
      _scheduleType.push(
        <a-select-option key={ScheduleState[item]}>
          {getScheduleState(ScheduleState[item])}
        </a-select-option>,
      );
    }
    return (
      <div class="sm-schedule-schedule">
        <a-form form={this.form}>
          <a-row gutter={24} >
            <div class="sm-schedule-schedule-body">
              <div>  {/* 添加基础信息 */}
                <a-col sm={12} md={12}>
                  <a-form-item
                    label="所属专业"
                    label-col={{ span: 4 }} wrapper-col={{ span: 20 }}
                  >
                    <DataDictionaryTreeSelect
                      axios={this.axios}
                      groupCode={'Profession'}
                      disabled={this.isShow}
                      placeholder={this.pageState == PageState.View ? '' : '请选择故障等级'}
                      v-decorator={[
                        'professionId',
                        {
                          initialValue: null,
                          rules: [{required: true,message: '请选择所属专业'}],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
                {this.single ? //单个添加
                  <a-col sm={12} md={12}>
                    <a-form-item
                      label="工作名称"
                      label-col={{ span: 4 }} wrapper-col={{ span: 20 }}
                    >
                      <a-input
                        placeholder={this.pageState == PageState.Add ? '' : '请输入工作名称'}
                        disabled={this.isShow}
                        v-decorator={[
                          'name',
                          {
                            initialValue: '',
                            rules: [
                              {
                                required: true,
                                message: '请输入工作名称',
                                whitespace: true,
                              },
                            ],
                          },
                        ]}
                      />
                    </a-form-item>
                  </a-col>: undefined}
                <a-col sm={12} md={12}>
                  <a-form-item
                    label="工作类型"
                    label-col={{ span: 4 }} wrapper-col={{ span: 20 }}
                  >
                    <a-select
                      placeholder={this.isShow ? '' : '请选择工作类型'}
                      disabled={this.isShow}
                      v-decorator={[
                        'type',
                        {
                          initialValue: undefined,
                          rules: [{ required: true, message: '请选择工作类型' }],
                        },
                      ]}
                    >
                      {_workType}
                    </a-select>
                  </a-form-item>
                </a-col>
                <a-col sm={12} md={12}>
                  <a-form-item
                    label="施工部位"
                    label-col={{ span: 4 }} wrapper-col={{ span: 20 }}
                  >
                    <a-input
                      placeholder={this.pageState == PageState.Add ? '' : '请输入施工部位'}
                      disabled={this.isShow}
                      v-decorator={[
                        'location',
                        {
                          initialValue: '',
                          rules: [
                            {
                              required: true,
                              message: '请输入施工部位',
                              whitespace: true,
                            },
                          ],
                        },
                      ]}
                    />
                  </a-form-item>
                </a-col>
                {this.single ? //单个添加
                  <a-col sm={12} md={12}>
                    <a-form-item
                      label="计划开始"
                      label-col={{ span: 4 }} wrapper-col={{ span: 20 }}
                    >
                      <a-date-picker
                        style="width:100%"
                        format="YYYY-MM-DD"
                        disabled={this.isShow}
                        onChange={value => {this.startTime = value;this.getDateGap();}}
                        v-decorator={[
                          'startTime',
                          {
                            initialValue: null,
                            rules: [
                              {
                                required: true,
                                message: '请选择开始时间',
                              },
                            ],
                          },
                        ]}
                      />
                    </a-form-item>
                  </a-col>: undefined}
                {this.single ? //单个添加
                  <a-col sm={12} md={12}>
                    <a-form-item
                      label="计划完成"
                      label-col={{ span: 4 }} wrapper-col={{ span: 20 }}
                    >
                      <a-date-picker
                        style="width:100%"
                        format="YYYY-MM-DD"
                        disabled={this.isShow}
                        onChange={value => {this.endTime = value;this.getDateGap();}}
                        v-decorator={[
                          'endTime',
                          {
                            initialValue: null,
                            rules: [
                              {
                                required: true,
                                message: '请选择结束时间',
                              },
                            ],
                          },
                        ]}
                      />
                    </a-form-item>
                  </a-col> : undefined}
                {this.single ? //单个添加
                  <a-col sm={12} md={12}>
                    <a-form-item
                      label="计划工期"
                      label-col={{ span: 4 }} wrapper-col={{ span: 20 }}
                    >
                      <a-input
                        placeholder={this.pageState == PageState.Add ? '' : '请输入计划工期'}
                        disabled={true}
                        v-decorator={[
                          'timeLimit',
                          {
                            initialValue: null,
                          },
                        ]}
                      />
                    </a-form-item>
                  </a-col> : undefined}
                {this.single ? //单个添加
                  <a-col sm={12} md={12}>
                    <a-form-item
                      label="前置任务"
                      label-col={{ span: 4 }} wrapper-col={{ span: 20 }}
                    >
                      <SchedulesSelect
                        axios={this.axios}
                        placeholder={this.isShow ? '' : '请选择前置任务'}
                        disabled={this.isShow}
                        multiple={true}
                        height={34}
                        margin={[4, 0, 0, 0]}
                        v-decorator={[
                          'scheduleRltSchedules',
                          {
                            initialValue: [],
                          },
                        ]}
                      >
                      
                      </SchedulesSelect>
                    </a-form-item>
                  </a-col> : undefined}

                {this.single ? undefined :  //批量添加时选择 WBS -- 工序模板
                  <a-col sm={12} md={12}>
                    <a-form-item
                      label="工序模板"
                      label-col={{ span: 4 }} wrapper-col={{ span: 20 }}
                    >
                      <SmStdBasicProcessTemplateSelece 
                        axios={this.axios}
                        maxTagCount={5}
                        allowClear={true}
                        value={[]}
                        treeCheckStrictly={true}
                        treeCheckable={true}
                        onInput={value => this.sheduleIdLists = value}
                      />
                    </a-form-item>
                  </a-col> }
                
                <a-col sm={12} md={12}>
                  {this.isShow ?  
                    <a-form-item
                      label="审核方式"
                      label-col={{ span: 4 }} wrapper-col={{ span: 20 }}
                    >
                      <a-input
                        placeholder={'请选择审核方式'}
                        disabled={this.isShow}
                        value={this.checkWay}
                      />
                    </a-form-item>
                    : <a-form-item
                      label="人工审核"
                      label-col={{ span: 4 }} wrapper-col={{ span: 20 }}
                    >
                      <a-tooltip placement="right">
                        <template slot="title">
                          {this.isArtificial ? <span>人工审核：需选择审核流程</span> : <span>自动审核：需设置审核流程</span>}
                        </template>
                        <a-switch
                          onChange={checked => {
                            this.isArtificial = checked;
                            if(checked){
                              this.flowTemplate = null;
                            }
                          }}
                        />
                      </a-tooltip>
                        
                    </a-form-item>
                  }
                </a-col>

                {this.isShow ? 
                  <a-col sm={12} md={12}>
                    <a-form-item
                      label="任务状态"
                      label-col={{ span: 4 }} wrapper-col={{ span: 20 }}
                    >
                      <a-select
                        disabled={this.isShow}
                        v-decorator={[
                          'state',
                        ]}
                      >
                      </a-select>
                    </a-form-item>
                  </a-col> : 
                  !this.isArtificial ? undefined : //当为自动审核时，为undefined
                    <a-col sm={12} md={12}>
                      <a-form-item
                        label="审核流程"
                        label-col={{ span: 4 }} wrapper-col={{ span: 20 }}
                      >
                        <a-input
                          disabled={this.isShow}
                          onClick={() => this.$refs.flowSelect.show()}
                          placeholder={'请选择审核流程'}
                          v-decorator={[
                            'scheduleFlowInfos',
                            {
                              initialValue: undefined,
                              rules: [{ required: true, message: '请选择审核流程' }],
                            },
                          ]}
                        >
                        </a-input>
                      </a-form-item>
                    </a-col>
                }
                
              </div>
            </div>
          </a-row>
          {this.single ? // 单个添加时会选择EBS关联和模型构建
            <a-tabs defaultActiveKey="ebs" onChange={() => {this.isEBS = !this.isEBS;this.changeTab();}}>
              <a-tab-pane key="ebs" tab='EBS关联'>
                <a-tabs type='card'>
                  <a-tab-pane key="rlt">
                    <span slot="tab">
                      关联工程工项或关联单项工程
                    </span>
                    <SmStdBasicProjectItemRltProcessTemplate
                      axios={this.axios}
                      datas={this.dataSource}
                      permissions={this.permissions}
                      useAlone={true}
                      onChange={(individualProjectIds,projectItemIds) => {
                        this.projectItemIds = individualProjectIds.concat(projectItemIds);
                      }}
                      value={this.projectItemIds}
                    // v-decorator={[  放弃使用v-decorator
                    //   'scheduleRltProjectItems',
                    //   {
                    //     initialValue: [],
                    //     rules: [{ required: true, message: '请选择工程类型' }],
                    //   },
                    // ]}
                    />
                  </a-tab-pane>
                </a-tabs>
              </a-tab-pane>

              <a-tab-pane key="model" tab='模型构建'>
                <a-tabs type='card'>
                  <a-tab-pane key="1">
                    <span slot="tab">
                      <a-icon type="inbox" />
                            模型关联
                    </span>
                    <a-table
                      columns={this.tab1Columns}
                      dataSource={this.modelsList}
                      rowKey={record => record.id}
                      pagination={false}
                      rowSelection={this.pageState == PageState.Add ? {
                        columnWidth: 30,
                        onChange: (selectedRowKeys,selectedRows) => {
                          console.log(selectedRows);
                          this.selectedEquipments = selectedRows;
                        },
                      }
                        : null
                      }
                      // customRow={record => {
                      //   return {
                      //     on: {
                      //       dblclick: (event) => {
                      //         console.log('++++++++++++');
                      //         record.editable = false;
                      //       },
                      //       mouseenter: event => {
                      //         console.log('------------');
                      //         record.editable = true;
                      //       },
                      //     },
                      //   };
                      // }}
                      {...{
                        scopedSlots: {
                          index: (text, record, index) => {
                            return index + 1 + this.maxResultCount * (this.pageIndex - 1);
                          },
                          name: (text, record) => {
                            return (
                              <a-tooltip placement="topLeft" title={record.name}>
                                <span>{record.name}</span>
                              </a-tooltip>);
                          },
                          code: (text, record) => {
                            return (
                              <a-tooltip placement="topLeft" title={record.code}>
                                <span>{record.code}</span>
                              </a-tooltip>);
                          },
                          startTime: (text, record) => {
                            return (
                              <a-date-picker
                                style="width:100%"
                                format="YYYY-MM-DD"
                                disabled={this.isShow}
                                value={record.startTime}
                                onChange={value => record.startTime = moment(value._d).format('YYYY-MM-DD')}
                              />
                            );
                          },
                          endTime: (text, record) => {
                            return (
                              <a-date-picker
                                style="width:100%"
                                format="YYYY-MM-DD"
                                disabled={this.isShow}
                                value={record.endTime}
                                onChange={value => record.endTime = moment(value._d).format('YYYY-MM-DD')}
                              />
                            );;
                          },
                          progress: (text, record) => {
                            return(
                              <a-input-number
                                style="width:100%"
                                min={0}
                                max={100}
                                precision={0}
                                disabled={this.isShow}
                                formatter={value => `${value}%`}
                                onChange={event => (record.progress = event)}
                                value={record.progress}
                              />
                            );
                          },
                          state: (text, record) => {
                            return (
                              <a-select
                                placeholder={this.isShow ? '' : '请选择状态'}
                                disabled={this.isShow}
                                value={record.state}
                                onChange={value => record.state = value}
                              >
                                {_scheduleType}
                              </a-select>
                            );
                          },
                        },
                      }}  
                    >
                      {/* <template slot="progress" slot-scope='text,record'>
                              <editable-cell text="text" onChange={this.onCellChange(record.key, 'progress', $event)} />
                            </template> */}
                    </a-table>
                          
                  </a-tab-pane>
                  <a-tab-pane key="2">
                    <span slot="tab">
                      <a-icon type="line-chart" />
                            用量对比
                    </span>
                    <a-table
                      columns={this.tab2Columns}
                      dataSource={this.gapDosagesList}
                      pagination={false}
                      // {...{
                      //   scopedSlots: {
                      //     index: (text, record, index) => {
                      //       return index + 1 + this.maxResultCount * (this.pageIndex - 1);
                      //     },
                      //     startTime: (text, record) => {
                      //       return this.form.getFieldValue('startTime');
                      //     },
                      //     endTime: (text, record) => {
                      //       return this.form.getFieldValue('endTime');
                      //     },
                      //     progress: (text, record) => {
                      //       return 0;
                      //     },
                      //     state: (text, record) => {
                      //       return '未开始';
                      //     },
                      //   },
                      // }}  
                    >
                    </a-table>
                  </a-tab-pane>
                </a-tabs>
                {/* 分页器 */}
                <a-pagination
                  style="margin-top:10px; padding-left: 12px;"
                  total={this.totalCount}
                  pageSize={this.maxResultCount}
                  current={this.pageIndex}
                  onChange={this.onPageChange}
                  onShowSizeChange={this.onPageChange}
                  showSizeChanger
                  showQuickJumper
                  showTotal={paginationConfig.showTotal}
                />
              </a-tab-pane>
            </a-tabs> 
            : undefined}
        </a-form>
        <div style="float: right;marginTop: 15px;">
          <a-button style="marginRight: 15px;" type="primary" onClick={() => { this.save(false); }} loading={this.loading}>保存</a-button>
          {/* {this.pageState == PageState.Add ? 
            <a-button style="marginRight: 15px;" type="primary" onClick={() => { this.isSubmit (); }} loading={this.loading}>提交</a-button> 
            : undefined} */}
          <a-button type="danger" onClick={() => {this.close(); }}>返回</a-button>
        </div>
        {/* 流程选择框 */}
        <SmBpmWorkflowModal
          ref="flowSelect"
          axios={this.axios}
          onSelected={value => {
            this.flowTemplate = value;
            this.form.setFieldsValue({ scheduleFlowInfos: value.name }); //可完善回显信息
          }}
        ></SmBpmWorkflowModal>
      </div>
    );
  },
};
    