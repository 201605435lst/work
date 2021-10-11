import './style/index.less';
import { requestIsSuccess, parseScope, getCableCoreType } from '../../_utils/utils';
import { CableCoreType } from '../../_utils/enum';
import { treeArrayLoop } from '../../_utils/tree_array_tools';

import SmD3TerminalLinkNodesModal from './src/SmD3TerminalLinkNodesModal';

import ApiTerminal from '../../sm-api/sm-resource/Terminal';
import ApiTerminalBusiness from '../../sm-api/sm-resource/TerminalBusiness';
import { template } from 'lodash';
let apiTerminal = new ApiTerminal();
let apiTerminalBusiness = new ApiTerminalBusiness();

export default {
  name: 'SmD3TerminalLink',
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false }, //面板是否弹出
    position: {
      type: Object, default: () => {
        return { left: '280px', bottom: '20px' };
      },
    },
    height: { type: String, default: '60%' },
    width: { type: String, default: '780px' },
    equipment: { type: Object, default: null },
  },
  data() {
    return {
      iVisible: false,
      terminalBusinessPath: {
        id: null,
        name: '',
        remark: '',
        nodes: [],
      },
      terminals: [],
      terminalActived: null,
      terminalLast: null,
      iEquipment: null,
      shwoInfo: false,
      pathLocked: true,
      loading: false,
    };
  },

  computed: {
    title: function () {
      return `端子配线（${this.iEquipment ? '设备：' + this.iEquipment.name : '请选择设备'}）`;
    },
    nodesLast: function () {
      return this.nodes[this.nodes.length - 1];
    },
    nodesLastPrev: function () {
      return this.nodes[this.nodes.length - 2];
    },
    nodes: function () {
      return this.terminalBusinessPath.nodes;
    },
  },

  watch: {
    visible: {
      handler: function (value, oldValue) {
        this.iVisible = value;
      },
      immediate: true,
    },
    equipment: {
      handler: function (value, oldValue) {
        this.iEquipment = this.iEquipment || this.equipment;
      },
    },
    iEquipment: {
      handler: function (value, oldValue) {
        this.initTerminals();
      },
      immediate: true,
    },
  },

  async created() {
    this.initAxios();
    this.initTerminals();
  },

  mounted() {

  },

  methods: {
    initAxios() {
      apiTerminal = new ApiTerminal(this.axios);
      apiTerminalBusiness = new ApiTerminalBusiness(this.axios);
    },
    async initTerminalBusinessPath(id) {
      let response = await apiTerminalBusiness.get({ id });
      if (requestIsSuccess(response)) {
        this.terminalBusinessPath = response.data;
      }
    },
    async initTerminals() {
      if (!this.iEquipment) {
        return;
      }
      let equipment = this.iEquipment;
      let response = await apiTerminal.getListByEquipmentId({ equipmentId: equipment.id });
      if (requestIsSuccess(response)) {
        response.data.map(item => item.equipment = equipment);
        this.terminals = response.data;

        this.$nextTick(() => {
          if (this.terminalActived) {
            let activedRow = this.$refs[this.terminalActived.id];

            if (activedRow) {
              activedRow.parentNode.scrollTo(0, activedRow.offsetTop - 90);
            }
          }
        });
      }
    },
    isPair(a, b) {
      let reg = /[NW]/g;
      if (a.equipmentId == b.equipmentId && a.name != b.name && a.name.replace(reg, '') == b.name.replace(reg, '')) {
        return true;
      }
      return false;
    },
    /**
     * 目标端子点击
     * @param {*} target 端子
     * @param {*} cableCore 线缆芯
     */
    onTerminalClick(target, cableCore) {
      this.terminalLast = this.terminalActived;
      this.terminalActived = target;
      this.iEquipment = target.equipment;
      // 1. nodes 为空时添加
      // 2. nodes 不为空 && 不能在已有nodes列表中 && 必须是最后一个端子的直连端子

      if (!this.pathLocked) {
        this.terminalBusinessPath.nodes.push({ terminal: target, cableCore });
      }
      else if (this.nodes.length === 0) {
        this.terminalBusinessPath.nodes.push({ terminal: target, cableCore });
      } else {
        let canPush = false;

        if (!this.nodesLast) {
          canPush = true;
        }
        // 不能重复添加
        else if (this.nodes.find(x => x.terminal.id === target.id) == null) {
          if (this.nodesLast.terminal.terminalLinks &&
            this.nodesLast.terminal.terminalLinks.find(x => x.terminalAId === target.id || x.terminalBId === target.id) != null
          ) {
            canPush = true;
          }
          else if (this.nodesLast.terminal.equipmentId == target.equipmentId && this.isPair(this.nodesLast.terminal, target)) {
            canPush = true;
          }
        }

        if (canPush) {
          this.terminalBusinessPath.nodes.push({ terminal: this.terminalActived, cableCore });
        }
      }

      if (this.terminalLast && this.terminalLast.id != target.id) {
        this.iEquipment = target.equipment;
        this.$emit('change', this.iEquipment);
      }
    },
    /**
     * 当前端子点击
     * @param {*} terminal 端子
     */
    onCurrentTerminalClick(terminal) {
      this.terminalActived = terminal;
      this.terminalActived.equipment = this.iEquipment;
      if (this.nodes.length == 0 || !this.pathLocked) {
        this.nodes.push({ terminal: this.terminalActived, cableCore: null });
      }
    },
    /**
     *  路径端子点击
     * @param {*} target 端子
     */
    onNodesTerminalClick(terminal) {

      if (this.terminalActived && this.terminalActived.id != terminal.id || !this.terminalActived) {
        this.terminalLast = this.terminalActived;
        this.terminalActived = terminal;
        this.iEquipment = terminal.equipment;
        this.$emit('change', this.iEquipment);
      }
    },
    /**
     * 移除路径节点
     * @param {*} node
     */
    removePathNode(node) {
      let index = this.nodes.findIndex(x => x.terminal.id === node.terminal.id);
      this.terminalBusinessPath.nodes.splice(index, this.nodes.length - index);
    },

    /**
     * 渲染目标节点
     * @param {*} item 端子
     */
    renderTargetTerminal(item) {
      return item.terminalLinks.map(link => {
        let target;
        if (link.terminalB && link.terminalAId === item.id) {
          target = link.terminalB;
        } else if (link.terminalA && link.terminalBId === item.id) {
          target = link.terminalA;
        }
        let _this = this;
        return (
          <div
            class="target-box">

            <a-popover title="电缆信息" trigger="hover">
              {
                link.cableCore != null || link.terminalBusinessPaths != null ?
                  <template slot="content">
                    {
                      link.cableCore != null ?
                        [<p>
                          电缆：
                          <a-button
                            style="padding: 0;"
                            size="small"
                            type="link"
                            onClick={() => {
                              let equipmentPair = {
                                groupName: link.cableCore.cable.group.name,
                                name: link.cableCore.cable.name,
                              };
                              this.$emit('flyTo', equipmentPair);
                              this.$emit('select', equipmentPair);
                            }}>
                            {link.cableCore.cable.name}
                          </a-button>
                        </p>,
                        <p>缆芯：{link.cableCore.name}</p>,
                        <p>类型：{getCableCoreType(link.cableCore.type)}</p>,
                        ] :
                        null
                    }

                    {link.terminalBusinessPaths ?
                      link.terminalBusinessPaths.map(item => {
                        console.log(item);
                        return <p>业务：
                          <a-button
                            style="padding: 0;"
                            size="small"
                            type="link"
                            title="详情"
                            onClick={() => {
                              _this.initTerminalBusinessPath(item.id);
                            }}
                          >
                            {item.name}
                          </a-button>
                        </p>;
                      }) :
                      null}
                  </template> : undefined
              }
              <div
                class={{
                  'nodes-line': true,
                  'cable': link.cableCore != null,
                  'business': link.terminalBusinessPaths != null && link.terminalBusinessPaths.length,
                  'cable-type-electric': link.cableCore != null && link.cableCore.type === CableCoreType.Electric,
                  'cable-type-optical': link.cableCore != null && link.cableCore.type === CableCoreType.Optical,
                }}
              >
                {link.cableCore != null ? <span class="cable-name" >{link.cableCore.cable.name}</span> : undefined}
                {link.cableCore != null ? <span class="cable-core-name" >{link.cableCore.name}</span> : undefined}
              </div>
            </a-popover>

            <div
              class={{
                "terminal-item": true,
                target: true,
                'nodes-last': this.nodesLast && this.nodesLast.terminal.id === target.id,
                'nodes-last-prev': this.nodesLastPrev && this.nodesLastPrev.terminal.id === target.id,
              }}
              onClick={() => {
                this.onTerminalClick(target, link.cableCore);
              }}
            >
              <span>
                <span class="terminal-icon"> <si-terminal /> </span>
                <span class="equipment-name">{`（${target.equipment ? target.equipment.name : ''}） ${target.name}`} </span>
              </span>

              <span class="btns">
                {target.equipment && target.equipment.group ?
                  <span
                    class="btn"
                    onClick={(evt) => {
                      evt.stopPropagation();
                      evt.preventDefault();
                      this.$emit('flyTo', { groupName: target.equipment.group.name, name: target.equipment.name });
                    }}> <a-icon type="environment" />
                  </span> : null
                }
              </span>
            </div>
          </div >
        );
      });
    },
  },
  render() {

    let content = [
      <sc-panel
        class="cable-link"
        flex="3"
        bordered={false}
        showHeaderClose={false}
      >
        <a-icon slot="icon" type="box-plot"></a-icon>
        <span slot="title">径路{this.terminalBusinessPath.id ? `：${this.terminalBusinessPath.name}` : ''}</span>
        <template slot="headerExtraContent">
          {/* 添加径路 */}
          {
            this.terminalBusinessPath.id ?
              <span
                class="header-btn"
                onClick={async () => {
                  this.$refs.SmD3TerminalLinkNodesModal.edit(this.terminalBusinessPath);
                }}
              >
                <a-icon type="save"></a-icon>
              </span> :
              <span
                class="header-btn"
                onClick={async () => {
                  this.$refs.SmD3TerminalLinkNodesModal.add(this.terminalBusinessPath);
                }}
              >
                <a-icon type="plus"></a-icon>
              </span>
          }

          {/* 删除 */}
          {this.nodes.length ?
            <span
              class="header-btn"
              onClick={() => {
                let _this = this;
                if (this.terminalBusinessPath.id) {
                  this.$confirm({
                    title: "确定要删除吗",
                    content: h => <div style="color:red;">删除后不可恢复</div>,
                    okType: 'danger',
                    onOk() {
                      return new Promise(async (resolve, reject) => {
                        let response;
                        if (_this.terminalBusinessPath && _this.terminalBusinessPath.id) {
                          response = await apiTerminalBusiness.delete(_this.terminalBusinessPath.id);
                          _this.terminalBusinessPath = { id: null, name: '', remark: '', nodes: [] };
                        }
                        setTimeout(requestIsSuccess(response) ? resolve : reject, 100);
                      });
                    },
                  });
                } else {
                  _this.terminalBusinessPath = { id: null, name: '', remark: '', nodes: [] };
                }
              }}
            >
              <a-icon type="delete"></a-icon>
            </span> :
            undefined
          }

          {/* 关闭 */}
          {this.nodes.length && this.terminalBusinessPath ?
            <span
              class="header-btn"
              onClick={() => {
                this.terminalBusinessPath = { id: null, name: '', remark: '', nodes: [] };
              }}
            >
              <a-icon type="close"></a-icon>
            </span> :
            undefined
          }

          <span
            class="header-btn"
            style={{
              color: !this.pathLocked ? "red" : "inherit",
            }}
            onClick={async () => {
              this.pathLocked = !this.pathLocked;
            }}
          >
            <a-icon type={!this.pathLocked ? "unlock" : "lock"}></a-icon>
          </span>
        </template>


        <div class="nodes">
          {this.nodes.map((item, index) => {
            let { terminal } = item;
            let cableCore = this.nodes[index + 1] ? this.nodes[index + 1].cableCore : null;
            return (
              <div
                class={{
                  "nodes-item": true,
                  'nodes-last': this.nodesLast && this.nodesLast.terminal.id === terminal.id,
                  'nodes-last-prev': this.nodesLastPrev && this.nodesLastPrev.terminal.id === terminal.id,
                }}
              >
                <div class="index">
                  <div class='terminal-index'>{index + 1}</div>
                </div>
                <div class="icon-col">
                  <div class='nodes-icon'><si-terminal /></div>
                  {cableCore != null ?
                    <a-popover title="电缆信息" trigger="hover">
                      <template slot="content">
                        <p>
                          电缆：
                          <a-button
                            style="padding: 0;"
                            size="small"
                            type="link"
                            onClick={() => {
                              let equipmentPair = {
                                groupName: cableCore.cable.group.name,
                                name: cableCore.cable.name,
                              };
                              this.$emit('flyTo', equipmentPair);
                              this.$emit('select', equipmentPair);
                            }}>
                            {cableCore.cable.name}
                          </a-button>
                        </p>
                        <p>缆芯：{cableCore.name}</p>
                        <p>类型：{getCableCoreType(cableCore.type)}</p>
                      </template>
                      <div
                        class={{
                          'nodes-line': true,
                          'cable': cableCore != null,
                          'cable-type-electric': cableCore != null && cableCore.type === CableCoreType.Electric,
                          'cable-type-optical': cableCore != null && cableCore.type === CableCoreType.Optical,
                        }}
                      >
                        <span class="cable-name">{`${cableCore.cable.name} / ${cableCore.name}`}</span>
                      </div>
                    </a-popover>
                    : <div class='nodes-line'></div>
                  }
                </div>
                <div class="terminal">
                  <div class='terminal-title'>
                    <span
                      class='terminal-name'
                      onClick={() => {
                        this.onNodesTerminalClick(terminal);
                      }}>
                      {`（${terminal.equipment.name}）${terminal.name}`}
                    </span>
                    <span class="btns">
                      <span
                        title="定位"
                        class="btn"
                        onClick={() => {
                          this.$emit('flyTo', { groupName: terminal.equipment.group.name, name: terminal.equipment.name });
                        }}
                      >
                        <a-icon type="environment" />
                      </span>
                      <span
                        title="删除"
                        class="btn"
                        onClick={() => {
                          this.removePathNode(item);
                        }}
                      >
                        <a-icon type="delete" />
                      </span>
                    </span>
                  </div>
                </div>
              </div>
            );
          })}
        </div>
      </sc-panel >,

      <sc-panel
        flex="6"
        bordered={false}
        showHeader={false}
        class="terminals"
      >
        <div class="terminal-row terminals-header" >
          <div>当前端子</div>
          <div style="max-width: 20px"></div>
          {/* <div>设备端子{this.iEquipment ? `（${this.iEquipment.name}）` : ''}</div> */}
          <div>目标端子</div>
        </div>
        <div class="terminals-body" >
          {this.terminals.map((item, index) => {
            return (
              <div class="terminal-row" ref={item.id}>
                <div
                  class={{
                    'terminal-item': true,
                    'current': true,
                    actived: this.terminalActived && this.terminalActived.id === item.id,
                    'nodes-last': this.nodesLast && this.nodesLast.terminal.id === item.id,
                    'nodes-last-prev': this.nodesLastPrev && this.nodesLastPrev.terminal.id === item.id,
                  }}
                  onClick={() => {
                    this.onTerminalClick(item, null);
                  }}
                >
                  <span class='terminal-name'> {`${item.name}`}</span>
                  <span class='terminal-icon'><si-terminal /></span>
                </div>

                <div class={{ "terminal-line": true, avitved: item.terminalLinks.length > 0 }} >
                  <span class="line" ></span>
                </div>

                <div class="targets">
                  {this.renderTargetTerminal(item)}
                </div>

              </div>
            );
          })}
        </div>
      </sc-panel >,

      this.terminalActived && this.shwoInfo ?
        <sc-panel
          class="terminal-info"
          title="端子信息"
          flex="4"
          bordered={false}
          showHeaderClose={false}
        >
          <si-terminal slot="icon" />

          {this.terminalActived ? [
            <div class="item">
              <div class="label">名称</div>
              <div class="value">{this.terminalActived.name}</div>
            </div>,
            // <div class="item">
            //   <div class="label">配线</div>
            //   <div class="value">{this.terminalActived.terminalLinkAs.length + this.terminalActived.terminalLinkBs.length}个</div>
            // </div>,
          ] : undefined}
        </sc-panel> : undefined
      ,
    ];

    return (
      <sc-panel
        class="sm-d3-terminal-link"
        bordered
        borderedRadius
        visible={this.iVisible}
        position={this.position}
        bodyFlex
        bodyFlexDirection="row"
        height={this.height}
        width={this.width}
        animate="bottomEnter"
        forceRender
        resizable
        title={this.title}
        onClose={visible => {
          this.iVisible = visible;
          this.$emit('close', this.iVisible);
        }}
      >
        <a-icon slot="icon" type="control" />

        {
          this.iEquipment ?
            <template slot="headerExtraContent" >
              <span
                title="刷新"
                class="header-btn"
                style={{ color: this.iEquipment === this.equipment ? 'inherit' : 'red' }}
                onClick={() => {
                  this.iEquipment = this.equipment;
                }}
              >
                <a-icon type="redo" />
              </span>

              <span
                title="端子信息"
                class="header-btn"
                onClick={() => {
                  this.shwoInfo = !this.shwoInfo;
                }}
              >
                <a-icon type="line-chart" />
              </span>

              <span
                title="定位设备"
                class="header-btn"
                onClick={() => {
                  this.$emit('flyTo', { groupName: this.iEquipment.group.name, name: this.iEquipment.name });
                }}
              >
                <a-icon type="environment" />
              </span>


            </template>
            : undefined
        }

        {
          content && this.iVisible ? (
            content
          ) : (
            <span
              style="
              margin-top: 10px;
              color: #bdbdbd;
              "
            >
                无数据
            </span>
          )
        }

        <SmD3TerminalLinkNodesModal axios={this.axios} ref="SmD3TerminalLinkNodesModal" />
      </sc-panel >
    );
  },
};
