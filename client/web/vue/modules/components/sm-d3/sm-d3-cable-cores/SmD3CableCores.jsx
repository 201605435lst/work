import './style/index.less';
import ApiCableCore from '../../sm-api/sm-resource/CableCore';
import { requestIsSuccess, getCableCoreType } from '../../_utils/utils';
let apiCableCore = new ApiCableCore();

export default {
  name: 'SmD3CableCores',
  props: {
    axios: { type: Function, default: null },
    visible: { type: Boolean, default: false }, //面板是否弹出
    position: {
      type: Object, default: () => {
        return { left: '280px', bottom: '20px' };
      },
    },
    height: { type: String, default: '60%' },
    equipment: { type: Object, default: null },
  },
  data() {
    return {
      iVisible: false,
      iEquipment: null,
      cableCores: [],
      currentEditId: '',
      currentId: '',
      cableType: '',
      businessFunction: '',
      canEdit: false,
    };
  },

  computed: {
    title: function () {
      let title = ``;
      if (this.iEquipment) {
        title += `（${this.iEquipment.name}）`;
      }
      return title;
    },
    iCableCores() {
      return this.cableCores;
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
        let _iequipment = this.iEquipment;
        this.iEquipment = this.equipment;
        if (_iequipment == null) {
          this.getCableCoreList();
        }
      },
      immediate: true,
    },
  },

  async created() {
    this.initAxios();
  },

  mounted() {
  },

  methods: {
    initAxios() {
      apiCableCore = new ApiCableCore(this.axios);
    },
    async getCableCoreList() {
      if (!this.equipment) {
        return;
      }
      let response = await apiCableCore.getList({ cableId: this.iEquipment.id });
      if (requestIsSuccess(response)) {
        this.cableCores = response.data;
      }
    },
    edit(item) {
      this.currentEditId = item.terminalLinkId;
      this.businessFunction = item.businessFunction;
      this.currentId = item.id;
      this.cableType = item.type;
    },
    async saveData() {
      let response = await apiCableCore.updateTerminalLink({
        terminalLinkId: this.currentEditId,
        businessFunction: this.businessFunction,
      });
      if (requestIsSuccess(response)) {
        this.getCableCoreList();
        this.currentEditId = null;
      }
    },
    async saveType() {
      let response = await apiCableCore.updateCableCore({
        id: this.currentId,
        type: this.cableType,
      });
      if (requestIsSuccess(response)) {
        this.getCableCoreList();
        this.currentId = null;
      }
    },
  },
  render() {
    return (
      <sc-panel
        class="sm-d3-cablecore"
        bordered
        borderedRadius
        visible={this.iVisible}
        position={this.position}
        bodyFlex
        bodyFlexDirection="row"
        height={this.height}
        animate="bottomEnter"
        forceRender
        resizable
        onClose={visible => {
          this.iVisible = visible;
          this.$emit('close', this.iVisible);
        }}
      >
        <si-cable slot="icon" size={20} style="margin-bottom: -2px;" />
        <span
          slot="title"
          title="刷新设备"
          class="header-btn"
          style={{
            'margin-left': '-8px',
          }}
          onClick={this.getCableCoreList}
        >
          <span>电缆芯</span>
          <span
            style={{
              color: this.equipment && this.iEquipment.id === this.equipment.id ? 'inherit' : 'red',
            }}
          >
            {this.iEquipment ? [
              this.title,
              <span > <a-icon type="sync" style="margin:0 8px" />刷新设备</span>,
            ] :
              <span style="margin:0 8px" >（请选择设备）</span>}
          </span>
        </span>

        <div class="core-row core-header" >
          <div>名称</div>
          <div>端子A</div>
          <div>端子B</div>
          <div>类型</div>
          <div>业务</div>
        </div>
        <div class="core-body" >
          {
            this.iCableCores.map(item => {
              return (
                <div class="core-row">
                  <div>{item.name}</div>
                  <div><span class="terminal" onClick={() => this.$emit('flyTo', { groupName: item.equipmentAGroupName, name: item.equipmentAName })}>({item.equipmentAName})</span>{item.terminalAName}</div>
                  <div><span class="terminal" onClick={() => this.$emit('flyTo', { groupName: item.equipmentBGroupName, name: item.equipmentBName })}>({item.equipmentBName})</span>{item.terminalBName}</div>
                  <div
                    onBlur={() => this.currentId = null}
                  >{this.currentId === item.id ?
                      <a-select width="100%" size="small"
                        value={this.cableType}
                        onSelect={(v) => this.cableType = v}
                        onBlur={() => this.saveType()}>
                        <a-select-option value={0}>未定义</a-select-option>
                        <a-select-option value={1}>电缆芯</a-select-option>
                        <a-select-option value={2}>光缆芯</a-select-option>
                      </a-select> :
                      <span
                        onClick={() => { this.edit(item); }}
                      >
                        {getCableCoreType(item.type)}
                      </span>
                    }
                  </div>
                  <div>
                    {item.terminalBusinessPaths.map(path => {
                      return <a-button
                        style="padding: 0;"
                        size="small"
                        type="link"
                        title="详情"
                        onClick={() => {
                          if (path && path.id) {
                            this.$emit('businessPathClick', path.id);
                          }
                        }}
                      >
                        {path.name}
                      </a-button>;
                    })
                    }
                    {/* {this.currentEditId === item.terminalLinkId ?
                      <a-input
                        size="small"
                        ref={`businessInput_${item.terminalLinkId}`}
                        onChange={(e) => this.businessFunction = e.target.value}
                        value={this.businessFunction}
                        onBlur={() => this.saveData()} /> :
                      <span>{item.businessFunction}</span>
                    } */}
                  </div>
                </div>
              );
            })
          }

        </div>
      </sc-panel >
    );
  },
};