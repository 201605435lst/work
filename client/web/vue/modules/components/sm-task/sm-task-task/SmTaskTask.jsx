
import './style';
import { requestIsSuccess,CreateGuid,getPriorityType, objFilterProps, getStateType } from '../../_utils/utils';
import { tips as tipsConfig } from '../../_utils/config';
import ApiTask from '../../sm-api/sm-task/Task';
import ApiAccount from '../../sm-api/sm-system/Account';
import SmReportSelectProjectModal from '../../sm-report/sm-report/src/SmReportSelectProjectModal';
import SmSystemMenbersSelect from '../../sm-system/sm-system-member-select';
import SmSystemMenbers from '../../sm-system/sm-system-member-modal';
import RichTextEditor from '../../sm-file/sm-file-text-editor/SmFileTextEditor';
import SmFileManageSelect from '../../sm-file/sm-file-manage-select';
import SmFlowBase from '../../sm-common/sm-flow-base';
import { TaskGroup, PriorityType, PageState } from '../../_utils/enum';
import moment from 'moment';
let apiTask = new ApiTask();
let apiAccount = new ApiAccount();

const formValues = [
  'name',//任务主题
  'priority',//优先级
  'startTime',//开始时间
  'endTime',//结束时间
  'manager',//指派给/负责人
  'ccPerson',//抄送人
  "taskRltFiles",
];

