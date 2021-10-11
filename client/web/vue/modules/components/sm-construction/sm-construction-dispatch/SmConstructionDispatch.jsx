
import './style';
import { requestIsSuccess, getSafetyMeasure, getControlType, objFilterProps, vIf, vP, getFileUrl } from '../../_utils/utils';
import SmConstructionBaseSectionSelect from '../../sm-construction-base/sm-construction-base-section-select';
import SmConstructionBaseStandardSelect from '../../sm-construction-base/sm-construction-base-standard-select';
import SmConstructionDispatchTemplateSelect from '../sm-construction-dispatch-template-select';
import SmSystemDataDictionaryTreeSelect from '../../sm-system/sm-system-data-dictionary-tree-select';
import SmConstructionPlanContentSelectModal from '../sm-construction-plan-content-select-modal';
import SmMaterialMaterialSelectModal from '../../sm-material/sm-material-material-select-modal';
import SmSystemMemberSelect from '../../sm-system/sm-system-member-select';
import SmVideo from '../../sm-common/sm-video/SmVideo';
import SmFileImageView from '../../sm-file/sm-file-manage/src/component/SmFileImageView';
import SmFileDocumentView from '../../sm-file/sm-file-manage/src/component/SmFileDocumentView';
import { PageState, SafetyMeasure, ControlType, DispatchState, WorkflowState } from '../../_utils/enum';
import ApiDispatch from '../../sm-api/sm-construction/Dispatch';
import OssRepository from '../../sm-file/sm-file-manage/src/ossRepository';
import SmFileUpload from '../../sm-file/sm-file-upload';
import moment from 'moment';
import FileSaver from 'file-saver';
import * as permissionsSmConstruction from '../../_permissions/sm-construction';
import { SaveSingleFile } from '../../sm-file/sm-file-manage/src/common';

let apiDispatch = new ApiDispatch();
let ossRepository = new OssRepository();

// 表单字段 
const formFields = [
  'name',
  'dispatchTemplateId',
  'code',
  'contractorId',
  'controlType',
  'dispatchRltSections',
  'dispatchRltStandards',
  'dispatchRltWorkers',
  'extraDescription',
  'isDismantle',
  'isHighWork',
  'isNeedLargeEquipment',
  'largeEquipment',
  'number',
  'process',
  'profession',
  'recoveryTime',
  'remark',
  'riskSources',
  'safetyMeasure',
  'team',
  'time',
];

