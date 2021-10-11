export default {
  name: 'BpmNode',
  components: {},
  props: {
    node: { type: Object, default: null },
    showDetail: { type: Boolean, default: false },
    isView: { type: Boolean, default: false }, //默认为查看模式，即可以看见节点名称和其他信息
  },
  data() {
    return {};
  },
  computed: {
    startNode() {
      return this.node.type === 'bpmStart';
    },
    endNode() {
      return this.node.type === 'bpmEnd';
    },
    nodeActive() {
      return this.node.active;
    },
    defaultNode() {
      return this.node.type === 'bpmApprove';
    },
  },
  watch: {},
  created() {
  },
  methods: {},
  render() {
    return (
      <div class="bpm-node">
        <div
          class={`node-container 
        ${this.startNode ? 'start-node' : null}
        ${this.defaultNode ? 'default-node' : null}
        ${this.endNode ? 'end-node' : null}
        ${this.nodeActive ? 'active' : null}
        `}
        >
          <div class="node-title start-title">
            <span>{this.node.name}</span>
          </div>
          <div class="node-body">
            {this.startNode ? (
              <div>
                <p>
                  <span class="key">发起人:</span>
                  <span>{this.node.creator}</span>
                </p>
                {this.isView ? undefined : 
                  <p>
                    <span class="key">发起时间:</span>
                    <span>{this.node.date}</span>
                  </p>}
              </div>
            ) : null}
            {this.endNode ? (
              this.node.date === ''|| this.node.date == '0001-01-01' || this.isView ? (
                <div>
                  <p class="end-title">
                    <span>流程未结束</span>
                  </p>
                </div>
              ) : (
                <div>
                  <p class="end-title-com">
                    <span>
                      <a-tag color="red">审核完成</a-tag>
                    </span>
                  </p>
                  <p class="end-title-com">
                    <span>时间：{this.node.date}</span>
                  </p>
                </div>
              )
            ) : null}
            {this.defaultNode ? (
              <div>
                <p>
                  <span class="key">审核人:</span>
                  <span>
                    {this.node.approvers.map(a => {
                      return a.name + ' ';
                    })}
                  </span>
                </p>
                {this.isView ? undefined : 
                  this.nodeActive ? (
                    <p>
                      <span class="key">流程状态:</span>
                      <span>
                        <a-tag color="orange">审核中...</a-tag>
                      </span>
                    </p>
                  ) : this.node.date === '' || this.node.date == '0001-01-01' ? (
                    <p>
                      <p>
                        <span class="key">流程状态:</span>
                        <span>
                          <a-tag color="purple">待审核...</a-tag>
                        </span>
                      </p>
                    </p>
                  ) : (
                    <p>
                      <span class="key">流程状态:</span>
                      {this.showDetail ? (
                        <span>
                          <a-popover
                            placement="top"
                            content={this.node.comments.map(a => {
                              return (
                                <p>
                                  <span>
                                    {a.approveTime}:{a.content}
                                  </span>
                                </p>
                              );
                            })}
                            title="审核意见"
                          >
                            <a-tag color="green">已审核</a-tag>
                          </a-popover>
                        </span>
                      ) : (
                        <a-tag color="green">已审核</a-tag>
                      )}
                    </p>
                  )}
              </div>
            ) : null}
          </div>
        </div>
      </div>
    );
  },
};