export default {
  name: 'SmTaskTask',
  props: {
    axios: { type: Function, default: null },
    permissions: { type: Array, default: () => [] },
    id: { type: String, default: null },
    pageState: { type: String, default: PageState.View}, //Edit为任务查看模式，有任务节点。View为任务反馈模式
    creatorId: { type: String, default: undefined }, //当前任务创建人Id
    hasParent: { type: Boolean, default: false}, //当前任务是否有父级
  },
  data() {
    return {
      form: {},
      loading: false,
      isManager: true, //选择指派用户or抄送用户
      managerIds:[], //授权责任用户
      ccPersonIds:[], //授权抄送用户
      fileIds:[], //关联附件
      isSwitch:false, //是否显示子任务
      userSelectVisible: false,//人员选择
      selectedUserIds: [],//批量选择人员
      queryParams: {
        taskList: [],//子任务信息
        managerList: [],
        ccPersonList: [],
        childManagerList: [],
        childCcPersonList: [],
      },
      content: null,//工作内容
      description: null,//完成情况描述
      fileServerEndPoint: '', //文件服务请求头
      tableTaskKey: null,
      isContinue: true,
      _task: null, //get接口处理后的数据
      jsonTasks: [], //流程节点数据
      percent: null, //反馈进度
      loadingMemberId: null,
    };
  },
  computed: {
    isShow() {
      return this.pageState == PageState.View; 
    },
    columns() {
      return this.isShow ? 
        [
          {
            title:'#',
            dataIndex:'index',
            scopedSlots:{customRender: 'index'},
          },
          {
            title:'任务名称',
            dataIndex:'childname',
            scopedSlots:{customRender: 'childname'},
          },
          {
            title:'反馈人',
            dataIndex:'feedbackPeople',
            scopedSlots:{customRender:'feedbackPeople'},
          },
          {
            title:'反馈时间',
            dataIndex:'feedbackTime',
            scopedSlots:{customRender:'feedbackTime'},
          },
          {
            title:'所属任务',
            dataIndex:'parentName',
            scopedSlots:{customRender: 'parentName'},
          },
          {
            title:'进度',
            dataIndex:'progress',
            scopedSlots:{customRender:'progress'},
          },
          {
            title:'情况描述',
            dataIndex:'description',
            scopedSlots: { customRender: 'description' },
          },
        ]
        : this.pageState == PageState.Edit ?
          [
            {
              title:'#',
              dataIndex:'index',
              scopedSlots:{customRender: 'index'},
            },
            {
              title:'任务名称',
              dataIndex:'childname',
              scopedSlots:{customRender: 'childname'},
            },
            {
              title:'反馈人',
              dataIndex:'feedbackPeople',
              scopedSlots:{customRender:'feedbackPeople'},
            },
            {
              title:'反馈时间',
              dataIndex:'feedbackTime',
              scopedSlots:{customRender:'feedbackTime'},
            },
            {
              title:'所属任务',
              dataIndex:'parentName',
              scopedSlots:{customRender: 'parentName'},
            },
            {
              title:'进度',
              dataIndex:'progress',
              scopedSlots:{customRender:'progress'},
            },
            {
              title:'情况描述',
              dataIndex:'description',
              scopedSlots: { customRender: 'description' },
            },
            {
              title:'操作',
              dataIndex:'editOperations',
              width: 169,
              scopedSlots: { customRender: 'editOperations' },
              fixed: 'right',
            },
          ] 
          :
          [
            {
              title:'#',
              dataIndex:'index',
              scopedSlots:{customRender: 'index'},
            },
            {
              title:'*任务名',
              dataIndex:'name',
              customHeaderCell:() => {return {style: {color: '#ff1818'}};},
              scopedSlots:{customRender: 'name'},
            },
            {
              title:'负责人',
              dataIndex:'manager',
              scopedSlots:{customRender:'manager'},
            },
            {
              title:'*抄送人',
              dataIndex:'ccPerson',
              customHeaderCell:() => {return {style: {color: '#ff1818'}};},
              scopedSlots:{customRender:'ccPerson'},
            },
            {
              title:'*结束时间',
              dataIndex:'endTime',
              customHeaderCell:() => {return {style: {color: '#ff1818'}};},
              scopedSlots:{customRender:'endTime'},
            },
            {
              title:'*权重',
              dataIndex:'weight',
              customHeaderCell:() => {return {style: {color: '#ff1818'}};},
              scopedSlots:{customRender:'weight'},
            },
            {
              title:'*占比',
              dataIndex:'proportion',
              customHeaderCell:() => {return {style: {color: '#ff1818'}};},
              scopedSlots:{customRender:'proportion'},
            },
            {
              title:'操作',
              dataIndex:'operations',
              width: 169,
              scopedSlots: { customRender: 'operations' },
              fixed: 'right',
            },
          ];
    },
    nodes(){
      return this.jsonTasks;
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
    this.getLoadingMemberId();
    this.form = this.$form.createForm(this, {});
    this.refresh();
    this.fileServerEndPoint = localStorage.getItem('fileServerEndPoint');
  },
  methods: {
    initAxios() {
      apiTask = new ApiTask(this.axios);
      apiAccount = new ApiAccount(this.axios);
    },

    //获取当前登录人的ID
    async getLoadingMemberId(){
      let response = await apiAccount.getAppConfig();
      if (requestIsSuccess(response)) {
        let _user = response.data.currentUser;
        this.loadingMemberId = _user.id;
      }
    },

    async refresh() {
      if (this.pageState !== PageState.Add && this.id !== null) {
        let response = await apiTask.get(this.id);
        if (requestIsSuccess(response) && response.data) {
          let taskData = response.data;
          let _manager = [];
          let _ccPerson = [];

          let _content = taskData.content;
          this.content = !_content ? null
            : _content.replace(new RegExp(`src="`, 'g'), `src="${this.fileServerEndPoint}`);

          taskData.taskRltMembers.map(item => {
            if(item.responsible == 2){
              _manager.push({id:item.memberId,type:3});
            }else if(item.responsible == 3){
              _ccPerson.push({id:item.memberId,type:3});
            }
          });

          let managerName = '';
          managerName = taskData.taskRltMembers.length > 0 ? taskData.taskRltMembers.map((item,ind) => item.responsible == 2 ? managerName + item.member.name + (taskData.taskRltMembers.length - 1 == ind ? "" : ",") : managerName + '') : null;
          managerName = managerName.join('');
          managerName = managerName != '' ? (managerName.indexOf(',') != -1 && managerName.lastIndexOf(',') == managerName.length - 1 ? managerName.substring(0,managerName.lastIndexOf(',')) : managerName) : '';
          let ccPersonName = '';
          ccPersonName = taskData.taskRltMembers.length > 0 ? taskData.taskRltMembers.map((item,ind) => item.responsible == 3 ? ccPersonName + item.member.name + (taskData.taskRltMembers.length - 1 == ind ? "" : ",") : ccPersonName + '') : null;
          ccPersonName = ccPersonName.join('');
          ccPersonName = ccPersonName != '' ? (ccPersonName.indexOf(',') != -1 && ccPersonName.lastIndexOf(',') == ccPersonName.length - 1 ? ccPersonName.substring(0,ccPersonName.lastIndexOf(',')) : ccPersonName) : '';
          
          //子任务表格
          taskData.childrenTasks.map(item => {
            this.queryParams.taskList.push({
              key: item.id,
              name: item.name,
              feedbackPeople: item.lastModifier ? item.lastModifier : '暂无',
              feedbackTime: item.lastModificationTime ? moment(item.lastModificationTime).format('YYYY-MM-DD') : '暂无',
              parentName: item.parent.name,
              progress: item.progress,
              description: item.description != null ? item.description : '暂无',
              id: item.id,
            });
          });

          this._task = {
            ...taskData,
            startTime: taskData.startTime && moment(taskData.startTime).format('YYYY-MM-DD') != '0001-01-01' ? moment(taskData.startTime) : null,
            endTime: taskData.endTime && moment(taskData.endTime).format('YYYY-MM-DD') != '0001-01-01' ? moment(taskData.endTime) : null,
            manager: _manager,
            ccPerson: _ccPerson,
            managerName: managerName,
            ccPersonName: ccPersonName,
            taskRltFiles: taskData.taskRltFiles.map(item => { return item.file; }),
          };
          this.percent = this._task.progress;
          //this.description = this._task.description;  再次反馈时的详情回显

          if(this.pageState == PageState.Edit){
            this.dataToFlatData(this._task,this.jsonTasks,false);
          }
          this.$nextTick(() => {
            let values = objFilterProps(this._task, formValues);
            this.form.setFieldsValue(values);
          });
        }
      }
    },

    isSubmit(){
      let _this = this;
      this.$confirm({
        title: '提交任务',
        content: h => <div style="color:red;">{'确定直接提交并启动此任务吗？'}</div>,
        okType: 'danger',
        onOk() {
          _this.save(true);
        },
        onCancel() { },
      });
    },

    save(isSubmit) {
      this.form.validateFields(async (error, value) => {
        if (!error) {
          let _values = value;
          if(this.pageState == PageState.Add){ //添加页面：
            let _content = this.$refs['sc-rich-text-editor'].content();
            let reg = new RegExp(`${this.fileServerEndPoint}`, 'g');
            _values.content = _content.replace(reg, '');

            _values.manager.map(item => this.managerIds.push({id:item.id,responsible:2}));
            _values.ccPerson.map(item => this.ccPersonIds.push({id:item.id,responsible:3}));
            _values.taskRltFiles.map(item => this.fileIds.push({id:item.id}));

            this.queryParams.taskList.map((item,index) => {
              if(item.name == '' || item.ccPerson == '' || item.endTime == '' || item.weight == ''){
                this.$message.error(`请检查第${index+1}条子任务`);
                this.isContinue = false;
                return false;
              }else{
                this.isContinue = true;
              }
              if(item.endTime <= _values.startTime || item.endTime >= _values.endTime){
                this.$message.error(`请检查第${index+1}条子任务的结束时间`);
                this.isContinue = false;
                return false;
              }else{
                this.isContinue = true;
              }
            });
            if(this.isContinue){
              this.queryParams.taskList.map(item => {
                this.queryParams.childManagerList = [];
                this.queryParams.childCcPersonList = [];
                item.managerList.map(item => this.queryParams.childManagerList.push({id:item.id,responsible:2}));
                item.ccPersonList.map(item => this.queryParams.childCcPersonList.push({id:item.id,responsible:3}));
                item.taskRltMembers = this.queryParams.childCcPersonList.concat(this.queryParams.childManagerList);
                item.proportion = item.proportion.substring(0,item.proportion.length-1);
              });
              let list = [...this.queryParams.taskList];
              
              let data = {
                ..._values,
                // startTime: _values.sartTime ? moment(_values.sartTime._d).format("YYYY-MM-DD HH:mm:ss"):null,
                // endTime: _values.endTime ? moment(_values.endTime._d).format("YYYY-MM-DD HH:mm:ss"):null,
                taskRltMembers: this.managerIds.concat(this.ccPersonIds),
                taskRltFiles: this.fileIds,
                childrenTasks: list,
                state: isSubmit ? 1 : 5,
              };
        
              this.loading = true;
              if (this.pageState == PageState.Add) {
                let response = await apiTask.create(data);
                if (requestIsSuccess(response)) {
                  this.$message.info('操作成功');
                  this.form.resetFields();
                  this.emptyData();
                  this.fileIds = [];
                  this.$emit('ok');
                }
              }
            }
          }else if(this.isShow){ //反馈页面：
            let _data = { id: this.id, description: this.description, taskRltFiles: this.fileIds, progress: this.percent};
            let response = await apiTask.feedback(_data);
            if (requestIsSuccess(response)) {
              this.$message.info('操作成功');
              this.fileIds = [];
              this.$emit('ok');
            }
          }else if(this.pageState == PageState.Edit){ //编辑页面：（只任务发起人可编辑，其余人只读）
            let _content = this.$refs['sc-rich-text-editor'].content();
            let reg = new RegExp(`${this.fileServerEndPoint}`, 'g');
            _values.content = _content.replace(reg, '');

            _values.manager.map(item => this.managerIds.push({id:item.id,responsible:2}));
            _values.ccPerson.map(item => this.ccPersonIds.push({id:item.id,responsible:3}));
            _values.taskRltFiles.map(item => this.fileIds.push({id:item.id}));

            let data = {
              ..._values,
              taskRltMembers: this.managerIds.concat(this.ccPersonIds),
              taskRltFiles: this.fileIds,
            };
      
            this.loading = true;
            let response = await apiTask.update({...data,id:this.id});
            if (requestIsSuccess(response)) {
              this.$message.info('操作成功');
              this.form.resetFields();
              this.emptyData();
              this.fileIds = [];
              this.$emit('ok');
            }
          }
        }
        
        this.loading = false;
      });
    },

    delete(record){
      let _this = this;
      this.$confirm({
        title: tipsConfig.remove.title,
        content: h => <div style="color:red;">{tipsConfig.remove.content}</div>,
        okType: 'danger',
        onOk() {
          if(record.progress != 0){
            _this.$message.error('当前任务已开始，不可删除！');
            return false;
          }
          return new Promise(async (resolve, reject) => {
            let response = await apiTask.delete(record.id);
            if (requestIsSuccess(response)) {
              _this.refresh();
              _this.$message.success('删除成功');
              setTimeout(resolve, 100);
            } else {
              setTimeout(reject, 100);
            }
          });
        },
        onCancel() {},
      });
    },
    noDelete(){
      this.$message.error('您没有此权限！请联系任务发起人');
    },
    saveEdit(){

    },

    selectProject() {
      this.$refs.SmReportSelectProjectModal.select();
    },
    addTaskInfo(value){
      let newTaskDataList = [];
      if (value !== null) {
        //给负责人和抄送人赋值
        let taskkey = this.queryParams.taskList.filter(item => item.key == this.tableTaskKey);
        if(taskkey[0].ccPerson != '' && !this.isManager){
          taskkey[0].ccPerson = '';
          taskkey[0].ccPersonList = [];
        }
        if(taskkey[0].manager != '' && this.isManager){
          taskkey[0].manager = '';
          taskkey[0].managerList = [];
        }

        if (taskkey.length > 0) {
          value.map((item,index) => {
            this.isManager ? taskkey[0].manager = taskkey[0].manager + item.name + (value.length - 1 != index ? ',' : '') : taskkey[0].ccPerson = taskkey[0].ccPerson + item.name + (value.length - 1 != index ? ',' : '');
            //taskkey[0].id += item.id + (value.length - 1 != index ? ',' : '');
            this.isManager ? taskkey[0].managerList.push(item) : taskkey[0].ccPersonList.push(item);
          });
          this.tableTaskKey = null;
        }
      }
      else {//空白添加
        newTaskDataList = [
          {
            key: CreateGuid(),
            name: '',
            manager: '',
            managerList:[],
            ccPerson: '',
            ccPersonList:[],
            endTime: '',
            weight: 1, //权重默认为1
            proportion: '',
          },
        ];
      }
      this.queryParams.taskList = [...this.queryParams.taskList, ...newTaskDataList];
      this.getProportion();
    },
    batchDeletion(){

    },
    deleteTask(selectedRowKeys){
      selectedRowKeys.map(id => {
        this.queryParams.taskList = this.queryParams.taskList.filter(item => item.key != id);
      });
      this.getProportion();
    },
    //向上变换题的顺序
    arrowUp(upOrDown, index) {
      if (upOrDown === "up" && index != 0) {
        this.queryParams.taskList[index] = this.queryParams.taskList.splice(index - 1, 1, this.queryParams.taskList[index])[0];
      }
      else if (upOrDown === "down" && index != this.queryParams.taskList.length - 1) {
        this.queryParams.taskList[index] = this.queryParams.taskList.splice(index + 1, 1, this.queryParams.taskList[index])[0];
      }
    },
    //根据权重得到占比
    getProportion(){
      let sum = 0;

      this.queryParams.taskList.map(item => {
        sum += Number(item.weight);
      });
      if(sum == 0){
        this.queryParams.taskList.map(item => {
          item.proportion = '0%';
        });
      }else{
        this.queryParams.taskList.map(item => {
          item.proportion = item.weight/sum;
          item.proportion = item.proportion.toFixed(2)*100 + '%';
        });
      }
    },
    startTask(checked){
      this.isSwitch = checked;
      if(this.isSwitch){
        this.addTaskInfo(null);
      }else{
        this.queryParams.taskList = [];
      }
      
    },
    cancel() {
      this.form.resetFields();
      this.emptyData();
      this.content = null;
      this.$emit('cancel');
    },
    emptyData(){
      this.fileIds= [],
      this.managerIds= [], //授权责任用户
      this.ccPersonIds= [], //授权抄送用户
      this.selectedUserIds= [],
      this.queryParams.taskList= [],//子任务信息
      this.queryParams.managerList= [],
      this.queryParams.ccPersonList= [],
      this.queryParams.childManagerList= [],
      this.queryParams.childCcPersonList= [];
    },
    dataToFlatData(data,flatData,isChild){
      if(!isChild){
        flatData.push({
          id: data.id,
          title: data.name,
          subTitle: data.managerName,
          perc: data.progress,
          type: getStateType(data.state),
          children: [],
          Child: data.childrenTasks,
        });
      }else{
        data.map(item => {
          flatData.push({
            id: item.id,
            title: item.name,
            subTitle: this.getManagerNames(item),
            perc: item.progress,
            type: getStateType(item.state),
            children: [],
            Child: item.childrenTasks,
          });
        });
      }
      for(let task of flatData){
        if(task.Child.length > 0){
          this.dataToFlatData(task.Child,task.children,true);
        }
      }
      
    },
    getManagerNames(taskData){
      let name = '';
      name = taskData.taskRltMembers.length > 0 ? taskData.taskRltMembers.map((item,ind) => item.responsible == 2 ? name + item.member.name + (taskData.taskRltMembers.length - 1 == ind ? "" : ",") : name + '') : null;
      name = name.join('');
      name = name != '' ? (name.indexOf(',') != -1 && name.lastIndexOf(',') == name.length - 1 ? name.substring(0,name.lastIndexOf(',')) : name) : '';
      return name;
    },
  },
  render() {
    let _taskGroup = [];
    for (let item in TaskGroup) {
      _taskGroup.push(TaskGroup[item]);
    }
    let _priorityType = [];
    for (let item in PriorityType) {
      _priorityType.push(
        <a-select-option key={PriorityType[item]}>
          {getPriorityType(PriorityType[item])}
        </a-select-option>,
      );
    }

    let childrenTask = (
      <div>
        <a-tabs defaultActiveKey="children">
          <a-tab-pane key="children" tab="子任务">
            {this.pageState != PageState.Add ? undefined :
              <div class="TaskTable">
                <a-button type="primary" icon="plus" size="small" shape="round" class="taskButton" onClick={()=>this.addTaskInfo(null)} disabled={this.isShow}>新增</a-button>
                <a-button type="danger" icon="delete" size="small" shape="round" class="taskButton" onClick={()=>this.deleteTask(this.selectedUserIds)} disabled={ this.queryParams.taskList.length === 1 || this.selectedUserIds.length == this.queryParams.taskList.length || this.isShow}>删除</a-button>
              </div>
            }
            <a-table
              columns={this.columns}
              dataSource={this.queryParams.taskList}
              rowSelection={this.pageState == PageState.Add ? {
                columnWidth: 30,
                onChange: selectedRowKeys => {
                  this.selectedUserIds = selectedRowKeys;
                },
              }
                : null
              }
              {...{
                scopedSlots: {
                  index: (text, record, index) => {
                    return index + 1; //+ 1 + this.queryParams.maxResultCount * (this.pageIndex - 1);
                  },
                  name: (text, record, index) => {
                    return [
                      <a-input
                        value={record.name}
                        onChange={value => {
                          record.name = value.target.value;
                        }}
                        class="cellWidth"
                      ></a-input>,
                    ];
                  },
                  manager:(text, record, index) => {
                    return [
                      <a-input
                        value={record.manager}
                        class="cellWidth"
                        onClick={() => {
                          this.tableTaskKey = record.key;
                          this.userSelectVisible = true;
                          this.isManager = true;
                        }}
                      ></a-input>,
                    ];
                  },
                  ccPerson:(text, record, index) => {
                    return [
                      <a-input
                        value={record.ccPerson}
                        class="cellWidth"
                        onClick={() => {
                          this.tableTaskKey = record.key;
                          this.userSelectVisible = true;
                          this.isManager = false;
                        }}
                      ></a-input>,
                    ];
                  },
                  endTime: (text, record, index) => {
                    return [
                      <a-date-picker
                        class="cellWidth"
                        showTime
                        placeholder="请填写正确的时间"
                        format="YYYY-MM-DD HH:mm:ss"
                        value={record.endTime ?record.endTime:null}
                        onChange={value => {
                          record.endTime = value;
                        }}
                      />,
                    ];
                  },
                  weight: (text, record, index) => {
                    return [
                      <a-input-number
                        //class="input cellWidth"
                        placeholder="请输入正确的正数或0！"
                        min={0}
                        value={record.weight}
                        onChange={value => {
                          record.weight = value;
                          this.getProportion();
                        }}
                      ></a-input-number>,
                    ];
                  },
                  proportion: (text, record, index) => {
                    return [
                      <a-input
                        class="cellWidth"
                        disabled={true}
                        value={record.proportion}
                        // onChange={value => {
                        //   record.proportion = value.target.value;
                        // }}
                      ></a-input>,
                    ];
                  },

                  childname: (text,record) => {
                    return (
                      <a-tooltip placement="topLeft" title={record.name}>
                        <span>{record.name}</span>
                      </a-tooltip>);
                  },
                  feedbackPeople: (text,record) => {
                    return (
                      <a-tooltip placement="topLeft" title={record.feedbackPeople}>
                        <span>{record.feedbackPeople}</span>
                      </a-tooltip>);
                  },
                  feedbackTime: (text,record) => {
                    return (
                      <a-tooltip placement="topLeft" title={record.feedbackTime}>
                        <span>{record.feedbackTime}</span>
                      </a-tooltip>);
                  },
                  parentName: (text,record) => {
                    return (
                      <a-tooltip placement="topLeft" title={record.parentName}>
                        <span>{record.parentName}</span>
                      </a-tooltip>);
                  },
                  progress: (text,record) => {
                    return(<a-progress percent={record.progress} />);
                  },
                  description: (text,record) => {
                    return (
                      <a-tooltip placement="topLeft" title={record.description}>
                        <span>{record.description}</span>
                      </a-tooltip>);
                  },
                  operations: (text, record, index) => {
                    return [
                      <a onClick={() => this.queryParams.taskList.length > 1 ? this.deleteTask([record.key]) : undefined}><a-icon type="delete" style="color: red;fontSize: 16px;" /></a>,
                      <a-divider type="vertical" />,
                      <a onClick={() => this.arrowUp("up", index)}><a-icon type="up-circle" style="fontSize: 16px;margin-left: 2px" /></a>,
                      <a onClick={() => this.arrowUp("down", index)}><a-icon type="down-circle" style="fontSize: 16px;" /></a>,
                    ];
                  },
                  editOperations: (text, record, index) => {
                    return [
                      <a onClick={() => this.loadingMemberId == this._task.creatorId ? this.delete(record) : this.noDelete()}><a-icon type="delete" style={"color: red;fontSize: 16px;"} /></a>,
                    ];
                  },
                },
              }
              }
            >
            </a-table>
          </a-tab-pane>
        </a-tabs>
      </div>
    );

    let feedback = (
      <div>
        <a-tabs defaultActiveKey="children">
          <a-tab-pane key="children" tab="任务反馈">
            <a-row gutter={24} >
              <a-col sm={12} md={12}>
                <a-form-item
                  label="当前进度"
                  label-col={{ span: 4 }} wrapper-col={{ span: 20 }}
                >
                  <a-popconfirm
                    placement="top"
                    disabled={this.queryParams.taskList.length == 0 ? false : true}
                  >
                    <template slot="title">
                      <a-slider
                        id="test"
                        style="width: 250px;"
                        step={10}
                        value={this.percent}
                        onChange={value => this.percent = value}
                      />
                    </template>
                    <a-icon slot="icon" type={this.percent == 0 ? "play-circle" : "pause"} style="top: 17px" />
                    <a><a-progress percent={this.percent} status="active" /></a>
                  </a-popconfirm>
                </a-form-item>
              </a-col>
              <a-col sm={24} md={24}>
                <a-form-item
                  label="完成情况描述"
                  label-col={{ span: 2 }} wrapper-col={{ span: 22 }}
                >
                  <a-textarea
                    axios={this.axios}
                    placeholder="请输入完成情况描述"
                    value={this.description}
                    onChange={$event => {
                      this.description = $event.target.value;
                    }}
                  />
                </a-form-item>
              </a-col>
              <a-col sm={24} md={24}>
                <a-form-item label="附件"
                  label-col={{span:2}}
                  wrapper-col={{span:22}}
                >
                  <SmFileManageSelect
                    class="ant-input"
                    axios={this.axios}
                    enableDownload={true}
                    height={40}
                    bordered={true}
                    onChange={value => value.map(item => this.fileIds.push({id: item.id}))}
                  />
                </a-form-item>
              </a-col>
            </a-row>
          </a-tab-pane>
        </a-tabs>
      </div>
    ); 

    return (
      <div class="sm-task-task">
        <div class="SmTaskTask">
          {this.pageState == PageState.Edit ? 
            <SmFlowBase
              nodes={this.nodes}
              height={400}
              width={600}
            >

            </SmFlowBase>
            : undefined
          }
          <a-form form={this.form}>
            <a-row gutter={24} >
              <a-col sm={12} md={12}>
                <a-form-item
                  label="任务主题"
                  label-col={{ span: 4 }} wrapper-col={{ span: 20 }}
                >
                  <a-input
                    placeholder={this.PageState == PageState.Add ? '' : '请输入主题'}
                    disabled={this.pageState == PageState.Add ? false : (this.isShow || (this.loadingMemberId == undefined || this.loadingMemberId != this.creatorId))}
                    v-decorator={[
                      'name',
                      {
                        initialValue: '',
                        rules: [
                          { required: true, message: '请输入主题',whitespace: true },
                          //{ max: 50, message: '标题最多输入30字符' },
                        ],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              <a-col sm={12} md={12}>
                <a-form-item
                  label="优先级"
                  label-col={{ span: 4 }} wrapper-col={{ span: 20 }}
                >
                  <a-select
                    allowClear={false}
                    placeholder={this.PageState == PageState.Add ? '' : '请选择任务优先级'}
                    disabled={this.pageState == PageState.Add ? false : (this.isShow || (this.loadingMemberId == undefined || this.loadingMemberId != this.creatorId) || this.hasParent)}
                    v-decorator={[
                      'priority',
                      {
                        initialValue: null,
                        rules: [
                          {
                            required: true,
                            message: '请选择任务优先级',
                          },
                        ],
                      },
                    ]}
                  >
                    {_priorityType}
                  </a-select>
                </a-form-item>
              </a-col>
              <a-col sm={12} md={12}>
                <a-form-item
                  label="开始时间"
                  label-col={{ span: 4 }} wrapper-col={{ span: 20 }}
                >
                  <a-date-picker
                    style="width:100%"
                    showTime
                    format="YYYY-MM-DD HH:mm:ss"
                    disabled={this.pageState == PageState.Add ? false : (this.isShow || (this.loadingMemberId == undefined || this.loadingMemberId != this.creatorId) || this.hasParent)}
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
              </a-col>
              <a-col sm={12} md={12}>
                <a-form-item
                  label="结束时间"
                  label-col={{ span: 4 }} wrapper-col={{ span: 20 }}
                >
                  <a-date-picker
                    style="width:100%"
                    showTime
                    format="YYYY-MM-DD HH:mm:ss"
                    disabled={this.pageState == PageState.Add ? false : (this.isShow || (this.loadingMemberId == undefined || this.loadingMemberId != this.creatorId))}
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
              </a-col>
              <a-col sm={24} md={24}>
                <a-form-item label="指派给" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                  {/* {this.isShow || this.pageState == PageState.Edit ? 
                    <a-input
                      axios={this.axios}
                      disabled={this.isShow || (this.loadingMemberId == undefined || this.loadingMemberId != this._task.creatorId)}
                      v-decorator={[
                        'manager',
                        {
                          initialValue: '',
                        },
                      ]}
                    >
                    </a-input> 
                    : */}
                  <SmSystemMenbersSelect
                    axios={this.axios}
                    height={50}
                    showUserTab={true}
                    placeholder={'请选择用户'}
                    disabled={this.pageState == PageState.Add ? false : (this.isShow || (this.loadingMemberId == undefined || this.loadingMemberId != this.creatorId))}
                    v-decorator={[
                      'manager',
                      {
                        initialValue: [],
                      },
                    ]}
                  />
                  
                </a-form-item>
              </a-col>
              <a-col sm={24} md={24}>
                <a-form-item label="抄送给" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                  <SmSystemMenbersSelect
                    axios={this.axios}
                    height={50}
                    showUserTab={true}
                    placeholder={'请选择用户'}
                    disabled={this.pageState == PageState.Add ? false : (this.isShow || (this.loadingMemberId == undefined || this.loadingMemberId != this.creatorId))}
                    v-decorator={[
                      'ccPerson',
                      {
                        initialValue: [],
                        rules: [
                          {
                            required: true,
                            message: '请选择用户',
                          },
                        ],
                      },
                    ]}
                  />
                  
                </a-form-item>
              </a-col>
              <a-col sm={24} md={24}>
                <a-form-item
                  label="任务内容"
                  label-col={{ span: 2 }} wrapper-col={{ span: 22 }}
                >
                  {/* {this.isShow ? 
                    <a-textarea
                      value={this.content}
                      axios={this.axios}
                      disabled={this.isShow || (this.loadingMemberId == undefined || this.loadingMemberId != this.creatorId) || this.hasParent}
                    >
                    </a-textarea> : */}
                  <RichTextEditor
                    disabled={this.pageState == PageState.Add ? false : (this.isShow ||  (this.loadingMemberId == undefined || this.loadingMemberId != this.creatorId)) || this.hasParent}
                    ref="sc-rich-text-editor"
                    axios={this.axios}
                    value={this.content}
                  />
                  
                </a-form-item>
              </a-col>
              <a-col sm={24} md={24}>
                <a-form-item label="附件"
                  label-col={{span:2}}
                  wrapper-col={{span:22}}
                >
                  <SmFileManageSelect
                    class="ant-input"
                    axios={this.axios}
                    disabled={this.pageState == PageState.Add ? false : (this.isShow || (this.loadingMemberId == undefined || this.loadingMemberId != this.creatorId) || this.hasParent)}
                    enableDownload={true}
                    height={40}
                    bordered={true}
                    v-decorator={[
                      'taskRltFiles',
                      {
                        initialValue: [],
                      },
                    ]}
                  />
                </a-form-item>
              </a-col>
              {this.pageState == PageState.Add
                ?
                <a-col sm={24} md={24}>
                  <a-form-item
                    label="启动子任务"
                    label-col={{ span: 2 }} wrapper-col={{ span: 22 }}
                  >
                    <a-switch onChange={checked => this.startTask(checked)} />
                  </a-form-item>
                </a-col>: undefined
              }
            </a-row>
            
          </a-form>
          
        </div>
        {this.isShow ?
          feedback
          : undefined
        }
        {this.isSwitch || (this.pageState != PageState.Add && this._task != null && this._task.childrenTasks.length != 0) ? 
          childrenTask
          : undefined
        }
        <div style="float: right;marginTop: 15px;">
          
          <a-button style="marginRight: 15px;" type="primary" onClick={() => { this.save(false); }} loading={this.loading}>保存</a-button>
          {this.pageState == PageState.Add ? 
            <a-button style="marginRight: 15px;" type="primary" onClick={() => { this.isSubmit (); }} loading={this.loading}>提交</a-button> 
            : undefined}
          <a-button type="danger" onClick={() => {this.cancel(); }}>返回</a-button>
        </div>

        <SmSystemMenbers
          axios={this.axios}
          visible={this.userSelectVisible}
          showUserTab={true}
          onChange={iValue => {
            this.userSelectVisible = iValue;
          }}
          onGetMemberInfo={
            value => {
              this.addTaskInfo(value);
            }
          }
        />
      </div>
    );
  },
};
    
