import './style';
import ApiSubItemContent from '../../sm-api/sm-construction-base/ApiSubItemContent';
import ApiSubItem from '../../sm-api/sm-construction-base/ApiSubItem';
import ApiProject from '../../sm-api/sm-project/Project';
import { requestIsSuccess } from '../../_utils/utils';
import { MoveTypeEnum} from '../sm-construction-base-sub-item/NodeTypeEnum';
import ApiProcedure from '../../sm-api/sm-construction-base/ApiProcedure';
import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import ApiDataDictionary from '../../sm-api/sm-system/DataDictionary';
import TabTable from '../sm-construction-base-procedure/TabTable';
import { TabEnum } from '../sm-construction-base-procedure/TabEnum';
import SmFileUpload from '../../sm-file/sm-file-manage/src/component/SmFileUpload';
import ApiFileManage from '../../sm-api/sm-file/fileManage';

let apiSubItemContent = new ApiSubItemContent();
let apiDataDictionary = new ApiDataDictionary();// 字典api 查工程类型用
let apiFileManage = new ApiFileManage();
let apiSubItem = new ApiSubItem();
let apiProject = new ApiProject();
let apiProcedure = new ApiProcedure();


export default {
  name: 'SetRltProcedureModal',
  props: {
    axios: { type: Function, default: null }, // axios
    border: { type: Boolean, default: true }, // 是否展示边框
    // 工程类型列表
    procedureTypes: { type: Array, default: () => [] },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      workers: [], // 公种列表
      equipmentTeams: [], // 设备台班列表
      materials: [], // 工程量列表
      files: [], // 文件列表
      selectFileIds: [], // 选中的文件ids
      selectMaterialIds: [], // 选中的工程量ids
      selectWorkerIds: [], // 选中的工种ids
      selectEquipmentTeamIds: [], // 选中的设备ids
      rltProcedureId: undefined, // 关联工序 id
      defaultKey: 'worker',  // 默认tab 切换到 worker 栏
    };
  },
  computed: {
    title() {
      // 计算模态框的标题变量
      return utils.getModalTitle(this.status);
    },
    visible() {
      // 计算模态框的显示变量
      return this.status !== ModalStatus.Hide;
    },
    // 工序 table 的 columns
    produceColumns() {
      return [
        { title: '工序名称', dataIndex: 'name'},
        { title: '工种', dataIndex: 'procedureWorkers', customRender: (text, record, index) => {
          let names = text.map(x => x.name);
          return this.getElement(text, names);
        }},
        { title: '工程量清单', dataIndex: 'procedureMaterials', customRender: (text, record, index) => {
          let names = text.map(x => x.name);
          return this.getElement(text, names);
        }},
        { title: '设备类型', dataIndex: 'procedureEquipmentTeams', customRender: (text, record, index) => {
          let names = text.map(x => x.name);
          return this.getElement(text, names);
        }},
        { title: '操作', customRender: (record) => {
          return (<div>
            <div onClick={() => this.moveRight(record)}><a>添加</a></div>
          </div>);
        }},
      ];
    },
    // 关联工序 table 的 columns
    rltProduceColumns() {
      return [
        { title: '工序名称', dataIndex: 'procedure', customRender: (text, record, index) => <span>{text.name}</span> },
        { title: '操作', customRender: (text,record,index) => {
          return (<div>
            <div style='display:inline' onClick={() => this.moveLeft(record,index)}><a>移出</a></div>
            <div style='display:inline;margin-left:10px' onClick={() => this.moveRltPro(record, MoveTypeEnum.Up)}><a>上移</a></div>
            <div style='display:inline;margin-left:10px' onClick={() => this.moveRltPro(record, MoveTypeEnum.Down)}><a>下移</a></div>
          </div>);
        }},
      ];
    },
    // worker table 的 columns
    workerColumns() {
      return [{ title: '工种名称', dataIndex: 'name' }];
    },
  },
  async created() {
    this.initAxios();
    await this.getMyFiles();
  },
  methods: {
    initAxios() {
      apiSubItemContent = new ApiSubItemContent(this.axios);
      apiDataDictionary = new ApiDataDictionary(this.axios);
      apiSubItem = new ApiSubItem(this.axios);
      apiProject = new ApiProject(this.axios);
      apiFileManage = new ApiFileManage(this.axios);
      apiProcedure = new ApiProcedure(this.axios);
    },
    // 给 string[] 加悬浮,不然列表放不下
    getElement(text, names) {
      let span = <span>{text.length === 1 ? names : names[0] + '...'}</span>;
      let toolTip = (<a-tooltip placement='bottom' title={names.join('  ,')}>{span}</a-tooltip>);
      return text.length === 0 ? undefined : toolTip;
    },

    // 获取 我自己 的文件列表
    async getMyFiles() {
      let { data: { items: res } } = await apiFileManage.getResourceList({ type: 2, isMine: true, size: 9999 });
      this.files = res;
    },

    // (模态框)自己调用打开自己的方法(工程量设置)
    async edit(id) {
      this.status = ModalStatus.Edit;
      console.log("rltProcedure的id 是", id);
      this.rltProcedureId = id;// 给 rltProcedureId  赋值
      await this.getRltProcedureRltOtherList(id); // 获取 关联表的列表
      await this.getSelectRltIdsByRltProcedureId(id); // 获取 关联的选择id 列表
    },
    // 根据  RltProcedureId 获取 关联 的选择 id 列表
    async getSelectRltIdsByRltProcedureId(rltProcedureId) {
      let res = await  apiSubItemContent.getSelectRltIdsByRltProcedureId(rltProcedureId);

      if (requestIsSuccess(res)) {
        this.selectFileIds= res.data.selectFileIds;
        this.selectMaterialIds= res.data.selectMaterialIds;
        this.selectWorkerIds= res.data.selectWorkerIds;
        this.selectEquipmentTeamIds= res.data.selectEquipmentTeamIds;
      }
    },

    // 获取 关联工序表 关联 的其他 表 的 列表 (worker ,material ,equipment )
    async getRltProcedureRltOtherList(rltProcedureId) {
      let res = await  apiSubItemContent.getRltProcedureRltOtherList(rltProcedureId);
      if (requestIsSuccess(res)) {
        this.workers = res.data.workers.map(x => ({ ...x, editable: false })); // 给列表的元素加一个可编辑 的属性
        this.equipmentTeams = res.data.equipmentTeams.map(x => ({ ...x, editable: false })); // 给列表的元素加一个可编辑 的属性
        this.materials = res.data.materials.map(x => ({ ...x, editable: false })); // 给列表的元素加一个可编辑 的属性
      }
    },
    // 关闭模态框
    close() {
      this.status = ModalStatus.Hide;
    },
    // Modal 提交
    ok: async function() {
      let data = {
        workerIds: this.selectWorkerIds.map((x) => {
          let index = this.workers.findIndex(m => m.id === x);
          if (index > -1) {
            return {
              workerId: x,
              count: this.workers[index].count,
            };
          }
        }),
      };
      return console.log(data);
      // let response = await apiProcedure.configProcedure(this.record.id, data1);
      // if (utils.requestIsSuccess(response)) {
      this.close();
      this.$message.success('关联工序成功');
      this.$emit('success');
      // }
    },
    // table 选择  id 后的事件
    tableSelectIds(data) {
      let { type: type, selectIds: selectIds, list: list } = data;
      switch (type) {
      case TabEnum.File:
        this.files = list;
        this.selectFileIds = selectIds;
        break;
      case TabEnum.Mat:
        this.materials = list;
        this.selectMaterialIds = selectIds;
        break;
      case TabEnum.EquipmentTeam:
        this.equipmentTeams = list;
        this.selectEquipmentTeamIds = selectIds;
        break;
      case TabEnum.Worker:
        this.workers = list;
        this.selectWorkerIds = selectIds;
        break;
      default:
        break;
      }
    },
    // tabTable 的回调
    tableOutList(data) {
      let { type: type, list: list } = data;
      switch (type) {
      case TabEnum.File:
        this.files = list;
        break;
      case TabEnum.Mat:
        this.materials = list;
        break;
      case TabEnum.EquipmentTeam:
        this.equipmentTeams = list;
        break;
      case TabEnum.Worker:
        this.workers = list;
        break;
      default:
        break;
      }
    },
  },
  render() {
    let workerColumns = [
      { title: '工种名称', dataIndex: 'name' },
      { title: '工日', dataIndex: 'count', scopedSlots: { customRender: 'count' } },
      { title: '操作', scopedSlots: { customRender: 'operations' } },
    ];
    let matColumns = [
      { title: '工程量清单', dataIndex: 'name' },
      { title: '计量单位', dataIndex: 'unitStr' },
      { title: '数量', dataIndex: 'count', scopedSlots: { customRender: 'count' } },
      { title: '操作', scopedSlots: { customRender: 'operations' } },
    ];
    let equipColumns = [
      { title: '设备类型', dataIndex: 'type' },
      { title: '名称', dataIndex: 'name' },
      { title: '规格', dataIndex: 'spec' },
      { title: '台班', dataIndex: 'count', scopedSlots: { customRender: 'count' } },
    ];
    let fileColumn = [{ title: '资料名称', dataIndex: 'name' }];
    return (
      <a-modal
        title={'工程量设置'}
        visible={this.visible}
        onCancel={this.close}
        destroyOnClose
        okText={'保存'}
        onOk={this.ok}
      >
        <a-tabs type='card' active-key={this.defaultKey} onChange={key => this.defaultKey = key}>
          <a-tab-pane key='worker' tab='工种信息'>
            <TabTable
              ref='workerTable'
              type={TabEnum.Worker}
              selectIds={this.selectWorkerIds}
              onSelectIds={(data) => this.tableSelectIds(data)}
              onOutList={(data) => this.tableOutList(data)}
              allList={this.workers}
              columns={workerColumns} />
          </a-tab-pane>
          <a-tab-pane key='material' tab='工程量信息'>
            <TabTable
              ref='matTable'
              type={TabEnum.Mat}
              selectIds={this.selectMaterialIds}
              onSelectIds={(data) => this.tableSelectIds(data)}
              onOutList={(data) => this.tableOutList(data)}
              allList={this.materials}
              columns={matColumns} />
          </a-tab-pane>
          <a-tab-pane key='equipmentTeam' tab='设备台班'>
            <TabTable
              type={TabEnum.EquipmentTeam}
              ref='equipTable'
              selectIds={this.selectEquipmentTeamIds}
              columns={equipColumns} onSelectIds={(data) => this.tableSelectIds(data)}
              onOutList={(data) => this.tableOutList(data)}
              allList={this.equipmentTeams}
            />
          </a-tab-pane>
          <a-tab-pane type={TabEnum.File} key='file' tab='相关资料'>
            <div style='position: absolute; right: 0px; '>
              <SmFileUpload
                showIcon
                icon='vertical-align-top'
                title='上传文件'
                onBeforeUpload={(e, v) => this.uploadHandler(Array.from(v))}
              />
            </div>
            <a-divider />
            <TabTable
              ref='fileTable'
              type={TabEnum.File}
              columns={fileColumn}
              onSelectIds={(data) => this.tableSelectIds(data)}
              allList={this.files}
              selectIds={this.selectFileIds}
            />
          </a-tab-pane>
        </a-tabs>
      </a-modal>
    );
  },
};
