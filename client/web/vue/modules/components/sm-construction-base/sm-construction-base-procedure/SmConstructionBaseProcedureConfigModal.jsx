import './style';
import * as utils from '../../_utils/utils';
import { ModalStatus } from '../../_utils/enum';
import { requestIsSuccess, vIf } from '../../_utils/utils';
import ApiProcedure from '../../sm-api/sm-construction-base/ApiProcedure';
import TabTable from './TabTable';
import { TabEnum } from './TabEnum';
import SmFileUpload from '../../sm-file/sm-file-manage/src/component/SmFileUpload';
import { CreateUUID, MesageQueue, OssType } from '../../sm-file/sm-file-manage/src/common';
import ApiFile from '../../sm-api/sm-file/file';
import OssRepository from '../../sm-file/sm-file-manage/src/ossRepository';
import ApiFileManage from '../../sm-api/sm-file/fileManage';

let apiProcedure = new ApiProcedure();
let apiFile = new ApiFile();
let apiFileManage = new ApiFileManage();
let messageQueue = new MesageQueue(); // 文件上传队列
let ossRepository = new OssRepository();
let saveQueue = new MesageQueue(); // 文件保存队列


// 工序配置 modal
export default {
  name: 'SmConstructionBaseProcedureConfigModal',
  props: {
    axios: { type: Function, default: null },
    // 工序类型列表
    procedureTypes: { type: Array, default: () => [] },
  },
  data() {
    return {
      status: ModalStatus.Hide, // 模态框状态
      form: {}, // 表单
      record: {
        procedureEquipmentTeams: [],
        procedureMaterials: [],
        procedureWorkers: [],
        procedureRtlFiles: [],
      }, // 表单绑定的对象
      defaultKey: 'worker',  // 默认tab 切换到 worker 栏
      workers: [],
      selectWorkerIds: [],
      files: [],
      selectFileIds: [],
      materials: [],
      selectMaterialIds: [],
      equipmentTeams: [],
      selectEquipmentTeamIds: [],
      uploadList: [], // 需要上传的文件列表
      completeList: [],
      fileState: {
        // 文件状态，记录传入后台的文件的实际存储状态
        isPublic: false,
        folderId: null,
        organizationId: null,
        folders: null, // 该文件隶属几层文件夹，当上传整个文件夹时需要
        isVersion: false,
        fileId: null,// 文件id，当上传文件版本时有效
      },
    };
  },
  watch: {
    uploadList: {
      handler(nVal, oVal) {
        this.uploadList = nVal;
        this.fileUpload();
      },
      immediate: true,
    },
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
  },
  async created() {
    this.initAxios();
    await this.getMyFiles();
    // 创建表单
    this.form = this.$form.createForm(this, {});
  },
  methods: {
    initAxios() {
      apiProcedure = new ApiProcedure(this.axios);
      apiFile = new ApiFile(this.axios);
      apiFileManage = new ApiFileManage(this.axios);
    },
    // 获取 我自己 的文件列表
    async getMyFiles() {
      let { data: { items: res } } = await apiFileManage.getResourceList({ type: 2, isMine: true, size: 9999 });
      this.files = res;
    },

    // 上传文件方法
    fileUpload() {
      let _this = this;
      while (messageQueue.size() > 0) {
        // 获取数据，进行文件上传
        let item = messageQueue.dequeue();
        if (item != null) {
          if (item.ossType == OssType.aliyun) {
            ossRepository
              .aliyunUpload(item, progress => (item.progress = progress))
              .then(async response => {
                _this.completeList.push(item);
                _this.uploadList = _this.uploadList.filter(a => a.id != item.id);
                // 保存文件
                // await _this.saveFile(item);
                saveQueue.enqueue(item);
              })
              .catch(err => console.log(err));
          } else if (item.ossType === OssType.minio || item.ossType == OssType.amazons3) {
            ossRepository
              .upload(item, progress => (item.progress = progress))
              .then(async response => {
                console.log(response);
                _this.completeList.push(item);
                _this.uploadList = _this.uploadList.filter(a => a.id != item.id);
                // 保存文件
                saveQueue.enqueue(item);
              })
              .catch(err => console.log(err));
          }
        }
      }
    },

    // 文件上传前处理，包括文件签名等获取
    uploadHandler(fileList) {
      let _this = this;
      if (fileList.length > 0) {
        // 获取上传签名
        fileList.forEach(a => {
          if (a.webkitRelativePath != null) {
            a.path = a.webkitRelativePath.substring(
              0,
              a.webkitRelativePath.lastIndexOf('/'),
            );
          }
          let sufixx = a.name.substring(a.name.lastIndexOf('.'));
          this.getPersiginUrl(sufixx).then(async res => {
            let data = _this.getFileData(res, a, sufixx);
            // 获取签名后 将 文件id 什么的 保存 到数据库
            let data1 = {
              fileId: data.id,
              name: data.name,
              type: data.type,
              size: data.size,
              isPublic: false, // 改成false ,这样根据 用户 来查询 自己的文件列表
              organizationId: data.state.organizationId,
              folderId: data.state.folderId,
              folderPath: data.path,
              ossFileName: data.ossFileName,
              url: data.relativeUrl,
            };
            const response = await apiFile.create(data1);
            if (!requestIsSuccess(response)) {
              _this.$message.error('保存文件失败!');
            }
            await this.getMyFiles();
            this.uploadList.push(data);
            messageQueue.enqueue(data);
          });
        });
      }
    },

    // 根据条件获取文件数据实体
    getFileData(res, file, sufixx) {
      let newFile = new File([file], `${res.fileId}${sufixx}`);
      return {
        file: newFile,
        size: file.size,
        name: file.name.substring(0, file.name.lastIndexOf('.')),
        type: sufixx,
        editTime: file.lastModifiedDate,
        id: CreateUUID(),
        progress: 0,
        error: false,
        upload: true,
        path: file.path,
        state: this.fileState,
        presignUrl: res.presignUrl,
        relativeUrl: res.relativePath,
        ossType: res.ossType,
      };
    },

    // 获取文件上传的签名地址
    getPersiginUrl(sufixx) {
      let promise = new Promise(async (res, err) => {
        let response = await apiFile.getPresignUrl({ sufixx });
        if (requestIsSuccess(response)) {
          res(response.data);
        }
      });
      return promise;
    },

    // 获取 关联 的 表 的 数据 (list) 除了 fileList fileList单独获取
    async getRltList(id) {
      let { data: res } = await apiProcedure.getRltList(id);
      this.workers = res.workers;
      this.equipmentTeams = res.equipmentTeams;
      this.materials = res.materials.map(x => ({ ...x, editable: false }));
    },

    // tabTable 的回调
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
      this.materials = list;
    },
    // 关闭模态框
    close() {
      this.status = ModalStatus.Hide;
    },
    // 资源配置
    config(record) {
      this.status = ModalStatus.Edit;
      this.record = record;
      this.selectWorkerIds = record.procedureWorkers.map(x => x.id);
      this.selectMaterialIds = record.procedureMaterials.map(x => x.id);
      this.selectEquipmentTeamIds = record.procedureEquipmentTeams.map(x => x.id);
      this.selectFileIds = record.procedureRtlFiles.map(x => x.id);
      this.getRltList(record.id);
      this.$nextTick(() => {
        // this.form.setFieldsValue({ ...utils.objFilterProps(record, formFields) });
      });
    },
    // 数据提交
    async ok() {
      let data1 = {
        workerIds: this.selectWorkerIds,
        materialIds: this.selectMaterialIds.map(x => {
          let index = this.materials.findIndex(m => m.id === x);
          if (index > -1) {
            return { materialId: x, count: this.materials[index].count };
          }
        }),
        equipmentTeamIds: this.selectEquipmentTeamIds,
        fileIds: this.selectFileIds,
      };
      let response = await apiProcedure.configProcedure(this.record.id, data1);
      if (utils.requestIsSuccess(response)) {
        this.$message.success('配置成功');
        this.close();
        this.$emit('success');
      }
    },
  },
  render() {
    let workerColumns = [{ title: '工种名称', dataIndex: 'name' }];
    let matColumns = [{ title: '工程量清单', dataIndex: 'name' }, { title: '计量单位', dataIndex: 'unitStr' }, { title: '数量', dataIndex: 'count', scopedSlots: { customRender: 'count' } }, { title: '操作', scopedSlots: { customRender: 'operations' } }];
    let equipColumns = [{ title: '设备类型', dataIndex: 'name' }];
    let fileColumn = [{ title: '资料名称', dataIndex: 'name' }];
    return (
      <a-modal
        title={'资源配置'}
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