export default {
  name: 'SmConstructionDispatch',
  props: {
    axios: { type: Function, default: null },
    pageState: { type: String, default: PageState.Add }, // 页面状态
    id: { type: String, default: null },
    isApprove: { type: Boolean, default: false },
    permissions: { type: Array, default: () => [] },
  },
  data() {
    return {
      iId: null,
      form: this.$form.createForm(this, {}),
      iPageState: PageState.Add,
      record: null,
      fileList: [],
      loading: false,
      planContentModal: false,//任务模态框是否弹出
      materialModal: false,//材料模态框是否弹出
      planContents: [],//施工任务
      equipments: [],//待安装设备
      materials: [],//待安装材料
      imgtypes: ['.jpg', '.png', '.tif', 'gif', '.JPG', '.PNG', '.GIF', '.jpeg', '.JPEG'],
    };
  },
  computed: {

    plantContentColumn() {
      let array = [
        {
          title: '序号',
          ellipsis: true,
          dataIndex: 'index',
          width: 170,
          align: 'center',
          customRender: (text, record, index) => {
            return index + 1;
          },
        },
        {
          title: '任务名称',
          ellipsis: true,
          dataIndex: 'name',
          align: 'center',
        },
        {
          title: '工作内容',
          ellipsis: true,
          dataIndex: 'content',
          align: 'center',
        },
      ];
      return this.iPageState === PageState.View ?
        array :
        [
          ...array,
          {
            title: '操作',
            dataIndex: 'operations',
            align: 'center',
            width: 150,
            customRender: (text, record, index) => {
              return <a onClick={() => this.removePlanContent(record)}>删除</a>;
            },
          },
        ];
    },
    equipmentColumn() {
      return [
        {
          title: '序号',
          ellipsis: true,
          dataIndex: 'index',
          width: 170,
          align: 'center',
          customRender: (text, record, index) => {
            return index + 1;
          },
        },
        {
          title: '设备名称',
          ellipsis: true,
          dataIndex: 'name',
          align: 'center',
        },
        {
          title: '规格型号',
          ellipsis: true,
          dataIndex: 'spec',
          align: 'center',
        },
        {
          title: '计量单位',
          ellipsis: true,
          dataIndex: 'unit',
          align: 'center',
          width: 250,
        },
        {
          title: '工程数量',
          ellipsis: true,
          dataIndex: 'quantity',
          align: 'center',
          width: 250,
        },
      ];
    },
    materialColumn() {
      let array = [
        {
          title: '序号',
          ellipsis: true,
          dataIndex: 'index',
          width: 170,
          align: 'center',
          customRender: (text, record, index) => {
            return index + 1;
          },
        },
        {
          title: '材料名称',
          ellipsis: true,
          dataIndex: 'name',
          align: 'center',
        },
        {
          title: '规格型号',
          ellipsis: true,
          dataIndex: 'spec',
          align: 'center',
        },
        {
          title: '计量单位',
          ellipsis: true,
          dataIndex: 'unit',
          align: 'center',
          width: 250,
        },
      ];
      return this.iPageState === PageState.View ?
        [
          ...array,
          {
            title: '工程数量',
            ellipsis: true,
            dataIndex: 'count',
            align: 'center',
            width: 250,
          },
        ] :
        [
          ...array,
          {
            title: '工程数量',
            ellipsis: true,
            dataIndex: 'count',
            align: 'center',
            width: 250,
            customRender: (text, record, index) => {
              return <a-input-number
                placeholder="请输入工程数量"
                value={record.count}
                min={0}
                precision={3}
                onChange={value => record.count = value}
              />;
            },
          },
          this.iPageState === PageState.View ? { width: 0 } :
            {
              title: '操作',
              dataIndex: 'operations',
              align: 'center',
              width: 150,
              customRender: (text, record, index) => {
                return <a onClick={() => this.removeMaterial(record)}>删除</a>;
              },
            },
        ];
    },
    workFlowNodeColumn() {
      return [
        {
          title: '序号',
          ellipsis: true,
          dataIndex: 'index',
          width: 170,
          align: 'center',
          customRender: (text, record, index) => {
            return index + 1;
          },
        },
        {
          title: '审批意见',
          ellipsis: true,
          dataIndex: 'content',
          align: 'center',
        },
        {
          title: '审批状态',
          ellipsis: true,
          dataIndex: 'state',
          align: 'center',
          customRender: (text, record, index) => {
            let result = WorkflowState.Finished ? "审核通过" : record.State == WorkflowState.Stopped ? "审核未通过" : "";
            return result;
          },
        },
        {
          title: '审批人',
          ellipsis: true,
          dataIndex: 'user.name',
          align: 'center',
        },
        {
          title: '审批时间',
          ellipsis: true,
          dataIndex: 'approveTime',
          align: 'center',
          customRender: (text, record, index) => {
            return moment(record.approveTime).format("YYYY-MM-DD HH:mm:ss");
          },
        },
      ];
    },
  },
  watch: {
    id: {
      handler: function (value, oldValue) {
        this.iId = this.id;
        this.form.resetFields();
        if (value) {
          this.initAxios();
          this.refresh();
        }
      },
      immediate: true,
    },

    pageState: {
      handler: function (value, oldValue) {
        this.iId = this.id;
        this.iPageState = value;
        if (value != PageState.Add) {
          this.initAxios();
          this.refresh();
        } else {
          this.form.resetFields();
        }
      },
      immediate: true,
    },
  },
  async created() {
    this.form = this.$form.createForm(this, {});
    this.form.resetFields();
    this.initAxios();
    this.refresh();
  },
  methods: {
    async initAxios() {
      apiDispatch = new ApiDispatch(this.axios);
    },

    //初始化表单
    async refresh(id) {
      if (id) {
        this.iId = id;
      }
      if (!this.pageState || this.pageState === PageState.Add || !this.iId) {
        return;
      }
      let response = await apiDispatch.get(this.iId);
      if (requestIsSuccess(response)) {
        let _record = response.data;
        let controlTypeArray = _record ? _record.controlType.split(',') : [];
        let controlTypeString = '';
        if (controlTypeArray.length > 0) {
          controlTypeArray.map(item => {
            if (!controlTypeString) {
              controlTypeString = getControlType(parseInt(item));
            } else {
              controlTypeString = controlTypeString + '、' + getControlType(parseInt(item));
            }
          });
        }

        let safetyMeasureArray = _record ? _record.safetyMeasure.split(',') : [];
        let safetyMeasureString = '';
        if (safetyMeasureArray.length > 0) {
          safetyMeasureArray.map(item => {
            if (!safetyMeasureString) {
              safetyMeasureString = getSafetyMeasure(parseInt(item));
            } else {
              safetyMeasureString = safetyMeasureString + '、' + getSafetyMeasure(parseInt(item));
            }
          });
        }

        let standardString = '';
        if (_record && _record.dispatchRltStandards && _record.dispatchRltStandards.length > 0) {
          _record.dispatchRltStandards.map(item => {
            if (item.standard) {
              if (!standardString) {
                standardString = item.standard.name;
              } else {
                standardString = standardString + '、' + item.standard.name;
              }
            }
          });
        }

        let workerString = '';
        if (_record && _record.dispatchRltWorkers && _record.dispatchRltWorkers.length > 0) {
          _record.dispatchRltWorkers.map(item => {
            if (item.worker) {
              if (!workerString) {
                workerString = item.worker.name;
              } else {
                workerString = workerString + '、' + item.worker.name;
              }
            }
          });
        }

        let sectionString = '';
        if (_record && _record.dispatchRltSections && _record.dispatchRltSections.length > 0) {
          _record.dispatchRltSections.map(item => {
            if (item.section) {
              if (!sectionString) {
                sectionString = item.section.name;
              } else {
                sectionString = sectionString + '、' + item.section.name;
              }
            }
          });
        }

        this.record = {
          ..._record,
          workFlowNodes: _record && _record.workFlowNodes && _record.workFlowNodes.length > 0 ?
            _record.workFlowNodes.map((item, index) => {
              return {
                ...item,
                index,
              };
            })
            : [],
          dispatchRltSections: this.iPageState === PageState.Edit ? _record && _record.dispatchRltSections && _record.dispatchRltSections.length > 0 ?
            _record.dispatchRltSections.map(item => item.sectionId) : []
            : sectionString,
          dispatchRltStandards: this.iPageState === PageState.Edit ? _record && _record.dispatchRltStandards && _record.dispatchRltStandards.length > 0 ?
            _record.dispatchRltStandards.map(item => item.standardId) : []
            : standardString,
          dispatchRltWorkers: _record && _record.dispatchRltWorkers && _record.dispatchRltWorkers.length > 0 && this.iPageState === PageState.Edit ?
            _record.dispatchRltWorkers.map(item => {
              return {
                type: 3,
                id: item.workerId,
              };
            })
            : workerString,
          time: _record.time ? this.iPageState === PageState.Edit ? moment(_record.time) : moment(_record.time).format('YYYY-MM-DD HH:mm:ss') : null,
          recoveryTime: _record.recoveryTime ? this.iPageState === PageState.Edit ? moment(_record.recoveryTime) : moment(_record.recoveryTime).format('YYYY-MM-DD HH:mm:ss') : null,
          controlType: this.iPageState === PageState.Edit ? _record && _record.controlType && _record.controlType.split(',').length > 0 ?
            _record.controlType.split(',').map(Number) : []
            : controlTypeString,
          safetyMeasure: this.iPageState === PageState.Edit ? _record && _record.safetyMeasure && _record.safetyMeasure.split(',').length > 0 ?
            _record.safetyMeasure.split(',').map(Number) : []
            : safetyMeasureString,
        };
        this.planContents = _record && _record.dispatchRltPlanContents && _record.dispatchRltPlanContents.length > 0 ?
          _record.dispatchRltPlanContents.map(item => {
            if (item.planContent && item.planContent.planMaterials && item.planContent.planMaterials.length > 0) {
              item.planContent.planMaterials.map(x => {
                let data = {
                  ...x,
                  name: x.planMaterialRltEquipments &&
                    x.planMaterialRltEquipments.length > 0 &&
                    x.planMaterialRltEquipments[0].equipment ?
                    x.planMaterialRltEquipments[0].equipment.name : '',
                };
                if (!this.equipments.find(y => y.id === data.id)) {
                  this.equipments.push(data);
                }
              });
            }
            return { ...item.planContent, children: null };
          })
          : [];
        this.materials = _record && _record.dispatchRltMaterials && _record.dispatchRltMaterials.length > 0 ?
          _record.dispatchRltMaterials.map(item => {
            return {
              ...item.material,
              count: item.count,
            };
          })
          : [];
        this.fileList = _record && _record.dispatchRltFiles && _record.dispatchRltFiles.length > 0 ? _record.dispatchRltFiles.map(item => item.file) : [];
        if (this.iPageState === PageState.Edit) {
          this.$nextTick(() => {
            this.form.setFieldsValue(objFilterProps(this.record, formFields));
          });
        }
      }
    },

    //添加施工任务
    addPlanContents(values) {
      if (values && values.length > 0) {
        values.map(item => {
          if (!this.planContents.find(_item => _item.id === item.id)) {
            this.planContents.push({ ...item, children: null });
          }
          if (item.planMaterials && item.planMaterials.length > 0) {
            item.planMaterials.map(x => {
              let data = {
                ...x,
                name: x.planMaterialRltEquipments &&
                  x.planMaterialRltEquipments.length > 0 &&
                  x.planMaterialRltEquipments[0].equipment ?
                  x.planMaterialRltEquipments[0].equipment.name : '',
              };
              if (!this.equipments.find(y => y.id === data.id)) {
                this.equipments.push(data);
              }
            });
          }
        });
      }
    },

    //添加待安装材料
    addMaterials(values) {
      if (values && values.length > 0) {
        values.map(item => {
          if (!this.materials.find(_item => _item.id === item.id)) {
            this.materials.push({ ...item, count: 1 });
          }
        });
      }
    },

    //删除施工任务
    removePlanContent(record) {
      this.planContents = this.planContents.filter(item => item.id !== record.id);
      if (record.planMaterials && record.planMaterials.length > 0) {
        record.planMaterials.map(item => {
          this.equipments = this.equipments.filter(_item => _item.id !== item.id);
        });
      }
    },

    //删除待安装材料
    removeMaterial(record) {
      this.materials = this.materials.filter(item => item.id !== record.id);
    },

    save() {
      this.form.validateFields(async (err, values) => {
        if (!err) {
          if (this.fileList.length > 0) {
            _ = await this.$refs.fileUpload.commit(); // 提交文件
          }
          if (this.planContents.length < 0) {
            this.$message.error('请添加施工任务');
            return;
          }
          if (this.materials.find(item => item.count <= 0)) {
            this.$message.error('施工材料数量不能小于等于0');
            return;
          }
          let data = {
            ...values,
            time: values.time ? moment(values.time).format() : '',
            recoveryTime: values.recoveryTime ? moment(values.recoveryTime).format() : '',
            controlType: values.controlType ? values.controlType.toString() : '',
            safetyMeasure: values.safetyMeasure ? values.safetyMeasure.toString() : '',
            dispatchRltSections: values.dispatchRltSections && values.dispatchRltSections.length > 0 ?
              values.dispatchRltSections.map(item => { return { sectionId: item }; })
              : [],
            dispatchRltStandards: values.dispatchRltStandards && values.dispatchRltStandards.length > 0 ?
              values.dispatchRltStandards.map(item => { return { standardId: item }; })
              : [],
            dispatchRltWorkers: values.dispatchRltWorkers && values.dispatchRltWorkers.length > 0 ?
              values.dispatchRltWorkers.map(item => { return { workerId: item.id }; })
              : [],
            dispatchRltPlanContents: this.planContents.length > 0 ?
              this.planContents.map(item => { return { planContentId: item.id }; })
              : [],
            dispatchRltMaterials: this.materials.length > 0 ?
              this.materials.map(item => {
                return {
                  materialId: item.id,
                  count: item.count ? item.count : 0,
                };
              })
              : [],
            dispatchRltFiles: this.fileList.length > 0 ?
              this.fileList.map(item => {
                return {
                  fileId: item.id,
                };
              })
              : [],
          };
          this.loading = true;
          let response = null;
          if (this.iPageState === PageState.Add) {
            response = await apiDispatch.create(data);
          } else if (this.iPageState === PageState.Edit) {
            response = await apiDispatch.update({ id: this.record.id, ...data });
          }
          if (response && requestIsSuccess(response)) {
            this.$message.success('操作成功');
            this.$emit('ok', this.iId);
            this.reset();
          }
        }
      });
      this.loading = false;

    },

    //数据重置
    reset() {
      this.materials = [];
      this.equipments = [];
      this.planContents = [];
      this.form.resetFields();
      this.record = null;
      this.loading = false;
      this.fileList = [];
      this.iId = null;
      this.iPageState = PageState.Add;
    },


    //取消
    cancel() {
      this.$emit('cancel');
      this.reset();
    },

    //派工审批
    process(isPassed) {
      let _this = this;
      this.form.validateFields(async (err, values) => {
        if (!err) {
          let data = {
            content: values.content,
            dispatchId: this.record.id,
            state: isPassed ? DispatchState.Pass : DispatchState.UnPass,
          };
          this.loading = true;
          if (!isPassed) {
            this.$confirm({
              title: "温馨提示",
              content: h => <div style="color:red;">确定要驳回此审批流程吗？</div>,
              okType: 'danger',
              onOk() {
                return new Promise(async (resolve, reject) => {
                  let response = await apiDispatch.process(data);
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
          } else {
            let response = await apiDispatch.process(data);
            if (response && requestIsSuccess(response)) {
              this.$message.success('操作成功');
              this.$emit('ok', this.iId);
              this.reset();
            }
          }
        }
      });
      this.loading = false;
    },

    //导出
    async export() {
      if (this.record && this.record.id) {
        this.loading = true;
        let response = await apiDispatch.export(this.record.id);
        if (requestIsSuccess(response)) {
          if (response.data.byteLength != 0) {
            this.$message.info('导出成功');
            FileSaver.saveAs(
              new Blob([response.data], { type: 'application/vnd.ms-excel' }),
              `派工单明细.docx`,
            );
          }
        }
      }
      this.loading = false;
    },

    //打印
    onPrint() {
      printJS({
        printable: 'sm-construction-dispatch-view',
        type: 'html',
        maxWidth: '100%',
        targetStyles: ['*'],
      });
    },

    //附件展示
    fileShow(files) {
      let _file = [];
      files && files.map(item => {
        _file.push(
          <span class="dispatch-file-item" style="height:50px" onClick={() => {
            this.play(item);
          }}>
            {this.imgtypes.includes(item.type) ?
              <img src={getFileUrl(item.url)} alt={`${item.name}`} class="dispatch-image" />
              :
              <a-tag>{item.name + item.type}</a-tag>
            }
          </span>,
        );
      });
      return _file;
    },

    downloadClick(file) {
      let _this = this;
      if (file) {
        ossRepository
          .download(getFileUrl(file.url), () => { })
          .then(blob => {
            SaveSingleFile(`${file.name}${file.type}`, file.size, blob).then(a => {
              _this.$notification['success']({
                message: '温馨提示',
                description: `${file.name}下载成功`,
                duration: 2,
              });
            });
          });
      }
    },

    play(file) {
      let imgtypes = ['.jpg', '.png', '.tif', 'gif', '.JPG', '.PNG', '.GIF', '.jpeg', '.JPEG'];
      // let videoTypes = ['.avi', '.mov', '.rmvb', '.rm', '.flv', '.mp4', '.3gp', '.mpeg', '.mpg'];
      let videoTypes = [];
      if (file.type === '.pdf') {
        this.$refs.SmFileDocumentView.view(file);
      } else if (imgtypes.includes(file.type)) {
        this.$refs.SmFileImageView.view(file);
      } else if (videoTypes.includes(file.type)) {
        this.$refs.SmVideo.preview(true, getFileUrl(file.url), file.name);
      } else {
        this.$message.warning('当前文件不支持预览，开始下载中...');
        this.downloadClick(file);
      }
    },
  },
  render() {
    //安全防护措施
    let safetyMeasureOption = [];
    for (let item in SafetyMeasure) {
      safetyMeasureOption.push(
        <a-checkbox key={SafetyMeasure[item]} value={SafetyMeasure[item]}>
          {getSafetyMeasure(SafetyMeasure[item])}
        </a-checkbox>,
      );
    }
    //工序控制类型
    let controlTypeOption = [];
    for (let item in ControlType) {
      controlTypeOption.push(
        <a-checkbox key={ControlType[item]} value={ControlType[item]}>
          {getControlType(ControlType[item])}
        </a-checkbox>,
      );
    }
    return <div class="sm-construction-dispatch">
      <a-form form={this.form} id={this.iPageState == PageState.View ? "sm-construction-dispatch-view" : ''}>
        {this.iPageState === PageState.View ? <div style="text-align:right; margin-bottom:5px">派单日期：{this.record ? this.record.time : ''}</div> : undefined}
        <a-row style="margin:0;" gutter={24} class={{ 'sm-construction-dispatch-row': this.iPageState === PageState.View }}>

          <a-col sm={24} md={12} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <div class='sm-construction-dispatch-label'>派工单名称</div>,
              <div class='sm-construction-dispatch-text' style="height:30px">{this.record && this.record.name ? this.record.name : '无'}</div>] :
              <a-form-item label="派工单名称" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <a-input
                  placeholder='请输入派工单名称'
                  v-decorator={[
                    'name',
                    {
                      initialValue: moment().format('YYYY年MM月DD日') + '派工单',
                      rules: [{ required: true, message: '请输入派工单名称' }],
                    },
                  ]}
                />
              </a-form-item>
            }
          </a-col>
          {this.iPageState === PageState.View ? undefined :
            <a-col sm={24} md={12} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
              <a-form-item label="派工单模板" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <SmConstructionDispatchTemplateSelect
                  placeholder='请选择派工单模板'
                  axios={this.axios}
                  v-decorator={[
                    'dispatchTemplateId',
                    {
                      initialValue: undefined,
                      rules: [{ required: true, message: '请选择派工单模板' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
          }

          <a-col sm={24} md={12} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <div class='sm-construction-dispatch-label'>派工单编号</div>,
              <div class='sm-construction-dispatch-text' style="height:30px">{this.record && this.record.code ? this.record.code : '无'}</div>] :
              <a-form-item label="派工单编号" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <a-input
                  disabled
                  placeholder='自动生成'
                  v-decorator={[
                    'code',
                    {
                      initialValue: '',
                    },
                  ]}
                />
              </a-form-item>
            }
          </a-col>

          <a-col sm={24} md={12} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <span class='sm-construction-dispatch-label'>施工专业</span>,
              <span class='sm-construction-dispatch-text' style="height:30px">{this.record && this.record.profession ? this.record.profession : '无'}</span>] :
              <a-form-item label="施工专业" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <a-input
                  placeholder='请输入'
                  v-decorator={[
                    'profession',
                    {
                      initialValue: '',
                      rules: [{ required: true, message: '请输入施工专业', whitespace: true }],
                    },
                  ]}
                />
              </a-form-item>
            }
          </a-col>
          <a-col sm={24} md={12} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <span class='sm-construction-dispatch-label'>承包商</span>,
              <a-tooltip placement="topLeft" title={this.record && this.record.contractor && this.record.contractor.name ? this.record.contractor.name : '无'}>
                <span class='sm-construction-dispatch-text' style="height:30px">
                  {this.record && this.record.contractor && this.record.contractor.name ? this.record.contractor.name : '无'}
                </span>
              </a-tooltip>
              ,
            ] :
              <a-form-item label="承包商" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <SmSystemDataDictionaryTreeSelect
                  axios={this.axios}
                  disabled={this.iPageState == PageState.View}
                  groupCode="ConstructionDispatch.Contractor"
                  placeholder={this.iPageState == PageState.View ? '' : '请选择'}
                  v-decorator={[
                    'contractorId',
                    {
                      initialValue: undefined,
                      rules: [{ required: true, message: '请选择承包商' }],
                    },
                  ]}
                />
              </a-form-item>
            }
          </a-col>

          <a-col sm={24} md={12} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <span class='sm-construction-dispatch-label'>施工班组</span>,
              <span class='sm-construction-dispatch-text' style="height:30px">{this.record && this.record.team ? this.record.team : '无'}</span>] :
              <a-form-item label="施工班组" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <a-input
                  disabled={this.iPageState == PageState.View}
                  placeholder={this.iPageState == PageState.View ? '' : '请输入'}
                  v-decorator={[
                    'team',
                    {
                      initialValue: '',
                      rules: [{ required: true, message: '请输入施工班组', whitespace: true }],
                    },
                  ]}
                />
              </a-form-item>
            }
          </a-col>
          <a-col sm={24} md={12} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <span class='sm-construction-dispatch-label'>施工人员数量</span>,
              <span class='sm-construction-dispatch-text' style="height:30px">{this.record && this.record.number ? this.record.number : '无'}</span>] :
              <a-form-item label="施工人员数量" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <a-input-number
                  disabled={this.iPageState == PageState.View}
                  placeholder={this.iPageState == PageState.View ? '' : '请输入'}
                  style="width:100%"
                  min={0}
                  precision={0}
                  v-decorator={[
                    'number',
                    {
                      initialValue: 0,
                      rules: [{ required: true, message: '请输入施工人员数量' }],
                    },
                  ]}
                />
              </a-form-item>
            }
          </a-col>
          {this.iPageState === PageState.View ? undefined :
            <a-col sm={24} md={12} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
              <a-form-item label="派单日期" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <a-date-picker
                  style="width: 100%"
                  showTime={true}
                  disabled={this.iPageState == PageState.View}
                  placeholder={this.iPageState == PageState.View ? '' : '请选择'}
                  v-decorator={[
                    'time',
                    {
                      initialValue: moment(),
                      rules: [{ required: true, message: '请选择派单日期' }],
                    },
                  ]}
                />
              </a-form-item>
            </a-col>
          }

          <a-col sm={24} md={this.iPageState === PageState.View ? 24 : 12} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <span class='sm-construction-dispatch-label'>施工区段</span>,
              <span class='sm-construction-dispatch-text'>{this.record && this.record.dispatchRltSections ? this.record.dispatchRltSections : '无'}</span>] :
              <a-form-item class="sm-construction-dispatch-item" label="施工区段" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <SmConstructionBaseSectionSelect
                  axios={this.axios}
                  disabled={this.iPageState == PageState.View}
                  placeholder={this.iPageState == PageState.View ? '' : '请选择'}
                  modalTitle="施工区段选择框"
                  // height={33}
                  multiple
                  v-decorator={[
                    'dispatchRltSections',
                    {
                      initialValue: [],
                      rules: [{ required: true, message: '请选择施工区段' }],
                    },
                  ]}
                />

              </a-form-item>
            }
          </a-col>




          <a-col sm={24} md={this.iPageState === PageState.View ? 24 : 12} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <span class='sm-construction-dispatch-label'>现场施工员</span>,
              <span class='sm-construction-dispatch-text'>{this.record && this.record.dispatchRltWorkers ? this.record.dispatchRltWorkers : '无'}</span>] :
              <a-form-item class="sm-construction-dispatch-item" label="现场施工员" label-col={{ span: 4 }} wrapper-col={{ span: 20 }}>
                <SmSystemMemberSelect
                  axios={this.axios}
                  disabled={this.iPageState == PageState.View}
                  placeholder={this.iPageState == PageState.View ? '' : '请选择'}
                  height={80}
                  showUserTab={true}
                  v-decorator={[
                    'dispatchRltWorkers',
                    {
                      initialValue: [],
                      rules: [{ required: true, message: '请选择现场施工员' }],
                    },
                  ]}
                />
              </a-form-item>
            }
          </a-col>
          <a-col sm={24} md={24} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <span class='sm-construction-dispatch-label'>工序指引</span>,
              <span class='sm-construction-dispatch-text'>{this.record && this.record.dispatchRltStandards ? this.record.dispatchRltStandards : '无'}</span>] :
              <a-form-item class="sm-construction-dispatch-item" label="工序指引" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                <SmConstructionBaseStandardSelect
                  axios={this.axios}
                  disabled={this.iPageState == PageState.View}
                  placeholder={this.iPageState == PageState.View ? '' : '请选择'}
                  modalTitle="工序指引选择框"
                  // height={33}
                  multiple={true}
                  v-decorator={[
                    'dispatchRltStandards',
                    {
                      initialValue: [],
                      rules: [{ required: true, message: '请选择工序指引' }],
                    },
                  ]}
                />
              </a-form-item>
            }
          </a-col>


          {/* 施工任务 */}
          <a-col sm={24} md={24} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ?
              <span class='sm-construction-dispatch-text content-style'>施工任务 </span>
              :
              <a-tabs>
                <a-tab-pane key="1" ><span slot="tab"><span style="color:#f5222d;margin-right: 2px;">*</span> 施工任务</span></a-tab-pane>
                <a-button
                  slot="tabBarExtraContent"
                  type="primary"
                  size="small"
                  onClick={() => this.planContentModal = true}
                >
                  选择任务
                </a-button>
              </a-tabs>

            }
          </a-col>
          {this.iPageState === PageState.View ?
            this.planContents.length > 0 ?
              <a-table
                class="sm-construction-dispatch-table"
                dataSource={this.planContents}
                columns={this.plantContentColumn}
                rowKey={record => record.id}
                bordered={true}
                size="small"
                scroll={{ y: 300 }}
                pagination={false}
              /> :
              <a-col sm={24} md={24} class='sm-construction-dispatch-col'>
                <span class='sm-construction-dispatch-text content-style'>暂无施工任务</span>
              </a-col>
            :
            <a-col sm={24} md={24} style="margin-bottom: 24px;">
              <a-table
                dataSource={this.planContents}
                columns={this.plantContentColumn}
                rowKey={record => record.id}
                bordered={true}
                scroll={{ y: 300 }}
                size="small"
                pagination={false}
              />
            </a-col>}

          {/* 待安装设备 */}
          <a-col sm={24} md={24} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ?
              <span class='sm-construction-dispatch-text content-style'>待安装设备</span>
              :
              <a-tabs>
                <a-tab-pane key="1" tab="待安装设备" />
              </a-tabs>
            }
          </a-col>
          {this.iPageState === PageState.View ?
            this.equipments.length > 0 ?
              <a-table
                class="sm-construction-dispatch-table"
                dataSource={this.equipments}
                columns={this.equipmentColumn}
                rowKey={record => record.id}
                bordered={true}
                size="small"
                scroll={{ y: 300 }}
                pagination={false}
              /> :
              <a-col sm={24} md={24} class='sm-construction-dispatch-col'>
                <span class='sm-construction-dispatch-text content-style'>暂无安装设备</span>
              </a-col>
            :
            <a-col sm={24} md={24} style="margin-bottom: 24px;">
              <a-table
                dataSource={this.equipments}
                columns={this.equipmentColumn}
                rowKey={record => record.id}
                scroll={{ y: 300 }}
                bordered={true}
                size="small"
                pagination={false}
              />
            </a-col>}


          {/* 待施工材料 */}
          <a-col sm={24} md={24} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ?
              <span class='sm-construction-dispatch-text content-style'>待施工材料</span>
              :
              <a-tabs>
                <a-tab-pane key="1" tab="待施工材料" />
                <a-button
                  slot="tabBarExtraContent"
                  type="primary"
                  size="small"
                  onClick={() => this.materialModal = true}
                >
                  选择材料
                </a-button>
              </a-tabs>
            }
          </a-col>
          {this.iPageState === PageState.View ?
            this.materials.length > 0 ? <a-table
              class="sm-construction-dispatch-table"
              dataSource={this.materials}
              columns={this.materialColumn}
              rowKey={record => record.id}
              bordered={true}
              scroll={{ y: 300 }}
              size="small"
              pagination={false}
            /> :
              <a-col sm={24} md={24} class='sm-construction-dispatch-col'>
                <span class='sm-construction-dispatch-text content-style'>暂无施工材料</span>
              </a-col>
            :
            <a-col sm={24} md={24} style="margin-bottom: 24px;">
              <a-table
                dataSource={this.materials}
                columns={this.materialColumn}
                rowKey={record => record.id}
                scroll={{ y: 300 }}
                bordered={true}
                size="small"
                pagination={false}
              />
            </a-col>}

          <a-col sm={24} md={24} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <span class='sm-construction-dispatch-label' >补充说明</span>,
              <span class='sm-construction-dispatch-text' >{this.record && this.record.extraDescription ? this.record.extraDescription : '无'}</span>] :
              <a-form-item label="补充说明" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                <a-textarea
                  row={3}
                  disabled={this.iPageState == PageState.View}
                  placeholder={this.iPageState == PageState.View ? '' : '请输入'}
                  v-decorator={[
                    'extraDescription',
                    {
                      initialValue: '',
                      rules: [{ whitespace: true }],
                    },
                  ]}
                />
              </a-form-item>
            }
          </a-col>

          <a-col sm={24} md={24} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ?
              <span class='sm-construction-dispatch-text content-style'>其他内容</span>
              :
              <a-tabs>
                <a-tab-pane key="1" tab="其他内容" />
              </a-tabs>
              // <a-form-item colon={false} style="border-bottom: 1px solid #dddddd;" label="其他内容" label-col={{ span: 2 }} wrapper-col={{ span: 22 }} />
            }
          </a-col>

          <a-col sm={24} md={this.iPageState === PageState.View ? 24 : 12} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <span class='sm-construction-dispatch-label'>安全风险源</span>,
              <span class='sm-construction-dispatch-text' >{this.record && this.record.riskSources ? this.record.riskSources : '无'}</span>] :
              <a-form-item label="安全风险源" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                <a-input
                  disabled={this.iPageState == PageState.View}
                  placeholder={this.iPageState == PageState.View ? '' : '请输入'}
                  v-decorator={[
                    'riskSources',
                    {
                      initialValue: '',
                      rules: [{ whitespace: true }],
                    },
                  ]}
                />
              </a-form-item>
            }
          </a-col>
          <a-col sm={24} md={this.iPageState === PageState.View ? 24 : 12} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <span class='sm-construction-dispatch-label'>计划恢复时间</span>,
              <span class='sm-construction-dispatch-text'>{this.record && this.record.recoveryTime ? this.record.recoveryTime : '无'}</span>] :
              <a-form-item label="计划恢复时间" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                <a-date-picker
                  style="width: 100%"
                  showTime={true}
                  disabled={this.iPageState == PageState.View}
                  placeholder={this.iPageState == PageState.View ? '' : '请选择'}
                  v-decorator={[
                    'recoveryTime',
                    {
                      initialValue: null,
                    },
                  ]}
                />
              </a-form-item>
            }
          </a-col>

          <a-col sm={24} md={12} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <span class='sm-construction-dispatch-label' >安全防护措施</span>,
              <span class='sm-construction-dispatch-text' >{this.record && this.record.safetyMeasure ? this.record.safetyMeasure : '无'}</span>] :
              <a-form-item label="安全防护措施" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                <a-checkbox-group
                  v-decorator={[
                    'safetyMeasure',
                    {
                      initialValue: [],
                    },
                  ]}>
                  {safetyMeasureOption}
                </a-checkbox-group>
              </a-form-item>
            }
          </a-col>

          <a-col sm={24} md={12} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <span class='sm-construction-dispatch-label' >工序控制类型</span>,
              <span class='sm-construction-dispatch-text' >{this.record && this.record.controlType ? this.record.controlType : '无'}</span>] :
              <a-form-item label="工序控制类型" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                <a-checkbox-group
                  v-decorator={[
                    'controlType',
                    {
                      initialValue: [],
                    },
                  ]}>
                  {controlTypeOption}
                </a-checkbox-group>
              </a-form-item>
            }
          </a-col>

          <a-col sm={24} md={this.iPageState === PageState.View ? 24 : 12} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <span class='sm-construction-dispatch-label' >是否需要大型吊装</span>,
              <span class='sm-construction-dispatch-text' >{this.record && this.record.isNeedLargeEquipment ? '是' : '否'}</span>] :
              <a-form-item label="是否需要大型吊装" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                <a-radio-group
                  v-decorator={[
                    'isNeedLargeEquipment',
                    {
                      initialValue: false,
                    },
                  ]}>
                  <a-radio value={true}>
                    是
                  </a-radio>
                  <a-radio value={false}>
                    否
                  </a-radio>
                </a-radio-group>
              </a-form-item>
            }
          </a-col>
          <a-col sm={24} md={this.iPageState === PageState.View ? 24 : 12} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <span class='sm-construction-dispatch-label'>大型设备吊装机具</span>,
              <span class='sm-construction-dispatch-text' >{this.record && this.record.largeEquipment ? this.record.largeEquipment : '无'}</span>] :
              <a-form-item label="大型设备吊装机具" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                <a-input
                  disabled={this.iPageState == PageState.View}
                  placeholder={this.iPageState == PageState.View ? '' : '请输入'}
                  v-decorator={[
                    'largeEquipment',
                    {
                      initialValue: '',
                      rules: [{ whitespace: true }],
                    },
                  ]}
                />
              </a-form-item>
            }
          </a-col>
          <a-col sm={24} md={12} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <span class='sm-construction-dispatch-label' >是否涉及围蔽拆除</span>,
              <span class='sm-construction-dispatch-text' >{this.record && this.record.isDismantle ? '是' : '否'}</span>] :
              <a-form-item label="是否涉及围蔽拆除" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                <a-radio-group
                  v-decorator={[
                    'isDismantle',
                    {
                      initialValue: false,
                    },
                  ]}>
                  <a-radio value={true}>
                    是
                  </a-radio>
                  <a-radio value={false}>
                    否
                  </a-radio>
                </a-radio-group>
              </a-form-item>
            }
          </a-col>
          <a-col sm={24} md={12} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <span class='sm-construction-dispatch-label' >是否高空作业</span>,
              <span class='sm-construction-dispatch-text' >{this.record && this.record.isHighWork ? '是' : '否'}</span>] :
              <a-form-item label="是否高空作业" label-col={{ span: 5 }} wrapper-col={{ span: 19 }}>
                <a-radio-group
                  v-decorator={[
                    'isHighWork',
                    {
                      initialValue: false,
                    },
                  ]}>
                  <a-radio value={true}>
                    是
                  </a-radio>
                  <a-radio value={false}>
                    否
                  </a-radio>
                </a-radio-group>
              </a-form-item>
            }
          </a-col>
          <a-col sm={24} md={24} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <span class='sm-construction-dispatch-label'>处理方式</span>,
              <span class='sm-construction-dispatch-text' >{this.record && this.record.process ? this.record.process : '无'}</span>] :
              <a-form-item label="处理方式" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                <a-input
                  disabled={this.iPageState == PageState.View}
                  placeholder={this.iPageState == PageState.View ? '' : '请输入'}
                  v-decorator={[
                    'process',
                    {
                      initialValue: '',
                      rules: [{ whitespace: true }],
                    },
                  ]}
                />
              </a-form-item>
            }
          </a-col>



          <a-col sm={24} md={24} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <span class='sm-construction-dispatch-label' >文件资料</span>,
              <span class='sm-construction-dispatch-text'>{this.fileList && this.fileList.length > 0 ? this.fileShow(this.fileList) : '无'}</span>] :
              <a-form-item label="文件资料" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                <SmFileUpload
                  ref="fileUpload"
                  axios={this.axios}
                  multiple
                  placeholder='请上传文件资料'
                  fileList={this.fileList}
                  onSelected={(values) => {
                    this.fileList = values;
                  }}
                />
              </a-form-item>}
          </a-col>
          <a-col sm={24} md={24} class={{ 'sm-construction-dispatch-col': this.iPageState === PageState.View }}>
            {this.iPageState === PageState.View ? [
              <span class='sm-construction-dispatch-label' >其他事宜</span>,
              <span class='sm-construction-dispatch-text'>{this.record && this.record.remark ? this.record.remark : '无'}</span>] :
              <a-form-item label="其他事宜" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
                <a-textarea
                  row={3}
                  disabled={this.iPageState == PageState.View}
                  placeholder={this.iPageState == PageState.View ? '' : '请输入'}
                  v-decorator={[
                    'remark',
                    {
                      initialValue: '',
                      rules: [{ whitespace: true }],
                    },
                  ]}
                />
              </a-form-item>
            }
          </a-col>

          {/* 待安装设备 */}
          {this.record && this.record.workFlowNodes && this.record.workFlowNodes.length > 0 && this.iPageState === PageState.View && !this.isApprove ?
            [
              <a-col sm={24} md={24} class='sm-construction-dispatch-col' >
                <span class='sm-construction-dispatch-text content-style'>审批结果</span>
              </a-col>,
              <a-table
                class="sm-construction-dispatch-table"
                dataSource={this.record.workFlowNodes}
                columns={this.workFlowNodeColumn}
                rowKey={record => record.index}
                bordered={true}
                size="small"
                pagination={false}
              />,
            ]
            : undefined}
        </a-row>
        {this.isApprove && this.iPageState == PageState.View ?
          <a-form-item style="margin-top:20px" label="审批意见" label-col={{ span: 2 }} wrapper-col={{ span: 22 }}>
            <a-textarea
              row={3}
              placeholder={this.isApprove && this.iPageState == PageState.View ? '请输入' : '无'}
              v-decorator={[
                'content',
                {
                  initialValue: '',
                  rules: [{ required: true, message: '请输入审批意见', whitespace: true }],
                },
              ]}
            />
          </a-form-item>
          : undefined}
      </a-form>
      <div style="float: right; margin-top:20px">
        <a-button
          style="margin:0 20px 20px 0"
          onClick={() => this.cancel()}
        >
          取消
        </a-button>
        {this.iPageState == PageState.View ? [
          this.isApprove ? [
            <a-button
              type="danger"
              style="margin:0 20px 20px 0"
              onClick={() => this.process(false)}
              loading={this.loading}
            >
              驳回
            </a-button>,
            <a-button
              type="primary"
              style="margin:0 0 10px 0"
              onClick={() => this.process(true)}
              loading={this.loading}
            >
              通过
            </a-button>,
          ] : [
            // <a-button
            //   type="primary"
            //   style="margin:0 20px 20px 0"
            //   onClick={this.onPrint}
            // >
            //   打印
            // </a-button>,
            vIf(
              <a-button
                type="primary"
                style="margin:0 0 10px 0"
                onClick={this.export}
                loading={this.loading}
              >
                导出
              </a-button>,
              vP(this.permissions, permissionsSmConstruction.Dispatch.Export),
            ),
          ],
        ] :
          <a-button
            type="primary"
            style="margin:0 10px 20px 0"
            onClick={this.save}
            loading={this.loading}
          >
            保存
          </a-button>}
      </div>
      <SmConstructionPlanContentSelectModal
        ref="SmConstructionPlanContentSelectModal"
        axios={this.axios}
        visible={this.planContentModal}
        onClose={() => this.planContentModal = false}
        onChange={value => this.addPlanContents(value)}
      />

      <SmMaterialMaterialSelectModal
        ref="SmMaterialMaterialSelectModal"
        axios={this.axios}
        visible={this.materialModal}
        onChange={(visible, value) => {
          this.materialModal = visible;
          this.addMaterials(value);
        }}
      />

      <SmVideo axios={this.axios} ref="SmVideo" />
      {/* 图片类预览组件 */}
      <SmFileImageView axios={this.axios} ref="SmFileImageView" />
      {/* 文档浏览组件 */}
      <SmFileDocumentView axios={this.axios} ref="SmFileDocumentView" />
    </div>;
  },
};
