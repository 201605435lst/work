import { BpmMessageType,WorkflowState } from "../../_utils/enum";
import moment from "moment";
import './style';
import ApiSkylightPlan from '../../sm-api/sm-cr-plan/SkyLightPlan';
import * as utils from '../../_utils/utils';
import ApiUser from '../../sm-api/sm-system/Account';

let apiSkylightPlan = new ApiSkylightPlan();
let apiUser = new ApiUser();

let bpmMessage = null;
let noticeMessage = null;
export default {
  name: 'SmMessageNotice',
  components: {},
  props: {
    signalr: { type: Function, default: null },
    axios:{type:Function,default:null},
  },
  data() {
    return {
      loading: false,
      visible: false,
      tooltip: false,
      bpmTopic: 'bpm',
      bpmMessageMethod: 'ReceiveMessage',
      noticeTopic: 'notice',
      noticeMessageMethod: 'ReceiveMessage',
      noticeArray: [], // 通知类消息
      messageArray: [], // 消息类消息
      todoArray: [], // 待办类消息
      message: [],
      currentUserId:null,//当前登录信息
    };
  },
  computed: {
    messageCount() {
      return this.noticeArray.length + this.messageArray.length + this.todoArray.length;
    },
    noticeList() {
      return this.noticeArray;
    },
    messageList() {
      return this.messageArray;
    },
    todoList() {
      return this.todoArray;
    },
  },
  watch: {},
  created() {
    this.initAxios();
    this.initCurrentUser();
    this.initSignalr(); // 初始化消息驱动
  },
  methods: {
    initAxios() {
      apiSkylightPlan = new ApiSkylightPlan(this.axios);
      apiUser = new ApiUser(this.axios);
    },
    //获取当前登录信息
    async initCurrentUser() {
      let response = await apiUser.getAppConfig();
      if (utils.requestIsSuccess(response) && response.data) {
        this.currentUserId = response.data.currentUser.id;
      }

    },
    initSignalr() {
      //获取当前登录信息
      const SignalR = this.signalr;
      bpmMessage = new SignalR(this.bpmTopic);
      noticeMessage = new SignalR(this.noticeTopic);
      if (noticeMessage) {
        noticeMessage
          .init(this.initBpmMessageData)
          .on(this.noticeMessageMethod, this.receiveBpmMessageData);
      }
      if (bpmMessage) {
        bpmMessage
          .init(this.initBpmMessageData)
          .on(this.bpmMessageMethod, this.receiveBpmMessageData);
      }
    },
    initBpmMessageData(data) {
      if (data.length > 0) {
        data.map(item => {
          if (item.Content !== undefined) {
            let type = JSON.parse(item.Content).Type;
            if (type == "CrPlan") {
              this.todoArray.push(item);
            }
          } else {
            this.noticeArray.push(item);
          }
        });
      }
    },
    receiveBpmMessageData(type, data) {
      let content = JSON.parse(data.Content);
      if (content != undefined && content.Type == 'CrPlan' && data.UserId === this.currentUserId) {
        this.todoArray.push(data);
        // this.todoArray = this.todoArray.filter(item=>)
      } else if (type == this.bpmTopic) {
        this.noticeArray.push(data);
        this.tooltipTaggle();
      } 
    },

    fetchNotice() {
      if (!this.visible) {
        this.loading = true;
        setTimeout(() => {
          this.loading = false;
        }, 100);
      } else {
        this.loading = false;
      }
      this.visible = !this.visible;
    },
    tooltipTaggle() {
      this.tooltip = true;
      setTimeout(() => {
        this.tooltip = false;
      }, 1000);
    },
    messageItemClick(data) {
      this.$emit('detailsClick', data);
    },
    read(id) {
      if (bpmMessage) {
        bpmMessage.send('Process', id);
        this.noticeArray = this.noticeArray.filter(a => a.MessageId !== id);
      } else if (noticeMessage) {
        noticeMessage.send('Process', id);
        this.todoArray = this.todoArray.filter(a => a.Id !== id);
      }
    },
    //代办消息点击
    todoMessageClick() {
      this.$emit('todoProcess');
    },
    //代办消息确认
    async confirmTodoMessage(id) {
      let data = {
        messageId:id,
      };
      this.todoArray = this.todoArray.filter(item => item.Id != id);
      let response = await apiSkylightPlan.confirmTodoMessage(data);
      if (utils.requestIsSuccess(response) && response.data) {
        this.$emit("todoProcess");
      }
    },
  },
  render() {
    return (
      <a-popover
        v-model={this.visible}
        trigger="click"
        overlay-class-name="sm-message-notice"
        get-popup-container={() => this.$refs.noticeRef.parentElement}
        auto-adjust-overflow={true}
        arrow-point-atCenter={true}
        overlay-style={{ width: '300px', top: '50px' }}
      >
        <template slot="content">
          <a-spin spinning={this.loading}>
            <a-tabs>
              <a-tab-pane key="1">
                <span slot="tab">
                  <a-badge dot={this.noticeList.length > 0}>通知</a-badge>
                </span>
                <div class="sm-message-list">
                  {this.noticeList.map(item => {
                    let content = (
                      <div class="message-item" onClick={() => this.messageItemClick(item)}>
                        <div class="message-avatar">
                          <a-icon type="message" />
                        </div>
                        <div class="message-content">
                          {item.Type === BpmMessageType.Cc ? (
                            <p class="message-title">
                              <span class="state-d">{item.Sponsor}</span>提交的
                              <span class="state-d">{item.FlowName}</span>流程，请知晓！
                            </p>
                          ) : (
                            undefined
                          )}
                          {item.Type === BpmMessageType.Notice ? (
                            <p class="message-title">
                              <a>{item.Processor}</a>
                              {item.State === WorkflowState.Waiting ? (
                                <span class="state-d">通过了</span>
                              ) : null}
                              {item.State === WorkflowState.Rejected ? (
                                <span class="state-j">拒绝了</span>
                              ) : null}
                              {item.State === WorkflowState.Stopped ? (
                                <span class="state-z">终止了</span>
                              ) : null}
                              您提交的<a>{item.FlowName}</a>流程，请知晓！
                            </p>
                          ) : (
                            undefined
                          )}
                          {item.Type === BpmMessageType.Approval ? (
                            <p class="message-title">
                              <a>{item.Sponsor}</a>提交的<a>{item.FlowName}</a>
                              流程需要您的审批！，请知晓！
                            </p>
                          ) : (
                            undefined
                          )}
                          <p class="message-sub">
                            <span>{moment(item.CreateTime).format('yyyy-MM-DD HH:mm:ss')}</span>
                            <span>
                              <a-button
                                class=""
                                size="small"
                                onClick={() => this.read(item.MessageId)}
                              >
                                标记
                              </a-button>
                            </span>
                          </p>
                        </div>
                      </div>
                    );
                    return content;
                  })}
                </div>
                {this.noticeList.length === 0 ? <span>暂无消息</span> : null}
              </a-tab-pane>
              <a-tab-pane key="2">
                <span slot="tab">
                  <a-badge dot={this.messageList.length > 0}>消息</a-badge>
                </span>
              </a-tab-pane>
              <a-tab-pane key="3">
                <span slot="tab">
                  <a-badge dot={this.todoList.length > 0}>待办</a-badge>
                </span>
                <div class="sm-message-list">
                  {this.todoList.map(item => {
                    let planCntent = JSON.parse(item.Content);
                    let content = [
                      <div class="time" style="color:#0058f9">
                        {moment(item.CreationTime).format('yyyy-MM-DD HH:mm:ss')}
                      </div>,
                      <div class="sm-todo-list">
                        <div
                          class="content"
                          title={planCntent.PlanContent}
                          onClick={() => {
                            this.todoMessageClick();
                          }}
                        >
                          {planCntent.PlanContent}
                        </div>
                        <div>
                          <a-button size="small" class="mark" onClick={() => {
                            this.confirmTodoMessage(item.Id);
                          }}>
                            确认
                          </a-button>
                        </div>
                      </div>,
                    ];
                      
                    return content;
                  })}
                </div>
                {this.todoList.length === 0 ? <span>暂无消息</span> : null}
              </a-tab-pane>
            </a-tabs>
          </a-spin>
        </template>
        <span
          ref="noticeRef"
          class="header-notice"
          style="padding: 0 18px"
          onClick={() => this.fetchNotice()}
        >
          <a-badge count={this.messageCount} title={`您有${this.messageCount}条未读消息`}>
            <a-tooltip placement="bottom" visible={this.tooltip}>
              <template slot="title">
                <span>您有新消息</span>
              </template>
              <a-icon style="font-size: 16px; padding: 4px" type="bell" />
            </a-tooltip>
          </a-badge>
        </span>
      </a-popover>
    );
  },
};
